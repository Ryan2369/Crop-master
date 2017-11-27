using System;
using System.Collections.Generic;
using System.Text;

namespace Crop.Models
{
    public class Price
    {
        public int PriceId { get; set; }
        public DateTime AsOf { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}
