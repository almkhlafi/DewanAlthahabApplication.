<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class CustomerSearchForm
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
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CustomerSearchForm))
        CustomerSearchDGV = New DataGridView()
        SearchTB = New TextBox()
        SelectBT = New Button()
        CancelBT = New Button()
        Label1 = New Label()
        SearchForCustomer = New PictureBox()
        GroupBoxAddCustomer = New GroupBox()
        RemoveCustomerBT = New Button()
        AddCustomerBT = New Button()
        addUserTB = New RichTextBox()
        CType(CustomerSearchDGV, ComponentModel.ISupportInitialize).BeginInit()
        CType(SearchForCustomer, ComponentModel.ISupportInitialize).BeginInit()
        GroupBoxAddCustomer.SuspendLayout()
        SuspendLayout()
        ' 
        ' CustomerSearchDGV
        ' 
        CustomerSearchDGV.AllowUserToAddRows = False
        CustomerSearchDGV.BackgroundColor = SystemColors.ButtonHighlight
        CustomerSearchDGV.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        DataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = SystemColors.Control
        DataGridViewCellStyle1.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        DataGridViewCellStyle1.ForeColor = SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = DataGridViewTriState.True
        CustomerSearchDGV.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        CustomerSearchDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = SystemColors.Window
        DataGridViewCellStyle2.Font = New Font("Segoe UI Semibold", 9.0F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        DataGridViewCellStyle2.ForeColor = SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = DataGridViewTriState.False
        CustomerSearchDGV.DefaultCellStyle = DataGridViewCellStyle2
        CustomerSearchDGV.Location = New Point(12, 50)
        CustomerSearchDGV.Name = "CustomerSearchDGV"
        CustomerSearchDGV.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken
        DataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle3.BackColor = SystemColors.Control
        DataGridViewCellStyle3.Font = New Font("Segoe UI Black", 9.0F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        DataGridViewCellStyle3.ForeColor = SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = DataGridViewTriState.True
        CustomerSearchDGV.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        CustomerSearchDGV.RowHeadersVisible = False
        CustomerSearchDGV.RowHeadersWidth = 51
        DataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter
        CustomerSearchDGV.RowsDefaultCellStyle = DataGridViewCellStyle4
        CustomerSearchDGV.RowTemplate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        CustomerSearchDGV.Size = New Size(560, 300)
        CustomerSearchDGV.TabIndex = 1
        ' 
        ' SearchTB
        ' 
        SearchTB.Font = New Font("Segoe UI", 10.0F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        SearchTB.Location = New Point(15, 20)
        SearchTB.Name = "SearchTB"
        SearchTB.PlaceholderText = "البحث بالاسم أو الرقم..."
        SearchTB.RightToLeft = RightToLeft.Yes
        SearchTB.Size = New Size(450, 25)
        SearchTB.TabIndex = 0
        ' 
        ' SelectBT
        ' 
        SelectBT.BackColor = SystemColors.ButtonHighlight
        SelectBT.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        SelectBT.ForeColor = Color.YellowGreen
        SelectBT.Location = New Point(497, 360)
        SelectBT.Name = "SelectBT"
        SelectBT.RightToLeft = RightToLeft.Yes
        SelectBT.Size = New Size(60, 30)
        SelectBT.TabIndex = 2
        SelectBT.Text = " اختيار"
        SelectBT.UseVisualStyleBackColor = False
        ' 
        ' CancelBT
        ' 
        CancelBT.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        CancelBT.ForeColor = Color.LightCoral
        CancelBT.Location = New Point(427, 360)
        CancelBT.Name = "CancelBT"
        CancelBT.RightToLeft = RightToLeft.Yes
        CancelBT.Size = New Size(64, 30)
        CancelBT.TabIndex = 3
        CancelBT.Text = "الغاء"
        CancelBT.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label1.Location = New Point(491, 26)
        Label1.Name = "Label1"
        Label1.RightToLeft = RightToLeft.Yes
        Label1.Size = New Size(79, 15)
        Label1.TabIndex = 4
        Label1.Text = "بحث عن عميل:"
        ' 
        ' SearchForCustomer
        ' 
        SearchForCustomer.BackColor = Color.Transparent
        SearchForCustomer.Image = CType(resources.GetObject("SearchForCustomer.Image"), Image)
        SearchForCustomer.Location = New Point(466, 20)
        SearchForCustomer.Name = "SearchForCustomer"
        SearchForCustomer.Size = New Size(24, 25)
        SearchForCustomer.SizeMode = PictureBoxSizeMode.StretchImage
        SearchForCustomer.TabIndex = 9
        SearchForCustomer.TabStop = False
        ' 
        ' GroupBoxAddCustomer
        ' 
        GroupBoxAddCustomer.Controls.Add(RemoveCustomerBT)
        GroupBoxAddCustomer.Controls.Add(AddCustomerBT)
        GroupBoxAddCustomer.Controls.Add(addUserTB)
        GroupBoxAddCustomer.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        GroupBoxAddCustomer.Location = New Point(150, 408)
        GroupBoxAddCustomer.Name = "GroupBoxAddCustomer"
        GroupBoxAddCustomer.RightToLeft = RightToLeft.Yes
        GroupBoxAddCustomer.Size = New Size(420, 80)
        GroupBoxAddCustomer.TabIndex = 10
        GroupBoxAddCustomer.TabStop = False
        GroupBoxAddCustomer.Text = "إضافة عميل جديد"
        ' 
        ' RemoveCustomerBT
        ' 
        RemoveCustomerBT.BackColor = Color.Transparent
        RemoveCustomerBT.BackgroundImageLayout = ImageLayout.Center
        RemoveCustomerBT.Font = New Font("Segoe UI", 8.0F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        RemoveCustomerBT.Image = CType(resources.GetObject("RemoveCustomerBT.Image"), Image)
        RemoveCustomerBT.Location = New Point(10, 28)
        RemoveCustomerBT.Name = "RemoveCustomerBT"
        RemoveCustomerBT.Size = New Size(40, 41)
        RemoveCustomerBT.TabIndex = 4
        RemoveCustomerBT.UseVisualStyleBackColor = False
        ' 
        ' AddCustomerBT
        ' 
        AddCustomerBT.BackColor = Color.Transparent
        AddCustomerBT.BackgroundImageLayout = ImageLayout.Center
        AddCustomerBT.Font = New Font("Segoe UI", 8.0F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        AddCustomerBT.Image = CType(resources.GetObject("AddCustomerBT.Image"), Image)
        AddCustomerBT.Location = New Point(56, 28)
        AddCustomerBT.Name = "AddCustomerBT"
        AddCustomerBT.Size = New Size(41, 41)
        AddCustomerBT.TabIndex = 5
        AddCustomerBT.UseVisualStyleBackColor = False
        ' 
        ' addUserTB
        ' 
        addUserTB.Location = New Point(103, 38)
        addUserTB.Multiline = False
        addUserTB.Name = "addUserTB"
        addUserTB.Size = New Size(294, 25)
        addUserTB.TabIndex = 3
        addUserTB.Text = ""
        ' 
        ' CustomerSearchForm
        ' 
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(584, 500)
        Controls.Add(GroupBoxAddCustomer)
        Controls.Add(SearchForCustomer)
        Controls.Add(Label1)
        Controls.Add(CancelBT)
        Controls.Add(SelectBT)
        Controls.Add(SearchTB)
        Controls.Add(CustomerSearchDGV)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        Name = "CustomerSearchForm"
        RightToLeft = RightToLeft.No
        StartPosition = FormStartPosition.CenterParent
        Text = "البحث عن عميل"
        CType(CustomerSearchDGV, ComponentModel.ISupportInitialize).EndInit()
        CType(SearchForCustomer, ComponentModel.ISupportInitialize).EndInit()
        GroupBoxAddCustomer.ResumeLayout(False)
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents CustomerSearchDGV As DataGridView
    Friend WithEvents SearchTB As TextBox
    Friend WithEvents SelectBT As Button
    Friend WithEvents CancelBT As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents SearchForCustomer As PictureBox
    Friend WithEvents GroupBoxAddCustomer As GroupBox
    Friend WithEvents RemoveCustomerBT As Button
    Friend WithEvents AddCustomerBT As Button
    Friend WithEvents addUserTB As RichTextBox
End Class