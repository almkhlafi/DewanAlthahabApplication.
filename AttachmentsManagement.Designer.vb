<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AttachmentsManagement
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AttachmentsManagement))
        AttachmetTableDGV = New DataGridView()
        GroupBox1 = New GroupBox()
        GroupBox2 = New GroupBox()
        AddAttachmentBT = New Button()
        RemoveAttachmentBT = New Button()
        AttachmentTB = New RichTextBox()
        CType(AttachmetTableDGV, ComponentModel.ISupportInitialize).BeginInit()
        GroupBox1.SuspendLayout()
        GroupBox2.SuspendLayout()
        SuspendLayout()
        ' 
        ' AttachmetTableDGV
        ' 
        AttachmetTableDGV.AllowUserToAddRows = False
        AttachmetTableDGV.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        AttachmetTableDGV.BackgroundColor = SystemColors.ControlLightLight
        AttachmetTableDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        AttachmetTableDGV.Location = New Point(15, 25)
        AttachmetTableDGV.Name = "AttachmetTableDGV"
        AttachmetTableDGV.RowHeadersVisible = False
        AttachmetTableDGV.Size = New Size(571, 297)
        AttachmetTableDGV.TabIndex = 0
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Anchor = AnchorStyles.None
        GroupBox1.Controls.Add(GroupBox2)
        GroupBox1.Controls.Add(AttachmetTableDGV)
        GroupBox1.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        GroupBox1.Location = New Point(3, 10)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.RightToLeft = RightToLeft.Yes
        GroupBox1.Size = New Size(599, 460)
        GroupBox1.TabIndex = 1
        GroupBox1.TabStop = False
        GroupBox1.Text = " "
        ' 
        ' GroupBox2
        ' 
        GroupBox2.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        GroupBox2.Controls.Add(AddAttachmentBT)
        GroupBox2.Controls.Add(RemoveAttachmentBT)
        GroupBox2.Controls.Add(AttachmentTB)
        GroupBox2.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        GroupBox2.Location = New Point(91, 343)
        GroupBox2.Name = "GroupBox2"
        GroupBox2.RightToLeft = RightToLeft.Yes
        GroupBox2.Size = New Size(405, 72)
        GroupBox2.TabIndex = 4
        GroupBox2.TabStop = False
        GroupBox2.Text = " اضف مـــرفــــق جديد"
        ' 
        ' AddAttachmentBT
        ' 
        AddAttachmentBT.BackColor = Color.Transparent
        AddAttachmentBT.BackgroundImageLayout = ImageLayout.Center
        AddAttachmentBT.FlatAppearance.BorderSize = 0
        AddAttachmentBT.ForeColor = SystemColors.ButtonHighlight
        AddAttachmentBT.Image = CType(resources.GetObject("AddAttachmentBT.Image"), Image)
        AddAttachmentBT.Location = New Point(54, 23)
        AddAttachmentBT.Name = "AddAttachmentBT"
        AddAttachmentBT.Size = New Size(39, 37)
        AddAttachmentBT.TabIndex = 5
        AddAttachmentBT.UseVisualStyleBackColor = False
        ' 
        ' RemoveAttachmentBT
        ' 
        RemoveAttachmentBT.BackColor = Color.Transparent
        RemoveAttachmentBT.BackgroundImageLayout = ImageLayout.Center
        RemoveAttachmentBT.FlatAppearance.BorderSize = 0
        RemoveAttachmentBT.ForeColor = SystemColors.ButtonHighlight
        RemoveAttachmentBT.Image = CType(resources.GetObject("RemoveAttachmentBT.Image"), Image)
        RemoveAttachmentBT.Location = New Point(15, 22)
        RemoveAttachmentBT.Name = "RemoveAttachmentBT"
        RemoveAttachmentBT.Size = New Size(33, 38)
        RemoveAttachmentBT.TabIndex = 4
        RemoveAttachmentBT.UseVisualStyleBackColor = False
        ' 
        ' AttachmentTB
        ' 
        AttachmentTB.Location = New Point(110, 31)
        AttachmentTB.Multiline = False
        AttachmentTB.Name = "AttachmentTB"
        AttachmentTB.Size = New Size(255, 29)
        AttachmentTB.TabIndex = 3
        AttachmentTB.Text = ""
        ' 
        ' AttachmentsManagement
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = SystemColors.ButtonHighlight
        ClientSize = New Size(610, 481)
        Controls.Add(GroupBox1)
        FormBorderStyle = FormBorderStyle.FixedSingle
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Name = "AttachmentsManagement"
        StartPosition = FormStartPosition.CenterScreen
        Text = "المـرفـــقـــــات"
        CType(AttachmetTableDGV, ComponentModel.ISupportInitialize).EndInit()
        GroupBox1.ResumeLayout(False)
        GroupBox2.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents AttachmetTableDGV As DataGridView
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents AddAttachmentBT As Button
    Friend WithEvents RemoveAttachmentBT As Button
    Friend WithEvents AttachmentTB As RichTextBox
End Class
