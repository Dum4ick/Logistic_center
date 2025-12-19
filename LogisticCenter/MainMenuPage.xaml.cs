using LogisticCenter.ViewModels;

namespace LogisticCenter;

public partial class MainMenuPage : ContentPage
{
    public MainMenuPage()
    {
        InitializeComponent();
        BindingContext = new MainMenuViewModel();
    }

}
