using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LogisticCenter.Data;
using System.Collections.ObjectModel;

namespace LogisticCenter.ViewModels;

public record OrderDetailsViewModelChangedMessage(string Reason);

[QueryProperty(nameof(OrderId), "orderId")]
public partial class OrderDetailsViewModel : ObservableObject
{
    public RoleService Roles => RoleService.Instance;

    private readonly ApiData _api = new();

    [ObservableProperty]
    int currentStatusId;

    [ObservableProperty]
    decimal orderTotal;

    [ObservableProperty]
    string statusName;   

    public ObservableCollection<OrderItemModel> Items { get; } = new();

    private bool _isLoading;

    public bool CanConfirm => (CurrentStatusId == 1) && Roles.Level2;
    public bool CanCancel => CurrentStatusId is 1 or 2;
    public bool CanDelete => (CurrentStatusId is 1 or 5) && Roles.Level2;
    public bool IsCompleted => CurrentStatusId == 4;

    public OrderDetailsViewModel()
    {
        Roles.PropertyChanged += (_, __) =>
        {
            RefreshButtons();
        };

        WeakReferenceMessenger.Default.Register<OrderDetailsViewModelChangedMessage>(
            this,
            async (r, m) =>
            {
                await Load();
                RefreshButtons();
            });
    }

    private void RefreshButtons()
    {
        OnPropertyChanged(nameof(CanConfirm));
        OnPropertyChanged(nameof(CanCancel));
        OnPropertyChanged(nameof(CanDelete));
        OnPropertyChanged(nameof(IsCompleted));
    }


    partial void OnCurrentStatusIdChanged(int value)
    {
        OnPropertyChanged(nameof(CanConfirm));
        OnPropertyChanged(nameof(CanCancel));
        OnPropertyChanged(nameof(CanDelete));
        OnPropertyChanged(nameof(IsCompleted));
    }


    private int orderId;
    public int OrderId
    {
        get => orderId;
        set
        {
            orderId = value;
            LoadCommand.Execute(null);
        }
    }


    [RelayCommand]
    async Task Load()
    {
        if (OrderId <= 0) return;

        _isLoading = true;

        Items.Clear();
        OrderTotal = 0;

        // Загружаем товары
        var items = await _api.GetOrderItemsAsync(OrderId);

        // Загружаем статус заказа
        var status = await _api.GetOrderStatusAsync(OrderId);

        if (status != null)
        {
            CurrentStatusId = Convert.ToInt32(status.Id);
            StatusName = status.Name; 
        }

        foreach (var item in items)
        {
            Items.Add(item);
            OrderTotal += item.Total;
        }

        _isLoading = false;
    }

    [RelayCommand]
    async Task ConfirmOrder()
    {
        var result = await _api.ConfirmOrderAsync(OrderId);

        //if (!result.success)
        //{
        //    await Shell.Current.DisplayAlert("Ошибка", result.message, "OK");
        //    return;
        //}

        // Заказ автоматически становится статус 2
        CurrentStatusId = 2;

        WeakReferenceMessenger.Default.Send(new OrdersChangedMessage("StatusChanged"));
        WeakReferenceMessenger.Default.Send(new ShipmentsChangedMessage("StatusChanged"));

        await Load();
    }

    [RelayCommand]
    async Task CancelOrder()
    {
        bool confirm = await Shell.Current.DisplayAlert(
            "Отмена заказа",
            "Вы уверены, что хотите отменить заказ?",
            "Да", "Нет");

        if (!confirm) return;

        var result = await _api.CancelOrderAsync(OrderId);

        await Shell.Current.DisplayAlert(
            result.success ? "Готово" : "Ошибка",
            result.message,
            "OK");

        if (result.success)
        {
            CurrentStatusId = 5;

            WeakReferenceMessenger.Default.Send(new OrdersChangedMessage("StatusChanged"));
            WeakReferenceMessenger.Default.Send(new ShipmentsChangedMessage("StatusChanged"));

            await Load();
        }
    }

    [RelayCommand]
    async Task DeleteOrder()
    {
        bool confirm = await Shell.Current.DisplayAlert(
            "Удаление заказа",
            "Удалить заказ без возможности восстановления?",
            "Да", "Нет");

        if (!confirm) return;

        var result = await _api.DeleteOrderAsync(OrderId);

        await Shell.Current.DisplayAlert(
            result.success ? "Готово" : "Ошибка",
            result.message,
            "OK");

        if (result.success)
        {
            WeakReferenceMessenger.Default.Send(new OrdersChangedMessage("Deleted"));
            WeakReferenceMessenger.Default.Send(new ShipmentsChangedMessage("Deleted"));

            await Shell.Current.GoToAsync("//orders");
        }
    }

    [RelayCommand]
    async Task GoBack()
    {
        await Shell.Current.GoToAsync("//orders");
    }
}
