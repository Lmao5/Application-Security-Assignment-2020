using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
            //store success string
            public string success { get; set; }
            //store list of error messages from api
            public List<string> ErrorMessage { get; set; }
        }

        public class userInfo
        {
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string PasswordHash { get; set; }
            public string PasswordSalt { get; set; }
            public DateTime LastUpdate { get; set; }
            public byte[] IV { get; set; }
            public byte[] Key { get; set; }
            public userInfo()
            {

            }
            public userInfo(string email, string firstName, string lastName,
                string passwordHash, string passwordSalt, DateTime lastUpdate, byte[] iv, byte[] key)
            {
                Email = email;
                FirstName = firstName;
                LastName = lastName;
                PasswordHash = passwordHash;
                PasswordSalt = passwordSalt;
                LastUpdate = lastUpdate;
                IV = iv;
                Key = key;
            }
            public userInfo getUserInfo(string email)
            {
                string DatabaseConnectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
                using (SqlConnection sqlConn = new SqlConnection(DatabaseConnectionString))
                {
                    string sqlStatement = "SELECT * FROM userInfo WHERE email = @paraEmail";
                    using (SqlDataAdapter da = new SqlDataAdapter(sqlStatement, sqlConn))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@paraEmail", email);
                        using (DataSet ds = new DataSet())
                        {
                            try
                            {
                                da.Fill(ds);

                                userInfo uif = null;
                                int rec_cnt = ds.Tables[0].Rows.Count;
                                if (rec_cnt == 1)
                                {
                                    DataRow row = ds.Tables[0].Rows[0];

                                    //string email = row["email"].ToString();
                                    string firstName = row["firstName"].ToString();
                                    string lastName = row["lastName"].ToString();
                                    string passwordHash = row["passwordHash"].ToString();
                                    string passwordSalt = row["passwordSalt"].ToString();
                                    DateTime lastUpdate = Convert.ToDateTime(row["lastUpdate"].ToString());
                                    byte[] iv = Convert.FromBase64String(row["IV"].ToString());
                                    byte[] key = Convert.FromBase64String(row["Key"].ToString());

                                    uif = new userInfo(email, firstName, lastName, passwordHash, passwordSalt, lastUpdate, iv, key);
                                }
                                return uif;
                            }
                            catch (SqlException ex)
                            {
                                throw new Exception(ex.ToString());
                            }
                            finally
                            {
                                Debug.WriteLine("Retrieve userInfo complete!");
                            }
                        }
                    }

                }
            }
            public void updatePassword(string passwordHash, string passwordSalt, string email)
            {
                string DatabaseConnectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
                using (SqlConnection sqlConn = new SqlConnection(DatabaseConnectionString))
                {
                    string sqlStatement = "UPDATE userInfo SET passwordHash = @paraPasswordHash, passwordSalt = @paraPasswordSalt, lastUpdate = @paraLastUpdate WHERE email = @paraEmail";
                    using (SqlCommand sqlComm = new SqlCommand(sqlStatement))
                    {
                        sqlComm.CommandType = CommandType.Text;

                        sqlComm.Parameters.AddWithValue("@paraEmail", email);

                        sqlComm.Parameters.AddWithValue("@paraPasswordHash", passwordHash);
                        sqlComm.Parameters.AddWithValue("@paraPasswordSalt", passwordSalt);
                        sqlComm.Parameters.AddWithValue("@paraLastUpdate", DateTime.Now);
                        

                        sqlComm.Connection = sqlConn;
                        try
                        {
                            sqlConn.Open();
                            sqlComm.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            Debug.WriteLine(ex.ToString());
                            throw new Exception(ex.ToString());
                        }
                        finally
                        {
                            sqlConn.Close();
                        }
                    }
                }

            }
        }

        //Google Recaptcha API V3
        public bool ValidateCaptcha()
        {
            bool result = true;

            //Retrieves captcha response from captcha api
            string captchaResponse = Request.Form["g-recaptcha-response"];

            //Request to Google API
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
            //Catch any exception error from recaptcha
            catch (WebException ex)
            {
                throw ex;
            }
        }

        private bool ValidateInput()
        {
            //Store green colour in here
            var greenColour = Color.Green;

            //Checks if currentPassword field is empty
            if (String.IsNullOrEmpty(currentPasswordTB.Text))
            {
                currentPasswordError.Text = "Please enter your password";
                currentPasswordError.ForeColor = Color.Red;
                currentPasswordError.Visible = true;
            }
            else
            {
                currentPasswordError.Text = "Excellent";
                currentPasswordError.ForeColor = greenColour;
                currentPasswordError.Visible = true;
            }

            //Checks if new password field field is empty
            if (String.IsNullOrEmpty(newPasswordTB.Text))
            {
                newPasswordError.Text = "Please enter your new password";
                newPasswordError.ForeColor = Color.Red;
                newPasswordError.Visible = true;
            }
            //Checks to ensure that old password is not used again
            else if (currentPasswordTB.Text == newPasswordTB.Text)
            {
                newPasswordError.Text = "Please enter that your new password does not match your current password";
                newPasswordError.ForeColor = Color.Red;
                newPasswordError.Visible = true;
            }
            else
            {
                newPasswordError.Text = "Excellent";
                newPasswordError.ForeColor = greenColour;
                newPasswordError.Visible = true;
            }

            //Checks if new password field 2 field is empty
            if (String.IsNullOrEmpty(newPasswordTB2.Text))
            {
                newPasswordError2.Text = "Please enter your new password again";
                newPasswordError2.ForeColor = Color.Red;
                newPasswordError2.Visible = true;
            }
            //Checks to ensure that old password is not used again
            else if (currentPasswordTB.Text == newPasswordTB2.Text)
            {
                newPasswordError2.Text = "Please enter that your new password does not match your current password";
                newPasswordError2.ForeColor = Color.Red;
                newPasswordError2.Visible = true;
            }
            //Checks if both fields are the same
            else if (newPasswordTB.Text != newPasswordTB2.Text)
            {
                newPasswordError2.Text = "Please ensure that both passwords are the same";
                newPasswordError2.ForeColor = Color.Red;
                newPasswordError2.Visible = true;
            }
            else
            {
                newPasswordError2.Text = "Excellent";
                newPasswordError2.ForeColor = greenColour;
                newPasswordError2.Visible = true;
            }


            if (currentPasswordError.ForeColor == greenColour &&
                newPasswordError.ForeColor == greenColour &&
                newPasswordError2.ForeColor == greenColour)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private int checkPassword(string password)
        {
            int score = 0;
            //if password length is less than 8
            if (password.Length < 8)
            {
                return 1;
            }
            //score 1
            else
            {
                score = 1;
            }
            //score 2
            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++;
            }

            //score 3
            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score++;
            }

            //score 4
            if (Regex.IsMatch(password, "[0-9]"))
            {
                score++;
            }

            if (Regex.IsMatch(password, "[$&+,:;=?@#|'<>.^*()%!-]"))
            {
                score++;
            }

            return score;
        }
        private bool checkScore(int passwordScore)
        {
            switch (passwordScore)
            {
                case 1:
                    newPasswordError.Text = "Very Weak";
                    newPasswordError.ForeColor = Color.Red;
                    return false;
                case 2:
                    newPasswordError.Text = "Weak";
                    newPasswordError.ForeColor = Color.Red;
                    return false;
                case 3:
                    newPasswordError.Text = "Medium";
                    newPasswordError.ForeColor = Color.Red;
                    return false;
                case 4:
                    newPasswordError.Text = "Strong";
                    newPasswordError.ForeColor = Color.Red;
                    return false;
                case 5:
                    newPasswordError.Text = "Excellent";
                    newPasswordError.ForeColor = Color.Green;
                    return true;
                default:
                    return false;
            }
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
        //This functions checks if user enters the correct current password
        protected bool ValidateEntry()
        {
            string email = Session["emailLogin"].ToString();
            string password = currentPasswordTB.Text.Trim();
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
        protected bool allowPasswordChange()
        {
            string userEmail = Session["emailLogin"].ToString();

            userInfo uif = new userInfo();

            userInfo resultUser = uif.getUserInfo(userEmail);
            Debug.WriteLine("It works");
            //errorMsg.Text = resultUser.LastName;
            Debug.WriteLine(resultUser.LastUpdate);
            Debug.WriteLine(DateTime.Now);

            DateTime currentDate = DateTime.Now;
            double minuteDifference = currentDate.Subtract(resultUser.LastUpdate).TotalMinutes;
            Debug.WriteLine("===================");
            Debug.WriteLine(minuteDifference);

            if (minuteDifference < 5)
            {
                return false;
            }
            else if (minuteDifference > 15)
            {
                return true;
            }
            else
            {
                return true;
            }
        }
        /*protected byte[] encryptData(byte[]iv, byte[] key,string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = iv;
                cipher.Key = key;
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
        }*/

        protected void submitBtn_Click(object sender, EventArgs e)
        {
            bool validCaptcha = ValidateCaptcha();

            bool validInput = ValidateInput();

            int passwordScore = checkPassword(currentPasswordTB.Text);

            bool goodPassword = checkScore(passwordScore);

            bool validEntry = ValidateEntry();

            //if user has entered valid format and is not a bot
            if (validCaptcha == true && validInput == true)
            {
                //if password has all the necesssary characters & user has entered the correct current password
                if (goodPassword == true && validEntry == true)
                {
                    //Checks when password is recently changed
                    bool passwordChange = allowPasswordChange();
                    if (passwordChange == true)
                    {
                        string userEmail = Session["emailLogin"].ToString();

                        string newPassword = newPasswordTB.Text.ToString().Trim();

                        RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                        byte[] saltByte = new byte[8];

                        rng.GetBytes(saltByte);
                        string salt = Convert.ToBase64String(saltByte);

                        SHA512Managed hashing = new SHA512Managed();
                        string pwdWithSalt = newPassword + salt;

                        byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(newPassword));
                        byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

                        string finalHash = Convert.ToBase64String(hashWithSalt);

                        //Call update password function here

                        userInfo uif = new userInfo();

                        uif.updatePassword(finalHash, salt, userEmail);
                        Response.Redirect("User.aspx",false);
                    }
                    else
                    {
                        Debug.WriteLine("Password age is below 5 minutes");
                        //Error Code here
                    }
                }
                else
                {
                    //Error Code here
                }
            }
            else
            {
                //Error Code here
                //Response.Redirect("User.aspx", false);
            }

        }
    }
}