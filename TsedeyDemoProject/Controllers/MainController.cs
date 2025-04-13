using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using TsedeyDemoProject.Models;
using TsedeyDemoProject.Services;
using TsedeyDemoProject.Utils;
using System.Xml;
using Microsoft.OpenApi.Validations;
using TsedeyDemoProject.Logs;

namespace TsedeyDemoProject.Controllers
{
    
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly IConfiguration config;
        public MainController(IConfiguration _configs) 
        { 
           this.config = _configs;
        }

        [Route("api/elmacore/dynamic/validate")]
        [HttpPost]

        public IActionResult Validate(MerchantRequest request)
        {
            StringBuilder LogTrace = new StringBuilder();

            MerchantResponse merchantResponse = new MerchantResponse();

            var result = new Dictionary<string, object>() { };

            ProcessRequest processRequest = new ProcessRequest();

            string? ApplicationID = config.GetSection("ServiceConfig").GetSection("ApplicationID").Value.ToString();

            string? Country = config.GetSection("ServiceConfig").GetSection("Country").Value.ToString();

            string? BankID = request.BankID;

            string? CustomerID = request.CustomerID;

            string? UniqueID = string.IsNullOrEmpty(request.UniqueID) ? Guid.NewGuid().ToString() : request.UniqueID;

            string? ConnectionString = string.IsNullOrEmpty(request.AppDetail?.ConnString) ? config["AppSettings:ConnectionString"] : request.AppDetail?.ConnString;

            string? FunctionName = request.FunctionName;
            string? ServiceName = request.MerchantConfig?.DLLCallID;
            string? Provider = request.MerchantConfig?.MerchantProvider;

            LogTrace.AppendLine(DateTime.Now.ToString().Replace("/", "-") + "\t" + "Service Start------------------");

            var req = JObject.FromObject(request);

            req["AppDetail"]["ConnString"] = "";

            LogTrace.AppendLine(DateTime.Now.ToString().Replace("/", "-") + "\t" + "Request Data" + "\t" + JsonConvert.SerializeObject(req));


            try
            {
                string AccessKey = string.Empty;
                string ValidationUrl = string.Empty;
                string PaymentUrl = string.Empty;

                #region Get Service Configuration
                try
                {
                    string ConfigXml = ProcessConfigs.GetConfig(LogTrace, ApplicationID, Country, BankID, CustomerID, UniqueID, ConnectionString, FunctionName, ServiceName, Provider);

                    if (string.IsNullOrEmpty(ConfigXml))
                    {
                        LogTrace.AppendLine(DateTime.Now.ToString().Replace("/", "-") + "\t" + "EXEPTION" + "\t" + "Could not retrive configuration XML");

                        merchantResponse.Status = "091";
                        merchantResponse.Message = "Config Not Found";

                        return Ok(merchantResponse);
                    }
                    else
                    {
                        LogTrace.AppendLine(DateTime.Now.ToString().Replace("/", "-") + "\t" + "CONFIG RESPONSE" + "\t" + "Found Configuration XML");
                    }

                    XmlDocument document = new XmlDocument();

                    document.LoadXml(ConfigXml);

                    string EndPoint = document.GetElementsByTagName("EndPoint")[0].InnerText;
                    string ActionPoint = document.GetElementsByTagName("Action")[0].InnerText;

                    XmlElement AccessKeyElt = document.SelectSingleNode("//Credential[@Name='AccessKey' and @Type='XmlBody']") as XmlElement;
                    XmlElement ValidationUrlElt = document.SelectSingleNode("//Credential[@Name='ValidationUrl' and @Type='XmlBody']") as XmlElement;
                    XmlElement PaymentUrlElt = document.SelectSingleNode("//Credential[@Name='PaymentUrl' and @Type='XmlBody']") as XmlElement;

                    AccessKey = AccessKeyElt.InnerText;
                    ValidationUrl = ValidationUrlElt.InnerText;
                    PaymentUrl = PaymentUrlElt.InnerText;
                }
                catch (Exception ex)
                {
                    LogTrace.AppendLine(DateTime.Now.ToString().Replace("/", "-") + "\t" + "EXCEPTION" + "\t" + ex.StackTrace);


                    merchantResponse.Status = "091";
                    merchantResponse.Message = "Invalid Config";

                    return Ok(merchantResponse);
                }
                #endregion

                #region Validation
                string? studentNo = request.PaymentDetails?.AccountID;

                if (string.IsNullOrEmpty(studentNo)) 
                {

                    merchantResponse.Status = "091";
                    merchantResponse.Message = "Student Account Id is required";

                    return Ok(merchantResponse);
                }

                ValidationUrl = ValidationUrl.Replace("{transno}", UniqueID).Replace("{account}", studentNo);

                result = processRequest.Validate(ValidationUrl,AccessKey, LogTrace).Result;

                if (result["Status"].ToString().Equals("000"))
                {
                    merchantResponse.Status = "000";
                    merchantResponse.Message = "Validation successful";

                    return Ok(merchantResponse);
                }
                else 
                {
                    merchantResponse.Status = result["Status"].ToString();

                    merchantResponse.Message = result["Message"].ToString();

                    return Ok(merchantResponse);
                }
                #endregion

            }
            catch (Exception ex) 
            {
                LogTrace.AppendLine(DateTime.Now.ToString().Replace("/", "-") + "\t" + "Exception" + "\t" + ex.ToString());

                merchantResponse.Status = "091";
                merchantResponse.Message = "Unable to process the request at the moment";

                return Ok(merchantResponse);
            }
            finally      
            {
                LogTrace.AppendLine(DateTime.Now.ToString().Replace("/", "-") + "\t" + "Service End------------------");


                Loggers.WriteTextLogs(ApplicationID, Country, BankID, UniqueID, CustomerID, "Logs", LogTrace.ToString(), config);
            }
        }


        [Route("api/elmacore/dynamic/payment")]
        [HttpPost]

        public IActionResult Payment(MerchantRequest request)
        {
            StringBuilder LogTrace = new StringBuilder();

            MerchantResponse merchantResponse = new MerchantResponse();

            var result = new Dictionary<string, object>() { };

            ProcessRequest processRequest = new ProcessRequest();

            string? ApplicationID = config.GetSection("ServiceConfig").GetSection("ApplicationID").Value.ToString();

            string? Country = config.GetSection("ServiceConfig").GetSection("Country").Value.ToString();

            string? BankID = request.BankID;

            string? CustomerID = request.CustomerID;

            string? UniqueID = string.IsNullOrEmpty(request.UniqueID) ? Guid.NewGuid().ToString() : request.UniqueID;

            string? ConnectionString = string.IsNullOrEmpty(request.AppDetail?.ConnString) ? config["AppSettings:ConnectionString"] : request.AppDetail?.ConnString;

            string? FunctionName = request.FunctionName;
            string? ServiceName = request.MerchantConfig?.DLLCallID;
            string? Provider = request.MerchantConfig?.MerchantProvider;

            string? TrxSource = request.AppDetail.TrxSource;

            LogTrace.AppendLine(DateTime.Now.ToString().Replace("/", "-") + "\t" + "Service Start------------------");

            var req = JObject.FromObject(request);

            req["AppDetail"]["ConnString"] = "";

            LogTrace.AppendLine(DateTime.Now.ToString().Replace("/", "-") + "\t" + "Request Data" + "\t" + JsonConvert.SerializeObject(req));


            try
            {
                string AccessKey = string.Empty;
                string ValidationUrl = string.Empty;
                string PaymentUrl = string.Empty;

                #region Get Service Configuration
                try
                {
                    string ConfigXml = ProcessConfigs.GetConfig(LogTrace, ApplicationID, Country, BankID, CustomerID, UniqueID, ConnectionString, FunctionName, ServiceName, Provider);

                    if (string.IsNullOrEmpty(ConfigXml))
                    {
                        LogTrace.AppendLine(DateTime.Now.ToString().Replace("/", "-") + "\t" + "EXEPTION" + "\t" + "Could not retrive configuration XML");

                        merchantResponse.Status = "091";
                        merchantResponse.Message = "Config Not Found";

                        return Ok(merchantResponse);
                    }
                    else
                    {
                        LogTrace.AppendLine(DateTime.Now.ToString().Replace("/", "-") + "\t" + "CONFIG RESPONSE" + "\t" + "Found Configuration XML");
                    }

                    XmlDocument document = new XmlDocument();

                    document.LoadXml(ConfigXml);

                    string EndPoint = document.GetElementsByTagName("EndPoint")[0].InnerText;
                    string ActionPoint = document.GetElementsByTagName("Action")[0].InnerText;

                    XmlElement AccessKeyElt = document.SelectSingleNode("//Credential[@Name='AccessKey' and @Type='XmlBody']") as XmlElement;
                    XmlElement ValidationUrlElt = document.SelectSingleNode("//Credential[@Name='ValidationUrl' and @Type='XmlBody']") as XmlElement;
                    XmlElement PaymentUrlElt = document.SelectSingleNode("//Credential[@Name='PaymentUrl' and @Type='XmlBody']") as XmlElement;

                    AccessKey = AccessKeyElt.InnerText;
                    ValidationUrl = ValidationUrlElt.InnerText;
                    PaymentUrl = PaymentUrlElt.InnerText;
                }
                catch (Exception ex)
                {
                    LogTrace.AppendLine(DateTime.Now.ToString().Replace("/", "-") + "\t" + "EXCEPTION" + "\t" + ex.StackTrace);


                    merchantResponse.Status = "091";
                    merchantResponse.Message = "Invalid Config";

                    return Ok(merchantResponse);
                }
                #endregion

                #region Payment
                string? transno = request.InfoFields?.InfoField8;

                if (string.IsNullOrEmpty(transno))
                {

                    merchantResponse.Status = "091";
                    merchantResponse.Message = "transaction Id is required";

                    return Ok(merchantResponse);
                }

                PaymentUrl = PaymentUrl.Replace("{transno}", transno).Replace("{trxsource}", TrxSource);

                result = processRequest.Payment(PaymentUrl, AccessKey, LogTrace).Result;

                if (result["Status"].ToString().Equals("000"))
                {
                    merchantResponse.Status = "000";
                    merchantResponse.Message = "Payment successful";

                    return Ok(merchantResponse);
                }
                else
                {
                    merchantResponse.Status = result["Status"].ToString();

                    merchantResponse.Message = result["Message"].ToString();

                    return Ok(merchantResponse);
                }
                #endregion

            }
            catch (Exception ex)
            {
                LogTrace.AppendLine(DateTime.Now.ToString().Replace("/", "-") + "\t" + "Exception" + "\t" + ex.ToString());

                merchantResponse.Status = "091";
                merchantResponse.Message = "Unable to process the request at the moment";

                return Ok(merchantResponse);
            }
            finally
            {
                LogTrace.AppendLine(DateTime.Now.ToString().Replace("/", "-") + "\t" + "Service End------------------");


                Loggers.WriteTextLogs(ApplicationID, Country, BankID, UniqueID, CustomerID, "Logs", LogTrace.ToString(), config);
            }
        }
    }
}
