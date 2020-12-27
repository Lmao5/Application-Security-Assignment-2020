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
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Add session stuff here
            //session counter
        }

        private bool ValidateInput()
        {
            return false;
        }
        public bool ValidateCaptcha()
        {
            return false;
        }
    }
}