using LogisticCenter.ViewModels;

namespace LogisticCenter;

public partial class WarehousesPage : ContentPage
{
	public WarehousesPage()
	{
		InitializeComponent();
        BindingContext = new WarehousesViewModel();
    }
}