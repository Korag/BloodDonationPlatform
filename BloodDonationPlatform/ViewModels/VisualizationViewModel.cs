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

        public ICollection<DonatorViewModel> DonatorsWithDonations { get; set; }

        //[Required(ErrorMessage = "Należy zaznaczyć conajmniej jeden Obszar.")]
        public ICollection<string> SelectedFiles { get; set; }

        //[Display(Name = "Obszar")]
        public IList<SelectListItem> AvailableFiles { get; set; }

    }


    public class VisualizationViewModelValidator : AbstractValidator<VisualizationViewModel>
    {
        public VisualizationViewModelValidator()
        {
            RuleFor(x => x.CsvFile).NotNull();
        }
    }
}
