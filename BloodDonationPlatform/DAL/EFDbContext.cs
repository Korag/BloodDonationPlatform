using BloodDonationPlatform.Models;
using BloodDonationPlatform.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public void AddDataFromNewCSVFile(ICollection<ReadFromCSVViewModel> records, string NameOfFile)
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

                    OriginFileName = NameOfFile
                };

                if(IfDonatorExists(donator))
                {
                    AddNewDonator(donator);
                    donation.DonatorId = donator.DonatorId;
                }

                Guid id = GetDonatorID(donator);
                donation.DonatorId = id;
                AddNewDonation(donation);
            }
        }

        public bool CheckIfNameOfFileIsUnique(string NameOfFile)
        {
            var Donations = _context.Donations.Where(z => z.OriginFileName == NameOfFile).ToList();

            if (Donations.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Guid GetDonatorID(Donator donator)
        {
            return  _context.Donators.Where(z => z.FirstName == donator.FirstName && z.LastName == donator.LastName
                                      && z.BloodGroup == donator.BloodGroup && z.BloodFactor == donator.BloodFactor).Select(z=> z.DonatorId).FirstOrDefault();
        }

        public void AddNewDonation(Donation donation)
        {
            _context.Donations.Add(donation);
            _context.SaveChanges();
        }

        public void AddNewDonator(Donator donator)
        {
            _context.Donators.Add(donator);
            _context.SaveChanges();
        }

        public bool IfDonatorExists(Donator donator)
        {
            var Donator = _context.Donators.Where(z => z.FirstName == donator.FirstName && z.LastName == donator.LastName
                                    && z.BloodGroup == donator.BloodGroup && z.BloodFactor == donator.BloodFactor).FirstOrDefault();

            if (Donator != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public ICollection<SelectListItem> GetFileNamesAsSelectList()
        {
            var FilesNames = GetFileNames();
            List<SelectListItem> SelectList = new List<SelectListItem>();

            foreach (var fileName in FilesNames)
            {
                SelectList.Add
                    (
                        new SelectListItem()
                        {
                            Text = fileName,
                            Value = fileName
                        }
                    );
            };

            return SelectList;
        }

        public ICollection<string> GetFileNames()
        {
            var FileNames = _context.Donations.Select(z => z.OriginFileName).Distinct().ToList();
            return FileNames;
        }

        public ICollection<DonatorViewModel> GetDonatorsWithDonations(ICollection<string> FileNames)
        {
            List<DonatorViewModel> DonatorsWithDonations = new List<DonatorViewModel>();

            foreach (var donator in _context.Donators)
            {
                DonatorViewModel _donator = new DonatorViewModel()
                {
                    DonatorId = donator.DonatorId,
                    FirstName = donator.FirstName,
                    LastName = donator.LastName,
                    BloodGroup = donator.BloodGroup,
                    BloodFactor = donator.BloodFactor,

                    Donations = new List<Donation>()
                };

                if (FileNames.Count == 0)
                {
                    _donator.Donations = _context.Donations.Where(z => z.DonatorId == _donator.DonatorId).ToList();
                }
                else
                {
                    _donator.Donations = _context.Donations.Where(z => z.DonatorId == _donator.DonatorId && FileNames.Contains(z.OriginFileName)).ToList();
                }

                DonatorsWithDonations.Add(_donator);
            }

            return DonatorsWithDonations;
        }

    }
}
