using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using Nelibur.ServiceModel.Services;
using Nelibur.ServiceModel.Services.Operations;
using Nelibur.Sword.Extensions;
using NLog;
using Spity.Contracts;
using Spity.Terminal.Properties;
using Spity.Terminal.ServiceProviders;
using Spity.Terminal.ServiceProviders.Commands;

namespace Spity.Terminal
{
    public sealed class ServiceBuilder : IDisposable
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private bool _disposed;
        private CorsEnabledServiceHost<ServiceProvider> _serviceProvider;

        public void BuildApplication()
        {
            _logger.Debug("Starting...");
            RegisterService();
            OpenCommunicationServices();
            _logger.Info("Spity is running. Press <Esc> or <Enter> to stop.");
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _serviceProvider.Close();
            _disposed = true;
        }

        private static string ServiceHostToString(ServiceHost host)
        {
            var stringBuilder = new StringBuilder();
            foreach (ServiceEndpoint serviceEndpoint in host.Description.Endpoints)
            {
                string message = string.Format("Service: {0} address: {1} binding: {2}",
                    host.Description.Name, serviceEndpoint.Address, serviceEndpoint.Binding.Name);
                stringBuilder.Append(message);
            }
            return stringBuilder.ToString();
        }

        private static void ServiceProviderFaulted(object sender, EventArgs e)
        {
            _logger.Warn("Spity Service failed");
        }

        private void Bind<TRequest, TProcessor>(IConfiguration configuration)
            where TRequest : class
            where TProcessor : IRequestOperation
        {
            Func<TProcessor> processorFactory = () => ServiceContainer.Container.Get<TProcessor>();
            configuration.Bind<TRequest, TProcessor>(processorFactory);
        }

        private void OpenCommunicationServices()
        {
            _serviceProvider = new CorsEnabledServiceHost<ServiceProvider>(Settings.Default.ServiceAddress);
            _serviceProvider.Description.Endpoints.Select(x => x.Binding as WebHttpBinding).Iter(x => x.MaxReceivedMessageSize = 2000000000);
            _serviceProvider.Faulted += ServiceProviderFaulted;
            _serviceProvider.Open();
            _logger.Info("{0} has been started", ServiceHostToString(_serviceProvider));
        }

        private void RegisterService()
        {
            NeliburRestService.Configure(x => { Bind<FeedbackObject, SaveFeedbackCommand>(x); });
        }
    }
}
