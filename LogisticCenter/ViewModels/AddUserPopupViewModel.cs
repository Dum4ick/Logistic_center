using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LogisticCenter.Data;
using System.Xml.Linq;

namespace LogisticCenter.ViewModels;

public partial class AddUserPopupViewModel : ObservableObject
{
    private readonly Popup _popup;
    private readonly ApiData _api = new();

    [ObservableProperty] private string username;
    [ObservableProperty] private string email;
    [ObservableProperty] private string password;

    public AddUserPopupViewModel(Popup popup)
    {
        _popup = popup;
    }

    [RelayCommand]
    async Task Close()
    {
        _popup.Close();
    }

    [RelayCommand]
    async Task Save()
    {
        if (string.IsNullOrWhiteSpace(Username) ||
            string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Password))
        {
            await Application.Current.MainPage.DisplayAlert(
                "Ошибка", "Заполните все поля", "OK");
            return;
        }

        var result = await _api.RegisterUser(new UserModel
        {
            Name = Username,
            Email = Email,
            Password = Password
        });

        await Application.Current.MainPage.DisplayAlert("Результат", result, "OK");

        _popup.Close();

        // Обновляем список пользователей
        if (Application.Current.MainPage.BindingContext is UsersViewModel usersVm)
        {
            usersVm.LoadUsersCommand.Execute(null);
        }
    }
}
