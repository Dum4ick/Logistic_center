using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogisticCenter.Data;

namespace LogisticCenter.ViewModels
{
    public partial class ShipmentReportViewModel : ObservableObject
    {
        private readonly ApiData _api = new();

        public ObservableCollection<ShipmentReportModel> Items { get; } = new();

        public ShipmentReportViewModel()
        {
            LoadCommand.Execute(null);
        }

        [RelayCommand]
        async Task Load()
        {
            Items.Clear();
            var data = await _api.GetShipmentReportAsync();
            foreach (var item in data)
                Items.Add(item);
        }

        [RelayCommand]
        async Task GoBack()
        {
            await Shell.Current.GoToAsync("//reports");
        }
    }

}
