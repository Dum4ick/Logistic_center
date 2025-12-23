using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LogisticCenter.Data;
using LogisticCenter.Services;
using System.ComponentModel;
using System.Threading.Tasks;

namespace LogisticCenter.ViewModels
{
    public partial class MainMenuViewModel : ObservableObject
    {
        public MainMenuViewModel()
        {
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

        public string Username => $"@{UserSession.Instance.Username}";

        public string Email => UserSession.Instance.Email ?? string.Empty;

        // ===== СОСТОЯНИЕ МЕНЮ ПРОФИЛЯ =====

        [ObservableProperty]
        private bool isProfileMenuVisible;

        // ===== КОМАНДЫ =====

        [RelayCommand]
        void ToggleProfileMenu()
        {
            IsProfileMenuVisible = !IsProfileMenuVisible;
        }

        // 🔹 НАЗНАЧЕНИЕ ИМЕНИ (ТО ЧТО ТЕБЕ НУЖНО)
        [RelayCommand]
        async Task SetFullName()
        {
            IsProfileMenuVisible = false;

            string result = await Shell.Current.DisplayPromptAsync(
                "Назначить имя",
                "Введите имя (до 200 символов)",
                accept: "Сохранить",
                cancel: "Отмена",
                placeholder: "Полное имя",
                maxLength: 200,
                keyboard: Keyboard.Text
            );

            if (string.IsNullOrWhiteSpace(result))
                return;

            var api = new ApiData();
            var response = await api.UpdateFullNameAsync(
                Convert.ToInt32(UserSession.Instance.Id),
                result.Trim()
            );

            if (response == "OK")
            {
                UserSession.Instance.FullName = result.Trim();
            }
            else
            {
                await Shell.Current.DisplayAlert(
                    "Ошибка",
                    response,
                    "OK"
                );
            }
        }

        // ===== ВЫХОД С ПОДТВЕРЖДЕНИЕМ =====
        [RelayCommand]
        async Task Logout_Clicked()
        {
            IsProfileMenuVisible = false;

            bool confirm = await Shell.Current.DisplayAlert(
                "Выход",
                "Вы действительно хотите выйти из аккаунта?",
                "Да",
                "Отмена"
            );

            if (!confirm) return;

            UserSession.Instance.Clear();
            await Shell.Current.GoToAsync("//login");
        }

        [RelayCommand]
        async Task CloseApp()
        {
            IsProfileMenuVisible = false;

            bool confirm = await Shell.Current.DisplayAlert(
                "Выход",
                "Вы действительно хотите выйти?",
                "Да",
                "Отмена"
            );

            if (!confirm) return;

            Application.Current.Quit();
        }

        [RelayCommand]
        async Task GoToUsers()
        {
            await Shell.Current.GoToAsync("//users");
        }

        [RelayCommand]
        async Task GoToProducts()
        {
            await Shell.Current.GoToAsync("//products");
        }
    }
}
