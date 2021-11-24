<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Summary.aspx.vb" Inherits="Default2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Riepilogo corsi LAB FOR PRO</title>
    <meta name="viewport" content="width=device-width,initial-scale=1" />
    <script type="text/javascript" src="Scripts/jquery-2.1.4.min.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>  
    <link href="content/bootstrap.min.css" rel="stylesheet" />  
    <style type="text/css">
            body {
                font-family: Arial;
                font-size: 18px;
                height: 100%;
                background-color: white;
                overflow-y: scroll
            }         
            .auto-style1 {
                width: 172px;
            }        
            #container {
                position: absolute;
                top: 50%;
                left: 50%;
                padding-bottom: 10px;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server" style="position:absolute; left: 25px; right:25px; top: 15px">
  <center>
                    <div runat="server" style="text-align:center;border:1px">
                        <asp:Label ID="title" runat="server" Text="RIEPILOGO CORSI LAB FOR PRO" Font-Size="XX-Large" Font-Bold="True" style="position:center"></asp:Label>
                    </div>
                    <table width="100%">
                        <tr>
                            <td>
                                <div class="immagine">
                                    <img src="http://www.san-marco.com/dist/images/logos/logo-sanmarco.png" alt="logo" width="200" height="100" style="text-align:center;" />
                                </div>
                            </td>
                            <td>
                                <center>
                                    <div style="padding-top:10px;">
                                        <asp:LinkButton ID="lnkExcelExport" runat="server" class="btn btn-success" Font-Size="Large" width="180px" OnClick="lnkExcelExport_Click">Esporta in Excel</asp:LinkButton>
                                    </div>
                                </center>
                            </td>
                            <td>
                                <div class="immagine" style="float:right;position:relative">
                                    &nbsp;<img src="https://s-media-cache-ak0.pinimg.com/736x/6d/26/0a/6d260acbcfab64a1c8eb790c06302af6.jpg" alt="logo2" width="200" height="100" style="text-align:center;float:left" /></div>
                            </td>
                        </tr>
                    </table>
                </center>
                <center>
                    <div class="container">
                        <asp:Panel ID="pnlFilters" runat="server" style="position:relative;padding-bottom:10px">
                            <asp:Label id="label50" runat="server" text="Stagione:" />
                            <asp:DropDownList ID="DropDownList1" class="form-control" runat="server" />
                            <asp:Label id="label51" runat="server" text="Corso:" />
                            <asp:DropDownList ID="DropDownList2" class="form-control" runat="server" />
                            <asp:Label id="label52" runat="server" text="Capo Area:" />
                            <asp:DropDownList ID="DropDownList3" class="form-control" runat="server" />
                            <asp:Label id="label53" runat="server" text="Agente:" />
                            <asp:DropDownList ID="DropDownList4" class="form-control" runat="server" />
                            <asp:Label id="label54" runat="server" text="Cod. Cliente:" />
                            <asp:DropDownList ID="DropDownList5" class="form-control" runat="server" />
                            <asp:Label id="label55" runat="server" text="Stato:" />
                            <asp:DropDownList ID="DropDownList6" class="form-control" runat="server" />
                            <asp:Label id="label56" runat="server" text="Provincia:" />
                            <asp:DropDownList ID="DropDownList7" class="form-control" runat="server" />
                            <div style="position:relative;padding-top:20px;padding-bottom:10px">
                                <asp:Button ID="btnFilter" runat="server" class="btn btn-lg btn-primary btn-block" Font-Size="Large" onClick="btnFilter_Click" Text="Applica" />
                                <asp:Button ID="btnReset" runat="server" class="btn btn-lg btn-primary btn-block" Font-Size="Large" Text="Reset" />
                            </div>
                        </asp:Panel>
                    </div>
                </center>
                <asp:Panel ID="pnlGrid" runat="server" style="padding-top:10px">
                    <asp:GridView ID="grdItems" style="position:relative;" runat="server" AllowPaging="True" autoGenerateDeleteButton="True" autoGenerateSelectButton="True" CssClass="table table-striped table-bordered table-hover" OnPageIndexChanging="grdItems_PageIndexChanging" PageSize="20">
                        <pagersettings position="TopAndBottom" />
                    </asp:GridView>
                </asp:Panel>
    </form>
</body>
</html>
