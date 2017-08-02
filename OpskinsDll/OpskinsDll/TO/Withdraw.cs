using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpskinsDll.TO
{
    public class Withdraw
    {
        public List<Offer> offers { get; set; }
    }

    public class Offer
    {
        public int bot_id { get; set; }
        public string tradeoffer_id { get; set; }
        public object tradeoffer_error { get; set; }
        public List<int> items { get; set; }
        public string bot_id64 { get; set; }
    }
}
