using System;
using Nelibur.ServiceModel.Services.Operations;
using Spity.Contracts;
using Spity.Terminal.Repositories;

namespace Spity.Terminal.ServiceProviders.Queries
{
    public sealed class GetFeedbacksQuery : IGet<GetFeedbacksRequestObject>
    {
        private readonly FeedbackRepository _repository;

        public GetFeedbacksQuery(FeedbackRepository repository)
        {
            _repository = repository;
        }

        public object Get(GetFeedbacksRequestObject request)
        {
            return _repository.GetAll();
        }
    }
}
