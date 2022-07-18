using SmsIrRestful;
using System;
using System.Collections.Generic;

namespace AlmaCMS.Sms
{

    public class SendSms
    {
        private readonly string SecurityCode = "sdfguf654sdffyadtfss";
        private readonly string ApiKey = "7b5fb5724ccebd463bc3be17";
        private string GetToken()
        {
            Token tk = new Token();
            string result = tk.GetToken(ApiKey, SecurityCode);
            return result;
        }
        public bool Send(string phone, string message)
        {
            try
            {
                var token = GetToken();

                var messageSendObject = new MessageSendObject()
                {
                    Messages = new List<string> { message }.ToArray(),
                    MobileNumbers = new List<string> { phone }.ToArray(),
                    LineNumber = "30004005330330",
                    SendDateTime = null,
                    CanContinueInCaseOfError = true
                };

                MessageSendResponseObject messageSendResponseObject = new MessageSend().Send(token, messageSendObject);

                return messageSendResponseObject.IsSuccessful;
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
