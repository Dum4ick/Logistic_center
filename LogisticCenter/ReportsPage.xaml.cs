using LogisticCenter.ViewModels;

namespace LogisticCenter;

public partial class ReportsPage : ContentPage
{
	public ReportsPage()
	{
		InitializeComponent();
        BindingContext = new ReportsViewModel();
    }
}