using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpskinsDll.TO
{
    public class ListItemsFilter
    {
        public List<ItemsFilter> items { get; set; }
    }

    public class ItemsFilter
    {
        public string appid { get; set; }
        public string contextid { get; set; }
        public string assetid { get; set; }
        public int price { get; set; }
        public List<object> addons { get; set; }
    }

}
