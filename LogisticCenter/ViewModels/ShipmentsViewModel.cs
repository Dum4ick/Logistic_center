using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace LogisticCenter.ViewModels;

public partial class ShipmentsViewModel : ObservableObject
{
    public ObservableCollection<ShipmentItemModel> Shipments { get; } = new();

    public ShipmentsViewModel()
    {
        Shipments.Add(new ShipmentItemModel
        {
            ShipmentNumber = "SHP-001",
            Date = "12.05.2025",
            Status = "Отправлена"
        });

        Shipments.Add(new ShipmentItemModel
        {
            ShipmentNumber = "SHP-002",
            Date = "13.05.2025",
            Status = "Готовится"
        });
    }

    [RelayCommand]
    async Task GoBack()
    {
        await Shell.Current.GoToAsync("//main");
    }

}

public class ShipmentItemModel
{
    public string ShipmentNumber { get; set; }
    public string Date { get; set; }
    public string Status { get; set; }
}
