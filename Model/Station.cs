using System.Collections.Generic;

namespace Netatmo.Net.Model
{
    public class Station
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public List<Module> Modules { get; set; }

        public Station() { }

        public Station(string name, string id)
        {
            Name = name;
            Id = id;
            Modules = new List<Module>();
        }
    }
}
