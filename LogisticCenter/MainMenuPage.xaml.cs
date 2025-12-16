namespace LogisticCenter;

public partial class MainMenuPage : ContentPage
{
    public MainMenuPage()
    {
        InitializeComponent();
    }

    private async void Logout_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//login");
    }

}
