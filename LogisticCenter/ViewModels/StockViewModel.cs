using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LogisticCenter.Data;
using System.Collections.ObjectModel;

namespace LogisticCenter.ViewModels;

public partial class StockViewModel : ObservableObject
{
    private readonly ApiData _api = new();

    public ObservableCollection<StockModel> Stocks { get; } = new();

    private readonly List<StockModel> _allStocks = new();

    [ObservableProperty]
    StockModel selectedStock;

    [ObservableProperty]
    string searchText;

    public StockViewModel()
    {
        WeakReferenceMessenger.Default.Register<StockChangedMessage>(
            this,
            async (r, m) =>
            {
               await Load();
            });
        LoadCommand.Execute(null);
    }

    // Загрузка остатков
    [RelayCommand]
    private async Task Load()
    {
        Stocks.Clear();
        _allStocks.Clear();

        var data = await _api.GetStockAsync();

        foreach (var item in data)
        {
            _allStocks.Add(item);
            Stocks.Add(item);
        }
    }

    // Поиск (как у товаров)
    [RelayCommand]
    private void Search()
    {
        if (_allStocks.Count == 0)
            return;

        Stocks.Clear();

        string text = SearchText?.ToLower() ?? "";

        var filtered = _allStocks.Where(s =>
            string.IsNullOrWhiteSpace(text)
            || s.ProductName?.ToLower().Contains(text) == true
            || s.WarehouseName?.ToLower().Contains(text) == true
        );

        foreach (var s in filtered)
            Stocks.Add(s);
    }

    // Автопоиск при вводе
    partial void OnSearchTextChanged(string value)
    {
        SearchCommand.Execute(null);
    }

    [RelayCommand]
    async Task GoBack()
    {
        await Shell.Current.GoToAsync("//main");
    }

    [RelayCommand]
    async Task Add()
    {
        var result = await Shell.Current.ShowPopupAsync(
            new AddStockPopup(_api));

        if (result is true)
            LoadCommand.Execute(null);
    }

    [RelayCommand]
    async Task WriteOff()
    {
        var result = await Shell.Current.ShowPopupAsync(
            new WriteOffStockPopup(_api));

        if (result is true)
            LoadCommand.Execute(null);
    }
}
