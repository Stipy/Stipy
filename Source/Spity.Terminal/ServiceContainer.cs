using System;
using Ninject;
using Spity.Terminal.Dependencies;

namespace Spity.Terminal
{
    public sealed class ServiceContainer
    {
        private static readonly ServiceContainer _serviceContainer = new ServiceContainer();
        private readonly IKernel _kernel = new StandardKernel(new ApplicationDependencyModule());

        private ServiceContainer()
        {
        }

        public static ServiceContainer Container
        {
            get { return _serviceContainer; }
        }

        public T Get<T>()
        {
            return _kernel.Get<T>();
        }
    }
}
