using CommunityToolkit.Maui.Views;
using LogisticCenter.ViewModels;
using LogisticCenter.Data;

namespace LogisticCenter.Popups;

public partial class AddEditWarehousePopup : Popup
{
    public AddEditWarehousePopup(ApiData api, WarehouseModel warehouse = null)
    {
        InitializeComponent();
        BindingContext = new AddEditWarehouseViewModel(this, api, warehouse);
    }
}
