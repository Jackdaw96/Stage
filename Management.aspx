<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Management.aspx.vb" Inherits="Management" EnableEventValidation="false"%>
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>Gestione</title>
        <meta name="viewport" content="width=device-width,initial-scale=1" />
        <script type="text/javascript" src="Scripts/jquery-2.1.4.min.js"></script>
        <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>
<%--        <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>--%>
        <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.3.7/js/bootstrap.min.js"></script>
        <link href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.3.7/css/bootstrap.min.css" rel="stylesheet" />
        <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap-theme.min.css" />

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
           <nav class="navbar navbar-default">
        <div class="container-fluid">
        <div class="navbar-header">
            <a class="navbar-brand" href="#">Stage</a>
        </div>
        <div>
            <ul class="nav navbar-nav navbar-right">
                <li>
                    <a href="default.aspx" class="glyphicon glyphicon-home"></a>
                </li>
            </ul>
        </div>
        </div>
    </nav>

        <form id="form1" runat="server" style="position:absolute; left: 25px; right:25px; top: 5px">

                <center>
                    <div runat="server" style="text-align:center;padding-bottom:10px">
                        <asp:Label ID="title" runat="server" Text="" Font-Size="XX-Large" Font-Bold="True" style="position:center"></asp:Label>
                    </div>
                    <table width="100%;">
                        <tr>
                            <td>
                                <div class="immagine">
                                    <img src="Images/logo_san_marco.png" alt="logo" width="150" height="150" style="text-align:center;" />
                                </div>
                            </td>
                            <td>
                                <center>
                                    <div id="menu">
                                        <asp:Button ID="btnRegistrations" runat="server" class="btn btn-primary" Font-Size="Large" onclick="btnRegistrations_Click" text="Registrazioni" width="180px" />
                                        <asp:Button ID="btnCalendar" runat="server" class="btn btn-primary" Font-Size="Large" onclick="btnCalendar_Click" text="Calendario" width="180px" />
                                        <asp:Button ID="btnCourses" runat="server" class="btn btn-primary" Font-Size="Large" onclick="btnCourses_Click" width="180px" text="Corsi" />
                                        <asp:Button ID="btnLocations" runat="server" class="btn btn-primary" Font-Size="Large" onclick="btnLocations_Click" width="180px" text="Siti" />
                                        <asp:Button ID="btnCosts" runat="server" class="btn btn-primary" Font-Size="Large" onclick="btnCosts_Click" width="180px" text="Costi" />
                                        <asp:Button ID="btnSummaryCourses" runat="server" class="btn btn-primary" Font-Size="Large" onclick="btnSummaryCourses_Click" width="180px" text="Riepilogo Corsi" />
                                    </div>
                                    <div style="padding-top:10px;">
                                        <asp:LinkButton ID="lnkCreateNew" runat="server" onclick="lnkCreateNew_Click" class="btn btn-success" Font-Size="Large" width="180px">Nuovo</asp:LinkButton>
                                        <asp:LinkButton ID="lnkExcelExport" runat="server" onclick="lnkExcelExport_Click" class="btn btn-success" Font-Size="Large" width="180px">Esporta in Excel</asp:LinkButton>
                                    </div>
                                </center>
                            </td>
                            <td>
                                <div class="immagine" style="float:right;position:relative">
                                    &nbsp;<img src="Images/logo_lab.jpg" alt="logo2" width="150" height="150" style="text-align:center;float:left" /></div>
                            </td>
                        </tr>
                    </table>
                </center>
                <center>
                    <div class="container">
                        <asp:Panel ID="pnlFields" runat="server" style="position:relative;padding-bottom:10px"></asp:Panel>
                        <asp:Button ID="btnSave" runat="server" class="btn btn-lg btn-primary btn-block" Font-Size="Large" onclick="btnSave_Click" visible="False" validationGroup="group1" text="Salva" />
                        <asp:Panel ID="pnlFilters" runat="server" Visible="false" style="position:relative;padding-bottom:10px">
                            <asp:Label id="label50" runat="server" text="Stagione:" />
                            <asp:DropDownList ID="DropDownList1" class="form-control" runat="server" />
                            <asp:Label id="label51" runat="server" text="Corso:" />
                            <asp:DropDownList ID="DropDownList2" class="form-control" runat="server" />
                            <asp:Label id="label57" runat="server" text="Data:" />
                            <asp:textbox ID="text8" class="form-control" runat="server" />
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
                               <%-- <asp:Button ID="btnReset" runat="server" class="btn btn-lg btn-primary btn-block" Font-Size="Large" onClick="btnReset_Click" Text="Reset" />--%>
                            </div>
                        </asp:Panel>
                    </div>
                </center>
                <asp:Panel ID="pnlGrid" runat="server" style="padding-top:10px">
                    <asp:GridView ID="grdItems" style="position:relative;" runat="server" AllowPaging="True" AutoGenerateSelectButton="true" CssClass="table table-striped table-bordered table-hover" OnPageIndexChanging="grdItems_PageIndexChanging" PageSize="20">

                     <pagersettings position="TopAndBottom" />

                     </asp:GridView>
                </asp:Panel>
          <div class="modal fade" id="myModal" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
               <asp:panel ID="upModal" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                  <div class="modal-content">
                     <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title"><asp:Label ID="modaltitle" runat="server"></asp:Label></h4>
                     </div>
                     <div class="modal-body">
                        <asp:Label runat="server" ID="modaltext" Text=""></asp:Label>
                     </div>
                     <div class="modal-footer">
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">Close</button>
                     </div>
                  </div>
               </asp:panel>
            </div>
         </div>
        </form>
    </body>

    </html>