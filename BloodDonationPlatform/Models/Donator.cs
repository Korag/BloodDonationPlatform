using System;

namespace BloodDonationPlatform.Models
{
    public class Donator
    {
        public Guid DonatorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BloodGroup { get; set; }
        public string BloodFactor { get; set; }
    }
}