using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

namespace Application_Security_Assignment_190246N
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Add session stuff here
            //session counter
        }

        private bool ValidateInput()
        {
            //Checks if email address is empty
            if (String.IsNullOrEmpty(emailTB.Text))
            {
                emailError.Text = "Please enter your email address";
                emailError.ForeColor = Color.Red;
                emailError.Visible = true;
            }
            else
            {
                //Checks if email addres is actually email address
                bool isEmail = Regex.IsMatch(emailTB.Text, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                if (isEmail == true)
                {
                    emailError.Text = "";
                    emailError.ForeColor = Color.Green;
                    emailError.Visible = false;
                }
                else
                {
                    emailError.Text = "Please enter a valid email address";
                    emailError.ForeColor = Color.Red;
                    emailError.Visible = true;
                }
            }
            //Checks if both passwords are the same
            if (String.IsNullOrEmpty(passwordTB.Text))
            {
                passwordError.Text = "Please enter your password";
                passwordError.Visible = true;
                passwordError.ForeColor = Color.Red;
            }
            else
            {
                passwordError.Text = "";
                passwordError.Visible = false;
                passwordError.ForeColor = Color.Green;
            }
            if(emailError.ForeColor == Color.Green && passwordError.ForeColor == Color.Green)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //an object for recaptcha to store info
        public class reCaptchaResponseObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }


        public bool ValidateCaptcha()
        {
            bool result = true;

            //Retrieves captcha response from captcha api
            string captchaResponse = Request.Form["g-recaptcha-response"];

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=6LekWxYaAAAAAOJF1_WrjjmjffAFI2YN2ZWlmm1i &response=" + captchaResponse);

            try
            {
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        //Read entire json response from recaptcha
                        string jsonResponse = readStream.ReadToEnd();

                        JavaScriptSerializer js = new JavaScriptSerializer();

                        reCaptchaResponseObject jsonObject = js.Deserialize<reCaptchaResponseObject>(jsonResponse);

                        //Console.WriteLine("--- Testing ---");
                        //Console.WriteLine(jsonObject);
                        //Read success property in json object
                        result = Convert.ToBoolean(jsonObject.success);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        protected void submitBtn_Click(object sender, EventArgs e)
        {
            bool validInput = ValidateInput();

            bool validCaptcha = ValidateCaptcha();

            if (validCaptcha == true && validInput == true)
            {

            }
            else
            {

            }
        }
    }
}