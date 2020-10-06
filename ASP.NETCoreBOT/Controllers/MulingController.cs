using ASP.NETCoreBOT.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ASP.NETCoreBOT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MulingController : ControllerBase
    {
        private readonly MulingServices _mulingServices;
        public MulingController(MulingServices mulingServices)
        {
            _mulingServices = mulingServices;
        }
        // GET: /<controller>/
        [HttpGet]
        public ActionResult<string> Get() =>
            _mulingServices.get();

        [HttpPut]
        public IActionResult Update([FromBody]JsonElement obj)
        {
            //gets the json and cuts it until substring
            string trades = System.Text.Json.JsonSerializer.Serialize(obj).Split(':')[1].Split('}')[0];
            trades = trades.Substring(0, 1).ToUpper() + trades.Substring(1);
            trades = trades.Replace("\"", "");
            AccountsDatabaseSettings settings = new AccountsDatabaseSettings();
            MongoClient client = new MongoClient("mongodb+srv://login:password@cluster0-c5jud.azure.mongodb.net/test?retryWrites=true&w=majority");
            var database = client.GetDatabase("Muling");
            IMongoCollection<Muling> collection = database.GetCollection<Muling>("Muling");
            var builder = Builders<Muling>.Filter;
            var filter = builder.Exists("Trades");
            Muling muling = database.GetCollection<Muling>("Muling").Find(filter).FirstOrDefault();
            if (muling == null)
            {
                return NotFound();
            }
            if (trades == "remove")
            {
                if(muling.Trades != "0")
                {
                    int temp = int.Parse(muling.Trades);
                    temp--;
                    muling.Trades = temp.ToString();
                }
                else
                {
                    return Ok(muling.Trades);
                }
            }
            else if(trades == "add")
            {
                int temp = int.Parse(muling.Trades);
                temp++;
                muling.Trades = temp.ToString();
            }
            _mulingServices.Update(muling, collection);
            return Ok(muling.Trades);
        }
    }
}
