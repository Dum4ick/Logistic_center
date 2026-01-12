using CommunityToolkit.Maui.Views;
using LogisticCenter.ViewModels;

namespace LogisticCenter;

public partial class AddUserPopup : Popup
{
    public AddUserPopup(UsersViewModel users)
    {
        InitializeComponent();
        BindingContext = new AddUserPopupViewModel(this, users);
    }
}
