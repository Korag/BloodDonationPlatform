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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace BloodDonationPlatform.Controllers
{
    public class BloodDonationController : Controller
    {
        private IHostingEnvironment _environment;

        private IDbContext _context;
        private CsvService _csvReader;

        public BloodDonationController(IHostingEnvironment environment)
        {
            _environment = environment;

            _context = new EFDbContext();
            _csvReader = new CsvService();
        }

        public IActionResult Index(VisualizationViewModel visualizationModel)
        {
            visualizationModel.AvailableFiles = new List<SelectListItem>();
            visualizationModel.AvailableFiles = _context.GetFileNamesAsSelectList().ToList();

            if (visualizationModel.SelectedFiles == null)
            {
                visualizationModel.SelectedFiles = visualizationModel.AvailableFiles.Select(z=> z.Text).ToList();
            }

            visualizationModel.DonatorsWithDonations = _context.GetDonatorsWithDonations(visualizationModel.SelectedFiles);

            if (visualizationModel.DonatorsWithDonations.Count != 0)
            {
                // łączna ilość zebranej krwi przez wszystkich donatorów 
                var amountOfBlood = visualizationModel.DonatorsWithDonations.Sum(z => z.Donations.Sum(s => s.QuantityOfBlood));
                List<NameValueViewModel> TotalAmountOfDonatedBlood = new List<NameValueViewModel> { new NameValueViewModel { name = "Total amount of Donated Blood \n (l)", steps = amountOfBlood / 1000 } };


                // łączna zebrana ilość krwi w każdym z plików
                var TotalAmountOfBloodBySingleFile = new List<NameValueViewModel>();

                foreach (var file in visualizationModel.SelectedFiles)
                {
                    var PartialSum = visualizationModel.DonatorsWithDonations.Select(z => z.Donations.Where(d => d.OriginFileName == file).Sum(c => c.QuantityOfBlood)).Sum();

                    NameValueViewModel AmountOfBloodInSingleFile = new NameValueViewModel { name = file, steps = PartialSum };
                    TotalAmountOfBloodBySingleFile.Add(AmountOfBloodInSingleFile);
                }

                // średnia oddawana ilość krwi w każdym z plików na 1 donację
                var AverageAmountOfBloodBySingleFile = new List<NameValueViewModel>();

                foreach (var file in visualizationModel.SelectedFiles)
                {
                    var AmountOfDonations = visualizationModel.DonatorsWithDonations.Select(z => z.Donations.Where(d => d.OriginFileName == file)).Count();

                    NameValueViewModel AverageAmountOfBloodInSingleFile = new NameValueViewModel { name = file, steps = (TotalAmountOfBloodBySingleFile.Where(z => z.name == file).Select(z => z.steps).FirstOrDefault() / AmountOfDonations) };
                    AverageAmountOfBloodBySingleFile.Add(AverageAmountOfBloodInSingleFile);
                }

                //// liczba donacji na osobę
                //var QuantityOfDonationsPerPerson = visualizationModel.DonatorsWithDonations.Select(z => new { Person = z.FirstName + " " + z.LastName, Value = z.Donations.Count }).ToList();

                //// średnia ilość oddanej krwi każdej osoby 
                //var AvgQuantityOfDonatedBloodPerPerson = visualizationModel.DonatorsWithDonations.Select(z => new NameValueViewModel { name = z.FirstName + " " + z.LastName, steps = z.Donations.Sum(c => c.QuantityOfBlood)/z.Donations.Select(s=> s.DonatorId).Count() }).ToList();

                // liczba donacji na osobę połączona ze średnią ilością oddanej krwi danej osoby
                var AvgQuantityOfDonatedBloodWithQuantityOfDonationsPerPerson = visualizationModel.DonatorsWithDonations.Select(z => new NameValueValueViewModel { name = z.FirstName + " " + z.LastName, steps = z.Donations.Sum(c => c.QuantityOfBlood) / z.Donations.Select(s => s.DonatorId).Count(), steps2 = z.Donations.Count }).ToList();

                // średnia ilość oddanej krwi
                var avgQuantityOfDonatedBlood = visualizationModel.DonatorsWithDonations.Select(z => new { Person = z.FirstName + " " + z.LastName, z.Donations.Count, Value = z.Donations.Sum(c => c.QuantityOfBlood) }).Select(z => z.Value / z.Count).Average();
                List<NameValueViewModel> AvgQuantityOfDonatedBlood = new List<NameValueViewModel> { new NameValueViewModel { name = "Average Quantity Of \n Donated Blood \n (ml)", steps = (int)(avgQuantityOfDonatedBlood) } };

                // 20 osób które oddały najwięcej krwi
                var Top20Donators = visualizationModel.DonatorsWithDonations.Select(z => new NameValueViewModel { name = z.FirstName[0] + ". " + z.LastName, steps = z.Donations.Sum(s => s.QuantityOfBlood) }).OrderByDescending(z => z.steps).Take(20).ToList();

                // procent ludzi z daną grupą krwi 
                var BloodGroupPercentage = visualizationModel.DonatorsWithDonations.GroupBy(z => z.BloodGroup).Select(z => new NameValueViewModel { name = z.Key, steps = z.Count() }).ToList();

                // procent ludzi z danym czynnikiem krwi
                var BloodFactorPercentage = visualizationModel.DonatorsWithDonations.GroupBy(z => z.BloodFactor).Select(z => new NameValueViewModel { name = z.Key, steps = z.Count() }).ToList();

                visualizationModel.TotalAmountOfDonatedBlood = JsonConvert.SerializeObject(TotalAmountOfDonatedBlood);
                visualizationModel.Top20DonatorsSum = JsonConvert.SerializeObject(Top20Donators);
                visualizationModel.TotalAmountOfDonatedBloodInSingleFile = JsonConvert.SerializeObject(TotalAmountOfBloodBySingleFile);

                visualizationModel.AverageAmountOfBloodBySingleFile = JsonConvert.SerializeObject(AverageAmountOfBloodBySingleFile);
                visualizationModel.AvgQuantityOfDonatedBloodWithQuantityOfDonationsPerPerson = JsonConvert.SerializeObject(AvgQuantityOfDonatedBloodWithQuantityOfDonationsPerPerson);
                visualizationModel.AvgQuantityOfDonatedBlood = JsonConvert.SerializeObject(AvgQuantityOfDonatedBlood);

                visualizationModel.BloodGroupPercentage = JsonConvert.SerializeObject(BloodGroupPercentage);
                visualizationModel.BloodFactorPercentage = JsonConvert.SerializeObject(BloodFactorPercentage);
            }
            
            // PANEL SZCZEGÓŁÓW POJEDYNCZEGO DONATORA -> COMPONENT

            return View(visualizationModel);
        }

        [HttpPost]
        public  IActionResult SelectFiles(VisualizationViewModel visualizationModel)
        {
            return RedirectToAction("Index", new { SelectedFiles = visualizationModel.SelectedFiles });
        }

        [HttpPost]
        public IActionResult ReadCSVFile(VisualizationViewModel visualizationModel)
        {

            if (ModelState.IsValid)
            {
                if (!_context.CheckIfNameOfFileIsUnique(visualizationModel.NameOfFile))
                {
                    return RedirectToAction("Index");
                }

                var folderName = "Upload";
                ReadFromCSVCollectionWithErrors records = new ReadFromCSVCollectionWithErrors();

                var savePath = Path.Combine(_environment.WebRootPath, folderName, visualizationModel.CsvFile.FileName);

                using (var fileStream = new FileStream(savePath, FileMode.Create))
                {
                    visualizationModel.CsvFile.CopyTo(fileStream);
                }

                records = _csvReader.ReadContentOfCsvFile(savePath);

                if (records.Errors.Count == 0)
                {
                    try
                    {
                        _context.AddDataFromNewCSVFile(records.CollectionOfRecords, visualizationModel.NameOfFile);
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


                return RedirectToAction("Index");

            }
            else
            {
                return RedirectToAction("Index", new {visualizationModel = visualizationModel });
            }
        }
    }
}