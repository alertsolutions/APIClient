using System;
using System.Linq;

namespace AlertSolutions.API
{
    public class ApiContact
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string SmsPhone { get; set; }
        public string VoicePhone { get; set; }
        public string FaxPhone { get; set; }

        public const string NAME_HEADER = "Name";
        public const string EMAIL_HEADER = "Email";
        public const string SMS_HEADER = "SmsPhone";
        public const string VOICE_HEADER = "VoicePhone";
        public const string FAX_HEADER = "FaxPhone";
    }
}
