using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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

        public class userInfo
        {
            public string firstName { get; set; }
            public string lastName { get; set; }
            //public string firstName { get; set; }

        }

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

                    String fullName = retrieveInfo(Session["emailLogin"].ToString());
                    if (validUser == true)
                    {
                        userName.Text = fullName;
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
            /*bool validUser = true;//validateUser();
            if (validUser == true)
            {
                //Sample name
                userName.Text = "Johnny Lim";
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }*/
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
        protected bool validateUser(string email)
        {
            bool resultEmail = false;
            string resultFirstName = null;
            string resultLastName = null;
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
        //Checks if user entering website is registered or not
        protected string retrieveInfo(string email)
        {
            //userInfo UserInfo = new userInfo();
            bool resultEmail = false;
            string resultFirstName = null;
            string resultLastName = null;
            string fullName = null;
            SqlConnection con = new SqlConnection(DatabaseConnectionString);

            //Find salt based on email
            string sqlString = "SELECT * FROM userInfo WHERE email=@Email";
            SqlCommand com = new SqlCommand(sqlString, con);
            com.Parameters.AddWithValue("@Email", email);

            try
            {
                con.Open();
                using (SqlDataReader reader = com.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        resultFirstName = reader["firstName"].ToString();
                        resultLastName = reader["lastName"].ToString();
                        fullName = resultFirstName + " " + resultLastName;
                        /*if (reader["*"] != null)
                        {
                            if (reader["*"] != DBNull.Value)
                            {
                                //resultEmail = reader["email"].ToString();
                                //resultEmail = true;
                                resultFirstName = reader["firstName"].ToString();
                                resultLastName = reader["lastName"].ToString();
                                //userInfo UI = new userInfo();
                                //UserInfo.firstName = resultFirstName;
                                //UserInfo.lastName = resultLastName;
                                fullName = resultFirstName + " " + resultLastName;
                            }
                            else
                            {
                                resultEmail = false;
                            }
                        }
                        else
                        {
                            resultEmail = false;
                        }*/
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

            return fullName;
        }

        protected void changePassBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("changePassword.aspx", false);
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