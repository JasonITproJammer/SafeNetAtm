using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using SafeNetAtm.Data;
using SafeNetAtm.Domain;

namespace SafeNetAtm.Tests
{
    public class AtmRepositoryTests
    {
        [Fact]
        public void BalanceTest()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<AtmContext>()
                .UseInMemoryDatabase($"CourseDatabaseForTesting{Guid.NewGuid()}")
                .Options;
            using (var context = new AtmContext(options))
            {
                var inventories = new Inventory[]
                {
                    new Inventory{Denomination=100,BillQuantity=10},
                    new Inventory{Denomination=50,BillQuantity=10},
                    new Inventory{Denomination=20,BillQuantity=10},
                    new Inventory{Denomination=10,BillQuantity=10},
                    new Inventory{Denomination=5,BillQuantity=10},
                    new Inventory{Denomination=1,BillQuantity=10}
                };
                foreach (Inventory i in inventories)
                {
                    context.Inventories.Add(i);
                }
                context.SaveChanges();
            }

            using (var context = new AtmContext(options))
            {
                var repo = new Models.AtmRepository(context);

                //Act
                var list = repo.Balance;

                //Assert
                Assert.NotNull(list);
            } 
        }

        [Fact]
        public void DenominationBalanceTest()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<AtmContext>()
                .UseInMemoryDatabase($"CourseDatabaseForTesting{Guid.NewGuid()}")
                .Options;
            using (var context = new AtmContext(options))
            {
                var inventories = new Inventory[]
                {
                    new Inventory{Denomination=100,BillQuantity=10},
                    new Inventory{Denomination=50,BillQuantity=10},
                    new Inventory{Denomination=20,BillQuantity=10},
                    new Inventory{Denomination=10,BillQuantity=10},
                    new Inventory{Denomination=5,BillQuantity=10},
                    new Inventory{Denomination=1,BillQuantity=10}
                };
                foreach (Inventory i in inventories)
                {
                    context.Inventories.Add(i);
                }
                context.SaveChanges();
            }

            using (var context = new AtmContext(options))
            {
                var repo = new Models.AtmRepository(context);

                //Act
                var obj = repo.DenominationBalance(100);

                //Assert
                Assert.Equal(10, obj.BillQuantity);
            }
        }

        [Fact]
        public void WithdrawTest_Success_Single()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<AtmContext>()
                .UseInMemoryDatabase($"CourseDatabaseForTesting{Guid.NewGuid()}")
                .Options;
            using (var context = new AtmContext(options))
            {
                var inventories = new Inventory[]
                {
                    new Inventory{Denomination=100,BillQuantity=10},
                    new Inventory{Denomination=50,BillQuantity=10},
                    new Inventory{Denomination=20,BillQuantity=10},
                    new Inventory{Denomination=10,BillQuantity=10},
                    new Inventory{Denomination=5,BillQuantity=10},
                    new Inventory{Denomination=1,BillQuantity=10}
                };
                foreach (Inventory i in inventories)
                {
                    context.Inventories.Add(i);
                }
                context.SaveChanges();
            }

            using (var context = new AtmContext(options))
            {
                var repo = new Models.AtmRepository(context);

                //Withdraw $208
                repo.Withdraw(208);
                foreach (var item in repo.Balance)
                {
                    switch (item.Denomination)
                    {
                        case 100:
                            Assert.Equal(8, item.BillQuantity);
                            break;
                        case 50:
                            Assert.Equal(10, item.BillQuantity);
                            break;
                        case 20:
                            Assert.Equal(10, item.BillQuantity);
                            break;
                        case 10:
                            Assert.Equal(10, item.BillQuantity);
                            break;
                        case 5:
                            Assert.Equal(9, item.BillQuantity);
                            break;
                        case 1:
                            Assert.Equal(7, item.BillQuantity);
                            break;
                    }
                }
            }
        }

        [Fact]
        public void WithdrawTest_Failure_Single()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<AtmContext>()
                .UseInMemoryDatabase($"CourseDatabaseForTesting{Guid.NewGuid()}")
                .Options;
            using (var context = new AtmContext(options))
            {
                var inventories = new Inventory[]
                {
                    new Inventory{Denomination=100,BillQuantity=10},
                    new Inventory{Denomination=50,BillQuantity=10},
                    new Inventory{Denomination=20,BillQuantity=10},
                    new Inventory{Denomination=10,BillQuantity=10},
                    new Inventory{Denomination=5,BillQuantity=10},
                    new Inventory{Denomination=1,BillQuantity=10}
                };
                foreach (Inventory i in inventories)
                {
                    context.Inventories.Add(i);
                }
                context.SaveChanges();
            }

            using (var context = new AtmContext(options))
            {
                var repo = new Models.AtmRepository(context);

                //Withdraw $2,000,000
                ArgumentException exception = Assert.Throws<ArgumentException>(() => repo.Withdraw(2000000));
                Assert.Equal("Failure: insufficient funds", exception.Message);

                //Withdraw $208
                //This tests that when a failure occurs the changes 
                //  are rolled back to their original amounts.
                repo.Withdraw(208);
                foreach (var item in repo.Balance)
                {
                    switch (item.Denomination)
                    {
                        case 100:
                            Assert.Equal(8, item.BillQuantity);
                            break;
                        case 50:
                            Assert.Equal(10, item.BillQuantity);
                            break;
                        case 20:
                            Assert.Equal(10, item.BillQuantity);
                            break;
                        case 10:
                            Assert.Equal(10, item.BillQuantity);
                            break;
                        case 5:
                            Assert.Equal(9, item.BillQuantity);
                            break;
                        case 1:
                            Assert.Equal(7, item.BillQuantity);
                            break;
                    }
                }
            }
        }

        [Fact]
        public void WithdrawTest_Success_Many()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<AtmContext>()
                .UseInMemoryDatabase($"CourseDatabaseForTesting{Guid.NewGuid()}")
                .Options;
            using (var context = new AtmContext(options))
            {
                var inventories = new Inventory[]
                {
                    new Inventory{Denomination=100,BillQuantity=10},
                    new Inventory{Denomination=50,BillQuantity=10},
                    new Inventory{Denomination=20,BillQuantity=10},
                    new Inventory{Denomination=10,BillQuantity=10},
                    new Inventory{Denomination=5,BillQuantity=10},
                    new Inventory{Denomination=1,BillQuantity=10}
                };
                foreach (Inventory i in inventories)
                {
                    context.Inventories.Add(i);
                }
                context.SaveChanges();
            }

            using (var context = new AtmContext(options))
            {
                var repo = new Models.AtmRepository(context);

                //Withdraw $208
                repo.Withdraw(208);
                foreach(var item in repo.Balance)
                {
                    switch (item.Denomination)
                    {
                        case 100: Assert.Equal(8, item.BillQuantity);
                            break;
                        case 50:
                            Assert.Equal(10, item.BillQuantity);
                            break;
                        case 20:
                            Assert.Equal(10, item.BillQuantity);
                            break;
                        case 10:
                            Assert.Equal(10, item.BillQuantity);
                            break;
                        case 5:
                            Assert.Equal(9, item.BillQuantity);
                            break;
                        case 1:
                            Assert.Equal(7, item.BillQuantity);
                            break;
                    }
                }

                //Withdraw $1,398
                repo.Withdraw(1398);
                foreach (var item in repo.Balance)
                {
                    switch (item.Denomination)
                    {
                        case 100:
                            Assert.Equal(0, item.BillQuantity);
                            break;
                        case 50:
                            Assert.Equal(0, item.BillQuantity);
                            break;
                        case 20:
                            Assert.Equal(6, item.BillQuantity);
                            break;
                        case 10:
                            Assert.Equal(9, item.BillQuantity);
                            break;
                        case 5:
                            Assert.Equal(8, item.BillQuantity);
                            break;
                        case 1:
                            Assert.Equal(4, item.BillQuantity);
                            break;
                    }
                }

                //Withdraw $1,398
                repo.Restock();
                repo.Withdraw(1398);
                foreach (var item in repo.Balance)
                {
                    switch (item.Denomination)
                    {
                        case 100:
                            Assert.Equal(0, item.BillQuantity);
                            break;
                        case 50:
                            Assert.Equal(3, item.BillQuantity);
                            break;
                        case 20:
                            Assert.Equal(8, item.BillQuantity);
                            break;
                        case 10:
                            Assert.Equal(10, item.BillQuantity);
                            break;
                        case 5:
                            Assert.Equal(9, item.BillQuantity);
                            break;
                        case 1:
                            Assert.Equal(7, item.BillQuantity);
                            break;
                    }
                }

                //Withdraw $189
                repo.Restock();
                repo.Withdraw(189);
                foreach (var item in repo.Balance)
                {
                    switch (item.Denomination)
                    {
                        case 100:
                            Assert.Equal(9, item.BillQuantity);
                            break;
                        case 50:
                            Assert.Equal(9, item.BillQuantity);
                            break;
                        case 20:
                            Assert.Equal(9, item.BillQuantity);
                            break;
                        case 10:
                            Assert.Equal(9, item.BillQuantity);
                            break;
                        case 5:
                            Assert.Equal(9, item.BillQuantity);
                            break;
                        case 1:
                            Assert.Equal(6, item.BillQuantity);
                            break;
                    }
                }
            }
        }

        [Fact]
        public void SafeNetTest()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<AtmContext>()
                .UseInMemoryDatabase($"CourseDatabaseForTesting{Guid.NewGuid()}")
                .Options;
            using (var context = new AtmContext(options))
            {
                var inventories = new Inventory[]
                {
                    new Inventory{Denomination=100,BillQuantity=10},
                    new Inventory{Denomination=50,BillQuantity=10},
                    new Inventory{Denomination=20,BillQuantity=10},
                    new Inventory{Denomination=10,BillQuantity=10},
                    new Inventory{Denomination=5,BillQuantity=10},
                    new Inventory{Denomination=1,BillQuantity=10}
                };
                foreach (Inventory i in inventories)
                {
                    context.Inventories.Add(i);
                }
                context.SaveChanges();
            }

            using (var context = new AtmContext(options))
            {
                var repo = new Models.AtmRepository(context);

                //Withdraw $208
                repo.Withdraw(208);
                foreach (var item in repo.Balance)
                {
                    switch (item.Denomination)
                    {
                        case 100:
                            Assert.Equal(8, item.BillQuantity);
                            break;
                        case 50:
                            Assert.Equal(10, item.BillQuantity);
                            break;
                        case 20:
                            Assert.Equal(10, item.BillQuantity);
                            break;
                        case 10:
                            Assert.Equal(10, item.BillQuantity);
                            break;
                        case 5:
                            Assert.Equal(9, item.BillQuantity);
                            break;
                        case 1:
                            Assert.Equal(7, item.BillQuantity);
                            break;
                    }
                }

                //Withdraw $9
                repo.Withdraw(9);
                foreach (var item in repo.Balance)
                {
                    switch (item.Denomination)
                    {
                        case 100:
                            Assert.Equal(8, item.BillQuantity);
                            break;
                        case 50:
                            Assert.Equal(10, item.BillQuantity);
                            break;
                        case 20:
                            Assert.Equal(10, item.BillQuantity);
                            break;
                        case 10:
                            Assert.Equal(10, item.BillQuantity);
                            break;
                        case 5:
                            Assert.Equal(8, item.BillQuantity);
                            break;
                        case 1:
                            Assert.Equal(3, item.BillQuantity);
                            break;
                    }
                }

                //Withdraw $9
                ArgumentException exception = Assert.Throws<ArgumentException>(() => repo.Withdraw(9));
                Assert.Equal("Failure: insufficient funds", exception.Message);

                //Get Denomination for $20
                var obj = repo.DenominationBalance(20);
                Assert.Equal(10, obj.BillQuantity);

                //Get Denomination for $1
                obj = repo.DenominationBalance(1);
                Assert.Equal(3, obj.BillQuantity);

                //Get Denomination for $100
                obj = repo.DenominationBalance(100);
                Assert.Equal(8, obj.BillQuantity);

                //Restock
                repo.Restock();
                foreach (var item in repo.Balance)
                {
                    switch (item.Denomination)
                    {
                        case 100:
                            Assert.Equal(10, item.BillQuantity);
                            break;
                        case 50:
                            Assert.Equal(10, item.BillQuantity);
                            break;
                        case 20:
                            Assert.Equal(10, item.BillQuantity);
                            break;
                        case 10:
                            Assert.Equal(10, item.BillQuantity);
                            break;
                        case 5:
                            Assert.Equal(10, item.BillQuantity);
                            break;
                        case 1:
                            Assert.Equal(10, item.BillQuantity);
                            break;
                    }
                }
            }
        }
    }
}
