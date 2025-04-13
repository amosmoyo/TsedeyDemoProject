using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Net;
using TsedeyDemoProject.Models;

namespace TsedeyDemoProject.Logs
{
    public class Loggers
    {
        public Loggers() { }


        public static void WriteTextLogs(string? ApplicationID, string Country, string BankID, string UniqueID, string CustomerID, string Header, string LogText, IConfiguration configs)
        {
            ApplicationID = configs["AppSettings:ApplicationID"];


            try
            {
                WriteLocalLogs(CustomerID, ApplicationID, UniqueID, Country, BankID, Header, LogText, configs);

                string? WRITE_REMOTE_LOGS = configs.GetSection("AppSettings").GetSection("WRITE_REMOTE_LOGS").Value.ToString();

                if (WRITE_REMOTE_LOGS.ToUpper().Equals("TRUE"))
                {
                    string text = DateTime.Now.ToString();
                    string Time = DateTime.Now.ToString().Replace("/", "-");
                    Header = Time + "/" + Header;
                    TextLogQueue textLogQueue = new TextLogQueue();
                    textLogQueue.LogText = LogText;
                    textLogQueue.HeaderText = Header;
                    textLogQueue.ServerDateTime = Time;
                    textLogQueue.ApplicationID = ApplicationID;
                    textLogQueue.BankID = BankID;
                    textLogQueue.Country = Country;
                    textLogQueue.UniqueID = UniqueID;
                    textLogQueue.CustomerID = CustomerID;

                    String JsonRequest = JsonConvert.SerializeObject(textLogQueue);

                    String? QueueUrl = configs["AppSettings:ELMATextLogQueue"];

                    using (var client = new WebClient())
                    {
                        try
                        {
                            client.Headers[HttpRequestHeader.ContentType] = "application/json";
                            client.UploadString(QueueUrl, "POST", JsonRequest);
                        }
                        catch (Exception ex)
                        {
                            var xxx = ex.ToString();
                        }
                    }

                }
            }
            catch (Exception)
            {
            }
        }


        private static void WriteLocalLogs(string CustomerID, string? ApplicationID, string UniqueID, string Country, string BankID, string Header, string LogText, IConfiguration configs)
        {
            string? WRITE_LOCAL_LOGS = configs["AppSettings:WRITE_LOCAL_LOGS"];

            if (WRITE_LOCAL_LOGS.Trim().ToUpper().Equals("TRUE"))
            {
                string sYear = DateTime.Now.Year.ToString();
                string sMonth = DateTime.Now.Month.ToString();
                string sDay = DateTime.Now.Day.ToString();
                DateTime date = new DateTime(Int32.Parse(sYear), Int32.Parse(sMonth), 1);
                string monthName = date.ToString("MMMM");
                string? dir = configs["AppSettings:LOCAL_LOGS"];
                string sPathName = dir + Country + "\\" + BankID + "\\" + sYear + "\\" + monthName + "\\" + sDay + "\\";

                if (!Directory.Exists(sPathName))
                {
                    Directory.CreateDirectory(sPathName);
                }
                var currentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

                //open a text file with the errors for the day 
                // If it does not exist create it

                using (StreamWriter sw = new StreamWriter(sPathName + CustomerID + ".txt", true))
                {
                    sw.WriteLine(currentDate + "\t" + UniqueID + "\t" + Header + "\t" + LogText);
                }
            }
        }


        public String WriteLocalLogs(SqlCommand OriginalCommand)
        {
            int i;
            String CommandText = OriginalCommand.CommandText + " ";

            try
            {
                for (i = 0; i < OriginalCommand.Parameters.Count; i++)
                {
                    if (OriginalCommand.Parameters[i].Value != null)
                        CommandText = CommandText + " " + OriginalCommand.Parameters[i].ParameterName + "='" + OriginalCommand.Parameters[i].Value.ToString() + "',";
                }
            }
            catch
            {
            }
            return CommandText.TrimEnd(',');
        }


    }
}
