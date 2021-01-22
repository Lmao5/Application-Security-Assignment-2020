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
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Application_Security_Assignment_190246N
{
    public partial class Registration : System.Web.UI.Page
    {
        //store database directory string
        string DatabaseConnectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        protected void Page_Load(object sender, EventArgs e)
        {
            //User needs to input all correct information before sending off
        }

        protected void submitBtn_Click(object sender, EventArgs e)
        {

            int scores = checkPassword(firstPasswordTB.Text);
            switch (scores)
            {
                case 1:
                    firstPasswordError.Text = "Very Weak";
                    firstPasswordError.ForeColor = Color.Red;
                    break;
                case 2:
                    firstPasswordError.Text = "Weak";
                    firstPasswordError.ForeColor = Color.Red;
                    break;
                case 3:
                    firstPasswordError.Text = "Medium";
                    firstPasswordError.ForeColor = Color.Red;
                    break;
                case 4:
                    firstPasswordError.Text = "Strong";
                    firstPasswordError.ForeColor = Color.Red;
                    break;
                case 5:
                    firstPasswordError.Text = "Excellent";
                    firstPasswordError.ForeColor = Color.Green;
                    break;
                default:
                    break;
            }

            bool validInput = ValidateInput();

            bool validCaptcha = ValidateCaptcha();

            if (validInput == true && validCaptcha == true && scores == 5)
            {
                bool presentEmail = getEmail(emailTB.Text);

                //Checks if email is present or not
                if (presentEmail == true)
                {
                    errorMsg.Text = "Please choose another email for registration";
                    errorMsg.ForeColor = Color.Red;
                    emailError.Text = "Please choose another email for registration";
                    emailError.ForeColor = Color.Red;
                    //Response.Redirect("Registration.aspx",false);
                }
                else
                {
                    //submitBtn.Enabled = true;
                    //secondPasswordTB.Text = validCaptcha.ToString();
                    //Retrieve password input
                    string password = firstPasswordTB.Text.ToString().Trim();

                    //Generate random salt
                    RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                    byte[] saltByte = new byte[8];

                    //Fills array of bytes with a cryptographically strong sequence of random values.
                    rng.GetBytes(saltByte);
                    salt = Convert.ToBase64String(saltByte);

                    SHA512Managed hashing = new SHA512Managed();
                    string pwdWithSalt = password + salt;

                    byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(password));
                    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

                    finalHash = Convert.ToBase64String(hashWithSalt);

                    RijndaelManaged cipher = new RijndaelManaged();
                    cipher.GenerateKey();
                    Key = cipher.Key;
                    IV = cipher.IV;
                    CreateAccount();
                    //Redirect to Login page
                    Response.Redirect("Login.aspx", false);
                }

            }
            else
            {
                ValidateInput();
                //submitBtn.Enabled = false;
                //Response.Redirect("Registration.aspx", false);
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

        private bool ValidateInput()
        {
            DateTime currentDate = DateTime.Now;

            //Checks if first name is empty
            if (String.IsNullOrEmpty(firstNameTB.Text))
            {
                firstNameError.Text = "Please enter your first name";
                firstNameError.ForeColor = Color.Red;
                firstNameError.Visible = true;
            }
            else if (!Regex.IsMatch(firstNameTB.Text, "^[a-zA-Z0-9 ]*$"))
            {
                firstNameError.Text = "Please enter valid first name";
                firstNameError.ForeColor = Color.Red;
                firstNameError.Visible = true;
            }
            else
            {
                firstNameError.Text = "Excellent";
                firstNameError.ForeColor = Color.Green;
                firstNameError.Visible = true;
            }

            //Checks if last name is empty
            if (String.IsNullOrEmpty(lastNameTB.Text))
            {
                lastNameError.Text = "Please enter your last name";
                lastNameError.ForeColor = Color.Red;
                lastNameError.Visible = true;
            }
            else if (!Regex.IsMatch(lastNameTB.Text, "^[a-zA-Z0-9 ]*$"))
            {
                lastNameError.Text = "Please enter valid last name";
                lastNameError.ForeColor = Color.Red;
                lastNameError.Visible = true;
            }
            else
            {
                lastNameError.Text = "Excellent";
                lastNameError.ForeColor = Color.Green;
                lastNameError.Visible = true;
            }

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
                    emailError.Text = "Excellent";
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

            //Checks if birth date is chosen or not
            if (String.IsNullOrEmpty(dobTB.Text))
            {
                dobTBError.Text = "Please select birth date";
                dobTBError.ForeColor = Color.Red;
                dobTBError.Visible = true;
            }
            /*else if (!Regex.IsMatch(dobTB.Text, "^[a-zA-Z0-9 ]*$"))
            {
                dobTBError.Text = "Please choose a valid birth date";
                dobTBError.ForeColor = Color.Red;
                dobTBError.Visible = true;
            }*/
            else
            {
                dobTBError.Text = "Excellent";
                dobTBError.ForeColor = Color.Green;
                dobTBError.Visible = true;
            }

            //Checks if card name is empty
            if (String.IsNullOrEmpty(nameOnCardTB.Text))
            {
                nameOnCardError.Text = "Please enter name on card";
                nameOnCardError.ForeColor = Color.Red;
                nameOnCardError.Visible = true;
            }
            else if (!Regex.IsMatch(nameOnCardTB.Text, "^[a-zA-Z0-9 ]*$"))
            {
                nameOnCardError.Text = "Please enter valid card name";
                nameOnCardError.ForeColor = Color.Red;
                nameOnCardError.Visible = true;
            }
            else
            {
                nameOnCardError.Text = "Excellent";
                nameOnCardError.ForeColor = Color.Green;
                nameOnCardError.Visible = true;
            }

            //checks if card number is actually numbers     
            //checks if there is other character other than numbers
            if (!Regex.IsMatch(cardNumberTB.Text, "^[0-9]*$"))
            {
                //cardNumberError.Text = "Testing";
                cardNumberError.Text = "Please enter a valid card number";
                cardNumberError.ForeColor = Color.Red;
                cardNumberError.Visible = true;
            }
            //checks if its empty or null
            else if (String.IsNullOrEmpty(cardNumberTB.Text))
            {
                cardNumberError.Text = "Please enter card number";
                cardNumberError.ForeColor = Color.Red;
                cardNumberError.Visible = true;
            }
            else if (cardNumberTB.Text.Length > 16)
            {
                cardNumberError.Text = "Please enter a valid card number";
                cardNumberError.ForeColor = Color.Red;
                cardNumberError.Visible = true;
            }
            else
            {
                cardNumberError.Text = "Excellent";
                cardNumberError.ForeColor = Color.Green;
                cardNumberError.Visible = true;
            }

            //Checks Card CVV Number
            if (!Regex.IsMatch(CVVTB.Text, "^[0-9]*$"))
            {
                CVVError.Text = "Please enter a valid CVV number";
                CVVError.ForeColor = Color.Red;
                CVVError.Visible = true;
            }
            //checks if its empty or null
            else if (String.IsNullOrEmpty(CVVTB.Text))
            {
                CVVError.Text = "Please enter CVV number";
                CVVError.ForeColor = Color.Red;
                CVVError.Visible = true;
            }
            else if (CVVTB.Text.Length != 4)
            {
                CVVError.Text = "Please enter a 4 digit CVV number";
                CVVError.ForeColor = Color.Red;
                CVVError.Visible = true;
            }
            else
            {
                CVVError.Text = "Excellent";
                CVVError.ForeColor = Color.Green;
                CVVError.Visible = true;
            }

            //Checks if card expiry date is chosen or not
            if (String.IsNullOrEmpty(cardExpiryTB.Text))
            {
                cardExpiryError.Text = "Please select card expiry date";
                cardExpiryError.ForeColor = Color.Red;
                cardExpiryError.Visible = true;
            }
            else
            {
                DateTime inputDate = Convert.ToDateTime(cardExpiryTB.Text);
                double monthDifference = inputDate.Subtract(currentDate).Days / (365.25 / 12);
                if (monthDifference < 3)
                {
                    cardExpiryError.Text = "Please ensure that your expiry date is at least 3 months from current date";
                    cardExpiryError.ForeColor = Color.Red;
                    cardExpiryError.Visible = true;
                }
                else
                {
                    //cardExpiryError.Text = monthDifference.ToString();
                    //cardExpiryError.ForeColor = Color.Black;
                    cardExpiryError.Text = "Excellent";
                    cardExpiryError.ForeColor = Color.Green;
                    cardExpiryError.Visible = true;
                }
            }

            //Checks if both passwords are the same
            if (firstPasswordTB.Text != secondPasswordTB.Text)
            {
                secondPasswordError.Text = "Please ensure that both passwords are the same";
                secondPasswordError.Visible = true;
                secondPasswordError.ForeColor = Color.Red;
            }
            else if (String.IsNullOrEmpty(secondPasswordTB.Text))
            {
                secondPasswordError.Text = "Please enter your password again";
                secondPasswordError.Visible = true;
                secondPasswordError.ForeColor = Color.Red;
            }
            else
            {
                firstPasswordError.Text = "Excellent";
                firstPasswordError.ForeColor = Color.Green;
                firstPasswordError.Visible = true;
                secondPasswordError.Text = "Excellent";
                secondPasswordError.ForeColor = Color.Green;
                secondPasswordError.Visible = true;
            }

            var redColor = Color.Red;
            var greenColor = Color.Green;

            //checks if any error labels is green or not
            if (firstNameError.ForeColor == greenColor
                && lastNameError.ForeColor == greenColor
                && emailError.ForeColor == greenColor
                && dobTBError.ForeColor == greenColor
                && cardNumberError.ForeColor == greenColor
                && nameOnCardError.ForeColor == greenColor
                && cardExpiryError.ForeColor == greenColor
                && CVVError.ForeColor == greenColor
                && firstPasswordError.ForeColor == greenColor
                && secondPasswordError.ForeColor == greenColor)
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

                        //This code will be displayed in the output window
                        Debug.WriteLine("--- Testing ---");
                        Debug.WriteLine(jsonObject);
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

        private void CreateAccount()
        {
            try
            {
                //Establishing database connection
                using (SqlConnection con = new SqlConnection(DatabaseConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO userInfo VALUES(@email, @firstName, @lastName, @dob, @nameOnCard, @numberCard, @cvvNumber, @cardExpiry, @passwordHash, @passwordSalt, @lastUpdate, @IV, @Key)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@email", emailTB.Text.Trim());
                            cmd.Parameters.AddWithValue("@firstName", firstNameTB.Text.Trim());
                            cmd.Parameters.AddWithValue("@lastName", lastNameTB.Text.Trim());
                            cmd.Parameters.AddWithValue("@dob", Convert.ToDateTime(dobTB.Text));

                            //Card Info
                            cmd.Parameters.AddWithValue("@nameOnCard", Convert.ToBase64String(encryptData(nameOnCardTB.Text.Trim())));
                            cmd.Parameters.AddWithValue("@numberCard", Convert.ToBase64String(encryptData(cardNumberTB.Text.Trim())));
                            cmd.Parameters.AddWithValue("@cvvNumber", Convert.ToBase64String(encryptData(CVVTB.Text.Trim())));
                            cmd.Parameters.AddWithValue("@cardExpiry", Convert.ToBase64String(encryptData(cardExpiryTB.Text.Trim())));

                            //Password salt & hash
                            cmd.Parameters.AddWithValue("@passwordHash", finalHash);
                            cmd.Parameters.AddWithValue("@passwordSalt", salt);

                            //Last Updated
                            cmd.Parameters.AddWithValue("@lastUpdate", DateTime.Now);

                            //Key and IV
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
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
        }

        protected bool getEmail(string email)
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
    }
}