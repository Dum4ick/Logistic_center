using CommunityToolkit.Maui.Views;
using LogisticCenter.ViewModels;
using LogisticCenter.Data;

namespace LogisticCenter;

public partial class AddStockPopup : Popup
{
    public AddStockPopup(ApiData api)
    {
        InitializeComponent();
        BindingContext = new AddStockViewModel(this, api);
    }
}
