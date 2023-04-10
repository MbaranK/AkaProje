<%@ Page Language="C#" AutoEventWireup="true" UnobtrusiveValidationMode="None" CodeBehind="Register.aspx.cs" Inherits="AkaProje.Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Kayıt Ol</title>
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
    <link href="css/register.css" rel="stylesheet" />
    <link href="//cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/css/toastr.min.css" rel="stylesheet" />
    <script src="//cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/js/toastr.min.js"></script>
    <link href="https://cdn.jsdelivr.net/ndynamic tables input by userpm/bootstrap@5.3.0-alpha2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-aFq/bzH65dt+w6FI2ooMVUpc+21e0SRygnTpmBvdBgSdnuTN7QbdgL+OapgHtvPp" crossorigin="anonymous"/>
     <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-aFq/bzH65dt+w6FI2ooMVUpc+21e0SRygnTpmBvdBgSdnuTN7QbdgL+OapgHtvPp" crossorigin="anonymous"/>
    
</head>
<body>
    <form id="form1" runat="server">   
        <div class="customform">
            <h2 class="text-center">Kayıt Ekranı</h2>
            <div class="form-group">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Display="Dynamic" runat="server" ErrorMessage="Ad Geçersiz" ForeColor="Red" ControlToValidate="txtAd" CssClass="alert-danger" ValidationExpression="^[^0-9]+$"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"  ControlToValidate="txtAd" ErrorMessage="Ad boş bırakılamaz" ForeColor="Red"></asp:RequiredFieldValidator>
            <asp:TextBox ID="txtAd" CssClass="form-control" placeholder="Adınızı giriniz" runat="server"></asp:TextBox>
                
            </div>
             <div class="form-group">
                 <asp:RegularExpressionValidator ID="RegularExpressionValidator4" Display="Dynamic" runat="server" ErrorMessage="Soyad Geçersiz" ForeColor="Red" ControlToValidate="txtSoyad" CssClass="alert-danger" ValidationExpression="^[^0-9]+$"></asp:RegularExpressionValidator>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"  ControlToValidate="txtSoyad" ErrorMessage="Soyad boş bırakılamaz" ForeColor="Red"></asp:RequiredFieldValidator>
            <asp:TextBox ID="txtSoyad" CssClass="form-control" placeholder="Soyadınızı giriniz" runat="server"></asp:TextBox>      
             </div>
            
            <div class="form-group">
                 <asp:RegularExpressionValidator ID="RegularExpressionValidator3" Display="Dynamic" runat="server" ControlToValidate="txtEmail" CssClass="alert-danger" ErrorMessage="Email adresi geçersiz" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="AllValidationGroup"></asp:RegularExpressionValidator>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"  ControlToValidate="txtEmail" ErrorMessage="Email boş bırakılamaz" ForeColor="Red"></asp:RequiredFieldValidator>
            <asp:TextBox ID="txtEmail" CssClass="form-control" placeholder="Email adresinizi giriniz." runat="server" TextMode="Email"></asp:TextBox>
            </div>
           
            <div class="form-group">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" Display="Dynamic" runat="server"  CssClass="alert-danger" ErrorMessage="Kullanıcı Adı geçersiz" ControlToValidate="txtKullaniciAdi" ValidationExpression="^[a-z0-9_-]+$" ValidationGroup="AllValidationGroup" ></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"  ControlToValidate="txtKullaniciAdi" ErrorMessage="Kullanıcı Adı boş bırakılamaz" ForeColor="Red"></asp:RequiredFieldValidator>
            <asp:TextBox ID="txtKullaniciAdi" CssClass="form-control" placeholder="Kullanıcı Adı giriniz." runat="server"></asp:TextBox>
            </div>
                <div class="form-group">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtSifre" ErrorMessage="Şifre boş bırakılamaz" ForeColor="Red"></asp:RequiredFieldValidator>
            <asp:TextBox ID="txtSifre" CssClass="form-control" placeholder="Şifre giriniz" runat="server" TextMode="Password"></asp:TextBox>
            </div>
            <div>              
                    <asp:Button ID="btnGeri" CssClass="mt-3 btn btn-med btn-danger" runat="server" Text="Geri Dön" CausesValidation="False" PostBackUrl="~/Login.aspx" />
                    <asp:Button ID="btnKayit" CssClass="mt-3 btn btn-med btn-warning" runat="server" Text="Kayıt Ol" OnClick="btnKayit_Click" />              
            </div>        
        </div>
    </form>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha2/dist/js/bootstrap.bundle.min.js" integrity="sha384-qKXV1j0HvMUeCBQ+QVp7JcfGl760yU08IQ+GpUo5hlbpg51QRiuqHAJz8+BrxE/N" crossorigin="anonymous"></script>

</body>
</html>
