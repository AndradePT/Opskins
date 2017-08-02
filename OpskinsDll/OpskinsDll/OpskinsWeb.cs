using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpskinsDll.TO;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace OpskinsDll
{
    public class OpskinsWeb 
    {
        private readonly string OPKINS_ENDPOINT = "https://api.opskins.com/";
        public string API_KEY = "";
        private int QueriesRemaining = 0;

        private readonly string BOT_LIST = "IStatus/GetBotList/v1/";
        private readonly string GET_BALANCE = "IUser/GetBalance/v1/";
        private readonly string GET_AllLowestListPrices = "IPricing/GetAllLowestListPrices/v1/";
        private readonly string GET_PriceList = "IPricing/GetPriceList/v2/";
        private readonly string GET_Sales = "ISales/GetSales/v1/";
        private readonly string GET_ListItems = "ISales/ListItems/v1/";
        private readonly string GET_ReturnItems = "ISales/ReturnItems/v1/";
        private readonly string GET_EditPrice = "ISales/EditPrice/v1/";
        private readonly string GET_EditPriceMulti = "ISales/EditPriceMulti/v1/";
        private readonly string GET_ActiveTradeOffers = "ISales/GetActiveTradeOffers/v1/";
        private readonly string GET_Search = "ISales/Search/v1/";
        private readonly string GET_BuyItems = "ISales/BuyItems/v1/";
        private readonly string GET_Inventory = "IInventory/GetInventory/v1/";
        private readonly string GET_Withdraw = "IInventory/Withdraw/v1/";
        private readonly string GET_Deposit = "IInventory/Deposit/v1/";


        public OpskinsWeb(string API_KEY)
        {
            this.API_KEY = API_KEY;
        }

        public Task<string> InvokeGet(string _Method, List<Parameter> data = null)
        {
            var client = new RestClient(OPKINS_ENDPOINT);
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest(_Method, Method.GET);

            request.AddParameter("key", this.API_KEY, ParameterType.GetOrPost); // adds to POST or URL querystring based on Method

            if (data != null)
            {
                foreach (var item in data)
                {
                    if(item.Value != null)
                        request.AddParameter(item); // adds to POST or URL querystring based on Method
                }
            }

            // execute the request
            IRestResponse response = client.Execute(request);
            var content = response.Content; // raw content as string

            var Querie = response.Headers.Where(x => x.Name == "X-Queries-Remaining");
            if (Querie.Count() > 0)
            {
                QueriesRemaining = Convert.ToInt32(Querie.FirstOrDefault().Value);
            }

              

            return Task.FromResult<string>(content);
        }

        public Task<string> InvokePost(string _Method, List<Parameter> data)
        {
            var client = new RestClient(OPKINS_ENDPOINT);
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest(_Method, Method.POST)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = new RestSharp.Serializers.JsonSerializer()
            };

            request.AddParameter("key", this.API_KEY, ParameterType.GetOrPost); // adds to POST or URL querystring based on Method

            if (data != null)
            {
                foreach (var item in data)
                {
                    if (item.Value != null || item.Name != null)
                        request.AddParameter(item); // adds to POST or URL querystring based on Method
                }
            }


          


            // execute the request
            IRestResponse response = client.Execute(request);
            var content = response.Content; // raw content as string

            return Task.FromResult<string>(content);
        }


        private OpSkinsResponse<T> ToResponse<T>(OpSkinsResponse<T> response, DateTime requestStart, DateTime lastByteStamp, long latency ,bool HasError ,string Error)
        {
            OpSkinsResponse<T> r = new OpSkinsResponse<T>();
            TimeReq req = new TimeReq();
            r = response;
            req.Error = Error;
            req.HasError = HasError;
            req.LatencyMS = latency;
            req.LastByte = lastByteStamp;
            req.RequestStart = requestStart;
            req.QueriesRemaining = QueriesRemaining;
            r.RequestTime = req;
            return  r;
        }


        public Task<OpSkinsResponse<List<BotStatus>>> GetBotStatus()
        {

            DateTime requestStart = DateTime.Now;
            Stopwatch watch = new Stopwatch();
            watch.Start();



            OpSkinsResponse<List<BotStatus>> _botstatus = new OpSkinsResponse<List<TO.BotStatus>>();

            var ValueReturn =  InvokeGet(BOT_LIST);

            return ValueReturn.ContinueWith(c =>
            {
                try
                {
                    try
                    {

                        var Value = JObject.Parse(c.Result);

                        var _bots = Value["response"]["bots"].Values<JToken>().Select(z => z.First.ToObject<BotStatus>()).ToList();


                        Value.Property("response").Remove();
                        _botstatus = JsonConvert.DeserializeObject<OpSkinsResponse<List<BotStatus>>>(Value.ToString());


                        _botstatus.response = _bots;

                        var lastByte = DateTime.Now;
                        watch.Stop();

                        return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, false, "");
                    }
                    catch (Exception ex)
                    {
                        var lastByte = DateTime.Now;
                        watch.Stop();

                        _botstatus.status = 0;
                        _botstatus.message = ex.Message;

                        return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, true, ex.Message);
                    }
                }
                catch (Exception ex)
                {

                    var lastByte = DateTime.Now;
                    watch.Stop();

                    _botstatus.status = 0;
                    _botstatus.message = c.Result;

                    return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, true, ex.Message);

                }
            });

        }

        public Task<OpSkinsResponse<string>> GetBalance()
        {
            DateTime requestStart = DateTime.Now;
            Stopwatch watch = new Stopwatch();
            watch.Start();

            OpSkinsResponse<string> _botstatus = new OpSkinsResponse<string>();

            var ValueReturn = InvokeGet(GET_BALANCE);


            return ValueReturn.ContinueWith(c =>
            {
                try
                {
                    try
                    {
                        _botstatus = JsonConvert.DeserializeObject<OpSkinsResponse<string>>(c.Result);

                        var lastByte = DateTime.Now;
                        watch.Stop();

                        return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, false, "");
                    }
                    catch (Exception ex)
                    {
                        var lastByte = DateTime.Now;
                        watch.Stop();

                        _botstatus.status = 0;
                        _botstatus.message = ex.Message;

                        return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, true, ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    var lastByte = DateTime.Now;
                    watch.Stop();

                    _botstatus.status = 0;
                    _botstatus.message = c.Result;

                    return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, true, ex.Message);
                }
            });

        }

        public Task<OpSkinsResponse<List<LowestListPrices>>> GetAllLowestListPrices(string appid)
        {
            DateTime requestStart = DateTime.Now;
            Stopwatch watch = new Stopwatch();
            watch.Start();

            OpSkinsResponse<List<LowestListPrices>> _botstatus = new OpSkinsResponse<List<LowestListPrices>>();

            Parameter data = new Parameter();
            data.Name = "appid";
            data.Value = appid;
            data.Type = ParameterType.GetOrPost;

            var ValueReturn = InvokeGet(GET_AllLowestListPrices, new List<Parameter>() { data });

            return ValueReturn.ContinueWith(c =>
            {
                try
                {
                    try
                    {
                        var Value = JObject.Parse(c.Result);

                        var _bots = Value["response"].Values<JToken>().Select(z => new LowestListPrices()
                        {
                            Description = z.First.ToObject<ListPricesDescription>(),
                            Name = z.ToObject<JProperty>().Name

                        }).ToList();

                        Value.Property("response").Remove();
                        _botstatus = JsonConvert.DeserializeObject<OpSkinsResponse<List<LowestListPrices>>>(Value.ToString());


                        _botstatus.response = _bots;

                        var lastByte = DateTime.Now;
                        watch.Stop();

                        return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, false, "");
                    }
                    catch (Exception ex)
                    {
                        var lastByte = DateTime.Now;
                        watch.Stop();

                        _botstatus.status = 0;
                        _botstatus.message = ex.Message;

                        return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, true, ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    var lastByte = DateTime.Now;
                    watch.Stop();

                    _botstatus.status = 0;
                    _botstatus.message = c.Result;

                    return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, true, ex.Message);
                }
            });

        }

        public Task<OpSkinsResponse<List<PriceList>>> GetPriceList(string appid)
        {
            DateTime requestStart = DateTime.Now;
            Stopwatch watch = new Stopwatch();
            watch.Start();

            OpSkinsResponse<List<PriceList>> _botstatus = new OpSkinsResponse<List<PriceList>>();

            Parameter data = new Parameter();
            data.Name = "appid";
            data.Value = appid.ToString();
            data.Type = ParameterType.GetOrPost;

            var ValueReturn = InvokeGet(GET_PriceList, new List<Parameter>() { data });

            return ValueReturn.ContinueWith(c =>
            {
                try
                {
                    try
                    {
                        var Value = JObject.Parse(c.Result);

                        var _bots = Value["response"].Values<JToken>().Select(z => new PriceList()
                        {
                            Name = z.ToObject<JProperty>().Name,
                            days = z.Value<JToken>().SelectMany(x => x.Value<JToken>().Select(y => new DayDescription()
                            {
                                date = y.ToObject<JProperty>().Name,
                                description = y.First.ToObject<ItemDescription>()

                            })).ToList()

                        }).ToList();

                        Value.Property("response").Remove();
                        _botstatus = JsonConvert.DeserializeObject<OpSkinsResponse<List<PriceList>>>(Value.ToString());


                        _botstatus.response = _bots;

                        var lastByte = DateTime.Now;
                        watch.Stop();

                        return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, false, "");
                    }
                    catch (Exception ex)
                    {
                        var lastByte = DateTime.Now;
                        watch.Stop();

                        _botstatus.status = 0;
                        _botstatus.message = ex.Message;

                        return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, true, ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    var lastByte = DateTime.Now;
                    watch.Stop();

                    _botstatus.status = 0;
                    _botstatus.message = c.Result;

                    return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, true, ex.Message);
                }
            });
        }

        public Task<OpSkinsResponse<List<Sales>>> GetSales()
        {
            DateTime requestStart = DateTime.Now;
            Stopwatch watch = new Stopwatch();
            watch.Start();

            OpSkinsResponse<List<Sales>> _botstatus = new OpSkinsResponse<List<Sales>>();


            var ValueReturn = InvokeGet(GET_Sales);

            return ValueReturn.ContinueWith(c =>
            {
                try
                {
                    try
                    {
                        _botstatus = JsonConvert.DeserializeObject<OpSkinsResponse<List<Sales>>>(c.Result);

                        var lastByte = DateTime.Now;
                        watch.Stop();

                        return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, false, "");
                    }
                    catch (Exception ex)
                    {
                        var lastByte = DateTime.Now;
                        watch.Stop();

                        _botstatus.status = 0;
                        _botstatus.message = ex.Message;

                        return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, true, ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    var lastByte = DateTime.Now;
                    watch.Stop();

                    _botstatus.status = 0;
                    _botstatus.message = c.Result;

                    return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, true, ex.Message);
                }
            });


        }

        public Task<OpSkinsResponse<List<Sales>>> GetSales(SalesFilter Filter)
        {
            DateTime requestStart = DateTime.Now;
            Stopwatch watch = new Stopwatch();
            watch.Start();

            OpSkinsResponse<List<Sales>> _botstatus = new OpSkinsResponse<List<Sales>>();


            List<Parameter> data = new List<Parameter>()
            {
                new Parameter(){ Name = "type" , Value =  (int)Filter.Type , Type = ParameterType.GetOrPost},
                Filter.appid != null ? new Parameter(){ Name = "appid" , Value =  Filter.appid , Type = ParameterType.GetOrPost} : new Parameter(),
                Filter.appid != null ? new Parameter(){ Name = "page" , Value =  Filter.page , Type = ParameterType.GetOrPost} : new Parameter(),
                Filter.appid != null ? new Parameter(){ Name = "per_page" , Value =  Filter.per_page , Type = ParameterType.GetOrPost} : new Parameter(),
                Filter.appid != null ? new Parameter(){ Name = "sort" , Value =  Filter.Sort , Type = ParameterType.GetOrPost} : new Parameter(),
            };



            var ValueReturn = InvokeGet(GET_Sales, data);


            return ValueReturn.ContinueWith(c =>
            {
                try
                {
                    try
                    {
                        _botstatus = JsonConvert.DeserializeObject<OpSkinsResponse<List<Sales>>>(c.Result);

                        var lastByte = DateTime.Now;
                        watch.Stop();

                        return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, false, "");
                    }
                    catch (Exception ex)
                    {
                        var lastByte = DateTime.Now;
                        watch.Stop();

                        _botstatus.status = 0;
                        _botstatus.message = ex.Message;

                        return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, true, ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    var lastByte = DateTime.Now;
                    watch.Stop();

                    _botstatus.status = 0;
                    _botstatus.message = c.Result;

                    return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, true, ex.Message);
                }
            });

        }

        public Task<OpSkinsResponse<ListItems>> GetListItems(List<ItemsFilter> Filter)
        {
            DateTime requestStart = DateTime.Now;
            Stopwatch watch = new Stopwatch();
            watch.Start();

            OpSkinsResponse<ListItems> _botstatus = new OpSkinsResponse<ListItems>();

            List<Parameter> data = new List<Parameter>()
            {
                new Parameter(){ Name = "items",Value = JsonConvert.SerializeObject(Filter),Type = ParameterType.GetOrPost }
            };

            var ValueReturn = InvokePost(GET_ListItems, data);

            return ValueReturn.ContinueWith(c =>
            {
                try
                {
                    try
                    {
                        _botstatus = JsonConvert.DeserializeObject<OpSkinsResponse<ListItems>>(c.Result);

                        var lastByte = DateTime.Now;
                        watch.Stop();

                        return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, false, "");
                    }
                    catch (Exception ex)
                    {
                        var lastByte = DateTime.Now;
                        watch.Stop();

                        _botstatus.status = 0;
                        _botstatus.message = ex.Message;

                        return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, true, ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    var lastByte = DateTime.Now;
                    watch.Stop();

                    _botstatus.status = 0;
                    _botstatus.message = c.Result;

                    return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, true, ex.Message);
                }
            });
        }

        public Task<OpSkinsResponse<ReturnItems>> GetReturnItems(List<int> Filter)
        {
            DateTime requestStart = DateTime.Now;
            Stopwatch watch = new Stopwatch();
            watch.Start();

            OpSkinsResponse<ReturnItems> _botstatus = new OpSkinsResponse<ReturnItems>();


            object Ids = string.Join(",", Filter);
            List<Parameter> data = new List<Parameter>()
            {
                new Parameter(){ Name = "items" ,  Value = Ids , Type = ParameterType.GetOrPost}
            };

            var ValueReturn = InvokePost(GET_ReturnItems, data);

           

            return ValueReturn.ContinueWith(c =>
            {
                try
                {
                    try
                    {
                        _botstatus = JsonConvert.DeserializeObject<OpSkinsResponse<ReturnItems>>(c.Result);

                        var lastByte = DateTime.Now;
                        watch.Stop();

                        return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, false, "");
                    }
                    catch (Exception ex)
                    {
                        var lastByte = DateTime.Now;
                        watch.Stop();

                        _botstatus.status = 0;
                        _botstatus.message = ex.Message;

                        return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, true, ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    var lastByte = DateTime.Now;
                    watch.Stop();

                    _botstatus.status = 0;
                    _botstatus.message = c.Result;

                    return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, true, ex.Message);
                }
            });

        }

        public Task<OpSkinsResponse<EditPrice>> GetEditPrice(EditPriceFilter Filter)
        {
            DateTime requestStart = DateTime.Now;
            Stopwatch watch = new Stopwatch();
            watch.Start();

            OpSkinsResponse<EditPrice> _botstatus = new OpSkinsResponse<EditPrice>();

            List<Parameter> data = new List<Parameter>()
            {

                new Parameter(){ Name = "saleid" , Value = Filter.saleid , Type = ParameterType.GetOrPost },
                new Parameter(){ Name = "price" , Value = Filter.price,  Type = ParameterType.GetOrPost  }

            };

            var ValueReturn = InvokePost(GET_EditPrice, data);

            return ValueReturn.ContinueWith(c =>
            {
                try
                {
                    try
                    {
                        _botstatus = JsonConvert.DeserializeObject<OpSkinsResponse<EditPrice>>(c.Result);

                        var lastByte = DateTime.Now;
                        watch.Stop();

                        return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, false, "");
                    }
                    catch (Exception ex)
                    {
                        var lastByte = DateTime.Now;
                        watch.Stop();

                        _botstatus.status = 0;
                        _botstatus.message = ex.Message;

                        return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, true, ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    var lastByte = DateTime.Now;
                    watch.Stop();

                    _botstatus.status = 0;
                    _botstatus.message = c.Result;

                    return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, true, ex.Message);
                }
            });
        }

        ////public OpSkinsResponse<string> GetEditPriceMulti(List<EditPriceFilter> Filter)
        ////{
        ////    OpSkinsResponse<string> _botstatus = new OpSkinsResponse<string>();


        ////    var dictionary = new Dictionary<string, object>();

        ////    List<Parameter> data = new List<Parameter>();

        ////    foreach (var item in Filter)
        ////    {
        ////        dictionary.Add(item.saleid.ToString() , item.price);
        ////    }






        ////        data.Add(new Parameter() { Name = "items", Value = h, Type = ParameterType.GetOrPost });




        ////    var ValueReturn = InvokePost(GET_EditPriceMulti, data);

        ////    try
        ////    {
        ////        _botstatus = JsonConvert.DeserializeObject<OpSkinsResponse<string>>(ValueReturn);

        ////        return _botstatus;
        ////    }
        ////    catch (Exception)
        ////    {
        ////        return _botstatus;
        ////    }

        ////}

        public Task<OpSkinsResponse<List<ActiveTradeOffers>>> GetActiveTradeOffers()
        {
            DateTime requestStart = DateTime.Now;
            Stopwatch watch = new Stopwatch();
            watch.Start();

            OpSkinsResponse<List<ActiveTradeOffers>> _botstatus = new OpSkinsResponse<List<ActiveTradeOffers>>();

            var ValueReturn = InvokeGet(GET_ActiveTradeOffers);

            return ValueReturn.ContinueWith(c =>
            {
                try
                {
                    try
                    {
                        var Value = JObject.Parse(c.Result);

                        var _bots = Value["response"]["offers"].Values<JToken>().Select(z => new ActiveTradeOffers()
                        {
                            offersId = z.ToObject<JProperty>().Name,
                            offers = z.First.ToObject<ActiveTradeOffersReq>()
                        }).ToList();

                        Value.Property("response").Remove();

                        _botstatus = JsonConvert.DeserializeObject<OpSkinsResponse<List<ActiveTradeOffers>>>(Value.ToString());


                        _botstatus.response = _bots;
                        var lastByte = DateTime.Now;
                        watch.Stop();

                        return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, false, "");
                    }
                    catch (Exception ex)
                    {
                        var lastByte = DateTime.Now;
                        watch.Stop();

                        _botstatus.status = 0;
                        _botstatus.message = ex.Message;

                        return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, true, ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    var lastByte = DateTime.Now;
                    watch.Stop();

                    _botstatus.status = 0;
                    _botstatus.message = c.Result;

                    return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, true, ex.Message);
                }
            });

        }

        public Task<OpSkinsResponse<Search>> GetSearch(SearchFilter Filter)
        {
            DateTime requestStart = DateTime.Now;
            Stopwatch watch = new Stopwatch();
            watch.Start();

            OpSkinsResponse<Search> _botstatus = new OpSkinsResponse<Search>();

            List<Parameter> data = new List<Parameter>()
            {
                Filter.app != null ? new Parameter(){ Name = "app", Value = Filter.app ,Type= ParameterType.GetOrPost } : new Parameter(),
                Filter.search_item != null ? new Parameter(){ Name = "search_item", Value = Filter.search_item ,Type= ParameterType.GetOrPost } : new Parameter(),
                Filter.min != null ? new Parameter(){ Name = "min", Value = Filter.min ,Type= ParameterType.GetOrPost } : new Parameter(),
                Filter.max != null ? new Parameter(){ Name = "app", Value = Filter.max ,Type= ParameterType.GetOrPost } : new Parameter(),
            };

            var ValueReturn = InvokeGet(GET_Search, data);

            return ValueReturn.ContinueWith(c =>
            {
                try
                {
                    try
                    {

                        _botstatus = JsonConvert.DeserializeObject<OpSkinsResponse<Search>>(c.Result);

                        var lastByte = DateTime.Now;
                        watch.Stop();

                        return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, false, "");
                    }
                    catch (Exception ex)
                    {
                        var lastByte = DateTime.Now;
                        watch.Stop();

                        _botstatus.status = 0;
                        _botstatus.message = ex.Message;

                        return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, true, ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    var lastByte = DateTime.Now;
                    watch.Stop();

                    _botstatus.status = 0;
                    _botstatus.message = c.Result;

                    return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, true, ex.Message);
                }
            });

        }

        public Task<OpSkinsResponse<BuyItems>> GetBuyItems(BuyItemsFilter Filter)
        {
            DateTime requestStart = DateTime.Now;
            Stopwatch watch = new Stopwatch();
            watch.Start();

            OpSkinsResponse<BuyItems> _botstatus = new OpSkinsResponse<BuyItems>();

            List<Parameter> data = new List<Parameter>()
            {
                Filter.saleids != null ? new Parameter(){ Name = "saleids", Value = Filter.GetSaleids() ,Type= ParameterType.GetOrPost } : new Parameter(),
                Filter.total > 0 ? new Parameter(){ Name = "total", Value = Filter.total ,Type= ParameterType.GetOrPost } : new Parameter(),
            };

            var ValueReturn = InvokePost(GET_BuyItems, data);

            return ValueReturn.ContinueWith(c =>
            {
                try
                {
                    try
                    {
                        _botstatus = JsonConvert.DeserializeObject<OpSkinsResponse<BuyItems>>(c.Result);

                        var lastByte = DateTime.Now;
                        watch.Stop();

                        return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, false, "");
                    }
                    catch (Exception ex)
                    {
                        var lastByte = DateTime.Now;
                        watch.Stop();

                        _botstatus.status = 0;
                        _botstatus.message = ex.Message;

                        return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, true, ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    var lastByte = DateTime.Now;
                    watch.Stop();

                    _botstatus.status = 0;
                    _botstatus.message = c.Result;

                    return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, true, ex.Message);
                }
            });

        }

        public Task<OpSkinsResponse<OpSkinsInventory>> GetInventory()
        {
            DateTime requestStart = DateTime.Now;
            Stopwatch watch = new Stopwatch();
            watch.Start();

            OpSkinsResponse<OpSkinsInventory> _botstatus = new OpSkinsResponse<OpSkinsInventory>();

            var ValueReturn = InvokeGet(GET_Inventory);

            return ValueReturn.ContinueWith(c =>
            {
                try
                {

                    try
                    {
                        var Value = JObject.Parse(c.Result);

                        var _bots = Value["response"]["limits"].Values<JToken>().Select(z => new Limits()
                        {
                            app = z.ToObject<JProperty>().Name,
                            free_slots = z.ToObject<JProperty>().Value.Select(x => x.First.ToObject<string>()).FirstOrDefault()
                        }).ToList();

                        var __bots = JsonConvert.DeserializeObject<List<ItemIventory>>(Value["response"]["items"].ToString());


                        Value.Property("response").Remove();
                        _botstatus = JsonConvert.DeserializeObject<OpSkinsResponse<OpSkinsInventory>>(Value.ToString());


                        _botstatus.response = new OpSkinsInventory() { items = __bots, limits = _bots };

                        var lastByte = DateTime.Now;
                        watch.Stop();

                        return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, false, "");
                    }
                    catch (Exception ex)
                    {
                        var lastByte = DateTime.Now;
                        watch.Stop();

                        _botstatus.status = 0;
                        _botstatus.message = ex.Message;

                        return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, true, ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    var lastByte = DateTime.Now;
                    watch.Stop();

                    _botstatus.status = 0;
                    _botstatus.message = c.Result;

                    return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, true, ex.Message);
                }
            });
        }

        public Task<OpSkinsResponse<Withdraw>> GetWithdraw(List<int> IDs)
        {
            DateTime requestStart = DateTime.Now;
            Stopwatch watch = new Stopwatch();
            watch.Start();

            OpSkinsResponse<Withdraw> _botstatus = new OpSkinsResponse<Withdraw>();

            object Ids = string.Join(",", IDs);
            List<Parameter> data = new List<Parameter>()
            {
                new Parameter(){ Name = "items",Value =  Ids,Type = ParameterType.GetOrPost}

            };

            var ValueReturn = InvokePost(GET_Withdraw, data);

            return ValueReturn.ContinueWith(c =>
            {
                try
                {
                    try
                    {
                        _botstatus = JsonConvert.DeserializeObject<OpSkinsResponse<Withdraw>>(c.Result);


                        var lastByte = DateTime.Now;
                        watch.Stop();

                        return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, false, "");
                    }
                    catch (Exception ex)
                    {
                        var lastByte = DateTime.Now;
                        watch.Stop();

                        _botstatus.status = 0;
                        _botstatus.message = ex.Message;

                        return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, true, ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    var lastByte = DateTime.Now;
                    watch.Stop();

                    _botstatus.status = 0;
                    _botstatus.message = c.Result;

                    return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, true, ex.Message);
                }
            });
        }

        public Task<OpSkinsResponse<Deposit>> GetDeposit(List<DepositFilter> Filter)
        {
            DateTime requestStart = DateTime.Now;
            Stopwatch watch = new Stopwatch();
            watch.Start();

            OpSkinsResponse<Deposit> _botstatus = new OpSkinsResponse<Deposit>();

            List<Parameter> data = new List<Parameter>()
            {
                new Parameter(){ Name = "items",Value = JsonConvert.SerializeObject(Filter),Type = ParameterType.GetOrPost }
            };

            var ValueReturn = InvokePost(GET_Deposit, data);

            return ValueReturn.ContinueWith(c =>
            {
                try
                {
                    try
                    {
                        _botstatus = JsonConvert.DeserializeObject<OpSkinsResponse<Deposit>>(c.Result);

                        var lastByte = DateTime.Now;
                        watch.Stop();

                        return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, false, "");
                    }
                    catch (Exception ex)
                    {
                        var lastByte = DateTime.Now;
                        watch.Stop();

                        _botstatus.status = 0;
                        _botstatus.message = ex.Message;

                        return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, true, ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    var lastByte = DateTime.Now;
                    watch.Stop();

                    _botstatus.status = 0;
                    _botstatus.message = c.Result;

                    return ToResponse(_botstatus, requestStart, lastByte, watch.ElapsedMilliseconds, true, ex.Message);
                }
            });
        }

    }
}
