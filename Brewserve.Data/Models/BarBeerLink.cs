using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brewserve.Data.Models
{
    public class BarBeerLink
    {
        public int BarId { get; set; }
        public Bar Bar { get; set; }
        public int BeerId { get; set; }
        public Beer Beer { get; set; }
    }
}
