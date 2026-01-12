using LogisticCenter.ViewModels;

namespace LogisticCenter;

public partial class ShipmentDetailsPage : ContentPage
{
    public ShipmentDetailsPage()
    {
        InitializeComponent();
        BindingContext = new ShipmentDetailsViewModel();
    }
}
