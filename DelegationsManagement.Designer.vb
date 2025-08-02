<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DelegationsManagement
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DelegationsManagement))
        GroupBox1 = New GroupBox()
        GroupBox4 = New GroupBox()
        RemoveDelegatorBT = New Button()
        AddDelegatorBT = New Button()
        dateOfEndOfDelegateEpireTimePicker = New DateTimePicker()
        dateOfStartOfDelegatorTimePicker = New DateTimePicker()
        Label7 = New Label()
        Label6 = New Label()
        Label5 = New Label()
        Label4 = New Label()
        Label3 = New Label()
        Label2 = New Label()
        Label1 = New Label()
        DelegatorNationalityTB = New TextBox()
        DelegatorPhoneNumberTB = New TextBox()
        DelegatorIdentityBT = New TextBox()
        DelegatorNameTB = New TextBox()
        DelegatorTypeTB = New TextBox()
        dataGDlistAllDelegators = New DataGridView()
        GroupBox2 = New GroupBox()
        GroupBox5 = New GroupBox()
        RemovePermissionBT = New Button()
        AddPermissionBT = New Button()
        AddPermissionTB = New TextBox()
        dataGDDelegations = New DataGridView()
        GroupBox3 = New GroupBox()
        CustomerNameLable = New Label()
        dataGDOfDivenPermissions = New DataGridView()
        DelegateToCustomerBT = New Button()
        unDelegateFromCustomerBT = New Button()
        GroupBox1.SuspendLayout()
        GroupBox4.SuspendLayout()
        CType(dataGDlistAllDelegators, ComponentModel.ISupportInitialize).BeginInit()
        GroupBox2.SuspendLayout()
        GroupBox5.SuspendLayout()
        CType(dataGDDelegations, ComponentModel.ISupportInitialize).BeginInit()
        GroupBox3.SuspendLayout()
        CType(dataGDOfDivenPermissions, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        GroupBox1.Controls.Add(GroupBox4)
        GroupBox1.Controls.Add(dataGDlistAllDelegators)
        GroupBox1.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        GroupBox1.Location = New Point(12, 12)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.RightToLeft = RightToLeft.Yes
        GroupBox1.Size = New Size(763, 638)
        GroupBox1.TabIndex = 0
        GroupBox1.TabStop = False
        GroupBox1.Text = "المفوضين"
        ' 
        ' GroupBox4
        ' 
        GroupBox4.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        GroupBox4.Controls.Add(RemoveDelegatorBT)
        GroupBox4.Controls.Add(AddDelegatorBT)
        GroupBox4.Controls.Add(dateOfEndOfDelegateEpireTimePicker)
        GroupBox4.Controls.Add(dateOfStartOfDelegatorTimePicker)
        GroupBox4.Controls.Add(Label7)
        GroupBox4.Controls.Add(Label6)
        GroupBox4.Controls.Add(Label5)
        GroupBox4.Controls.Add(Label4)
        GroupBox4.Controls.Add(Label3)
        GroupBox4.Controls.Add(Label2)
        GroupBox4.Controls.Add(Label1)
        GroupBox4.Controls.Add(DelegatorNationalityTB)
        GroupBox4.Controls.Add(DelegatorPhoneNumberTB)
        GroupBox4.Controls.Add(DelegatorIdentityBT)
        GroupBox4.Controls.Add(DelegatorNameTB)
        GroupBox4.Controls.Add(DelegatorTypeTB)
        GroupBox4.Location = New Point(22, 468)
        GroupBox4.Name = "GroupBox4"
        GroupBox4.Size = New Size(716, 153)
        GroupBox4.TabIndex = 1
        GroupBox4.TabStop = False
        GroupBox4.Text = "أضف مفوض جديد"
        ' 
        ' RemoveDelegatorBT
        ' 
        RemoveDelegatorBT.Anchor = AnchorStyles.Bottom
        RemoveDelegatorBT.BackColor = Color.Transparent
        RemoveDelegatorBT.BackgroundImageLayout = ImageLayout.Center
        RemoveDelegatorBT.Image = CType(resources.GetObject("RemoveDelegatorBT.Image"), Image)
        RemoveDelegatorBT.Location = New Point(75, 107)
        RemoveDelegatorBT.Name = "RemoveDelegatorBT"
        RemoveDelegatorBT.Size = New Size(49, 39)
        RemoveDelegatorBT.TabIndex = 14
        RemoveDelegatorBT.UseVisualStyleBackColor = False
        ' 
        ' AddDelegatorBT
        ' 
        AddDelegatorBT.Anchor = AnchorStyles.Bottom
        AddDelegatorBT.BackColor = Color.Transparent
        AddDelegatorBT.BackgroundImageLayout = ImageLayout.Center
        AddDelegatorBT.FlatAppearance.BorderSize = 0
        AddDelegatorBT.Image = CType(resources.GetObject("AddDelegatorBT.Image"), Image)
        AddDelegatorBT.Location = New Point(618, 108)
        AddDelegatorBT.Name = "AddDelegatorBT"
        AddDelegatorBT.Size = New Size(49, 37)
        AddDelegatorBT.TabIndex = 15
        AddDelegatorBT.UseVisualStyleBackColor = False
        ' 
        ' dateOfEndOfDelegateEpireTimePicker
        ' 
        dateOfEndOfDelegateEpireTimePicker.Anchor = AnchorStyles.Bottom
        dateOfEndOfDelegateEpireTimePicker.Location = New Point(377, 119)
        dateOfEndOfDelegateEpireTimePicker.Name = "dateOfEndOfDelegateEpireTimePicker"
        dateOfEndOfDelegateEpireTimePicker.Size = New Size(200, 23)
        dateOfEndOfDelegateEpireTimePicker.TabIndex = 13
        ' 
        ' dateOfStartOfDelegatorTimePicker
        ' 
        dateOfStartOfDelegatorTimePicker.Anchor = AnchorStyles.Bottom
        dateOfStartOfDelegatorTimePicker.Location = New Point(151, 119)
        dateOfStartOfDelegatorTimePicker.Name = "dateOfStartOfDelegatorTimePicker"
        dateOfStartOfDelegatorTimePicker.Size = New Size(200, 23)
        dateOfStartOfDelegatorTimePicker.TabIndex = 12
        ' 
        ' Label7
        ' 
        Label7.Anchor = AnchorStyles.Bottom
        Label7.AutoSize = True
        Label7.Location = New Point(278, 96)
        Label7.Name = "Label7"
        Label7.Size = New Size(73, 15)
        Label7.TabIndex = 11
        Label7.Text = "تاريخ التصريح"
        ' 
        ' Label6
        ' 
        Label6.Anchor = AnchorStyles.Bottom
        Label6.AutoSize = True
        Label6.Location = New Point(472, 96)
        Label6.Name = "Label6"
        Label6.Size = New Size(104, 15)
        Label6.TabIndex = 10
        Label6.Text = "تاريخ  انتهاء التصريح"
        ' 
        ' Label5
        ' 
        Label5.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Right
        Label5.AutoSize = True
        Label5.Location = New Point(94, 35)
        Label5.Name = "Label5"
        Label5.Size = New Size(52, 15)
        Label5.TabIndex = 9
        Label5.Text = "الوظيـفــه"
        ' 
        ' Label4
        ' 
        Label4.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Right
        Label4.AutoSize = True
        Label4.Location = New Point(235, 35)
        Label4.Name = "Label4"
        Label4.Size = New Size(63, 15)
        Label4.TabIndex = 8
        Label4.Text = "رقم الهــاتف"
        ' 
        ' Label3
        ' 
        Label3.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Right
        Label3.AutoSize = True
        Label3.Location = New Point(380, 40)
        Label3.Name = "Label3"
        Label3.Size = New Size(49, 15)
        Label3.TabIndex = 7
        Label3.Text = "الجنسيــه"
        ' 
        ' Label2
        ' 
        Label2.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Right
        Label2.AutoSize = True
        Label2.Location = New Point(521, 35)
        Label2.Name = "Label2"
        Label2.Size = New Size(41, 15)
        Label2.TabIndex = 6
        Label2.Text = "الهويــه"
        ' 
        ' Label1
        ' 
        Label1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Right
        Label1.AutoSize = True
        Label1.Location = New Point(672, 35)
        Label1.Name = "Label1"
        Label1.Size = New Size(34, 15)
        Label1.TabIndex = 5
        Label1.Text = "الاسم"
        ' 
        ' DelegatorNationalityTB
        ' 
        DelegatorNationalityTB.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Right
        DelegatorNationalityTB.Location = New Point(309, 58)
        DelegatorNationalityTB.Name = "DelegatorNationalityTB"
        DelegatorNationalityTB.Size = New Size(126, 23)
        DelegatorNationalityTB.TabIndex = 4
        ' 
        ' DelegatorPhoneNumberTB
        ' 
        DelegatorPhoneNumberTB.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Right
        DelegatorPhoneNumberTB.Location = New Point(163, 58)
        DelegatorPhoneNumberTB.Name = "DelegatorPhoneNumberTB"
        DelegatorPhoneNumberTB.Size = New Size(135, 23)
        DelegatorPhoneNumberTB.TabIndex = 3
        ' 
        ' DelegatorIdentityBT
        ' 
        DelegatorIdentityBT.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Right
        DelegatorIdentityBT.Location = New Point(445, 58)
        DelegatorIdentityBT.Name = "DelegatorIdentityBT"
        DelegatorIdentityBT.Size = New Size(126, 23)
        DelegatorIdentityBT.TabIndex = 2
        ' 
        ' DelegatorNameTB
        ' 
        DelegatorNameTB.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Right
        DelegatorNameTB.Location = New Point(584, 58)
        DelegatorNameTB.Name = "DelegatorNameTB"
        DelegatorNameTB.Size = New Size(126, 23)
        DelegatorNameTB.TabIndex = 1
        ' 
        ' DelegatorTypeTB
        ' 
        DelegatorTypeTB.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Right
        DelegatorTypeTB.Location = New Point(10, 58)
        DelegatorTypeTB.Name = "DelegatorTypeTB"
        DelegatorTypeTB.Size = New Size(139, 23)
        DelegatorTypeTB.TabIndex = 0
        ' 
        ' dataGDlistAllDelegators
        ' 
        dataGDlistAllDelegators.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        dataGDlistAllDelegators.BackgroundColor = SystemColors.ButtonHighlight
        dataGDlistAllDelegators.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dataGDlistAllDelegators.Location = New Point(6, 22)
        dataGDlistAllDelegators.Name = "dataGDlistAllDelegators"
        dataGDlistAllDelegators.RowHeadersWidth = 51
        dataGDlistAllDelegators.Size = New Size(751, 425)
        dataGDlistAllDelegators.TabIndex = 0
        ' 
        ' GroupBox2
        ' 
        GroupBox2.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Right
        GroupBox2.Controls.Add(GroupBox5)
        GroupBox2.Controls.Add(dataGDDelegations)
        GroupBox2.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        GroupBox2.ForeColor = Color.Green
        GroupBox2.Location = New Point(1182, 11)
        GroupBox2.Name = "GroupBox2"
        GroupBox2.RightToLeft = RightToLeft.Yes
        GroupBox2.Size = New Size(305, 622)
        GroupBox2.TabIndex = 1
        GroupBox2.TabStop = False
        GroupBox2.Text = "التصريحات"
        ' 
        ' GroupBox5
        ' 
        GroupBox5.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        GroupBox5.Controls.Add(RemovePermissionBT)
        GroupBox5.Controls.Add(AddPermissionBT)
        GroupBox5.Controls.Add(AddPermissionTB)
        GroupBox5.ForeColor = Color.ForestGreen
        GroupBox5.Location = New Point(13, 556)
        GroupBox5.Name = "GroupBox5"
        GroupBox5.RightToLeft = RightToLeft.Yes
        GroupBox5.Size = New Size(281, 60)
        GroupBox5.TabIndex = 8
        GroupBox5.TabStop = False
        GroupBox5.Text = "أضافة تصريح جديد"
        ' 
        ' RemovePermissionBT
        ' 
        RemovePermissionBT.BackColor = Color.Transparent
        RemovePermissionBT.BackgroundImageLayout = ImageLayout.Center
        RemovePermissionBT.Image = CType(resources.GetObject("RemovePermissionBT.Image"), Image)
        RemovePermissionBT.Location = New Point(6, 20)
        RemovePermissionBT.Name = "RemovePermissionBT"
        RemovePermissionBT.Size = New Size(38, 36)
        RemovePermissionBT.TabIndex = 12
        RemovePermissionBT.UseVisualStyleBackColor = False
        ' 
        ' AddPermissionBT
        ' 
        AddPermissionBT.BackColor = Color.Transparent
        AddPermissionBT.BackgroundImageLayout = ImageLayout.Center
        AddPermissionBT.FlatAppearance.BorderSize = 0
        AddPermissionBT.Image = CType(resources.GetObject("AddPermissionBT.Image"), Image)
        AddPermissionBT.Location = New Point(50, 20)
        AddPermissionBT.Name = "AddPermissionBT"
        AddPermissionBT.Size = New Size(37, 34)
        AddPermissionBT.TabIndex = 13
        AddPermissionBT.UseVisualStyleBackColor = False
        ' 
        ' AddPermissionTB
        ' 
        AddPermissionTB.Location = New Point(92, 26)
        AddPermissionTB.Name = "AddPermissionTB"
        AddPermissionTB.Size = New Size(183, 23)
        AddPermissionTB.TabIndex = 10
        ' 
        ' dataGDDelegations
        ' 
        dataGDDelegations.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Right
        dataGDDelegations.BackgroundColor = SystemColors.ButtonHighlight
        dataGDDelegations.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dataGDDelegations.Location = New Point(6, 22)
        dataGDDelegations.Name = "dataGDDelegations"
        dataGDDelegations.RowHeadersWidth = 51
        dataGDDelegations.Size = New Size(287, 525)
        dataGDDelegations.TabIndex = 0
        ' 
        ' GroupBox3
        ' 
        GroupBox3.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Right
        GroupBox3.Controls.Add(CustomerNameLable)
        GroupBox3.Controls.Add(dataGDOfDivenPermissions)
        GroupBox3.Font = New Font("Segoe UI Black", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        GroupBox3.ForeColor = Color.DarkRed
        GroupBox3.Location = New Point(809, 11)
        GroupBox3.Name = "GroupBox3"
        GroupBox3.RightToLeft = RightToLeft.Yes
        GroupBox3.Size = New Size(303, 622)
        GroupBox3.TabIndex = 2
        GroupBox3.TabStop = False
        GroupBox3.Text = "التصريحات المصرحه"
        ' 
        ' CustomerNameLable
        ' 
        CustomerNameLable.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        CustomerNameLable.AutoSize = True
        CustomerNameLable.Location = New Point(162, 586)
        CustomerNameLable.Name = "CustomerNameLable"
        CustomerNameLable.Size = New Size(71, 15)
        CustomerNameLable.TabIndex = 12
        CustomerNameLable.Text = "أسم العميل : "
        ' 
        ' dataGDOfDivenPermissions
        ' 
        dataGDOfDivenPermissions.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom
        dataGDOfDivenPermissions.BackgroundColor = SystemColors.ButtonHighlight
        dataGDOfDivenPermissions.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dataGDOfDivenPermissions.Location = New Point(6, 22)
        dataGDOfDivenPermissions.Name = "dataGDOfDivenPermissions"
        dataGDOfDivenPermissions.RowHeadersWidth = 51
        dataGDOfDivenPermissions.Size = New Size(288, 527)
        dataGDOfDivenPermissions.TabIndex = 0
        ' 
        ' DelegateToCustomerBT
        ' 
        DelegateToCustomerBT.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        DelegateToCustomerBT.BackColor = Color.Transparent
        DelegateToCustomerBT.BackgroundImageLayout = ImageLayout.Center
        DelegateToCustomerBT.FlatAppearance.BorderSize = 0
        DelegateToCustomerBT.Image = CType(resources.GetObject("DelegateToCustomerBT.Image"), Image)
        DelegateToCustomerBT.Location = New Point(1118, 160)
        DelegateToCustomerBT.Name = "DelegateToCustomerBT"
        DelegateToCustomerBT.Size = New Size(49, 37)
        DelegateToCustomerBT.TabIndex = 16
        DelegateToCustomerBT.UseVisualStyleBackColor = False
        ' 
        ' unDelegateFromCustomerBT
        ' 
        unDelegateFromCustomerBT.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        unDelegateFromCustomerBT.BackColor = Color.Transparent
        unDelegateFromCustomerBT.BackgroundImageLayout = ImageLayout.Center
        unDelegateFromCustomerBT.FlatAppearance.BorderSize = 0
        unDelegateFromCustomerBT.Image = CType(resources.GetObject("unDelegateFromCustomerBT.Image"), Image)
        unDelegateFromCustomerBT.Location = New Point(1118, 373)
        unDelegateFromCustomerBT.Name = "unDelegateFromCustomerBT"
        unDelegateFromCustomerBT.Size = New Size(49, 37)
        unDelegateFromCustomerBT.TabIndex = 17
        unDelegateFromCustomerBT.UseVisualStyleBackColor = False
        ' 
        ' DelegationsManagement
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1499, 662)
        Controls.Add(unDelegateFromCustomerBT)
        Controls.Add(DelegateToCustomerBT)
        Controls.Add(GroupBox3)
        Controls.Add(GroupBox2)
        Controls.Add(GroupBox1)
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Name = "DelegationsManagement"
        Text = "DelegationsManagement"
        GroupBox1.ResumeLayout(False)
        GroupBox4.ResumeLayout(False)
        GroupBox4.PerformLayout()
        CType(dataGDlistAllDelegators, ComponentModel.ISupportInitialize).EndInit()
        GroupBox2.ResumeLayout(False)
        GroupBox5.ResumeLayout(False)
        GroupBox5.PerformLayout()
        CType(dataGDDelegations, ComponentModel.ISupportInitialize).EndInit()
        GroupBox3.ResumeLayout(False)
        GroupBox3.PerformLayout()
        CType(dataGDOfDivenPermissions, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents dataGDlistAllDelegators As DataGridView
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents dataGDDelegations As DataGridView
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents dataGDOfDivenPermissions As DataGridView
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents DelegatorPhoneNumberTB As TextBox
    Friend WithEvents DelegatorIdentityBT As TextBox
    Friend WithEvents DelegatorNameTB As TextBox
    Friend WithEvents DelegatorTypeTB As TextBox
    Friend WithEvents dateOfDelegateTimePicker As DateTimePicker
    Friend WithEvents dateOfPermisionTimePicker As DateTimePicker
    Friend WithEvents Label7 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents DelegatorNationalityTB As TextBox
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents AddPermissionTB As TextBox
    Friend WithEvents CustomerNameLable As Label
    Friend WithEvents RemoveDelegatorBT As Button
    Friend WithEvents AddDelegatorBT As Button
    Friend WithEvents RemovePermissionBT As Button
    Friend WithEvents AddPermissionBT As Button
    Friend WithEvents DelegateToCustomerBT As Button
    Friend WithEvents unDelegateFromCustomerBT As Button
    Friend WithEvents dateOfEndOfDelegateEpireTimePicker As DateTimePicker
    Friend WithEvents dateOfStartOfDelegatorTimePicker As DateTimePicker
End Class
