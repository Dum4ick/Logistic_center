using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace LogisticCenter.ViewModels;

public partial class StockViewModel : ObservableObject
{
    public ObservableCollection<StockItemModel> Items { get; } = new();

    public StockViewModel()
    {
        Items.Add(new StockItemModel
        {
            ProductName = "Монитор Samsung",
            Warehouse = "Центральный склад",
            Quantity = 42
        });

        Items.Add(new StockItemModel
        {
            ProductName = "Клавиатура Logitech",
            Warehouse = "Склад №2",
            Quantity = 15
        });
    }

    [RelayCommand]
    async Task GoBack()
    {
        await Shell.Current.GoToAsync("//main");
    }
}


public class StockItemModel
{
    public string ProductName { get; set; }
    public string Warehouse { get; set; }
    public int Quantity { get; set; }
}
