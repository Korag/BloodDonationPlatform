using BloodDonationPlatform.DAL;
using BloodDonationPlatform.Models;
using BloodDonationPlatform.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BloodDonationPlatform.Components
{
    public class DonatorsTableViewComponent : ViewComponent
    {
        private IDbContext _context; 

        public DonatorsTableViewComponent()
        {
            _context = new EFDbContext();
        }

        public IViewComponentResult Invoke(ICollection<DonatorViewModel> donatorsWithDonations)
        {
            return View(donatorsWithDonations);
        }
    }
}
