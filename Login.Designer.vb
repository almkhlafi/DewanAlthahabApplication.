<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Login
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        GroupBox1 = New GroupBox()
        userNameTB = New TextBox()
        passwordTB = New TextBox()
        Label1 = New Label()
        Label2 = New Label()
        loginBT = New Button()
        GroupBox1.SuspendLayout()
        SuspendLayout()
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Controls.Add(loginBT)
        GroupBox1.Controls.Add(Label2)
        GroupBox1.Controls.Add(Label1)
        GroupBox1.Controls.Add(passwordTB)
        GroupBox1.Controls.Add(userNameTB)
        GroupBox1.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        GroupBox1.Location = New Point(12, 21)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.RightToLeft = RightToLeft.Yes
        GroupBox1.Size = New Size(546, 177)
        GroupBox1.TabIndex = 0
        GroupBox1.TabStop = False
        GroupBox1.Text = "تسجيل دخول"
        ' 
        ' userNameTB
        ' 
        userNameTB.Location = New Point(135, 49)
        userNameTB.Name = "userNameTB"
        userNameTB.Size = New Size(219, 23)
        userNameTB.TabIndex = 0
        ' 
        ' passwordTB
        ' 
        passwordTB.Location = New Point(135, 91)
        passwordTB.Name = "passwordTB"
        passwordTB.PasswordChar = "*"c
        passwordTB.Size = New Size(219, 23)
        passwordTB.TabIndex = 1
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(360, 57)
        Label1.Name = "Label1"
        Label1.Size = New Size(81, 15)
        Label1.TabIndex = 2
        Label1.Text = "اسم المستخدم "
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(364, 91)
        Label2.Name = "Label2"
        Label2.Size = New Size(62, 15)
        Label2.TabIndex = 3
        Label2.Text = "كلمة المرور"
        ' 
        ' loginBT
        ' 
        loginBT.Location = New Point(203, 130)
        loginBT.Name = "loginBT"
        loginBT.Size = New Size(101, 28)
        loginBT.TabIndex = 4
        loginBT.Text = "دخــــول"
        loginBT.UseVisualStyleBackColor = True
        ' 
        ' Login
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(585, 210)
        Controls.Add(GroupBox1)
        Name = "Login"
        Text = "Login"
        GroupBox1.ResumeLayout(False)
        GroupBox1.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents loginBT As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents passwordTB As TextBox
    Friend WithEvents userNameTB As TextBox
End Class
