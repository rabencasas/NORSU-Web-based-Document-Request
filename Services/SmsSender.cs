using System;
using System.Net;
using System.Collections.Specialized;

namespace NGODP.Services
{
    public class SmsSender : ISmsSender
    {
        public SmsSender()
		{
		}

		public object SendMessage(string Number, string Message, string Code)
		{
			object functionReturnValue = null;
			
			using (WebClient client = new WebClient()) {
				
				NameValueCollection parameter = new NameValueCollection();
				
				string url = "https://www.itexmo.com/php_api/api.php";

				parameter.Add("1", Number);
				parameter.Add("2", Message);
				parameter.Add("3", Code);

				dynamic rpb = client.UploadValues(url, "POST", parameter);

				functionReturnValue = (new System.Text.UTF8Encoding()).GetString(rpb);
			}

			return functionReturnValue;
		}
    }
}
