<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DocumentsManagement
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
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DocumentsManagement))
        GroupBox1 = New GroupBox()
        DocumnetsDGV = New DataGridView()
        GroupBox2 = New GroupBox()
        SacnDocumentBT = New Button()
        UpdateDocumenrBT = New Button()
        GroupBox3 = New GroupBox()
        ExpiredLB = New Label()
        AboutToExpireBT = New Button()
        ExpiredBT = New Button()
        RefreshShowAllDocuments = New Button()
        AboutToExpireLB = New Label()
        GroupBox1.SuspendLayout()
        CType(DocumnetsDGV, ComponentModel.ISupportInitialize).BeginInit()
        GroupBox2.SuspendLayout()
        GroupBox3.SuspendLayout()
        SuspendLayout()
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        GroupBox1.Controls.Add(DocumnetsDGV)
        GroupBox1.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        GroupBox1.ForeColor = SystemColors.ControlDarkDark
        GroupBox1.Location = New Point(12, 12)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.RightToLeft = RightToLeft.Yes
        GroupBox1.Size = New Size(1335, 644)
        GroupBox1.TabIndex = 0
        GroupBox1.TabStop = False
        GroupBox1.Text = " الوثائـــق"
        ' 
        ' DocumnetsDGV
        ' 
        DocumnetsDGV.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = SystemColors.Control
        DataGridViewCellStyle1.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        DataGridViewCellStyle1.ForeColor = SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = DataGridViewTriState.True
        DocumnetsDGV.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        DocumnetsDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DocumnetsDGV.Location = New Point(6, 28)
        DocumnetsDGV.Name = "DocumnetsDGV"
        DocumnetsDGV.RowHeadersWidth = 51
        DocumnetsDGV.Size = New Size(1323, 610)
        DocumnetsDGV.TabIndex = 0
        ' 
        ' GroupBox2
        ' 
        GroupBox2.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        GroupBox2.Controls.Add(SacnDocumentBT)
        GroupBox2.Controls.Add(UpdateDocumenrBT)
        GroupBox2.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        GroupBox2.Location = New Point(1347, 88)
        GroupBox2.Name = "GroupBox2"
        GroupBox2.Size = New Size(62, 146)
        GroupBox2.TabIndex = 1
        GroupBox2.TabStop = False
        GroupBox2.Text = "خيارات"
        ' 
        ' SacnDocumentBT
        ' 
        SacnDocumentBT.BackColor = Color.Transparent
        SacnDocumentBT.BackgroundImageLayout = ImageLayout.Center
        SacnDocumentBT.FlatAppearance.BorderSize = 0
        SacnDocumentBT.ForeColor = SystemColors.ButtonHighlight
        SacnDocumentBT.Image = CType(resources.GetObject("SacnDocumentBT.Image"), Image)
        SacnDocumentBT.Location = New Point(11, 28)
        SacnDocumentBT.Name = "SacnDocumentBT"
        SacnDocumentBT.Size = New Size(43, 48)
        SacnDocumentBT.TabIndex = 7
        SacnDocumentBT.UseVisualStyleBackColor = False
        ' 
        ' UpdateDocumenrBT
        ' 
        UpdateDocumenrBT.BackColor = Color.Transparent
        UpdateDocumenrBT.BackgroundImageLayout = ImageLayout.Center
        UpdateDocumenrBT.FlatAppearance.BorderSize = 0
        UpdateDocumenrBT.ForeColor = SystemColors.ButtonHighlight
        UpdateDocumenrBT.Image = CType(resources.GetObject("UpdateDocumenrBT.Image"), Image)
        UpdateDocumenrBT.Location = New Point(12, 82)
        UpdateDocumenrBT.Name = "UpdateDocumenrBT"
        UpdateDocumenrBT.Size = New Size(42, 51)
        UpdateDocumenrBT.TabIndex = 6
        UpdateDocumenrBT.UseVisualStyleBackColor = False
        ' 
        ' GroupBox3
        ' 
        GroupBox3.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        GroupBox3.Controls.Add(AboutToExpireLB)
        GroupBox3.Controls.Add(ExpiredLB)
        GroupBox3.Controls.Add(AboutToExpireBT)
        GroupBox3.Controls.Add(ExpiredBT)
        GroupBox3.Font = New Font("Segoe UI Black", 8.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        GroupBox3.Location = New Point(1347, 453)
        GroupBox3.Name = "GroupBox3"
        GroupBox3.Size = New Size(74, 169)
        GroupBox3.TabIndex = 2
        GroupBox3.TabStop = False
        GroupBox3.Text = "اشعارات"
        ' 
        ' ExpiredLB
        ' 
        ExpiredLB.AutoSize = True
        ExpiredLB.BackColor = Color.Transparent
        ExpiredLB.Font = New Font("Segoe UI", 9F, FontStyle.Bold Or FontStyle.Italic)
        ExpiredLB.ForeColor = Color.FromArgb(CByte(192), CByte(0), CByte(0))
        ExpiredLB.Location = New Point(46, 98)
        ExpiredLB.Name = "ExpiredLB"
        ExpiredLB.Size = New Size(14, 15)
        ExpiredLB.TabIndex = 10
        ExpiredLB.Text = "0"
        ' 
        ' AboutToExpireBT
        ' 
        AboutToExpireBT.BackColor = Color.Transparent
        AboutToExpireBT.BackgroundImageLayout = ImageLayout.Center
        AboutToExpireBT.FlatAppearance.BorderSize = 0
        AboutToExpireBT.ForeColor = SystemColors.ButtonHighlight
        AboutToExpireBT.Image = CType(resources.GetObject("AboutToExpireBT.Image"), Image)
        AboutToExpireBT.Location = New Point(11, 31)
        AboutToExpireBT.Name = "AboutToExpireBT"
        AboutToExpireBT.Size = New Size(49, 55)
        AboutToExpireBT.TabIndex = 7
        AboutToExpireBT.UseVisualStyleBackColor = False
        ' 
        ' ExpiredBT
        ' 
        ExpiredBT.BackColor = Color.Transparent
        ExpiredBT.BackgroundImageLayout = ImageLayout.Center
        ExpiredBT.FlatAppearance.BorderSize = 0
        ExpiredBT.ForeColor = SystemColors.ButtonHighlight
        ExpiredBT.Image = CType(resources.GetObject("ExpiredBT.Image"), Image)
        ExpiredBT.Location = New Point(13, 100)
        ExpiredBT.Name = "ExpiredBT"
        ExpiredBT.Size = New Size(48, 58)
        ExpiredBT.TabIndex = 6
        ExpiredBT.UseVisualStyleBackColor = False
        ' 
        ' RefreshShowAllDocuments
        ' 
        RefreshShowAllDocuments.Anchor = AnchorStyles.Right
        RefreshShowAllDocuments.BackColor = Color.Transparent
        RefreshShowAllDocuments.BackgroundImageLayout = ImageLayout.Center
        RefreshShowAllDocuments.FlatAppearance.BorderSize = 0
        RefreshShowAllDocuments.ForeColor = SystemColors.ButtonHighlight
        RefreshShowAllDocuments.Image = CType(resources.GetObject("RefreshShowAllDocuments.Image"), Image)
        RefreshShowAllDocuments.Location = New Point(1359, 305)
        RefreshShowAllDocuments.Name = "RefreshShowAllDocuments"
        RefreshShowAllDocuments.Size = New Size(48, 56)
        RefreshShowAllDocuments.TabIndex = 8
        RefreshShowAllDocuments.UseVisualStyleBackColor = False
        ' 
        ' AboutToExpireLB
        ' 
        AboutToExpireLB.AutoSize = True
        AboutToExpireLB.BackColor = Color.Transparent
        AboutToExpireLB.Font = New Font("Segoe UI", 9F, FontStyle.Bold Or FontStyle.Italic)
        AboutToExpireLB.ForeColor = Color.Orange
        AboutToExpireLB.Location = New Point(51, 33)
        AboutToExpireLB.Name = "AboutToExpireLB"
        AboutToExpireLB.Size = New Size(14, 15)
        AboutToExpireLB.TabIndex = 11
        AboutToExpireLB.Text = "0"
        ' 
        ' DocumentsManagement
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1433, 668)
        Controls.Add(RefreshShowAllDocuments)
        Controls.Add(GroupBox3)
        Controls.Add(GroupBox2)
        Controls.Add(GroupBox1)
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Name = "DocumentsManagement"
        Text = "DocumentsManagement"
        GroupBox1.ResumeLayout(False)
        CType(DocumnetsDGV, ComponentModel.ISupportInitialize).EndInit()
        GroupBox2.ResumeLayout(False)
        GroupBox3.ResumeLayout(False)
        GroupBox3.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents DocumnetsDGV As DataGridView
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents SacnDocumentBT As Button
    Friend WithEvents UpdateDocumenrBT As Button
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents AboutToExpireBT As Button
    Friend WithEvents ExpiredBT As Button
    Friend WithEvents RefreshShowAllDocuments As Button
    Friend WithEvents ExpiredLB As Label
    Friend WithEvents AboutToExpireLB As Label
End Class
