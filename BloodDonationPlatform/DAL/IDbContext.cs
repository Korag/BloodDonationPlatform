using BloodDonationPlatform.Models;
using BloodDonationPlatform.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BloodDonationPlatform.DAL
{
    public interface IDbContext
    {
        void AddDataFromNewCSVFile(ICollection<ReadFromCSVViewModel> records, string NameOfFile);
        bool CheckIfNameOfFileIsUnique(string NameOfFile);

        void AddNewDonation(Donation donation);
        void AddNewDonator(Donator donator);
        bool IfDonatorExists(Donator donator);

        ICollection<SelectListItem> GetFileNamesAsSelectList();
        ICollection<string> GetFileNames();
    }
}
