using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace LogisticCenter.ViewModels;

public partial class OrdersViewModel : ObservableObject
{
    public ObservableCollection<OrderItemModel> Orders { get; } = new();

    public OrdersViewModel()
    {
        Orders.Add(new OrderItemModel
        {
            OrderNumber = "№1001",
            ClientName = "ООО Ромашка",
            Status = "В обработке"
        });

        Orders.Add(new OrderItemModel
        {
            OrderNumber = "№1002",
            ClientName = "ИП Иванов",
            Status = "Завершён"
        });
    }

    [RelayCommand]
    async Task GoBack()
    {
        await Shell.Current.GoToAsync("//main");
    }
}

public class OrderItemModel
{
    public string OrderNumber { get; set; }
    public string ClientName { get; set; }
    public string Status { get; set; }
}
