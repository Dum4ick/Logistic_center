
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LogisticCenter.Data;
using LogisticCenter.Services;

namespace LogisticCenter.ViewModels
{
    public partial class RegViewModel : ObservableObject
    {
        //Логин должен содержать от 6 до 50 символов.
        //Разрешены только латинские буквы и цифры.
        public static readonly Regex LoginRegex = new(
            @"^[a-zA-Z0-9_.-]{6,50}$",
            RegexOptions.Compiled);


        // Email: логин@домен.ru
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
        private async Task Registration()
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
            Console.WriteLine("resultMessage: " + resultMessage);

            if (resultMessage == "Регистрация прошла успешно")
            {
                var (success, loggedUser, _) =
                    await api.LoginAsync(Useremail, Userpassword);

                Console.WriteLine("Ответ: "+success);

                if (success)
                {
                    UserSession.Instance.SetUser(loggedUser);
                    await Shell.Current.GoToAsync("//main");
                }
            }


        }

        private bool checkInfo(string username_, string email_, string password_)
        {
            if (!IsLoginValid(username_))
            {
                RegResponse = "Имя пользователя введено неправильно! Логин должен содержать от 6 до 50 символов.\n" +
                    "Разрешены только латинские буквы и цифры.";
                return false;
            }
            else if (!IsEmailValid(email_))
            {
                RegResponse = "Почта введена неправильно! Формат должен быть: логин@домен.ru";
                return false;
            }
            else if (!IsPasswordValid(password_))
            {
                RegResponse = "Пароль введён неправильно! Минимум 8 символов, буква(латиица), цифра, спецсимвол";
                return false;
            }
            else 
            {
                RegResponse = "Все данные верны!";
                return true; 
            }
            
        }

        [RelayCommand]
        async Task GoToLogin()
        {
            await Shell.Current.GoToAsync("//login");
        }

        public static bool IsLoginValid(string login) =>
            !string.IsNullOrEmpty(login) && LoginRegex.IsMatch(login);

        public static bool IsEmailValid(string email) =>
        !string.IsNullOrEmpty(email) && EmailRegex.IsMatch(email);

        public static bool IsPasswordValid(string password) =>
            !string.IsNullOrEmpty(password) && PasswordRegex.IsMatch(password);
    }
}
