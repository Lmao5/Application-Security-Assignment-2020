<%@ Page Title="Registration" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="Application_Security_Assignment_190246N.Registration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContent" runat="server">
    <br />
    <br />
    <h1>Registration</h1>
    <div class="mt-5">
        <div class="row">
            <div class="col-md-8">
           <form id="registrationForm">
               <div class="mb-3 row">
                   <div class="col-md-6">
                        <label for="firstNameLbl" class="form-label">First Name</label>
                        <asp:TextBox ID="firstNameTB" runat="server" placeholder="First Name" CssClass="form-control" onkeyup ="validateFirstName()"></asp:TextBox>
                        <asp:Label ID="firstNameError" runat="server"></asp:Label>
                   </div>
                   <div class="col-md-6">
                       <label for="lastNameLbl" class="form-label">Last Name</label>
                       <asp:TextBox ID="lastNameTB" runat="server" placeholder="First Name" CssClass="form-control" onkeyup="validateLastName()"></asp:TextBox>
                       <asp:Label ID="lastNameError" runat="server"></asp:Label>
                   </div>
               </div>
          <div class="mb-3">
            <label for="emailLabel" class="form-label">Email address</label>
            <asp:TextBox ID="emailTB" runat="server" placeholder="Email Address" type="email" CssClass="form-control" onkeyup="ValidateEmail()" required></asp:TextBox>
            <asp:Label ID="emailError" runat="server"></asp:Label>
          </div>
            <div class="mb-3">
            <label for="dobLbl" class="form-label">Date of Birth (DD MM YYYY)</label>
            <asp:TextBox ID="dobTB" runat="server" placeholder="Date of Birth" type="date" CssClass="form-control" onkeyup="dobValidation()"></asp:TextBox>
            <asp:Label ID="dobTBError" runat="server"></asp:Label>
          </div>
            <div class="mb-3">
            <label for="nameOnCard" class="form-label">Name On Card</label>
            <asp:TextBox ID="nameOnCardTB" runat="server" placeholder="Name On Card" CssClass="form-control" onkeyup="nameOnCardValidation()"></asp:TextBox>
            <asp:Label ID="nameOnCardError" runat="server"></asp:Label>
          </div>
           <div class="mb-3">
            <label for="creditCardNumber" class="form-label">Credit Card Number</label>
            <asp:TextBox ID="cardNumberTB" runat="server" placeholder="Card Number" CssClass="form-control" onkeyup="cardNumberValidation()"></asp:TextBox>
            <asp:Label ID="cardNumberError" runat="server"></asp:Label>
          </div>
               <div class="row mb-3">
                   <div class="col-md-6">
                        <label for="cardExpiryLbl" class="form-label">Credit Card Expiry (MM YYYY)</label>
                        <asp:TextBox ID="cardExpiryTB" runat="server" type="month" placeholder="Card Expiry (MM YY)" CssClass="form-control" onkeyup="cardExpiryValidation()"></asp:TextBox>
                        <asp:Label ID="cardExpiryError" runat="server"></asp:Label>
                   </div>
                   <div class="col-md-6">
                       <label for="cvvLbl" class="form-label">CVV Number</label>
                        <asp:TextBox ID="CVVTB" runat="server" placeholder="CVV Number" CssClass="form-control" onkeyup="cvvNumberValidation()"></asp:TextBox>
                        <asp:Label ID="CVVError" runat="server"></asp:Label>
                   </div>
               </div>
          <div class="mb-3">
            <label for="firstPassword" class="form-label">Password</label>
            <asp:TextBox ID="firstPasswordTB" runat="server" placeholder="Password" CssClass="form-control" type="password" onkeyup="javascript:ValidatePassword()"></asp:TextBox>
            <asp:Label ID="firstPasswordError" runat="server"></asp:Label>
          </div>
            <div class="mb-3">
            <label for="secondPassword" class="form-label">Re-enter Password</label>
            <asp:TextBox ID="secondPasswordTB" runat="server" placeholder="Re-enter Password" CssClass="form-control" type="password" onkeyup="javascript:ValidatePassword()"></asp:TextBox>
            <asp:Label ID="secondPasswordError" runat="server"></asp:Label>
            </div>
           <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
          <asp:Button ID="submitBtn" runat="server" Text="Submit" CssClass="btn btn-primary btn-style" OnClick="submitBtn_Click"/>
        </form>
            </div>
            <div class="col-md-4"></div>
        </div>
     </div>
        <!--- firstName Validation --->
    <script type="text/javascript">
        function validateFirstName() {
            var firstName = document.getElementById('<%=firstNameTB.ClientID%>').value;
            //console.log(firstName);

            if (firstName.length <= 0) {
                document.getElementById('<%=firstNameError.ClientID%>').innerHTML = "Please enter a valid first name";
                document.getElementById('<%=firstNameError.ClientID%>').style.color = "Red";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
            }
            //ensures that no special characters are in the firstnameTB
            else if (firstName.search(/[`!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?~]/) != -1) {
                document.getElementById('<%=firstNameError.ClientID%>').innerHTML = "Please enter a valid first name";
                document.getElementById('<%=firstNameError.ClientID%>').style.color = "Red";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
            }
            else {
                document.getElementById('<%=firstNameError.ClientID%>').innerHTML = "Excellent";
                document.getElementById('<%=firstNameError.ClientID%>').style.color = "Green";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = false;
            }
        }
    </script>
            <!--- lastName Validation --->
    <script type="text/javascript">
        function validateLastName()
        {
            var lastName = document.getElementById('<%=lastNameTB.ClientID%>').value;
            if (lastName.length <= 0) {
                document.getElementById('<%=lastNameError.ClientID%>').innerHTML = "Please enter a valid last name";
                document.getElementById('<%=lastNameError.ClientID%>').style.color = "Red";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
            }
            else if (lastName.search(/[`!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?~]/) != -1) {
                document.getElementById('<%=lastNameError.ClientID%>').innerHTML = "Please enter a valid first name";
                document.getElementById('<%=lastNameError.ClientID%>').style.color = "Red";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
            }
            else {
                document.getElementById('<%=lastNameError.ClientID%>').innerHTML = "Excellent";
                document.getElementById('<%=lastNameError.ClientID%>').style.color = "Green";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = false;
            }
        }
    </script>
    <!--- Email Validation --->
    <script type="text/javascript">
        function ValidateEmail() {
            var email = document.getElementById('<%=emailTB.ClientID%>').value;
            //console.log("testing");
            //console.log(email.search(/[@]/));
            if (email.length <= 0) {
                document.getElementById('<%=emailError.ClientID%>').innerHTML = "Please enter your email address";
                document.getElementById('<%=emailError.ClientID%>').style.color = "Red";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
            }
            else if (email.search(/[@]/) == -1) {
                document.getElementById('<%=emailError.ClientID%>').innerHTML = "Please enter a valid email address";
                document.getElementById('<%=emailError.ClientID%>').style.color = "Red";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
            }
            else {
                document.getElementById('<%=emailError.ClientID%>').innerHTML = "Excellent";
                document.getElementById('<%=emailError.ClientID%>').style.color = "Green";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = false;
            }
        }
    </script>
    <!--- Date of Birth Validation --->
    <script type="text/javascript">
        function dobValidation() {
            var dob = document.getElementById('<%=dobTB.ClientID%>').value;

            if (dob.length <= 0) {
                document.getElementById('<%=dobTBError.ClientID%>').innerHTML = "Please choose your date of birth";
                document.getElementById('<%=dobTBError.ClientID%>').style.color = "Red";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
            }
            else {
                document.getElementById('<%=dobTBError.ClientID%>').innerHTML = "Excellent";
                document.getElementById('<%=dobTBError.ClientID%>').style.color = "Green";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = false;
            }
        }
    </script>
    <!--- nameOnCard Validation --->
    <script type="text/javascript">
        function nameOnCardValidation() {
            var nameOnCard = document.getElementById('<%=nameOnCardTB.ClientID%>').value;
            console.log(nameOnCard.search(/[`!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?~]/));
            if (nameOnCard.length <= 0) {
                document.getElementById('<%=nameOnCardError.ClientID%>').innerHTML = "Please enter your name on card";
                document.getElementById('<%=nameOnCardError.ClientID%>').style.color = "Red";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
            }
            //ensures that no special characters are in the cardnameTB
            else if (nameOnCard.search(/[`!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?~]/) != -1) {
                document.getElementById('<%=nameOnCardError.ClientID%>').innerHTML = "Please enter a valid name on card";
                document.getElementById('<%=nameOnCardError.ClientID%>').style.color = "Red";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
            }
            else {
                document.getElementById('<%=nameOnCardError.ClientID%>').innerHTML = "Excellent";
                document.getElementById('<%=nameOnCardError.ClientID%>').style.color = "Green";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = false;
            }
        }
    </script>
    <!--- cardNumber Validation --->
    <script type="text/javascript">
        function cardNumberValidation() {
            var cardNumber = document.getElementById('<%=cardNumberTB.ClientID%>').value;

            if (cardNumber.length <= 0) {
                document.getElementById('<%=cardNumberError.ClientID%>').innerHTML = "Please enter your card number";
                document.getElementById('<%=cardNumberError.ClientID%>').style.color = "Red";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
            }
            else if (cardNumber.length != 16) {
                document.getElementById('<%=cardNumberError.ClientID%>').innerHTML = "Please enter your 16-digit card number";
                document.getElementById('<%=cardNumberError.ClientID%>').style.color = "Red";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
            }
            else if (cardNumber.search(/[ `!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?~]/) != -1) {
                document.getElementById('<%=cardNumberError.ClientID%>').innerHTML = "Please enter a valid 16-digit card number";
                document.getElementById('<%=cardNumberError.ClientID%>').style.color = "Red";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
            }
            else if (cardNumber.search(/[A-Z]/) != -1) {
                document.getElementById('<%=cardNumberError.ClientID%>').innerHTML = "Please enter a valid 16-digit card number";
                document.getElementById('<%=cardNumberError.ClientID%>').style.color = "Red";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
            }
            else if (cardNumber.search(/[a-z]/) != -1) {
                document.getElementById('<%=cardNumberError.ClientID%>').innerHTML = "Please enter a valid 16-digit card number";
                document.getElementById('<%=cardNumberError.ClientID%>').style.color = "Red";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
            }
            else{
                document.getElementById('<%=cardNumberError.ClientID%>').innerHTML = "Excellent";
                document.getElementById('<%=cardNumberError.ClientID%>').style.color = "Green";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = false;
            }
        }
    </script>
    <!--- cardExpiry Validation --->
    <script type="text/javascript">
        function cardExpiryValidation() {
            var cardExpiry = document.getElementById('<%=cardExpiryTB.ClientID%>').value;
            var currentDate = new Date();
            var monthDifference = currentDate.getMonth() - cardExpiry.getMonth();


            if (cardExpiry.length <= 0) {
                document.getElementById('<%=cardExpiryError.ClientID%>').innerHTML = "Please choose your card expiry date";
                document.getElementById('<%=cardExpiryError.ClientID%>').style.color = "Red";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
            }
            else if (currentDate.getMonth() > cardExpiry.getMonth()) {
                document.getElementById('<%=cardExpiryError.ClientID%>').innerHTML = "Please ensure that your card is not expired";
                document.getElementById('<%=cardExpiryError.ClientID%>').style.color = "Red";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
            }
            else if (monthDifference < 3) {
                document.getElementById('<%=cardExpiryError.ClientID%>').innerHTML = "Please ensure that your card expiries 3 months from now";
                document.getElementById('<%=cardExpiryError.ClientID%>').style.color = "Red";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
            }
            else {
                document.getElementById('<%=cardExpiryError.ClientID%>').innerHTML = "Excellent";
                document.getElementById('<%=cardExpiryError.ClientID%>').style.color = "Green";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = false;
            }

        }
    </script>
    <!--- cvvNumber Validation --->
    <script type="text/javascript">
        function cvvNumberValidation() {
            var cvvNumber = document.getElementById('<%=CVVTB.ClientID%>').value;

            if (cvvNumber.length <= 0) {
                document.getElementById('<%=CVVError.ClientID%>').innerHTML = "Please enter your CVV number";
                document.getElementById('<%=CVVError.ClientID%>').style.color = "Red";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
            }
            else if (cvvNumber.length != 4) {
                document.getElementById('<%=CVVError.ClientID%>').innerHTML = "Please enter your 4-digit CVV number";
                document.getElementById('<%=CVVError.ClientID%>').style.color = "Red";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
            }
            else if (cvvNumber.search(/[ `!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?~]/) != -1) {
                document.getElementById('<%=CVVError.ClientID%>').innerHTML = "Please enter a valid 4-digit CVV number";
                document.getElementById('<%=CVVError.ClientID%>').style.color = "Red";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
            }
            else if (cvvNumber.search(/[A-Z]/) != -1) {
                document.getElementById('<%=CVVError.ClientID%>').innerHTML = "Please enter a valid 4-digit CVV number";
                document.getElementById('<%=CVVError.ClientID%>').style.color = "Red";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
            }
            else if (cvvNumber.search(/[a-z]/) != -1) {
                document.getElementById('<%=CVVError.ClientID%>').innerHTML = "Please enter a valid 4-digit CVV number";
                document.getElementById('<%=CVVError.ClientID%>').style.color = "Red";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
            }
            else{
                document.getElementById('<%=CVVError.ClientID%>').innerHTML = "Excellent";
                document.getElementById('<%=CVVError.ClientID%>').style.color = "Green";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = false;
            }
        }
    </script>
    <!--- Password Validation --->
       <script type="text/javascript">
           function ValidatePassword() {
               var str = document.getElementById('<%=firstPasswordTB.ClientID %>').value;
               var str2 = document.getElementById('<%=secondPasswordTB.ClientID%>').value;

               if (str.length <= 0) {
                   document.getElementById('<%=firstPasswordError.ClientID%>').innerHTML = "Password length must be at least 8 characters";
                   document.getElementById('<%=firstPasswordError.ClientID%>').style.color = "Red";
                   document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
                   return "Too Short"
               }
               else if (str.search(/[0-9]/) == -1) {
                   document.getElementById('<%=firstPasswordError.ClientID%>').innerHTML = "Password must contain at least 1 number";
                   document.getElementById('<%=firstPasswordError.ClientID%>').style.color = "Red";
                   document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
                   return "No number"
               }
               else if (str.search(/[$&+,:;=?@#|'<>.^*()%!-]/) == -1) {
                   document.getElementById('<%=firstPasswordError.ClientID%>').innerHTML = "Password must contain at least 1 special character";
                   document.getElementById('<%=firstPasswordError.ClientID%>').style.color = "Red";
                   document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
                   return "No special character";
               }
               else if (str.search(/[A-Z]/) == -1) {
                   document.getElementById('<%=firstPasswordError.ClientID%>').innerHTML = "Password must contain at least 1 uppercase letter";
                   document.getElementById('<%=firstPasswordError.ClientID%>').style.color = "Red";
                   document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
                   return "No uppercase characters";
               }
               else if (str.search(/[a-z]/) == -1) {
                   document.getElementById('<%=firstPasswordError.ClientID%>').innerHTML = "Password must contain at least 1 lowercase letter";
                   document.getElementById('<%=firstPasswordError.ClientID%>').style.color = "Red";
                   document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
                   return "No lowercase characters";
               }
               else {
                   if (str != str2) {
                       document.getElementById('<%=firstPasswordError.ClientID%>').innerHTML = "Excellent";
                       document.getElementById('<%=firstPasswordError.ClientID%>').style.color = "Green";
                       document.getElementById('<%=secondPasswordError.ClientID%>').innerHTML = "Ensure that both passwords are the same";
                       document.getElementById('<%=secondPasswordError.ClientID%>').style.color = "Red";
                       document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
                       return "Passwords are not the same";
                   }
                   else {
                       document.getElementById('<%=firstPasswordError.ClientID%>').innerHTML = "Excellent";
                       document.getElementById('<%=firstPasswordError.ClientID%>').style.color = "Green";
                       document.getElementById('<%=secondPasswordError.ClientID%>').innerHTML = "Excellent";
                       document.getElementById('<%=secondPasswordError.ClientID%>').style.color = "Green";
                       document.getElementById('<%=submitBtn.ClientID%>').disabled = false;
                   }
               }
           }
       </script>
</asp:Content>
