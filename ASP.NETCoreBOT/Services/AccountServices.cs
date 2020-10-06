using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NETCoreBOT.Services
{
    public class AccountServices
    {
        private readonly IMongoCollection<Accounts> _Accounts;

        public AccountServices(IAccountDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
        }
        /*public List<Accounts> get() =>
            _Accounts.Find(Accounts => true).ToList();*/

        /*public Accounts get(String email) =>
            _Accounts.Find(account => account.Email == email).FirstOrDefault();

        public Accounts Create(Accounts accounts)
        {
            _Accounts.InsertOne(accounts);
            return accounts;
        }*/

        public void Update(string email, Accounts accountIn, IMongoCollection<Accounts> collection) =>
            collection.ReplaceOne(account => account.email == email, accountIn);

       public void Remove(string email, Accounts accountIn, IMongoCollection<Accounts> collection) =>
            _Accounts.DeleteOne(account => account.email == email);

       /* public void Remove(string email) =>
            _Accounts.DeleteOne(account => account.Email == email);*/
    }
}
