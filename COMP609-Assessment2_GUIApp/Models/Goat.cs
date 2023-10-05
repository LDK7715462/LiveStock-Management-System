using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP609_Assessment2_GUIApp.Models
{
    internal class Goat : Animals
    {
        public Goat(string type, int id, double water, int cost, int weight, string colour, double milk) : base(type, id, water, cost, weight, colour, milk)
        {
        }
        public override string ToString()
        {
            return $"{this.GetType().Name}-{ID}-{Water}-{Cost}-{Weight}-{Colour}-{Wool_Milk}";
        }
    }
}
