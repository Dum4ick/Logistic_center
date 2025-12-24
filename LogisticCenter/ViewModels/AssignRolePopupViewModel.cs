using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using LogisticCenter.ViewModels;
using LogisticCenter.Data;

namespace LogisticCenter;

public partial class AssignRolePopupViewModel : ObservableObject
{
    private readonly Popup _popup;
    private readonly ApiData _api = new();
    private readonly UsersViewModel _usersVm;
    private readonly int _userId;

    public ObservableCollection<RoleModel> Roles { get; } = new();

    [ObservableProperty]
    private RoleModel selectedRole;

    public AssignRolePopupViewModel(
        Popup popup,
        int userId,
        UsersViewModel usersVm)
    {
        _popup = popup;
        _userId = userId;
        _usersVm = usersVm;

        LoadRolesCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadRoles()
    {
        Roles.Clear();

        var roles = await _api.GetRolesAsync();
        foreach (var r in roles)
            Roles.Add(r);
    }

    [RelayCommand]
    private async Task Save()
    {
        if (SelectedRole == null)
            return;

        var result = await _api.AssignRoleAsync(_userId, Convert.ToInt32(SelectedRole.Id));

        if (result.success)
        {
            await Application.Current.MainPage.DisplayAlert("Успех", result.message, "OK");
            _popup.Close();
            _usersVm.LoadUsersCommand.Execute(null);
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Ошибка", result.message, "OK");
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        _popup.Close();
    }
}