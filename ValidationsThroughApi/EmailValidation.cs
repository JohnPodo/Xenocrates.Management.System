
using Newtonsoft.Json;
using RestSharp;

namespace ValidationsThroughApi
{
    public class EmailValidation
    {
        public static bool CheckEmail(string email)
        {
            var client = new RestClient("https://zerobounce1.p.rapidapi.com/v2/validate");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-key", "d97387d27amsh92f9e6f2c0d42dfp1e629fjsn0c7e722b90bf");
            request.AddHeader("x-rapidapi-host", "zerobounce1.p.rapidapi.com");
            request.AddParameter("ip_address", "");
            request.AddParameter("email", email);
            IRestResponse response = client.Execute(request);
            EmailValidationRoot myDeserializedClass = JsonConvert.DeserializeObject<EmailValidationRoot>(response.Content);
            if (myDeserializedClass.status == "valid")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public class EmailValidationRoot
    {
        public string address { get; set; }
        public string status { get; set; }
        public string sub_status { get; set; }
        public bool free_email { get; set; }
        public object did_you_mean { get; set; }
        public string account { get; set; }
        public string domain { get; set; }
        public string domain_age_days { get; set; }
        public string smtp_provider { get; set; }
        public string mx_found { get; set; }
        public object mx_record { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string gender { get; set; }
        public object country { get; set; }
        public object region { get; set; }
        public object city { get; set; }
        public object zipcode { get; set; }
        public string processed_at { get; set; }
    }
}
