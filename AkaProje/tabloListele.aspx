<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="tabloListele.aspx.cs" Inherits="AkaProje.tabloListele" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/dinamik.css" rel="stylesheet" />
</asp:Content>
 
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   

 <h2 class="display-4 text-center">Tablo Listeleme</h2>
     <div class="m-5">         
         <p>Listelemek İstediğiniz Tabloyu Seçiniz.</p>
         
         <colspan>Tablolarım</colspan>
        <asp:DropDownList ID="ddlTablolar" runat="server" AutoPostBack="true">
            <asp:ListItem Text="Select" Value=""></asp:ListItem>
        </asp:DropDownList>
        <asp:Button ID="btnListele" CssClass="btn btn-primary" runat="server" Text="Tabloyu Gör" OnClick="btnListele_Click" />
    </div>
    <div class="m-5">
        <asp:GridView ID="gridView" runat="server" AutoGenerateInsertButton="true" CellPadding="4" ForeColor="#333333" GridLines="None" Width="317px" OnSelectedIndexChanged="OnSelectedIndexChanged" OnRowCancelingEdit="OnRowCancel" OnRowDeleting="onRowDeleting" OnRowEditing="onEdit" OnRowUpdating="onRowUpdating"  >
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:CommandField ShowEditButton="True" />
                <asp:CommandField ShowDeleteButton="True" />
                <asp:CommandField ShowSelectButton="True" />
            </Columns>
            <EditRowStyle BackColor="#999999" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#E9E7E2" />
            <SortedAscendingHeaderStyle BackColor="#506C8C" />
            <SortedDescendingCellStyle BackColor="#FFFDF8" />
            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
        </asp:GridView>            
                <asp:Button ID="btnVeriEkle" CssClass=" mt-3 btn btn-warning" runat="server" Text="Veri Ekle" OnClick="btnVeriEkle_Click" />      
                <asp:Panel ID="pnlControl2" runat="server">
                </asp:Panel>
        <div class="col-md-12 text-center">
        <asp:Button ID="btnKaydet" CssClass=" mt-5 btn btn-success" runat="server" Text="Kaydet" OnClick="btnKaydet_Click" />
    </div>
    </div>
    </asp:Content>
