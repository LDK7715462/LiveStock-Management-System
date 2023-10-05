using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP609_Assessment2_GUIApp.Models
{
    internal abstract class LiveStockManagement
    {
        public string Type { get; set; }

        public LiveStockManagement(string type)
        {
            this.Type = type;
        }
    }
}
