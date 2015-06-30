using System;
using MongoDB.Bson;
using MongoDB.Driver;
using Spity.Contracts;
using Spity.Terminal.Repositories.Entities;

namespace Spity.Terminal.Repositories
{
    public sealed class FeedbackRepository
    {
        private const string FeedbackName = "Feedback";
        private readonly ConnectionFactory _connectionFactory;

        public FeedbackRepository(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void Save(FeedbackObject request)
        {
            var entity = new FeedbackEntity
            {
                Id = ObjectId.GenerateNewId(),
                Text = request.Text,
                Image = request.Image
            };

            OpenConnection()
                .GetCollection<FeedbackEntity>(FeedbackName)
                .InsertOneAsync(entity).Wait();
        }

        private IMongoDatabase OpenConnection()
        {
            return _connectionFactory.OpenConnection();
        }
    }
}
