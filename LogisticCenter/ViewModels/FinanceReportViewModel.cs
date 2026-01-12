using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogisticCenter.Data;

namespace LogisticCenter.ViewModels
{
    public partial class FinanceReportViewModel : ObservableObject
    {
        private readonly ApiData _api = new();

        [ObservableProperty] FinanceReportModel report;

        public FinanceReportViewModel()
        {
            LoadCommand.Execute(null);
        }

        [RelayCommand]
        async Task Load()
        {
            Report = await _api.GetFinanceReportAsync();
        }

        [RelayCommand]
        async Task GoBack()
        {
            await Shell.Current.GoToAsync("//reports");
        }
    }

}
