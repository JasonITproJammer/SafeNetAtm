using System.Collections.Generic;
using SafeNetAtm.Domain;

namespace SafeNetAtm.Models
{
    public class AtmViewModel
    {
        public IEnumerable<Inventory> InventoryList { get; set; }
        public int WithdrawalAmount { get; set; }
        public int[] Denomination { get; set; }
    }
}
