using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Net.Security;
using System.Text;

namespace TsedeyDemoProject.Utils
{
    public class ProcessConfigs
    {
        public ProcessConfigs() { }

        public static async Task<Dictionary<string, object>> postmenthod(string url, string payload, string httpMethod, string token, StringBuilder LogTracer)
        {

            #region Certificates
            ServicePointManager.ServerCertificateValidationCallback = new
                   RemoteCertificateValidationCallback
                   (
                      delegate { return true; }
                   );

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls13 | SecurityProtocolType.Tls;
            #endregion

            var results = new Dictionary<string, object>();

            #region xml rigion
            // string soapRequestXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
            //<soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope"">
            //    <soap:Body>
            //        <!-- Your SOAP request here -->
            //    </soap:Body>
            //</soap:Envelope>";

            //HttpContent content = new StringContent(soapRequestXml, Encoding.UTF8, "application/soap+xml");
            #endregion
            try
            {
                LogTracer.AppendLine(DateTime.Now.ToString().Replace("/", "-") + "\t" + "URL" + "\t" + url);

                LogTracer.AppendLine(DateTime.Now.ToString().Replace("/", "-") + "\t" + "Payload" + "\t" + payload);

                using (HttpClient client = new HttpClient())
                {

                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");

                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "");

                    // Add a custom header
                    //client.DefaultRequestHeaders.Add("Custom-Header", "custom-value");

                    client.DefaultRequestHeaders.Add("X-API-KEY", token);
                    try
                    {
                        HttpResponseMessage response = null;

                        if (httpMethod == "POST")
                        {
                            HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

                            // response = await client.PostAsync(url, content);

                            //OR
                            response = client.PostAsync(url, content).Result;

                        }
                        else if (httpMethod == "GET")
                        {
                            // response =  client.GetAsync(url).Result;

                            //OR
                            response = client.GetAsync(url).Result;
                        }
                        else
                        {
                            //Console.WriteLine("Invalid HttpMenthod");
                            results.Add("Status", "091");
                            results.Add("Reference", "");
                            results.Add("Message", "Invalid Http Menthod");
                            results.Add("Data", "");

                            return results;
                        }



                        string responseBody = await response.Content.ReadAsStringAsync();

                        LogTracer.AppendLine(DateTime.Now.ToString().Replace("/", "-") + "\t" + "responseBody" + "\t" + responseBody);

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            // 200 OK - Request succeeded
                            ///Console.WriteLine("Success (200): " + responseBody);

                            results.Add("Status", "000");
                            results.Add("Reference", "");
                            results.Add("Message", "Success");
                            results.Add("Data", responseBody);

                            return results;
                        }
                        else if (response.StatusCode == HttpStatusCode.Created)
                        {
                            // 201 Created - Resource was successfully created
                            //Console.WriteLine("Resource created (201): " + responseBody);
                        }
                        else if (response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            // 400 Bad Request - The server could not understand the request
                            //Console.WriteLine("Bad Request (400): " + responseBody);

                            results.Add("Status", "091");
                            results.Add("Reference", "");
                            results.Add("Message", "Unable to process request try again later");
                            results.Add("Data", responseBody);

                            return results;
                        }
                        else if (response.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            // 401 Unauthorized - Authentication failed
                            //Console.WriteLine("Unauthorized (401): " + responseBody);

                            results.Add("Status", "091");
                            results.Add("Reference", "");
                            results.Add("Message", "Unable to process request try again later");
                            results.Add("Data", responseBody);

                            return results;
                        }
                        else if (response.StatusCode == HttpStatusCode.Forbidden)
                        {
                            // 403 Forbidden - Server understands the request but access is denied
                            //Console.WriteLine("Forbidden (403): " + responseBody);

                            results.Add("Status", "091");
                            results.Add("Reference", "");
                            results.Add("Message", "Unable to process request try again later");
                            results.Add("Data", responseBody);

                            return results;
                        }
                        else if (response.StatusCode == HttpStatusCode.NotFound)
                        {
                            // 404 Not Found - The requested resource could not be found
                            //Console.WriteLine("Not Found (404): " + responseBody);

                            // 401 Unauthorized - Authentication failed
                            //Console.WriteLine("Unauthorized (401): " + responseBody);

                            results.Add("Status", "091");
                            results.Add("Reference", "");
                            results.Add("Message", "Unable to process request try again later");
                            results.Add("Data", responseBody);

                            return results;
                        }
                        else if (response.StatusCode == HttpStatusCode.InternalServerError)
                        {
                            // 500 Internal Server Error - A generic server error
                            //Console.WriteLine("Internal Server Error (500): " + responseBody);

                            results.Add("Status", "092");
                            results.Add("Reference", "");
                            results.Add("Message", "Unable to process request try again later");
                            results.Add("Data", responseBody);

                            return results;
                        }
                        else
                        {
                            // Handle any unexpected status code
                            //Console.WriteLine($"Unexpected status code {(int)response.StatusCode}: " + responseBody);

                            results.Add("Status", "092");
                            results.Add("Reference", "");
                            results.Add("Message", "Unable to process request try again later");
                            results.Add("Data", responseBody);

                            return results;
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        //Console.WriteLine(ex.ToString());

                        LogTracer.AppendLine(DateTime.Now.ToString().Replace("/", "-") + "\t" + "responseBody" + "\t" + ex.ToString());

                        results.Add("Status", "092");
                        results.Add("Reference", "");
                        results.Add("Message", "Unable to process request try again later");
                        results.Add("Data", "");

                        return results;
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine(ex.ToString());

                        LogTracer.AppendLine(DateTime.Now.ToString().Replace("/", "-") + "\t" + "responseBody" + "\t" + ex.ToString());

                        results.Add("Status", "091");
                        results.Add("Reference", "");
                        results.Add("Message", "Unable to process request try again later");
                        results.Add("Data", "");

                        return results;
                    }
                }


                return results;
            }
            catch (Exception ex)
            {
                ///Console.WriteLine(ex.ToString());

                LogTracer.AppendLine(DateTime.Now.ToString().Replace("/", "-") + "\t" + "responseBody" + "\t" + ex.ToString());

                results.Add("Status", "091");
                results.Add("Reference", "");
                results.Add("Message", "Unable to process request try again later");
                results.Add("Data", "");

                return results;
            }
        }

        public static string basicAuth(string username, string password)
        {
            var res = Encoding.ASCII.GetBytes($"{username}:{password}");

            return Convert.ToBase64String(res);
        }


        public static string GetConfig(StringBuilder stringBuilder, string ApplicationID, string Country, string BankID, string CustomerID, string UniqueID, string connectionString, string FunctionName, string ServiceName, string Provider)
        {
            string ConfigXml = "";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("p_GetExternalServiceConfigXml", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("@Country", Country);
                sqlCommand.Parameters.AddWithValue("@ServiceName", ServiceName);
                sqlCommand.Parameters.AddWithValue("@FunctionName", FunctionName);
                sqlCommand.Parameters.AddWithValue("@BankID", BankID);
                sqlCommand.Parameters.AddWithValue("@Provider", Provider);

                SqlDataReader SafResultSet = sqlCommand.ExecuteReader();

                while (SafResultSet.Read())
                {
                    ConfigXml = SafResultSet["Configuration"].ToString();
                }
                if (string.IsNullOrEmpty(ConfigXml))
                {
                    //Loggers.WriteTextLog(ApplicationID, Country, BankID, CustomerID, UniqueID, "EXCEPTION ", "Could not retrive configuration XML");
                    stringBuilder.AppendLine(DateTime.Now.ToString().Replace("/", "-") + "\t" + "EXCEPTION" + "\t" + "Could not retrive configuration XML");
                }
                else
                {
                    //Loggers.WriteTextLog(ApplicationID, Country, BankID, CustomerID, UniqueID, "CONFIG RESPONSE ", "Found Configuration XML");
                    stringBuilder.AppendLine(DateTime.Now.ToString().Replace("/", "-") + "\t" + "EXCEPTION" + "\t" + "Found configuration XML");
                }
                return ConfigXml;
            }
        }
    }
}
