using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Views;
using LogisticCenter.Data;
using System.Collections.ObjectModel;

namespace LogisticCenter.ViewModels;

public partial class WriteOffStockViewModel : ObservableObject
{
    private readonly Popup _popup;
    private readonly ApiData _api;

    public ObservableCollection<WarehouseModel> Warehouses { get; } = new();

    [ObservableProperty]
    ProductModel selectedProduct;

    [ObservableProperty]
    WarehouseModel selectedWarehouse;

    [ObservableProperty]
    string quantity;

    public WriteOffStockViewModel(Popup popup, ApiData api)
    {
        _popup = popup;
        _api = api;
        LoadWarehouses();
    }

    private async void LoadWarehouses()
    {
        var warehouses = await _api.GetWarehousesAsync();
        foreach (var w in warehouses)
            Warehouses.Add(w);
    }

    [RelayCommand]
    async Task SelectProduct()
    {
        if (SelectedWarehouse == null)
        {
            await Shell.Current.DisplayAlert(
                "Ошибка",
                "Сначала выберите склад",
                "OK");
            return;
        }

        var result = await Shell.Current.ShowPopupAsync(
            new SelectStockPopup(
                _api,
                Convert.ToInt32(SelectedWarehouse.Id)
            )
        );

        if (result is StockModel stock)
            SelectedProduct = new ProductModel
            {
                Id = stock.ProductId,
                Name = stock.ProductName
            };
    }



    [RelayCommand]
    async Task WriteOff()
    {
        if (SelectedProduct == null || SelectedWarehouse == null)
        {
            await Shell.Current.DisplayAlert(
                "Ошибка",
                "Выберите товар и склад",
                "OK");
            return;
        }

        if (!int.TryParse(Quantity, out int qty) || qty <= 0)
        {
            await Shell.Current.DisplayAlert(
                "Ошибка",
                "Введите корректное количество",
                "OK");
            return;
        }

        var result = await _api.WriteOffStockAsync(
            Convert.ToInt32(SelectedProduct.Id),
            Convert.ToInt32(SelectedWarehouse.Id),
            qty
        );

        if (!result.success)
        {
            await Shell.Current.DisplayAlert("Ошибка", result.message, "OK");
            return;
        }

        _popup.Close(true);
    }

    [RelayCommand]
    void Close()
    {
        _popup.Close(false);
    }
}
