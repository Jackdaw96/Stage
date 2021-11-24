Imports System.Data.SqlClient
Imports System.Data

Partial Class Questionnaire
    Inherits System.Web.UI.Page
    Dim connString As String = "Data Source=CSMWAR\CSMWAR;Initial Catalog=SageInterface;User ID=STAGE; pwd=STAGE21"

    Protected Property counter() As Integer
        Get
            Return CInt(Session("counter"))
        End Get
        Set(value As Integer)
            Session("counter") = value
        End Set
    End Property

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            btnBack.Text = labelTranslated("back", Session("language"))
            btnNext.Text = labelTranslated("next", Session("language"))
            counter = lastQuestion(Session("participant"), Session("questionnaire")) + 1
        End If
        loadQuestion(Session("questionnaire"), counter, Session("language"))
    End Sub

    Protected Sub btnBack_Click(sender As Object, e As System.EventArgs)
        If counter > 1 Then
            counter = previusQuestion(Session("participant"), Session("questionnaire"), counter)
            pnlAnswers.Controls.Clear()
            loadQuestion(Session("questionnaire"), counter, Session("language"))
            loadAnswer(Session("participant"), Session("questionnaire"), counter, Session("language"))
        End If
    End Sub

    Protected Sub btnNext_Click(sender As Object, e As System.EventArgs)
        If counter < nQuestions(Session("questionnaire"), Session("language")) Then
            If saveAnswer(Session("participant"), Session("questionnaire"), counter, Session("language")) Then
                counter += 1
                pnlAnswers.Controls.Clear()
                loadQuestion(Session("questionnaire"), counter, Session("language"))
            End If
        ElseIf counter = nQuestions(Session("questionnaire"), Session("language")) Then
            saveAnswer(Session("participant"), Session("questionnaire"), counter, Session("language"))
            Response.Redirect("EndQuestionnaire.aspx")
        End If
    End Sub

    Public Sub loadQuestion(ByVal nQuestionnaire As Integer, ByVal nQuestion As Integer, ByVal idLanguage As Integer)
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        While checkExceptions(nQuestionnaire, nQuestion, Session("language"))
            counter += 1
            nQuestion = counter
        End While
        Try
            cmd.CommandText = "select ID,QUESTION,QUESTIONTYPE,NQUESTION from STAGE_QUESTIONS where NQUESTION='" & nQuestion & "' and NQUESTIONNAIRE='" & nQuestionnaire & "' " &
                              "and IDLANGUAGE='" & idLanguage & "' and GROUPID='" & Session("groupid") & "'"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            While reader.Read
                lblQuestion.Text = reader(3) & ") " & reader(1)
                lblQuestion.Font.Bold = True
                loadAnswers(nQuestionnaire, reader(0), reader(2), idLanguage)
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

    Public Sub loadAnswers(ByVal nQuestionnaire As Integer, ByVal idQuestion As Integer, ByVal questionType As String, ByVal idLanguage As Integer)
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        Try
            cmd.CommandText = "select ANSWER from STAGE_ANSWERS where NQUESTIONNAIRE='" & nQuestionnaire & "' and IDLANGUAGE='" & idLanguage & "' and IDQUESTION='" &
                              idQuestion & "' and GROUPID='" & Session("groupid") & "' order by NANSWER"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            Select Case questionType
                Case "check"
                    Dim checkList As New CheckBoxList
                    checkList.ID = "CheckBoxList1"
                    While reader.Read()
                        Dim check As New CheckBox
                        check.Text = reader(0)
                        check.Checked = False
                        checkList.Items.Add(check.Text)
                    End While
                    pnlAnswers.Controls.Add(checkList)
                Case "radio"
                    Dim radioList As New RadioButtonList
                    radioList.ID = "RadioButtonList1"
                    While reader.Read
                        Dim radio As New RadioButton
                        radio.Text = reader(0)
                        radioList.Items.Add(radio.Text)
                    End While
                    pnlAnswers.Controls.Add(radioList)
                Case "text"
                    Dim text As New TextBox
                    text.ID = "TextBox1"
                    pnlAnswers.Controls.Add(text)
            End Select
            sqlConnection1.Close()
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
        End Try
    End Sub

    Protected Sub loadAnswer(ByVal idParticipants As Integer, ByVal nQuestionnaire As Integer, ByVal nQuestion As Integer, ByVal idLanguage As Integer)
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        Try
            cmd.CommandText = "select ANSWER from STAGE_ANSWERSPARTICIPANTS where IDPARTICIPANTS='" & idParticipants & "' and NQUESTIONNAIRE='" & nQuestionnaire & "' and NQUESTION='" & nQuestion & "' " &
                              " and GROUPID='" & Session("groupid") & "'"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            While reader.Read
                Select Case questionType(nQuestionnaire, nQuestion, idLanguage)
                    Case "check"
                        Dim checklist As CheckBoxList = CType(pnlAnswers.FindControl("CheckBoxList1"), CheckBoxList)
                        checklist.Items(reader(0) - 1).Selected = True
                    Case "radio"
                        Dim radiolist As RadioButtonList = CType(pnlAnswers.FindControl("RadioButtonList1"), RadioButtonList)
                        radiolist.Items(reader(0) - 1).Selected = True
                End Select
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

    Protected Function saveAnswer(ByVal idParticipants As Integer, ByVal nQuestionnaire As Integer, ByVal nQuestion As Integer, ByVal idLanguage As Integer) As Boolean
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim checkChecked As Integer = 0
        Dim answer As Integer = 0
        If nQuestion <= lastQuestion(idParticipants, nQuestionnaire) Then
            deleteAnswer(idParticipants, nQuestionnaire, nQuestion)
        End If
        saveAnswer = False
        Try
            Select Case questionType(nQuestionnaire, nQuestion, idLanguage)
                Case "check"
                    Dim max As Integer = maxAnswers(nQuestionnaire, nQuestion, idLanguage)
                    Dim checklist As CheckBoxList = CType(pnlAnswers.FindControl("CheckBoxList1"), CheckBoxList)
                    For Each checkBox As ListItem In checklist.Items
                        If checkBox.Selected Then
                            checkChecked += 1
                        End If
                    Next
                    If (checkChecked <= max And checkChecked > 0) Or max = 0 Then
                        Dim idCheck As Integer = 1
                        For Each checkBox As ListItem In checklist.Items
                            If checkBox.Selected Then
                                cmd.CommandText = "insert into STAGE_ANSWERSPARTICIPANTS (IDPARTICIPANTS,NQUESTIONNAIRE,NQUESTION,ANSWER,GROUPID) values ('" & idParticipants & "','" & nQuestionnaire &
                                                  "','" & nQuestion & "','" & idCheck & "','" & Session("groupid") & "')"
                                cmd.CommandType = CommandType.Text
                                cmd.Connection = sqlConnection1
                                sqlConnection1.Open()
                                cmd.ExecuteNonQuery()
                                sqlConnection1.Close()
                                answer = idCheck
                            End If
                            idCheck += 1
                        Next
                        saveAnswer = True
                    Else
                        ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('No selected answers or exceeded max answers (" & max & ")');", True)
                    End If
                Case "radio"
                    Dim radiolist As RadioButtonList = CType(pnlAnswers.FindControl("RadioButtonList1"), RadioButtonList)
                    If radiolist.SelectedIndex + 1 > 0 Then
                        cmd.CommandText = "insert into STAGE_ANSWERSPARTICIPANTS (IDPARTICIPANTS,NQUESTIONNAIRE,NQUESTION,ANSWER,GROUPID) values ('" & idParticipants & "','" & nQuestionnaire &
                                          "','" & nQuestion & "','" & radiolist.SelectedIndex + 1 & "','" & Session("groupid") & "')"
                        cmd.CommandType = CommandType.Text
                        cmd.Connection = sqlConnection1
                        sqlConnection1.Open()
                        cmd.ExecuteNonQuery()
                        sqlConnection1.Close()
                        answer = radiolist.SelectedIndex + 1
                        saveAnswer = True
                    Else
                        ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('No selected answers');", True)
                    End If
                Case "text"
                    Dim text As TextBox = CType(pnlAnswers.FindControl("TextBox1"), TextBox)
                    cmd.CommandText = "insert into STAGE_ANSWERSPARTICIPANTS (IDPARTICIPANTS,NQUESTIONNAIRE,NQUESTION,ANSWER,GROUPID) values ('" & idParticipants & "','" & nQuestionnaire &
                                          "','" & nQuestion & "','" & text.Text & "','" & Session("groupid") & "')"
                    cmd.CommandType = CommandType.Text
                    cmd.Connection = sqlConnection1
                    sqlConnection1.Open()
                    cmd.ExecuteNonQuery()
                    sqlConnection1.Close()
                    saveAnswer = True
            End Select
            If nQuestionnaire = 1 And answer = 2 And (nQuestion = 4 Or nQuestion = 6 Or nQuestion = 11 Or nQuestion = 13 Or nQuestion = 15) Then
                counter += 1
                deleteAnswer(idParticipants, nQuestionnaire, counter)
            ElseIf nQuestionnaire = 2 And answer = 2 And (nQuestion = 21 Or nQuestion = 23) Then
                counter += 1
                deleteAnswer(idParticipants, nQuestionnaire, counter)
            End If
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
        End Try
        Return saveAnswer
    End Function

    Protected Sub deleteAnswer(ByVal idParticipants As Integer, ByVal nQuestionnaire As Integer, ByVal nQuestion As Integer)
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Try
            cmd.CommandText = "delete from STAGE_ANSWERSPARTICIPANTS where IDPARTICIPANTS='" & idParticipants & "' and NQUESTIONNAIRE='" & nQuestionnaire & "' and NQUESTION='" & nQuestion & "' and " &
                              "GROUPID='" & Session("groupid") & "'"
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

    Protected Function questionType(ByVal nQuestionnaire As Integer, ByVal nQuestion As Integer, ByVal idLanguage As Integer) As String
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        questionType = ""
        Try
            cmd.CommandText = "select QUESTIONTYPE from STAGE_QUESTIONS where NQUESTIONNAIRE='" & nQuestionnaire & "' and NQUESTION='" & nQuestion & "' and IDLANGUAGE='" & idLanguage & "' and " &
                              "GROUPID='" & Session("groupid") & "'"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            While reader.Read
                questionType = reader(0)
            End While
            sqlConnection1.Close()
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
        End Try
        Return questionType
    End Function

    Protected Function nQuestions(ByVal nQuestionnaire As Integer, ByVal idLanguage As Integer) As Integer
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        nQuestions = 0
        Try
            cmd.CommandText = "select COUNT(*) from STAGE_QUESTIONS where NQUESTIONNAIRE='" & nQuestionnaire & "' and IDLANGUAGE='" & idLanguage & "' and GROUPID='" & Session("groupid") & "'"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            While reader.Read
                nQuestions = IIf(IsDBNull(reader(0)), 0, reader(0))
            End While
            sqlConnection1.Close()
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
        End Try
        Return nQuestions
    End Function

    Protected Function maxAnswers(ByVal nQuestionnaire As Integer, ByVal nQuestion As Integer, ByVal idLanguage As Integer) As Integer
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        maxAnswers = 0
        Try
            cmd.CommandText = "select MAXANSWERS from STAGE_QUESTIONS where NQUESTIONNAIRE='" & nQuestionnaire & "' and NQUESTION='" & nQuestion & "' and IDLANGUAGE='" & idLanguage & "' and " &
                              "GROUPID='" & Session("groupid") & "'"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            While reader.Read
                maxAnswers = IIf(IsDBNull(reader(0)), 0, reader(0))
            End While
            sqlConnection1.Close()
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
        End Try
        Return maxAnswers
    End Function

    Protected Function checkExceptions(ByVal nQuestionnaire As Integer, ByVal nQuestion As Integer, ByVal idLanguage As Integer) As Boolean
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        Dim exceptions As String = ""
        checkExceptions = False
        Try
            cmd.CommandText = "select EXCEPTIONS from STAGE_QUESTIONS where NQUESTIONNAIRE='" & nQuestionnaire & "' and NQUESTION='" & nQuestion & "' and IDLANGUAGE='" & idLanguage & "' and " &
                              "GROUPID='" & Session("groupid") & "'"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            While reader.Read
                exceptions = IIf(IsDBNull(reader(0)), "", reader(0))
            End While
            sqlConnection1.Close()
            checkExceptions = IIf(exceptions.Contains(Session("courseCode")), True, False)
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
        End Try
        Return checkExceptions
    End Function

    Protected Function lastQuestion(ByVal idParticipants As Integer, ByVal nQuestionnaire As Integer) As Integer
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        lastQuestion = 0
        Try
            cmd.CommandText = "select top(1) NQUESTION from STAGE_ANSWERSPARTICIPANTS where NQUESTIONNAIRE='" & nQuestionnaire & "' and IDPARTICIPANTS='" & idParticipants & "' and " &
                              "GROUPID='" & Session("groupid") & "' order by NQUESTION desc"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            While reader.Read
                lastQuestion = IIf(IsDBNull(reader(0)), 0, reader(0))
            End While
            sqlConnection1.Close()
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
        End Try
        Return lastQuestion
    End Function

    Protected Function previusQuestion(ByVal idParticipants As Integer, ByVal nQuestionnaire As Integer, ByVal nQuestion As Integer) As Integer
        'Dim sqlConnection1 As New SqlConnection("Data Source=CSMDBS;Initial Catalog=Stage;User ID=sa; pwd=ttprstf")
        Dim sqlConnection1 As New SqlConnection(connString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        Dim questions(30) As Integer
        Dim i As Integer = 0
        previusQuestion = 0
        Try
            cmd.CommandText = "select distinct NQUESTION from STAGE_ANSWERSPARTICIPANTS where NQUESTIONNAIRE='" & nQuestionnaire & "' and IDPARTICIPANTS='" & idParticipants & "' and GROUPID='" & Session("groupid") & "'"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection1
            sqlConnection1.Open()
            reader = cmd.ExecuteReader()
            While reader.Read
                questions(i) = reader(0)
                i += 1
            End While
            sqlConnection1.Close()
            i = -1
            While i < 0
                nQuestion -= 1
                i = Array.FindIndex(questions, Function(x) (x = nQuestion))
            End While
            previusQuestion = questions(i)
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('" & ex.Message & "');", True)
        Finally
            If sqlConnection1.State = ConnectionState.Open Then
                sqlConnection1.Close()
            End If
        End Try
        Return previusQuestion
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
