using Microsoft.EntityFrameworkCore;

namespace BloodDonationPlatform.Models
{
    public class DbContextModel : DbContext
    {
        public DbSet<Donator> Donators { get; set; }
        public DbSet<Donation> Donations { get; set; }

        public DbContextModel() : base()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source = (localdb)\MSSQLLocalDB; Database = BloodDonationPlatform; Integrated Security = True");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }
    }
}
