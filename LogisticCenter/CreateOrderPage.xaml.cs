using LogisticCenter.ViewModels;

namespace LogisticCenter;

public partial class CreateOrderPage : ContentPage
{
    public CreateOrderPage()
    {
        InitializeComponent();
        BindingContext = new CreateOrderViewModel(); 
    }
}
