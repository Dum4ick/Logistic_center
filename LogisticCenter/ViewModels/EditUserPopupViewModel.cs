using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LogisticCenter.Data;

namespace LogisticCenter.ViewModels;

public partial class EditUserPopupViewModel : ObservableObject
{
    private readonly Popup _popup;
    private readonly UsersViewModel _usersVm;
    private readonly ApiData _api = new();
    private readonly int _userId;

    [ObservableProperty] private string fullName;
    [ObservableProperty] private string email;

    public EditUserPopupViewModel(
        Popup popup,
        UserItemModel user,
        UsersViewModel usersVm)
    {
        _popup = popup;
        _usersVm = usersVm;
        _userId = Convert.ToInt32(user.Id);

        FullName = user.FullName;
        Email = user.Email;
    }

    [RelayCommand]
    void Cancel() => _popup.Close();

    [RelayCommand]
    async Task Save()
    {
        if (string.IsNullOrWhiteSpace(FullName) ||
            string.IsNullOrWhiteSpace(Email))
        {
            await Shell.Current.DisplayAlert(
                "Ошибка",
                "Заполните все поля",
                "OK");
            return;
        }

        var result = await _api.UpdateUserAsync(
            _userId,
            FullName,
            Email);

        if (result.success)
        {
            await Shell.Current.DisplayAlert("Успех", result.message, "OK");
            _usersVm.LoadUsersCommand.Execute(null);
            _popup.Close();
        }
        else
        {
            await Shell.Current.DisplayAlert("Ошибка", result.message, "OK");
        }
    }
}
