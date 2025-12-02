using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace LogisticCenter
{
    public partial class GetUserViewModel : ObservableObject
    {
        //[ObservableProperty]

        //[RelayCommand]

        [ObservableProperty]
        private ObservableCollection<UserModel> users;

        private readonly ApiData apiData;

        public GetUserViewModel()
        {
            apiData = new ApiData();
            //LoadUsers();
        }

        [RelayCommand]
        private async void LoadUsers()
        {
            var userList = await apiData.GetUsers();
            Users = new ObservableCollection<UserModel>(userList);
        }

    }
}