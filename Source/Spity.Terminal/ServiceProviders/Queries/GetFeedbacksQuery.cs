using System;
using Nelibur.ServiceModel.Services.Operations;
using Spity.Terminal.Repositories;

namespace Spity.Terminal.ServiceProviders.Queries
{
    public sealed class GetFeedbacksQuery : IGet<GetFeedbacksQuery>
    {
        private readonly FeedbackRepository _repository;

        public GetFeedbacksQuery(FeedbackRepository repository)
        {
            _repository = repository;
        }

        public object Get(GetFeedbacksQuery request)
        {
            return _repository.GetAll();
        }
    }
}
