using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace allinone
{
    internal class Roles
    {
        public static List<Role> All { get; set; } = DB.Context.Role.ToList();
    }
}
