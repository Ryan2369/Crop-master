using System;

using Xamarin.Forms;

namespace Crop.Xam.UI
{
    public partial class NewItemPage : ContentPage
    {
        public Crop.Models.BuyerProduct Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();

            Item = new Models.BuyerProduct()
            {
                Buyer = new Models.Buyer() {Name = "Buyer name" },
                Product = new Models.Product() { Name = "This is a product description." }
            };

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopToRootAsync();
        }
    }
}
