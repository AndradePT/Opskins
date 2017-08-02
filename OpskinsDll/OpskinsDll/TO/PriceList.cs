using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpskinsDll.TO
{
    public class PriceList
    {
        public string Name { get; set; }
        public List<DayDescription> days { get; set; }

    }

    public class DayDescription
    {
        public string date { get; set; }
        public ItemDescription description { get; set; }
    }



    public class ItemDescription
    {
        public int mean { get; set; }
        public int min { get; set; }
        public int max { get; set; }
        public int normalized_mean { get; set; }
        public int normalized_min { get; set; }
        public int normalized_max { get; set; }
        public int std_dev { get; set; }
    }

}
