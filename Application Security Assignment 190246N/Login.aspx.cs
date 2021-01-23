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
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;

namespace Application_Security_Assignment_190246N
{
    public partial class Login : System.Web.UI.Page
    {
        //store database directory string
        string DatabaseConnectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            errorMsg.Text = "Testing";
            errorMsg.ForeColor = Color.Red;
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
                        if (Session["lastFailDate"] == null)
                        {
                            Session["lastFailDate"] = DateTime.Now;
                            submitBtn.Enabled = false;
                        }
                        else
                        {
                            DateTime lastFailDate = Convert.ToDateTime(Session["lastFailDate"]);
                            double minuteDifference = currentDate.Subtract(lastFailDate).TotalMinutes;
                            if (minuteDifference > 1)
                            {
                                //Remove counter and last failure session
                                Session.Remove("lastFailDate");
                                Session.Remove("loginFailureCount");

                                //Empties auth token
                                Response.Cookies["AuthTokenFailCount"].Value = string.Empty;
                                Response.Cookies["AuthTokenFailCount"].Expires = DateTime.Now.AddMonths(-20);

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
                //emailError.Text = loginFailCount.ToString();
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
            //Checks if password exists or not
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
            if (emailError.ForeColor == Color.Green && passwordError.ForeColor == Color.Green)
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
            //store success string
            public string success { get; set; }
            //store list of error messages
            public List<string> ErrorMessage { get; set; }
        }

        //Google Recaptcha API V3
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

            //Check if all input is valid or not
            if (validCaptcha == true && validInput == true)
            {
                bool validEntry = ValidateEntry();

                //Then we call the database
                if (validEntry == true)
                {
                    //User session
                    Session["emailLogin"] = emailTB.Text;
                    string guid = Guid.NewGuid().ToString();
                    Session["AuthToken"] = guid;

                    Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                    Response.Redirect("User.aspx", false);
                }
                else
                {
                    //ValidateInput();
                    string guid = Guid.NewGuid().ToString();
                    Session["AuthTokenFailCount"] = guid;
                    int currentLoginFailCount = (int)Session["loginFailureCount"];
                    Session["loginFailureCount"] = currentLoginFailCount + 1;
                    Response.Cookies.Add(new HttpCookie("AuthTokenFailCount", guid));
                    Response.Redirect("Login.aspx", false);
                }
            }
            else
            {
                //ValidateInput();
                string guid = Guid.NewGuid().ToString();
                Session["AuthTokenFailCount"] = guid;
                int currentLoginFailCount = (int)Session["loginFailureCount"];
                Session["loginFailureCount"] = currentLoginFailCount + 1;
                Response.Cookies.Add(new HttpCookie("AuthTokenFailCount", guid));
                Response.Redirect("Login.aspx", false);
            }
        }

        //Encrypt text
        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                //ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0,
               plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return cipherText;
        }

        //Retrieves passwordHash from specific email
        protected string getDBHash(string email)
        {
            string dbHash = null;
            SqlConnection con = new SqlConnection(DatabaseConnectionString);

            //Find hash based on email
            string sqlString = "SELECT passwordHash FROM userInfo WHERE email=@Email";
            SqlCommand com = new SqlCommand(sqlString, con);
            com.Parameters.AddWithValue("@Email", email);

            try
            {
                con.Open();
                using (SqlDataReader reader = com.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["passwordHash"] != null)
                        {
                            if (reader["passwordHash"] != DBNull.Value)
                            {
                                dbHash = reader["passwordHash"].ToString();
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                //Error Code here
                Console.WriteLine(ex);
                throw new Exception(ex.ToString());
            }
            finally { con.Close(); }

            return dbHash;
        }
        //Retrieves passwordSalt from specific email
        protected string getDBSalt(string email)
        {
            string dbSalt = null;
            //Initialise DB connection
            SqlConnection con = new SqlConnection(DatabaseConnectionString);

            //Find salt based on email
            string sqlString = "SELECT passwordSalt FROM userInfo WHERE email=@Email";
            SqlCommand com = new SqlCommand(sqlString, con);
            com.Parameters.AddWithValue("@Email", email);

            try
            {
                con.Open();
                using (SqlDataReader reader = com.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["passwordSalt"] != null)
                        {
                            if (reader["passwordSalt"] != DBNull.Value)
                            {
                                dbSalt = reader["passwordSalt"].ToString();
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                //Error Code here
                Console.WriteLine(ex);
                throw new Exception(ex.ToString());
            }
            finally { con.Close(); }

            return dbSalt;
        }

        //This functions checks if email exists in the database or not
        protected bool ValidateEntry()
        {
            string email = emailTB.Text.Trim();
            string password = passwordTB.Text.Trim();
            SHA512Managed hashing = new SHA512Managed();
            string resultHash = getDBHash(email);
            string resultSalt = getDBSalt(email);
            try
            {
                if (resultHash != null && resultHash.Length > 0 && resultSalt != null && resultSalt.Length > 0)
                {
                    string passwordWithSalt = password + resultSalt;
                    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(passwordWithSalt));
                    string userHash = Convert.ToBase64String(hashWithSalt);

                    //If calculated salt matches hash
                    if (userHash.Equals(resultHash))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.ToString());
                errorMsg.Text = ex.ToString();
                return false;
            }
        }
    }
}