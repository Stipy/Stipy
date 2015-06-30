using System;
using Ninject.Modules;
using Spity.Terminal.Properties;
using Spity.Terminal.Repositories;

namespace Spity.Terminal.Dependencies
{
    public sealed class ApplicationDependencyModule : NinjectModule
    {
        public override void Load()
        {
            RegisterConnectionFactory();
            RegisterRepositories();
        }

        private void RegisterConnectionFactory()
        {
            var connectionFactory = new ConnectionFactory(Settings.Default.MongoDbConnectionString);
            Bind<ConnectionFactory>().ToConstant(connectionFactory).InSingletonScope();
        }

        private void RegisterRepositories()
        {
            Bind<FeedbackRepository>().To<FeedbackRepository>().InSingletonScope();
        }
    }
}
