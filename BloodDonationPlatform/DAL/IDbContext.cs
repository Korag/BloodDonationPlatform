using BloodDonationPlatform.ViewModels;
using System.Collections.Generic;

namespace BloodDonationPlatform.DAL
{
    public interface IDbContext
    {
        void AddDataFromNewCSVFile(ICollection<ReadFromCSVViewModel> records);
    }
}
