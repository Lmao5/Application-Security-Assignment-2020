<%@ Page Title="User Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="Application_Security_Assignment_190246N.User" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContent" runat="server">
    <br />
    <br />
    <div class="mt-5">
        <h1 class="mb-3">Welcome back
            <asp:Label ID="userName" runat="server"></asp:Label>!</h1>
        <div class="mt-5">
            <div class="row">
                <div class="col-md-8">
                    <p>What would you like to do?</p>
                    <asp:Button ID="changePassBtn" runat="server" Text="Change Password" CssClass="btn btn-primary btn-style" OnClick="changePassBtn_Click" />
                    <asp:Button ID="logoutBtn" runat="server" Text="Logout" CssClass="btn btn-primary btn-style" OnClick="logoutBtn_Click" />
                </div>
                <div class="col-md-4">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
