Imports Microsoft.Data.SqlClient

Public Class Login
    Private db As New DBconnections()

    Private Sub loginBT_Click(sender As Object, e As EventArgs) Handles loginBT.Click
        Dim username As String = userNameTB.Text.Trim()
        Dim password As String = passwordTB.Text.Trim()

        If String.IsNullOrWhiteSpace(username) OrElse String.IsNullOrWhiteSpace(password) Then
            MessageBox.Show("يرجى إدخال اسم المستخدم وكلمة المرور", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            userNameTB.Focus()
            Return
        End If

        Using conn As Microsoft.Data.SqlClient.SqlConnection = db.GetConnection()
            Using cmd As New Microsoft.Data.SqlClient.SqlCommand("SELECT Id, Name FROM Users WHERE username = @username AND password = @password", conn)
                cmd.Parameters.AddWithValue("@username", username)
                cmd.Parameters.AddWithValue("@password", password)

                Try
                    conn.Open()
                    Using reader As Microsoft.Data.SqlClient.SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Session.LoggedInUserId = reader.GetInt32(0)
                            Session.LoggedInUsername = username
                            Session.LoggedInName = reader.GetString(1)

                            System.Diagnostics.Debug.WriteLine("Database login successful")
                            Me.DialogResult = DialogResult.OK
                            Me.Close()
                        Else
                            MessageBox.Show("اسم المستخدم أو كلمة المرور غير صحيحة", "خطأ في تسجيل الدخول", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            passwordTB.Clear()
                            passwordTB.Focus()
                        End If
                    End Using
                Catch ex As Exception
                    MessageBox.Show("حدث خطأ أثناء الاتصال بقاعدة البيانات: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End Using
    End Sub

    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        userNameTB.Focus()
    End Sub

    Private Sub passwordTB_KeyPress(sender As Object, e As KeyPressEventArgs) Handles passwordTB.KeyPress
        If e.KeyChar = Chr(13) Then
            loginBT_Click(sender, Nothing)
            e.Handled = True
        End If
    End Sub
End Class
