using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using Nelibur.Sword.Extensions;
using NLog;
using Spity.Contracts;
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

        public List<FeedbackResponseObject> GetAll()
        {
            return OpenConnection()
                .GetCollection<FeedbackEntity>(FeedbackName)
                .Find("{}")
                .ToListAsync()
                .Result
                .OrderBy(x => x.CreateTime)
                .ConvertAll(Convert);
        }

        public void Save(FeedbackEntity entity)
        {
            entity.Id = ObjectId.GenerateNewId();
            entity.CreateTime = DateTime.Now;

            _logger.Debug("-> Save {0}", entity.Id);

            OpenConnection()
                .GetCollection<FeedbackEntity>(FeedbackName)
                .InsertOneAsync(entity).Wait();

            _logger.Debug("<- Save {0}", entity.Id);
        }

        private FeedbackResponseObject Convert(FeedbackEntity entity)
        {
            return new FeedbackResponseObject
            {
                Image = entity.Image,
                Text = entity.Text,
                CreateTime = entity.CreateTime
            };
        }

        private IMongoDatabase OpenConnection()
        {
            return _connectionFactory.OpenConnection();
        }
    }
}
