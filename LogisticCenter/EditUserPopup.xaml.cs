using CommunityToolkit.Maui.Views;
using LogisticCenter.ViewModels;

namespace LogisticCenter;

public partial class EditUserPopup : Popup
{
    public EditUserPopup(UserItemModel user, UsersViewModel usersVm)
    {
        InitializeComponent();
        BindingContext = new EditUserPopupViewModel(this, user, usersVm);
    }
}
