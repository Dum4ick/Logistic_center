using CommunityToolkit.Maui.Views;
using LogisticCenter.Data;
using LogisticCenter.ViewModels;

namespace LogisticCenter;

public partial class SelectProductPopup : Popup
{
    public SelectProductPopup(ApiData api)
    {
        InitializeComponent();
        BindingContext = new SelectProductPopupViewModel(this, api);
    }
}


