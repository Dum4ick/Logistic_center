using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogisticCenter.Data;

namespace LogisticCenter.ViewModels
{
    public partial class SelectStockPopupViewModel : ObservableObject
    {
        private readonly Popup _popup;
        private readonly ApiData _api;
        private readonly int _warehouseId;

        public ObservableCollection<StockModel> Products { get; } = new();

        [ObservableProperty]
        StockModel selectedProduct;

        [ObservableProperty]
        string searchText;

        public SelectStockPopupViewModel(
            Popup popup,
            ApiData api,
            int warehouseId)
        {
            _popup = popup;
            _api = api;
            _warehouseId = warehouseId;

            LoadCommand.Execute(null);
        }

        [RelayCommand]
        async Task Load()
        {
            Products.Clear();

            var stock = await _api.GetStockWarehouseAsync(_warehouseId);

            foreach (var item in stock.Where(x => Convert.ToInt32(x.Quantity) > 0))
                Products.Add(item);
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
                .Where(x => x.ProductName?.ToLower().Contains(text) == true)
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

}
