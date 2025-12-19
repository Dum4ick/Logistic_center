using CommunityToolkit.Mvvm.ComponentModel;

namespace LogisticCenter.Services
{
    public partial class UserSession : ObservableObject
    {
        private static UserSession _instance;
        public static UserSession Instance =>
            _instance ??= new UserSession();

        private UserSession() { }

        [ObservableProperty]
        private string id;

        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string fullName;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string roleId;

        public void SetUser(UserModel user)
        {
            Id = user.Id;
            Username = user.Name;
            Email = user.Email;
            RoleId = user.RoleId;

            // если full_name null — используем username
            FullName = string.IsNullOrWhiteSpace(user.FullName)
            ? user.Name
            : user.FullName;

            System.Diagnostics.Debug.WriteLine($"EMAIL FROM API: {user.Email}");
        }

        public void Clear()
        {
            Id = null;
            Username = null;
            FullName = null;
            Email = null;
            RoleId = null;
        }
    }
}
