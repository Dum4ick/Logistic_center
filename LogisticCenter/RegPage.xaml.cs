using LogisticCenter.ViewModels;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;
using System.Text.Json;

namespace LogisticCenter;

    public partial class RegPage : ContentPage
    {

        public RegPage()
        {
            InitializeComponent();
            BindingContext = new RegViewModel();
        }

        
    }


