using LogisticCenter.ViewModels;

namespace LogisticCenter;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        BindingContext = new LoginPageViewModel();
    }
}
