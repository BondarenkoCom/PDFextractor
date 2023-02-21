namespace ExtractTextTableInfoFromPDF.Models
{
    public class ClientCredentials
    {
        public string? client_id { get; set; }
        public string? client_secret { get; set; }
    }

    public class ServiceAccountCredentials
    {
        public string? organization_id { get; set; }
        public string? account_id { get; set; }
        public string? private_key_file { get; set; }
    }

    public class Credentials
    {
        public ClientCredentials client_credentials { get; set; }
        public ServiceAccountCredentials service_account_credentials { get; set; }
    }
}