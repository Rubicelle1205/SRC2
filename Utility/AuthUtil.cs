using Microsoft.Extensions.Configuration;

namespace Utility
{

    public class AuthUtil
    {
        string SSOaccessToken = string.Empty;
        string StdaccessToken = string.Empty;

        SSOAuth.getIdentitySoapClient SSOsoap = new SSOAuth.getIdentitySoapClient(SSOAuth.getIdentitySoapClient.EndpointConfiguration.getIdentitySoap12);
        StdSevice.ServiceSoapClient Stdsoap = new StdSevice.ServiceSoapClient(StdSevice.ServiceSoapClient.EndpointConfiguration.ServiceSoap12);

        public AuthUtil()
        {
            var builder = new ConfigurationBuilder().AddJsonFile(@"appsettings.json");
            IConfiguration config = builder.Build();
            SSOaccessToken = config.GetValue<string>("SSOAuthSetting:accessToken");
            StdaccessToken = config.GetValue<string>("StdService:accessToken");
        }

        public async Task<SSOAuth.clsJSONResult> GetSSOAuthData(string Guid)
        {
            var Result = await SSOsoap.getComplexIdentityAsync(Guid, SSOaccessToken);

            SSOAuth.clsJSONResult sResultData = Result.Body.getComplexIdentityResult;

            return sResultData;
        }

        public async Task<bool> ChkStudent(string StudentNo)
        {
            var Result = await Stdsoap.isMyStudentAsync(StdaccessToken, StudentNo);

            bool BlnRtn = Result;

            return BlnRtn;
        }
    }
}
