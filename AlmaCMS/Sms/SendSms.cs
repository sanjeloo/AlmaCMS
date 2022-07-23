using Kavenegar;
using SmsIrRestful;
using System;
using System.Collections.Generic;

namespace AlmaCMS.Sms
{

    public static class SendSms
    {
        public static int SendVerification(string receptor , string code)
        {
            try
            {
                var api = new KavenegarApi("6A5968516B63326F34433763567A6E424C38657039686A5555343944494A345A724F314C6C746D7A6A6D453D");
                var result = api.VerifyLookup(receptor, code, "Verification");
                return result.Status;
            }
            catch (Exception ex)
            {
                Console.Write("Message : " + ex.Message);
                return -1;
            }
        }
        public static int Send(string receptor , string message)
        {
            try
            {
                var api = new KavenegarApi("6A5968516B63326F34433763567A6E424C38657039686A5555343944494A345A724F314C6C746D7A6A6D453D");
                var result = api.Send("2000500666", receptor, message);
                return result.Status;
            }
            catch (Exception ex)
            {
                Console.Write("Message : " + ex.Message);
                return -1;
            }
        }
    }
}
