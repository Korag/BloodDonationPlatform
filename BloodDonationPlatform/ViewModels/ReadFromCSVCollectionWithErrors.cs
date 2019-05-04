using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodDonationPlatform.ViewModels
{
    public class ReadFromCSVCollectionWithErrors
    {
        public ICollection<ReadFromCSVViewModel> CollectionOfRecords { get; set; }
        public ICollection<string> Errors { get; set; }
    }
}
