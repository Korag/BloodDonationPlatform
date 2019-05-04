using BloodDonationPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodDonationPlatform.ViewModels
{
    public class DonatorViewModel
    {
        public Guid DonatorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BloodGroup { get; set; }
        public string BloodFactor { get; set; }
        public ICollection<Donation> Donations { get; set; }
    }
}
