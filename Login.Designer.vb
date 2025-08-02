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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Login))
        GroupBox1 = New GroupBox()
        loginBT = New Button()
        Label2 = New Label()
        Label1 = New Label()
        passwordTB = New TextBox()
        userNameTB = New TextBox()
        PictureBox1 = New PictureBox()
        GroupBox1.SuspendLayout()
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
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
        GroupBox1.Location = New Point(15, 40)
        GroupBox1.Margin = New Padding(8)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.RightToLeft = RightToLeft.Yes
        GroupBox1.Size = New Size(377, 184)
        GroupBox1.TabIndex = 0
        GroupBox1.TabStop = False
        ' 
        ' loginBT
        ' 
        loginBT.Image = CType(resources.GetObject("loginBT.Image"), Image)
        loginBT.ImageAlign = ContentAlignment.MiddleLeft
        loginBT.Location = New Point(143, 143)
        loginBT.Name = "loginBT"
        loginBT.Size = New Size(58, 35)
        loginBT.TabIndex = 4
        loginBT.TextAlign = ContentAlignment.MiddleRight
        loginBT.UseVisualStyleBackColor = True
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(267, 99)
        Label2.Name = "Label2"
        Label2.Size = New Size(62, 15)
        Label2.TabIndex = 3
        Label2.Text = "كلمة المرور"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(263, 65)
        Label1.Name = "Label1"
        Label1.Size = New Size(81, 15)
        Label1.TabIndex = 2
        Label1.Text = "اسم المستخدم "
        ' 
        ' passwordTB
        ' 
        passwordTB.Location = New Point(69, 107)
        passwordTB.Name = "passwordTB"
        passwordTB.PasswordChar = "*"c
        passwordTB.Size = New Size(188, 23)
        passwordTB.TabIndex = 1
        ' 
        ' userNameTB
        ' 
        userNameTB.Location = New Point(69, 65)
        userNameTB.Name = "userNameTB"
        userNameTB.Size = New Size(188, 23)
        userNameTB.TabIndex = 0
        ' 
        ' PictureBox1
        ' 
        PictureBox1.BackgroundImage = CType(resources.GetObject("PictureBox1.BackgroundImage"), Image)
        PictureBox1.BackgroundImageLayout = ImageLayout.Stretch
        PictureBox1.Cursor = Cursors.IBeam
        PictureBox1.Location = New Point(158, 18)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(69, 70)
        PictureBox1.SizeMode = PictureBoxSizeMode.CenterImage
        PictureBox1.TabIndex = 6
        PictureBox1.TabStop = False
        ' 
        ' Login
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(415, 231)
        Controls.Add(PictureBox1)
        Controls.Add(GroupBox1)
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        MaximizeBox = False
        Name = "Login"
        StartPosition = FormStartPosition.CenterScreen
        Text = "تسجبل دخول"
        GroupBox1.ResumeLayout(False)
        GroupBox1.PerformLayout()
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents loginBT As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents passwordTB As TextBox
    Friend WithEvents userNameTB As TextBox
    Friend WithEvents PictureBox1 As PictureBox
End Class
