Imports System.Data.SqlClient
Imports System.Data
Imports System.Text.RegularExpressions

Partial Class updateAnagraphic
    Inherits System.Web.UI.Page
    Dim connString As String = "Data Source=CSMWAR\CSMWAR;Initial Catalog=SageInterface;User ID=STAGE; pwd=STAGE21"

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        btnConfirm.Text = labelTranslated("confirm", Session("language"))
        loadFields()
        loadItem()
    End Sub

    Protected Sub btnConfirm_Click(sender As Object, e As System.EventArgs)
        If CType(pnlFields1.FindControl("TextBox1"), TextBox).Text = "" Or CType(pnlFields1.FindControl("TextBox2"), TextBox).Text = "" Or
           CType(pnlFields1.FindControl("DropDownList1"), DropDownList).Text = 0 Or CType(pnlFields2.FindControl("DropDownList3"), DropDownList).Text = "" Or
           Regex.IsMatch(CType(pnlFields2.FindControl("TextBox11"), TextBox).Text, "^[a-zA-Z'.]{1,40}$") Or
           Regex.IsMatch(CType(pnlFields2.FindControl("TextBox12"), TextBox).Text, "^[a-zA-Z'.]{1,40}$") Or
           Regex.IsMatch(CType(pnlFields2.FindControl("TextBox13"), TextBox).Text, "^[a-zA-Z'.]{1,40}$") Then
            If CType(pnlFields1.FindControl("TextBox1"), TextBox).Text = "" Then
                CType(pnlFields1.FindControl("TextBox1"), TextBox).BorderColor = Drawing.Color.Red
            Else
                CType(pnlFields1.FindControl("TextBox1"), TextBox).BorderColor = Drawing.Color.LightGray
            End If
            If CType(pnlFields1.FindControl("TextBox2"), TextBox).Text = "" Then
                CType(pnlFields1.FindControl("TextBox2"), TextBox).BorderColor = Drawing.Color.Red
            Else
                CType(pnlFields1.FindControl("TextBox2"), TextBox).BorderColor = Drawing.Color.LightGray
            End If
            If Regex.IsMatch(CType(pnlFields2.FindControl("TextBox11"), TextBox).Text, "^[a-zA-Z'.]{1,40}$") Then
                CType(pnlFields2.FindControl("TextBox11"), TextBox).Text = "* just numbers are allowed"
                CType(pnlFields2.FindControl("TextBox11"), TextBox).ForeColor = Drawing.Color.Red
                CType(pnlFields2.FindControl("TextBox11"), TextBox).BorderColor = Drawing.Color.Red
            Else
                CType(pnlFields2.FindControl("TextBox11"), TextBox).ForeColor = Drawing.Color.Black
                CType(pnlFields2.FindControl("TextBox11"), TextBox).BorderColor = Drawing.Color.LightGray
            End If
            If Regex.IsMatch(CType(pnlFields2.FindControl("TextBox12"), TextBox).Text, "^[a-zA-Z'.]{1,40}$") Then
                CType(pnlFields2.FindControl("TextBox12"), TextBox).Text = "* just numbers are allowed"
                CType(pnlFields2.FindControl("TextBox12"), TextBox).ForeColor = Drawing.Color.Red
                CType(pnlFields2.FindControl("TextBox12"), TextBox).BorderColor = Drawing.Color.Red
            Else
                CType(pnlFields2.FindControl("TextBox12"), TextBox).ForeColor = Drawing.Color.Black
                CType(pnlFields2.FindControl("TextBox12"), TextBox).BorderColor = Drawing.Color.LightGray
            End If
            If Regex.IsMatch(CType(pnlFields2.FindControl("TextBox13"), TextBox).Text, "^[a-zA-Z'.]{1,40}$") Then
                CType(pnlFields2.FindControl("TextBox13"), TextBox).Text = "* just numbers are allowed"
                CType(pnlFields2.FindControl("TextBox13"), TextBox).ForeColor = Drawing.Color.Red
                CType(pnlFields2.FindControl("TextBox13"), TextBox).BorderColor = Drawing.Color.Red
            Else
                CType(pnlFields2.FindControl("TextBox13"), TextBox).ForeColor = Drawing.Color.Black
                CType(pnlFields2.FindControl("TextBox13"), TextBox).BorderColor = Drawing.Color.LightGray
            End If
            If CType(pnlFields1.FindControl("DropDownList1"), DropDownList).Text = 0 Then
                CType(pnlFields1.FindControl("DropDownList1"), DropDownList).BorderColor = Drawing.Color.Red
            Else
                CType(pnlFields1.FindControl("DropDownList1"), DropDownList).BorderColor = Drawing.Color.LightGray
            End If
            If CType(pnlFields2.FindControl("DropDownList3"), DropDownList).Text = "" Then
                CType(pnlFields2.FindControl("DropDownList3"), DropDownList).BorderColor = Drawing.Color.Red
            Else
                CType(pnlFields1.FindControl("DropDownList3"), DropDownList).BorderColor = Drawing.Color.LightGray
            End If
        Else
            update()
            Response.Redirect("Questionnaire.aspx")
        End If
    End Sub

    Protected Sub loadFields()
        Dim pnl As Panel
        Dim idLanguage As Integer = Session("language")
        Dim fieldsNames(14) As String
        fieldsNames = New String() {labelTranslated("name", idLanguage), labelTranslated("lastname", idLanguage), labelTranslated("birthYear", idLanguage), labelTranslated("address", idLanguage),
                                    labelTranslated("bpcnum_0", idLanguage), labelTranslated("city", idLanguage), labelTranslated("county", idLanguage), labelTranslated("state", idLanguage),
                                    labelTranslated("language", idLanguage), labelTranslated("email", idLanguage), labelTranslated("phone", idLanguage), labelTranslated("fax", idLanguage),
                                    labelTranslated("mobilePhone", idLanguage), labelTranslated("customerType", idLanguage)}
        For i As Integer = 0 To fieldsNames.Length - 1
            Dim label As New Label
            Dim text As New TextBox
            If i < 7 Then
                pnl = CType(form1.FindControl("pnlFields1"), Panel)
            Else
                pnl = CType(form1.FindControl("pnlFields2"), Panel)
            End If
            label.Text = fieldsNames(i)
            label.ID = "Label" & i + 1
            label.Font.Bold = True
            pnl.Controls.Add(label)
            pnl.Controls.Add(New LiteralControl("<br />"))
            If i = 2 Then
                Dim drop As New DropDownList
                drop.ID = "DropDownList1"
                drop.CssClass = "form-control"
                drop.EnableViewState = True
                pnl.Controls.Add(drop)
                loadYears()
            ElseIf i = 8 Then
                Dim drop As New DropDownList
                drop.ID = "DropDownList2"
                drop.CssClass = "form-control"
                drop.EnableViewState = True
                pnl.Controls.Add(drop)
                loadLanguages()
            ElseIf i = 13 Then
                Dim drop As New DropDownList
                drop.ID = "DropDownList3"
                drop.CssClass = "form-control"
                drop.EnableViewState = True
                drop.Items.Add("")
                drop.Items.Add(labelTranslated("applicator", idLanguage))
                drop.Items.Add(labelTranslated("retailer", idLanguage))
                drop.Items.Add(labelTranslated("other", idLanguage))
                pnl.Controls.Add(drop)
            Else
                text.ID = "TextBox" & i + 1
                text.EnableViewState = True
                text.Attributes.Add("onkeydown", "return (event.keyCode!=13);")
                text.CssClass = "form-control"
                pnl.Controls.Add(text)
            End If
            pnl.Controls.Add(New LiteralControl("<br />"))
        Next
    End Sub

    Protected Sub loadItem()
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        Dim pnl As Panel
        Dim TextBox As TextBox
        Dim DropDownList As DropDownList
        Try
            cmd.CommandText = "select NAME,LASTNAME,BIRTHYEAR,ADDRESS,ZIPCODE,CITY,COUNTY,STATE,IDLANGUAGE,EMAIL,PHONE,FAX,MOBILEPHONE,CUSTOMERTYPE from STAGE_PARTICIPANTS where ID='" & Session("participant") & "'"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            While reader.Read
                If CType(pnlFields1.FindControl("TextBox1"), TextBox) IsNot Nothing Then
                    For i As Integer = 0 To 13
                        If i < 7 Then
                            pnl = CType(form1.FindControl("pnlFields1"), Panel)
                        Else
                            pnl = CType(form1.FindControl("pnlFields2"), Panel)
                        End If
                        If i = 2 Then
                            DropDownList = CType(pnl.FindControl("DropDownList1"), DropDownList)
                            DropDownList.Text = IIf(IsDBNull(reader(i)), vbNullString, reader(i))
                        ElseIf i = 8 Then
                            DropDownList = CType(pnl.FindControl("DropDownList2"), DropDownList)
                            DropDownList.Text = IIf(IsDBNull(reader(i)), vbNullString, descLanguage(reader(i)))
                        ElseIf i = 13 Then
                            DropDownList = CType(pnl.FindControl("DropDownList3"), DropDownList)
                            DropDownList.Text = IIf(IsDBNull(reader(i)), vbNullString, reader(i))
                        Else
                            TextBox = CType(pnl.FindControl("TextBox" & i + 1), TextBox)
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

    Protected Sub loadYears()
        Dim DropDownList As DropDownList = CType(pnlFields2.FindControl("DropDownList1"), DropDownList)
        DropDownList.Items.Clear()
        DropDownList.Items.Add(0)
        For i As Integer = 1940 To Year(Date.Now) - 15
            DropDownList.Items.Add(i)
        Next
    End Sub

    Protected Sub loadLanguages()
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection("Data Source=CSMWAR\CSMWAR;Initial Catalog=StageInterface;User ID=STAGE; pwd=STAGE21")
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        Dim DropDownList As DropDownList = CType(pnlFields2.FindControl("DropDownList2"), DropDownList)
        Try
            cmd.CommandText = "select DESCLANGUAGE from STAGE_LANGUAGES"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            DropDownList.Items.Clear()
            While reader.Read
                DropDownList.Items.Add(reader(0))
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

    Protected Sub update()
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Try
            Session("language") = idLanguage(CType(pnlFields2.FindControl("DropDownList2"), DropDownList).Text)
            cmd.CommandText = "update STAGE_PARTICIPANTS set NAME='" & Replace(CType(pnlFields1.FindControl("TextBox1"), TextBox).Text, "'", "''") & "',LASTNAME='" & Replace(CType(pnlFields1.FindControl("TextBox2"), TextBox).Text, "'", "''") &
                              "',BIRTHYEAR='" & CType(pnlFields1.FindControl("DropDownList1"), DropDownList).Text & "',ADDRESS='" & Replace(CType(pnlFields1.FindControl("TextBox4"), TextBox).Text, "'", "''") &
                              "',ZIPCODE='" & Replace(CType(pnlFields1.FindControl("TextBox5"), TextBox).Text, "'", "''") & "',CITY='" & Replace(CType(pnlFields1.FindControl("TextBox6"), TextBox).Text, "'", "''") &
                              "',COUNTY='" & Replace(CType(pnlFields1.FindControl("TextBox7"), TextBox).Text, "'", "''") & "',STATE='" & Replace(CType(pnlFields2.FindControl("TextBox8"), TextBox).Text, "'", "''") &
                              "',IDLANGUAGE='" & Session("language") & "',EMAIL='" & Replace(CType(pnlFields2.FindControl("TextBox10"), TextBox).Text, "'", "''") &
                              "',PHONE='" & Replace(CType(pnlFields2.FindControl("TextBox11"), TextBox).Text, "'", "''") & "',FAX='" & Replace(CType(pnlFields2.FindControl("TextBox12"), TextBox).Text, "'", "''") &
                              "',MOBILEPHONE='" & Replace(CType(pnlFields2.FindControl("TextBox13"), TextBox).Text, "'", "''") & "',CUSTOMERTYPE='" & CType(pnlFields2.FindControl("DropDownList3"), DropDownList).Text &
                              "' where ID='" & Session("participant") & "'"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            cmd.ExecuteNonQuery()
            sqlConnection1.Close()
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
        End Try
    End Sub

    Protected Function idLanguage(ByVal descLanguage As String) As Integer
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        idLanguage = -1
        Try
            cmd.CommandText = "select ID from STAGE_LANGUAGES where DESCLANGUAGE='" & descLanguage & "'"
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

    Protected Function labelTranslated(ByVal label As String, ByVal idLanguage As Integer) As String
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        labelTranslated = ""
        Try
            cmd.CommandText = "select TRANSLATION from STAGE_LABELS where LABEL='" & label & "' and IDLANGUAGE='" & idLanguage & "'"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            While reader.Read
                labelTranslated = reader(0)
            End While
            sqlConnection1.Close()
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
        End Try
        Return labelTranslated
    End Function

End Class
