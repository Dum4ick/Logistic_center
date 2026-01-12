using LogisticCenter.ViewModels;

namespace LogisticCenter;

public partial class FinanceReportPage : ContentPage
{
	public FinanceReportPage()
	{
		InitializeComponent();
        BindingContext = new FinanceReportViewModel();
    }
}