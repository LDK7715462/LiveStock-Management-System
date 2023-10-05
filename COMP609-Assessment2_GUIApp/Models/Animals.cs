using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP609_Assessment2_GUIApp.Models
{
    internal abstract class Animals
    {
        public int ID { get; set; }

        public double Water { get; set; }

        public double Cost { get; set; }

        public double Weight { get; set; }

        public string Colour { get; set; }

        public double Wool_Milk { get; set; }

        public Animals(string type, int id, double water, double cost, double weight, string colour, double wool_milk)
        {
            this.ID = id;
            this.Water = water;
            this.Cost = cost;
            this.Weight = weight;
            this.Colour = colour;
            this.Wool_Milk = wool_milk;
        }
    }
}
