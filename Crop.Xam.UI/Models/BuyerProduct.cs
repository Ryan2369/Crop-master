using System;
using System.Collections.Generic;
using System.Text;

namespace Crop.Models
{
    public class BuyerProduct
    {
        public string Id { get; set; }
        public Buyer Buyer { get; set; }
        public Product Product { get; set; }
        public Price Price { get; set; }

        public BuyerProduct()
        {
           
        }
    }
}
