
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogisticCenter.ViewModels
{
    public partial class RegViewModel : ObservableObject
    {
        public static readonly Regex LoginRegex = new(
            @"^[a-zA-Z_.-]{6,50}$",
            RegexOptions.Compiled);

        // Email: простой и рабочий вариант
        public static readonly Regex EmailRegex = new(
            @"^[\w.-]+@[\w.-]+\.[A-Za-z]{2,}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        // Пароль: минимум 8 символов, буква, цифра, спецсимвол
        public static readonly Regex PasswordRegex = new(
            @"^(?=.*[A-Za-z])(?=.*\d)(?=.*\W).{8,}$",
            RegexOptions.Compiled);

        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string useremail;

        [ObservableProperty]
        private string userpassword;

        [ObservableProperty]
        private string regResponse;

        [RelayCommand]
        private async void Registration()
        {
            if (!checkInfo(Username, Useremail, Userpassword))
                return;

            var api = new ApiData();
            var user = new UserModel
            {
                Name = Username,
                Email = Useremail,
                Password = Userpassword
            };

            var resultMessage = await api.RegisterUser(user);
            RegResponse = resultMessage;

        }

        private bool checkInfo(string username_, string email_, string password_)
        {
            if (!IsLoginValid(username_))
            {
                RegResponse = "Имя пользователя введено неправильно!";
                return false;
            }
            else if (!IsEmailValid(email_))
            {
                RegResponse = "Почта введена неправильно!";
                return false;
            }
            else if (!IsPasswordValid(password_))
            {
                RegResponse = "Пароль введён неправильно!";
                return false;
            }
            else 
            {
                RegResponse = "Все данные верны!";
                return true; 
            }
            
        }

        public static bool IsLoginValid(string login) =>
            !string.IsNullOrEmpty(login) && LoginRegex.IsMatch(login);

        public static bool IsEmailValid(string email) =>
        !string.IsNullOrEmpty(email) && EmailRegex.IsMatch(email);

        public static bool IsPasswordValid(string password) =>
            !string.IsNullOrEmpty(password) && PasswordRegex.IsMatch(password);
    }
}
