using LogisticCenter.ViewModels;

namespace LogisticCenter;

public partial class ProductsPage : ContentPage
{
	public ProductsPage()
	{
		InitializeComponent();
        BindingContext = new ProductsViewModel();
	}
}