using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LogisticCenter.Data;
using LogisticCenter.Services;
using System.Collections.ObjectModel;

namespace LogisticCenter.ViewModels;

public partial class OrdersViewModel : ObservableObject
{

    private bool _isLoading;

    public RoleService Roles => RoleService.Instance;
    public UserSession Session => UserSession.Instance;

    private readonly ApiData _api = new();

    private readonly List<OrderModel> _allOrders = new();

    public ObservableCollection<OrderModel> Orders { get; } = new();
    public ObservableCollection<OrderStatusModel> Statuses { get; } = new();


    [ObservableProperty]
    private string searchText;

    [ObservableProperty]
    private OrderModel selectedOrder;

    [ObservableProperty]
    private OrderStatusModel selectedStatus;


    public OrdersViewModel()
    {
        //Обновление при изменении заказов
        WeakReferenceMessenger.Default.Register<OrdersChangedMessage>(
            this,
            async (r, m) => await Load());

        //Обновление при смене роли
        WeakReferenceMessenger.Default.Register<RoleChangedMessage>(
            this,
            async (r, m) => await Load());

        //Обновление при смене пользователя
        Session.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(UserSession.Id))
                LoadCommand.Execute(null);
        };

        LoadCommand.Execute(null);
    }


    [RelayCommand]
    private async Task Load()
    {
        if (_isLoading)
            return;

        try
        {
            _isLoading = true;

            Orders.Clear();
            _allOrders.Clear();

            var orders = await _api.GetOrdersAsync();
            var statuses = await _api.GetOrderStatusesAsync();

            Statuses.Clear();
            Statuses.Add(new OrderStatusModel { Id = "0", Name = "Все статусы" });

            foreach (var s in statuses)
                Statuses.Add(s);

            SelectedStatus = Statuses.FirstOrDefault();

            IEnumerable<OrderModel> filteredOrders = orders;

            if (Roles.Level1 && !Roles.Level2 && Session.IsLoggedIn)
            {
                filteredOrders = orders.Where(o => o.UserId == Session.Id);
            }

            foreach (var o in filteredOrders)
            {
                _allOrders.Add(o);
                Orders.Add(o);
            }
        }
        finally
        {
            _isLoading = false;
        }
    }



    [RelayCommand]
    private void Search()
    {
        Orders.Clear();

        string text = SearchText?.ToLower() ?? "";
        string statusId = SelectedStatus?.Id;

        var filtered = _allOrders.Where(o =>
            (
                string.IsNullOrWhiteSpace(text)
                || o.Id.Contains(text)
                || o.UserName?.ToLower().Contains(text) == true
            )
            &&
            (
                statusId == "0"
                || o.StatusId == statusId
            )
        );

        foreach (var o in filtered)
            Orders.Add(o);
    }

    partial void OnSearchTextChanged(string value)
        => SearchCommand.Execute(null);

    partial void OnSelectedStatusChanged(OrderStatusModel value)
        => SearchCommand.Execute(null);

    [RelayCommand]
    private async Task OpenOrder(OrderModel order)
    {
        if (order == null)
            return;

        await Shell.Current.GoToAsync(
            "//orderdetails",
            new Dictionary<string, object>
            {
                ["orderId"] = int.Parse(order.Id)
            });

        SelectedOrder = null;
    }

    [RelayCommand]
    private async Task CreateOrder()
    {
        await Shell.Current.GoToAsync("//createorder");
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("//main");
    }
}
