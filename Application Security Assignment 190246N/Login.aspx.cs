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
            DateTime currentDate = DateTime.Now;

            //If first time visiting this login page
            /*if(Session["loginFailureCount"] == null && Session["AuthTokenFailCount"] == null && Request.Cookies["AuthTokenFailCount"] == null)
            {
                //Set counter
                Session["loginFailureCount"] = 0;
                var loginFailCount = Session["loginFailureCount"];
                //Below code for testing
                emailError.Text = loginFailCount.ToString();
            }
            else
            {
                //Checks if cookie is the same as session
                if (!Session["AuthTokenFailCount"].ToString().Equals(Request.Cookies["AuthTokenFailCount"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                }
                else
                {
                    if ((int)Session["loginFailureCount"] > 3)
                    {
                        submitBtn.Enabled = false;
                    }
                    else
                    {
                        submitBtn.Enabled = true;
                    }
                }
            }*/

            //Checks if Session and cookies exist
            if (Session["loginFailureCount"] != null && Session["AuthTokenFailCount"] != null && Request.Cookies["AuthTokenFailCount"] != null)
            {
                //Checks if session and cookies are the same
                if (!Session["AuthTokenFailCount"].ToString().Equals(Request.Cookies["AuthTokenFailCount"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                }
                else
                {
                    var loginFailCount = Session["loginFailureCount"];
                    //Print out counter for testing purposes
                    emailError.Text = loginFailCount.ToString();
                    //if login failure more than 3 times
                    if ((int)Session["loginFailureCount"] > 3)
                    {
                        //first time disable button
                        if(Session["lastFailDate"] == null)
                        {
                            Session["lastFailDate"] = DateTime.Now;
                            submitBtn.Enabled = false;
                        }
                        else
                        {
                            DateTime lastFailDate = Convert.ToDateTime(Session["lastFailDate"]);
                            double minuteDifference = currentDate.Subtract(lastFailDate).TotalMinutes;
                            if(minuteDifference > 1)
                            {
                                Session.Remove("lastFailDate");
                                Session.Remove("loginFailureCount");

                                emailError.Text = minuteDifference.ToString();
                                submitBtn.Enabled = true;
                            }
                            else
                            {
                                emailError.Text = minuteDifference.ToString();
                                submitBtn.Enabled = false;
                            }
                        }
                    }
                    else
                    {
                        //enable button
                        submitBtn.Enabled = true;
                    }
                }
            }
            //First time visiting the website
            else
            {
                Session["loginFailureCount"] = 0;
                var loginFailCount = Session["loginFailureCount"];
                emailError.Text = loginFailCount.ToString();
            }
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
                string guid = Guid.NewGuid().ToString();
                Session["AuthTokenFailCount"] = guid;
                int currentLoginFailCount = (int)Session["loginFailureCount"];
                Session["loginFailureCount"] = currentLoginFailCount + 1;
                Response.Cookies.Add(new HttpCookie("AuthTokenFailCount", guid));
                Response.Redirect("Login.aspx", false);
            }
        }
    }
}