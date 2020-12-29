using System;
using Xunit;
using SafeNetAtm.Models;
using Microsoft.EntityFrameworkCore;
using SafeNetAtm.Data;
using SafeNetAtm.Domain;
using System.Collections.Generic;

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
        public void WithdrawTest_Success()
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
                IEnumerable<Inventory> list = repo.Balance;
                foreach(var item in list)
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
                        default:
                            Assert.Throws<ArgumentException>("test", () => repo.Withdraw(208));
                            break;
                    }
                }

                //Withdraw $9
                repo.Restock();
                repo.Withdraw(9);
                list = repo.Balance;
                foreach (var item in list)
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
                            Assert.Equal(9, item.BillQuantity);
                            break;
                        case 1:
                            Assert.Equal(6, item.BillQuantity);
                            break;
                        default:
                            Assert.Throws<ArgumentException>("test", () => repo.Withdraw(9));
                            break;
                    }
                }

                //Withdraw $189
                repo.Restock();
                repo.Withdraw(189);
                list = repo.Balance;
                foreach (var item in list)
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
                        default:
                            Assert.Throws<ArgumentException>("test", () => repo.Withdraw(189));
                            break;
                    }
                }
            }
        }
    }
}
