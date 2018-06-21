using Simplic.Configuration;
using System;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Xml;

namespace Simplic.Sms.Service.Provider.Lox24
{
    public class Lox24Provider : ISmsProvider
    {
        #region Private Fields
        private string apiUrl;
        /// <summary>
        /// Lox24 Client ID (same as Login ID)
        /// </summary>
        private string konto;
        
        /// <summary>
        /// MD5 Hash of the account password
        /// Example: The MD5-Hash of „Passwort“ is: 3e45af4ca27ea2b03fc6183af40ea112
        /// </summary>
        private string password;

        /// <summary>
        /// Service ID of the sms Type you want to use. You can
        /// find the value in your user account’s dashboard. After
        /// registration it can take 24 hours till you see the service-IDs                
        /// </summary>
        private string service;

        /// <summary>
        /// Defines if you want to send a normal GSM 03.38
        /// SMS(160 character) or a Unicode text to transfer
        /// characters like Cyrillic, Arabic, Chinese and Japanese (70 character).
        /// GSM: encoding=0
        /// Unicode: encoding=1
        /// </summary>
        private string encoding;

        /// <summary>
        /// Unix-timestamp of a scheduled delivery. If 0 SMS is delivered immediately.
        /// </summary>
        private string timestamp;

        /// <summary>
        /// With this variable, the response format is specified.
        /// With 'text', the answer is as multiline text answer given
        /// back. 'xml' provides an XML schema.
        /// </summary>
        private string _return;

        /// <summary>
        /// If http head is set to 1, so errors are reported in the
        /// HTTP header(other than 200). If set to 0, the error is
        /// only listed in text/xml response.
        /// </summary>
        private string httphead;
        
        /// <summary>
        /// Parameter can be set to 'send' or 'info'. The value 'send'
        /// transmits the SMS to the gateway for delivery. 'info' is
        /// used to test the api response without sending a real short message.
        /// </summary>       
        private string action;

        /// <summary>
        /// Sender-ID of the message. Can be a number (15 digits) or a text (11 character).
        /// </summary>
        private string from;

        private readonly IConfigurationService configurationService;
        #endregion

        #region Settings        
        private const string SettingsPluginName = "Lox24SmsProvider";         
        private const string Lox24ApiUrl = "api-url";        
        private const string Lox24Konto = "konto";        
        private const string Lox24Password = "password";
        private const string Lox24Service = "service";        
        private const string Lox24Encoding = "encoding";
        private const string Lox24From = "from";               
        private const string Lox24Timestamp = "timestamp";        
        private const string Lox24Return = "return";
        private const string Lox24Httphead = "httphead";
        private const string Lox24Action = "action";
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Lox24Provider(IConfigurationService configurationService)
        {
            this.configurationService = configurationService;

            konto = GetSetting(Lox24Konto);
            password = CalculateMD5Hash(GetSetting(Lox24Password));
            service = GetSetting(Lox24Service);
            encoding = GetSetting(Lox24Encoding);
            timestamp = GetSetting(Lox24Timestamp);
            _return = GetSetting(Lox24Return);
            httphead = GetSetting(Lox24Httphead);
            action = GetSetting(Lox24Action);
            apiUrl = GetSetting(Lox24ApiUrl);
            from = GetSetting(Lox24From);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Sends an sms using a provider (lox24 is default) 
        /// </summary>
        /// <param name="number">Phone number</param>
        /// <param name="body">Sms text</param>
        /// <returns>True if successfull</returns>
        public object SendSms(string number, string body)
        {
            /* 
                Example numbers
                
                 +491701234567 (+ must be urlencoded)
                 00491701234567
                 491701234567               
            */

            var loxUrl = BuildUrl(number, body);

            using (var httpClient = new HttpClient())
            {
                var responseString = httpClient.GetStringAsync(loxUrl);

                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(responseString.Result);

                var returnCode = xmlDoc.SelectSingleNode("answer/code");
                var returnText = xmlDoc.SelectSingleNode("answer/info");

                if (returnCode != null && returnText != null)
                {
                    return new Lox24Result
                    {
                        StatusCode = (Lox24ResultStatus)Convert.ToInt32(returnCode.InnerText),
                        ResultText = returnText.InnerXml
                    };
                }
                else
                    return null;
            }
        }
        #endregion

        #region Private Methods

        #region BuildUrl
        /// <summary>
        /// Creates a proper URL for lox24
        /// </summary>
        /// <param name="text">SMS Text</param>
        /// <param name="to">Phone number to send an sms to</param>
        /// <returns>LOX 24 URL</returns>
        private string BuildUrl(string to, string text)
        {
            var sb = new StringBuilder();
            sb.Append(apiUrl);
            sb.Append($"?konto={konto}");
            sb.Append($"&password={password}");
            sb.Append($"&service={service}");
            sb.Append($"&text={HttpUtility.UrlEncode(text)}");
            sb.Append($"&encoding={encoding}");
            sb.Append($"&from={from}");
            sb.Append($"&to={to}");
            sb.Append($"&timestamp={timestamp}");
            sb.Append($"&return={_return}");
            sb.Append($"&httphead={httphead}");
            sb.Append($"&action={action}");

            return sb.ToString();
        }
        #endregion

        #region CalculateMD5Hash
        /// <summary>
        /// Generates an MD5 Hash
        /// </summary>
        /// <param name="input">Text to hash</param>
        /// <returns>Hashed MD5</returns>
        private string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input

            var md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }

            return sb.ToString();
        }
        #endregion

        #region GetSetting
        /// <summary>
        /// Reads a setting value from db
        /// </summary>
        /// <param name="key">Setting key</param>
        /// <returns>Setting value</returns>
        private string GetSetting(string key)
        {
            return configurationService.GetValue<string>(key, SettingsPluginName, "");
        } 
        #endregion

        #endregion
    }
}
