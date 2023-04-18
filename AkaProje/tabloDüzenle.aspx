<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" UnobtrusiveValidationMode="None" AutoEventWireup="true" CodeBehind="tabloDüzenle.aspx.cs" Inherits="AkaProje.tabloDüzenle" %>

<%@ Register Assembly="DevExpress.Web.v21.2, Version=21.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
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
