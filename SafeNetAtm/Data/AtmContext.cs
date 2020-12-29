using Microsoft.EntityFrameworkCore;
using SafeNetAtm.Domain;

namespace SafeNetAtm.Data
{
    public class AtmContext : DbContext
    {
        //private DbContextOptions<AtmContext> options;
        public AtmContext() 
        {
            //options = new DbContextOptionsBuilder<AtmContext>()
            //    .UseSqlServer("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = ATM")
            //    .Options;
        }

        public AtmContext(DbContextOptions<AtmContext> options) : base(options)
        {
        }

        public DbSet<Inventory> Inventories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inventory>().ToTable("Inventory");
        }
    }
}
