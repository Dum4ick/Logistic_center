using LogisticCenter.ViewModels;

namespace LogisticCenter;

public partial class OrdersPage : ContentPage
{
	public OrdersPage()
	{
        InitializeComponent();
        BindingContext = new OrdersViewModel();
    }
}