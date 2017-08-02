using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpskinsDll.TO
{
    public class SalesFilter
    {
        public SalesType Type { get; set; } = SalesType.On_sale;
        public string appid { get; set; } = "730";
        public string after_saleid { get; set; }
        public int page { get; set; } = 1;
        public int per_page { get; set; } = 10000;
        public SalesSort Sort { get; set; } = SalesSort.last_sold;

    }


    public enum SalesType
    {
        Awaiting_pickup=1,
        On_sale=2,
        Sold=3,
        Sold_and_delivered=4,
        Returned=5,
        Requested_returned=6
    }

    public enum SalesSort
    {
        oldest_bump,
        last_sold,
        price_desc,
        price_asc,
        alpha,
        bot,
        activity_old,
        activity_new,
    }
}
