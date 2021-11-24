
Imports System.Data
Imports System.Data.SqlClient

Partial Class Default2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            loadItems("")
        End If
    End Sub

    Protected Sub grdItems_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        grdItems.PageIndex = e.NewPageIndex
        loadItems("")
    End Sub

    Protected Sub btnFilter_Click(sender As Object, e As EventArgs)
        Dim filter As String = ""
        If CType(pnlFilters.FindControl("textbox1"), TextBox).Text <> "" Then
            filter += "SEASON like '%" & CType(pnlFilters.FindControl("textbox1"), TextBox).Text & "%' and "
        End If
        If CType(pnlFilters.FindControl("textbox2"), TextBox).Text <> "" Then
            filter += "CODE like '%" & CType(pnlFilters.FindControl("textbox2"), TextBox).Text & "%' and "
        End If
        If CType(pnlFilters.FindControl("textbox3"), TextBox).Text <> "" Then
            filter += "AREAMANAGER like '%" & CType(pnlFilters.FindControl("textbox3"), TextBox).Text & "%' and "
        End If
        If CType(pnlFilters.FindControl("textbox4"), TextBox).Text <> "" Then
            filter += "AGENT like '%" & CType(pnlFilters.FindControl("textbox4"), TextBox).Text & "%' and "
        End If
        If CType(pnlFilters.FindControl("textbox5"), TextBox).Text <> "" Then
            filter += "BPCNUM_0 like '%" & CType(pnlFilters.FindControl("textbox5"), TextBox).Text & "%' and "
        End If
        If CType(pnlFilters.FindControl("textbox6"), TextBox).Text <> "" Then
            filter += "SAT_0 like '%" & CType(pnlFilters.FindControl("textbox6"), TextBox).Text & "%' and "
        End If
        If CType(pnlFilters.FindControl("textbox7"), TextBox).Text <> "" Then
            filter += "CRY_0 like '%" & CType(pnlFilters.FindControl("textbox7"), TextBox).Text & "%' and "
        End If
        loadItems(filter)
    End Sub

    Protected Sub loadItems(ByVal filter As String)
        'Dim sqlConnection As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection As New SqlConnection("Data Source=CSMWAR\CSMWAR;Initial Catalog=SageInterface;User ID=STAGE; pwd=STAGE21")
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        Dim dt As DataTable = New DataTable
        Dim column As Integer = 0
        Try
            cmd.CommandText = "select AREAMANAGER,AGENT,BPCNUM_0,BPCNAM_0,SAT_0,CRY_0,CODE,NAME,DATE,DURATION,NPARTECIPANTS,SEASON,GROUPID from STAGE_COURSESSUMMARY " &
                              "where " & filter & "1=1 group by AREAMANAGER,AGENT,BPCNUM_0,BPCNAM_0,SAT_0,CRY_0,CODE,NAME,DATE,DURATION,NPARTECIPANTS,SEASON,GROUPID"
            column = 13
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
            dt.Columns.Add("Gruppo")
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection
            sqlConnection.Open()
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
            sqlConnection.Close()
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection.State = ConnectionState.Open Then
                sqlConnection.Close()
            End If
        End Try
    End Sub

End Class
