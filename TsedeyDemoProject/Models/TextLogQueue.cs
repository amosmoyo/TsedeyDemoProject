namespace TsedeyDemoProject.Models
{
    public class TextLogQueue
    {
        public string CustomerID { get; set; }
        public string ApplicationID { get; set; }
        public string HeaderText { get; set; }
        public string BankID { get; set; }
        public string Country { get; set; }
        public string ServerDateTime { get; set; }
        public string UniqueID { get; set; }
        public string LogText { get; set; }
        public string Priority { get; set; }
    }
}
