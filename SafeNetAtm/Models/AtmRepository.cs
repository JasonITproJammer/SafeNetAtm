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
            var test = amount;
            var denominationsList = _atmContext.Inventories.Where(x => x.BillQuantity > 0)
                    .Select(x => x.Denomination).ToList();
            foreach (var d in denominationsList)
            {
                test = amount % d;
            }
        }

        public void Restock()
        {
            var items = _atmContext.Inventories.Where(x => x.BillQuantity < 10).ToList();
            items.ForEach(x => x.BillQuantity = 10);
            _atmContext.SaveChanges();
        }
    }
}
