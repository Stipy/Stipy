using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using Nelibur.ServiceModel.Contracts;
using Spity.Terminal.ServiceProviders.Cors;

namespace Spity.Terminal.ServiceProviders
{
    [ServiceContract]
    public interface IServiceProvider
    {
        [OperationContract, CorsEnabled]
        [WebGet(RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, UriTemplate = RestServiceMetadata.Path.Get)]
        Message Get(Message message);

        [OperationContract, CorsEnabled]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, UriTemplate = RestServiceMetadata.Path.PostOneWay)]
        void PostOneWay(Message message);
    }
}
