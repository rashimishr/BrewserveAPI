using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brewserve.Core.Payloads
{
    public class BarBeerLinkRequest
    {
        public int BarId { get; set; }
        public int BeerId { get; set; }
    }
}
