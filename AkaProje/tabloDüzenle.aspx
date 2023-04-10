<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="tabloDüzenle.aspx.cs" Inherits="AkaProje.tabloDüzenle" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/dinamik.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <h2 class="display-4 text-center">Tablo Düzenleme</h2>
     <div class="m-5">
         <colspan>Düzenlenecek Tabloyu Seçin</colspan>
        <asp:DropDownList ID="ddlTablolar" runat="server" AutoPostBack="true">
            <asp:ListItem Text="Select" Value=""></asp:ListItem>
        </asp:DropDownList>
        <asp:Button ID="btnTabloDüzenle" CssClass="btn btn-primary" runat="server" Text="Tabloyu Düzenle" OnClick="btnTabloDüzenle_Click" />
         <asp:Panel ID="pnlControl" runat="server">

         </asp:Panel>
     </div>
</asp:Content>
