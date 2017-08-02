using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpskinsDll.TO
{
    public class ReturnItems
    {
        public List<Return> offers { get; set; }
    }

    public class Return
    {
        public string bot_id { get; set; }
        public List<string> items { get; set; }
        public string tradeoffer_id { get; set; }
        public string tradeoffer_error { get; set; }
    }
}
