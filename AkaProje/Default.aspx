<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AkaProje.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
        Hello</p>
    <asp:Button ID="Button1" runat="server" Text="Çıkış" OnClick="Button1_Click" />
    <asp:Button ID="Button2" runat="server" Text="Hata Verdir" OnClick="Button2_Click" />
</asp:Content>
