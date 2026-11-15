using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MountFeastMVC.Controllers
{
    public class ContactController : Controller
    {
        [Route("~/hotel-in-naran-contact-number", Name = "hotelnarancontactnumber")]
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(FormCollection frm)
        {
            try
            {
                string name = Request.Form["name"];
                string email = Request.Form["email"];
                string subject = Request.Form["subject"];
                string phone = Request.Form["phone"];
                string message = Request.Form["message"];

                if (email != "")
                {
                    StringBuilder htmlString = new StringBuilder();
                    htmlString.Append(@"<html>");
                    htmlString.Append("<body>");
                    htmlString.Append("</h3>New Query</h3>");
                    htmlString.Append("<span><b>Name:</b></span><span>" + name + "</span><br />");
                    htmlString.Append("<span><b>Email:</b></span><span>" + email + "</span><br />");
                    htmlString.Append("<span><b>Subject:</b></span><span>" + subject + "</span><br />");
                    htmlString.Append("<span><b>Phone:</b></span><span>" + phone + "</span><br />");
                    htmlString.Append("<span><b>Message:</b></span><p>" + message + "</p><br />");
                    htmlString.Append("</body>");
                    htmlString.Append("</html>");

                    MailMessage mm = new MailMessage();
                    mm.IsBodyHtml = true;
                    mm.To.Add("asimshahzad78@gmail.com");

                    mm.From = new MailAddress("info@theiconichotels.com");
                    mm.Subject = "New Query";
                    mm.Body = htmlString.ToString();
                    SmtpClient smc = new SmtpClient();
                    smc.Host = "smtp.gmail.com";
                    smc.Port = 587;
                    //smc.UseDefaultCredentials = false;
                    smc.EnableSsl = true;
                  
                   // smc.Credentials = new System.Net.NetworkCredential("saadkhanm007@gmail.com", "Emirates12");
                    smc.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smc.Credentials = new System.Net.NetworkCredential("info@theiconichotels.com", "NhCMg3RS%&y64qB%7");
                    // smc.EnableSsl = false;
                    smc.Send(mm);
                    TempData["msg"] = "We received your message. Get back to you soon. Thank you";

                    return RedirectToAction("Index");

                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
                return RedirectToAction("Index");
            }

            TempData["msg"] = "We received your message. Get back to you soon. Thank you";

            return RedirectToAction("Index");
        }
    }
}