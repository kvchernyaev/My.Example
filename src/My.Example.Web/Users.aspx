<%@ Page Title="Пользователи" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs"
    Inherits="My.Example.Web.Users" %>
<%@ Import Namespace="My.Common" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <uc:UpdateProgress runat="server" ID="up" />
    <asp:Button runat="server" Text="Выделить меня" ID="bLocateMe" OnClick="bLocateMe_OnClick" />

    <asp:UpdatePanel ID="upUsers" runat="server">
        <ContentTemplate>

            <fieldset>
                <legend>Фильтр</legend>
                <span title="Id, логин, ФИО, email, телефон, факс">Строка поиска: &nbsp;
                    <asp:TextBox runat="server" ID="tbSearchString" AutoPostBack="True" OnTextChanged="FilterChanged" /></span>
                <br />

                <table style="border: 0; width: 100%;" rules="none" border="0">
                    <tr>
                        <td style="text-align: left;">

                            <table cellpadding="5" style="vertical-align: top;" rules="cols">
                                <tr>
                                    <td style="vertical-align: top;">
                                        Роли:
                                        <br />
                                        <asp:CheckBox ID="cbRoleAny" runat="server" Text="любая" Checked="True" onclick="javascript: if ($('.RolesFilterCheckBox input').attr('checked') == 'checked') $('.RolesFilterArea').hide(); else  $('.RolesFilterArea').show();"
                                            CssClass="RolesFilterCheckBox" AutoPostBack="True" OnCheckedChanged="FilterChanged" />
                                        <br />
                                        <span class="RolesFilterArea" style='<%= cbRoleAny.Checked?"display: none;": "display: inline-block;"%>'>
                                            <ucc:CheckBoxListEx AutoPostBack="True" runat="server" ID="cblRolesFilter" DataSourceID="odsRoles" DataTextField="Caption"
                                                DataValueField="Id" OnSelectedIndexChanged="FilterChanged" SkinID="Filter" />
                                        </span>
                                    </td>
                                    <td style="vertical-align: top; width: 1px;">
                                        Активность

                                        <asp:DropDownList runat="server" ID="ddlActivity" AutoPostBack="True" OnSelectedIndexChanged="FilterChanged" DataSourceID="odsActivity"
                                            DataTextField="Caption" DataValueField="Id" />

                                        <asp:ObjectDataSource runat="server" ID="odsActivity" TypeName="My.Example.Web.Users" SelectMethod="SelectActivity"
                                            OnObjectCreating="ods_ObjectCreating" OnObjectDisposing="ods_ObjectDisposing" />

                                    </td>
                                    <td style="vertical-align: top; width: 1px;">
                                        Дата&nbsp;рег.
                                        <div style="white-space: nowrap;">
                                            <asp:CheckBox ID="cbCreatedDateAny" runat="server" Text="любая" Checked="True" onclick="javascript: if ($('.CreatedDateFilterCheckBox input').attr('checked') == 'checked') $('.CreatedDateFilterArea').hide(); else  $('.CreatedDateFilterArea').show();"
                                                CssClass="CreatedDateFilterCheckBox" AutoPostBack="True" OnCheckedChanged="FilterChanged" />
                                        </div>
                                        <div class="CreatedDateFilterArea" style='<%= cbCreatedDateAny.Checked?"display: none;": "display: inline-block;"%>'>
                                            <div style="white-space: nowrap;">
                                                От:
                                                <asp:TextBox runat="server" ID="tbBeginDate" Width="80" AutoPostBack="True" OnTextChanged="FilterChanged" />
                                                <asp:ImageButton runat="server" ID="bBeginDate" ImageUrl="~/Images/calendar.png" />
                                                <ajaxToolkit:CalendarExtender ID="calFrom" runat="server" TargetControlID="tbBeginDate" PopupButtonID="bBeginDate"
                                                    Format="dd.MM.yyyy" />
                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="tbBeginDate" Text="*" ToolTip="Укажите начальную дату"
                                                    ValidationGroup="UseCreatedDateFilter" />
                                                <asp:CompareValidator runat="server" ControlToValidate="tbBeginDate" Type="Date" Text="*" ToolTip="Формат даты - дд.мм.гггг"
                                                    Operator="DataTypeCheck" ValidationGroup="UseCreatedDateFilter" />
                                            </div>
                                            <div style="white-space: nowrap;">
                                                До:
                                                <asp:TextBox runat="server" ID="tbEndDate" Width="80" AutoPostBack="True" OnTextChanged="FilterChanged" />
                                                <asp:ImageButton runat="server" ID="bEndDate" ImageUrl="~/Images/calendar.png" />
                                                <ajaxToolkit:CalendarExtender ID="calTo" runat="server" TargetControlID="tbEndDate" PopupButtonID="bEndDate" Format="dd.MM.yyyy" />
                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="tbEndDate" Text="*" ToolTip="Укажите конечную дату"
                                                    ValidationGroup="UseCreatedDateFilter" />
                                                <asp:CompareValidator runat="server" ControlToValidate="tbEndDate" Type="Date" Text="*" Operator="DataTypeCheck"
                                                    ToolTip="Формат даты - дд.мм.гггг" ValidationGroup="UseCreatedDateFilter" />
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>

                        </td>
                        <td style="text-align: right; vert-align: bottom; vertical-align: bottom;">
                            <asp:HyperLink runat="server" Text="Очистить" NavigateUrl="~/Users.aspx" />
                            <br />
                            <asp:HyperLink runat="server" ID="hlFilterLink" Text="Ссылка" ToolTip="Ссылка с установленным выбранным фильтром" />
                        </td>
                    </tr>
                </table>

                <asp:ObjectDataSource runat="server" ID="odsRoles" TypeName="My.Example.Web.Users" SelectMethod="SelectUserRoles"
                    OnObjectCreating="ods_ObjectCreating" OnObjectDisposing="ods_ObjectDisposing" />

            </fieldset>

            <table style="border: 0; width: 100%;" cellpadding="0" cellspacing="0" rules="none">
                <tr>
                    <td style="text-align: left;">
                        Найдено пользователей:
                        <asp:Label runat="server" ID="lFoundCount" />
                    </td>
                    <td style="text-align: right;">
                        <asp:Button runat="server" ID="bRefresh" Text="Обновить" OnClick="bRefresh_OnClick" />
                    </td>
                </tr>
            </table>

            <br />
            <ucc:CountChooser runat="server" ID="cc" Counts="10,20,50,100,200" Title="Выводить на странице:" AutoPostBack="True"
                OnSelectedIndexChanged="FilterChanged" />

            <asp:GridView runat="server" ID="gvUsers" AllowSorting="True" AllowPaging="True" DataSourceID="odsUsers" OnRowDataBound="gvUsers_OnRowDataBound"
                OnRowCreated="gvUsers_OnRowCreated">
                <EmptyDataTemplate>
                    Не найдено ни одного пользователя
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderText="№" SortExpression="UserId">
                        <HeaderStyle Wrap="False" />
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("UserId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Логин" SortExpression="Login">
                        <HeaderStyle Wrap="False" />
                        <ItemStyle Wrap="False" />
                        <ItemTemplate>
                            <asp:HyperLink runat="server" NavigateUrl='<%# string.Format("~/EditUser.aspx?UserId={0}", Eval("UserId")) %>'
                                Text='<%# Eval("Login") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ФИО" SortExpression="UserFIO">
                        <HeaderStyle Wrap="False" />
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# string.IsNullOrEmpty((string) Eval("UserFIO"))? "-":  Eval("UserFIO") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Телефон" SortExpression="Telephone">
                        <HeaderStyle Wrap="False" />
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# string.IsNullOrEmpty((string) Eval("Telephone"))? "-":TelForHtml ((string)Eval("Telephone")) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Email" SortExpression="Email">
                        <HeaderStyle Wrap="False" />
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# string.IsNullOrEmpty((string) Eval("Email"))? "-":  Eval("Email") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Факс" SortExpression="Fax">
                        <HeaderStyle Wrap="False" />
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# string.IsNullOrEmpty((string) Eval("Fax"))? "-": TelForHtml((string)  Eval("Fax")) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Акт." SortExpression="IsActive">
                        <HeaderStyle Wrap="False" />
                        <ItemTemplate>
                            <asp:CheckBox runat="server" Checked='<%# Eval("IsActive") %>' Enabled="False" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Роли">
                        <HeaderStyle Wrap="False" />
                        <ItemTemplate>

                            <asp:ListView runat="server" ID="lvRoles">
                                <EmptyDataTemplate>
                                    <span style="color: gray;">нет ни одной роли</span>
                                </EmptyDataTemplate>
                                <ItemTemplate>
                                    <span style="white-space: nowrap;">
                                        <asp:Label runat="server" Text='<%# Eval("Caption") %>' ToolTip='<%# Eval("ToolTip") %>' ForeColor='<%# Eval("ForeColor") %>' />
                                    </span>
                                </ItemTemplate>
                                <ItemSeparatorTemplate>
                                    ,
                                </ItemSeparatorTemplate>
                            </asp:ListView>

                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Дата рег." SortExpression="CreatedDate">
                        <HeaderStyle Wrap="False" />
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Parser.DateTimeForUI(Eval("CreatedDate"), true) %>' ToolTip='<%# Parser.DateTimeForUI(Eval("CreatedDate"), isForTextBox:true) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Акт.">
                        <ItemStyle Wrap="False" />
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# ((DateTime)Eval("LastActivityDate"))==default(DateTime)? "-" : Parser.DateTimeForUI(Eval("LastActivityDate"), true, omitDateIfToday:true) %>'
                                ToolTip='<%# ((DateTime)Eval("LastActivityDate"))==default(DateTime)? "информации нет" : Parser.DateTimeForUI(Eval("LastActivityDate"), isForTextBox:true)  %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Смена пароля">
                        <ItemStyle Wrap="False" />
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# ((DateTime)Eval("LastPasswordChangeDate"))==default(DateTime)? "-" : Parser.DateTimeForUI(Eval("LastPasswordChangeDate"), true, omitDateIfToday:true) %>'
                                ToolTip='<%# ((DateTime)Eval("LastPasswordChangeDate"))==default(DateTime)? "информации нет" : Parser.DateTimeForUI(Eval("LastPasswordChangeDate"), isForTextBox:true)  %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <asp:ObjectDataSource runat="server" ID="odsUsers" TypeName="My.Example.Web.Users" SelectMethod="Select" OnObjectCreating="ods_ObjectCreating"
                OnObjectDisposing="ods_ObjectDisposing" EnablePaging="True" MaximumRowsParameterName="count" SelectCountMethod="SelectCount"
                SortParameterName="orderBy" StartRowIndexParameterName="startIndex" />

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

