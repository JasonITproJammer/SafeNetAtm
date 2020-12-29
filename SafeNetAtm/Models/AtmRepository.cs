using SafeNetAtm.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SafeNetAtm.Domain;

namespace SafeNetAtm.Models
{
    public class AtmRepository : IInventory
    {
        private readonly AtmContext _atmContext;

        public AtmRepository(AtmContext atmContext)
        {
            _atmContext = atmContext;
        }

        public IEnumerable<Inventory> Balance
        {
            get
            {
                return _atmContext.Inventories.ToList();
            }
        }

        public Inventory DenominationBalance(int denom)
        {
            return _atmContext.Inventories.SingleOrDefault(x => x.Denomination == denom);
        }

        public void Withdraw(int amount)
        {
            try
            {
                foreach (var item in _atmContext.Inventories
                        .Where(x => x.BillQuantity > 0)
                        .OrderByDescending(x => x.Denomination))
                {
                    while ((amount - item.Denomination) > -1 && item.BillQuantity > 0)
                    {
                        item.BillQuantity -= 1;
                        amount -= item.Denomination;
                    }
                }

                if (amount > 0)
                {
                    RollBack();
                    throw new ArgumentException("Failure: insufficient funds");
                }
                else
                {
                    _atmContext.SaveChanges();
                }
            }
            catch (Exception)
            {
                RollBack();
                throw;
            }
        }

        public void Restock()
        {
            try
            {
                _atmContext.Inventories.ToList().Where(x => x.BillQuantity < 10)
                .ToList().ForEach(x => x.BillQuantity = 10);
                _atmContext.SaveChanges();
            }
            catch (Exception)
            {
                RollBack();
                throw;
            }
        }

        private void RollBack()
        {
            var changedEntries = _atmContext.ChangeTracker.Entries()
                .Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }
    }
}
