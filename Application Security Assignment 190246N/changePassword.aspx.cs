using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Application_Security_Assignment_190246N
{
    public partial class changePassword : System.Web.UI.Page
    {
        //store database directory string
        string DatabaseConnectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;


        protected void Page_Load(object sender, EventArgs e)
        {
            //Checks a list of sessions in order to prove that user is registered or not
            if (Session["emailLogin"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                }
                else
                {
                    bool validUser = validateUser(Session["emailLogin"].ToString());

                    //String fullName = retrieveInfo(Session["emailLogin"].ToString());
                    if (validUser == true)
                    {
                        currentPasswordError.Text = "It works";
                        //userName.Text = fullName;
                    }
                    else
                    {
                        Response.Redirect("Login.aspx", false);
                    }
                }
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
        }
        protected bool validateUser(string email)
        {
            bool resultEmail = false;
            //string resultFirstName = null;
            //string resultLastName = null;
            SqlConnection con = new SqlConnection(DatabaseConnectionString);

            //Find salt based on email
            string sqlString = "SELECT email FROM userInfo WHERE email=@Email";
            SqlCommand com = new SqlCommand(sqlString, con);
            com.Parameters.AddWithValue("@Email", email);

            try
            {
                con.Open();
                using (SqlDataReader reader = com.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["email"] != null)
                        {
                            if (reader["email"] != DBNull.Value)
                            {
                                //resultEmail = reader["email"].ToString();
                                resultEmail = true;
                            }
                            else
                            {
                                resultEmail = false;
                            }
                        }
                        else
                        {
                            resultEmail = false;
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

            return resultEmail;
        }
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

        private bool ValidateInput()
        {
            return true;
        }

        protected void submitBtn_Click(object sender, EventArgs e)
        {
            bool validCaptcha = ValidateCaptcha();

        }
    }
}