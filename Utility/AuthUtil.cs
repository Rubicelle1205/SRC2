using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Crypto;

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
            SSOAuth.getComplexIdentityResponse Result = new SSOAuth.getComplexIdentityResponse();
            SSOAuth.clsJSONResult sResultData = new SSOAuth.clsJSONResult();

            switch (Guid)
            {
                case "staff_test":
                    sResultData.JSONData = "{\"Account\":\"B123456\",\"Role\":\"staff\",\"Name\":\"陳大叔\",\"Department\":\"課外組\"}";
                    sResultData.bError = false;
                    sResultData.bResultIsValid = true;
                    break;
                case "student_test":
                    sResultData.JSONData = "{\"Account\":\"A2345678\",\"Role\":\"student\",\"Name\":\"王小明\",\"Department\":\"造船系2A\"}";
                    sResultData.bError = false;
                    sResultData.bResultIsValid = true;
                    break;
                case "teacher_test":
                    sResultData.JSONData = "{\"Account\":\"B234567\",\"Role\":\"teacher\",\"Name\":\"李大姐\",\"Department\":\"俄文系\"}";
                    sResultData.bError = false;
                    sResultData.bResultIsValid = true;
                    break;
                case "supervisor_teacher":
                    sResultData.JSONData = "{\"Account\":\"P10316871\",\"Role\":\"teacher\",\"Name\":\"張書飄\",\"Department\":\"課外活動組\"}";
                    sResultData.bError = false;
                    sResultData.bResultIsValid = true;
                    break;
                case "supervisor_staff":
                    sResultData.JSONData = "{\"Account\":\"P10316871\",\"Role\":\"staff\",\"Name\":\"張書飄\",\"Department\":\"課外活動組\"}";
                    sResultData.bError = false;
                    sResultData.bResultIsValid = true;
                    break;
                default:
                    Result = await SSOsoap.getComplexIdentityAsync(Guid, SSOaccessToken);
                    sResultData = Result.Body.getComplexIdentityResult;
                    break;
            }

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
