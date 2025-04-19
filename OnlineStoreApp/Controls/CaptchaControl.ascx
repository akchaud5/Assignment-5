<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CaptchaControl.ascx.cs" Inherits="OnlineStoreApp.Controls.Controls_CaptchaControl" %>

<div>
    <p><img id="imgCaptcha" runat="server" alt="Captcha Image" style="height: 80px; width: 200px; border: 1px solid #ddd;" /></p>
    <p>Enter the text shown above:</p>
    <asp:TextBox ID="txtCaptcha" runat="server"></asp:TextBox>
    <asp:Button ID="btnRefresh" runat="server" Text="Refresh" OnClick="btnRefresh_Click" />
    <asp:RequiredFieldValidator ID="rfvCaptcha" runat="server" ControlToValidate="txtCaptcha"
        ErrorMessage="Captcha is required" Display="Dynamic" ForeColor="Red">
    </asp:RequiredFieldValidator>
</div>