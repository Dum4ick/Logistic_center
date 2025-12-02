using LogisticCenter.ViewModels;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;
using System.Text.Json;

namespace LogisticCenter
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new RegViewModel();
        }

        
    }

}
