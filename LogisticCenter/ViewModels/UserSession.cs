using CommunityToolkit.Mvvm.ComponentModel;

namespace LogisticCenter.Services
{
    public partial class UserSession : ObservableObject
    {
        private static UserSession _instance;
        public static UserSession Instance => _instance ??= new UserSession();

        private UserSession()
        {
            LoadFromStorage();
        }

        private string id;
        private string username;
        private string email;
        private string fullName;

        public string Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string FullName
        {
            get => fullName;
            set => SetProperty(ref fullName, value);
        }

        //сохранение данного пользователя
        public void SetUser(UserModel user)
        {
            Id = user.Id;
            Username = user.Name;
            Email = user.Email;
            FullName = user.FullName;

            SaveToStorage();
        }

        //сохранение
        private void SaveToStorage()
        {
            Preferences.Set("user_id", Id);
            Preferences.Set("username", Username);
            Preferences.Set("email", Email);
            Preferences.Set("full_name", FullName);
        }

        //загрузка при старте
        private void LoadFromStorage()
        {
            Id = Preferences.Get("user_id", null);
            Username = Preferences.Get("username", null);
            Email = Preferences.Get("email", null);
            FullName = Preferences.Get("full_name", null);
        }

        //выход
        public void Clear()
        {
            Preferences.Clear();

            Id = null;
            Username = null;
            Email = null;
            FullName = null;
        }

        public bool IsLoggedIn => !string.IsNullOrEmpty(Id);
    }
}
