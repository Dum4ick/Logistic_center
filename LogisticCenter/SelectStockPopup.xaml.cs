using CommunityToolkit.Maui.Views;
using LogisticCenter.Data;
using LogisticCenter.ViewModels;

namespace LogisticCenter;


    public partial class SelectStockPopup : Popup
    {
        public SelectStockPopup(ApiData api, int warehouseId)
        {
            InitializeComponent();
            BindingContext = new SelectStockPopupViewModel(this, api, warehouseId);
        }
    }
