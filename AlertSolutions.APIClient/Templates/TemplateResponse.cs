using System;
using System.Linq;

namespace AlertSolutions.API.Templates
{
    public class TemplateResponse
    {
        public int TemplateId { get; set; }
        public RequestResultType Status { get; set; }
        public string Error { get; set; }
    }
}