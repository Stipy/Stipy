using System;
using MongoDB.Bson;
using MongoDB.Driver;
using NLog;
using Spity.Terminal.Repositories.Entities;

namespace Spity.Terminal.Repositories
{
    public sealed class FeedbackRepository
    {
        private const string FeedbackName = "Feedback";
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ConnectionFactory _connectionFactory;

        public FeedbackRepository(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void Save(FeedbackEntity entity)
        {
            entity.Id = ObjectId.GenerateNewId();
            _logger.Debug("-> Save {0}", entity.Id);

            OpenConnection()
                .GetCollection<FeedbackEntity>(FeedbackName)
                .InsertOneAsync(entity).Wait();

            _logger.Debug("<- Save {0}", entity.Id);
        }

        private IMongoDatabase OpenConnection()
        {
            return _connectionFactory.OpenConnection();
        }
    }
}
