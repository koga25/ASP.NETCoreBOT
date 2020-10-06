using ASP.NETCoreBOT.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASP.NETCoreBOT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AccountServices _accountsServices;

        public AccountsController(AccountServices accountServices)
        {
            _accountsServices = accountServices;
        }

        /*[HttpGet]
        public ActionResult<List<Accounts>> get() =>
            _accountsServices.get();*/

        /*[HttpGet("{email:}", Name = "GetAccounts")]
        public ActionResult<Accounts> Get(String email)
        {
            var account = _accountsServices.get(email);
            if (account == null)
            {
                return NotFound();
            }
            return account;
        }

        [HttpPost]
        public ActionResult<Accounts> Create(Accounts account)
        {
            _accountsServices.Create(account);

            return CreatedAtRoute("GetAccounts", new { email = account.Email}, account);
        }*/

        [HttpDelete]
        public IActionResult Delete([FromBody]Accounts accountsIn)
        {
            AccountsDatabaseSettings settings = new AccountsDatabaseSettings();
            MongoClient client = new MongoClient("mongodb+srv://login:password@cluster0-c5jud.azure.mongodb.net/test?retryWrites=true&w=majority");
            var database = client.GetDatabase("Cluster0");

            var collections = database.ListCollectionNames().ToList();
            var bre = new BsonRegularExpression(accountsIn.email);
            var copt = new ListCollectionsOptions
            {
                Filter =
                    Builders<BsonDocument>.Filter.Regex("email", bre)
            };
            var collection = database.ListCollections(copt).FirstOrDefault();
            IMongoCollection<Accounts> coll = null;
            var emailFilter = Builders<BsonDocument>.Filter.Eq("email", accountsIn.email);
            for (int i = 0; i < collections.Count; i++)
            {
                if (database.GetCollection<BsonDocument>(collections[i]).CountDocuments(emailFilter) != 0)
                {
                    coll = database.GetCollection<Accounts>(collections[i]);
                    database.DropCollection(collections[i]);
                    break;
                }
            }
            if (accountsIn.Id == null)
            {
                return NotFound();
            }

            _accountsServices.Remove(accountsIn.email, accountsIn, coll);
            return NoContent();
        }

        [HttpPut]
        public IActionResult Update([FromBody]Accounts accountsIn)
        {
            AccountsDatabaseSettings settings = new AccountsDatabaseSettings();
            MongoClient client = new MongoClient("mongodb+srv://login:password@cluster0-c5jud.azure.mongodb.net/test?retryWrites=true&w=majority");
            var database = client.GetDatabase("Cluster0");

            var collections = database.ListCollectionNames().ToList();
            var bre = new BsonRegularExpression("415263");
            var copt = new ListCollectionsOptions
            {
                Filter =
                    Builders<BsonDocument>.Filter.Exists("password")
            };
            var collection = database.ListCollections(copt).FirstOrDefault();
            var builder = Builders<Accounts>.Filter;
            var filter = builder.Eq("accountType", "Tutorial");
            IMongoCollection<Accounts> coll = null;
            var emailFilter = Builders<BsonDocument>.Filter.Eq("email", accountsIn.email);
            //passes through all collections until finding the one with the filter
            for(int i=0; i<collections.Count; i++)
            {
                if (database.GetCollection<BsonDocument>(collections[i]).CountDocuments(emailFilter) != 0)
                {

                    coll = database.GetCollection<Accounts>(collections[i]);
                    var elements = database.GetCollection<BsonDocument>(collections[i]).Find(emailFilter).FirstOrDefault().Elements.ToList();
                    Accounts temp = new Accounts();
                    for(int x = 0; x < elements.Count; x++)
                    {
                         if(elements.ElementAt(x).Name == "_id")
                        {
                            temp.Id = elements.ElementAt(x).Value.ToString();
                        }
                      
                    }

                    //query database for all collections that have certain account type
                    var type = database.ListCollectionNames().ToList();
                    int types = 0;
                    var typeFilter = Builders<BsonDocument>.Filter.Eq("accountType", "TomatoShop");
                    for (int y = 0; y < type.Count; y++)
                    {
                        if (database.GetCollection<BsonDocument>(collections[y]).CountDocuments(typeFilter) > 0)
                        {
                            types++;
                        }
                        if (types >= 15)
                        {
                            temp.accountType = "Quests";
                            accountsIn.accountType = temp.accountType;
                            break;
                        }
                    }
                    ///////////////////////////////////////////////////////////////////////////////////////////
                    

                    accountsIn.Id = temp.Id;
                    database.GetCollection<BsonDocument>(collections[i]).FindOneAndReplace<BsonDocument>(filter.ToBsonDocument(), accountsIn.ToBsonDocument());


         ;
                    break;
                }
            }
            
            
               
            if (accountsIn.Id == null)
            {
                return NotFound();
            }

            _accountsServices.Update(accountsIn.email, accountsIn, coll);
            return NoContent();
        }
    }
}