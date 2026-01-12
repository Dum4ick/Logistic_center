using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Xml.Linq;
using LogisticCenter.Data;

namespace LogisticCenter.ViewModels;

public partial class AddEditProductViewModel : ObservableObject
{
    private readonly ApiData _api;
    private readonly Popup _popup;
    private readonly ProductModel _editing;

    [ObservableProperty] string name;
    [ObservableProperty] string weight;
    [ObservableProperty] string price;

    [ObservableProperty] List<CategoryModel> categories;
    [ObservableProperty] CategoryModel selectedCategory;


    public AddEditProductViewModel(
        Popup popup,
        ApiData api,
        ProductModel product = null)
    {
        _popup = popup;
        _api = api;
        _editing = product;

        LoadCategories();

        if (product != null)
        {
            Name = product.Name;
            Weight = product.Weight;
            Price = product.Price;
        }
    }

    async void LoadCategories()
    {
        Categories = await _api.GetCategories();

        if (_editing != null)
            SelectedCategory = Categories.FirstOrDefault(c => c.Id == _editing.CategoryID);
    }


    [RelayCommand]
    async Task Save()
    {
        if (SelectedCategory == null)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Ошибка", "Выберите категорию", "OK");
            return;
        }

        var p = new ProductModel
        {
            Id = _editing?.Id ?? "0",
            Name = Name,
            CategoryID = SelectedCategory.Id,
            Weight = Weight,
            Price = Price
        };

        (bool success, string message) result =
            _editing == null
                ? await _api.AddProduct(p)
                : await _api.UpdateProduct(p);

        if (!result.success)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Ошибка", result.message, "OK");
            return;
        }

        _popup.Close(true);
    }

    [RelayCommand]
    void Close() => _popup.Close(false);

    public bool IsEdit => _editing != null;
    public string Title => IsEdit ? "Редактирование товара" : "Добавление товара";
    public string SaveText => IsEdit ? "Сохранить" : "Создать";

}
