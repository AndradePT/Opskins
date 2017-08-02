using OpskinsDll;
using OpskinsDll.Listener;
using OpskinsDll.TO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace testeOpskins
{
    class Program
    {
        static void Main(string[] args)
        {
            OpskinsDll.OpskinsWeb web = new OpskinsDll.OpskinsWeb("apikey");

            OpSkinsListener marketListener = OpSkinsListener.Create(web, "730", 1);


            AutoResetEvent waitHandle = new AutoResetEvent(false);

            var marketSubscription = marketListener.SubscribeGetPrices()
                .SubscribeOn(Scheduler.Default)
                .Subscribe(
                tick =>
                {

                    Console.WriteLine
                    ("Count : " +tick.Count());

                   
                },
                () =>
                {
                    Console.WriteLine("Run Bot: {0} | Market finished");
                });

            waitHandle.WaitOne();
            marketSubscription.Dispose();

            Console.ReadKey();
           
        }
    }
}
