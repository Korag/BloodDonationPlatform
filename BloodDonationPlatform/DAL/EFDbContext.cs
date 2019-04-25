using BloodDonationPlatform.Models;

namespace BloodDonationPlatform.DAL
{
    public class EFDbContext : IDbContext
    {
        private DbContextModel _context;

        public EFDbContext()
        {
            _context = new DbContextModel();
        }


    }
}
