using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LogisticCenter.ViewModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LogisticCenter.ViewModels
{
    public partial class UsersViewModel : ObservableObject
    {
        public ObservableCollection<UserItemModel> Users { get; } = new();

        [ObservableProperty]
        private UserItemModel selectedUser;

        public UsersViewModel()
        {
            LoadTestUsers();
        }

        // ===== ТЕСТОВЫЕ ДАННЫЕ =====
        private void LoadTestUsers()
        {
            Users.Add(new UserItemModel
            {
                Id = "1",
                FullName = "Администратор",
                Email = "admin@mail.ru",
                Role = "Admin",
                IsBlocked = false
            });

            Users.Add(new UserItemModel
            {
                Id = "2",
                FullName = "Менеджер склада",
                Email = "manager@mail.ru",
                Role = "Manager",
                IsBlocked = false
            });

            Users.Add(new UserItemModel
            {
                Id = "3",
                FullName = "Заблокированный пользователь",
                Email = "blocked@mail.ru",
                Role = "User",
                IsBlocked = true
            });
        }

        // ===== КОМАНДЫ =====

        [RelayCommand]
        async Task AddUser()
        {
            await Shell.Current.DisplayAlert("Добавить", "Добавить пользователя (заглушка)", "OK");
        }

        [RelayCommand]
        async Task EditUser()
        {
            if (SelectedUser == null) return;
            await Shell.Current.DisplayAlert("Редактировать", SelectedUser.FullName, "OK");
        }

        [RelayCommand]
        async Task DeleteUser()
        {
            if (SelectedUser == null) return;
            Users.Remove(SelectedUser);
        }

        [RelayCommand]
        async Task AssignRole()
        {
            if (SelectedUser == null) return;
            await Shell.Current.DisplayAlert("Роль", "Назначить роль (заглушка)", "OK");
        }

        [RelayCommand]
        async Task BlockUser()
        {
            if (SelectedUser == null) return;
            SelectedUser.IsBlocked = !SelectedUser.IsBlocked;
            OnPropertyChanged(nameof(Users));
        }

        [RelayCommand]
        async Task GoBack()
        {
            await Shell.Current.GoToAsync("//main");
        }

    }
}
