<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="contract_list.aspx.cs" Inherits="CRM.contract_list" EnableEventValidation="false" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 186px;
            margin-right:20px;
        }

        .auto-style2 {
            height: 23px;
            width: auto;
            margin-right: 47px;
        }

        .display-next {
            clear: both;
            display: block;
            float: left;
        }

        .auto-style7 {
            width: 319px;
            height: 23px;
            text-align:right;
        }
        TEXTAREA, INPUT[type="text"] {
            /* font size, line height, face */
            font: 14px/1.5 "Trebuchet MS", Arial, Verdana, sans-serif;
            /* useful for supporting 100% width inclusive of padding and border */
            box-sizing: border-box;
        }
        .span1 { background-color: #DDD; Width:100px; float: left; height:30px; margin-right:3px; }

        .auto-style8 {
            width: 186px;
            margin-right: 20px;
            height: 23px;
        }

    </style>

</head>
<body>
    <form id="form1" runat="server">

        <div runat ="server">
            <div>
                <fieldset style="height: auto; width: 900px; margin: 0 auto;">
                    <legend>Contract & Record List</legend>
                <table class="auto-style2">
                    <tr>
                        <td class="auto-style7">Contract No :</td>
                        <td class="auto-style8">
                            <asp:TextBox ID="tb_contract_no" runat="server" Width="300px" Height="30px"></asp:TextBox>
                        </td>
                        <td class="auto-style7" >Objective :</td>
                        <td class="auto-style8">
                            <asp:TextBox ID="tb_Objective" runat="server" Width="300px" Height="30px"></asp:TextBox>
                        </td>

                    </tr>
                    <tr>
                        <td class="auto-style7">Type :</td>
                        <td class="auto-style1">
                            <asp:DropDownList ID="ddlType" runat="server" Height="30px" Width="300px">
                                <asp:ListItem Selected="True" Value="-1">-- All --</asp:ListItem>
                                <asp:ListItem Value="0">Renewable</asp:ListItem>
                                <asp:ListItem Value="1">Non-Renewable</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="auto-style7">Branch :</td>
                        <td class="auto-style1">
                            <asp:DropDownList ID="ddlBranch" runat="server" Height="30px" Width="300px"> </asp:DropDownList>
                        </td>

                    </tr>
                    <tr>                        
                        <td colspan="4">
                            <div style="height: 30px; width: 315px; margin: 0 auto;">
                                <div class="span1"><asp:Button ID="btnSearch" runat="server" Text="Search" Width="100%" Height="100%" OnClick="btnSearch_Click" /></div>
                                <div class="span1"><asp:Button ID="btnAddNew" runat="server" Text="Add New" Width="100%" Height="100%" OnClick="btnAddNew_Click" /></div>
                                <div class="span1"><asp:Button ID="btnExport" runat="server" Text="Export" Width="100%" Height="100%" OnClick="btnExport_Click" /></div>
                            </div>        
                        </td>
                    </tr>
                </table>
                </fieldset>
            </div>
            <br />

            <asp:GridView ID="gvFile" runat="server" AutoGenerateColumns="false" ShowFooter="false" Width="100%" HeaderStyle-Height ="30px"
                DataKeyNames="Contract_No" AllowPaging="true" PageSize="10" ShowHeaderWhenEmpty="true" OnPageIndexChanging="gvFile_PageIndexChanging">

                <Columns>

                    <asp:TemplateField HeaderText="No">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Contract No">
                        <ItemTemplate>
                            <asp:Label ID="lblContract_No" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Contract_No") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Contract Date">
                        <ItemTemplate>
                            <asp:Label ID="lblContract_Date" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Contract_Date", "{0:dd-MM-yyyy}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Objective">
                        <ItemTemplate>
                            <asp:Label ID="lblObjective" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Objective") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Effective Date">
                        <ItemTemplate>
                            <asp:Label ID="lblEffective_Date" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Effective_Date", "{0:dd-MM-yyyy}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Expire Date">
                        <ItemTemplate>
                            <asp:Label ID="lblExpire_Date" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Expire_Date", "{0:dd-MM-yyyy}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="File Name">
                        <ItemTemplate>
                            <asp:Label ID="lblFile_Name" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "File_Name") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>

                            <asp:LinkButton ID="lnkDownload" runat="server"
                                CommandArgument='<%# Eval("Contract_Id")%>'
                                Text="Download" OnClick="gvFile_Download"></asp:LinkButton>
                            <asp:LinkButton ID="lnkView" runat="server"
                                CommandArgument='<%# Eval("Contract_Id")%>'
                                Text="View" PostBackUrl='<%# "addfiles.aspx?Contract_Id=" + DataBinder.Eval(Container.DataItem, "Contract_Id")%>'></asp:LinkButton>
                            <asp:LinkButton ID="lnkDelete" runat="server"
                                CommandArgument='<%# Eval("Contract_Id")%>'
                                OnClientClick="return confirm('Do you want to delete this purpose?')"
                                Text="Delete" OnClick="gvFile_Delete"></asp:LinkButton>

                        </ItemTemplate>

                    </asp:TemplateField>


                </Columns>

                <HeaderStyle BackColor="#FF6600" ForeColor="White" />

            </asp:GridView>

        </div>
    </form>
</body>
</html>
