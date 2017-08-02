using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpskinsDll.TO
{
    public class Deposit
    {
        public string tradeoffer_id { get; set; }
        public object tradeoffer_error { get; set; }
        public int bot_id { get; set; }
        public string bot_id64 { get; set; }
        public string security_token { get; set; }
        public List<SaleReq> sales { get; set; }
    }

    public class SaleReq
    {
        public int saleid { get; set; }
        public int appid { get; set; }
        public string contextid { get; set; }
        public string assetid { get; set; }
        public string market_name { get; set; }
        public List<object> addons { get; set; }
    }
}
