using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Application_Security_Assignment_190246N
{
    public partial class User : System.Web.UI.Page
    {
        //store database directory string
        string DatabaseConnectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        protected void Page_Load(object sender, EventArgs e)
        {
            bool validUser = true;//validateUser();
            if (validUser == true)
            {
                //Sample name
                userName.Text = "Johnny Lim";
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
            //Checks a list of sessions in order to prove that user is registered or not
            //if()
        }
        protected string decryptData(byte[] cipherText)
        {
            string plainText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                //Create the streams used for decryption

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptTransform, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            //Read the decrypted bytes from the decrypting stream
                            //and place them in a string
                            plainText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return plainText;
        }

        //Checks if user entering website is registered or not
        protected bool validateUser()
        {
            return true;
        }

        protected void changePassBtn_Click(object sender, EventArgs e)
        {

        }

        protected void logoutBtn_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            Response.Redirect("Login.aspx", false);

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }
            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
        }
    }
}