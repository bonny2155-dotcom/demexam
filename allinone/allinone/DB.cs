using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace allinone
{
    internal class DB
    {
        public static DEM_usersEntities Context { get; set; } = new DEM_usersEntities();
    }
}
