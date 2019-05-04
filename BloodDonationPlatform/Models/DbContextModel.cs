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
            modelBuilder.Entity<Donator>() .Property(z => z.FirstName).IsRequired();
            modelBuilder.Entity<Donator>().Property(z => z.LastName).IsRequired();
            modelBuilder.Entity<Donator>().Property(z => z.BloodGroup).IsRequired();
            modelBuilder.Entity<Donator>().Property(z => z.BloodFactor).IsRequired();

            modelBuilder.Entity<Donation>().Property(z => z.DonatorId).IsRequired();
            modelBuilder.Entity<Donation>().Property(z => z.DateOfDonation).IsRequired();
            modelBuilder.Entity<Donation>().Property(z => z.PlaceOfDonation).IsRequired();
            modelBuilder.Entity<Donation>().Property(z => z.QuantityOfBlood).IsRequired();

            modelBuilder.Entity<Donation>().Property(z => z.OriginFileName).IsRequired();
        }
    }
}
