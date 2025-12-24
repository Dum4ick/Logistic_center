using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LogisticCenter.Data;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace LogisticCenter.ViewModels;

public partial class AddUserPopupViewModel : ObservableObject
{
    private readonly Popup _popup;
    private readonly UsersViewModel _usersVm;
    private readonly ApiData _api = new();

    [ObservableProperty] private string username;
    [ObservableProperty] private string email;
    [ObservableProperty] private string password;

    //Логин должен содержать от 6 до 50 символов.
    //Разрешены только латинские буквы и цифры.
    public static readonly Regex LoginRegex = new(
        @"^[a-zA-Z0-9_.-]{6,50}$",
        RegexOptions.Compiled);


    // Email: логин@домен.ru
    public static readonly Regex EmailRegex = new(
        @"^[\w.-]+@[\w.-]+\.[A-Za-z]{2,}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    // Пароль: минимум 8 символов, буква, цифра, спецсимвол
    public static readonly Regex PasswordRegex = new(
        @"^(?=.*[A-Za-z])(?=.*\d)(?=.*\W).{8,}$",
        RegexOptions.Compiled);

    public AddUserPopupViewModel(Popup popup, UsersViewModel usersVm)
    {
        _popup = popup;
        _usersVm = usersVm;
    }

    [RelayCommand]
    async Task Close()
    {
        _popup.Close();
    }

    private async Task<bool> CheckInfoAsync()
    {
        if (!LoginRegex.IsMatch(Username))
        {
            await Application.Current.MainPage.DisplayAlert(
                "Ошибка",
                "Имя пользователя должно содержать от 6 до 50 символов.\nДопустимы латинские буквы, цифры, . _ -",
                "OK");
            return false;
        }

        if (!EmailRegex.IsMatch(Email))
        {
            await Application.Current.MainPage.DisplayAlert(
                "Ошибка",
                "Неверный формат email (логин@домен.ru)",
                "OK");
            return false;
        }

        if (!PasswordRegex.IsMatch(Password))
        {
            await Application.Current.MainPage.DisplayAlert(
                "Ошибка",
                "Пароль: минимум 8 символов, латинская буква, цифра и спецсимвол",
                "OK");
            return false;
        }

        return true;
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

        bool isValid = await CheckInfoAsync();
        if (!isValid)
            return;

        var result = await _api.RegisterUser(new UserModel
        {
            Name = Username,
            Email = Email,
            Password = Password
        });

        await Application.Current.MainPage.DisplayAlert("Результат", result, "OK");

        _popup.Close();

        _usersVm.LoadUsersCommand.Execute(null);
    }

}
