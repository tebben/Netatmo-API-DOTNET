using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netatmo.Net.Model
{
    public class iDiamantSetShutter
    {
        public class Module : Root
        {
            public string id { get; set; }
            public int target_position { get; set; }
            public string bridge { get; set; }
        }

        public class Home : Root
        {
            public string id { get; set; }
            public List<Module> modules { get; set; }
        }

        public class Root
        {
            public Home home { get; set; }
        }
    }
}
