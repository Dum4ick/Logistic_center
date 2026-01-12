using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using LogisticCenter.Services;

namespace LogisticCenter
{
    public record RoleChangedMessage(string Reason);
    public partial class RoleService : ObservableObject
    {
        private static RoleService _instance;
        public static RoleService Instance => _instance ??= new RoleService();

        private RoleService()
        {
            WeakReferenceMessenger.Default.Register<RoleService>(
            this,
            async (r, m) =>
            {
                Refresh();
            });

            UserSession.Instance.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(UserSession.RoleId))
                    Refresh();
            };

            Refresh();
        }

        [ObservableProperty]
        private bool isAdmin;

        [ObservableProperty]
        private bool isManager;

        [ObservableProperty]
        private bool isUser;


        [ObservableProperty]
        private bool level1;

        [ObservableProperty]
        private bool level2;

        [ObservableProperty]
        private bool level3;


        public void Refresh()
        {
            string role = UserSession.Instance.RoleId;

            IsAdmin = role == "3";
            IsManager = role == "2";
            IsUser = role == "1";

            Level3 = IsAdmin;
            Level2 = IsAdmin || IsManager;
            Level1 = IsAdmin || IsManager || IsUser;
        }
    }
}


