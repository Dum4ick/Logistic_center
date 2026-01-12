using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LogisticCenter.Data;


namespace LogisticCenter.ViewModels;

public partial class AddEditWarehouseViewModel : ObservableObject
{
    private readonly ApiData _api;
    private readonly Popup _popup;
    private readonly WarehouseModel _editing;

    [ObservableProperty] string name;
    [ObservableProperty] string address;

    public AddEditWarehouseViewModel(Popup popup, ApiData api, WarehouseModel warehouse = null)
    {
        _popup = popup;
        _api = api;
        _editing = warehouse;

        if (warehouse != null)
        {
            Name = warehouse.Name;
            Address = warehouse.Address;
        }
    }

    [RelayCommand]
    async Task Save()
    {
        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Address))
        {
            await Application.Current.MainPage.DisplayAlert(
                "Ошибка", "Введите название и адрес склада", "OK");
            return;
        }

        var w = new WarehouseModel
        {
            Id = _editing?.Id ?? "0",
            Name = Name,
            Address = Address
        };

        (bool success, string message) result =
            _editing == null
                ? await _api.AddWarehouseAsync(w)
                : await _api.UpdateWarehouseAsync(w);

        if (!result.success)
        {
            await Application.Current.MainPage.DisplayAlert("Ошибка", result.message, "OK");
            return;
        }

        _popup.Close(true);
    }

    [RelayCommand]
    void Close() => _popup.Close(false);

    public bool IsEdit => _editing != null;
    public string Title => IsEdit ? "Редактирование склада" : "Добавление склада";
    public string SaveText => IsEdit ? "Сохранить" : "Создать";
}
