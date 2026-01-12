using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using LogisticCenter.Data;
using CommunityToolkit.Mvvm.Messaging;

namespace LogisticCenter.ViewModels;
public partial class CreateOrderViewModel : ObservableObject
{
    private readonly ApiData _api = new();

    public ObservableCollection<CartItemModel> Cart { get; } = new();
    public ObservableCollection<WarehouseModel> Warehouses { get; } = new();

    [ObservableProperty]
    WarehouseModel selectedWarehouse;

    public CreateOrderViewModel()
    {
        LoadCommand.Execute(null);
    }

    [RelayCommand]
    async Task Load()
    {
        Warehouses.Clear();
        var data = await _api.GetWarehousesAsync();
        foreach (var w in data)
            Warehouses.Add(w);
    }

    [RelayCommand]
    async Task AddProduct()
    {
        if (SelectedWarehouse == null)
        {
            await Shell.Current.DisplayAlert("Ошибка", "Выберите склад", "OK");
            return;
        }

        var stock = await Shell.Current.ShowPopupAsync(
            new SelectStockPopup(
                _api,
                Convert.ToInt32(SelectedWarehouse.Id)
            )
        ) as StockModel;

        if (stock == null)
            return;

        var exists = Cart.FirstOrDefault(x =>
            x.ProductId == Convert.ToInt32(stock.ProductId));

        if (exists != null)
        {
            if (exists.Quantity < exists.MaxQuantity)
                exists.Quantity++;

            return;
        }

        Cart.Add(new CartItemModel
        {
            ProductId = Convert.ToInt32(stock.ProductId),
            ProductName = stock.ProductName,
            Quantity = 1,
            MaxQuantity = Convert.ToInt32(stock.Quantity),
            Price = stock.Price
        });
    }



    [RelayCommand]
    async Task CreateOrder()
    {
        if (SelectedWarehouse == null || Cart.Count == 0)
        {
            await Shell.Current.DisplayAlert(
                "Ошибка",
                "Выберите склад и добавьте товары",
                "OK");
            return;
        }

        var order = await _api.CreateOrderAsync(
            Convert.ToInt32(Preferences.Get("user_id", null)),
            Convert.ToInt32(SelectedWarehouse.Id));

        if (!order.success)
        {
            await Shell.Current.DisplayAlert("Ошибка", order.message, "OK");
            return;
        }

        foreach (var item in Cart)
        {
            var addItem = await _api.AddOrderItemAsync(
                order.orderId,
                item.ProductId,
                item.Quantity,
                item.Price);

            if (!addItem.success)
            {
                await Shell.Current.DisplayAlert(
                    "Ошибка",
                    $"Товар {item.ProductName} не добавлен",
                    "OK");
                return;
            }
        }


        await Shell.Current.DisplayAlert("Успех", "Заказ успешно создан", "OK");

        await Shell.Current.GoToAsync("//orders");


        if (Shell.Current.CurrentPage.BindingContext is OrdersViewModel ordersVM)
        {
            ordersVM.LoadCommand.Execute(null);
        }
    }

    [RelayCommand]
    async Task GoBack()
    {
        await Shell.Current.GoToAsync("//orders");
    }

}
