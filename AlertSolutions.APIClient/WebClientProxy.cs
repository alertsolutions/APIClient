using System.Net;

namespace AlertSolutions.API
{
    public interface IWebClientProxy
    {
        string UploadString(string location, string xml, string header = "Content-Type: text/xml; charset=UTF-8");
        string DownloadString(string location, string header = "Content-Type: text/xml; charset=UTF-8");
    }

    public class WebClientProxy : IWebClientProxy
    {

        public string UploadString(string location, string xml, string header = "Content-Type: text/xml; charset=UTF-8")
        {
            using (var webClient = new WebClient())
            {
                webClient.Encoding = System.Text.Encoding.UTF8;
                webClient.Headers.Add(header);
                return webClient.UploadString(location, xml);
            }
        }

        public string DownloadString(string location, string header = "Content-Type: text/xml; charset=UTF-8")
        {
            using (var webClient = new WebClient())
            {
                webClient.Encoding = System.Text.Encoding.UTF8;
                webClient.Headers.Add(header);
                return webClient.DownloadString(location);
            }
        }
    }
}
