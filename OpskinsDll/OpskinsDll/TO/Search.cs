using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpskinsDll.TO
{
    public class Search
    {
        public List<Sale> sales { get; set; }
    }

    public class Sale
    {
        public int id { get; set; }
        public int amount { get; set; }
        public string classid { get; set; }
        public string instanceid { get; set; }
        public string img { get; set; }
        public string market_name { get; set; }
        public string inspect { get; set; }
        public string type { get; set; }
        public string item_id { get; set; }
        public string stickers { get; set; }
        public object wear { get; set; }
        public int appid { get; set; }
        public string contextid { get; set; }
        public int bot_id { get; set; }
    }

}
