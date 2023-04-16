using MathNet.Numerics.LinearAlgebra.Factorization;
using Microsoft.Extensions.Configuration;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Utility
{
    public class AuthUtil
    {
        string mBaseUrl = string.Empty;
        string account = string.Empty;
        string password = string.Empty;
        string appID = string.Empty;
        string accessToken = string.Empty;

        private readonly HttpClient _httpClient;

        public AuthUtil(HttpClient httpClient)
        {
            _httpClient = httpClient;

            var builder = new ConfigurationBuilder().AddJsonFile(@"appsettings.json");
            IConfiguration config = builder.Build();
            //mBaseUrl = "https://pip.moi.gov.tw/asmx/WS1.asmx?op=GetG5";
            mBaseUrl = "https://ap2.pccu.edu.tw:8888/AssociatesService/Authen/Authentication.asmx?op=CheckIDAndRole2";
            //mBaseUrl = "https://ap2.pccu.edu.tw:8888/AssociatesService/Authen/Authentication.asmx?op=CheckIDWithDetail2";
            account = config.GetValue<string>("SSOAuthSetting:account");
            password = config.GetValue<string>("SSOAuthSetting:password");
            appID = config.GetValue<string>("SSOAuthSetting:appID");
            accessToken = config.GetValue<string>("SSOAuthSetting:accessToken");
        }

        public bool ChkAccountData()
        {
            string strRtn = string.Empty;
            string strReqBody = string.Empty;
            try
            {
                strReqBody = GenRequestBody();

            }
            catch (Exception e)
            {
                return false;
            }

            try
            {
                var req = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(mBaseUrl),
                    Content = new StringContent(strReqBody, System.Text.Encoding.UTF8, "text/xml")
                };

                var resp = _httpClient.SendAsync(req).GetAwaiter().GetResult();
                var content = resp.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                string mRawOut = content;
            }
            catch (WebException webex)
            {

            }

            return true;
        }


        private string GenRequestBody()
        {
            string str = string.Empty;

            str = $@"<?xml version=""1.0"" encoding=""utf-8""?>
                    <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                      <soap:Body>
                        <CheckIDAndRole2 xmlns=""http://pccu.edu.tw/"">
                          <account>{account}</account>
                          <password>{password}</password>
                          <appID>{appID}</appID>
                          <accessToken>{accessToken}</accessToken>
                        </CheckIDAndRole2>
                      </soap:Body>
                    </soap:Envelope>";

//            str = @"<?xml version=""1.0"" encoding=""utf-8""?>
//<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
//  <soap:Body>
//    <GetG5 xmlns=""https://pip.moi.gov.tw/"">
//      <Year>103</Year>
//      <City>新北市</City>
//      <DisputeOrigin></DisputeOrigin>
//      <DisputeCause></DisputeCause>
//    </GetG5>
//  </soap:Body>
//</soap:Envelope>";

            return str;
        }
    }
}
