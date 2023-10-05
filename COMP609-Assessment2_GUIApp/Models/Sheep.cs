using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP609_Assessment2_GUIApp.Models
{
    internal class Sheep : Animals
    {
        public Sheep(string type, int id, double water, double cost, double weight, string colour, double wool) : base(type, id, water, cost, weight, colour, wool)
        {
        }
        public override string ToString()
        {
            return $"{this.GetType().Name}-{ID}-{Water}-{Cost}-{Weight}-{Colour}-{Wool_Milk}";
        }
    }
}
