<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <meta name="viewport" content="width=device-width,height=device-height,initial-scale=1.0" />
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
    <nav class="navbar navbar-default">
        <div class="container-fluid">
        <div class="navbar-header">
            <a class="navbar-brand" href="#">Stage</a>
        </div>
        <div>
            <ul class="nav navbar-nav navbar-right">
                <li class="dropdown">
                    <a class="dropdown-toggle" href="#" data-toggle="dropdown"><span class="glyphicon glyphicon-cog"></span> Management <strong class="caret"></strong></a>
                    <div class="dropdown-menu" style="padding: 15px; padding-bottom: 15px;">
                        <asp:TextBox runat="server" type="text" id="txtUsername" class="form-control" placeholder="Username" style="margin-bottom: 15px;" autofocus="true" />
                        <asp:TextBox runat="server" type="password" id="txtPassword" class="form-control" placeholder="Password" style="margin-bottom: 15px;" autofocus="true" />
                        <asp:Button runat="server" id="btnManagement" onClick="btnManagement_Click" Font-Size="Large" text="Login" class="btn btn-lg btn-primary btn-block"/>
                    </div>
                </li>
            </ul>
        </div>
        </div>
    </nav>
        <center>
    <div class="container" style="position:absolute; top:30%; right: 0; left: 0;">  
        <div class="row">  
            <div class="col-lg-3"></div>  
            <div class="col-lg-6" style="border: thin solid #337ab7">                         
                <h2 class="form-signin-heading">
                    <img alt="LogoSAN" class="auto-style1" src="Images/logo_san_marco.png" style="width:150px; height:150px; float:left"/>
                    <img alt="LogoLAB" class="auto-style1" src="Images/logo_lab.jpg" style="width:150px; height:150px;float:right"/>  
                </h2>  
                <div class="col-lg-3" style="height: 50px;"></div>                         
                <div class="form-group">                         
                    <asp:TextBox  type="accesscode" id="txtCode" runat="server" class="form-control" placeholder="Login code" autofocus="true" /> 
                </div>                                                        
                <asp:Button runat="server" id="btnLogin" onClick="btnLogin_Click" Text="Login" class="btn btn-lg btn-primary btn-block" useSubmitBehavior="false" data-dismiss="modal"/>
             

<div class="modal fade" id="myModal" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="width:100%">
    <div class="modal-dialog" style="width:800px">
        <asp:panel ID="upModal" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title"><asp:Label ID="modaltitle" runat="server" Text=""></asp:Label></h4>
                    </div>
                    <div class="modal-body">
                        <asp:Label ID="modaltext" runat="server" Text="Informativa sulla privacy"></asp:Label>
                    </div>
                    <div class="modal-footer">
                        <button class="btn btn-primary" data-dismiss="modal" aria-hidden="true">Close</button>
                        <asp:Button runat="server" aria-hidden="true" id="Button1" onClick="btnConfirm_Click" Text="Accetto" class="btn btn-success" useSubmitBehavior="false" data-dismiss="modal" />
                    </div>
                </div>
        </asp:panel>
    </div>
</div>
                <div class="col-lg-3" style="height: 20px"></div>
            </div>  
            <div class="col-lg-3"></div>  
        </div> 
    </div>
            </center>
    </form>
</body>
</html>
