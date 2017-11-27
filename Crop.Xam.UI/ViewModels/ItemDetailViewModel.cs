using System;

namespace Crop.Xam.UI
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Crop.Models.BuyerProduct Item { get; set; }
        public ItemDetailViewModel(Crop.Models.BuyerProduct item = null)
        {
            Title = item?.Buyer.Name;
            Item = item;
        }
    }
}
