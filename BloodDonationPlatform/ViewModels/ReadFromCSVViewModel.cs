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
            Map(m => m.FirstName).NameIndex(0);
            Map(m => m.LastName).NameIndex(1); 
            Map(m => m.PESEL).NameIndex(2); 
            Map(m => m.BloodGroup).NameIndex(3); 
            Map(m => m.BloodFactor).NameIndex(4);
            Map(m => m.QuantityOfBlood).NameIndex(5);
            Map(m => m.DateOfDonation).NameIndex(6).ToString(); 
            Map(m => m.PlaceOfDonation).NameIndex(7); 
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
