﻿using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using Nelibur.ServiceModel.Services;
using Nelibur.ServiceModel.Services.Operations;
using Ninject;
using NLog;
using Spity.Contracts;
using Spity.Terminal.Dependencies;
using Spity.Terminal.Properties;
using Spity.Terminal.ServiceProviders;
using Spity.Terminal.ServiceProviders.Commands;

namespace Spity.Terminal
{
    public sealed class ServiceBuilder : IDisposable
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IKernel kernel;
        private bool disposed;
        private CorsEnabledServiceHost<ServiceProvider> serviceProvider;

        public ServiceBuilder()
        {
            kernel = new StandardKernel(new ApplicationDependencyModule());
        }

        public void BuildApplication()
        {
            logger.Debug("Starting...");
            RegisterService();
            OpenCommunicationServices();
            logger.Info("Spity is running. Press <Esc> or <Enter> to stop.");
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            serviceProvider.Close();
            disposed = true;
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
            logger.Warn("Spity Service failed");
        }

        private void Bind<TRequest, TProcessor>(IConfiguration configuration)
            where TRequest : class
            where TProcessor : IRequestOperation
        {
            Func<TProcessor> processorFactory = () => kernel.Get<TProcessor>();
            configuration.Bind<TRequest, TProcessor>(processorFactory);
        }

        private void OpenCommunicationServices()
        {
            serviceProvider = new CorsEnabledServiceHost<ServiceProvider>(Settings.Default.ServiceAddress);
            serviceProvider.Faulted += ServiceProviderFaulted;
            serviceProvider.Open();
            logger.Info("{0} has been started", ServiceHostToString(serviceProvider));
        }

        private void RegisterService()
        {
            NeliburRestService.Configure(x => { Bind<FeedbackObject, SaveFeedbackCommand>(x); });
        }
    }
}