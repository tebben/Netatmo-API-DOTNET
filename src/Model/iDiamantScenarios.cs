using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netatmo.Net.Model
{
    class iDiamantScenarios
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class Scenario
        {
            public string id { get; set; }
            public int target_position { get; set; }
            public string type { get; set; }
            public bool customizable { get; set; }
            public string category { get; set; }
            public bool editable { get; set; }
            public string name { get; set; }
        }

        public class Module
        {
            public string id { get; set; }
            public List<Scenario> scenarios { get; set; }
        }

        public class Home
        {
            public string id { get; set; }
            public List<Module> modules { get; set; }
            public List<Scenario> scenarios { get; set; }
        }

        public class Body
        {
            public Home home { get; set; }
        }

        public class Root
        {
            public string status { get; set; }
            public int time_server { get; set; }
            public Body body { get; set; }
        }

    }
}
