<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        GroupBox1 = New GroupBox()
        LogoutBT = New Button()
        GroupBox5 = New GroupBox()
        GroupBox2 = New GroupBox()
        Delegations = New PictureBox()
        DocumentsSettings = New PictureBox()
        PictureBox3 = New PictureBox()
        AddAttachmentsBT = New PictureBox()
        PictureBox5 = New PictureBox()
        NotifyIcon1 = New NotifyIcon(components)
        GroupBox5.SuspendLayout()
        GroupBox2.SuspendLayout()
        CType(Delegations, ComponentModel.ISupportInitialize).BeginInit()
        CType(DocumentsSettings, ComponentModel.ISupportInitialize).BeginInit()
        CType(PictureBox3, ComponentModel.ISupportInitialize).BeginInit()
        CType(AddAttachmentsBT, ComponentModel.ISupportInitialize).BeginInit()
        CType(PictureBox5, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        GroupBox1.Location = New Point(-8, 85)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.RightToLeft = RightToLeft.Yes
        GroupBox1.Size = New Size(1409, 679)
        GroupBox1.TabIndex = 0
        GroupBox1.TabStop = False
        ' 
        ' LogoutBT
        ' 
        LogoutBT.BackColor = Color.Transparent
        LogoutBT.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        LogoutBT.ForeColor = Color.White
        LogoutBT.Image = CType(resources.GetObject("LogoutBT.Image"), Image)
        LogoutBT.Location = New Point(7, 22)
        LogoutBT.Name = "LogoutBT"
        LogoutBT.Size = New Size(37, 41)
        LogoutBT.TabIndex = 1
        LogoutBT.UseVisualStyleBackColor = False
        ' 
        ' GroupBox5
        ' 
        GroupBox5.Controls.Add(LogoutBT)
        GroupBox5.Location = New Point(10, 6)
        GroupBox5.Name = "GroupBox5"
        GroupBox5.RightToLeft = RightToLeft.Yes
        GroupBox5.Size = New Size(1452, 89)
        GroupBox5.TabIndex = 1
        GroupBox5.TabStop = False
        ' 
        ' GroupBox2
        ' 
        GroupBox2.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        GroupBox2.BackColor = Color.Transparent
        GroupBox2.Controls.Add(Delegations)
        GroupBox2.Controls.Add(DocumentsSettings)
        GroupBox2.Controls.Add(PictureBox3)
        GroupBox2.Controls.Add(AddAttachmentsBT)
        GroupBox2.Controls.Add(PictureBox5)
        GroupBox2.Font = New Font("Segoe UI Black", 8.0F, FontStyle.Bold)
        GroupBox2.Location = New Point(1402, 95)
        GroupBox2.Name = "GroupBox2"
        GroupBox2.RightToLeft = RightToLeft.Yes
        GroupBox2.Size = New Size(60, 352)
        GroupBox2.TabIndex = 6
        GroupBox2.TabStop = False
        ' 
        ' Delegations
        ' 
        Delegations.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Delegations.BackColor = Color.Transparent
        Delegations.BackgroundImageLayout = ImageLayout.None
        Delegations.Image = CType(resources.GetObject("Delegations.Image"), Image)
        Delegations.ImageLocation = ""
        Delegations.InitialImage = CType(resources.GetObject("Delegations.InitialImage"), Image)
        Delegations.Location = New Point(13, 252)
        Delegations.Name = "Delegations"
        Delegations.Size = New Size(35, 34)
        Delegations.SizeMode = PictureBoxSizeMode.StretchImage
        Delegations.TabIndex = 8
        Delegations.TabStop = False
        ' 
        ' DocumentsSettings
        ' 
        DocumentsSettings.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        DocumentsSettings.BackColor = Color.Transparent
        DocumentsSettings.Image = CType(resources.GetObject("DocumentsSettings.Image"), Image)
        DocumentsSettings.Location = New Point(12, 183)
        DocumentsSettings.Name = "DocumentsSettings"
        DocumentsSettings.Size = New Size(35, 36)
        DocumentsSettings.SizeMode = PictureBoxSizeMode.StretchImage
        DocumentsSettings.TabIndex = 7
        DocumentsSettings.TabStop = False
        ' 
        ' PictureBox3
        ' 
        PictureBox3.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        PictureBox3.BackColor = Color.Transparent
        PictureBox3.BackgroundImageLayout = ImageLayout.None
        PictureBox3.Image = CType(resources.GetObject("PictureBox3.Image"), Image)
        PictureBox3.ImageLocation = ""
        PictureBox3.Location = New Point(13, 15)
        PictureBox3.Name = "PictureBox3"
        PictureBox3.Size = New Size(34, 33)
        PictureBox3.SizeMode = PictureBoxSizeMode.StretchImage
        PictureBox3.TabIndex = 7
        PictureBox3.TabStop = False
        ' 
        ' AddAttachmentsBT
        ' 
        AddAttachmentsBT.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        AddAttachmentsBT.BackColor = Color.Transparent
        AddAttachmentsBT.Image = CType(resources.GetObject("AddAttachmentsBT.Image"), Image)
        AddAttachmentsBT.Location = New Point(13, 141)
        AddAttachmentsBT.Name = "AddAttachmentsBT"
        AddAttachmentsBT.Size = New Size(35, 34)
        AddAttachmentsBT.SizeMode = PictureBoxSizeMode.StretchImage
        AddAttachmentsBT.TabIndex = 5
        AddAttachmentsBT.TabStop = False
        ' 
        ' PictureBox5
        ' 
        PictureBox5.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        PictureBox5.Image = CType(resources.GetObject("PictureBox5.Image"), Image)
        PictureBox5.Location = New Point(13, 54)
        PictureBox5.Name = "PictureBox5"
        PictureBox5.Size = New Size(34, 36)
        PictureBox5.TabIndex = 8
        PictureBox5.TabStop = False
        ' 
        ' NotifyIcon1
        ' 
        NotifyIcon1.BalloonTipText = "تم بنجاح"
        NotifyIcon1.BalloonTipTitle = "تم مسح اختيار العميل بنجاح"
        NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), Icon)
        NotifyIcon1.Text = "NotifyIcon1"
        NotifyIcon1.Visible = True
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = SystemColors.ButtonHighlight
        ClientSize = New Size(1470, 761)
        Controls.Add(GroupBox2)
        Controls.Add(GroupBox5)
        Controls.Add(GroupBox1)
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Name = "Form1"
        Text = "الـعــمــــــــلاء"
        WindowState = FormWindowState.Maximized
        GroupBox5.ResumeLayout(False)
        GroupBox2.ResumeLayout(False)
        CType(Delegations, ComponentModel.ISupportInitialize).EndInit()
        CType(DocumentsSettings, ComponentModel.ISupportInitialize).EndInit()
        CType(PictureBox3, ComponentModel.ISupportInitialize).EndInit()
        CType(AddAttachmentsBT, ComponentModel.ISupportInitialize).EndInit()
        CType(PictureBox5, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents LogoutBT As Button
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Delegations As PictureBox
    Friend WithEvents DocumentsSettings As PictureBox
    Friend WithEvents PictureBox3 As PictureBox
    Friend WithEvents AddAttachmentsBT As PictureBox
    Friend WithEvents PictureBox5 As PictureBox
    Friend WithEvents NotifyIcon1 As NotifyIcon

End Class
