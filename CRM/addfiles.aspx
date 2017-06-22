<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="addfiles.aspx.cs" Inherits="CRM.addfiles" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 186px;
        }

        .auto-style2 {
            height: 23px;
            width: 1295px;
            margin-right: 47px;
        }

        .display-next {
            clear: both;
            display: block;
            float: left;
        }

        .auto-style6 {
            width: 186px;
            height: 23px;
        }

        .auto-style7 {
            width: 319px;
            height: 23px;
        }

        TEXTAREA, INPUT[type="text"] {
            /* font size, line height, face */
            font: 14px/1.5 "Trebuchet MS", Arial, Verdana, sans-serif;
            /* useful for supporting 100% width inclusive of padding and border */
            box-sizing: border-box;
        }
        
    </style>

</head>
<body>
    <form id="form1" runat="server">

        <div>
            <div>
                <fieldset style="height: auto; width: 587px; margin: 0 auto;">

                    <legend>Contract & Record Management</legend>
                    <asp:ScriptManager ID="ScriptManager1" runat="server" />

                    <table>
                        <tr>
                            <td></td>
                            <td class="auto-style1">
                                <asp:TextBox ID="tb_contract_id" runat="server" Width="300px" Height="30px" Visible="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style7">Contract No :</td>
                            <td class="auto-style1">
                                <asp:TextBox ID="tb_contract_no" runat="server" Width="300px" Height="30px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Contract no is empty!" ControlToValidate="tb_contract_no" ForeColor="Red" ValidationGroup="toValidate" Display="Dynamic"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style7">Branch :</td>
                            <td class="auto-style1">
                                <asp:DropDownList ID="ddlBranch" runat="server" Height="30px" Width="300px" AutoPostBack="True"> </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style7">Contract Date :</td>
                            <td class="auto-style1">

                                <asp:TextBox ID="tb_contract_date" runat="server" Width="300px" Height="30px"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="tb_date_CalendarExtender" Format="dd/MM/yyyy" runat="server" TargetControlID="tb_contract_date" />
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please select date!" ControlToValidate="tb_contract_date" ForeColor="Red" ValidationGroup="toValidate" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="tb_contract_date" ErrorMessage="Date is invalide!" ForeColor="Red" ValidationExpression="^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$" ValidationGroup="toValidate" Display="Dynamic"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style7">Objective :</td>
                            <td class="auto-style1">
                                <asp:TextBox ID="tb_objective" runat="server" Width="300px" Height="30px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Objective is empty!" ControlToValidate="tb_objective" ForeColor="Red" ValidationGroup="toValidate" Display="Dynamic"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style7">Type :</td>
                            <td class="auto-style1">
                                <asp:DropDownList ID="ddlType" runat="server" Height="30px" Width="300px">
                                    <asp:ListItem Selected="True" Value="0">Renewable</asp:ListItem>
                                    <asp:ListItem Value="1">Non-Renewable</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style7">Effective Date : </td>
                            <td class="auto-style1">
                                <asp:TextBox ID="tb_effective" runat="server" Width="300px" Height="30px"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="TextBox1_CalendarExtender" runat="server" Format="dd/MM/yyyy" TargetControlID="tb_effective" />
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please select date!" ControlToValidate="tb_effective" ForeColor="Red" ValidationGroup="toValidate" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="tb_effective" ErrorMessage="Date is invalide!" ForeColor="Red" ValidationExpression="^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$" ValidationGroup="toValidate" Display="Dynamic"></asp:RegularExpressionValidator>
                                <asp:CompareValidator ID="CompareValidator2" runat="server" Type="Date" ControlToCompare="tb_contract_date" ControlToValidate="tb_effective" ErrorMessage="Effective date must be higher than contract date!" ForeColor="Red" Operator="GreaterThanEqual" Display="Dynamic"  ValidationGroup="toValidate"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style7">Expire Date : </td>
                            <td class="auto-style6">
                                <asp:TextBox ID="tb_expire" runat="server" Width="300px" Height="30px"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="TextBox4_CalendarExtender" runat="server" Format="dd/MM/yyyy" TargetControlID="tb_expire" />
                            </td>
                            <td class="auto-style2">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Please select date!" ControlToValidate="tb_expire" ForeColor="Red" ValidationGroup="toValidate" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="tb_expire" ErrorMessage="Date is invalide!" ForeColor="Red" ValidationExpression="^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$" ValidationGroup="toValidate" Display="Dynamic"></asp:RegularExpressionValidator>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" Type="Date" ControlToCompare="tb_effective" ControlToValidate="tb_expire" Display="Dynamic" ErrorMessage="Expire date must be higher than effective date!" ForeColor="Red" Operator="GreaterThan"  ValidationGroup="toValidate"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style7">Remind on (Days) : </td>
                            <td class="auto-style1">
                                <asp:TextBox ID="tb_remindon" runat="server" Width="300px" Height="30px"></asp:TextBox></td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="tb_remindon" ErrorMessage="Remind days  is empty!" ForeColor="Red" ValidationGroup="toValidate" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="tb_remindon" ErrorMessage="Number of day is invalid!" ForeColor="Red" ValidationExpression="^[0-9]{1,2}$" ValidationGroup="toValidate" Display="Dynamic"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style7">Responder : </td>
                            <td class="auto-style1">
                                <asp:TextBox ID="tb_responder" runat="server" Width="300px" Height="30px"></asp:TextBox></td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Responder  is empty!" ControlToValidate="tb_responder" ForeColor="Red" ValidationGroup="toValidate" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="tb_responder" Display="Dynamic" ErrorMessage="Invalid email!" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@cambodiapostbank\.com$" ValidationGroup="toValidate"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style7">Recipient : </td>
                            <td class="auto-style1">
                                <asp:TextBox ID="tb_recipient" runat="server" Width="300px" Height="60px" TextMode="multiline" Rows="10"></asp:TextBox></td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Recipient  is empty!" ControlToValidate="tb_recipient" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style7">File : </td>
                            <td class="auto-style1">
                                <asp:FileUpload ID="tb_file" runat="server" Width="300px" Height="30px" />
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFileUpload" runat="server" EnableClientScript="false" ErrorMessage="Please select file!" ControlToValidate="tb_file" ForeColor="Red" ValidationGroup="toValidate" Display="Dynamic"></asp:RequiredFieldValidator>
                            </td>
                        </tr>

                        <tr>
                            <td class="auto-style7">&nbsp;</td>
                            <td class="auto-style1" style="padding-left:5px;">

                                <asp:Button ID="btnAdd" runat="server" Text="Add" Width="95px" Height="30px" OnClick="btnAdd_Click" ValidationGroup="toValidate" />
                                <asp:Button ID="btnClear" runat="server" Text="Cancel" Width="95px" Height="30px" OnClick="btnClear_Click" CausesValidation="False" />
                                <asp:Button ID="btnGotoList" runat="server" Text="Goto List" Width="95px" Height="30px" CausesValidation="False" OnClick="btnGotoList_Click" />
                            </td>
                            <td></td>
                        </tr>
                    </table>

                </fieldset>
            </div>
            <br />



        </div>
    </form>
</body>
</html>
