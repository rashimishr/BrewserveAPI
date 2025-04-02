using Brewserve.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brewserve.Core.Payloads
{
    public class BreweryBeerLinkRequest
    {
        public int BreweryId { get; set; }
        public int BeerId { get; set; }
    }
}
