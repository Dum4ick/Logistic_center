using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LogisticCenter.Data;
using LogisticCenter.Services;
using LogisticCenter.ViewModels;

namespace LogisticCenter.ViewModels;

public partial class LoginPageViewModel : ObservableObject
{
    [ObservableProperty] private string email;
    [ObservableProperty] private string password;
    [ObservableProperty] private string loginResponse;

    private readonly ApiData _apiData;

    public LoginPageViewModel()
    {
        _apiData = new ApiData();
    }

    [RelayCommand]
    async Task Login()
    {
        var (success, user, message) =
            await _apiData.LoginAsync(Email, Password);

        if (success)
        {
            // ✅ сохраняем пользователя
            UserSession.Instance.SetUser(user);

            await Shell.Current.GoToAsync("//main");
        }
        else
        {
            LoginResponse = message;
        }
    }


    [RelayCommand]
    async Task GoToRegister()
    {
        await Shell.Current.GoToAsync("//register");
    }

}
