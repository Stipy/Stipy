using System;
using System.Collections.Specialized;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;
using Nelibur.ServiceModel.Contracts;
using Nelibur.ServiceModel.Services;
using Nelibur.ServiceModel.Services.Operations;
using Nelibur.Sword.DataStructures;
using Newtonsoft.Json;
using NLog;
using Spity.Contracts;
using Spity.Terminal.ServiceProviders.Commands;

namespace Spity.Terminal.ServiceProviders
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class ServiceProvider : IServiceProvider
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public Message Get(Message message)
        {
            return Process(() => NeliburRestService.Process(message));
        }

        public void PostOneWay(Message message)
        {
            string content = GetMessageContent(message);
            ProcessRequest(content);
        }

        private static TResult Process<TResult>(Func<TResult> action)
        {
            try
            {
                return action();
            }
            catch (ArgumentException ex)
            {
                _logger.Error(ex);
                throw new WebFaultException(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw new WebFaultException(HttpStatusCode.InternalServerError);
            }
        }

        private string GetMessageContent(Message message)
        {
            MessageBuffer buffer = message.CreateBufferedCopy(int.MaxValue);
            Message copy = buffer.CreateMessage();
            XmlDictionaryReader bodyReader = copy.GetReaderAtBodyContents();
            bodyReader.ReadStartElement("Binary");
            byte[] bodyBytes = bodyReader.ReadContentAsBase64();
            return Encoding.UTF8.GetString(bodyBytes);
        }

        private Option<string> GetRequestType()
        {
            UriTemplateMatch templateMatch = WebOperationContext.Current.IncomingRequest.UriTemplateMatch;
            NameValueCollection queryParams = templateMatch.QueryParameters;
            foreach (string key in queryParams.AllKeys)
            {
                if (string.Equals(key, RestServiceMetadata.ParamName.Type, StringComparison.OrdinalIgnoreCase))
                {
                    return new Option<string>(queryParams[key].Trim());
                }
            }
            return Option<string>.Empty;
        }

        private bool IsSuppoertedRequest<T>(string requestTypeName)
        {
            return string.Equals(requestTypeName, typeof(T).Name, StringComparison.OrdinalIgnoreCase);
        }

        private void ProcessRequest(string requestData)
        {
            Option<string> request = GetRequestType();
            if (request.HasNoValue)
            {
                throw new WebFaultException(HttpStatusCode.BadRequest);
            }
            if (IsSuppoertedRequest<FeedbackObject>(request.Value))
            {
                ProcessRequest<FeedbackObject, SaveFeedbackCommand>(requestData);
            }
        }

        private void ProcessRequest<TRequest, TProcessor>(string requestData)
            where TRequest : class
            where TProcessor : IPostOneWay<TRequest>
        {
            var request = JsonConvert.DeserializeObject<TRequest>(requestData);
            Func<TProcessor> processor = () => ServiceContainer.Container.Get<TProcessor>();
            processor().PostOneWay(request);
        }
    }
}
