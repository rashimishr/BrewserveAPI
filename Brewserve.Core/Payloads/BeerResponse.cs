using Brewserve.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brewserve.Core.Payloads
{
    public class BeerResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal PercentageAlcoholByVolume { get; set; }
    }
}
