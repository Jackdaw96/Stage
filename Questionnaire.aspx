<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Questionnaire.aspx.vb" Inherits="Questionnaire" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
   <head runat="server">
      <title>Questionario</title>
      <meta name="viewport" content="width=device-width,initial-scale=1" />
      <script type="text/javascript" src="Scripts/jquery-2.1.4.min.js"></script>
      <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>  
      <link href="content/bootstrap.min.css" rel="stylesheet" />
      <style type="text/css">            
         body
         {
         font-family: Arial;
         font-size: 18px;
         }  
      </style>
   </head>
   <body>
      <form id="form1" runat="server" style="position:absolute; top:10%; right: 0; left: 0;">
         <div class="container">
            <div class="col-lg-3"></div>
            <div class="col-lg-6">
               <div class="panel panel-primary">
                  <asp:Panel ID="pnlQuestions" runat="server" class="panel-heading">
                     <asp:Label ID="lblQuestion" runat="server"/>
                  </asp:Panel>
                  <asp:Panel ID="pnlAnswers" runat="server" class="panel-body"></asp:Panel>
                  <asp:Panel id="pnlFooter" runat="server" Class="panel-footer btn-group btn-group-justified">
                     <nav aria-label="...">
                        <ul class="pager">
                           <li>
                              <asp:Button ID="btnBack" runat="server" class="btn btn-primary" Font-Size="Large" onClick="btnBack_Click" Width="100px"/>
                           </li>
                           <li>
                              <asp:Button ID="btnNext" runat="server" class="btn btn-primary" Font-Size="Large" onClick="btnNext_Click" Width="100px"/>
                           </li>
                        </ul>
                     </nav>
                  </asp:Panel>
               </div>
            </div>
            <div class="col-lg-3"></div>
         </div>
         <div class="modal fade" id="myModal" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog" style="width:800px">
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