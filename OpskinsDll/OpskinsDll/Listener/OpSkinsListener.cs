using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using System.Collections.Concurrent;
using System.Threading;
using OpskinsDll.TO;

namespace OpskinsDll.Listener
{
    public class OpSkinsListener
    {

        private static OpSkinsListener listener = null;
        private int connectionCount;
        private OpskinsWeb client;
        private static DateTime lastRequestStart;
        private string Appid;

        private static DateTime latestDataRequestStart = DateTime.Now;
        private static DateTime latestDataRequestFinish = DateTime.Now;

        private static object lockObj = new object();

        private ConcurrentDictionary<string, IObservable<List<LowestListPrices>>> markets =
           new ConcurrentDictionary<string, IObservable<List<LowestListPrices>>>();

        private ConcurrentDictionary<string, IObserver<List<LowestListPrices>>> observers =
            new ConcurrentDictionary<string, IObserver<List<LowestListPrices>>>();

        private OpSkinsListener(OpskinsWeb client,string Appid, int connectionCount)
        {
            this.client = client;
            this.connectionCount = connectionCount;
            this.Appid = Appid;
            Task.Run(() => PollMarketBooks());
        }

        public static OpSkinsListener Create(OpskinsWeb client,string Appid ,int connectionCount)
        {

            if (listener == null)
                listener = new OpSkinsListener(client, Appid, connectionCount);

            return listener;
        }

        public IObservable<List<LowestListPrices>> SubscribeGetPrices()
        {
            IObservable<List<LowestListPrices>> market;
            if (markets.TryGetValue("", out market))
                return market;

            var observable = Observable.Create<List<LowestListPrices>>(
               (IObserver<List<LowestListPrices>> observer) =>
               {
                   observers.AddOrUpdate("", observer, (key, existingVal) => existingVal);
                   return Disposable.Create(() =>
                   {
                       IObserver<List<LowestListPrices>> ob;
                       IObservable<List<LowestListPrices>> o;
                       markets.TryRemove("", out o);
                       observers.TryRemove("", out ob);
                   });
               })
               .Publish()
               .RefCount();

            markets.AddOrUpdate("", observable, (key, existingVal) => existingVal);
            return observable;
        }


        // TODO:// replace this with the Rx scheduler 
        private void PollMarketBooks()
        {
            for (int i = 0; i < connectionCount; i++)
            {
                Task.Run(() =>
                {
                    while (true)
                    {
                        if (markets.Count > 0)
                        {
                            // TODO:// look at spinwait or signalling instead of this
                            while (connectionCount > 1 && DateTime.Now.Subtract(lastRequestStart).TotalMilliseconds < (5000 / connectionCount))
                            {
                                int waitMs = (1000 / connectionCount) - (int)DateTime.Now.Subtract(lastRequestStart).TotalMilliseconds;
                                Thread.Sleep(waitMs > 0 ? waitMs : 0);
                            }

                            lock (lockObj)
                                lastRequestStart = DateTime.Now;

                            
                             var book = client.GetAllLowestListPrices(Appid).Result;

                            if (!book.RequestTime.HasError)
                            {
                                // we may have fresher data than the response to this request
                                if (book.RequestTime.RequestStart < latestDataRequestStart && book.RequestTime.LastByte > latestDataRequestFinish)
                                    continue;
                                else
                                {
                                    lock (lockObj)
                                    {
                                        latestDataRequestStart = book.RequestTime.RequestStart;
                                        latestDataRequestFinish = book.RequestTime.LastByte;
                                    }
                                }


                                    IObserver<List<LowestListPrices>> o;
                                    if (observers.TryGetValue("", out o))
                                    {
                                        // check to see if the market is finished
                                        if (book.RequestTime.HasError)
                                            o.OnCompleted();
                                        else
                                            o.OnNext(book.response);
                                    }
                               
                            }
 
                        }
                        else
                            // TODO:// will die with rx scheduler
                            Thread.Sleep(500);
                    }
                });
                Thread.Sleep(1000 / connectionCount);
            }
        }
    }
}

