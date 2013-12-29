using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funq;
using ServiceStack;

namespace AlertSolutions.API
{
    public class ApiClientResolver
    {
        private static ApiClientResolver instance = new ApiClientResolver();
        private Container container;

        public static ApiClientResolver Instance {
            get { return instance; }
        }

        public Container Container {
            get { return this.container; }
        }

        private ApiClientResolver()
        {
            Init();
        }

        private void Init()
        {
            this.container = new Container();
            Register(this.container);
        }

        public void Register(Container container)
        {
            container.RegisterAutoWiredAs<WebClientProxy, IWebClientProxy>().ReusedWithin(ReuseScope.Request);
        }

        public void Reset()
        {
            Init();
        }

    }
}
