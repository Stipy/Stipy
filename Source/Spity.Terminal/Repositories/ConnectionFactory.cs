using System;
using MongoDB.Driver;

namespace Spity.Terminal.Repositories
{
    public sealed class ConnectionFactory
    {
        private readonly IMongoDatabase _database;

        public ConnectionFactory(string connectionString)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("Spity");
        }

        public IMongoDatabase OpenConnection()
        {
            return _database;
        }
    }
}
