using System;
using System.ComponentModel.DataAnnotations;

namespace BloodDonationPlatform.Models
{
    public class Donation
    {
        public Guid DonationId { get; set; }

        [Display(Name = "Date of Donation")]
        public DateTime DateOfDonation { get; set; }
        [Display(Name = "Place of Donation")]
        public string PlaceOfDonation { get; set; }
        [Display(Name = "Quantity Of Blood")]
        public int QuantityOfBlood { get; set; }
        [Display(Name = "File Name")]
        public string OriginFileName { get; set; }

        public Guid DonatorId { get; set; }
    }
}