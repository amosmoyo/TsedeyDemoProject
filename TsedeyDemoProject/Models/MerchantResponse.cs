namespace TsedeyDemoProject.Models
{
    public class MerchantResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public string Reference { get; set; }
        public string MerchantStatusID { get; set; }
        public string DueDate { get; set; }
        public string DueAmount { get; set; }
        public string AccountID { get; set; }
        public string AccountName { get; set; }
        public string ReceiverName { get; set; }
        public string PowerUnits { get; set; }
    }
}
