using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace LogisticCenter.ViewModels;

public partial class ReportsViewModel : ObservableObject
{
    public RoleService Roles => RoleService.Instance;

    [RelayCommand]
    async Task OpenStockReport()
    {
        await Shell.Current.GoToAsync("//stockreport");
    }

    [RelayCommand]
    async Task OpenShipmentReport()
    {
        await Shell.Current.GoToAsync("//shipmentreport");
    }

    [RelayCommand]
    async Task OpenFinanceReport()
    {
        await Shell.Current.GoToAsync("//financereport");
    }

    [RelayCommand]
    async Task GoBack()
    {
        await Shell.Current.GoToAsync("//main");
    }
}
