using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpskinsDll.TO
{


    public class ListItems
    {
        public string tradeoffer_id { get; set; }
        public string tradeoffer_error { get; set; }
        public string bot_id { get; set; }
        public string bot_id64 { get; set; }
        public string security_token { get; set; }
        public List<SalesItem> sales { get; set; }
    }

    public class SalesItem
    {
        public string saleid { get; set; }
        public string appid { get; set; }
        public string contextid { get; set; }
        public string assetid { get; set; }
        public string market_name { get; set; }
        public int price { get; set; }
        public List<object> addons { get; set; }
    }
}
