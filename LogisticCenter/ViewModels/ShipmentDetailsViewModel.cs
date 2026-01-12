using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LogisticCenter.Data;
using System.Collections.ObjectModel;

namespace LogisticCenter.ViewModels;

[QueryProperty(nameof(ShipmentId), "shipmentId")]
public partial class ShipmentDetailsViewModel : ObservableObject
{
    private readonly ApiData _api = new();

    [ObservableProperty]
    int shipmentId;

    [ObservableProperty]
    int currentStatusId;

    [ObservableProperty]
    string statusName;

    public ObservableCollection<ShipmentItemModel> Items { get; } = new();

    public bool CanStart => CurrentStatusId == 1;          // Создана
    public bool CanSendToDelivery => CurrentStatusId == 2; // Собирается
    public bool CanComplete => CurrentStatusId == 3;       // В доставке
    public bool CanCancel => CurrentStatusId is 1 or 2;    // Можно отменить до отправки

    partial void OnCurrentStatusIdChanged(int value)
    {
        OnPropertyChanged(nameof(CanStart));
        OnPropertyChanged(nameof(CanSendToDelivery));
        OnPropertyChanged(nameof(CanComplete));
        OnPropertyChanged(nameof(CanCancel));
    }


    [RelayCommand]
    async Task Load()
    {
        if (ShipmentId <= 0) return;

        Items.Clear();

        var items = await _api.GetShipmentItemsAsync(ShipmentId);
        var status = await _api.GetShipmentStatusAsync(ShipmentId);

        if (status != null)
        {
            CurrentStatusId = Convert.ToInt32(status.Id);
            StatusName = status.Name;
        }

        foreach (var i in items)
            Items.Add(i);
    }

    partial void OnShipmentIdChanged(int value)
    {
        LoadCommand.Execute(null);
    }

    [RelayCommand]
    async Task StartShipment()
    {
        await _api.UpdateShipmentStatusAsync(ShipmentId, 2);

        WeakReferenceMessenger.Default.Send(new OrdersChangedMessage("StatusChanged"));
        WeakReferenceMessenger.Default.Send(new ShipmentsChangedMessage("StatusChanged"));

        await Load();
    }

    [RelayCommand]
    async Task SendToDelivery()
    {
        await _api.UpdateShipmentStatusAsync(ShipmentId, 3);

        WeakReferenceMessenger.Default.Send(new OrdersChangedMessage("StatusChanged"));
        WeakReferenceMessenger.Default.Send(new ShipmentsChangedMessage("StatusChanged"));
        WeakReferenceMessenger.Default.Send(new StockChangedMessage("QuantityChanged"));

        await Load();
    }


    [RelayCommand]
    async Task CompleteShipment()
    {
        await _api.UpdateShipmentStatusAsync(ShipmentId, 4);

        WeakReferenceMessenger.Default.Send(new OrdersChangedMessage("StatusChanged"));
        WeakReferenceMessenger.Default.Send(new ShipmentsChangedMessage("StatusChanged"));

        await Load();
    }

    [RelayCommand]
    async Task CancelShipment()
    {
        await _api.UpdateShipmentStatusAsync(ShipmentId, 5);

        WeakReferenceMessenger.Default.Send(new OrdersChangedMessage("StatusChanged"));
        WeakReferenceMessenger.Default.Send(new ShipmentsChangedMessage("StatusChanged"));

        await Load();
    }



    [RelayCommand]
    async Task GoBack()
    {
        await Shell.Current.GoToAsync("//shipment");
    }
}
