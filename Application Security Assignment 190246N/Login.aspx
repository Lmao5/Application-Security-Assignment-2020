<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Application_Security_Assignment_190246N.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContent" runat="server">
    <br />
    <br />
    <h1>Login</h1>
    <div class="row mt-5 mb-5">
        <div class="col-md-4">
            <form id="loginForm">
                <p><asp:Label ID="errorMsg" runat="server"></asp:Label></p>
                <div class="mb-3">
                <label for="emailLabel" class="form-label">Email address</label>
                <asp:TextBox ID="emailTB" runat="server" placeholder="Email Address" type="email" CssClass="form-control" onkeyup="javascript:validateEmail()"></asp:TextBox>
                <asp:Label ID="emailError" runat="server"></asp:Label>
                </div>
                <div class="mb-3">
                <label for="passwordLbl" class="form-label">Password</label>
                <asp:TextBox ID="passwordTB" runat="server" placeholder="Password" CssClass="form-control" type="password" onkeyup="javascript:validatePassword()"></asp:TextBox>
                <asp:Label ID="passwordError" runat="server"></asp:Label>
                </div>
                <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
                <asp:Button ID="submitBtn" runat="server" Text="Submit" CssClass="btn btn-primary btn-style" OnClick="submitBtn_Click"/>
            </form>
        </div>
        <div class="col-md-8">
        </div>
    </div>
    <script type="text/javascript">
        function validateEmail() {
            var email = document.getElementById('<%=emailTB.ClientID%>').value;

            //Checks if email has a least the minimum length of 4
            if (email.length < 4) {
                document.getElementById('<%=emailError.ClientID%>').innerHTML = "Please enter a valid email address";
                document.getElementById('<%=emailError.ClientID%>').style.color = "Red";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
                return "Email is non-existant";
            }
            else {
                document.getElementById('<%=emailError.ClientID%>').innerHTML = "Excellent";
                document.getElementById('<%=emailError.ClientID%>').style.color = "Green";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = false;
                return "Email exists";
            }
        }
    </script>
    <script type="text/javascript">
        function validatePassword() {
            var password = document.getElementById('<%=passwordTB.ClientID%>').value;

            //Minimum length is 8 because all passwords are longer than 8 characters
            if (password.length < 8) {
                document.getElementById('<%=passwordError.ClientID%>').innerHTML = "Please enter a valid password";
                document.getElementById('<%=passwordError.ClientID%>').style.color = "Red";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
                return "Password is invalid"
            }
            else {
                document.getElementById('<%=passwordError.ClientID%>').innerHTML = "Excellent";
                document.getElementById('<%=passwordError.ClientID%>').style.color = "Green";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = false;
                return "Password is valid"
            }
        }
    </script>
</asp:Content>
