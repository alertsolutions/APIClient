using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AlertSolutions.API
{
    // copied from here:
    //http://ayende.com/blog/2886/building-an-ioc-container-in-15-lines-of-code

    public class Container
    {
        public delegate object Resolver(Container container);

        private readonly Dictionary<string, object> _configuration
                       = new Dictionary<string, object>();
        private readonly Dictionary<Type, Resolver> _typeToCreator
                       = new Dictionary<Type, Resolver>();

        public Dictionary<string, object> Configuration
        {
            get { return _configuration; }
        }

        public void Register<T>(Resolver resolver)
        {
            // added this check/removal bit so tests could register mock objects
            // not sure what drawbacks there are to doing this
            if (_typeToCreator.ContainsKey(typeof(T)))
            {
                _typeToCreator.Remove(typeof(T));
            }

            _typeToCreator.Add(typeof(T), resolver);
        }

        public T Resolve<T>()
        {
            return (T)_typeToCreator[typeof(T)](this);
        }

        public T GetConfiguration<T>(string name)
        {
            return (T)_configuration[name];
        }
    }
}
