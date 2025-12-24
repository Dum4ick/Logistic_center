using LogisticCenter.ViewModels;

namespace LogisticCenter;

public partial class StockPage : ContentPage
{
	public StockPage()
	{
		InitializeComponent();
        BindingContext = new StockViewModel();
    }
}