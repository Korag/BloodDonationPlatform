using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BloodDonationPlatform.DAL;
using BloodDonationPlatform.ViewModels;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonationPlatform.Controllers
{
    public class BloodDonationController : Controller
    {
        private IDbContext _context;

        public BloodDonationController()
        {
            _context = new EFDbContext();
        }

        public IActionResult Index()
        {
            using (var reader = new StreamReader(@"C:\Users\Łukasz\Desktop\MOCK_DATA.csv"))
            {
                using (var csv = new CsvReader(reader))
                {
                    var records = csv.GetRecords<ReadFromCSVViewModel>();
                }
            }

            return View();
        }
    }
}