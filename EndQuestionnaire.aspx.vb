Imports System.Data.SqlClient
Imports System.Data

Partial Class EndQuestionnaire
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection("Data Source=CSMWAR\CSMWAR;Initial Catalog=SageInterface;User ID=STAGE; pwd=STAGE21")
        Dim cmd As New SqlCommand
        Try
            cmd.CommandText = "insert into STAGE_QUESTIONNAIRESPARTICIPANTS (IDPARTICIPANTS,NQUESTIONNAIRE,GROUPID) values ('" & Session("participant") & "','" &
                               Session("questionnaire") & "','" & Session(" GROUPID") & "')"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            cmd.ExecuteNonQuery()
            sqlConnection1.Close()
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", ex.Message, True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
        End Try
    End Sub

End Class
