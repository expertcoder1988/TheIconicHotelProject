using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace MountFeastMVC.Models
{
    public class EncryptionManager
    {
        private byte[] Key = { 89, 83, 45, 236, 140, 228, 180, 79, 209, 164, 231, 131, 28, 7, 110, 73, 140, 235, 118, 52, 225, 46, 202, 118 };
        private byte[] IV = { 161, 200, 187, 207, 22, 92, 119, 227 };
        public string Encrypt(string inputString)
        {
            //Step #4
            byte[] buffer = Encoding.ASCII.GetBytes(inputString);

            TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider()
            {
                Key = Key,
                IV = IV
            };
            //Step #4,5,6
            ICryptoTransform ITransform = tripleDes.CreateEncryptor();
            return Convert.ToBase64String(ITransform.TransformFinalBlock(buffer, 0, buffer.Length));
        }
        public string Decrypt(string inputString)
        {
            byte[] buffer = Convert.FromBase64String(inputString);
            TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider()
            {
                Key = Key,
                IV = IV
            };
            ICryptoTransform ITransform = tripleDes.CreateDecryptor();
            return Encoding.ASCII.GetString(ITransform.TransformFinalBlock(buffer, 0, buffer.Length));
        }
    }

    public static class Accessories
    {
        public static bool ValidateANHU(string text)
        {
            if (
                text.Contains("<") ||
                text.Contains(">") ||
                text.Contains("$") ||
                text.Contains("!") ||
                text.Contains("&") ||
                text.Contains("^") ||
                text.Contains("*") ||
                text.Contains("(") ||
                text.Contains(")") ||
                text.Contains("[") ||
                text.Contains("]") ||
                text.Contains("{") ||
                text.Contains("}") ||
                text.Contains("\\") ||
                text.Contains(";") ||
                text.Contains("`") ||
                text.Contains("~") ||
                text.Contains("?")
                )
                return false;
            else
                return true;
        }

        public static void FormatGridView(GridView gv, bool footer)
        {
            if (gv.Rows.Count > 0)
            {
                gv.UseAccessibleHeader = true;
                gv.HeaderRow.TableSection = TableRowSection.TableHeader;
                if (footer)
                    gv.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        public static object GetDataKeyValueFromGrid(ref object sender)
        {
            GridViewRow r = null;
            GridView gv = null;
            if (sender.GetType() == typeof(LinkButton))
            {
                r = (GridViewRow)((LinkButton)sender).NamingContainer;
                gv = (GridView)r.NamingContainer;
            }
            else if (sender.GetType() == typeof(ImageButton))
            {
                r = (GridViewRow)((ImageButton)sender).NamingContainer;
                gv = (GridView)r.NamingContainer;
            }
            else if (sender.GetType() == typeof(Button))
            {
                r = (GridViewRow)((Button)sender).NamingContainer;
                gv = (GridView)r.NamingContainer;
            }
            else if (sender.GetType() == typeof(CheckBox))
            {
                r = (GridViewRow)((CheckBox)sender).NamingContainer;
                gv = (GridView)r.NamingContainer;
            }
            else if (sender.GetType() == typeof(GridView))
            {
                gv = (GridView)sender;
                r = gv.SelectedRow;
            }
            return gv.DataKeys[r.RowIndex].Value;
        }

        public static object GetDataKeyValueFromGrid(ref object sender, string name)
        {
            GridViewRow r = null;
            GridView gv = null;
            if (sender.GetType() == typeof(LinkButton))
            {
                r = (GridViewRow)((LinkButton)sender).NamingContainer;
                gv = (GridView)r.NamingContainer;
            }
            else if (sender.GetType() == typeof(ImageButton))
            {
                r = (GridViewRow)((ImageButton)sender).NamingContainer;
                gv = (GridView)r.NamingContainer;
            }
            else if (sender.GetType() == typeof(Button))
            {
                r = (GridViewRow)((Button)sender).NamingContainer;
                gv = (GridView)r.NamingContainer;
            }
            else if (sender.GetType() == typeof(CheckBox))
            {
                r = (GridViewRow)((CheckBox)sender).NamingContainer;
                gv = (GridView)r.NamingContainer;
            }
            else if (sender.GetType() == typeof(GridView))
            {
                gv = (GridView)sender;
                r = gv.SelectedRow;
            }
            return gv.DataKeys[r.RowIndex][name];
        }

        public static void ResizeImgae(ref Image img, int OldWidth, int OldHeight, int NewWidth, int NewHeigh)
        {
            if (OldWidth > NewWidth)
            {
                img.Width = new Unit(NewWidth, UnitType.Pixel);
                img.Height = (NewWidth * OldHeight) / OldWidth;
            }
            else if (OldHeight > NewHeigh)
            {
                img.Height = new Unit(NewHeigh, UnitType.Pixel);
                img.Width = (NewHeigh * OldWidth) / OldHeight;
            }

            if (img.Width.Value > NewWidth || img.Height.Value > NewHeigh)
            {
                ResizeImgae(ref img, (int)img.Width.Value, (int)img.Height.Value, NewWidth, NewHeigh);
            }
        }

        #region ENCRYPTION / DECRYPTION
        static EncryptionManager em = new EncryptionManager();
        public static string Encrypt(string plainText)
        {
            return em.Encrypt(plainText);
        }

        public static string Decrypt(string cipher)
        {
            return em.Decrypt(cipher);
        }
        #endregion

        #region ACTIVATION CODE
        private static readonly Random _rng = new Random();
        private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789"; //Added 1-9

        public static string GenerateActivationCode()
        {
            int size = 16;
            char[] buffer = new char[size];

            for (int i = 0; i < size; i++)
            {
                buffer[i] = _chars[_rng.Next(_chars.Length)];
            }
            return new string(buffer);
        }
        #endregion

        #region SEND EMAIL
        public static void SendEmail(string to, string body, string sub)
        {
            MailMessage mm = new MailMessage();
            mm.From = new MailAddress(em.Decrypt(ConfigurationManager.AppSettings["adminemail"]), em.Decrypt(ConfigurationManager.AppSettings["adminmailname"]));
            mm.To.Add(to);
            mm.Body = body;
            mm.Subject = sub;
            mm.IsBodyHtml = true;
            SmtpClient smc = new SmtpClient();
            smc.Port = 587;
            smc.EnableSsl = true;
            smc.DeliveryMethod = SmtpDeliveryMethod.Network;
            smc.UseDefaultCredentials = false;
            smc.Host = em.Decrypt(ConfigurationManager.AppSettings["smtpServer"]);
            smc.Credentials = new NetworkCredential(em.Decrypt(ConfigurationManager.AppSettings["adminemail"]), em.Decrypt(ConfigurationManager.AppSettings["adminpassword"]));
            smc.Send(mm);
        }
        #endregion

        public static int ValidateDDL(ref DropDownList ddl, string validationMessage)
        {
            if (ddl.SelectedIndex <= 0)
            {
                ddl.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                ddl.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#fcdafe");
                if (!string.IsNullOrEmpty(validationMessage))
                    ddl.ToolTip = validationMessage;
                else
                    ddl.ToolTip = "This field must not be left blank";
                return 1;
            }
            else
            {
                ddl.Style.Add(HtmlTextWriterStyle.BorderColor, "#DADADA");
                ddl.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#FFF");
                ddl.ToolTip = "";
                return 0;
            }
        }

        public static int ValidateTxt(ref TextBox txt, string validationMessage)
        {
            if (string.IsNullOrEmpty(txt.Text.Trim()))
            {
                txt.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                txt.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#fcdafe");
                if (!string.IsNullOrEmpty(validationMessage))
                    txt.ToolTip = validationMessage;
                else
                    txt.ToolTip = "This field must not be left blank";
                return 1;
            }
            else
            {
                txt.Style.Add(HtmlTextWriterStyle.BorderColor, "#DADADA");
                txt.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#FFF");
                txt.ToolTip = "";
                return 0;
            }
        }

        public static int ValidateIntTxt(ref TextBox txt, string validationMessage)
        {
            int v = 0;
            int.TryParse(txt.Text, out v);
            if (string.IsNullOrEmpty(txt.Text.Trim()))
            {
                txt.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                txt.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#fcdafe");
                if (!string.IsNullOrEmpty(validationMessage))
                    txt.ToolTip = validationMessage;
                else
                    txt.ToolTip = "This field must not be left blank";
                return 1;
            }
            else if (v <= 0)
            {
                txt.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                txt.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#fcdafe");
                if (!string.IsNullOrEmpty(validationMessage))
                    txt.ToolTip = validationMessage;
                else
                    txt.ToolTip = "Value must be integer, non negative number and greater than zero";
                return 1;
            }
            else
            {
                txt.Style.Add(HtmlTextWriterStyle.BorderColor, "#DADADA");
                txt.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#FFF");
                txt.ToolTip = "";
                return 0;
            }
        }

        public static int ValidateForm(ref Control p)
        {
            int isvalidated = 0;
            ControlCollection ctrl = p.Controls;
            for (int i = 0; i < ctrl.Count; i++)
            {
                Control c = ctrl[i];
                if (c.HasControls())
                    isvalidated += ValidateForm(ref c);
                else
                    if (c is DropDownList && ((DropDownList)c).CssClass.Contains("required"))
                {
                    DropDownList ddl = (DropDownList)c;
                    isvalidated += ValidateDDL(ref ddl, "");
                }
                else
                        if (c is TextBox && ((TextBox)c).CssClass.Contains("required"))
                {
                    TextBox txt = (TextBox)c;
                    isvalidated += ValidateTxt(ref txt, "");
                }
            }
            return isvalidated;
        }
    }
}