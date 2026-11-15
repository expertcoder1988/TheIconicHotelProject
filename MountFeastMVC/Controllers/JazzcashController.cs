using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MountFeastMVC.Controllers
{
    public class JazzcashController : Controller
    {
        // GET: Jazzcash
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Payment(string billReference, double GrandTotal)
        {
            string MerchantID = "33303563"; //"MC37526"; //Your Merchant from transaction Credentials 
            string Password = "udfu0v40u2"; // "e1z615h0xx"; //Your Password from transaction Credentials 
            string ReturnURL = "https://theiconichotels.com/searchresult/thankyou"; //Your Return URL 
            string HashKey = "2h10003su2";// "66v5u0c368";//Your HashKey from transaction Credentials 
                                          // string PostURL = "https://sandbox.jazzcash.com.pk/CustomerPortal/transactionmanagement/merchantform"; 
                                          //Live https://payments.jazzcash.com.pk/CustomerPortal/transactionmanagement/merchantform

            // date_default_timezone_set("Asia/karachi"); 

            double gTotal =GrandTotal * 100;
            double Amount = Convert.ToDouble(gTotal); //Last two digits will be considered as Decimal 
            string BillReference = billReference;
            string Description = "Thank you for using Jazz Cash";
            string Language = "EN";
            string TxnCurrency = "PKR";
            string TxnDateTime = DateTime.Now.ToString(DateTimeConstants.Formats.DatabaseDateTimeFormat); // date('YmdHis');
            string TxnExpiryDateTime = DateTime.Now.AddDays(1).ToString(DateTimeConstants.Formats.DatabaseDateTimeFormat);  // date('YmdHis', strtotime('+8 Days'));
            string TxnRefNumber = "T" + TxnDateTime; // date('YmdHis');
            string TxnType = "";
            string Version = "1.1";
            string SubMerchantID = "";
            string DiscountedAmount = "";
            string DiscountedBank = "";
            string ppmpf_1 = "";
            string ppmpf_2 = "";
            string ppmpf_3 = "";
            string ppmpf_4 = "";
            string ppmpf_5 = "";

            string[] HashArray = { Amount.ToString(), BillReference, Description, DiscountedAmount, DiscountedBank, Language, MerchantID, Password, ReturnURL, TxnCurrency, TxnDateTime, TxnExpiryDateTime, TxnRefNumber, TxnType, Version, ppmpf_1, ppmpf_2, ppmpf_3, ppmpf_4, ppmpf_5 };

            string SortedArray = HashKey;
            for (int i = 0; i < HashArray.Count(); i++)
            {
                if (HashArray[i] != "undefined" && HashArray[i] != null && HashArray[i] != "")
                {
                    SortedArray = SortedArray + "&" + HashArray[i];
                }
            }

            string Securehash; // hash_hmac('sha256', SortedArray, HashKey);
            Encoding encoding = Encoding.UTF8;
            var keyByte = encoding.GetBytes(HashKey);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                hmacsha256.ComputeHash(encoding.GetBytes(SortedArray));
                Securehash = ByteToString(hmacsha256.Hash);
            }
            //////////////////////////////////////////////////////

         

            NameValueCollection data = new NameValueCollection();


            data.Add("pp_Version", Version);
            data.Add("pp_TxnType", TxnType);
            data.Add("pp_Language", Language);
            data.Add("pp_MerchantID", MerchantID);
            data.Add("pp_SubMerchantID", SubMerchantID);
            data.Add("pp_Password", Password);
            data.Add("pp_TxnExpiryDateTime", TxnExpiryDateTime);
            data.Add("pp_TxnRefNo", TxnRefNumber);
            data.Add("pp_Amount", Amount.ToString());
            data.Add("pp_DiscountedAmount", DiscountedAmount);
            data.Add("pp_DiscountBank", DiscountedBank);
            data.Add("pp_TxnCurrency", TxnCurrency);
            data.Add("pp_TxnDateTime", TxnDateTime);
            data.Add("pp_BillReference", BillReference);
            data.Add("pp_Description", Description);          
            data.Add("pp_ReturnURL", ReturnURL);
            data.Add("pp_SecureHash", Securehash);
            data.Add("ppmpf_1", ppmpf_1);
            data.Add("ppmpf_2", ppmpf_2);
            data.Add("ppmpf_3", ppmpf_3);
            data.Add("ppmpf_4", ppmpf_4);
            data.Add("ppmpf_5", ppmpf_5);

            IDictionary<string, string> parameters = new Dictionary<string, string>();

            foreach (string key in data)
            {
                parameters.Add(key, data[key]);
            }

            TempData["FormData"] = data;


            return View();
        }

        static string ByteToString(byte[] buff)
        {
            string sbinary = "";
            for (int i = 0; i < buff.Length; i++)
                sbinary += buff[i].ToString("X2"); /* hex format */
            return sbinary;
        }
    }

    public class DateTimeConstants
    {
        /// <summary>
        /// Contains names of Days of Week.
        /// </summary>
        public static class DaysofWeek
        {
            public const string Monday = "Monday";
            public const string Tuesday = "Tuesday";
            public const string Wednesday = "Wednesday";
            public const string Thursday = "Thursday";
            public const string Friday = "Friday";
            public const string Saturday = "Saturday";
            public const string Sunday = "Sunday";
        }

        /// <summary>
        /// Contains formats of Date Time used on:
        ///  - Switch
        ///  - User Interface
        /// </summary>
        public static class Formats
        {
            //Switch
            public static readonly string SwitchDateTimeFormat = "yyyyMMddHHmmss";

            public static readonly string SwitchDateFormat = "yyyyMMdd";
            public static readonly string SwitchTimeFormat = "HHmmss";

            //Database - Keeping the same format again so if at some point either switch or database format changes, it will
            //not create problem for the software using this library.
            public static readonly string DatabaseDateTimeFormat = "yyyyMMddHHmmss";

            public static readonly string DatabaseDateFormat = "yyyyMMdd";
            public static readonly string DatabaseTimeFormat = "HHmmss";

            //User Interface
            public static readonly string UIDateFormatWithSlash = "dd/MM/yyyy";

            public static readonly string UIDateTimeFormatWithSlash = "dd/MM/yyyy hh:mm:ss";
            public static readonly string UIDateTimeFormat = "dd MMM yyyy hh:mm:ss";
            public static readonly string UIDateFormatWithSpace = "dd MMM yyyy";
            public static readonly string UITimeFormat = "hh:mm:ss tt";
            public static readonly string UIDateFormatWithDash = "dd-MMM-yyyy";

            public static readonly string JQueryDatePickerFormat = "dd M yy";
            public static readonly string JQueryExpiryDatePickerFormat = "M yy";
            public static readonly string JQueryDateTimePickerFormat = "dd M yy";

            public static readonly string DateTimeFormat = "";

            public static readonly string Time24H = "HH:mm";

            public static readonly string DotNetDateTimeFormat = "yyyy-MM-dd\"T\"H:mm:ss";
            public static readonly string DotNetDateTimeFormatForOracle = "YYYY-MM-DD\"T\"HH24:MI:SS";
            public static readonly string HandShakeDateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";

        }

        /// <summary>
        /// Contains any constants pertaining to Date Time.
        /// </summary>
        public static class Constants
        {
            public static readonly string FromTime = "12:00:00 AM";
            public static readonly string ToTime = "11:59:59 PM";
        }



        public static class Helper
        {
            public static string CalculateHMACSHA256(string SecureSecret, SortedList<string, string> ParamList)
            {
                // Hex Decode the Secure Secret for use in using the HMACSHA256 hasher
                // hex decoding eliminates this source of error as it is independent of the character encoding

                StringBuilder sb = new StringBuilder();
                SortedList<string, string> list = ParamList;
                foreach (KeyValuePair<string, string> kvp in list)
                {
                    if (!String.IsNullOrEmpty(kvp.Value))
                    {
                        sb.Append(kvp.Value + "&");
                    }
                }
                sb.Remove(sb.Length - 1, 1);

                // Create secureHash on string
                string hexHash = "";
                using (HMACSHA256 hasher = new HMACSHA256(Encoding.UTF8.GetBytes(SecureSecret)))
                {

                    byte[] utf8bytes = Encoding.UTF8.GetBytes(sb.ToString());
                    byte[] iso8859bytes = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding("iso-8859-1"), utf8bytes);
                    byte[] hashValue = hasher.ComputeHash(iso8859bytes);

                    foreach (byte b in hashValue)
                    {
                        hexHash += b.ToString("X2");
                    }
                }
                return hexHash;
            }

            public static string CalculateHash(SortedList<string, string> Fields)
            {
                string Hash = string.Empty;

                StringBuilder builder = new StringBuilder();
                StringBuilder Keys = new StringBuilder();
                int i;

                for (i = 0; i < Fields.Count; i++)
                {
                    builder.Append(Fields[Fields.Keys[i]]);
                    Keys.Append(Fields.Keys[i]);
                }

                byte[] TextBytes = ASCIIEncoding.ASCII.GetBytes(builder.ToString());
                SHA256Managed objSHA256 = new SHA256Managed();
                byte[] hashedByte = objSHA256.ComputeHash(TextBytes);


                StringBuilder sOutput = new StringBuilder(hashedByte.Length);
                for (i = 0; i < hashedByte.Length; i++)
                {
                    sOutput.Append(hashedByte[i].ToString("X2"));
                }
                return sOutput.ToString();


            }

        }
    }
}