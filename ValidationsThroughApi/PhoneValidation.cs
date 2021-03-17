using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace ValidationsThroughApi
{
    class PhoneValidation
    {
        public static bool CheckPhone(string phone)
        {
            var client = new RestClient("https://neutrinoapi-phone-validate.p.rapidapi.com/phone-validate");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("x-rapidapi-key", "e5daa1f4b9mshced4ea07e1ff787p17b643jsn8a14fab4315f");
            request.AddHeader("x-rapidapi-host", "neutrinoapi-phone-validate.p.rapidapi.com");
            request.AddParameter("application/x-www-form-urlencoded", $"number={phone}&country-code={"GR"}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            PhoneValidationRoot myDeserializedClass = JsonConvert.DeserializeObject<PhoneValidationRoot>(response.Content);
            return myDeserializedClass.valid;
        }
    }
    public class PhoneValidationRoot
    {
        public bool valid { get; set; }
        public string country { get; set; }

        [JsonProperty("country-code")]
        public string CountryCode { get; set; }

        [JsonProperty("prefix-network")]
        public string PrefixNetwork { get; set; }

        [JsonProperty("international-number")]
        public string InternationalNumber { get; set; }
        public string location { get; set; }

        [JsonProperty("local-number")]
        public string LocalNumber { get; set; }
        public string type { get; set; }

        [JsonProperty("currency-code")]
        public string CurrencyCode { get; set; }

        [JsonProperty("international-calling-code")]
        public string InternationalCallingCode { get; set; }

        [JsonProperty("is-mobile")]
        public bool IsMobile { get; set; }

        [JsonProperty("country-code3")]
        public string CountryCode3 { get; set; }
    }
}
