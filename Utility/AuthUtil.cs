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
using System.Runtime.Intrinsics.X86;
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


        public AuthUtil()
        {
            var builder = new ConfigurationBuilder().AddJsonFile(@"appsettings.json");
            IConfiguration config = builder.Build();
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


            //    ServiceReference1.AuthenticationSoapClient soap =
            //new ServiceReference1.AuthenticationSoapClient(ServiceReference1.AuthenticationSoapClient.EndpointConfiguration.AuthenticationSoap12);
            //    var result = await soap.CheckIDWithDetail2Async("testing", "", "Pintech", "6C5EA5C1-F6EB-4D4A-984E-4B209C62EB8B");
            //    ServiceReference1.ResultObjectWithPersonalData2 data = result.Body.CheckIDWithDetail2Result;
            //    SSO.getIdentitySoapClient soap2 = new SSO.getIdentitySoapClient(SSO.getIdentitySoapClient.EndpointConfiguration.getIdentitySoap12);
            //    var result2 = await soap2.getComplexIdentityAsync("123456789", "BE94A7D8-76D9-428B-BC10-50CF17819C9F");

            //    string RtnData = result2.Body.getComplexIdentityResult.JSONData;


                //var req = new HttpRequestMessage
                //{
                //    Method = HttpMethod.Post,
                //    RequestUri = new Uri(mBaseUrl),
                //    Content = new StringContent(strReqBody, System.Text.Encoding.UTF8, "text/xml")
                //};


                //var resp = _httpClient.SendAsync(req).GetAwaiter().GetResult();
                //var content = resp.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                //string mRawOut = content;
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
