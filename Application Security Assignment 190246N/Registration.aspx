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
                        <asp:TextBox ID="firstNameTB" runat="server" placeholder="First Name" CssClass="form-control" required></asp:TextBox>
                        <asp:Label ID="firstNameError" runat="server"></asp:Label>
                   </div>
                   <div class="col-md-6">
                       <label for="lastNameLbl" class="form-label">Last Name</label>
                       <asp:TextBox ID="lastNameTB" runat="server" placeholder="First Name" CssClass="form-control" required></asp:TextBox>
                       <asp:Label ID="lastNameError" runat="server"></asp:Label>
                   </div>
               </div>
          <div class="mb-3">
            <label for="emailLabel" class="form-label">Email address</label>
            <asp:TextBox ID="emailTB" runat="server" placeholder="Email Address" type="email" CssClass="form-control" required></asp:TextBox>
            <asp:Label ID="emailError" runat="server"></asp:Label>
          </div>
            <div class="mb-3">
            <label for="dobLbl" class="form-label">Date of Birth (DD MM YYYY)</label>
            <asp:TextBox ID="dobTB" runat="server" placeholder="Date of Birth" type="date" CssClass="form-control" required></asp:TextBox>
            <asp:Label ID="dobTBError" runat="server"></asp:Label>
          </div>
            <div class="mb-3">
            <label for="nameOnCard" class="form-label">Name On Card</label>
            <asp:TextBox ID="nameOnCardTB" runat="server" placeholder="Name On Card" CssClass="form-control" required></asp:TextBox>
            <asp:Label ID="nameOnCardError" runat="server"></asp:Label>
          </div>
           <div class="mb-3">
            <label for="creditCardNumber" class="form-label">Credit Card Number</label>
            <asp:TextBox ID="cardNumberTB" runat="server" placeholder="Card Number" CssClass="form-control" required></asp:TextBox>
            <asp:Label ID="cardNumberError" runat="server"></asp:Label>
          </div>
               <div class="row mb-3">
                   <div class="col-md-6">
                        <label for="cardExpiryLbl" class="form-label">Credit Card Expiry (MM YYYY)</label>
                        <asp:TextBox ID="cardExpiryTB" runat="server" type="month" placeholder="Card Expiry (MM YY)" CssClass="form-control" required></asp:TextBox>
                        <asp:Label ID="cardExpiryError" runat="server"></asp:Label>
                   </div>
                   <div class="col-md-6">
                       <label for="cvvLbl" class="form-label">CVV Number</label>
                        <asp:TextBox ID="CVVTB" runat="server" placeholder="CVV Number" CssClass="form-control" required></asp:TextBox>
                        <asp:Label ID="CVVError" runat="server"></asp:Label>
                   </div>
               </div>
          <div class="mb-3">
            <label for="firstPassword" class="form-label">Password</label>
            <asp:TextBox ID="firstPasswordTB" runat="server" placeholder="Password" CssClass="form-control" type="password" required></asp:TextBox>
            <asp:Label ID="firstPasswordError" runat="server"></asp:Label>
          </div>
            <div class="mb-3">
            <label for="secondPassword" class="form-label">Re-enter Password</label>
            <asp:TextBox ID="secondPasswordTB" runat="server" placeholder="Re-enter Password" CssClass="form-control" type="password" required></asp:TextBox>
            <asp:Label ID="secondPasswordError" runat="server"></asp:Label>
            </div>
          <asp:Button ID="submitBtn" runat="server" Text="Submit" CssClass="btn btn-primary btn-style" OnClick="submitBtn_Click"/>
        </form>
            </div>
            <div class="col-md-4"></div>
        </div>
     </div>
</asp:Content>
