using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticCenter.ViewModels
{
    public class UserItemModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsBlocked { get; set; }

        public string Status => IsBlocked ? "Заблокирован" : "Активен";

        public Color StatusColor =>
            IsBlocked ? Colors.Red : Colors.LightGreen;
    }
}

