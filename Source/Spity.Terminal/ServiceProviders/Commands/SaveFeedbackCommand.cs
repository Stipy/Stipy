using System;
using Nelibur.ServiceModel.Services.Operations;
using Nelibur.Sword.Extensions;
using Spity.Contracts;
using Spity.Terminal.Repositories;
using Spity.Terminal.Repositories.Entities;

namespace Spity.Terminal.ServiceProviders.Commands
{
    public sealed class SaveFeedbackCommand : IPostOneWay<FeedbackRequestObject>
    {
        private const int TextMaxLength = 500;
        private readonly FeedbackRepository _repository;

        public SaveFeedbackCommand(FeedbackRepository repository)
        {
            _repository = repository;
        }

        public void PostOneWay(FeedbackRequestObject request)
        {
            request.ToOption()
                   .Where(Validate)
                   .Map(x => Convert(x))
                   .Do(x => _repository.Save(x))
                   .ThrowOnEmpty<ArgumentException>();
        }

        private FeedbackEntity Convert(FeedbackRequestObject request)
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
            int endIndex = request.Image.IndexOf(searchString, 0, StringComparison.OrdinalIgnoreCase);
            result.Image = request.Image.Remove(0, endIndex + searchString.Length);
            return result;
        }

        private bool Validate(FeedbackRequestObject request)
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
