<%@ Page Language="C#" AutoEventWireup="true" UnobtrusiveValidationMode="None" CodeBehind="Login.aspx.cs" Inherits="AkaProje.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Giriş</title>
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
    <script src="https://kit.fontawesome.com/8e76246130.js" crossorigin="anonymous"></script>
    <link href="css/register.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-aFq/bzH65dt+w6FI2ooMVUpc+21e0SRygnTpmBvdBgSdnuTN7QbdgL+OapgHtvPp" crossorigin="anonymous"/>
    
</head>
<body>
    <form class="form-horizontal" id="form1" runat="server">
        <div class="customform">
            <h2>Giriş Ekranı</h2>
           <div>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtKullanici" ErrorMessage="Kullanıcı Adı boş bırakılamaz" ForeColor="Red"></asp:RequiredFieldValidator>
                <asp:TextBox ID="txtKullanici" CssClass="form-control" placeholder="Kullanıcı Adı" runat="server"></asp:TextBox>
            </div>
             <div>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtSifre" ErrorMessage="Şifre boş bırakılamaz" ForeColor="Red"></asp:RequiredFieldValidator>
                <asp:TextBox ID="txtSifre" CssClass="form-control" placeholder="Şifre" runat="server" TextMode="Password"></asp:TextBox>
            </div>
            <div>
                <asp:CheckBox ID="chkRemember" Text="Beni Hatırla" runat="server" OnCheckedChanged="chkRemember_CheckedChanged" />
            </div>
            
            <div class="mt-3">
                <asp:Button ID="btnGiris" CssClass="btn btn-success btn-med" runat="server" Text="Giriş Yap" OnClick="btnGiris_Click" /> 
                <asp:Button ID="btnKayit" CssClass="btn btn-warning btn-med" runat="server" Text="Kayıt Ol" CausesValidation="False" PostBackUrl="~/Register.aspx" />
            </div>
        </div>
    </form>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha2/dist/js/bootstrap.bundle.min.js" integrity="sha384-qKXV1j0HvMUeCBQ+QVp7JcfGl760yU08IQ+GpUo5hlbpg51QRiuqHAJz8+BrxE/N" crossorigin="anonymous"></script>
</body>
</html>
