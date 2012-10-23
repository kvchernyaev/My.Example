<%@ Page Title="Мой профиль" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyProfile.aspx.cs"
    Inherits="My.Example.Web.MyProfile" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <table style="border: 0;" rules="none" border="0" cellpadding="3">
        <tr>
            <td valign="top" style="vert-align: top; vertical-align: top;">

                <table cellpadding="0" cellspacing="0" rules="none" border="0" style="border: 0;">
                    <tr>
                        <td style="vertical-align: top;">

                            <fieldset style="text-align: left;">
                                <legend>Личные данные</legend>

                                <table cellpadding="5" rules="none" border="0" style="border: 0;">
                                    <tr>
                                        <td>
                                            Id пользователя
                                        </td>
                                        <td>
                                            <asp:HyperLink runat="server" ID="hpUserId" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Логин
                                        </td>
                                        <td>
                                            <asp:Literal runat="server" ID="lLogin" Mode="Encode" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            ФИО
                                        </td>
                                        <td>
                                            <asp:Literal runat="server" ID="lFIO" Mode="Encode" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Email
                                        </td>
                                        <td>
                                            <asp:Literal runat="server" ID="lEmail" Mode="PassThrough" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Телефон
                                        </td>
                                        <td>
                                            <asp:Literal runat="server" ID="lTelephone" Mode="Encode" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Факс
                                        </td>
                                        <td>
                                            <asp:Literal runat="server" ID="lFax" Mode="Encode" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Роли
                                        </td>
                                        <td>

                                            <asp:ListView runat="server" ID="lvRoles" DataSourceID="odsRoles">
                                                <EmptyDataTemplate>
                                                    Нет ни одной роли
                                                </EmptyDataTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%# Eval("Caption") %>' ToolTip='<%# Eval("ToolTip") %>' ForeColor='<%# Eval("ForeColor") %>' />
                                                </ItemTemplate>
                                                <ItemSeparatorTemplate>
                                                    <span>, </span>
                                                </ItemSeparatorTemplate>
                                            </asp:ListView>

                                            <asp:ObjectDataSource runat="server" ID="odsRoles" TypeName="My.Example.Web.MyProfile" SelectMethod="SelectUserRoles"
                                                OnObjectCreating="ods_ObjectCreating" OnObjectDisposing="ods_ObjectDisposing" />

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Дата регистрации
                                        </td>
                                        <td>
                                            <asp:Literal runat="server" ID="lRegistrationDate" Mode="PassThrough" />
                                        </td>
                                    </tr>
                                </table>

                            </fieldset>

                        </td>
                    </tr>
                </table>

                <fieldset style="text-align: center;">
                    <legend>Поменять пароль</legend>

                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>

                            <table style="text-align: left; width: 1px; border: 0;" cellspacing="5" rules="none" border="0">
                                <tr>
                                    <td style="white-space: nowrap;">
                                        Введите старый пароль:
                                    </td>
                                    <td style="white-space: nowrap;">
                                        <asp:TextBox ID="tbOldPsw" TextMode="Password" runat="server" />
                                        <asp:CustomValidator runat="server" ID="vOlPsw" ControlToValidate="tbOldPsw" Display="Dynamic" ForeColor="Red"
                                            OnServerValidate="vOlPsw_ServerValidate" ToolTip="Неправильный текущий пароль" ValidationGroup="ChangePswValGroup">*</asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="white-space: nowrap;">
                                        Введите новый пароль:
                                    </td>
                                    <td style="white-space: nowrap;">
                                        <asp:TextBox ID="tbNewPsw" TextMode="Password" runat="server" />
                                        <asp:RequiredFieldValidator runat="server" InitialValue="" Text="*" Display="Dynamic" ControlToValidate="tbNewPsw"
                                            ForeColor="Red" ToolTip="Пароль не может быть пустым" ValidationGroup="ChangePswValGroup" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Подтверждение:
                                    </td>
                                    <td style="white-space: nowrap;">
                                        <asp:TextBox ID="tbNewPswRepeat" TextMode="Password" runat="server" />
                                        <asp:RequiredFieldValidator runat="server" InitialValue="" Text="*" Display="Dynamic" ForeColor="Red" ControlToValidate="tbNewPswRepeat"
                                            ToolTip="Пароль не может быть пустым" ValidationGroup="ChangePswValGroup" />
                                        <asp:CompareValidator runat="server" ControlToValidate="tbNewPswRepeat" ControlToCompare="tbNewPsw" Operator="Equal"
                                            Text="*" Display="Dynamic" Type="String" ForeColor="Red" ToolTip="Подтверждение пароля не совпадает с новым паролем"
                                            ValidationGroup="ChangePswValGroup" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="bSubmitNewPass" runat="server" Text="Сохранить" OnClick="bSubmitNewPass_Click" ValidationGroup="ChangePswValGroup"
                                            OnClientClick="javascript:if(!confirm('Действительно изменить пароль?')) return false;" />
                                    </td>
                                </tr>
                            </table>

                        </ContentTemplate>
                    </asp:UpdatePanel>

                </fieldset>

            </td>
        </tr>
    </table>

</asp:Content>

