using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodDonationPlatform.ViewModels
{
    public class ReadFromCSVViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PESEL { get; set; }
        public string BloodGroup { get; set; }
        public string BloodFactor { get; set; }
        public DateTime DateOfDonation { get; set; }
        public string PlaceOfDonation { get; set; }
    }
}
