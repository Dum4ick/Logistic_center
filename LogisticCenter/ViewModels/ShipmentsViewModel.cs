using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LogisticCenter.Data;
using System.Collections.ObjectModel;

namespace LogisticCenter.ViewModels;

public partial class ShipmentsViewModel : ObservableObject
{
    private readonly ApiData _api = new();

    private List<ShipmentModel> _allShipments = new();

    public ObservableCollection<ShipmentModel> Shipments { get; } = new();

    public ObservableCollection<ShipmentStatusModel> Statuses { get; } = new();

    [ObservableProperty]
    ShipmentStatusModel selectedStatus;

    [ObservableProperty]
    string searchText;

    [ObservableProperty]
    ShipmentModel selectedShipment;

    public ShipmentsViewModel()
    {
        WeakReferenceMessenger.Default.Register<ShipmentsChangedMessage>(
             this, (_, _) => LoadCommand.Execute(null));

        LoadCommand.Execute(null);
    }

    [RelayCommand]
    async Task Load()
    {
        Shipments.Clear();
        _allShipments.Clear();
        Statuses.Clear();

        var shipments = await _api.GetShipmentsAsync();
        var statuses = await _api.GetShipmentStatusesAsync();

        Statuses.Add(new ShipmentStatusModel { Id = "0", Name = "Все статусы" });
        foreach (var s in statuses)
            Statuses.Add(s);

        SelectedStatus = Statuses.First();

        foreach (var s in shipments)
        {
            _allShipments.Add(s);
            Shipments.Add(s);
        }
    }

    [RelayCommand]
    void Search()
    {
        Shipments.Clear();

        string text = SearchText?.ToLower() ?? "";
        string statusId = SelectedStatus?.Id;

        var filtered = _allShipments.Where(s =>
            (
                string.IsNullOrWhiteSpace(text)
                || s.Id.ToString().Contains(text)
                || s.OrderId.ToString().Contains(text)
            )
            &&
            (
                statusId == "0"
                || s.StatusId == statusId
            )
        );

        foreach (var s in filtered)
            Shipments.Add(s);
    }

    partial void OnSearchTextChanged(string value)
        => SearchCommand.Execute(null);

    partial void OnSelectedStatusChanged(ShipmentStatusModel value)
        => SearchCommand.Execute(null);

    [RelayCommand]
    async Task OpenShipment()
    {
        await Task.Delay(10);

        if (SelectedShipment == null)
            return;

        await Shell.Current.GoToAsync(
            "//shipmentdetails",
            new Dictionary<string, object>
            {
                ["shipmentId"] = int.Parse(SelectedShipment.Id)
            });

        SelectedShipment = null;
    }


    [RelayCommand]
    async Task GoBack()
    {
        await Shell.Current.GoToAsync("//main");
    }
}
