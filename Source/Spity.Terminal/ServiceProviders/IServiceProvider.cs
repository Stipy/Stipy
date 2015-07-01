using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using Nelibur.ServiceModel.Contracts;
using Nelibur.ServiceModel.Services.Operations;
using Spity.Terminal.ServiceProviders.Cors;

namespace Spity.Terminal.ServiceProviders
{
    [ServiceContract]
    public interface IServiceProvider
    {
        [OperationContract, CorsEnabled]
        [WebInvoke(Method = OperationType.Get, RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, UriTemplate = RestServiceMetadata.Path.Get)]
        Message Get(Message message);

        [OperationContract, CorsEnabled]
        [WebInvoke(Method = OperationType.Post, RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, UriTemplate = RestServiceMetadata.Path.PostOneWay)]
        void PostOneWay(Message message);
    }
}
