using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SafeNetAtm.Data;
using SafeNetAtm.Domain;

namespace SafeNetAtm.Models
{
    public class AtmViewModel
    {
        public IEnumerable<Inventory> InventoryList { get; set; }
        public int WithdrawalAmount { get; set; }
        public int Denomination { get; set; }
    }
}
