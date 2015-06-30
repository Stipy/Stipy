using System;
using Nelibur.ServiceModel.Services.Operations;
using Nelibur.Sword.Extensions;
using Spity.Contracts;
using Spity.Terminal.Repositories;

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
                   .Do(x => _repository.Save(x))
                   .ThrowOnEmpty<ArgumentException>();
        }

        private bool Validate(FeedbackObject request)
        {
            if (request.IsNull())
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(request.Text) && request.Image == null)
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
