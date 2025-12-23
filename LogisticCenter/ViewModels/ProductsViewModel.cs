using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogisticCenter.ViewModels;
using LogisticCenter.Data;

namespace LogisticCenter.ViewModels;
public partial class ProductsViewModel : ObservableObject
{
    private readonly ApiData _api = new();

    [ObservableProperty] List<ProductModel> products;
    [ObservableProperty] ProductModel selectedProduct;
    [ObservableProperty] string searchText;

    public ProductsViewModel() => Load();

    async void Load()
        => Products = await _api.GetProducts();

    [RelayCommand]
    async Task Search()
        => Products = await _api.GetProducts(SearchText);

    [RelayCommand]
    async Task Add()
    {
        await Shell.Current.DisplayAlert("Добавить", "Окно добавления", "OK");
    }

    [RelayCommand]
    async Task Edit()
    {
        if (SelectedProduct == null) return;
        await _api.UpdateProduct(SelectedProduct);
        Load();
    }

    [RelayCommand]
    async Task Delete()
    {
        if (SelectedProduct == null) return;
        await _api.DeleteProduct(Convert.ToInt32(SelectedProduct.Id));
        Load();
    }

    [RelayCommand]
    async Task GoBack()
    {
        await Shell.Current.GoToAsync("//main");
    }
}


