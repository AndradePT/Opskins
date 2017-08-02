using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpskinsDll.TO
{
    public class Sales
    {
        public int id { get; set; }
        public int price { get; set; }
        public int? commission { get; set; }
        public int? tax { get; set; }
        public string classid { get; set; }
        public string instanceid { get; set; }
        public string img { get; set; }
        public int appid { get; set; }
        public string contextid { get; set; }
        public string assetid { get; set; }
        public string name { get; set; }
        public int bot { get; set; }
        public string offerid { get; set; }
        public int state { get; set; }
        public int escrow_end_date { get; set; }
        public int list_time { get; set; }
        public int bump_time { get; set; }
        public int last_updated { get; set; }
        public int? sale_time { get; set; }
        public string security_token { get; set; }
        public double? wear { get; set; }
        public int? txid { get; set; }
        public bool trade_locked { get; set; }
        public bool repair_attempted { get; set; }
        public List<object> addons { get; set; }
        public string sk { get; set; }
        public string bot_id64 { get; set; }
    }
}
