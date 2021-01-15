<%@ Page Title="404 Error" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="404.aspx.cs" Inherits="Application_Security_Assignment_190246N._404" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContent" runat="server">
    <br />
    <br />
    <div class="mt-5 container">
        <div class="text-center">
            <h1 class="mb-4">404 Error</h1>
            <asp:Button ID="backBtn" runat="server" Text="Home" CssClass="btn btn-primary btn-style" OnClick="backBtn_Click"/>
        </div>
    </div>
</asp:Content>
