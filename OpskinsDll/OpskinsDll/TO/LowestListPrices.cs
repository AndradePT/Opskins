using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpskinsDll.TO
{
    public class LowestListPrices
    {
        public string Name { get; set; }
        public ListPricesDescription Description { get; set; }
    }

    public class ListPricesDescription
    {
        public int price { get; set; }
        public int quantity { get; set; }
    }
}
