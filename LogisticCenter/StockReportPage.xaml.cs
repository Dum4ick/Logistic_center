using LogisticCenter.ViewModels;

namespace LogisticCenter;

public partial class StockReportPage : ContentPage
{
	public StockReportPage()
	{
		InitializeComponent();
        BindingContext = new StockReportViewModel();
    }
}