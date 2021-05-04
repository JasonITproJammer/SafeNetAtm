using System.Collections.Generic;
using SimpleAtm.Domain;

namespace SimpleAtm.Models
{
    public interface IInventory
    {
        IEnumerable<Inventory> Balance { get; }
        Inventory DenominationBalance(int denom);
        void Restock();
        void Withdraw(int amount);
    }
}
