using System;
using System.Collections.Generic;
using Crop.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Crop.Xam.UI
{
    public class USDA_WY : Crop.Xam.UI.IDataStore<BuyerProduct>
    {
        private const string WINTER_WHEAT = "Hard Red Winter Wheat";
        private const string SPRING_WHEAT = "Dark Northern Spring Wheat";
        private const string DURUM_WHEAT = "Durum Wheat";
        private const string END_CHECK = "nb - no bid";

        List<Models.BuyerProduct> items;
        public USDA_WY()
        {
            this.items = new List<Models.BuyerProduct>();
            DateTime asOf = DateTime.Now;
            Products currentProduct = Products.None;
            var scraper = new Crop.Xam.Utility.WebScraper("https://www.ams.usda.gov/mnreports/bl_gr110.txt");
            var input = scraper.Content;
            var lines = input.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                if (i == 1)
                {
                    asOf = DateTime.Parse(lines[i].Substring(22, 16));
                    continue;
                }

                if (lines[i].Contains(WINTER_WHEAT))
                {
                    currentProduct = Products.WinterWheat;
                    continue;
                }

                if (lines[i].Contains(SPRING_WHEAT))
                {
                    currentProduct = Products.SpringWheat;
                    continue;
                }

                if (currentProduct != Products.None && lines[i].Contains(DURUM_WHEAT))
                {
                    currentProduct = Products.DurumWheat;
                    continue;
                }

                if (lines[i].Contains(END_CHECK))
                {
                    currentProduct = Products.None;
                    continue;
                }

                if (currentProduct != Products.None)
                {
                    ParseLine(currentProduct, lines[i], asOf);
                }
            }

        }

        private void ParseLine( Products currentProduct, string line, DateTime asOf)
        {
            if (line.Length > 0)
            {

                var expression = new System.Text.RegularExpressions.Regex(@"\s\s+");
                var parts = expression.Split(line.TrimEnd());
                if (parts[0].Length > 0 && parts.Length > 1)
                {
                    var b = new Models.Buyer();
                    b.Name = parts[0];

                    switch (currentProduct)
                    {
                        case Products.WinterWheat:
                            ParseColumn(b, parts[1], WINTER_WHEAT, "Ordinary", asOf);
                            ParseColumn(b, parts[2], WINTER_WHEAT, "11 pct", asOf);
                            ParseColumn(b, parts[3], WINTER_WHEAT, "12 pct", asOf);
                            ParseColumn(b, parts[4], WINTER_WHEAT, "13 pct", asOf);
                            break;
                        case Products.SpringWheat:
                            ParseColumn(b, parts[1], SPRING_WHEAT, "13 pct", asOf);
                            ParseColumn(b, parts[2], SPRING_WHEAT, "14 pct", asOf);
                            ParseColumn(b, parts[3], SPRING_WHEAT, "15 pct", asOf);
                            break;
                        case Products.DurumWheat:
                            ParseColumn(b, parts[1], DURUM_WHEAT, "13 pct", asOf);
                            ParseColumn(b, parts[2], "Barley", "Malt", asOf);
                            ParseColumn(b, parts[3], "Barley", "Feed", asOf);
                            break;
                    }
                }
            }
        }

        private void ParseColumn( Models.Buyer buyer, string priceRange, string productName, string quality, DateTime asOf)
        {
            if (priceRange.TrimEnd() != "--" && priceRange.TrimEnd() != "nb" && priceRange.TrimEnd() != "na")
            {
                var p = new Models.Product();
                p.Name = productName;
                p.Quality = quality;

                var price = new Models.Price();
                price.AsOf = asOf;

                if (priceRange.Contains("-"))
                {
                    var prices = priceRange.Split('-');
                    price.MinPrice = decimal.Parse(prices[0]);
                    price.MaxPrice = decimal.Parse(prices[1]);
                }
                else
                {
                    price.MinPrice = decimal.Parse(priceRange);
                    price.MaxPrice = decimal.Parse(priceRange);
                }
                var bp = new Models.BuyerProduct();
                bp.Id = Guid.NewGuid().ToString();
                bp.Buyer = buyer;
                bp.Product = p;
                bp.Price = price;
                this.items.Add(bp);
            }
        }


        public async Task<bool> AddItemAsync(BuyerProduct item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(BuyerProduct item)
        {
            var _item = items.Where((BuyerProduct arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var _item = items.Where((BuyerProduct arg) => arg.Id == id).FirstOrDefault();
            items.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<BuyerProduct> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<BuyerProduct>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }

    public enum Products
    {
        None,
        WinterWheat,
        SpringWheat,
        DurumWheat
    }
}
