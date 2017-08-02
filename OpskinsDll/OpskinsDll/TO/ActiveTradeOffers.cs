using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpskinsDll.TO
{
    public class ActiveTradeOffers
    {
        public ActiveTradeOffersReq offers { get; set; }
        public string offersId { get; set; }
    }

    public class ActiveTradeOffersReq
    {
        public List<int> saleids { get; set; }
        public int bot_id { get; set; }
        public string bot_id64 { get; set; }
        public string type { get; set; }
    }
}
