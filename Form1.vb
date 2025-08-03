Public Class Form1

    Private dbConn As New DBconnections()

    ' Variables to track selected customer from search dialog
    Private currentSelectedCustomerId As Integer = 0
    Private currentSelectedCustomerName As String = ""

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Form initialization
        ' Setup NotifyIcon
        NotifyIcon1.Icon = Me.Icon
        NotifyIcon1.Visible = True
        NotifyIcon1.Text = "العملاء"


    End Sub
    Private Sub LogoutBT_Click(sender As Object, e As EventArgs) Handles LogoutBT.Click
        Dim result As DialogResult = MessageBox.Show("هل أنت متأكد من تسجيل الخروج؟", "تسجيل الخروج", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            ' Clear all session data including document access credentials
            Session.Clear()

            ' Close current form and show login form
            Me.Hide()

            Dim loginForm As New Login()
            If loginForm.ShowDialog() = DialogResult.OK Then
                ' User logged in successfully, show main form again
                Me.Show()
            Else
                ' User cancelled login, exit application
                Application.Exit()
            End If
        End If
    End Sub

    Private Sub AddAttachmentsBT_Click(sender As Object, e As EventArgs) Handles AddAttachmentsBT.Click
        ' Check if a customer is selected - if not, show search dialog first
        '  If currentSelectedCustomerId = 0 Then
        ' No customer selected - show customer search dialog
        '  Dim searchForm As New CustomerSearchForm

        ' If searchForm.ShowDialog = DialogResult.OK Then
        ' Customer was selected from search - store customer info
        '     currentSelectedCustomerId = searchForm.SelectedCustomerId
        '    currentSelectedCustomerName = searchForm.SelectedCustomerName
        '  End If
        '
        ' searchForm.Dispose()
        '  End If

        ' Open attachments management (regardless of customer selection)
        AttachmentsManagement.ShowDialog()
    End Sub

    Private Sub DocumentsSettings_Click(sender As Object, e As EventArgs) Handles DocumentsSettings.Click
        Dim docMgmt As New DocumentsManagement

        ' Check if a customer is selected
        If currentSelectedCustomerId > 0 Then
            ' Customer selected - show customer-specific documents
            docMgmt.SelectedCustomerId = currentSelectedCustomerId
            docMgmt.SelectedCustomerName = currentSelectedCustomerName
        Else
            ' No customer selected - show all documents for all customers
            docMgmt.SelectedCustomerId = 0 ' 0 means show all customers
            docMgmt.SelectedCustomerName = "جميع العملاء"
        End If

        docMgmt.ShowDialog()
        docMgmt.Dispose()
    End Sub

    Private Sub Delegations_Click_1(sender As Object, e As EventArgs) Handles Delegations.Click
        ' Check if a customer is selected
        If currentSelectedCustomerId > 0 Then
            ' Customer selected - create and show the delegations form
            Dim delegMgmt As New DelegationsManagement
            delegMgmt.CustomerIdToLoad = currentSelectedCustomerId

            delegMgmt.ShowDialog()
            delegMgmt.Dispose()
        Else
            ' No customer selected - show customer search dialog
            Dim searchForm As New CustomerSearchForm

            If searchForm.ShowDialog = DialogResult.OK Then
                ' Customer was selected from search - store and use selected customer
                currentSelectedCustomerId = searchForm.SelectedCustomerId
                currentSelectedCustomerName = searchForm.SelectedCustomerName

                Dim delegMgmt As New DelegationsManagement
                delegMgmt.CustomerIdToLoad = currentSelectedCustomerId

                delegMgmt.ShowDialog()
                delegMgmt.Dispose()
            End If

            searchForm.Dispose()
        End If
    End Sub

    Private Sub SearchForCustomer_Click(sender As Object, e As EventArgs) Handles PictureBox5.Click
        Dim searchForm As New CustomerSearchForm

        If searchForm.ShowDialog = DialogResult.OK Then
            ' Customer was selected - store customer info
            currentSelectedCustomerId = searchForm.SelectedCustomerId
            currentSelectedCustomerName = searchForm.SelectedCustomerName

            MessageBox.Show($"تم اختيار العميل: {currentSelectedCustomerName}", "تم الاختيار", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        searchForm.Dispose()
    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        ' Clear customer selection
        currentSelectedCustomerId = 0
        currentSelectedCustomerName = ""

        ' Show NotifyIcon balloon
        NotifyIcon1.BalloonTipTitle = "تم بنجاح"
        NotifyIcon1.BalloonTipText = "تم مسح اختيار العميل بنجاح!"
        NotifyIcon1.BalloonTipIcon = ToolTipIcon.Info
        NotifyIcon1.ShowBalloonTip(1000)

        ' Optional: Auto-closing popup window (like MessageBox)
        Dim toast As New AutoCloseMessageForm("تم مسح اختيار العميل بنجاح  ↻", 1000)
        toast.StartPosition = FormStartPosition.CenterScreen
        toast.Show()

    End Sub


End Class


