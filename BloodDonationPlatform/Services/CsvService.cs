using BloodDonationPlatform.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.TypeConversion;
using FluentValidation.Results;
using System.IO;

namespace BloodDonationPlatform.Services
{
    public class CsvService
    {
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

        public ReadFromCSVCollectionWithErrors ReadContentOfCsvFile(string path)
        {
            ReadFromCSVCollectionWithErrors records = new ReadFromCSVCollectionWithErrors
            {
                CollectionOfRecords = new List<ReadFromCSVViewModel>(),
                Errors = new List<string>()
            };
    
            using (var reader = new StreamReader(path))
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

                        records.CollectionOfRecords = csv.GetRecords<ReadFromCSVViewModel>().ToList();

                        ReadFromCSVValidator validator = new ReadFromCSVValidator();

                        int rowNumber = 1;
                        foreach (var record in records.CollectionOfRecords)
                        {
                            ValidationResult results = validator.Validate(record);

                            if (!results.IsValid)
                            {
                                foreach (var failure in results.Errors)
                                {
                                    records.Errors.Add("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
                                    records.Errors.Add("Wrong value of input '" + failure.AttemptedValue + "'.");
                                    records.Errors.Add("Row number in file " + rowNumber);
                                }
                            }
                            rowNumber++;
                        }

                    }
                }

                catch (TypeConverterException ex)
                {
                    records.Errors.Add("Property " + IndexOfProperty.GetValueOrDefault(ex.ReadingContext.CurrentIndex) + " failed validation. Error was: " + ex.Message);
                    records.Errors.Add("Wrong value of input '" + ex.Text + "'.");
                    records.Errors.Add("Row number in file " + ex.ReadingContext.Row + 1);
                }
            }

            return records;
        }

    }
}

