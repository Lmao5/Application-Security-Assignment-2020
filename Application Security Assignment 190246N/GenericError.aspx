<%@ Page Title="Generic Error" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GenericError.aspx.cs" Inherits="Application_Security_Assignment_190246N.GenericError" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContent" runat="server">
    <br />
    <br />
    <div class="mt-5 container">
        <div class="text-center">
            <h1 class="mb-4">Something went wrong somewhere...</h1>
            <p class="mb-4">You can go back to the home page</p>
            <asp:Button ID="backBtn" runat="server" Text="Home" CssClass="btn btn-primary btn-style" OnClick="backBtn_Click" />
        </div>
    </div>
</asp:Content>
