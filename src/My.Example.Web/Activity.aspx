<%@ Page Title="Активность пользователей" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Activity.aspx.cs"
    Inherits="My.Example.Web.Activity" %>
<%@ Import Namespace="My.Common" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <uc:UpdateProgress runat="server" ID="up" />

    <asp:UpdatePanel ID="upSuppliers" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <fieldset>
                <legend>Фильтр</legend>
                <div title="Url, браузер, IP-адрес" style="white-space: nowrap;">
                    Строка поиска: &nbsp;
                    <asp:TextBox runat="server" ID="tbSearchString" AutoPostBack="True" OnTextChanged="FilterChanged" Width="300" /></div>

                <table style="border: 0; width: 100%;" rules="none" border="0">
                    <tr>
                        <td style="text-align: left;">

                            <table cellpadding="5" style="vertical-align: top;" rules="cols">
                                <tr>
                                    <td style="vertical-align: text-top;">
                                        Пользователь:
                                        <asp:CheckBox ID="cbUserAny" runat="server" Text="любой" Checked="False" onclick="javascript: if ($('.UserFilterCheckBox input').attr('checked') == 'checked') $('.UserFilterArea').hide(); else  $('.UserFilterArea').show();"
                                            CssClass="UserFilterCheckBox" OnCheckedChanged="FilterChanged" AutoPostBack="True" />
                                        <br />
                                        <span class="UserFilterArea" style='<%= cbUserAny.Checked ? "display: none;": "display: inline-block;" %>'>
                                            <asp:TextBox runat="server" ID="tbUserFilter" AutoPostBack="True" OnTextChanged="FilterChanged" ToolTip="Номер пользователя, логин, часть фио, можно перечислить через запятую" />
                                            <br />
                                            <asp:CheckBox runat="server" ID="cbNotMe" Text="Исключить вас" Checked="True" OnCheckedChanged="FilterChanged"
                                                AutoPostBack="True" />
                                        </span>
                                    </td>
                                    <td style="vertical-align: text-top;">
                                        Время:
                                        <asp:CheckBox ID="cbCreatedTimeAny" runat="server" Text="любое" Checked="False" onclick="javascript: if ($('.CreatedTimeFilterCheckBox input').attr('checked') == 'checked') $('.CreatedTimeFilterArea').hide(); else  $('.CreatedTimeFilterArea').show();"
                                            CssClass="CreatedTimeFilterCheckBox" OnCheckedChanged="FilterChanged" AutoPostBack="True" />
                                        <br />
                                        <span class="CreatedTimeFilterArea" style='<%= cbCreatedTimeAny.Checked ? "display: none;": "display: inline-block;" %>'>
                                            От:
                                            <asp:TextBox runat="server" ID="tbBeginCreatedTime" Width="120" OnTextChanged="FilterChanged" AutoPostBack="True" />
                                            <asp:RegularExpressionValidator runat="server" ControlToValidate="tbBeginCreatedTime" Text="*" ValidationExpression="^((((31[\/\.](0?[13578]|1[02]))|((29|30)[\/\.](0?[1,3-9]|1[0-2])))[\/\.](1[6-9]|[2-9]\d)?\d{2})|(29[\/\.]0?2[\/\.](((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))|(0?[1-9]|1\d|2[0-8])[\/\.]((0?[1-9])|(1[0-2]))[\/\.]((1[6-9]|[2-9]\d)?\d{2})) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$"
                                                ToolTip="Формат времени - дд.ММ.гггг чч:мм:сс" ValidationGroup="Filter" ForeColor="red" />
                                            <br />
                                            До:
                                            <asp:TextBox runat="server" ID="tbEndCreatedTime" Width="120" OnTextChanged="FilterChanged" AutoPostBack="True" />
                                            <asp:RegularExpressionValidator runat="server" ControlToValidate="tbEndCreatedTime" Text="*" ValidationExpression="^((((31[\/\.](0?[13578]|1[02]))|((29|30)[\/\.](0?[1,3-9]|1[0-2])))[\/\.](1[6-9]|[2-9]\d)?\d{2})|(29[\/\.]0?2[\/\.](((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))|(0?[1-9]|1\d|2[0-8])[\/\.]((0?[1-9])|(1[0-2]))[\/\.]((1[6-9]|[2-9]\d)?\d{2})) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$"
                                                ToolTip="Формат времени - дд.ММ.гггг чч:мм:сс" ValidationGroup="Filter" ForeColor="red" />
                                        </span>
                                    </td>
                                    <td style="vertical-align: text-top;">
                                        IsPostBack

                                        <asp:DropDownList runat="server" ID="ddlIsPostBack" OnSelectedIndexChanged="FilterChanged" AutoPostBack="True">
                                            <asp:ListItem Value="-">Не важно</asp:ListItem>
                                            <asp:ListItem Value="true">+</asp:ListItem>
                                            <asp:ListItem Value="false" Selected="True">f5</asp:ListItem>
                                        </asp:DropDownList>

                                    </td>
                                </tr>
                            </table>

                        </td>
                        <td style="text-align: right; vert-align: bottom; vertical-align: bottom;">
                            <asp:HyperLink runat="server" Text="Очистить" NavigateUrl="~/Activity.aspx" />
                            <br />
                            <asp:HyperLink runat="server" ID="hlFilterLink" Text="Ссылка" ToolTip="Ссылка с установленным выбранным фильтром" />
                        </td>
                    </tr>
                </table>

            </fieldset>

            <table style="border: 0; width: 100%;" cellpadding="0" cellspacing="0" rules="none">
                <tr>
                    <td style="text-align: left;">
                        Найдено записей:
                        <asp:Label runat="server" ID="lFoundCount" />
                    </td>
                    <td style="text-align: right;">
                        <asp:Button runat="server" ID="bRefresh" Text="Обновить" OnClick="bRefresh_OnClick" />
                    </td>
                </tr>
            </table>

            <br />
            <ucc:CountChooser runat="server" ID="cc" Counts="10,20,50,100,200,500,1000,2000" Title="Выводить на странице:"
                AutoPostBack="True" OnSelectedIndexChanged="FilterChanged" />

            <asp:GridView runat="server" ID="gvAct" AllowSorting="True" AllowPaging="True" DataSourceID="odsAct">
                <EmptyDataTemplate>
                    Нет найдено ни одной записи
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderText="Пользователь" SortExpression="UserId">
                        <HeaderStyle Wrap="False" />
                        <ItemTemplate>

                            <table style="width: 100%;" cellpadding="0" cellspacing="0" border="0" rules="none">
                                <tr>
                                    <td style="horiz-align: left; text-align: left;">
                                        <asp:HyperLink runat="server" Text='<%# Eval("Login") %>' ToolTip='<%# string.Format("{0} ({1})", Eval("UserFio"), Eval("UserId")) %>'
                                            NavigateUrl='<%# "~/EditUser.aspx?UserId=" + Eval("UserId") %>' />
                                    </td>
                                    <td style="horiz-align: right; text-align: right;">
                                        &nbsp;<asp:ImageButton runat="server" ID="bFilterByUser" ImageUrl="~/Images/anchor.png" CommandArgument='<%# Eval("UserId") %>'
                                            OnClick="bFilterByUser_OnClick" ToolTip="Отфильтровать по этому пользователю" />
                                    </td>
                                </tr>
                            </table>

                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="RawUrl" SortExpression="RawUrl">
                        <HeaderStyle Wrap="False" />
                        <ItemTemplate>

                            <table style="width: 100%;" cellpadding="0" cellspacing="0" border="0" rules="none">
                                <tr>
                                    <td style="horiz-align: left; text-align: left;">
                                        <asp:HyperLink runat="server" Text='<%# Eval("RawUrl") %>' NavigateUrl='<%# "~" + Eval("RawUrl") %>' />
                                    </td>
                                    <td style="horiz-align: right; text-align: right;">
                                        &nbsp;<asp:ImageButton runat="server" ID="bFilterByRawUrl" ImageUrl="~/Images/anchor.png" CommandArgument='<%# Eval("RawUrl") %>'
                                            OnClick="bFilterBy_SearchString_OnClick" />
                                    </td>
                                </tr>
                            </table>

                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="CreatedDate" SortExpression="CreatedDate">
                        <HeaderStyle Wrap="False" />
                        <ItemTemplate>

                            <table style="width: 100%;" cellpadding="0" cellspacing="0" border="0" rules="none">
                                <tr>
                                    <td style="horiz-align: left; text-align: left;">
                                        <asp:Label runat="server" Text='<%# Parser.DateTimeForUI(Eval("CreatedDate"), omitDateIfToday: true) %>' />
                                    </td>
                                    <td style="horiz-align: right; text-align: right; white-space: nowrap;">
                                        &nbsp;<asp:ImageButton runat="server" ImageUrl="~/Images/arrow-000-small.png" CommandArgument='<%# ((DateTime) Eval("CreatedDate")).ToString("dd.MM.yyyy HH:mm:ss") %>'
                                            OnClick="bFilterByDateBegin_OnClick" ToolTip="Отфильтровать по дате, взяв эту как начало интервала" /><asp:ImageButton
                                                runat="server" ImageUrl="~/Images/arrow-180-small.png" CommandArgument='<%# ((DateTime) Eval("CreatedDate")).ToString("dd.MM.yyyy HH:mm:ss") %>'
                                                OnClick="bFilterByDateEnd_OnClick" ToolTip="Отфильтровать по дате, взяв эту как конец интервала" />
                                    </td>
                                </tr>
                            </table>

                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Прошло">
                        <HeaderStyle Wrap="False" />
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Parser.TimeSpanForUI(((TimeSpan) Eval("SecondsAgo"))) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="IsPostBack">
                        <HeaderStyle Wrap="False" />
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# (bool)Eval("IsPostBack")?"+":"f5" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Браузер" SortExpression="Browser">
                        <HeaderStyle Wrap="False" />
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("Browser") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="IP" SortExpression="UserHostAddress">
                        <HeaderStyle Wrap="False" />
                        <ItemTemplate>

                            <table style="width: 100%;" cellpadding="0" cellspacing="0" border="0" rules="none">
                                <tr>
                                    <td style="horiz-align: left; text-align: left;">
                                        <asp:Label runat="server" Text='<%# Eval("UserHostAddress") %>' />
                                    </td>
                                    <td style="horiz-align: right; text-align: right;">
                                        &nbsp;<asp:ImageButton runat="server" ID="bFilterByIP" ImageUrl="~/Images/anchor.png" CommandArgument='<%# Eval("UserHostAddress") %>'
                                            OnClick="bFilterBy_SearchString_OnClick" />
                                    </td>
                                </tr>
                            </table>

                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:ObjectDataSource runat="server" ID="odsAct" TypeName="My.Example.Web.Activity" SelectMethod="SelectAct" OnObjectCreating="ods_ObjectCreating"
        OnObjectDisposing="ods_ObjectDisposing" EnablePaging="True" MaximumRowsParameterName="count" SelectCountMethod="SelectActCount"
        SortParameterName="orderBy" StartRowIndexParameterName="startIndex" />

</asp:Content>

