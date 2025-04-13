using Newtonsoft.Json;
using System.Text;
using TsedeyDemoProject.Utils;

namespace TsedeyDemoProject.Services
{
    public class ProcessRequest
    {
        public ProcessRequest() { }

        public async Task<Dictionary<string, object>> Validate(string url, string APIKEY, StringBuilder LogTracer)
        {
            var results = new Dictionary<string, object>();

            try
            {
                
                string payload = @"";

                results = await ProcessConfigs.postmenthod(url, payload, "GET", APIKEY, LogTracer);


                if (!results["Status"].Equals("000")) return results;


                var response = results["Data"].ToString();

                dynamic respStuff = JsonConvert.DeserializeObject<dynamic>(response);

                string dueAmount = respStuff["data"]["total_outstanding_fee"];

                Decimal amnt = Decimal.Parse(dueAmount);

                //if (amnt > 0)
                //{

                //    merchantResponse.Status = "000";
                //    merchantResponse.Message = respStuff["message"];
                //    merchantResponse.DueAmount = respStuff["data"]["total_outstanding_fee"];
                //    merchantResponse.AccountName = respStuff["data"]["school_bank"]["account_holder"];
                //    merchantResponse.AccountID = respStuff["data"]["school_bank"]["account_no"];
                //    merchantResponse.ReveiverName = respStuff["data"]["student"]["full_name"];
                //    merchantResponse.Reference = UniqueID;
                //    merchantResponse.Data = respStuff;
                //}
                //else
                //{
                //    merchantResponse.Status = "091";
                //    merchantResponse.Message = "Bill already paid";
                //    merchantResponse.Data = respStuff;
                //}

                results.Clear();

                results.Add("Status", "091");
                results.Add("Reference", "");
                results.Add("Message", "Invalid HttpMenthod");
                results.Add("Data", "");

                return results;
            }
            catch (Exception ex)
            {
                results.Add("Status", "091");
                results.Add("Reference", "");
                results.Add("Message", "Unable to process request at the moment try again later");
                results.Add("Data", "");

                return results;
            }
        }


        public async Task<Dictionary<string, object>> Payment(string url, string APIKEY, StringBuilder LogTracer)
        {
            var results = new Dictionary<string, object>();

            try
            {
                //requestUrl = requestUrl.Replace("{transno}", UniqueID).Replace("{account}", StudentNo);
                string payload = @"";

                results = await ProcessConfigs.postmenthod(url, payload, "GET", APIKEY, LogTracer);


                if (!results["Status"].Equals("000")) return results;


                var response = results["Data"].ToString();

                dynamic respStuff = JsonConvert.DeserializeObject<dynamic>(response);

                string dueAmount = respStuff["data"]["total_outstanding_fee"];

                Decimal amnt = Decimal.Parse(dueAmount);

                //if (amnt > 0)
                //{

                //    merchantResponse.Status = "000";
                //    merchantResponse.Message = respStuff["message"];
                //    merchantResponse.DueAmount = respStuff["data"]["total_outstanding_fee"];
                //    merchantResponse.AccountName = respStuff["data"]["school_bank"]["account_holder"];
                //    merchantResponse.AccountID = respStuff["data"]["school_bank"]["account_no"];
                //    merchantResponse.ReveiverName = respStuff["data"]["student"]["full_name"];
                //    merchantResponse.Reference = UniqueID;
                //    merchantResponse.Data = respStuff;
                //}
                //else
                //{
                //    merchantResponse.Status = "091";
                //    merchantResponse.Message = "Bill already paid";
                //    merchantResponse.Data = respStuff;
                //}

                results.Clear();

                results.Add("Status", "091");
                results.Add("Reference", "");
                results.Add("Message", "Invalid HttpMenthod");
                results.Add("Data", "");

                return results;
            }
            catch (Exception ex)
            {
                results.Add("Status", "091");
                results.Add("Reference", "");
                results.Add("Message", "Unable to process request at the moment try again later");
                results.Add("Data", "");

                return results;
            }
        }

    }
}
