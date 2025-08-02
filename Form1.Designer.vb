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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As DataGridViewCellStyle = New DataGridViewCellStyle()
        GroupBox1 = New GroupBox()
        GroupBox4 = New GroupBox()
        Delegations = New Button()
        GroupBox3 = New GroupBox()
        DocumentsSettings = New Button()
        AddAttachmentsBT = New Button()
        GroupBox2 = New GroupBox()
        RemoveCustomerBT = New Button()
        AddCustomerBT = New Button()
        addUserTB = New RichTextBox()
        CustomerTableDGV = New DataGridView()
        GroupBox1.SuspendLayout()
        GroupBox4.SuspendLayout()
        GroupBox3.SuspendLayout()
        GroupBox2.SuspendLayout()
        CType(CustomerTableDGV, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Anchor = AnchorStyles.None
        GroupBox1.Controls.Add(GroupBox4)
        GroupBox1.Controls.Add(GroupBox3)
        GroupBox1.Controls.Add(GroupBox2)
        GroupBox1.Controls.Add(CustomerTableDGV)
        GroupBox1.Location = New Point(346, 192)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.RightToLeft = RightToLeft.Yes
        GroupBox1.Size = New Size(759, 371)
        GroupBox1.TabIndex = 0
        GroupBox1.TabStop = False
        ' 
        ' GroupBox4
        ' 
        GroupBox4.Controls.Add(Delegations)
        GroupBox4.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        GroupBox4.Location = New Point(78, 286)
        GroupBox4.Name = "GroupBox4"
        GroupBox4.RightToLeft = RightToLeft.Yes
        GroupBox4.Size = New Size(79, 72)
        GroupBox4.TabIndex = 7
        GroupBox4.TabStop = False
        GroupBox4.Text = "التصريحات"
        ' 
        ' Delegations
        ' 
        Delegations.BackColor = Color.Transparent
        Delegations.BackgroundImageLayout = ImageLayout.Center
        Delegations.Image = CType(resources.GetObject("Delegations.Image"), Image)
        Delegations.Location = New Point(9, 22)
        Delegations.Name = "Delegations"
        Delegations.Size = New Size(58, 41)
        Delegations.TabIndex = 6
        Delegations.UseVisualStyleBackColor = False
        ' 
        ' GroupBox3
        ' 
        GroupBox3.Controls.Add(DocumentsSettings)
        GroupBox3.Controls.Add(AddAttachmentsBT)
        GroupBox3.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        GroupBox3.Location = New Point(163, 287)
        GroupBox3.Name = "GroupBox3"
        GroupBox3.RightToLeft = RightToLeft.Yes
        GroupBox3.Size = New Size(121, 72)
        GroupBox3.TabIndex = 6
        GroupBox3.TabStop = False
        GroupBox3.Text = "المرفقات"
        ' 
        ' DocumentsSettings
        ' 
        DocumentsSettings.BackColor = Color.Transparent
        DocumentsSettings.BackgroundImageLayout = ImageLayout.Center
        DocumentsSettings.Image = CType(resources.GetObject("DocumentsSettings.Image"), Image)
        DocumentsSettings.Location = New Point(15, 20)
        DocumentsSettings.Name = "DocumentsSettings"
        DocumentsSettings.Size = New Size(48, 45)
        DocumentsSettings.TabIndex = 6
        DocumentsSettings.UseVisualStyleBackColor = False
        ' 
        ' AddAttachmentsBT
        ' 
        AddAttachmentsBT.BackColor = Color.Transparent
        AddAttachmentsBT.BackgroundImageLayout = ImageLayout.Center
        AddAttachmentsBT.Image = CType(resources.GetObject("AddAttachmentsBT.Image"), Image)
        AddAttachmentsBT.Location = New Point(67, 20)
        AddAttachmentsBT.Name = "AddAttachmentsBT"
        AddAttachmentsBT.Size = New Size(48, 45)
        AddAttachmentsBT.TabIndex = 5
        AddAttachmentsBT.UseVisualStyleBackColor = False
        ' 
        ' GroupBox2
        ' 
        GroupBox2.Controls.Add(RemoveCustomerBT)
        GroupBox2.Controls.Add(AddCustomerBT)
        GroupBox2.Controls.Add(addUserTB)
        GroupBox2.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        GroupBox2.Location = New Point(290, 287)
        GroupBox2.Name = "GroupBox2"
        GroupBox2.RightToLeft = RightToLeft.Yes
        GroupBox2.Size = New Size(451, 72)
        GroupBox2.TabIndex = 3
        GroupBox2.TabStop = False
        GroupBox2.Text = "اضافة مستخدم"
        ' 
        ' RemoveCustomerBT
        ' 
        RemoveCustomerBT.BackColor = Color.Transparent
        RemoveCustomerBT.BackgroundImageLayout = ImageLayout.Center
        RemoveCustomerBT.Image = CType(resources.GetObject("RemoveCustomerBT.Image"), Image)
        RemoveCustomerBT.Location = New Point(20, 23)
        RemoveCustomerBT.Name = "RemoveCustomerBT"
        RemoveCustomerBT.Size = New Size(45, 43)
        RemoveCustomerBT.TabIndex = 4
        RemoveCustomerBT.UseVisualStyleBackColor = False
        ' 
        ' AddCustomerBT
        ' 
        AddCustomerBT.BackColor = Color.Transparent
        AddCustomerBT.BackgroundImageLayout = ImageLayout.Center
        AddCustomerBT.FlatAppearance.BorderSize = 0
        AddCustomerBT.Image = CType(resources.GetObject("AddCustomerBT.Image"), Image)
        AddCustomerBT.Location = New Point(78, 25)
        AddCustomerBT.Name = "AddCustomerBT"
        AddCustomerBT.Size = New Size(45, 41)
        AddCustomerBT.TabIndex = 5
        AddCustomerBT.UseVisualStyleBackColor = False
        ' 
        ' addUserTB
        ' 
        addUserTB.Location = New Point(133, 30)
        addUserTB.Multiline = False
        addUserTB.Name = "addUserTB"
        addUserTB.Size = New Size(295, 29)
        addUserTB.TabIndex = 3
        addUserTB.Text = ""
        ' 
        ' CustomerTableDGV
        ' 
        CustomerTableDGV.AllowUserToAddRows = False
        CustomerTableDGV.Anchor = AnchorStyles.None
        CustomerTableDGV.BackgroundColor = SystemColors.ButtonHighlight
        CustomerTableDGV.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        DataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = SystemColors.Control
        DataGridViewCellStyle1.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        DataGridViewCellStyle1.ForeColor = SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = DataGridViewTriState.True
        CustomerTableDGV.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        CustomerTableDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = SystemColors.Window
        DataGridViewCellStyle2.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        DataGridViewCellStyle2.ForeColor = SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = DataGridViewTriState.False
        CustomerTableDGV.DefaultCellStyle = DataGridViewCellStyle2
        CustomerTableDGV.Location = New Point(10, 14)
        CustomerTableDGV.Name = "CustomerTableDGV"
        CustomerTableDGV.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken
        DataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle3.BackColor = SystemColors.Control
        DataGridViewCellStyle3.Font = New Font("Segoe UI Black", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        DataGridViewCellStyle3.ForeColor = SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = DataGridViewTriState.True
        CustomerTableDGV.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        CustomerTableDGV.RowHeadersVisible = False
        CustomerTableDGV.RowHeadersWidth = 51
        DataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter
        CustomerTableDGV.RowsDefaultCellStyle = DataGridViewCellStyle4
        CustomerTableDGV.RowTemplate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        CustomerTableDGV.Size = New Size(739, 268)
        CustomerTableDGV.TabIndex = 1
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = SystemColors.ButtonHighlight
        ClientSize = New Size(1360, 684)
        Controls.Add(GroupBox1)
        Name = "Form1"
        Text = "Form1"
        WindowState = FormWindowState.Maximized
        GroupBox1.ResumeLayout(False)
        GroupBox4.ResumeLayout(False)
        GroupBox3.ResumeLayout(False)
        GroupBox2.ResumeLayout(False)
        CType(CustomerTableDGV, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents CustomerTableDGV As DataGridView
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents AddAttachmentsBT As Button
    Friend WithEvents RemoveCustomerBT As Button
    Friend WithEvents addUserTB As RichTextBox
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents AddCustomerBT As Button
    Friend WithEvents DocumentsSettings As Button
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents Delegations As Button

End Class
