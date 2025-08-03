Public Class AutoCloseMessageForm
    Inherits Form

    Private WithEvents autoCloseTimer As Timer
    Private messageLabel As Label

    Public Sub New(message As String, timeoutMilliseconds As Integer)
        InitializeComponent()
        SetupForm(message, timeoutMilliseconds)
    End Sub

    Private Sub InitializeComponent()
        messageLabel = New Label()
        SuspendLayout()
        ' 
        ' messageLabel
        ' 
        messageLabel.BackColor = Color.Transparent
        messageLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        messageLabel.ForeColor = Color.DarkGreen
        messageLabel.Location = New Point(15, 20)
        messageLabel.Name = "messageLabel"
        messageLabel.Size = New Size(320, 60)
        messageLabel.TabIndex = 0
        messageLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' AutoCloseMessageForm
        ' 
        BackColor = Color.LightGreen
        ClientSize = New Size(334, 81)
        Controls.Add(messageLabel)
        FormBorderStyle = FormBorderStyle.None
        MaximizeBox = False
        MinimizeBox = False
        Name = "AutoCloseMessageForm"
        RightToLeft = RightToLeft.Yes
        ShowInTaskbar = False
        StartPosition = FormStartPosition.CenterScreen
        Text = "إشعار"
        TopMost = True
        ResumeLayout(False)
    End Sub

    Private Sub SetupForm(message As String, timeoutMilliseconds As Integer)
        ' Set message text
        messageLabel.Text = message

        ' Setup auto-close timer
        autoCloseTimer = New Timer()
        autoCloseTimer.Interval = timeoutMilliseconds
        autoCloseTimer.Start()
    End Sub

    Private Sub autoCloseTimer_Tick(sender As Object, e As EventArgs) Handles autoCloseTimer.Tick
        ' Stop timer and close form
        autoCloseTimer.Stop()
        autoCloseTimer.Dispose()
        Me.Close()
    End Sub

    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        ' Clean up timer if form is closed manually
        If autoCloseTimer IsNot Nothing Then
            autoCloseTimer.Stop()
            autoCloseTimer.Dispose()
        End If
        MyBase.OnFormClosing(e)
    End Sub
End Class