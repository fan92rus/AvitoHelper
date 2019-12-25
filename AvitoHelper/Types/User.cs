using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvitoHelper.Types
{
    public class User : BaseType
    {
        public DateTimeOffset LastEmailMessageTime { get; set; }
        public string email { get; set; }
        public string AccessKey { get; set; }
        public string PasswordHash { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public int Limit { get; set; }
        public DateTimeOffset DayEndSubscribe { get; set; }
        public int Deep { get; set; } = 2;
    }
}
