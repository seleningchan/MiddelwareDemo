using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiddelwareDemo
{
    public class DbConnection { }
    public class DbContext
    {
        public DbConnection Connection { get; }
        public DbContext(DbConnection dbConnection)
        {
            Connection = dbConnection;
        }
    }
    public class ServiceDescriptor
    {
        public Type ServiceType { get; }
        public Type ImplementionType { get; }
        public object? Instance { get; }
        public ServiceDescriptor(Type serviceType, Type implementionType, object? instance=null)
        {
            ServiceType = serviceType;
            ImplementionType = implementionType;
            Instance = instance;
        }
    }

    public interface IContainer
    {
        object? GetService(Type serviceType);
    }
    public interface IContainerBuilder
    {
        void Add(ServiceDescriptor serviceDescriptor);
        IContainer Build();
    }
    public class Container : IContainer
    {
        private IEnumerable<ServiceDescriptor> _services;
        public Container(IEnumerable<ServiceDescriptor> services)
        {
            _services = services;
        }

        public object GetService(Type serviceType)
        {
            var descriptor = _services.FirstOrDefault(a => a.ServiceType == serviceType);
            if (descriptor == null)
            {
                throw new InvalidOperationException("服务未注册");
            }
            var invokerType = typeof(Func<IContainer, object>);
            if (typeof(Func<IContainer, object>).IsInstanceOfType(descriptor.Instance))
            {
                var func = descriptor.Instance as Func<IContainer, object> ?? throw new ArgumentNullException();
                return func(this);
            }
            var constructor = serviceType.GetConstructors()
                .OrderByDescending(a => a.GetParameters().Length)
                .FirstOrDefault() ?? throw new ArgumentNullException();
            var parameters = constructor.GetParameters()
                .Select(s => GetService(s.ParameterType));
            return Activator.CreateInstance(descriptor.ImplementionType, parameters.ToArray());
        }
    }

    public class ContainerBuilder : IContainerBuilder
    {
        private List<ServiceDescriptor> _services = new();
        public void Add(ServiceDescriptor serviceDescriptor)
        {
            _services.Add(serviceDescriptor);
        }

        public IContainer Build()
        {
            return new Container(_services);
        }
    }

    public static class IContainerBuilderExtensions
    {
        public static void Add<TService>(this IContainerBuilder builder)
            where TService : class
        {
            builder.Add(new ServiceDescriptor(typeof(TService), typeof(TService)));
        }
        public static void Add<Tservice,TImplement>(this IContainerBuilder builder)
        {
            builder.Add(new ServiceDescriptor(typeof(Tservice), typeof(TImplement)));
        }
        public static void Add<TService>(this IContainerBuilder builder, 
            Func<IContainer, TService> func)
        {
            builder.Add(new ServiceDescriptor(typeof(TService),
                typeof(Action<IContainer, TService>), func));
        }
    }
}
