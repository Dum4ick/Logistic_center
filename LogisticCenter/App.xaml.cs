using LogisticCenter.Services;

namespace LogisticCenter
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            Dispatcher.Dispatch(async () =>
            {
                if (UserSession.Instance.IsLoggedIn)
                    await Shell.Current.GoToAsync("//main");
                else
                    await Shell.Current.GoToAsync("//login");
            });
        }
    }
}
