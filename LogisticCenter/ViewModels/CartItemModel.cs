using CommunityToolkit.Mvvm.ComponentModel;

namespace LogisticCenter.ViewModels;

public partial class CartItemModel : ObservableObject
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }

    [ObservableProperty]
    private int quantity;

    [ObservableProperty]
    private decimal price;

    public int MaxQuantity { get; set; }

    public decimal Total => Quantity * price;

    partial void OnQuantityChanged(int value)
    {
        if (Quantity > MaxQuantity)
            Quantity = MaxQuantity;

        OnPropertyChanged(nameof(Total));
    }

    partial void OnPriceChanged(decimal value)
    {
        OnPropertyChanged(nameof(Total));
    }
}

