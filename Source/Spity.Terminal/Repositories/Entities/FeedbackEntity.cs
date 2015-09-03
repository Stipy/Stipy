using System;
using MongoDB.Bson;

namespace Spity.Terminal.Repositories.Entities
{
    public sealed class FeedbackEntity
    {
        public DateTime CreateTime { get; set; }
        public ObjectId Id { get; set; }
        public string Image { get; set; }
        public string Text { get; set; }
    }
}
