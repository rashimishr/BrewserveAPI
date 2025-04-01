using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brewserve.Data.Models
{
    public class BreweryBeerLink
    {
        public int BreweryId { get; set; }
        public Brewery Brewery { get; set; }
        public int BeerId { get; set; }
        public Beer Beer { get; set; }
    }
}
