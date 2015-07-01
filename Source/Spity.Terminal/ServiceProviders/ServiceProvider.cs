using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using Nelibur.ServiceModel.Services;
using NLog;

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
            Process(() => NeliburRestService.ProcessOneWay(message));
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

        private static void Process(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw new WebFaultException(HttpStatusCode.InternalServerError);
            }
        }
    }
}
