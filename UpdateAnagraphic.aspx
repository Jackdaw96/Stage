<%@ Page Language="VB" AutoEventWireup="false" CodeFile="UpdateAnagraphic.aspx.vb" Inherits="UpdateAnagraphic" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Aggiornamento anagrafica</title>
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
    <form id="form1" runat="server">
    <div class="container" style="position:absolute; top:10%; right: 0; left: 0;">
        <div class="row">    
            <div class="col-lg-3"></div>          
            <div class="col-lg-3">        
                <asp:Panel ID="pnlFields1" runat="server">
                </asp:Panel> 
                
            
            </div>                                             
            <div class="col-lg-3">        
                <asp:Panel ID="pnlFields2" runat="server"></asp:Panel>                                
            </div> 
            <div class="col-lg-3"></div>                         
        </div> 
        <div class="row">    
            <div class="col-lg-3"></div>  
                                         
                                                        
            <div class="col-lg-6">  
                               
     <asp:Button ID="btnConfirm" runat="server" class="btn btn-primary" Font-Size="Large" onClick="btnConfirm_Click" Width="100%"/>              
            </div> 
            <div class="col-lg-3"></div>                         
        </div>                    
    </div>    
    </form>
</body>
</html>
