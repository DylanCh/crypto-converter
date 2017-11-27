using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Linq;

namespace crypto_converter.Controllers{
    
    public class MineController:Controller{
        private readonly string[] CURRENCIES =  new string[]{
             "BTC","USD","EUR","CNY","HKD","JPY","AUD","CND","NZD","SGD",
             "CAD","CLP","GBP","DKK","SEK","ISK","CHF","BRL","RUB"};

        [HttpGet]
         [Route("/Mine/Get")]
         public async Task<JsonResult> Get(){
            var cryptoCurrency = Request.Query["crypto"].ToString().ToUpper();
            var toCurrency=Request.Query["currencies"].ToString().ToUpper();
            if (string.IsNullOrEmpty( toCurrency)){
                toCurrency = string.Join(',',CURRENCIES);
            }
            if (string.IsNullOrEmpty(cryptoCurrency)){
                 throw new Exception ("Missing crypto currency type");
             }
             var url = $"https://min-api.cryptocompare.com/data/price?fsym={cryptoCurrency}&tsyms="
                + toCurrency;
            var client = new RestSharp.RestClient(url);
            var req = new RestRequest(Method.GET);
            req.RequestFormat = DataFormat.Json;
            req.AddHeader("Accept","application/json");
            var tcs=new TaskCompletionSource<JsonResult>();
            var result = new Models.Currencies();
            client.ExecuteAsync(req, response =>{
                result = JsonConvert.DeserializeObject<Models.Currencies>(response.Content);
                var returnObj = new ExpandoObject() as IDictionary<string, Object>;
                foreach(var prop in result.GetType().GetProperties()){
                    var val = prop.GetValue(result,null);
                    if(val is Double && (double)val !=0.0){
                        returnObj.Add(prop.Name, val);
                    }
                }
                tcs.SetResult(Json(returnObj));
            });
            
            return await tcs.Task;
         }

         [HttpGet]
        public JsonResult CheckSymbols(){
            return  Json( CURRENCIES.Where(o => o.Equals (Request.Query["filter"].ToString(),
                StringComparison.OrdinalIgnoreCase)).ToList());
        }
    }

    
}