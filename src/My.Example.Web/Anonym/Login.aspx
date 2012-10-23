<%@ Page Title="Авторизация" Language="C#" MasterPageFile="~/Anonym/Anonym.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs"
    Inherits="My.Example.Web.Anonym.Login" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center; height: 33%; width: 33%; position: absolute; top: 33%; left: 33%;">
        <asp:Login ID="LoginComponent" runat="server" EnableViewState="false" FailureText="Ваш логин или пароль неверный."
            LoginButtonText="Войти" PasswordLabelText="Пароль:" PasswordRequiredErrorMessage="Требуется пароль" RememberMeText="Запомнить меня"
            TitleText="" UserNameLabelText="Логин:" UserNameRequiredErrorMessage="Требуется логин." Width="100%" OnAuthenticate="LoginComponent_Authenticate"
            RememberMeSet="True">
            <LayoutTemplate>
                <div style="background-image: url('/Images/title_back.png'); background-repeat: no-repeat; height: 200px; padding-top: 5px;
                    text-align: left; vertical-align: middle; width: 400px; padding-left: 30px;">
                    <div style="text-align: left; font-size: 1em; color: #A9AEB1;">
                        Введите логин и пароль:
                    </div>
                    <div style="text-align: left;">
                        <asp:TextBox ID="UserName" runat="server" Width="170px" Style="margin: 1px;" />
                    </div>
                    <div style="text-align: left;">
                        <asp:TextBox ID="Password" runat="server" TextMode="Password" Width="170px" Style="margin: 1px;" />
                    </div>
                    <div style="text-align: right">
                        <div style="margin-right: 15px; margin-top: 2px;" align="left">
                            <asp:CheckBox ID="RememberMe" runat="server" Text="Запомнить меня " ForeColor="#69737D" Checked="True" />
                            &nbsp;
                            <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Войти" ValidationGroup="loginValidation" />
                            &nbsp;
                            <asp:HyperLink href="ForgetPassword.aspx" runat="server">Забыли пароль?</asp:HyperLink>
                        </div>
                    </div>
                    <div align="center" style="color: Red;">
                        <asp:Literal ID="FailureText" runat="server" />
                    </div>
                </div>
            </LayoutTemplate>
        </asp:Login>
    </div>
</asp:Content>

