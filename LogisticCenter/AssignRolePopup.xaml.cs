using CommunityToolkit.Maui.Views;
using LogisticCenter.ViewModels;

namespace LogisticCenter;

public partial class AssignRolePopup : Popup
{
    public AssignRolePopup(int userId, UsersViewModel usersVm)
    {
        InitializeComponent();
        BindingContext = new AssignRolePopupViewModel(this, userId, usersVm);
    }
}
