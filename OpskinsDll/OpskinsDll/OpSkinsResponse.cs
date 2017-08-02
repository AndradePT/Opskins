using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpskinsDll
{
    public class OpSkinsResponse<T>
    {
        public int status { get; set; }
        public int time { get; set; }
        public string message { get; set; }
        public int balance { get; set; }
        public int credits { get; set; }
        public int current_page { get; set; }
        public int total_pages { get; set; }
        public T response { get; set; }
        public TimeReq RequestTime { get; set; }
    }

    public class TimeReq
    {
        public DateTime LastByte { get; set; }
        public DateTime RequestStart { get; set; }
        public long LatencyMS { get; set; }
        public bool HasError { get; set; }
        public string Error { get; set; }
        public int QueriesRemaining { get; set; }
    }
}
