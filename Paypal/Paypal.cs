using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Paypal
{
    public class Paypal
    {
        public static async Task<string> MakePayment(decimal salary,string email)
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api-m.sandbox.paypal.com/v1/oauth2/token");

            request.Headers.TryAddWithoutValidation("Accept", "application/json");
            request.Headers.TryAddWithoutValidation("Accept-Language", "en_US");

            var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes("AQFDGRZsN8ulQ1ti5HnCKBYIa_-XG1Eu7KS3ewT8Yh0EJN3pntQvu-ueDGZ3DTYQmVblbBWxwb-qnK7h:EMRldqY1H1j5AO6EAZfHRl39qfZnRhQO6YoAuslKGQtIB1-LWPz8tp4qCXDR6dSBaZ6J8H-es6tBVx3e"));
            request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

            request.Content = new StringContent("grant_type=client_credentials");
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

            var response = await httpClient.SendAsync(request);
            AccessToken token = JsonConvert.DeserializeObject<AccessToken>(await response.Content.ReadAsStringAsync());


            httpClient = new HttpClient();
            request = new HttpRequestMessage(new HttpMethod("POST"), "https://api-m.sandbox.paypal.com/v1/payments/payouts");

            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token.access_token}");

            request.Content = new StringContent(ItemSetting(salary, email));
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            response = await httpClient.SendAsync(request);
            var dataStatus = response.StatusCode;
            PaymentInfo payment = JsonConvert.DeserializeObject<PaymentInfo>(await response.Content.ReadAsStringAsync());
            
            var json = new
            {
                Amount = salary,
                Date = DateTime.Now
            };
            string data = JsonConvert.SerializeObject(json);
            return (dataStatus == System.Net.HttpStatusCode.Created) ? data : null;
        }

        private static string ItemSetting(decimal salary,string email)
        {
            string[] sb=new string[]{"{ \"sender_batch_header\":{ ",
            "\"sender_batch_id\": \"\",",
            "\"recipient_type\": \"EMAIL\",",
            "\"email_subject\": \"You have money!\",",
            "\"email_message\": \"You received a payment. Thanks for using our service!\" },",
            "\"items\": [ {",
            "\"amount\": {",
            $"\"value\": \"{salary.ToString(CultureInfo.CreateSpecificCulture("en-GB"))}\",",
            "\"currency\": \"USD\"},",
            "\"sender_item_id\": \"\",",
            "\"recipient_wallet\": \"PAYPAL\",",
            $"\"receiver\": \"{email}\"","}]}" };
            var result = string.Concat(sb);
            return result;
        }
    }
}
