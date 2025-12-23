using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LogisticCenter.Data;
using System.Collections.ObjectModel;

namespace LogisticCenter.ViewModels;

public partial class UsersViewModel : ObservableObject
{
    private readonly ApiData _api = new();

    public ObservableCollection<UserItemModel> Users { get; } = new();

    [ObservableProperty]
    private UserItemModel selectedUser;

    public UsersViewModel()
    {
        LoadUsersCommand.Execute(null);
    }

    // ===== ЗАГРУЗКА С СЕРВЕРА =====
    [RelayCommand]
    private async Task LoadUsers()
    {
        Users.Clear();

        var users = await _api.GetUsers();

        foreach (var u in users)
        {
            Users.Add(new UserItemModel
            {
                Id = u.Id,
                FullName = string.IsNullOrEmpty(u.FullName) ? u.Name : u.FullName,
                Email = u.Email,
                Role = u.RoleName,
                IsBlocked = false
            });
        }
    }

    [RelayCommand]
    async Task AddUser()
    {
        var popup = new AddUserPopup();
        await Shell.Current.CurrentPage.ShowPopupAsync(popup);
    }



    // ===== КОМАНДЫ =====
    [RelayCommand]
    void BlockUser()
    {
        if (SelectedUser == null) return;
        SelectedUser.IsBlocked = !SelectedUser.IsBlocked;
    }

    [RelayCommand]
    async Task DeleteUser()
    {
        if (SelectedUser == null) return;
        Users.Remove(SelectedUser);
    }

    [RelayCommand]
    async Task GoBack()
    {
        await Shell.Current.GoToAsync("//main");
    }
}
