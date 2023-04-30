using Microsoft.Extensions.Configuration;

namespace Utility
{

    public class AuthUtil
    {
        //string account = string.Empty;
        //string password = string.Empty;
        //string appID = string.Empty;
        string accessToken = string.Empty;

        SSOAuth.getIdentitySoapClient soap = new SSOAuth.getIdentitySoapClient(SSOAuth.getIdentitySoapClient.EndpointConfiguration.getIdentitySoap12);

        public AuthUtil()
        {
            var builder = new ConfigurationBuilder().AddJsonFile(@"appsettings.json");
            IConfiguration config = builder.Build();
            //account = config.GetValue<string>("SSOAuthSetting:account");
            //password = config.GetValue<string>("SSOAuthSetting:password");
            //appID = config.GetValue<string>("SSOAuthSetting:appID");
            accessToken = config.GetValue<string>("SSOAuthSetting:accessToken");
        }

        public async Task<SSOAuth.clsJSONResult> GetSSOAuthData(string Guid)
        {
            var Result = await soap.getComplexIdentityAsync(Guid, accessToken);

            SSOAuth.clsJSONResult sResultData = Result.Body.getComplexIdentityResult;

            return sResultData;
        }


    }
}
