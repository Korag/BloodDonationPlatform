using BloodDonationPlatform.Models;
using BloodDonationPlatform.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BloodDonationPlatform.DAL
{
    public class EFDbContext : IDbContext
    {
        private DbContextModel _context;

        public EFDbContext()
        {
            _context = new DbContextModel();
            _context.Database.EnsureCreated();
        }

        public void AddDataFromNewCSVFile(ICollection<ReadFromCSVViewModel> records)
        {
            foreach (var model in records)
            {
                Donator donator = new Donator
                {
                    DonatorId = Guid.NewGuid(),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    BloodGroup = model.BloodGroup,
                    BloodFactor = model.BloodFactor
                };

                Donation donation = new Donation
                {
                    DonationId = Guid.NewGuid(),
                    DateOfDonation = model.DateOfDonation,
                    PlaceOfDonation = model.PlaceOfDonation,
                    QuantityOfBlood = model.QuantityOfBlood,

                    DonatorId = donator.DonatorId
                };

                if(IfDonatorExists(donator))
                {
                    AddNewDonator(donator);
                }
                AddNewDonation(donation);
            }
        }

        private void AddNewDonation(Donation donation)
        {
            _context.Donations.Add(donation);
            _context.SaveChanges();
        }

        private void AddNewDonator(Donator donator)
        {
            _context.Donators.Add(donator);
            _context.SaveChanges();
        }

        private bool IfDonatorExists(Donator donator)
        {
            var Donator = _context.Donators.Where(z => z.FirstName == donator.FirstName && z.LastName == donator.LastName
                                    && z.BloodGroup == donator.BloodGroup && z.BloodFactor == donator.BloodFactor).FirstOrDefault();

            if (Donator != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
