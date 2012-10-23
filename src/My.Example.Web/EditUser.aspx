<%@ Page Title="Пользователь {0}" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditUser.aspx.cs"
    Inherits="My.Example.Web.EditUser" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <asp:Panel runat="server" ID="allArea">

        <table cellpadding="5" border="0" rules="none" style="border: 0; width: 100%;">
            <tr>
                <td style="vertical-align: top; width: 1px;">

                    <fieldset style="text-align: left;">
                        <legend>Личные данные</legend>

                        <asp:Panel runat="server" DefaultButton="bSaveUser">

                            <table cellpadding="5" style="border: 0; width: 1px;" rules="none" border="0">
                                <tr>
                                    <td>
                                        Логин
                                    </td>
                                    <td style="white-space: nowrap;">
                                        <asp:TextBox runat="server" ID="tbLogin" Width="300px" />
                                        <asp:RequiredFieldValidator runat="server" InitialValue="" Text="*" Display="Dynamic" ControlToValidate="tbLogin"
                                            ForeColor="Red" ToolTip="Не указан логин" ValidationGroup="EditUser" />
                                        <asp:CustomValidator runat="server" ID="vLoginExists" Text="*" ForeColor="Red" ControlToValidate="tbLogin" Display="Dynamic"
                                            ToolTip="Логин занят" ValidationGroup="EditUser" OnServerValidate="vLoginExists_ServerValidate" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        ФИО
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="tbFIO" Width="300px" />
                                        <asp:RequiredFieldValidator runat="server" InitialValue="" Text="*" Display="Dynamic" ControlToValidate="tbFIO"
                                            ForeColor="Red" ToolTip="Не указаны ФИО" ValidationGroup="EditUser" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Email
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="tbEmail" Width="300px" />
                                        <asp:RequiredFieldValidator runat="server" InitialValue="" Text="*" Display="Dynamic" ControlToValidate="tbEmail"
                                            ForeColor="Red" ToolTip="Не указан email" ValidationGroup="EditUser" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Телефон
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="tbTelephone" Width="300px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Факс
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="tbFax" Width="300px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Роли
                                    </td>
                                    <td>
                                        <ucc:CheckBoxListEx runat="server" ID="cblRoles" DataTextField="Caption" DataValueField="Id" DataCheckField="Checked"
                                            ToolTipField="ToolTip" ForeColorField="Color" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Активен
                                    </td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="cbIsActive" Checked="True" />
                                    </td>
                                </tr>
                            </table>

                            <asp:Button runat="server" ID="bSaveUser" Text="Сохранить изменения" ValidationGroup="EditUser" OnClick="bSaveUser_Click"
                                OnClientClick="javascript:if(!confirm('Сохранить изменения свойств пользователя?')) return false;" />
                        </asp:Panel>

                        <div>
                            Дата создания
                            <asp:Label runat="server" ID="lCreatedDate" />
                        </div>

                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <ContentTemplate>

                                <asp:Panel runat="server" ID="changepswArea" DefaultButton="bChangePsw">
                                    <br />
                                    <br />

                                    <table style="border: 0;" rules="none" border="0">
                                        <tr>
                                            <td style="white-space: nowrap; padding-bottom: 0; margin-bottom: 0;">
                                                Пароль:
                                            </td>
                                            <td style="white-space: nowrap; padding-bottom: 0; margin-bottom: 0;">
                                                <asp:TextBox ID="tbPsw" TextMode="Password" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding-top: 0; margin-top: 0;">
                                                Подтверждение пароля:
                                            </td>
                                            <td style="white-space: nowrap; padding-top: 0; margin-top: 0;">
                                                <asp:TextBox ID="tbPswRepeat" TextMode="Password" runat="server" />

                                                <script type="text/javascript" language="javascript">
                                                    function checkPswRepeat(source, args) {
                                                        args.IsValid = $('#<%= tbPsw.ClientID %>').val() == $('#<%= tbPswRepeat.ClientID %>').val();
                                                    }
                                                </script>

                                                <asp:CustomValidator runat="server" Text="*" ForeColor="Red" Display="Dynamic" ToolTip="Подтверждение пароля неверное"
                                                    ValidationGroup="Psw" OnServerValidate="vLoginRepeat_ServerValidate" ClientValidationFunction="checkPswRepeat" />
                                            </td>
                                        </tr>
                                    </table>

                                    <asp:Button runat="server" ID="bChangePsw" Text="Изменить пароль" ValidationGroup="Psw" OnClick="bChangePsw_Click"
                                        OnClientClick="javascript:return confirm('Вы действительно хотите изменить пароль у пользователя {0}?');" />
                                </asp:Panel>

                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </fieldset>

                </td>
            </tr>
        </table>

    </asp:Panel>

    <asp:Panel runat="server" ID="areaAdminOnly">

        <table style="border: 0; width: 100%;" cellpadding="0" cellspacing="0" rules="none">
            <tr>
                <td style="text-align: left; vertical-align: top;">
                    <a href="<%= ResolveUrl("~/Activity.aspx?FilterUsers=" + Ps.UserId) %>">Активность этого пользователя</a>
                </td>
                <td style="text-align: right; vertical-align: top;" runat="server" id="areaDeleting">
                    <asp:Button runat="server" ID="bDelete" Text="Удалить этого пользователя" OnClick="bDelete_OnClick" OnClientClick="javascript:return confirm('Удалить этого пользователя???');" />
                    <br />
                    <asp:Label runat="server" ID="lDeletingError" ForeColor="Red" Visible="False" />
                </td>
            </tr>
        </table>

    </asp:Panel>

</asp:Content>

