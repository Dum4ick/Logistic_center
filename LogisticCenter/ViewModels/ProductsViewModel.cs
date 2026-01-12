using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LogisticCenter.Data;
using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;

namespace LogisticCenter.ViewModels;

public partial class ProductsViewModel : ObservableObject
{
    public RoleService Roles => RoleService.Instance;

    private readonly ApiData _api = new();

    public ObservableCollection<ProductModel> Products { get; } = new();
    public ObservableCollection<string> Categories { get; } = new();

    [ObservableProperty]
    ProductModel selectedProduct;

    [ObservableProperty]
    string searchText;

    [ObservableProperty]
    string selectedCategory;


    private readonly List<ProductModel> _allProducts = new();

    private const string AllCategory = "Все";

    public ProductsViewModel()
    {
        LoadProductsCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadProducts()
    {
        Products.Clear();
        Categories.Clear();
        _allProducts.Clear();

        var products = await _api.GetProducts();

        foreach (var p in products)
        {
            _allProducts.Add(p);
            Products.Add(p);
        }

        Categories.Add(AllCategory);

        foreach (var category in _allProducts
            .Select(p => p.Category)
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct()
            .OrderBy(c => c))
        {
            Categories.Add(category);
        }

        SelectedCategory = AllCategory;
    }

    [RelayCommand]
    private void Search()
    {
        if (_allProducts.Count == 0)
            return;

        Products.Clear();

        string text = SearchText?.ToLower() ?? "";
        string category = SelectedCategory;

        var filtered = _allProducts.Where(p =>
            (
                string.IsNullOrWhiteSpace(text)
                || p.Name?.ToLower().Contains(text) == true
                || p.Category?.ToLower().Contains(text) == true
            )
            &&
            (
                category == AllCategory
                || p.Category == category
            )
        );

        foreach (var p in filtered)
            Products.Add(p);
    }

    partial void OnSearchTextChanged(string value)
    {
        SearchCommand.Execute(null);
    }

    partial void OnSelectedCategoryChanged(string value)
    {
        SearchCommand.Execute(null);
    }

    [RelayCommand]
    async Task Add()
    {
        var result = await Shell.Current.ShowPopupAsync(
            new AddEditProductPopup(_api));

        if (result is true)
            LoadProductsCommand.Execute(null);
    }

    [RelayCommand]
    async Task Edit()
    {
        if (SelectedProduct == null)
            return;

        var result = await Shell.Current.ShowPopupAsync(
            new AddEditProductPopup(_api, SelectedProduct));

        if (result is true)
            LoadProductsCommand.Execute(null);
    }

    [RelayCommand]
    async Task Delete()
    {
        if (SelectedProduct == null)
            return;

        bool confirm = await Shell.Current.DisplayAlert(
            "Удаление товара",
            "Вы точно хотите удалить данный товар?",
            "Да",
            "Отмена");

        if (!confirm)
            return;

        await _api.DeleteProduct(Convert.ToInt32(SelectedProduct.Id));
        LoadProductsCommand.Execute(null);
    }

    [RelayCommand]
    async Task GoBack()
    {
        await Shell.Current.GoToAsync("//main");
    }
}
