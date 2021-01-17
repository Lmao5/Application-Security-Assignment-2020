<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="changePassword.aspx.cs" Inherits="Application_Security_Assignment_190246N.changePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContent" runat="server">
    <br />
    <br />
    <div class="mt-5">
        <h1 class="mb-3">Change Password</h1>
        <div class="mt-5">
            <div class="row">
                <div class="col-md-4">
                    <form id="loginForm">
                        <p>
                            <asp:Label ID="errorMsg" runat="server"></asp:Label>
                        </p>
                        <div class="mb-3">
                            <label for="currentPasswordLbl" class="form-label">Current Password:</label>
                            <asp:TextBox ID="currentPasswordTB" runat="server" placeholder="Current Password" CssClass="form-control" type="password" onkeyup="javascript:currentValidatePassword()"></asp:TextBox>
                            <asp:Label ID="currentPasswordError" runat="server"></asp:Label>
                        </div>
                        <div class="mb-3">
                            <label for="newPasswordLbl" class="form-label">New Password:</label>
                            <asp:TextBox ID="newPasswordTB" runat="server" placeholder="New Password" CssClass="form-control" type="password" onkeyup="javascript:ValidatePassword()"></asp:TextBox>
                            <asp:Label ID="newPasswordError" runat="server"></asp:Label>
                        </div>
                        <div class="mb-3">
                            <label for="newPasswordLbl2" class="form-label">Re-type Password:</label>
                            <asp:TextBox ID="newPasswordTB2" runat="server" placeholder="Re-type Password" CssClass="form-control" type="password" onkeyup="javascript:ValidatePassword()"></asp:TextBox>
                            <asp:Label ID="newPasswordError2" runat="server"></asp:Label>
                        </div>
                        <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response" />
                        <asp:Button ID="submitBtn" runat="server" Text="Submit" CssClass="btn btn-primary btn-style" />
                    </form>
                </div>
            </div>
            <div class="col-md-8"></div>
        </div>
    </div>
    <script type="text/javascript">
        function currentValidatePassword() {
            var currentPassword = document.getElementById('<%=currentPasswordTB.ClientID%>').value;

            //Minimum length is 8 because all passwords are longer than 8 characters
            if (currentPassword.length < 8) {
                document.getElementById('<%=currentPasswordError.ClientID%>').innerHTML = "Please enter a valid password";
                document.getElementById('<%=currentPasswordError.ClientID%>').style.color = "Red";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
                return "Password is invalid"
            }
            else {
                document.getElementById('<%=currentPasswordError.ClientID%>').innerHTML = "Excellent";
                document.getElementById('<%=currentPasswordError.ClientID%>').style.color = "Green";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = false;
                return "Password is valid"
            }
        }
    </script>
    <!--- New Password Validation --->
    <script type="text/javascript">
        function ValidatePassword() {
            var str = document.getElementById('<%=newPasswordTB.ClientID %>').value;
            var str2 = document.getElementById('<%=newPasswordTB2.ClientID%>').value;

            if (str.length <= 0) {
                document.getElementById('<%=newPasswordError.ClientID%>').innerHTML = "Password length must be at least 8 characters";
                document.getElementById('<%=newPasswordError.ClientID%>').style.color = "Red";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
                return "Too Short"
            }
            else if (str.search(/[0-9]/) == -1) {
                document.getElementById('<%=newPasswordError.ClientID%>').innerHTML = "Password must contain at least 1 number";
                document.getElementById('<%=newPasswordError.ClientID%>').style.color = "Red";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
                return "No number"
            }
            else if (str.search(/[$&+,:;=?@#|'<>.^*()%!-]/) == -1) {
                document.getElementById('<%=newPasswordError.ClientID%>').innerHTML = "Password must contain at least 1 special character";
                document.getElementById('<%=newPasswordError.ClientID%>').style.color = "Red";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
                return "No special character";
            }
            else if (str.search(/[A-Z]/) == -1) {
                document.getElementById('<%=newPasswordError.ClientID%>').innerHTML = "Password must contain at least 1 uppercase letter";
                document.getElementById('<%=newPasswordError.ClientID%>').style.color = "Red";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
                return "No uppercase characters";
            }
            else if (str.search(/[a-z]/) == -1) {
                document.getElementById('<%=newPasswordError.ClientID%>').innerHTML = "Password must contain at least 1 lowercase letter";
                document.getElementById('<%=newPasswordError.ClientID%>').style.color = "Red";
                document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
                return "No lowercase characters";
            }
            else {
                if (str != str2) {
                    document.getElementById('<%=newPasswordError.ClientID%>').innerHTML = "Excellent";
                       document.getElementById('<%=newPasswordError.ClientID%>').style.color = "Green";
                       document.getElementById('<%=newPasswordError2.ClientID%>').innerHTML = "Ensure that both passwords are the same";
                       document.getElementById('<%=newPasswordError2.ClientID%>').style.color = "Red";
                       document.getElementById('<%=submitBtn.ClientID%>').disabled = true;
                       return "Passwords are not the same";
                   }
                   else {
                       document.getElementById('<%=newPasswordError.ClientID%>').innerHTML = "Excellent";
                       document.getElementById('<%=newPasswordError.ClientID%>').style.color = "Green";
                       document.getElementById('<%=newPasswordError2.ClientID%>').innerHTML = "Excellent";
                       document.getElementById('<%=newPasswordError2.ClientID%>').style.color = "Green";
                       document.getElementById('<%=submitBtn.ClientID%>').disabled = false;
                }
            }
        }
    </script>
</asp:Content>
