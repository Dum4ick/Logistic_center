using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LogisticCenter.Data;
using LogisticCenter.Popups;
using System.Collections.ObjectModel;

namespace LogisticCenter.ViewModels;

public partial class WarehousesViewModel : ObservableObject
{
    public RoleService Roles => RoleService.Instance;

    private readonly ApiData _api = new();

    private List<WarehouseModel> _allWarehouses = new();

    public ObservableCollection<WarehouseModel> Warehouses { get; } = new();

    [ObservableProperty]
    WarehouseModel selectedWarehouse;

    [ObservableProperty]
    string searchText;

    public WarehousesViewModel()
    {
        LoadCommand.Execute(null);
    }

    [RelayCommand]
    async Task Load()
    {
        Warehouses.Clear();
        _allWarehouses.Clear();

        var data = await _api.GetWarehousesAsync();

        foreach (var w in data)
        {
            _allWarehouses.Add(w);
            Warehouses.Add(w);
        }
    }

    // SEARCH
    [RelayCommand]
    void Search()
    {
        Warehouses.Clear();

        string text = SearchText?.ToLower() ?? "";

        var filtered = _allWarehouses.Where(w =>
            string.IsNullOrWhiteSpace(text)
            || w.Name.ToLower().Contains(text)
            || w.Address.ToLower().Contains(text)
        );

        foreach (var w in filtered)
            Warehouses.Add(w);
    }

    partial void OnSearchTextChanged(string value)
        => SearchCommand.Execute(null);

    [RelayCommand]
    async Task AddWarehouse()
    {
        var result = await Shell.Current.ShowPopupAsync(new AddEditWarehousePopup(_api));

        if (result is true)
            await Load();
    }

    [RelayCommand]
    async Task EditWarehouse()
    {
        if (SelectedWarehouse == null)
            return;

        var result = await Shell.Current.ShowPopupAsync(
            new AddEditWarehousePopup(_api, SelectedWarehouse));

        if (result is true)
            await Load();
    }

    [RelayCommand]
    async Task DeleteWarehouse()
    {
        if (SelectedWarehouse == null)
            return;

        bool confirm = await Shell.Current.DisplayAlert(
            "Удаление склада",
            $"Удалить склад '{SelectedWarehouse.Name}'?",
            "Да", "Нет");

        if (!confirm) return;

        var result = await _api.DeleteWarehouseAsync(int.Parse(SelectedWarehouse.Id));

        if (!result.success)
        {
            await Shell.Current.DisplayAlert("Ошибка", result.message, "OK");
            return;
        }

        await Load();
    }

    [RelayCommand]
    async Task GoBack()
    {
        await Shell.Current.GoToAsync("//main");
    }
}
