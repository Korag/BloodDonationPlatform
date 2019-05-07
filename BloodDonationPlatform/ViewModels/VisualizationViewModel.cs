using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodDonationPlatform.ViewModels
{
    public class VisualizationViewModel
    {
        public IFormFile CsvFile { get; set; }
        public string NameOfFile { get; set; }
        public ICollection<string> Errors { get; set; }

        public ICollection<DonatorViewModel> DonatorsWithDonations { get; set; }


        public ICollection<string> SelectedFiles { get; set; }

        public IList<SelectListItem> AvailableFiles { get; set; }

        
        public string TotalAmountOfDonatedBlood { get; set; }

        public string Top20DonatorsSum { get; set; }

        public string TotalAmountOfDonatedBloodInSingleFile { get; set; }

        public string AverageAmountOfBloodBySingleFile { get; set; }

        public string AvgQuantityOfDonatedBloodWithQuantityOfDonationsPerPerson { get; set; }

        public string AvgQuantityOfDonatedBlood { get; set; }

        public string BloodGroupPercentage { get; set; }

        public string BloodFactorPercentage { get; set; }
    }


    public class VisualizationViewModelValidator : AbstractValidator<VisualizationViewModel>
    {
        public VisualizationViewModelValidator()
        {
            RuleFor(x => x.CsvFile).NotNull().WithName("File");
            RuleFor(x => x.NameOfFile).NotNull().WithName("File name");
        }
    }
}
