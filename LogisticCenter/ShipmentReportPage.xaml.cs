using LogisticCenter.ViewModels;

namespace LogisticCenter;

public partial class ShipmentReportPage : ContentPage
{
	public ShipmentReportPage()
	{
		InitializeComponent();
        BindingContext = new ShipmentReportViewModel();
    }
}