using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LogisticCenter.Services;
using System.ComponentModel;
using System.Threading.Tasks;
using LogisticCenter.Data;

namespace LogisticCenter.ViewModels
{
    public partial class MainMenuViewModel : ObservableObject
    {
        public RoleService Roles => RoleService.Instance;

        // ===== РОЛИ =====
        public bool IsAdmin => UserSession.Instance.RoleId == "3";
        public bool IsManager => UserSession.Instance.RoleId == "2";
        public bool IsUser => UserSession.Instance.RoleId == "1";

        // ===== УРОВНИ ДОСТУПА =====
        public bool Level3 => IsAdmin;                     
        public bool Level2 => IsAdmin || IsManager;       
        public bool Level1 => IsAdmin || IsManager || IsUser; 

        public MainMenuViewModel()
        {
            // Подписываемся на изменения UserSession
            UserSession.Instance.PropertyChanged += OnUserSessionChanged;

            // Обновляем состояние ролей при загрузке
            RefreshRoleState();
        }

        // Обновление ролей и данных пользователя
        public void RefreshRoleState()
        {
            OnPropertyChanged(nameof(IsAdmin));
            OnPropertyChanged(nameof(IsManager));
            OnPropertyChanged(nameof(IsUser));

            OnPropertyChanged(nameof(Level1));
            OnPropertyChanged(nameof(Level2));
            OnPropertyChanged(nameof(Level3));

            OnPropertyChanged(nameof(FullName));
            OnPropertyChanged(nameof(Username));
            OnPropertyChanged(nameof(Email));
        }

        private void OnUserSessionChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(UserSession.RoleId))
                RefreshRoleState();

            if (e.PropertyName == nameof(UserSession.FullName) ||
                e.PropertyName == nameof(UserSession.Username) ||
                e.PropertyName == nameof(UserSession.Email))
                RefreshRoleState();
        }

        public string FullName =>
            !string.IsNullOrWhiteSpace(UserSession.Instance.FullName)
                ? UserSession.Instance.FullName
                : UserSession.Instance.Username;

        public string Username => $"@{UserSession.Instance.Username}";
        public string Email => UserSession.Instance.Email ?? string.Empty;

        [ObservableProperty]
        private bool isProfileMenuVisible;

        [RelayCommand]
        void ToggleProfileMenu() => IsProfileMenuVisible = !IsProfileMenuVisible;

        [RelayCommand]
        async Task SetFullName()
        {
            IsProfileMenuVisible = false;

            string result = await Shell.Current.DisplayPromptAsync(
                "Назначить имя",
                "Введите имя (до 200 символов)",
                "Сохранить", "Отмена",
                placeholder: "Полное имя",
                maxLength: 200
            );

            if (string.IsNullOrWhiteSpace(result))
                return;

            var api = new ApiData();
            var response = await api.UpdateFullNameAsync(
                Convert.ToInt32(UserSession.Instance.Id),
                result.Trim()
            );

            if (response == "OK")
                UserSession.Instance.FullName = result.Trim();
            else
                await Shell.Current.DisplayAlert("Ошибка", response, "OK");
        }

        [RelayCommand]
        async Task Logout_Clicked()
        {
            IsProfileMenuVisible = false;

            bool confirm = await Shell.Current.DisplayAlert(
                "Выход", "Вы действительно хотите выйти?", "Да", "Отмена");

            if (!confirm) return;

            UserSession.Instance.Clear();
            await Shell.Current.GoToAsync("//login");
        }

        [RelayCommand]
        async Task CloseApp()
        {
            IsProfileMenuVisible = false;

            bool confirm = await Shell.Current.DisplayAlert(
                "Выход", "Вы действительно хотите выйти?", "Да", "Отмена");

            if (!confirm) return;

            Application.Current.Quit();
        }

        [RelayCommand] async Task GoToUsers() => await Shell.Current.GoToAsync("//users");
        [RelayCommand] async Task GoToProducts() => await Shell.Current.GoToAsync("//products");
        [RelayCommand] async Task GoToOrders() => await Shell.Current.GoToAsync("//orders");
        [RelayCommand] async Task GoToStock() => await Shell.Current.GoToAsync("//stock");
        [RelayCommand] async Task GoToShipment() => await Shell.Current.GoToAsync("//shipment");
        [RelayCommand] async Task GoToWarehouses() => await Shell.Current.GoToAsync("//warehouses");
        [RelayCommand] async Task GoToReports() => await Shell.Current.GoToAsync("//reports");



    }
}
