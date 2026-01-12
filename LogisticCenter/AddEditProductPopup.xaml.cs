using CommunityToolkit.Maui.Views;
using LogisticCenter.ViewModels;
using LogisticCenter.Data;

namespace LogisticCenter;

public partial class AddEditProductPopup : Popup
{
    public AddEditProductPopup(ApiData api, ProductModel product = null)
    {
        InitializeComponent();
        BindingContext = new AddEditProductViewModel(this, api, product);
    }
}
