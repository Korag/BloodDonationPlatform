using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BloodDonationPlatform.DAL;
using BloodDonationPlatform.Services;
using BloodDonationPlatform.ViewModels;
using CsvHelper;
using CsvHelper.TypeConversion;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonationPlatform.Controllers
{
    public class BloodDonationController : Controller
    {
        private IDbContext _context;
        private CsvService _csvReader;

        public BloodDonationController()
        {
            _context = new EFDbContext();
            _csvReader = new CsvService();
        }

        public IActionResult Index(string path)
        {
            ReadFromCSVCollectionWithErrors records = new ReadFromCSVCollectionWithErrors();

            records = _csvReader.ReadContentOfCsvFile(@"C:\Users\user\Documents\Visual Studio 2017\Projects\BloodDonationPlatform\BloodDonationPlatform\wwwroot\CSV Test File\MOCK_DATA.csv");

            if (records.Errors.Count == 0)
            {
                try
                {
                    _context.AddDataFromNewCSVFile(records.CollectionOfRecords);
                }
                catch (Exception e)
                {
                    // return błędy z zapisu do bazy
                }
            }
            else
            {
                // records.Errors wyrzucone na widok
            }

            return View();
        }
    }
}