using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpskinsDll.TO
{
    public class OpSkinsInventory
    {
        public List<Limits> limits { get; set; }
        public List<ItemIventory> items { get; set; }

    }

    public class ItemIventory
    {
        public int id { get; set; }
        public int id_parent { get; set; }
        public string name { get; set; }
        public string inspect { get; set; }
        public string type { get; set; }
        public int appid { get; set; }
        public string contextid { get; set; }
        public string assetid { get; set; }
        public string classid { get; set; }
        public string instanceid { get; set; }
        public string img { get; set; }
        public double wear { get; set; }
        public int bot_id { get; set; }
        public string bot_id64 { get; set; }
        public int added_time { get; set; }
        public object offer_id { get; set; }
        public bool offer_declined { get; set; }
        public bool offer_untradable { get; set; }
        public bool requires_support { get; set; }
        public object can_repair { get; set; }
    }

    public class Limits
    {
        public string app { get; set; }
        public string free_slots { get; set; }
    }

}
