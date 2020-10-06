using ASP.NETCoreBOT.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASP.NETCoreBOT.Services
{
    public class MulingServices
    {
        private readonly IMongoCollection<Muling> _Muling;
        public MulingServices(IAccountDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase("Muling");
            _Muling = database.GetCollection<Muling>("Muling");
        }

        public string get() =>
            _Muling.Find(Muling => true).FirstOrDefault().Trades.ToString();

        public void Update(Muling muleIn, IMongoCollection<Muling> collection) =>
            collection.ReplaceOne(muling => muling.Id == muleIn.Id, muleIn);


    }
}
