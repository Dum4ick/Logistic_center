using LogisticCenter.ViewModels;

namespace LogisticCenter;

public partial class ShipmentsPage : ContentPage
{
	public ShipmentsPage()
	{

        InitializeComponent();
        BindingContext = new ShipmentsViewModel();
    }
}