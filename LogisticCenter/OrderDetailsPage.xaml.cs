using LogisticCenter.ViewModels;

namespace LogisticCenter;

public partial class OrderDetailsPage : ContentPage
{
    public OrderDetailsPage()
    {
        InitializeComponent();
        BindingContext = new OrderDetailsViewModel();
    }
}

