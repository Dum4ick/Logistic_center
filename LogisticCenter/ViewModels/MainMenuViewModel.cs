using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LogisticCenter.Services;
using System.ComponentModel;
using System.Threading.Tasks;

namespace LogisticCenter.ViewModels
{
    public partial class MainMenuViewModel : ObservableObject
    {
        public MainMenuViewModel()
        {
            // 🔄 Обновляем UI при изменении данных пользователя
            UserSession.Instance.PropertyChanged += OnUserSessionChanged;
        }

        private void OnUserSessionChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(FullName));
            OnPropertyChanged(nameof(Username));
            OnPropertyChanged(nameof(Email));
        }

        // ===== ДАННЫЕ ПОЛЬЗОВАТЕЛЯ =====

        public string FullName =>
            !string.IsNullOrWhiteSpace(UserSession.Instance.FullName)
                ? UserSession.Instance.FullName
                : UserSession.Instance.Username;

        public string Username =>
            !string.IsNullOrWhiteSpace(UserSession.Instance.Username)
                ? $"@{UserSession.Instance.Username}"
                : string.Empty;

        public string Email =>
            UserSession.Instance.Email ?? string.Empty;

        // ===== СОСТОЯНИЕ МЕНЮ ПРОФИЛЯ =====

        [ObservableProperty]
        private bool isProfileMenuVisible;

        // ===== КОМАНДЫ =====

        [RelayCommand]
        void ToggleProfileMenu()
        {
            IsProfileMenuVisible = !IsProfileMenuVisible;
        }

        [RelayCommand]
        async Task Logout_Clicked()
        {
            IsProfileMenuVisible = false;

            bool confirm = await Shell.Current.DisplayAlert(
                "Выход из аккаунта",
                "Вы действительно хотите выйти?",
                "Да",
                "Отмена");

            if (!confirm)
                return;

            UserSession.Instance.Clear();
            await Shell.Current.GoToAsync("//login");
        }


        [RelayCommand]
        async Task GoToUsers()
        {
            await Shell.Current.GoToAsync("//users");
        }

    }
}
