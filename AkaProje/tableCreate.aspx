<%@ Page Title="" Language="C#" UnobtrusiveValidationMode="None" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="tableCreate.aspx.cs" Inherits="AkaProje.tableCreate" %>

<%@ Register Assembly="DevExpress.Web.v21.2, Version=21.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/dinamik.css" rel="stylesheet" />
    <link href="css/bootstrap.min.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p class=" display-4 mb-5 text-center">Tablo Oluşturma Sayfası</p>
 <div class="m-5">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <div>
        <colspan>Tablo İsmi: </colspan>
        <asp:TextBox ID="txtTablo" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="txtTablo" ForeColor="Red">Tablo İsmi Boş Olamaz</asp:RequiredFieldValidator>
            
        <colspan>Kolon Sayısı: </colspan>
        <asp:TextBox ID="txtTekrar" runat="server" Width="68px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="txtTekrar" ForeColor="Red">Geçerli Bir Sayı Giriniz.</asp:RequiredFieldValidator>
        <asp:Button ID="btnCogalt" CssClass=" m-2 btn btn-primary" runat="server" Text="Oluştur" OnClick="btnCogalt_Click" />
        </div>   
            <asp:Panel ID="pnlControls" runat="server">
    <!-- DİNAMİK GİRİŞ -->
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <div class="col-md-12 text-center">
        <asp:Button ID="Button1" CssClass=" mt-5 btn btn-success" runat="server" Text="Aktar" OnClick="Button1_Click" />
    </div>
  </div>     
</asp:Content>

