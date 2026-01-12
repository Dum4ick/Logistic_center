using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LogisticCenter.Data;
using LogisticCenter.ViewModels;
using System.Collections.ObjectModel;

namespace LogisticCenter;

public partial class UsersViewModel : ObservableObject
{
    private readonly ApiData _api = new();

    public ObservableCollection<UserModel> Users { get; } = new();

    [ObservableProperty]
    private UserModel selectedUser;

    //
    private List<UserModel> _allUsers = new();


    public UsersViewModel()
    {
        LoadUsersCommand.Execute(null);
        LoadRolesCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadUsers()
    {
        Users.Clear();
        _allUsers.Clear();

        var users = await _api.GetUsers();

        foreach (var u in users)
        {
            var user = new UserModel
            {
                Id = u.Id,
                FullName = string.IsNullOrEmpty(u.FullName) ? u.Name : u.FullName,
                Email = u.Email,
                RoleName = u.RoleName,
                RoleId = u.RoleId,
                IsBlocked = u.IsBlocked
            };

            _allUsers.Add(user);
            Users.Add(user);
        }
    }


    [RelayCommand]
    async Task AddUser()
    {
        var popup = new AddUserPopup(this);
        await Shell.Current.CurrentPage.ShowPopupAsync(popup);
    }

    [RelayCommand]
    async Task AssignRole()
    {
        if (SelectedUser == null)
            return;
        var popup = new AssignRolePopup(
            Convert.ToInt32(SelectedUser.Id),
            this);

        await Shell.Current.CurrentPage.ShowPopupAsync(popup);
    }

    [RelayCommand]
    async Task EditUser()
    {
        if (SelectedUser == null)
            return;

        var popup = new EditUserPopup(SelectedUser, this);
        await Shell.Current.CurrentPage.ShowPopupAsync(popup);
    }


    [RelayCommand]
    void BlockUser()
    {
        if (SelectedUser == null) return;
        //SelectedUser.IsBlocked = !SelectedUser.IsBlocked;
    }

    [RelayCommand]
    async Task LoadRoles()
    {
        Roles.Clear();

        Roles.Add(new RoleModel
        {
            Id = "0",
            RoleName = "Все роли"
        });

        var roles = await _api.GetRolesAsync();
        foreach (var r in roles)
            Roles.Add(r);

        SelectedRole = Roles.First();
    }

    [RelayCommand]
    void Search()
    {
        if (_allUsers.Count == 0)
            return;

        Users.Clear();

        string text = SearchText?.ToLower() ?? "";
        string selectedRoleId = SelectedRole?.Id;

        var filtered = _allUsers.Where(u =>
            (
                string.IsNullOrWhiteSpace(text)
                || (u.FullName?.ToLower().Contains(text) ?? false)
                || (u.Email?.ToLower().Contains(text) ?? false)
                || (u.RoleName?.ToLower().Contains(text) ?? false)
            )
            &&
            (
                selectedRoleId == "0"
                || u.RoleId == selectedRoleId
            )
        );

        foreach (var user in filtered)
            Users.Add(user);
    }

    partial void OnSearchTextChanged(string value)
    {
        SearchCommand.Execute(null);
    }


    [RelayCommand]
    async Task DeleteUser()
    {
        if (SelectedUser == null)
            return;

        bool confirm = await Shell.Current.DisplayAlert(
            "Удаление пользователя",
            $"Удалить пользователя {SelectedUser.FullName}?",
            "Да",
            "Отмена");

        if (!confirm)
            return;

        var result = await _api.DeleteUserAsync(SelectedUser.Id);

        if (result.success)
        {
            await Shell.Current.DisplayAlert("Успех", result.message, "OK");
            LoadUsersCommand.Execute(null); // перезагрузка списка
        }
        else
        {
            await Shell.Current.DisplayAlert("Ошибка", result.message, "OK");
        }
    }



    [ObservableProperty]
    string searchText;

    [ObservableProperty]
    RoleModel selectedRole;

    partial void OnSelectedRoleChanged(RoleModel value)
    {
        SearchCommand.Execute(null);
    }


    public ObservableCollection<RoleModel> Roles { get; } = new();


    [RelayCommand]
    async Task GoBack()
    {
        await Shell.Current.GoToAsync("//main");
    }
}
