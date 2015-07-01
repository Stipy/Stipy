using System;
using Nelibur.ServiceModel.Services.Operations;
using Nelibur.Sword.Extensions;
using Spity.Contracts;
using Spity.Terminal.Repositories;
using Spity.Terminal.Repositories.Entities;

namespace Spity.Terminal.ServiceProviders.Commands
{
    public sealed class SaveFeedbackCommand : IPostOneWay<FeedbackObject>
    {
        private const int TextMaxLength = 500;
        private readonly FeedbackRepository _repository;

        public SaveFeedbackCommand(FeedbackRepository repository)
        {
            _repository = repository;
        }

        public void PostOneWay(FeedbackObject request)
        {
            request.ToOption()
                   .Where(Validate)
                   .Map(x => Convert(x))
                   .Do(x => _repository.Save(x))
                   .ThrowOnEmpty<ArgumentException>();
        }

        private FeedbackEntity Convert(FeedbackObject request)
        {
            var result = new FeedbackEntity
            {
                Text = request.Text
            };
            if (string.IsNullOrWhiteSpace(request.Image))
            {
                return result;
            }
            string searchString = "base64,";
            var endIndex = request.Image.IndexOf(searchString, 0, StringComparison.OrdinalIgnoreCase);
            var image = request.Image.Remove(0, endIndex + searchString.Length);
            result.Image = System.Convert.FromBase64String(image);
            return result;
        }

        private bool Validate(FeedbackObject request)
        {
            if (request.IsNull())
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(request.Text) && string.IsNullOrWhiteSpace(request.Image))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(request.Text) == false && request.Text.Length > TextMaxLength)
            {
                return false;
            }
            return true;
        }
    }
}
