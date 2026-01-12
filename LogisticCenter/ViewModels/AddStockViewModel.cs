using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Views;
using LogisticCenter.Data;
using System.Collections.ObjectModel;

namespace LogisticCenter.ViewModels;

public partial class AddStockViewModel : ObservableObject
{
    private readonly Popup _popup;
    private readonly ApiData _api;

    public ObservableCollection<ProductModel> Products { get; } = new();
    public ObservableCollection<WarehouseModel> Warehouses { get; } = new();

    [ObservableProperty]
    ProductModel selectedProduct;

    [ObservableProperty]
    WarehouseModel selectedWarehouse;

    [ObservableProperty]
    string quantity;

    public AddStockViewModel(Popup popup, ApiData api)
    {
        _popup = popup;
        _api = api;

        LoadData();
    }

    private async void LoadData()
    {
        var products = await _api.GetProducts();
        foreach (var p in products)
            Products.Add(p);

        var warehouses = await _api.GetWarehousesAsync();
        foreach (var w in warehouses)
            Warehouses.Add(w);
    }

    [RelayCommand]
    async Task Save()
    {
        if (SelectedProduct == null || SelectedWarehouse == null)
        {
            await Shell.Current.DisplayAlert("Ошибка", "Выберите товар и склад", "OK");
            return;
        }

        if (!int.TryParse(Quantity, out int qty) || qty <= 0)
        {
            await Shell.Current.DisplayAlert("Ошибка", "Введите корректное количество", "OK");
            return;
        }

        var result = await _api.AddStockAsync(
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
            new SelectProductPopup(_api)
        );

        if (result is ProductModel p)
            SelectedProduct = p;
    }



}
