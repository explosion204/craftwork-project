using System.Threading.Tasks;
using CraftworkProject.Services.Interfaces;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace CraftworkProject.Services.Implementations
{
    public class SmsService : ISmsService
    {
        private readonly string _sender;
        private readonly string _accountSid;
        private readonly string _authToken;

        public SmsService(string sender, string accountSid, string authToken)
        {
            _sender = sender;
            _accountSid = accountSid;
            _authToken = authToken;
        }
        
        public async Task<bool> SendAsync(string receiver, string body)
        {
            TwilioClient.Init(_accountSid, _authToken);

            try
            {
                await MessageResource.CreateAsync(
                    body: body,
                    from: new PhoneNumber(_sender),
                    to: new PhoneNumber(receiver)
                );

                return true;
            }
            catch (ApiException)
            {
                return false;
            }
        }
    }
}