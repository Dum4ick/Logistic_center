using LogisticCenter.ViewModels;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;
using System.Text.Json;

namespace LogisticCenter;

    public partial class UsersPage : ContentPage

    {
        public UsersPage()
        {
            InitializeComponent();
            BindingContext = new UsersViewModel();
        }

            private void OnSearchCompleted(object sender, EventArgs e)
            {
                if (BindingContext is UsersViewModel vm)
                    vm.SearchCommand.Execute(null);
            }


}

