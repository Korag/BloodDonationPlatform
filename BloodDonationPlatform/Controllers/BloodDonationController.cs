﻿using System;
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
            if (visualizationModel.SelectedFiles == null)
            {
                visualizationModel.SelectedFiles = new List<String>();
            }

            visualizationModel.AvailableFiles = new List<SelectListItem>();
            visualizationModel.AvailableFiles = _context.GetFileNamesAsSelectList().ToList();

            // pobieranie kolekcji do wyświetlenia
            // defaultowo ładowane są wszystkie

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
                return RedirectToAction("Index");
            }
        }
    }
}