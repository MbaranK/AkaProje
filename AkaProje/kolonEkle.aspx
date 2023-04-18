<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" UnobtrusiveValidationMode="None" AutoEventWireup="true" CodeBehind="kolonEkle.aspx.cs" Inherits="AkaProje.kolonEkle" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/dinamik.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="display-4 text-center">Kolon Ekleme</h2>
     <div class="m-5">
         <colspan>Kolon Eklenecek Tabloyu Seçin</colspan>
        <asp:DropDownList ID="ddlTablolar" runat="server" AutoPostBack="true">
            <asp:ListItem Text="Select" Value=""></asp:ListItem>
        </asp:DropDownList>

         <asp:Button ID="btnKolonGetir" CssClass="btn btn-success" runat="server" Text="Kolon Oluştur" OnClick="btnKolonGetir_Click" />
         
         <asp:Panel ID="pnlControl" runat="server">

         </asp:Panel>
         </div>
   
</asp:Content>
