using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static COMP609_Assessment2_GUIApp.Models.LMSApp;

namespace COMP609_Assessment2_GUIApp.Models
{
    internal class Commodity : LiveStockManagement
    {
        public string Item { get; set; }
        public double Price { get; set; }
        public Commodity(string type, string item, double price) : base(type)
        {
            this.Item = item;
            this.Price = price;
        }
        public override string ToString()
        {
            return string.Format("{0,-20} {1,27:C}", // Adjust the widths as needed
                this.Item,
                this.Price
            );
        }
    }
}
