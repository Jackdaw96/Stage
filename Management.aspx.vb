Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Partial Class Management
    Inherits System.Web.UI.Page
    Dim connString As String = "Data Source=CSMWAR\CSMWAR;Initial Catalog=SageInterface;User ID=STAGE; pwd=STAGE21"

    Protected Property table() As String
        Get
            Return CStr(Session("table"))
        End Get
        Set(value As String)
            Session("table") = value
        End Set
    End Property

    Protected Property action() As String
        Get
            Return CStr(Session("action"))
        End Get
        Set(value As String)
            Session("action") = value
        End Set
    End Property

    Protected Property loadable() As Boolean
        Get
            Return CBool(Session("loadable"))
        End Get
        Set(value As Boolean)
            Session("loadable") = value
        End Set
    End Property

    Protected Property idRegistration() As Integer
        Get
            Return CInt(Session("idRegistration"))
        End Get
        Set(value As Integer)
            Session("idRegistration") = value
        End Set
    End Property

    Protected Property GROUPID() As String
        Get
            Return CStr(Session("groupid"))
        End Get
        Set(value As String)
            Session("groupid") = value
        End Set
    End Property

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim id As Integer = -1
        If Not IsPostBack Then
            table = "STAGE_REGISTRATIONS"
            action = "insert"
            loadable = False
            loadItems("")
        ElseIf loadable Then
            If (action.Length > 6) Then
                id = CInt(action.Remove(0, 6))
            End If
            loadFields()
            loadItem(id)
            loadable = False
        End If
    End Sub

    Protected Sub btnRegistrations_Click(sender As Object, e As System.EventArgs)
        table = "STAGE_REGISTRATIONS"
        loadable = False
        action = "insert"
        btnSave.Visible = False
        pnlFields.Controls.Clear()
        loadItems("")
    End Sub

    Protected Sub btnCalendar_Click(sender As Object, e As System.EventArgs)
        table = "STAGE_CALENDAR"
        loadable = False
        action = "insert"
        btnSave.Visible = False
        pnlFields.Controls.Clear()
        loadItems("")
    End Sub

    Protected Sub btnCourses_Click(sender As Object, e As System.EventArgs)
        table = "STAGE_COURSES"
        loadable = False
        action = "insert"
        btnSave.Visible = False
        pnlFields.Controls.Clear()
        loadItems("")
    End Sub

    Protected Sub btnLocations_Click(sender As Object, e As System.EventArgs)
        table = "STAGE_LOCATIONS"
        loadable = False
        action = "insert"
        btnSave.Visible = False
        pnlFields.Controls.Clear()
        loadItems("")
    End Sub

    Protected Sub btnCosts_Click(sender As Object, e As System.EventArgs)
        table = "STAGE_COSTS"
        loadable = False
        action = "insert"
        btnSave.Visible = False
        pnlFields.Controls.Clear()
        loadItems("")
    End Sub

    Protected Sub btnSummaryCourses_Click(sender As Object, e As System.EventArgs)
        table = "STAGE_COURSESSUMMARY"
        loadable = False
        btnSave.Visible = False
        pnlFields.Controls.Clear()
        loadItems("")
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs)
        If action = "insert" Then
            insert()
        ElseIf action.Contains("update") Then
            update()
        End If
    End Sub

    Protected Sub lnkExcelExport_Click(sender As Object, e As EventArgs)
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"
        Using sw As New StringWriter()
            Dim hw As New HtmlTextWriter(sw)
            grdItems.AllowPaging = False
            loadItems("")
            grdItems.RenderControl(hw)
            Dim style As String = "<style> .textmode { } </style>"
            Response.Write(style)
            Response.Output.Write(sw.ToString())
            Response.Flush()
            Response.[End]()
        End Using
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
    End Sub

    Protected Sub lnkCreateNew_Click(sender As Object, e As System.EventArgs)
        loadable = True
        btnSave.Visible = True
        action = "insert"
        pnlFields.Controls.Clear()
        loadFields()
    End Sub

    Protected Sub btnConfirmCAP_Click(sender As Object, e As System.EventArgs)
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMSQL;Initial Catalog=SANMARCO_AX50_PROD;User ID=sa; pwd=SanMarc055$")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim sqlConnection2 As New SqlConnection("Data Source=CSMSAGE\SAGEX3P;Initial Catalog=x3;User ID=Stage; pwd=STAGE21")
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        Dim calendar As String = CType(pnlFields.FindControl("DropDownList8"), DropDownList).Text
        Dim calendaArray() As String = calendar.Split("-")
        Try
            'cmd.CommandText = "select STAGE_CUSTTABLE.NAME,STAGE_CUSTTABLE.COUNTY,STAGE_CUSTTABLE.COUNTRYREGIONID,STAGE_CUSTTABLE.PHONE,TILECOMAGENT,STAGE_VENDTABLE.NAME from STAGE_CUSTTABLE left outer join " &
            '                  "STAGE_VENDTABLE on STAGE_CUSTTABLE.DATAAREAID = STAGE_VENDTABLE.DATAAREAID and STAGE_CUSTTABLE.TILECOMAGENT = STAGE_VENDTABLE.ACCOUNTNUM where STAGE_CUSTTABLE.ACCOUNTNUM='" &
            '                  CType(pnlFields.FindControl("TextBox2"), TextBox).Text & "' and STAGE_CUSTTABLE.DATAAREAID='" & calendaArray(3) & "'"

            cmd.CommandText = "SELECT CUSTTABLE.BPCNAM_0, INDIRIZZI.SAT_0, INDIRIZZI.CRY_0, INDIRIZZI.TEL_0, VENDTABLE.AGENTE, VENDTABLE.RAGSOCAGE " &
                                "FROM [CSMSAGE\SAGEX3P].x3.PROD.BPCUSTOMER As CUSTTABLE " &
                                                "LEFT OUTER JOIN [CSMWAR\CSMWAR].SageInterface.dbo.V_AGENTICLIENTIPAESI AS VENDTABLE ON CUSTTABLE.REP_0 = VENDTABLE.AGENTE COLLATE Latin1_General_CI_AS " &
                                                               "AND CUSTTABLE.BPCNUM_0 = VENDTABLE.CUSTCODE COLLATE Latin1_General_CI_AS " &
                                                "LEFT JOIN [CSMSAGE\SAGEX3P].x3.PROD.BPADDRESS AS INDIRIZZI ON CUSTTABLE.BPCNUM_0 = INDIRIZZI.BPANUM_0 COLLATE Latin1_General_CI_AS " &
                               " WHERE  INDIRIZZI.BPAADD_0 = 'SL'" &
                                       " AND  CUSTTABLE.BPCNUM_0 ='" & CType(pnlFields.FindControl("TextBox2"), TextBox).Text & "'"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            sqlConnection2.Open()
            reader = cmd.ExecuteReader()
            While reader.Read
                For i As Integer = 0 To 5
                    CType(pnlFields.FindControl("TextBox" & i + 3), TextBox).Text = reader(i)
                Next
            End While
            sqlConnection1.Close()
            sqlConnection2.Close()
            CType(pnlFields.FindControl("TextBox7"), TextBox).Focus()
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
            If sqlConnection2.State = ConnectionState.Open Then
                sqlConnection2.Close()
            End If
            loadable = True
        End Try
    End Sub

    Protected Sub btnGenerate_Click(sender As Object, e As System.EventArgs)
        Dim loginCode As String = "A0000"
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        Dim courseCode As String = infoCourse(idRegistration)
        courseCode = courseCode.Substring(0, 2)
        Try
            cmd.CommandText = "select top(1) LOGINCODE from STAGE_PARTICIPANTS where LOGINCODE like '" & courseCode & "%' order by ID desc"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            While reader.Read
                loginCode = IIf(IsDBNull(reader(0)), loginCode, reader(0))
            End While
            sqlConnection1.Close()
            loginCode = courseCode & String.Format("{0:000}", CInt(loginCode.Substring(2, 3)) + 1)
            CType(pnlFields.FindControl("TextBox16"), TextBox).Text = loginCode
            CType(pnlFields.FindControl("TextBox16"), TextBox).Focus()
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
            loadable = True
        End Try
    End Sub

    Protected Sub grdItems_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        grdItems.PageIndex = e.NewPageIndex
        loadItems("")
    End Sub

    Protected Sub grdItems_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdItems.RowDeleting
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        Try
            If table = "STAGE_REGISTRATIONS" Then
                cmd.CommandText = "delete from " & table & " where ID='" & grdItems.Rows(e.RowIndex).Cells(2).Text & "'"
            Else
                cmd.CommandText = "delete from " & table & " where ID='" & grdItems.Rows(e.RowIndex).Cells(1).Text & "'"
            End If
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            grdItems.DataSource = reader
            grdItems.DataBind()
            sqlConnection1.Close()
            pnlFields.Controls.Clear()
            loadItems("")
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
        End Try
    End Sub

    Protected Sub grdItems_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdItems.SelectedIndexChanged
        pnlFields.Controls.Clear()
        loadFields()
        If table = "STAGE_REGISTRATIONS" Then
            loadable = True
            btnSave.Visible = True
            loadItem(grdItems.Rows(grdItems.SelectedIndex).Cells(2).Text)
            action = "update" & grdItems.Rows(grdItems.SelectedIndex).Cells(2).Text
        Else
            loadable = True
            btnSave.Visible = True
            loadItem(grdItems.Rows(grdItems.SelectedIndex).Cells(1).Text)
            action = "update" & grdItems.Rows(grdItems.SelectedIndex).Cells(1).Text
        End If
    End Sub

    Protected Sub lnk_Click(sender As Object, e As EventArgs)
        Dim lnkView As LinkButton = TryCast(sender, LinkButton)
        Dim row As GridViewRow = TryCast(lnkView.NamingContainer, GridViewRow)
        table = "STAGE_PARTICIPANTS"
        loadable = False
        action = "insert"
        idRegistration = row.Cells(2).Text
        GROUPID = row.Cells(14).Text
        btnSave.Visible = False
        pnlFields.Controls.Clear()
        loadItems("")
    End Sub

    Protected Sub OnRowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdItems.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow And table = "STAGE_REGISTRATIONS" Then
            Dim lnkView As New LinkButton()
            lnkView.ID = "lnkView"
            lnkView.Text = "Partecipanti"
            AddHandler lnkView.Click, AddressOf lnk_Click
            e.Row.Cells(1).Controls.Add(lnkView)
        End If
    End Sub

    Protected Sub btnFilter_Click(sender As Object, e As EventArgs)
        Dim filter As String = ""
        If CType(pnlFilters.FindControl("textbox50"), TextBox).Text <> "" Then
            filter += "SEASON like '%" & CType(pnlFilters.FindControl("textbox50"), TextBox).Text & "%' and "
        End If
        If CType(pnlFilters.FindControl("textbox51"), TextBox).Text <> "" Then
            filter += "CODE like '%" & CType(pnlFilters.FindControl("textbox51"), TextBox).Text & "%' and "
        End If
        If CType(pnlFilters.FindControl("textbox52"), TextBox).Text <> "" Then
            filter += "AREAMANAGER like '%" & CType(pnlFilters.FindControl("textbox52"), TextBox).Text & "%' and "
        End If
        If CType(pnlFilters.FindControl("textbox53"), TextBox).Text <> "" Then
            filter += "AGENT like '%" & CType(pnlFilters.FindControl("textbox53"), TextBox).Text & "%' and "
        End If
        If CType(pnlFilters.FindControl("textbox54"), TextBox).Text <> "" Then
            filter += "BPCNUM_0 like '%" & CType(pnlFilters.FindControl("textbox54"), TextBox).Text & "%' and "
        End If
        If CType(pnlFilters.FindControl("textbox55"), TextBox).Text <> "" Then
            filter += "CRY_0 like '%" & CType(pnlFilters.FindControl("textbox55"), TextBox).Text & "%' and "
        End If
        If CType(pnlFilters.FindControl("textbox56"), TextBox).Text <> "" Then
            filter += "SAT_0 like '%" & CType(pnlFilters.FindControl("textbox56"), TextBox).Text & "%' and "
        End If
        loadItems(filter)
    End Sub

    Protected Sub loadItems(ByVal filter As String)
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        Dim dt As DataTable = New DataTable
        Dim tfield = New TemplateField()
        Dim column As Integer = 0
        Try
            tfield.HeaderText = ""
            pnlFilters.Visible = False
            grdItems.AutoGenerateDeleteButton = True
            grdItems.AutoGenerateSelectButton = True
            lnkExcelExport.Visible = True
            lnkCreateNew.Visible = True
            grdItems.Columns.Clear()
            Select Case table
                Case "STAGE_REGISTRATIONS"
                    title.Text = "REGISTRAZIONI"
                    cmd.CommandText = "select STAGE_REGISTRATIONS.ID,CODE+'-'+convert(nvarchar(20),STAGE_CALENDAR.DATE,103)+'-'+STAGE_CALENDAR.ORIGIN+'-'+STAGE_REGISTRATIONS.GROUPID,BPCNUM_0,BPCNAM_0,SAT_0,CRY_0," &
                                      "NPARTECIPANTS,NOVERNIGHTS,NONENIGHTS,NTWONIGHTS,NTHREENIGHTS,NNONIGHTS,PICKUP,STAGE_REGISTRATIONS.GROUPID from STAGE_REGISTRATIONS inner join STAGE_CALENDAR on STAGE_REGISTRATIONS.IDCALENDAR=STAGE_CALENDAR.ID " &
                                      "and STAGE_REGISTRATIONS.GROUPID=STAGE_CALENDAR.GROUPID inner join STAGE_COURSES on STAGE_CALENDAR.IDCOURSE=STAGE_COURSES.ID and STAGE_CALENDAR.GROUPID=STAGE_COURSES.GROUPID"
                    column = 14
                    dt.Columns.Add("ID")
                    dt.Columns.Add("Calendario")
                    dt.Columns.Add("Codice CAP")
                    dt.Columns.Add("Nome CAP")
                    dt.Columns.Add("Provincia CAP")
                    dt.Columns.Add("Stato CAP")
                    dt.Columns.Add("N° partecipanti")
                    dt.Columns.Add("N° pernottamenti")
                    dt.Columns.Add("N° una notte")
                    dt.Columns.Add("N° due notti")
                    dt.Columns.Add("N° tre notti")
                    dt.Columns.Add("N° nessuna notte")
                    dt.Columns.Add("Pickup")
                    dt.Columns.Add("Gruppo")
                    grdItems.Columns.Add(tfield)
                Case "STAGE_PARTICIPANTS"
                    title.Text = "PARTECIPANTI"
                    cmd.CommandText = "select ID,NAME,LASTNAME,PICKUP,LOGINCODE from STAGE_PARTICIPANTS where IDREGISTRATION='" & idRegistration & "'"
                    column = 5
                    dt.Columns.Add("ID")
                    dt.Columns.Add("Nome")
                    dt.Columns.Add("Cognome")
                    dt.Columns.Add("Pickup")
                    dt.Columns.Add("LoginCode")
                Case "STAGE_COURSES"
                    title.Text = "CORSI"
                    cmd.CommandText = "select ID,CODE,NAME,DURATION,MIN,MAX,GROUPID from STAGE_COURSES"
                    column = 7
                    dt.Columns.Add("ID")
                    dt.Columns.Add("Codice")
                    dt.Columns.Add("Corso")
                    dt.Columns.Add("Durata")
                    dt.Columns.Add("Min")
                    dt.Columns.Add("Max")
                    dt.Columns.Add("Gruppo")
                Case "STAGE_CALENDAR"
                    title.Text = "CALENDARI"
                    cmd.CommandText = "select STAGE_CALENDAR.ID,STAGE_LOCATIONS.NAME,CODE,convert(nvarchar(20),DATE,103),SEASON,PROFESSOR,ORIGIN,STAGE_CALENDAR.GROUPID from STAGE_CALENDAR inner join STAGE_COURSES on " &
                                      "STAGE_CALENDAR.IDCOURSE=STAGE_COURSES.ID and STAGE_CALENDAR.GROUPID=STAGE_COURSES.GROUPID inner join STAGE_LOCATIONS on STAGE_CALENDAR.IDLOCATION=STAGE_LOCATIONS.ID and STAGE_CALENDAR.GROUPID=STAGE_LOCATIONS.GROUPID"
                    column = 8
                    dt.Columns.Add("ID")
                    dt.Columns.Add("Training")
                    dt.Columns.Add("Codice corso")
                    dt.Columns.Add("Data corso")
                    dt.Columns.Add("Stagione")
                    dt.Columns.Add("Professore")
                    dt.Columns.Add("Origine")
                    dt.Columns.Add("Gruppo")
                Case "STAGE_LOCATIONS"
                    title.Text = "SITI"
                    cmd.CommandText = "select ID,SITE,NAME,ADDRESS,GROUPID from STAGE_LOCATIONS"
                    column = 5
                    dt.Columns.Add("ID")
                    dt.Columns.Add("Sito")
                    dt.Columns.Add("Training")
                    dt.Columns.Add("Indirizzo")
                    dt.Columns.Add("Gruppo")
                Case "STAGE_COSTS"
                    title.Text = "COSTI"
                    cmd.CommandText = "select STAGE_COSTS.ID,STAGE_COSTITEMS.NAME,STAGE_COURSES.CODE,STAGE_CALENDAR.DATE,STAGE_CALENDAR.ORIGIN,STAGE_COSTS.VALUE,STAGE_COSTS.GROUPID from STAGE_COSTITEMS inner join STAGE_COSTS on STAGE_COSTITEMS.ID=STAGE_COSTS.IDCOSTITEMS " &
                                      "inner join STAGE_CALENDAR on STAGE_COSTS.IDCALENDAR=STAGE_CALENDAR.ID and STAGE_COSTS.GROUPID=STAGE_CALENDAR.GROUPID inner join STAGE_COURSES on STAGE_CALENDAR.IDCOURSE=STAGE_COURSES.ID and STAGE_CALENDAR.GROUPID=STAGE_COURSES.GROUPID"
                    column = 7
                    dt.Columns.Add("ID")
                    dt.Columns.Add("Costo")
                    dt.Columns.Add("Corso")
                    dt.Columns.Add("Data corso")
                    dt.Columns.Add("Origine")
                    dt.Columns.Add("Importo")
                    dt.Columns.Add("Gruppo")
                Case "STAGE_COURSESSUMMARY" 'SUMMARY
                    title.Text = "RIEPILOGO CORSI"
                    pnlFilters.Visible = True
                    grdItems.AutoGenerateDeleteButton = False
                    grdItems.AutoGenerateSelectButton = False
                    lnkExcelExport.Visible = False
                    lnkCreateNew.Visible = False
                    cmd.CommandText = "select AREAMANAGER,AGENT,BPCNUM_0,BPCNAM_0,SAT_0,CRY_0,CODE,NAME,DATE,DURATION,NPARTECIPANTS,SEASON from STAGE_COURSESSUMMARY " '& "where " & filter & "1=1"
                    column = 12
                    dt.Columns.Add("Capo Area")
                    dt.Columns.Add("Agente")
                    dt.Columns.Add("Codice Cliente")
                    dt.Columns.Add("Cliente")
                    dt.Columns.Add("Provincia")
                    dt.Columns.Add("Stato")
                    dt.Columns.Add("Corso")
                    dt.Columns.Add("Nome Corso")
                    dt.Columns.Add("Data Corso")
                    dt.Columns.Add("Durata Corso")
                    dt.Columns.Add("N° Partecipanti")
                    dt.Columns.Add("Stagione")
            End Select
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            While reader.Read
                Dim dr As DataRow = dt.NewRow
                For i As Integer = 0 To column - 1
                    dr(i) = reader(i)
                Next
                dt.Rows.Add(dr)
            End While
            grdItems.DataSource = dt
            grdItems.DataBind()
            sqlConnection1.Close()
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
        End Try
    End Sub

    Protected Sub loadItem(ByVal id As Integer)
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        Dim TextBox As TextBox
        Dim drop As DropDownList
        Dim Checkbox As CheckBox
        Dim n As Integer = 0
        Try
            Select Case table
                Case "STAGE_REGISTRATIONS"
                    cmd.CommandText = "select CODE+'-'+convert(nvarchar(20),STAGE_CALENDAR.DATE,103)+'-'+STAGE_CALENDAR.ORIGIN+'-'+STAGE_REGISTRATIONS.GROUPID,BPCNUM_0,BPCNAM_0,SAT_0,CRY_0," &
                                      "CAPPHONE,AGENTCODE,AGENTNAME,NPARTECIPANTS,NOVERNIGHTS,REPRESENTATIVE,DISCOUNT10,HELPER,HELPERNAME,HELPERWORK,NONENIGHTS,NTWONIGHTS,NTHREENIGHTS,NNONIGHTS," &
                                      "PICKUP,NOTES,REGISTRATIONS.GROUPID from STAGE_REGISTRATIONS inner join STAGE_CALENDAR on STAGE_REGISTRATIONS.IDCALENDAR=STAGE_CALENDAR.ID and STAGE_REGISTRATIONS.GROUPID=STAGE_CALENDAR.GROUPID " &
                                      "inner join STAGE_COURSES on STAGE_CALENDAR.IDCOURSE=STAGE_COURSES.ID and STAGE_CALENDAR.GROUPID=STAGE_COURSES.GROUPID where STAGE_REGISTRATIONS.ID='" & id & "'"
                    n = 21
                Case "STAGE_PARTICIPANTS"
                    cmd.CommandText = "select NAME,LASTNAME,BIRTHYEAR,ADDRESS,CITY,ZIPCODE,COUNTY,STATE,DESCLANGUAGE,EMAIL,PHONE,FAX,MOBILEPHONE,CUSTOMERTYPE,PARTICIPANTS.PICKUP,LOGINCODE " &
                                       "from STAGE_REGISTRATIONS inner join STAGE_PARTICIPANTS on STAGE_REGISTRATIONS.ID=IDREGISTRATION and STAGE_REGISTRATIONS.GROUPID=STAGE_PARTICIPANTS.GROUPID inner join STAGE_LANGUAGES on IDLANGUAGE=STAGE_LANGUAGES.ID " &
                                       "where STAGE_PARTICIPANTS.ID='" & id & "'"
                    n = 15
                Case "STAGE_CALENDAR"
                    cmd.CommandText = "select STAGE_LOCATIONS.NAME+'-'+STAGE_CALENDAR.GROUPID,CODE+'-'+STAGE_CALENDAR.GROUPID,convert(nvarchar(20),DATE,103),SEASON,PROFESSOR,ORIGIN,STAGE_CALENDAR.GROUPID from STAGE_CALENDAR inner join STAGE_COURSES on STAGE_CALENDAR.IDCOURSE=STAGE_COURSES.ID and " &
                                      "STAGE_CALENDAR.GROUPID=STAGE_COURSES.GROUPID inner join STAGE_LOCATIONS on STAGE_CALENDAR.IDlOCATION=STAGE_LOCATIONS.ID and STAGE_CALENDAR.GROUPID=STAGE_LOCATIONS.GROUPID where STAGE_CALENDAR.ID='" & id & "'"
                    n = 6
                Case "STAGE_COURSES"
                    cmd.CommandText = "select CODE,NAME,DURATION,MIN,MAX,GROUPID from STAGE_COURSES where ID='" & id & "'"
                    n = 5
                Case "STAGE_LOCATIONS"
                    cmd.CommandText = "select SITE,NAME,ADDRESS,GROUPID from STAGE_LOCATIONS where ID='" & id & "'"
                    n = 3
                Case "STAGE_COSTS"
                    cmd.CommandText = "select STAGE_COSTITEMS.NAME,CODE+'-'+convert(nvarchar(20),date,103)+'-'+ORIGIN+'-'+STAGE_COSTS.GROUPID,VALUE,STAGE_COSTS.GROUPID from STAGE_COSTS inner join STAGE_COSTITEMS on STAGE_COSTS.IDCOSTITEMS=STAGE_COSTITEMS.ID " &
                                      "inner join STAGE_CALENDAR on STAGE_COSTS.IDCALENDAR=STAGE_CALENDAR.ID and STAGE_COSTS.GROUPID=STAGE_CALENDAR.GROUPID inner join STAGE_COURSES on STAGE_CALENDAR.IDCOURSE=STAGE_COURSES.ID and STAGE_CALENDAR.GROUPID=STAGE_COURSES.GROUPID " &
                                      "where STAGE_COSTS.ID='" & id & "'"
                    n = 3
            End Select
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            While reader.Read
                If CType(pnlFields.FindControl("TextBox1"), TextBox) IsNot Nothing Or CType(pnlFields.FindControl("DropDownList8"), DropDownList) IsNot Nothing Then
                    For i As Integer = 0 To n
                        If i = 0 And (table = "STAGE_REGISTRATIONS" Or table = "STAGE_CALENDAR" Or table = "STAGE_COSTS") Then
                            drop = CType(pnlFields.FindControl("DropDownList8"), DropDownList)
                            drop.SelectedValue = IIf(IsDBNull(reader(i)), vbNullString, reader(i))
                        ElseIf i = 1 And (table = "STAGE_CALENDAR" Or table = "STAGE_COSTS") Then
                            drop = CType(pnlFields.FindControl("DropDownList9"), DropDownList)
                            drop.SelectedValue = IIf(IsDBNull(reader(i)), vbNullString, reader(i))
                        ElseIf (i = 11 Or i = 12 Or i = 14 Or i = 19) And table = "STAGE_REGISTRATIONS" Then
                            Checkbox = CType(pnlFields.FindControl("CheckBox" & i + 1), CheckBox)
                            Checkbox.Checked = IIf(IsDBNull(reader(i)), False, reader(i))
                        ElseIf i = 8 And table = "STAGE_PARTICIPANTS" Then
                            drop = CType(pnlFields.FindControl("DropDownList8"), DropDownList)
                            drop.SelectedValue = IIf(IsDBNull(reader(i)), vbNullString, reader(i))
                        ElseIf i = 14 And table = "STAGE_PARTICIPANTS" Then
                            Checkbox = CType(pnlFields.FindControl("CheckBox" & i + 1), CheckBox)
                            Checkbox.Checked = IIf(IsDBNull(reader(i)), False, reader(i))
                        Else
                            TextBox = CType(pnlFields.FindControl("TextBox" & i + 1), TextBox)
                            TextBox.Text = IIf(IsDBNull(reader(i)), "", reader(i))
                        End If
                    Next
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
    End Sub

    Protected Sub loadFields()
        Dim fieldsNames(1) As String
        pnlFields.Controls.Clear()
        Select Case table
            Case "STAGE_REGISTRATIONS"
                Array.Resize(fieldsNames, 22)
                fieldsNames = New String() {"*Calendario:", "Codice CAP:", "Nome CAP:", "Provincia CAP:", "Stato CAP:", "Telefono CAP:", "Codice agente:", "Nome agente:", "*N° partecipanti:",
                                            "*N° pernottamenti:", "Referente:", "*Sconto 10%:", "Accompagnatore:", "Nome accompagnatore:", "Applica:", "N° una notte:", "N° due notti:", "N° tre notti:",
                                            "N° nessuna notte:", "Pickup:", "Note:", "*Gruppo:"}
            Case "STAGE_PARTICIPANTS"
                Array.Resize(fieldsNames, 16)
                fieldsNames = New String() {"Nome:", "Cognome:", "Anno di nascita:", "Indirizzo:", "Città:", "CAP:", "Provincia:", "Stato:", "*Lingua:", "Email:", "Telefono:", "Fax:", "Cellulare:",
                                            "Tipo cliente:", "Pickup:", "LoginCode:"}
            Case "STAGE_CALENDAR"
                Array.Resize(fieldsNames, 7)
                fieldsNames = New String() {"*Sito:", "*Corso:", "*Data:", "*Stagione:", "Docente:", "*Origine:", "*Gruppo:"}
            Case "STAGE_COURSES"
                Array.Resize(fieldsNames, 6)
                fieldsNames = New String() {"*Codice:", "Nome:", "*Durata:", "*Min:", "*Max:", "*Gruppo:"}
            Case "STAGE_LOCATIONS"
                Array.Resize(fieldsNames, 4)
                fieldsNames = New String() {"Sito:", "Training:", "Indirizzo:", "*Gruppo:"}
            Case "STAGE_COSTS"
                Array.Resize(fieldsNames, 4)
                fieldsNames = New String() {"*Costo:", "Calendario:", "Importo:", "*Gruppo:"}
        End Select
        pnlFields.Controls.Add(New LiteralControl("<br />"))
        For i As Integer = 0 To fieldsNames.Length - 1
            Dim label As New Label
            Dim text As New TextBox
            Dim drop As New DropDownList
            Dim check As New CheckBox
            Dim validate As New RequiredFieldValidator
            label.ID = "Label" & i
            label.Text = fieldsNames(i)
            pnlFields.Controls.Add(label)
            pnlFields.Controls.Add(New LiteralControl("<br />"))
            If i = 0 And (table = "STAGE_REGISTRATIONS" Or table = "STAGE_CALENDAR" Or table = "STAGE_COSTS") Then
                drop.ID = "DropDownList8"
                drop.CssClass = "form-control"
                drop.EnableViewState = True
                pnlFields.Controls.Add(drop)
                If table = "STAGE_REGISTRATIONS" Then
                    loadCalendar("DropDownList8")
                ElseIf table = "STAGE_CALENDAR" Then
                    loadLocations()
                Else
                    loadCostItems()
                End If
                If CType(pnlFields.FindControl("Label" & i), Label).Text.Contains("*") Then
                    validate.ID = "RequiredFieldValidator" & i
                    validate.ControlToValidate = drop.ID
                    validate.ErrorMessage = "*campo obbligatorio"
                    validate.Display = ValidatorDisplay.Dynamic
                    validate.ForeColor = Drawing.Color.Red
                    validate.ValidationGroup = "group1"
                    pnlFields.Controls.Add(validate)
                End If
            ElseIf i = 1 And (table = "STAGE_CALENDAR" Or table = "STAGE_COSTS") Then
                drop.ID = "DropDownList9"
                drop.CssClass = "form-control"
                drop.EnableViewState = True
                pnlFields.Controls.Add(drop)
                If table = "STAGE_CALENDAR" Then
                    loadCourses()
                Else
                    loadCalendar("DropDownList9")
                End If
                If CType(pnlFields.FindControl("Label" & i), Label).Text.Contains("*") Then
                    validate.ID = "RequiredFieldValidator" & i
                    validate.ControlToValidate = drop.ID
                    validate.ErrorMessage = "*campo obbligatorio"
                    validate.Display = ValidatorDisplay.Dynamic
                    validate.ForeColor = Drawing.Color.Red
                    validate.ValidationGroup = "group1"
                    pnlFields.Controls.Add(validate)
                End If
            ElseIf i = 8 And table = "STAGE_PARTICIPANTS" Then
                drop.ID = "DropDownList8"
                drop.CssClass = "form-control"
                drop.EnableViewState = True
                pnlFields.Controls.Add(drop)
                loadLanguages()
                If CType(pnlFields.FindControl("Label" & i), Label).Text.Contains("*") Then
                    validate.ID = "RequiredFieldValidator" & i
                    validate.ControlToValidate = drop.ID
                    validate.ErrorMessage = "*campo obbligatorio"
                    validate.Display = ValidatorDisplay.Dynamic
                    validate.ForeColor = Drawing.Color.Red
                    validate.ValidationGroup = "group1"
                    pnlFields.Controls.Add(validate)
                End If
            ElseIf (i = 11 Or i = 12 Or i = 14 Or i = 19) And table = "STAGE_REGISTRATIONS" Then
                check.ID = "CheckBox" & i + 1
                check.EnableViewState = True
                pnlFields.Controls.Add(check)
                pnlFields.Controls.Add(New LiteralControl("<br />"))
            ElseIf i = 14 And table = "STAGE_PARTICIPANTS" Then
                check.ID = "CheckBox" & i + 1
                check.EnableViewState = True
                pnlFields.Controls.Add(check)
                pnlFields.Controls.Add(New LiteralControl("<br />"))
            Else
                text.ID = "TextBox" & i + 1
                text.EnableViewState = True
                text.CssClass = "form-control"
                pnlFields.Controls.Add(text)
                If i = 1 And table = "STAGE_REGISTRATIONS" Then
                    Dim btn As New Button()
                    btn.ID = "btnCAP"
                    btn.Text = "Conferma CAP"
                    btn.CssClass = "btn btn-lg btn-primary btn-block"
                    AddHandler btn.Click, AddressOf btnConfirmCAP_Click
                    pnlFields.Controls.Add(btn)
                ElseIf i = 15 And table = "STAGE_PARTICIPANTS" Then
                    Dim btn As New Button()
                    btn.ID = "btnLoginCode"
                    btn.Text = "Genera Codice"
                    btn.CssClass = "btn btn-lg btn-primary btn-block"
                    AddHandler btn.Click, AddressOf btnGenerate_Click
                    pnlFields.Controls.Add(btn)
                End If
                If CType(pnlFields.FindControl("Label" & i), Label).Text.Contains("*") Then
                    validate.ID = "RequiredFieldValidator" & i
                    validate.ControlToValidate = text.ID
                    validate.ErrorMessage = "*campo obbligatorio"
                    validate.Display = ValidatorDisplay.Dynamic
                    validate.ForeColor = Drawing.Color.Red
                    validate.ValidationGroup = "group1"
                    pnlFields.Controls.Add(validate)
                End If
            End If
            pnlFields.Controls.Add(New LiteralControl("<br />"))
        Next
    End Sub

    Protected Sub loadLanguages()
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        Dim drop As DropDownList = CType(pnlFields.FindControl("DropDownList8"), DropDownList)
        Try
            cmd.CommandText = "select DESCLANGUAGE from STAGE_LANGUAGES"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            drop.Items.Clear()
            While reader.Read
                drop.Items.Add(reader(0))
            End While
            sqlConnection1.Close()
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
        End Try
    End Sub

    Protected Sub loadCostItems()
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        Dim drop As DropDownList = CType(pnlFields.FindControl("DropDownList8"), DropDownList)
        Try
            cmd.CommandText = "select NAME from STAGE_COSTITEMS"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            drop.Items.Clear()
            While reader.Read
                drop.Items.Add(reader(0))
            End While
            sqlConnection1.Close()
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
        End Try
    End Sub

    Protected Sub loadCalendar(ByVal field As String)
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        Dim drop As DropDownList = CType(pnlFields.FindControl(field), DropDownList)
        Try
            cmd.CommandText = "select CODE,DATE,ORIGIN,STAGE_CALENDAR.GROUPID from STAGE_COURSES inner join STAGE_CALENDAR on STAGE_COURSES.ID=STAGE_CALENDAR.IDCOURSE and STAGE_COURSES.GROUPID=STAGE_CALENDAR.GROUPID"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            drop.Items.Clear()
            While reader.Read
                drop.Items.Add(reader(0) & "-" & reader(1) & "-" & reader(2) & "-" & reader(3))
            End While
            sqlConnection1.Close()
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
        End Try
    End Sub

    Protected Sub loadCourses()
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        Dim drop As DropDownList = CType(pnlFields.FindControl("DropDownList9"), DropDownList)
        Try
            cmd.CommandText = "select CODE,GROUPID from STAGE_COURSES"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            drop.Items.Clear()
            While reader.Read
                drop.Items.Add(reader(0) & "-" & reader(1))
            End While
            sqlConnection1.Close()
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
        End Try
    End Sub

    Protected Sub loadLocations()
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        Dim drop As DropDownList = CType(pnlFields.FindControl("DropDownList8"), DropDownList)
        Try
            cmd.CommandText = "select NAME,GROUPID from STAGE_LOCATIONS"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            drop.Items.Clear()
            While reader.Read
                drop.Items.Add(reader(0) & "-" & reader(1))
            End While
            sqlConnection1.Close()
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
        End Try
    End Sub

    Protected Sub insert()
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Try
            Select Case table
                Case "STAGE_REGISTRATIONS"
                    cmd.CommandText = "insert into STAGE_REGISTRATIONS (IDCALENDAR,BPCNUM_0,BPCNAM_0,SAT_0,CRY_0,CAPPHONE,AGENTCODE,AGENTNAME,DATE,NPARTECIPANTS,NOVERNIGHTS,REPRESENTATIVE,DISCOUNT10,HELPER," &
                                      "HELPERNAME,HELPERWORK,NONENIGHTS,NTWONIGHTS,NTHREENIGHTS,NNONIGHTS,PICKUP,NOTES,GROUPID) values ('" & idCalendar(CType(pnlFields.FindControl("DropDownList8"), DropDownList).Text) & "','" &
                                      Replace(CType(pnlFields.FindControl("TextBox2"), TextBox).Text, "'", "''") & "','" & Replace(CType(pnlFields.FindControl("TextBox3"), TextBox).Text, "'", "''") & "','" &
                                      Replace(CType(pnlFields.FindControl("TextBox4"), TextBox).Text, "'", "''") & "','" & Replace(CType(pnlFields.FindControl("TextBox5"), TextBox).Text, "'", "''") & "','" &
                                      Replace(CType(pnlFields.FindControl("TextBox6"), TextBox).Text, "'", "''") & "','" & Replace(CType(pnlFields.FindControl("TextBox7"), TextBox).Text, "'", "''") & "','" &
                                      Replace(CType(pnlFields.FindControl("TextBox8"), TextBox).Text, "'", "''") & "','" & Format(Date.Now, "MM/dd/yyyy") & "','" & Replace(CType(pnlFields.FindControl("TextBox9"), TextBox).Text, "'", "''") & "','" &
                                      Replace(CType(pnlFields.FindControl("TextBox10"), TextBox).Text, "'", "''") & "','" & Replace(CType(pnlFields.FindControl("TextBox11"), TextBox).Text, "'", "''") & "','" &
                                      CType(pnlFields.FindControl("CheckBox12"), CheckBox).Checked & "','" & CType(pnlFields.FindControl("CheckBox13"), CheckBox).Checked & "','" &
                                      Replace(CType(pnlFields.FindControl("TextBox14"), TextBox).Text, "'", "''") & "','" & CType(pnlFields.FindControl("CheckBox15"), CheckBox).Checked & "','" &
                                      Replace(CType(pnlFields.FindControl("TextBox16"), TextBox).Text, "'", "''") & "','" & Replace(CType(pnlFields.FindControl("TextBox17"), TextBox).Text, "'", "''") & "','" &
                                      Replace(CType(pnlFields.FindControl("TextBox18"), TextBox).Text, "'", "''") & "','" & Replace(CType(pnlFields.FindControl("TextBox19"), TextBox).Text, "'", "''") & "','" &
                                      CType(pnlFields.FindControl("CheckBox20"), CheckBox).Checked & "','" & Replace(CType(pnlFields.FindControl("TextBox21"), TextBox).Text, "'", "''") & "','" &
                                      Replace(CType(pnlFields.FindControl("TextBox22"), TextBox).Text.ToUpper, "'", "''") & "')"
                Case "STAGE_PARTICIPANTS"
                    cmd.CommandText = "insert into STAGE_PARTICIPANTS (NAME,LASTNAME,BIRTHYEAR,ADDRESS,ZIPCODE,CITY,COUNTY,STATE,IDLANGUAGE,EMAIL,PHONE,FAX,MOBILEPHONE,CUSTOMERTYPE,IDREGISTRATION,PICKUP,LOGINCODE,GROUPID) " &
                                      "values('" & Replace(CType(pnlFields.FindControl("TextBox1"), TextBox).Text, "'", "''") & "','" & Replace(CType(pnlFields.FindControl("TextBox2"), TextBox).Text, "'", "''") & "','" &
                                      Replace(CType(pnlFields.FindControl("TextBox3"), TextBox).Text, "'", "''") & "','" & Replace(CType(pnlFields.FindControl("TextBox4"), TextBox).Text, "'", "''") & "','" &
                                      Replace(CType(pnlFields.FindControl("TextBox5"), TextBox).Text, "'", "''") & "','" & Replace(CType(pnlFields.FindControl("TextBox6"), TextBox).Text, "'", "''") & "','" &
                                      Replace(CType(pnlFields.FindControl("TextBox7"), TextBox).Text, "'", "''") & "','" & Replace(CType(pnlFields.FindControl("TextBox8"), TextBox).Text, "'", "''") & "','" &
                                      idLanguage(CType(pnlFields.FindControl("DropDownList8"), DropDownList).Text) & "','" & Replace(CType(pnlFields.FindControl("TextBox10"), TextBox).Text, "'", "''") & "','" &
                                      Replace(CType(pnlFields.FindControl("TextBox11"), TextBox).Text, "'", "''") & "','" & Replace(CType(pnlFields.FindControl("TextBox12"), TextBox).Text, "'", "''") & "','" &
                                      Replace(CType(pnlFields.FindControl("TextBox13"), TextBox).Text, "'", "''") & "','" & Replace(CType(pnlFields.FindControl("TextBox14"), TextBox).Text, "'", "''") & "','" &
                                      idRegistration & "','" & CType(pnlFields.FindControl("CheckBox15"), CheckBox).Checked & "','" & Replace(CType(pnlFields.FindControl("TextBox16"), TextBox).Text.ToUpper, "'", "''") & "','" &
                                      GROUPID & "')"
                Case "STAGE_CALENDAR"
                    cmd.CommandText = "insert into STAGE_CALENDAR (IDLOCATION,IDCOURSE,DATE,SEASON,PROFESSOR,ORIGIN,GROUPID) values ('" & idLocation(Replace(CType(pnlFields.FindControl("DropDownList8"), DropDownList).Text, "'", "''")) & "','" &
                                      idCourse(Replace(CType(pnlFields.FindControl("DropDownList9"), DropDownList).Text, "'", "''")) & "','" & Format(CDate(CType(pnlFields.FindControl("TextBox3"), TextBox).Text), "MM/dd/yyyy") & "','" &
                                      Replace(CType(pnlFields.FindControl("TextBox4"), TextBox).Text, "'", "''") & "','" & Replace(CType(pnlFields.FindControl("TextBox5"), TextBox).Text, "'", "''") & "'," &
                                      "'" & Replace(CType(pnlFields.FindControl("TextBox6"), TextBox).Text, "'", "''") & "','" & Replace(CType(pnlFields.FindControl("TextBox7"), TextBox).Text.ToUpper, "'", "''") & "')"
                Case "STAGE_COURSES"
                    cmd.CommandText = "insert into STAGE_COURSES (CODE,NAME,DURATION,MIN,MAX,GROUPID) values ('" & Replace(CType(pnlFields.FindControl("TextBox1"), TextBox).Text, "'", "''") & "','" &
                                      Replace(CType(pnlFields.FindControl("TextBox2"), TextBox).Text, "'", "''") & "','" & Replace(CType(pnlFields.FindControl("TextBox3"), TextBox).Text, "'", "''") & "','" &
                                      Replace(CType(pnlFields.FindControl("TextBox4"), TextBox).Text, "'", "''") & "','" & Replace(CType(pnlFields.FindControl("TextBox5"), TextBox).Text, "'", "''") & "','" &
                                      Replace(CType(pnlFields.FindControl("TextBox6"), TextBox).Text.ToUpper, "'", "''") & "')"
                Case "STAGE_LOCATIONS"
                    cmd.CommandText = "insert into STAGE_LOCATIONS (SITE,NAME,ADDRESS,GROUPID) values ('" & Replace(CType(pnlFields.FindControl("TextBox1"), TextBox).Text, "'", "''") & "','" &
                                      Replace(CType(pnlFields.FindControl("TextBox2"), TextBox).Text, "'", "''") & "','" & Replace(CType(pnlFields.FindControl("TextBox3"), TextBox).Text, "'", "''") & "','" &
                                      Replace(CType(pnlFields.FindControl("TextBox4"), TextBox).Text.ToUpper, "'", "''") & "')"
                Case "STAGE_COSTS"
                    cmd.CommandText = "insert into STAGE_COSTS (IDCOSTITEMS,IDCALENDAR,VALUE,GROUPID) values ('" & idCostItem(Replace(CType(pnlFields.FindControl("DropDownList8"), DropDownList).Text, "'", "''")) & "','" &
                                      idCalendar(Replace(CType(pnlFields.FindControl("DropDownList9"), DropDownList).Text, "'", "''")) & "','" & Replace(CType(pnlFields.FindControl("TextBox3"), TextBox).Text, "'", "''") & "','" &
                                      Replace(CType(pnlFields.FindControl("TextBox4"), TextBox).Text.ToUpper, "'", "''") & "')"
            End Select
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            cmd.ExecuteNonQuery()
            sqlConnection1.Close()
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('Insert done');", True)
            loadable = False
            btnSave.Visible = False
            pnlFields.Controls.Clear()
            loadItems("")
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
            action = ""
        End Try
    End Sub

    Protected Sub update()
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Try
            Select Case table
                Case "STAGE_REGISTRATIONS"
                    cmd.CommandText = "update STAGE_REGISTRATIONS set IDCALENDAR='" & idCalendar(CType(pnlFields.FindControl("DropDownList8"), DropDownList).Text) & "',BPCNUM_0='" & Replace(CType(pnlFields.FindControl("TextBox2"), TextBox).Text, "'", "''") &
                                      "',BPCNAM_0='" & Replace(CType(pnlFields.FindControl("TextBox3"), TextBox).Text, "'", "''") & "',SAT_0='" & Replace(CType(pnlFields.FindControl("TextBox4"), TextBox).Text, "'", "''") &
                                      "',CRY_0='" & Replace(CType(pnlFields.FindControl("TextBox5"), TextBox).Text, "'", "''") & "',CAPPHONE='" & Replace(CType(pnlFields.FindControl("TextBox6"), TextBox).Text, "'", "''") &
                                      "',AGENTCODE='" & Replace(CType(pnlFields.FindControl("TextBox7"), TextBox).Text, "'", "''") & "',AGENTNAME='" & Replace(CType(pnlFields.FindControl("TextBox8"), TextBox).Text, "'", "''") &
                                      "',NPARTECIPANTS='" & Replace(CType(pnlFields.FindControl("TextBox9"), TextBox).Text, "'", "''") & "',NOVERNIGHTS='" & Replace(CType(pnlFields.FindControl("TextBox10"), TextBox).Text, "'", "''") &
                                      "',REPRESENTATIVE='" & Replace(CType(pnlFields.FindControl("TextBox11"), TextBox).Text, "'", "''") & "',DISCOUNT10='" & CType(pnlFields.FindControl("CheckBox12"), CheckBox).Checked &
                                      "',HELPER='" & CType(pnlFields.FindControl("CheckBox13"), CheckBox).Checked & "',HELPERNAME='" & Replace(CType(pnlFields.FindControl("TextBox14"), TextBox).Text, "'", "''") &
                                      "',HELPERWORK='" & CType(pnlFields.FindControl("CheckBox15"), CheckBox).Checked & "',NONENIGHTS='" & Replace(CType(pnlFields.FindControl("TextBox16"), TextBox).Text, "'", "''") &
                                      "',NTWONIGHTS='" & Replace(CType(pnlFields.FindControl("TextBox17"), TextBox).Text, "'", "''") & "',NTHREENIGHTS='" & Replace(CType(pnlFields.FindControl("TextBox18"), TextBox).Text, "'", "''") &
                                      "',NNONIGHTS='" & Replace(CType(pnlFields.FindControl("TextBox19"), TextBox).Text, "'", "''") & "',PICKUP='" & CType(pnlFields.FindControl("CheckBox20"), CheckBox).Checked &
                                      "',NOTES='" & Replace(CType(pnlFields.FindControl("TextBox21"), TextBox).Text, "'", "''") & "',GROUPID='" & Replace(CType(pnlFields.FindControl("TextBox22"), TextBox).Text.ToUpper, "'", "''") &
                                      "' where ID='" & action.Remove(0, 6) & "'"
                Case "STAGE_PARTICIPANTS"
                    cmd.CommandText = "update STAGE_PARTICIPANTS set NAME='" & Replace(CType(pnlFields.FindControl("TextBox1"), TextBox).Text, "'", "''") & "',LASTNAME='" & Replace(CType(pnlFields.FindControl("TextBox2"), TextBox).Text, "'", "''") &
                                      "',BIRTHYEAR='" & Replace(CType(pnlFields.FindControl("TextBox3"), TextBox).Text, "'", "''") & "',ADDRESS='" & Replace(CType(pnlFields.FindControl("TextBox4"), TextBox).Text, "'", "''") &
                                      "',ZIPCODE='" & Replace(CType(pnlFields.FindControl("TextBox5"), TextBox).Text, "'", "''") & "',CITY='" & Replace(CType(pnlFields.FindControl("TextBox6"), TextBox).Text, "'", "''") &
                                      "',COUNTY='" & Replace(CType(pnlFields.FindControl("TextBox7"), TextBox).Text, "'", "''") & "',STATE='" & Replace(CType(pnlFields.FindControl("TextBox8"), TextBox).Text, "'", "''") &
                                      "',IDLANGUAGE='" & idLanguage(CType(pnlFields.FindControl("DropDownList8"), DropDownList).Text) & "',EMAIL='" & Replace(CType(pnlFields.FindControl("TextBox10"), TextBox).Text, "'", "''") &
                                      "',PHONE='" & Replace(CType(pnlFields.FindControl("TextBox11"), TextBox).Text, "'", "''") & "',FAX='" & Replace(CType(pnlFields.FindControl("TextBox12"), TextBox).Text, "'", "''") &
                                      "',MOBILEPHONE='" & Replace(CType(pnlFields.FindControl("TextBox13"), TextBox).Text, "'", "''") & "',CUSTOMERTYPE='" & Replace(CType(pnlFields.FindControl("TextBox14"), TextBox).Text, "'", "''") &
                                      "',IDREGISTRATION='" & idRegistration & "',PICKUP='" & CType(pnlFields.FindControl("CheckBox15"), CheckBox).Checked & "',LOGINCODE='" & Replace(CType(pnlFields.FindControl("TextBox16"), TextBox).Text, "'", "''") &
                                      "',GROUPID='" & GROUPID & "' where ID='" & action.Remove(0, 6) & "'"
                Case "STAGE_CALENDAR"
                    cmd.CommandText = "update STAGE_CALENDAR set IDLOCATION='" & idLocation(Replace(CType(pnlFields.FindControl("DropDownList8"), DropDownList).Text, "'", "''")) & "',IDCOURSE='" &
                                      idCourse(Replace(CType(pnlFields.FindControl("DropDownList9"), DropDownList).Text, "'", "''")) & "',DATE='" & Format(CDate(CType(pnlFields.FindControl("TextBox3"), TextBox).Text), "MM/dd/yyyy") & "'," &
                                      "SEASON='" & Replace(CType(pnlFields.FindControl("TextBox4"), TextBox).Text, "'", "''") & "',PROFESSOR='" & Replace(CType(pnlFields.FindControl("TextBox5"), TextBox).Text, "'", "''") & "' " &
                                      ",ORIGIN='" & Replace(CType(pnlFields.FindControl("TextBox6"), TextBox).Text, "'", "''") & "',GROUPID='" & Replace(CType(pnlFields.FindControl("TextBox7"), TextBox).Text.ToUpper, "'", "''") & "' " &
                                      "where ID='" & action.Remove(0, 6) & "'"
                Case "STAGE_COURSES"
                    cmd.CommandText = "update STAGE_COURSES set CODE='" & Replace(CType(pnlFields.FindControl("TextBox1"), TextBox).Text, "'", "''") & "',NAME='" & Replace(CType(pnlFields.FindControl("TextBox2"), TextBox).Text, "'", "''") &
                                      "',DURATION='" & Replace(CType(pnlFields.FindControl("TextBox3"), TextBox).Text, "'", "''") & "',MIN='" & Replace(CType(pnlFields.FindControl("TextBox4"), TextBox).Text, "'", "''") &
                                      "',MAX='" & Replace(CType(pnlFields.FindControl("TextBox5"), TextBox).Text, "'", "''") & "',GROUPID='" & Replace(CType(pnlFields.FindControl("TextBox6"), TextBox).Text.ToUpper, "'", "''") &
                                      "' where ID='" & action.Remove(0, 6) & "'"
                Case "STAGE_LOCATIONS"
                    cmd.CommandText = "update STAGE_LOCATIONS set SITE='" & Replace(CType(pnlFields.FindControl("TextBox1"), TextBox).Text, "'", "''") & "',NAME='" & Replace(CType(pnlFields.FindControl("TextBox2"), TextBox).Text, "'", "''") &
                                      "',ADDRESS='" & Replace(CType(pnlFields.FindControl("TextBox3"), TextBox).Text, "'", "''") & "',GROUPID='" & Replace(CType(pnlFields.FindControl("TextBox4"), TextBox).Text.ToUpper, "'", "''") &
                                      "' where ID='" & action.Remove(0, 6) & "'"
                Case "STAGE_COSTS"
                    cmd.CommandText = "update STAGE_COSTS set IDCOSTITEMS='" & idCostItem(Replace(CType(pnlFields.FindControl("DropDownList8"), DropDownList).Text, "'", "''")) & "',IDCALENDAR='" &
                                      idCalendar(Replace(CType(pnlFields.FindControl("DropDownList9"), DropDownList).Text, "'", "''")) & "',VALUE='" & Replace(CType(pnlFields.FindControl("TextBox3"), TextBox).Text, "'", "''") & "'," &
                                      "GROUPID='" & Replace(CType(pnlFields.FindControl("TextBox4"), TextBox).Text.ToUpper, "'", "''") & "' where ID='" & action.Remove(0, 6) & "'"
            End Select
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            cmd.ExecuteNonQuery()
            sqlConnection1.Close()
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('Update done');", True)
            loadable = False
            btnSave.Visible = False
            pnlFields.Controls.Clear()
            loadItems("")
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
            action = ""
        End Try
    End Sub

    Protected Function idLanguage(ByVal descLanguage As String) As Integer
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        idLanguage = -1
        Try
            cmd.CommandText = "select ID from LANGUAGES where STAGE_DESCLANGUAGE='" & descLanguage & "'"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            While reader.Read
                idLanguage = reader(0)
            End While
            sqlConnection1.Close()
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
        End Try
        Return idLanguage
    End Function

    Protected Function descLanguage(ByVal idLanguage As Integer) As String
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        descLanguage = ""
        Try
            cmd.CommandText = "select DESCLANGUAGE from STAGE_LANGUAGES where ID='" & idLanguage & "'"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            While reader.Read
                descLanguage = reader(0)
            End While
            sqlConnection1.Close()
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
        End Try
        Return descLanguage
    End Function

    Protected Function idCostItem(ByVal costItem As String) As Integer
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        idCostItem = -1
        Try
            cmd.CommandText = "select ID from STAGE_COSTITEMS where NAME='" & costItem & "'"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            While reader.Read
                idCostItem = reader(0)
            End While
            sqlConnection1.Close()
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
        End Try
        Return idCostItem
    End Function

    Protected Function idCalendar(ByVal calendar As String) As Integer
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        Dim calendarArray() As String = calendar.Split("-")
        idCalendar = -1
        Try
            cmd.CommandText = "select ID from STAGE_CALENDAR where IDCOURSE='" & idCourse(calendarArray(0) & "-" & calendarArray(3)) & "' and DATE='" & Format(CDate(calendarArray(1)), "MM/dd/yyyy") & "' and " &
                              "ORIGIN='" & calendarArray(2) & "' and GROUPID='" & calendarArray(3) & "'"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            While reader.Read
                idCalendar = reader(0)
            End While
            sqlConnection1.Close()
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
        End Try
        Return idCalendar
    End Function

    Protected Function idLocation(ByVal location As String) As Integer
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        Dim locationArray() As String = location.Split("-")
        idLocation = -1
        Try
            cmd.CommandText = "select ID from STAGE_LOCATIONS where NAME='" & locationArray(0) & "' and GROUPID='" & locationArray(1) & "'"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            While reader.Read
                idLocation = reader(0)
            End While
            sqlConnection1.Close()
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
        End Try
        Return idLocation
    End Function

    Protected Function idCourse(ByVal course As String) As Integer
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        Dim courseArray() As String = course.Split("-")
        idCourse = -1
        Try
            cmd.CommandText = "select ID from STAGE_COURSES where CODE='" & courseArray(0) & "' and GROUPID='" & courseArray(1) & "'"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            While reader.Read
                idCourse = reader(0)
            End While
            sqlConnection1.Close()
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
        End Try
        Return idCourse
    End Function

    Protected Function infoCourse(ByVal idCourse As Integer) As String
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        infoCourse = ""
        Try
            If table = "STAGE_PARTICIPANTS" Then
                cmd.CommandText = "select CODE,STAGE_COURSES.GROUPID from STAGE_REGISTRATIONS inner join STAGE_CALENDAR on STAGE_REGISTRATIONS.IDCALENDAR=STAGE_CALENDAR.ID and STAGE_REGISTRATIONS.GROUPID=STAGE_CALENDAR.GROUPID inner join STAGE_COURSES on " &
                                  "STAGE_CALENDAR.IDCOURSE=STAGE_COURSES.ID and STAGE_CALENDAR.GROUPID=STAGE_COURSES.GROUPID where STAGE_REGISTRATIONS.ID='" & idCourse & "'"
            Else

                cmd.CommandText = "select CODE,GROUPID from STAGE_COURSES where ID='" & idCourse & "'"
            End If
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            While reader.Read
                infoCourse = reader(0) & ", " & reader(1)
            End While
            sqlConnection1.Close()
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
        End Try
        Return infoCourse
    End Function

End Class
