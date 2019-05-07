using CsvHelper.Configuration;
using System;
using FluentValidation;
using System.Collections.Generic;

namespace BloodDonationPlatform.ViewModels
{
    public class ReadFromCSVViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PESEL { get; set; }
        public string BloodGroup { get; set; }
        public string BloodFactor { get; set; }
        public int QuantityOfBlood { get; set; }
        public DateTime DateOfDonation { get; set; }
        public string PlaceOfDonation { get; set; }
    }


    public sealed class ReadFromCSVViewModelMapper : ClassMap<ReadFromCSVViewModel>
    {
        public ReadFromCSVViewModelMapper()
        {
            Map(m => m.FirstName).Name("firstname", "first name", "imie", "imię");
            Map(m => m.LastName).Name("lastname", "last name", "nazwisko");
            Map(m => m.PESEL).Name("pesel"); 
            Map(m => m.BloodGroup).Name("bloodgroup", "blood group", "grupakrwi", "grupa krwi");
            Map(m => m.BloodFactor).Name("bloodfactor", "blood factor", "czynnikkrwi", "czynnik krwi");
            Map(m => m.QuantityOfBlood).Name("quantityofblood", "quantity ofblood", "quantityof blood", "quantity of blood", "ilosc", "ilość", "ilosc krwi", "ilosckrwi", "ilość krwi", "ilośćkrwi");
            Map(m => m.DateOfDonation).Name("date", "dateofdonation", "date ofdonation", "dateof donation", "date of donation", "data", "dataoddaniakrwi", "data oddaniakrwi", "dataoddania krwi", "data oddania krwi"); 
            Map(m => m.PlaceOfDonation).Name("place", "placeofdonation", "place ofdonation", "placeof donation", "place of donation", "miejsce", "miejsceoddaniakrwi", "miejsce oddaniakrwi", "miejsceoddania krwi", "miejsce oddania krwi"); 
        }
    }

    public class ReadFromCSVValidator : AbstractValidator<ReadFromCSVViewModel>
    {
        public ReadFromCSVValidator()
        {
            RuleFor(x => x.FirstName).NotNull().Matches(@"^[a-zA-Z]{1,40}").WithMessage("The FirstName field should contain only letters in the number from 1 to 40");
            RuleFor(x => x.LastName).NotNull().Matches(@"^[a-zA-Z]{1,40}").WithMessage("The LastName field should contain only letters in the number from 1 to 40");
            RuleFor(x => x.PESEL).NotNull().Matches(@"^\d{11}$").WithMessage("PESEL field format incorrect - the field should contain only 11 digits");
            
            List<string> BloodGroupConditions = new List<string> { "0", "A", "B", "AB" };
              
            RuleFor(x => x.BloodGroup).NotNull().Must(x => BloodGroupConditions.Contains(x))
                    .WithMessage("Permissible values: " + String.Join(",", BloodGroupConditions));

            List<string> BloodFactorConditions = new List<string> { "RH+", "RH-" };

            RuleFor(x => x.BloodFactor).NotNull().Must(x => BloodFactorConditions.Contains(x))
                    .WithMessage("Permissible values: " + String.Join(",", BloodFactorConditions)); 

            RuleFor(x => x.QuantityOfBlood).NotNull().ExclusiveBetween(0, 2500).WithMessage("QuantityOfBlood field format incorrect - the field should contain the number from interval 0-2500"); ;
            RuleFor(x => x.DateOfDonation).NotNull();
            RuleFor(x => x.PlaceOfDonation).NotNull().Length(0, 100); 
        }
    }
}
