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
                <div class="mb-3">
                <label for="emailLabel" class="form-label">Email address</label>
                <asp:TextBox ID="emailTB" runat="server" placeholder="Email Address" type="email" CssClass="form-control" ></asp:TextBox>
                <asp:Label ID="emailError" runat="server"></asp:Label>
                </div>
                <div class="mb-3">
                <label for="passwordLbl" class="form-label">Password</label>
                <asp:TextBox ID="passwordTB" runat="server" placeholder="Password" CssClass="form-control" type="password" onkeyup="javascript:Validate()"></asp:TextBox>
                <asp:Label ID="passwordError" runat="server"></asp:Label>
                </div>
                <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
                <asp:Button ID="submitBtn" runat="server" Text="Submit" CssClass="btn btn-primary btn-style" OnClick="submitBtn_Click"/>
            </form>
        </div>
        <div class="col-md-8">
        </div>
    </div>
</asp:Content>
