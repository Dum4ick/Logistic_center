using CommunityToolkit.Maui.Views;
using LogisticCenter.ViewModels;
using LogisticCenter.Data;

namespace LogisticCenter;

public partial class WriteOffStockPopup : Popup
{
    public WriteOffStockPopup(ApiData api)
    {
        InitializeComponent();
        BindingContext = new WriteOffStockViewModel(this, api);
    }
}
