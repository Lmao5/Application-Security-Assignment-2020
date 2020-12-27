using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Drawing;

namespace Application_Security_Assignment_190246N
{
    public partial class Registration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //User needs to input all correct information before sending off
            submitBtn.Enabled = false;
        }

        protected void submitBtn_Click(object sender, EventArgs e)
        {
            bool validInput = ValidateInput();

            if(validInput == true)
            {
                submitBtn.Enabled = true;
            }
            else
            {
                submitBtn.Enabled = false;
            }


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
                firstNameError.Text = "";
                firstNameError.Visible = false;
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
                lastNameError.Text = "";
                lastNameError.Visible = false;
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
                if(isEmail == true)
                {
                    emailError.Text = "";
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
            else if (!Regex.IsMatch(dobTB.Text, "^[a-zA-Z0-9 ]*$"))
            {
                dobTBError.Text = "Please choose a valid birth date";
                dobTBError.ForeColor = Color.Red;
                dobTBError.Visible = true;
            }
            else
            {
                dobTBError.Text = "";
                dobTBError.Visible = false;
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
                nameOnCardError.Text = "";
                nameOnCardError.Visible = false;
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
                cardNumberError.Text = "";
                cardNumberError.Visible = false;
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
                CVVError.Text = "";
                CVVError.Visible = false;
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
                    cardExpiryError.Text = "";
                    cardExpiryError.Visible = false;
                }
            }

            //Checks if both passwords are the same
            if(firstPasswordTB.Text != secondPasswordTB.Text)
            {
                secondPasswordError.Text = "Please ensure that both passwords are the same";
                secondPasswordError.Visible = true;
                secondPasswordError.ForeColor = Color.Red;
            }
            else
            {
                secondPasswordError.Text = "";
                secondPasswordError.Visible = false;
            }

            //checks if any error labels is empty or not
            if (String.IsNullOrEmpty(cardNumberError.Text)
                && String.IsNullOrEmpty(nameOnCardError.Text)
                && String.IsNullOrEmpty(cardExpiryError.Text)
                && String.IsNullOrEmpty(CVVError.Text))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool ValidateCaptcha()
        {
            return false;
        }
    }
}