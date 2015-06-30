using System;
using MongoDB.Bson;

namespace Spity.Terminal.Repositories.Entities
{
    public sealed class FeedbackEntity
    {
        public ObjectId Id { get; set; }
        public byte[] Image { get; set; }
        public string Text { get; set; }
    }
}
