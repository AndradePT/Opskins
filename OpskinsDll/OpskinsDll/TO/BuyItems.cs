using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpskinsDll.TO
{
    public class BuyItems
    {
        public List<Item> items { get; set; }
    }


    public class Item
    {
        public int saleid { get; set; }
        public int new_itemid { get; set; }
        public string name { get; set; }
        public int bot_id { get; set; }
    }
}
