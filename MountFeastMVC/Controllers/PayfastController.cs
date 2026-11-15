using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MountFeastMVC.Controllers
{
    public class PayfastController : Controller
    {
        public async Task<ActionResult> GetAccessToken(string billReference, string GrandTotal)
        {
            var url = "https://ipg1.apps.net.pk/Ecommerce/api/Transaction/GetAccessToken";

            // Set the parameters for AuthRequest
            var authRequest = new AuthRequest
            {
                MERCHANT_ID = "22339",
                SECURED_KEY = "jre-3x0GgGJ17xCUXnBB",
                TXNAMT = Convert.ToDecimal(GrandTotal),
                BASKET_ID = billReference,
                CURRENCY_CODE = "PKR"
                //merchant_id = "22339",
                //secured_key = "jre-3x0GgGJ17xCUXnBB"
            };

            var requestContent = new StringContent(JsonConvert.SerializeObject(authRequest), Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.PostAsync(url, requestContent);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var token = JsonConvert.DeserializeObject<TokenResponse>(responseContent); // Assuming the response can be deserialized into this model

                        // Call ProcessPaymentRequest with the token received
                        return await ProcessPaymentRequest(token.ACCESS_TOKEN, billReference, GrandTotal);
                    }

                    return new ContentResult
                    {
                        //StatusCode = (int)response.StatusCode,
                        Content = await response.Content.ReadAsStringAsync(),
                        ContentType = "application/json"
                    };
                }
                catch (HttpRequestException ex)
                {
                    return new HttpStatusCodeResult(500, $"Request to external service failed: {ex.Message}");
                }
            }
        }

        public async Task<ActionResult> GetAccessTokenUat(AuthRequest authRequest)
        {
            var url = "https://ipguat.apps.net.pk/Ecommerce/api/Transaction/GetAccessToken";

            var requestContent = new StringContent(JsonConvert.SerializeObject(authRequest), Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.PostAsync(url, requestContent);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var token = JsonConvert.DeserializeObject<TokenResponse>(responseContent); // Assuming the response can be deserialized into this model
                        return Json(token, JsonRequestBehavior.AllowGet);
                    }

                    return new ContentResult
                    {
                        //StatusCode = (int)response.StatusCode,
                        Content = await response.Content.ReadAsStringAsync(),
                        ContentType = "application/json"
                    };
                }
                catch (HttpRequestException ex)
                {
                    return new HttpStatusCodeResult(500, $"Request to external service failed: {ex.Message}");
                }
            }
        }

        public async Task<ActionResult> ProcessPaymentRequest(string TOKEN, string billReference, string GrandTotal)
        {
            string PaymentApiUrl = "https://ipg1.apps.net.pk/Ecommerce/api/Transaction/PostTransaction";

            // Dummy request data for testing
            var request = new TransactionRequest
            {
                MERCHANT_ID = "22339",
                TOKEN = TOKEN,
                TXNAMT = 5000,
                BASKET_ID = "123"
            };

            if (request == null)
            {
                return new HttpStatusCodeResult(400, "Invalid payment request data.");
            }

            if (string.IsNullOrEmpty(request.TOKEN))
            {
                return new HttpStatusCodeResult(500, "Failed to obtain access token.");
            }

            // Generate signature
            string signature = GenerateSignature(request.MERCHANT_ID, request.TXNAMT.ToString(), request.BASKET_ID);
            request.SIGNATURE = signature;

            // Prepare data for payment request
            var paymentData = new Dictionary<string, string>
        {
            { "CURRENCY_CODE", "PKR" },
            { "MERCHANT_ID", "22339" },
            { "TOKEN", TOKEN },
            { "SUCCESS_URL", "https://theiconichotels.com/searchresult/thankyou" },
            { "FAILURE_URL", "https://theiconichotels.com/searchresult/thankyou" },
            { "CHECKOUT_URL", "https://typedwebhook.tools/webhook/cfe4e40e-8c5c-4d5b-867a-017bce41070c" },
            { "CUSTOMER_EMAIL_ADDRESS", "sale1@malikexpress.com" },
            { "TXNAMT", GrandTotal },
            { "BASKET_ID", billReference },
            { "ORDER_DATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
            { "SIGNATURE", "" },
            { "VERSION", "MERCHANT-CART-0.1" },
            { "TXNDESC", "Item Purchased from Cart" },
            { "PROCCODE", "00" },
            { "TRAN_TYPE", "ECOMM_PURCHASE" },
            { "STORE_ID", "" }, // Optional
            { "RECURRING_TXN", "true" }
        };

            TempData["FormData"] = paymentData;
            return Redirect("RedirectToPayment");

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(PaymentApiUrl, new FormUrlEncodedContent(paymentData));

                    if (response.IsSuccessStatusCode)
                    {
                        // Store the necessary data in the ViewBag
                        ViewBag.Token = TOKEN;
                        ViewBag.BillReference = billReference;
                        ViewBag.GrandTotal = GrandTotal;
                        ViewBag.OrderDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        ViewBag.PaymentApiUrl = PaymentApiUrl;

                        // Return the RedirectToPayment view
                        return Redirect("RedirectToPayment");
                    }
                    else
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        return new HttpStatusCodeResult((int)response.StatusCode, $"Payment request failed: {responseData}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception details
                return new HttpStatusCodeResult(500, $"Internal server error: {ex.Message}");
            }
        }


        private string GenerateSignature(string MerchantId, string transactionAmount, string basketId)
        {
            string secretWord = ""; // Obtain secret word from merchant portal
            string responseString = $"{MerchantId}{basketId}{secretWord}{transactionAmount}";
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(responseString));
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public ActionResult RedirectToPayment()
        {
            return View();
        }
    }


    public class TransactionRequest
    {
        public string CURRENCY_CODE { get; set; } = "PKR";
        public string MERCHANT_ID { get; set; }
        public string TOKEN { get; set; }
        public string SUCCESS_URL { get; set; } = "http://merchant-site-example.com";
        public string FAILURE_URL { get; set; } = "http://merchant-site-example.com";
        public string CHECKOUT_URL { get; set; } = "https://typedwebhook.tools/webhook/cfe4e40e-8c5c-4d5b-867a-017bce41070c";
        public string CUSTOMER_EMAIL_ADDRESS { get; set; } = "some-email@example.com";
        public decimal TXNAMT { get; set; }
        public string BASKET_ID { get; set; }
        public DateTime ORDER_DATE { get; set; } = DateTime.Now;
        public string SIGNATURE { get; set; }
        public string VERSION { get; set; } = "MERCHANT-CART-0.1";
        public string TXNDESC { get; set; } = "Item Purchased from Cart";
        public string PROCCODE { get; set; } = "00";
        public string TRAN_TYPE { get; set; } = "ECOMM_PURCHASE";
        public string STORE_ID { get; set; } = string.Empty;
        public bool RECURRING_TXN { get; set; } = true;
    }

    public class AuthRequest
    {
        public string MERCHANT_ID { get; set; }
        public string SECURED_KEY { get; set; }
        public string BASKET_ID { get; set; }
        public string CURRENCY_CODE { get; set; }
        public decimal TXNAMT { get; set; }
        //public string merchant_id { get; set; }
        //public string secured_key { get; set; }
        // Add other properties as per the expected request data
    }

    public class TokenResponse
    {
        public string ACCESS_TOKEN { get; set; }
    }
}
