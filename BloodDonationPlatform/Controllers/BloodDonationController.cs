using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BloodDonationPlatform.DAL;
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

        public BloodDonationController()
        {
            _context = new EFDbContext();
        }

        public static Dictionary<int, string> IndexOfProperty = new Dictionary<int, string>
        {
            { 0, "FirstName" },
            { 1, "LastName" },
            { 2, "PESEL" },
            { 3, "BloodGroup" },
            { 4, "BloodFactor" },
            { 5, "QuantityOfBlood" },
            { 6, "DateOfDonation" },
            { 7, "PlaceOfDonation" }
       };

        public IActionResult Index()
        {
            List<ReadFromCSVViewModel> records = new List<ReadFromCSVViewModel>();

            // odczyt z pliku csv wyrzucic do jakiejs warstwy serwisow
            using (var reader = new StreamReader(@"C:\Users\user\Documents\Visual Studio 2017\Projects\BloodDonationPlatform\BloodDonationPlatform\wwwroot\CSV Test File\MOCK_DATA.csv"))
            {
                try
                {
                    using (var csv = new CsvReader(reader))
                    {
                        //csv.Configuration.PrepareHeaderForMatch = (string header, int index) => header.ToLower();
                        csv.Configuration.HasHeaderRecord = false;
                        csv.Configuration.RegisterClassMap<ReadFromCSVViewModelMapper>();
                        csv.Configuration.Delimiter = ",";
                        csv.Configuration.MissingFieldFound = null;

                        records = csv.GetRecords<ReadFromCSVViewModel>().ToList();

                        ReadFromCSVValidator validator = new ReadFromCSVValidator();

                        int rowNumber = 1;
                        foreach (var record in records)
                        {
                            ValidationResult results = validator.Validate(record);

                            if (!results.IsValid)
                            {
                                foreach (var failure in results.Errors)
                                {
                                    Console.WriteLine("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
                                    Console.WriteLine("Wrong value of input '" + failure.AttemptedValue + "'.");
                                    Console.WriteLine("Row number in file " + rowNumber);
                                    // break; return błędy w pliku takie i takie
                                }
                            }
                            rowNumber++;
                        }

                    }
                }

                catch (TypeConverterException ex)
                {
                    Console.WriteLine("Property " + IndexOfProperty.GetValueOrDefault(ex.ReadingContext.CurrentIndex) + " failed validation. Error was: " + ex.Message);
                    Console.WriteLine("Wrong value of input '" + ex.Text + "'.");
                    Console.WriteLine("Row number in file " + ex.ReadingContext.Row + 1);
                    // return błędy jak np jest złe rzutowanie na datę
                    throw;
                }
            }
            try
            {
                _context.AddDataFromNewCSVFile(records);
            }
            catch (Exception e)
            {
                // return błędy z zapisu do bazy
            }

            return View();
        }
    }
}