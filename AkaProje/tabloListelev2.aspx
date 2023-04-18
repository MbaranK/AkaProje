<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="tabloListelev2.aspx.cs" Inherits="AkaProje.tabloListelev2" %>

<%@ Register Assembly="DevExpress.Web.v21.2, Version=21.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/dinamik.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="display-4 text-center">Tablo Listeleme DevExpress</h2>
     <div class="m-5">         
         <p>Listelemek İstediğiniz Tabloyu Seçiniz.</p>
         
         <colspan>Tablolarım</colspan>
        <asp:DropDownList ID="ddlTablolar" runat="server" AutoPostBack="true">
            <asp:ListItem Text="Select" Value=""></asp:ListItem>
        </asp:DropDownList>
        <asp:Button ID="btnListele" CssClass="btn btn-primary" runat="server" Text="Tabloyu Gör" OnClick="btnListele_Click" />
         <%--<asp:Button ID="btnVeriEkle" CssClass=" mx-3 btn btn-warning" runat="server" Text="Veri Ekle" OnClick="btnVeriEkle_Click" />--%>
    </div>
    <dx:ASPxGridView ID="ASPxGridView1" runat="server">
        <Columns>
        <dx:GridViewCommandColumn VisibleIndex="0" ShowEditButton="True" ShowDeleteButton="True" ShowNewButton="True"/>
            </Columns>
    </dx:ASPxGridView>
</asp:Content>
