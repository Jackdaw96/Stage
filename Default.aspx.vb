Imports System.Data.SqlClient
Imports System.Data
Imports System.DirectoryServices

Partial Class Login
    Inherits System.Web.UI.Page
    Dim privacy As Boolean = False
    Dim connString As String = "Data Source=CSMWAR\CSMWAR;Initial Catalog=SageInterface;User ID=STAGE; pwd=STAGE21"

    Protected Property participant() As Integer
        Get
            Return CInt(Session("participant"))
        End Get
        Set(value As Integer)
            Session("participant") = value
        End Set
    End Property

    Protected Property customerType() As String
        Get
            Return CStr(Session("customerType"))
        End Get
        Set(value As String)
            Session("customerType") = value
        End Set
    End Property

    Protected Property language() As Integer
        Get
            Return CInt(Session("language"))
        End Get
        Set(value As Integer)
            Session("language") = value
        End Set
    End Property

    Protected Property courseCode() As String
        Get
            Return CStr(Session("courseCode"))
        End Get
        Set(value As String)
            Session("courseCode") = value
        End Set
    End Property
    Protected Sub btnConfirm_Click(sender As Object, e As System.EventArgs)
        If updatePolicy() Then
            questionnaire = nQuestionnaire() + 1
            Select Case questionnaire
                Case 1
                    Response.Redirect("UpdateAnagraphic.aspx")
                Case 2
                    Response.Redirect("Questionnaire.aspx")
                Case Else
                    modaltitle.CssClass = "glyphicon glyphicon-warning-sign"
                    modaltext.Text = "Questionnaries already done!"
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", True)
            End Select
        End If
    End Sub

    Protected Property questionnaire() As Integer
        Get
            Return CInt(Session("questionnaire"))
        End Get
        Set(value As Integer)
            Session("questionnaire") = value
        End Set
    End Property

    Protected Property GROUPID() As String
        Get
            Return CStr(Session("GROUPID"))
        End Get
        Set(value As String)
            Session("GROUPID") = value
        End Set
    End Property

    Protected Sub btnLogin_Click(sender As Object, e As System.EventArgs)
        If validCode() Then
            questionnaire = nQuestionnaire() + 1
            Select Case questionnaire
                Case 1
                    Response.Redirect("UpdateAnagraphic.aspx")
                Case 2
                    Response.Redirect("Questionnaire.aspx")
                Case Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('Questionnaires already done');", True)
            End Select
        Else
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('Login code error');", True)
        End If
    End Sub

    Protected Sub btnManagement_Click(sender As Object, e As System.EventArgs)
        If txtUsername.Text.Contains("MARK") Or txtUsername.Text = "ACQUI_05" Or txtUsername.Text = "CED_02" Then
            Dim de As New DirectoryEntry("LDAP://domain.sanmarcogroup.it", txtUsername.Text, txtPassword.Text, AuthenticationTypes.Secure)
            Try
                Dim ds As DirectorySearcher = New DirectorySearcher(de)
                ds.FindOne()
                Response.Redirect("Management.aspx")
            Catch ex As Exception
                ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
            End Try
        ElseIf txtUsername.Text.Contains("FUNZ") Or txtUsername.Text.Contains("ESTER") Or txtUsername.Text = "TEC_01" Or txtUsername.Text = "RS_05" Or txtUsername.Text = "RS_09" Or
               txtUsername.Text = "RS_12" Or txtUsername.Text = "RS_13" Or txtUsername.Text = "RS_14" Then
            Dim de As New DirectoryEntry("LDAP://domain.sanmarcogroup.it", txtUsername.Text, txtPassword.Text, AuthenticationTypes.Secure)
            Try
                Dim ds As DirectorySearcher = New DirectorySearcher(de)
                ds.FindOne()
                Response.Redirect("Summary.aspx")
            Catch ex As Exception
                ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
            End Try
        Else
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('User not enable');", True)
        End If
    End Sub

    Protected Function validCode() As Boolean
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        validCode = False
        Try
            cmd.CommandText = "select ID,CUSTOMERTYPE,IDLANGUAGE,IDREGISTRATION,GROUPID from STAGE_PARTICIPANTS where LOGINCODE='" & txtCode.Text & "'"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            While reader.Read
                If reader(0) Then
                    participant = reader(0)
                    customerType = reader(1)
                    language = reader(2)
                    GROUPID = reader(4)
                    courseCode = codeCourse(reader(3))
                    validCode = True
                End If
            End While
            sqlConnection1.Close()
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
        End Try
        Return validCode
    End Function

    Protected Function nQuestionnaire() As Integer
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        nQuestionnaire = 0
        Try
            cmd.CommandText = "select top (1) NQUESTIONNAIRE from STAGE_QUESTIONNAIRESPARTICIPANTS where IDPARTICIPANTS='" & participant & "' and GROUPID='" & GROUPID & "' order by NQUESTIONNAIRE desc"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            While reader.Read
                nQuestionnaire = reader(0)
            End While
            sqlConnection1.Close()
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
        End Try
        Return nQuestionnaire
    End Function

    Protected Function codeCourse(ByVal idRegistration As Integer) As String
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        codeCourse = ""
        Try
            cmd.CommandText = "select CODE from STAGE_COURSES inner join STAGE_CALENDAR on STAGE_COURSES.ID = STAGE_CALENDAR.IDCOURSE and STAGE_COURSES.GROUPID = STAGE_CALENDAR.GROUPID inner join STAGE_REGISTRATIONS on " &
                              "STAGE_CALENDAR.ID = STAGE_REGISTRATIONS.IDCALENDAR and STAGE_CALENDAR.GROUPID = STAGE_REGISTRATIONS.GROUPID where STAGE_REGISTRATIONS.ID = '" & idRegistration & "' and " &
                              "STAGE_REGISTRATIONS.GROUPID = '" & GROUPID & "'"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            While reader.Read
                codeCourse = reader(0)
            End While
            sqlConnection1.Close()
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
        End Try
        Return codeCourse
    End Function

    Protected Function privacyAgreement() As Boolean
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        Dim agree As Boolean = False
        Try
            cmd.CommandText = "select IDPARTICIPANTS from STAGE_PARTICIPANTS JOIN STAGE_PRIVACY ON STAGE_PRIVACY.IDPARTICIPANTS = STAGE_PARTICIPANTS.ID where IDPARTICIPANTS = " & participant
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            If reader.HasRows Then
                agree = True
            End If
            sqlConnection1.Close()
        Catch ex As Exception
            modaltitle.CssClass = "glyphicon glyphicon-exclamation-sign"
            modaltext.Text = ex.Message
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
        End Try
        Return agree
    End Function

    Protected Function strPrivacy() As String
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        Dim str As String = ""
        Try
            cmd.CommandText = "select descrizione from STAGE_PRIVACYTRANSLATIONS JOIN STAGE_PARTICIPANTS ON STAGE_PARTICIPANTS.IDLANGUAGE = STAGE_PRIVACYTRANSLATIONS.ID " &
                              "WHERE STAGE_PARTICIPANTS.ID='" & participant & "' and GROUPID='" & GROUPID & "'"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            While reader.Read
                str = reader(0)
            End While
            sqlConnection1.Close()
        Catch ex As Exception
            modaltitle.CssClass = "glyphicon glyphicon-exclamation-sign"
            modaltext.Text = ex.Message
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
        End Try
        Return str

    End Function

    Protected Function updatePolicy() As Boolean
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim update As Boolean = False
        Try
            cmd.CommandText = "INSERT INTO STAGE_PRIVACY(idparticipants, datetimeagree) VALUES (" & participant & ", convert(datetime, '" & System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "', 120))"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            cmd.ExecuteNonQuery()
            update = True
            sqlConnection1.Close()
        Catch ex As Exception
            modaltitle.CssClass = "glyphicon glyphicon-exclamation-sign"
            modaltext.Text = ex.Message
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
        End Try
        Return update
    End Function

End Class
