using System.Collections.Generic;
using SafeNetAtm.Domain;

namespace SafeNetAtm.Models
{
    public interface IInventory
    {
        IEnumerable<Inventory> Balance { get; }
        Inventory DenominationBalance(int denom);
        void Restock();
        void Withdraw(int amount);
    }
}
