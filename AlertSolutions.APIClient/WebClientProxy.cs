using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AlertSolutions.API
{
    public interface IWebClientProxy
    {
        string UploadString(string location, string xml, string header = "Content-Type: text/xml");
        string DownloadString(string location, string header = "Content-Type: text/xml");
    }

    public class WebClientProxy : IWebClientProxy
    {

        public string UploadString(string location, string xml, string header = "Content-Type: text/xml")
        {
            using (var webClient = new WebClient())
            {
                        webClient.Headers.Add(header);
                        return webClient.UploadString(location, xml);
            }
        }

        public string DownloadString(string location, string header = "Content-Type: text/xml")
        {
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add(header);
                return webClient.DownloadString(location);
            }
        }
    }
}
