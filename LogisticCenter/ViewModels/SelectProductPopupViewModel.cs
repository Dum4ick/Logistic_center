using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using LogisticCenter.Data;

namespace LogisticCenter.ViewModels;

public partial class SelectProductPopupViewModel : ObservableObject
{
    private readonly Popup _popup;
    private readonly ApiData _api;

    public ObservableCollection<ProductModel> Products { get; } = new();

    [ObservableProperty]
    ProductModel selectedProduct;

    [ObservableProperty]
    string searchText;

    public SelectProductPopupViewModel(Popup popup, ApiData api)
    {
        _popup = popup;
        _api = api;
        LoadCommand.Execute(null);
    }

    [RelayCommand]
    async Task Load()
    {
        Products.Clear();

        var list = await _api.GetProducts();
        foreach (var p in list)
            Products.Add(p);
    }

    partial void OnSearchTextChanged(string value)
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            LoadCommand.Execute(null);
            return;
        }

        var text = SearchText.ToLower();

        var filtered = Products
            .Where(x => x.Name?.ToLower().Contains(text) == true)
            .ToList();

        Products.Clear();
        foreach (var item in filtered)
            Products.Add(item);
    }

    [RelayCommand]
    void Select()
    {
        if (SelectedProduct != null)
            _popup.Close(SelectedProduct);
    }

    [RelayCommand]
    void Close() => _popup.Close(null);
}
