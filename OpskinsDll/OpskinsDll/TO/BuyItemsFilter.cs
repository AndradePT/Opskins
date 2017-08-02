using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpskinsDll.TO
{
    public class BuyItemsFilter
    {
        public List<int> saleids { get; set; }
        public int total  { get; set; }

        public BuyItemsFilter(List<int> saleids, int total)
        {
            this.total = total;
            this.saleids = saleids;
        }

        public object GetSaleids()
        {
            object Ids = string.Join(",", saleids);
            return Ids;
        }
    }
}
