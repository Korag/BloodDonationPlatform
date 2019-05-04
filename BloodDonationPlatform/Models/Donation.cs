using System;

namespace BloodDonationPlatform.Models
{
    public class Donation
    {
        public Guid DonationId { get; set; }
        public DateTime DateOfDonation { get; set; }
        public string PlaceOfDonation { get; set; }
        public int QuantityOfBlood { get; set; }

        public string OriginFileName { get; set; }

        public Guid DonatorId { get; set; }
    }
}