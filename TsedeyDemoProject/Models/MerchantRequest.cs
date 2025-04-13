namespace TsedeyDemoProject.Models
{
    public class MerchantRequest
    {
        public string? ApplicationID { get; set; }  // Required
        public string? CustomerID { get; set; }     // Required
        public string? Country { get; set; }        // Required
        public string? BankID { get; set; }         // Required
        public string? UniqueID { get; set; }       // Required
        public string? FunctionName { get; set; }   // Required

        // Optional fields
        public string? ISOFieldsRequest { get; set; }
        public string? ISOFieldsResponse { get; set; }

        public PaymentDetails? PaymentDetails { get; set; }
        public InfoFields? InfoFields { get; set; }
        public MerchantConfig? MerchantConfig { get; set; }
        public ISOResponseFields? ISOResponseFields { get; set; }
        public AppDetail? AppDetail { get; set; }
        public ResponseDetail? ResponseDetail { get; set; }
        public Customerdetail? Customerdetail { get; set; }
        public ISORequest? ISORequest { get; set; }

        // List should be initialized to avoid null reference issues
        public List<ResponseFields> ResponseFields { get; set; } = new List<ResponseFields>();
    }

    public class ISORequest
    {
        public string? BankID { get; set; }

        public string? MessageType { get; set; }


        public List<ISOField>? ISOFields { get; set; }
    }

    public class ISOField
    {
        public int FieldID { get; set; }

        public string? FieldValue { get; set; }
    }

    public class ISOResponseFields
    {
        public int FieldID { get; set; }
        public string? FieldValue { get; set; }
    }
    public class ResponseFields
    {

        public int FieldID { get; set; }
        public string? FieldValue { get; set; }
    }
    public class PaymentDetails
    {
        public string? MerchantID { get; set; }
        public string? FunctionName { get; set; }
        public string? AccountID { get; set; }
        public string? Amount { get; set; }
        public string? ReferenceNumber { get; set; }
    }

    public class InfoFields
    {
        public string? InfoField1 { get; set; }

        public string? InfoField2 { get; set; }

        public string? InfoField3 { get; set; }

        public string? InfoField4 { get; set; }

        public string? InfoField5 { get; set; }

        public string? InfoField6 { get; set; }

        public string? InfoField7 { get; set; }

        public string? InfoField8 { get; set; }

        public string? InfoField9 { get; set; }

        public string? InfoField10 { get; set; }

        public string? InfoField11 { get; set; }

        public string? InfoField12 { get; set; }

        public string? InfoField13 { get; set; }

        public string? InfoField14 { get; set; }

        public string? InfoField15 { get; set; }

        public string? InfoField16 { get; set; }

        public string? InfoField17 { get; set; }

        public string? InfoField18 { get; set; }

        public string? InfoField19 { get; set; }

        public string? InfoField20 { get; set; }

        public string? InfoField21 { get; set; }

        public string? InfoField22 { get; set; }

        public string? InfoField23 { get; set; }

        public string? InfoField24 { get; set; }

        public string? InfoField25 { get; set; }

        public string? InfoField26 { get; set; }

        public string? InfoField27 { get; set; }

        public string? InfoField28 { get; set; }

        public string? InfoField29 { get; set; }

        public string? InfoField30 { get; set; }

        public string? InfoField31 { get; set; }

        public string? InfoField32 { get; set; }

        public string? InfoField33 { get; set; }

        public string? InfoField34 { get; set; }

        public string? InfoField35 { get; set; }

        public string? InfoField36 { get; set; }

        public string? InfoField37 { get; set; }

        public string? InfoField38 { get; set; }

        public string? InfoField39 { get; set; }

        public string? InfoField40 { get; set; }

        public string? InfoField41 { get; set; }

        public string? InfoField42 { get; set; }

        public string? InfoField43 { get; set; }

        public string? InfoField44 { get; set; }

        public string? InfoField45 { get; set; }

        public string? InfoField46 { get; set; }

        public string? InfoField47 { get; set; }

        public string? InfoField48 { get; set; }

        public string? InfoField49 { get; set; }

        public string? InfoField50 { get; set; }
    }
    public class MerchantConfig
    {
        public string? DLLCallID { get; set; }
        public string? MerchantCode { get; set; }
        public string? MerchantName { get; set; }
        public string? TrxAuthontication { get; set; }
        public string? MerchantProvider { get; set; }
        public string? MerchantURL { get; set; }
        public string? MerchantReference { get; set; }
    }

    public class AppDetail
    {
        public string? AppName { get; set; }
        public string? Version { get; set; }
        public string? CodeBase { get; set; }
        public string? LATLON { get; set; }
        public string? TrxSource { get; set; }
        public string? DeviceNotificationID { get; set; }
        public string? DeviceIMEI { get; set; }
        public string? DeviceIMSI { get; set; }
        public string? ConnString { get; set; }
        public string? EncrytKey { get; set; }
    }

    public class ResponseDetail
    {
        public string? StanNumber { get; set; }
        public string? BankReference { get; set; }
        public string? APPMessage { get; set; }
        public string? USSDMessage { get; set; }
        public string? OtherReference { get; set; }
        public string? ExtraField1 { get; set; }
        public string? ExtraField2 { get; set; }
        public string? ExtraField3 { get; set; }
        public string? CustomerSMSText { get; set; }
        public string? CustomerEMailText { get; set; }
        public string? NotifySMSNumber { get; set; }
        public string? NotifySMSText { get; set; }
        public string? NotifyEmailID { get; set; }
        public string? NotifyEMailText { get; set; }
        public string? FooterText { get; set; }
    }

    public class Customerdetail
    {
        public string? CustomerID { get; set; }
        public string? Country { get; set; }
        public string? MobileNumber { get; set; }
        public string? EmailID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? IMEI { get; set; }
        public string? IMSI { get; set; }
        public string? AppNotificationID { get; set; }
    }
}
