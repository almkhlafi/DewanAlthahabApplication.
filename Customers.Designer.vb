<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Customers
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
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Customers))
        GroupBox1 = New GroupBox()
        GroupBox6 = New GroupBox()
        CurrencyDGV = New DataGridView()
        GroupBox4 = New GroupBox()
        BranchesInfoDGV = New DataGridView()
        GroupBox3 = New GroupBox()
        TextBox16 = New TextBox()
        TextBox15 = New TextBox()
        TextBox14 = New TextBox()
        TextBox13 = New TextBox()
        TextBox12 = New TextBox()
        TextBox11 = New TextBox()
        TextBox10 = New TextBox()
        ComboBox2 = New ComboBox()
        ComboBox1 = New ComboBox()
        CheckBox2 = New CheckBox()
        TextBox9 = New TextBox()
        Label20 = New Label()
        TextBox8 = New TextBox()
        AreaCB = New ComboBox()
        CountryCB = New ComboBox()
        TextBox7 = New TextBox()
        TextBox6 = New TextBox()
        TextBox5 = New TextBox()
        RichTextBox1 = New RichTextBox()
        TextBox4 = New TextBox()
        CheckBox1 = New CheckBox()
        CustomerAccountNumber = New TextBox()
        AccountNumber = New TextBox()
        identityNB = New TextBox()
        Label19 = New Label()
        Label18 = New Label()
        Label17 = New Label()
        Label16 = New Label()
        Label15 = New Label()
        Label14 = New Label()
        Label13 = New Label()
        Label11 = New Label()
        Label10 = New Label()
        Label9 = New Label()
        Label8 = New Label()
        Label7 = New Label()
        Label6 = New Label()
        Label5 = New Label()
        Label4 = New Label()
        Label3 = New Label()
        Label2 = New Label()
        Label1 = New Label()
        GroupBox5 = New GroupBox()
        PictureBox1 = New PictureBox()
        GroupBox2 = New GroupBox()
        Delegations = New PictureBox()
        DocumentsSettings = New PictureBox()
        PictureBox3 = New PictureBox()
        AddAttachmentsBT = New PictureBox()
        PictureBox5 = New PictureBox()
        NotifyIcon1 = New NotifyIcon(components)
        GroupBox1.SuspendLayout()
        GroupBox6.SuspendLayout()
        CType(CurrencyDGV, ComponentModel.ISupportInitialize).BeginInit()
        GroupBox4.SuspendLayout()
        CType(BranchesInfoDGV, ComponentModel.ISupportInitialize).BeginInit()
        GroupBox3.SuspendLayout()
        GroupBox5.SuspendLayout()
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
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
        GroupBox1.Controls.Add(GroupBox6)
        GroupBox1.Controls.Add(GroupBox4)
        GroupBox1.Controls.Add(GroupBox3)
        GroupBox1.Location = New Point(6, 80)
        GroupBox1.Margin = New Padding(3, 4, 3, 4)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Padding = New Padding(3, 4, 3, 4)
        GroupBox1.RightToLeft = RightToLeft.Yes
        GroupBox1.Size = New Size(1546, 885)
        GroupBox1.TabIndex = 0
        GroupBox1.TabStop = False
        ' 
        ' GroupBox6
        ' 
        GroupBox6.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        GroupBox6.Controls.Add(CurrencyDGV)
        GroupBox6.Location = New Point(-1, 13)
        GroupBox6.Name = "GroupBox6"
        GroupBox6.Size = New Size(880, 171)
        GroupBox6.TabIndex = 5
        GroupBox6.TabStop = False
        GroupBox6.Text = "العمله"
        ' 
        ' CurrencyDGV
        ' 
        CurrencyDGV.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        CurrencyDGV.BackgroundColor = SystemColors.ButtonHighlight
        CurrencyDGV.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = SystemColors.Control
        DataGridViewCellStyle1.Font = New Font("Segoe UI", 9.0F)
        DataGridViewCellStyle1.ForeColor = SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = DataGridViewTriState.True
        CurrencyDGV.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        CurrencyDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = SystemColors.Window
        DataGridViewCellStyle2.Font = New Font("Segoe UI", 9.0F)
        DataGridViewCellStyle2.ForeColor = SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = DataGridViewTriState.False
        CurrencyDGV.DefaultCellStyle = DataGridViewCellStyle2
        CurrencyDGV.Location = New Point(7, 26)
        CurrencyDGV.Name = "CurrencyDGV"
        CurrencyDGV.ReadOnly = True
        CurrencyDGV.RowHeadersWidth = 51
        CurrencyDGV.Size = New Size(867, 139)
        CurrencyDGV.TabIndex = 4
        ' 
        ' GroupBox4
        ' 
        GroupBox4.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        GroupBox4.Controls.Add(BranchesInfoDGV)
        GroupBox4.Location = New Point(6, 197)
        GroupBox4.Name = "GroupBox4"
        GroupBox4.Size = New Size(873, 679)
        GroupBox4.TabIndex = 4
        GroupBox4.TabStop = False
        GroupBox4.Text = "معلومات الفرع"
        ' 
        ' BranchesInfoDGV
        ' 
        BranchesInfoDGV.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        BranchesInfoDGV.BackgroundColor = SystemColors.ButtonHighlight
        DataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle3.BackColor = SystemColors.Control
        DataGridViewCellStyle3.Font = New Font("Segoe UI", 9.0F)
        DataGridViewCellStyle3.ForeColor = SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = DataGridViewTriState.True
        BranchesInfoDGV.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle3
        BranchesInfoDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle4.BackColor = SystemColors.Window
        DataGridViewCellStyle4.Font = New Font("Segoe UI", 9.0F)
        DataGridViewCellStyle4.ForeColor = SystemColors.ControlText
        DataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = DataGridViewTriState.False
        BranchesInfoDGV.DefaultCellStyle = DataGridViewCellStyle4
        BranchesInfoDGV.Location = New Point(6, 24)
        BranchesInfoDGV.Name = "BranchesInfoDGV"
        BranchesInfoDGV.ReadOnly = True
        BranchesInfoDGV.RowHeadersWidth = 51
        BranchesInfoDGV.Size = New Size(861, 649)
        BranchesInfoDGV.TabIndex = 5
        ' 
        ' GroupBox3
        ' 
        GroupBox3.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Right
        GroupBox3.Controls.Add(TextBox16)
        GroupBox3.Controls.Add(TextBox15)
        GroupBox3.Controls.Add(TextBox14)
        GroupBox3.Controls.Add(TextBox13)
        GroupBox3.Controls.Add(TextBox12)
        GroupBox3.Controls.Add(TextBox11)
        GroupBox3.Controls.Add(TextBox10)
        GroupBox3.Controls.Add(ComboBox2)
        GroupBox3.Controls.Add(ComboBox1)
        GroupBox3.Controls.Add(CheckBox2)
        GroupBox3.Controls.Add(TextBox9)
        GroupBox3.Controls.Add(Label20)
        GroupBox3.Controls.Add(TextBox8)
        GroupBox3.Controls.Add(AreaCB)
        GroupBox3.Controls.Add(CountryCB)
        GroupBox3.Controls.Add(TextBox7)
        GroupBox3.Controls.Add(TextBox6)
        GroupBox3.Controls.Add(TextBox5)
        GroupBox3.Controls.Add(RichTextBox1)
        GroupBox3.Controls.Add(TextBox4)
        GroupBox3.Controls.Add(CheckBox1)
        GroupBox3.Controls.Add(CustomerAccountNumber)
        GroupBox3.Controls.Add(AccountNumber)
        GroupBox3.Controls.Add(identityNB)
        GroupBox3.Controls.Add(Label19)
        GroupBox3.Controls.Add(Label18)
        GroupBox3.Controls.Add(Label17)
        GroupBox3.Controls.Add(Label16)
        GroupBox3.Controls.Add(Label15)
        GroupBox3.Controls.Add(Label14)
        GroupBox3.Controls.Add(Label13)
        GroupBox3.Controls.Add(Label11)
        GroupBox3.Controls.Add(Label10)
        GroupBox3.Controls.Add(Label9)
        GroupBox3.Controls.Add(Label8)
        GroupBox3.Controls.Add(Label7)
        GroupBox3.Controls.Add(Label6)
        GroupBox3.Controls.Add(Label5)
        GroupBox3.Controls.Add(Label4)
        GroupBox3.Controls.Add(Label3)
        GroupBox3.Controls.Add(Label2)
        GroupBox3.Controls.Add(Label1)
        GroupBox3.Location = New Point(884, 13)
        GroupBox3.Margin = New Padding(2, 3, 2, 3)
        GroupBox3.Name = "GroupBox3"
        GroupBox3.Padding = New Padding(2, 3, 2, 3)
        GroupBox3.Size = New Size(657, 865)
        GroupBox3.TabIndex = 2
        GroupBox3.TabStop = False
        ' 
        ' TextBox16
        ' 
        TextBox16.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        TextBox16.Location = New Point(162, 809)
        TextBox16.Margin = New Padding(2, 3, 2, 3)
        TextBox16.Name = "TextBox16"
        TextBox16.Size = New Size(257, 27)
        TextBox16.TabIndex = 42
        ' 
        ' TextBox15
        ' 
        TextBox15.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        TextBox15.Location = New Point(162, 755)
        TextBox15.Margin = New Padding(2, 3, 2, 3)
        TextBox15.Name = "TextBox15"
        TextBox15.Size = New Size(257, 27)
        TextBox15.TabIndex = 41
        ' 
        ' TextBox14
        ' 
        TextBox14.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        TextBox14.Location = New Point(483, 809)
        TextBox14.Margin = New Padding(2, 3, 2, 3)
        TextBox14.Name = "TextBox14"
        TextBox14.Size = New Size(166, 27)
        TextBox14.TabIndex = 40
        ' 
        ' TextBox13
        ' 
        TextBox13.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        TextBox13.Location = New Point(479, 757)
        TextBox13.Margin = New Padding(2, 3, 2, 3)
        TextBox13.Name = "TextBox13"
        TextBox13.Size = New Size(166, 27)
        TextBox13.TabIndex = 39
        ' 
        ' TextBox12
        ' 
        TextBox12.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        TextBox12.Location = New Point(479, 705)
        TextBox12.Margin = New Padding(2, 3, 2, 3)
        TextBox12.Name = "TextBox12"
        TextBox12.Size = New Size(166, 27)
        TextBox12.TabIndex = 38
        ' 
        ' TextBox11
        ' 
        TextBox11.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        TextBox11.Location = New Point(283, 624)
        TextBox11.Margin = New Padding(2, 3, 2, 3)
        TextBox11.Name = "TextBox11"
        TextBox11.Size = New Size(166, 27)
        TextBox11.TabIndex = 37
        ' 
        ' TextBox10
        ' 
        TextBox10.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        TextBox10.Location = New Point(466, 624)
        TextBox10.Margin = New Padding(2, 3, 2, 3)
        TextBox10.Name = "TextBox10"
        TextBox10.Size = New Size(166, 27)
        TextBox10.TabIndex = 36
        ' 
        ' ComboBox2
        ' 
        ComboBox2.FormattingEnabled = True
        ComboBox2.Location = New Point(363, 16)
        ComboBox2.Margin = New Padding(2, 3, 2, 3)
        ComboBox2.Name = "ComboBox2"
        ComboBox2.Size = New Size(143, 28)
        ComboBox2.TabIndex = 35
        ' 
        ' ComboBox1
        ' 
        ComboBox1.FormattingEnabled = True
        ComboBox1.Location = New Point(510, 16)
        ComboBox1.Margin = New Padding(2, 3, 2, 3)
        ComboBox1.Name = "ComboBox1"
        ComboBox1.Size = New Size(143, 28)
        ComboBox1.TabIndex = 34
        ' 
        ' CheckBox2
        ' 
        CheckBox2.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        CheckBox2.AutoSize = True
        CheckBox2.Location = New Point(478, 556)
        CheckBox2.Margin = New Padding(2, 3, 2, 3)
        CheckBox2.Name = "CheckBox2"
        CheckBox2.Size = New Size(165, 24)
        CheckBox2.TabIndex = 33
        CheckBox2.Text = "خاضع للرقم الضريبي "
        CheckBox2.UseVisualStyleBackColor = True
        ' 
        ' TextBox9
        ' 
        TextBox9.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        TextBox9.Location = New Point(152, 467)
        TextBox9.Margin = New Padding(2, 3, 2, 3)
        TextBox9.Name = "TextBox9"
        TextBox9.Size = New Size(257, 27)
        TextBox9.TabIndex = 32
        ' 
        ' Label20
        ' 
        Label20.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label20.AutoSize = True
        Label20.Location = New Point(318, 445)
        Label20.Margin = New Padding(2, 0, 2, 0)
        Label20.Name = "Label20"
        Label20.Size = New Size(91, 20)
        Label20.TabIndex = 31
        Label20.Text = "الاسم التجاري"
        ' 
        ' TextBox8
        ' 
        TextBox8.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        TextBox8.Location = New Point(427, 467)
        TextBox8.Margin = New Padding(2, 3, 2, 3)
        TextBox8.Name = "TextBox8"
        TextBox8.Size = New Size(212, 27)
        TextBox8.TabIndex = 30
        ' 
        ' AreaCB
        ' 
        AreaCB.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        AreaCB.FormattingEnabled = True
        AreaCB.Location = New Point(89, 363)
        AreaCB.Margin = New Padding(2, 3, 2, 3)
        AreaCB.Name = "AreaCB"
        AreaCB.Size = New Size(319, 28)
        AreaCB.TabIndex = 29
        ' 
        ' CountryCB
        ' 
        CountryCB.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        CountryCB.FormattingEnabled = True
        CountryCB.Location = New Point(89, 295)
        CountryCB.Margin = New Padding(2, 3, 2, 3)
        CountryCB.Name = "CountryCB"
        CountryCB.Size = New Size(319, 28)
        CountryCB.TabIndex = 28
        ' 
        ' TextBox7
        ' 
        TextBox7.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        TextBox7.Location = New Point(427, 401)
        TextBox7.Margin = New Padding(2, 3, 2, 3)
        TextBox7.Name = "TextBox7"
        TextBox7.Size = New Size(217, 27)
        TextBox7.TabIndex = 27
        ' 
        ' TextBox6
        ' 
        TextBox6.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        TextBox6.Location = New Point(427, 297)
        TextBox6.Margin = New Padding(2, 3, 2, 3)
        TextBox6.Name = "TextBox6"
        TextBox6.Size = New Size(217, 27)
        TextBox6.TabIndex = 26
        ' 
        ' TextBox5
        ' 
        TextBox5.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        TextBox5.Location = New Point(426, 347)
        TextBox5.Margin = New Padding(2, 3, 2, 3)
        TextBox5.Name = "TextBox5"
        TextBox5.Size = New Size(217, 27)
        TextBox5.TabIndex = 25
        ' 
        ' RichTextBox1
        ' 
        RichTextBox1.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        RichTextBox1.Location = New Point(5, 91)
        RichTextBox1.Margin = New Padding(2, 3, 2, 3)
        RichTextBox1.Name = "RichTextBox1"
        RichTextBox1.Size = New Size(238, 143)
        RichTextBox1.TabIndex = 24
        RichTextBox1.Text = ""
        ' 
        ' TextBox4
        ' 
        TextBox4.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        TextBox4.Location = New Point(250, 205)
        TextBox4.Margin = New Padding(2, 3, 2, 3)
        TextBox4.Name = "TextBox4"
        TextBox4.Size = New Size(394, 27)
        TextBox4.TabIndex = 23
        ' 
        ' CheckBox1
        ' 
        CheckBox1.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        CheckBox1.AutoSize = True
        CheckBox1.Location = New Point(295, 147)
        CheckBox1.Margin = New Padding(2, 3, 2, 3)
        CheckBox1.Name = "CheckBox1"
        CheckBox1.Size = New Size(141, 24)
        CheckBox1.TabIndex = 22
        CheckBox1.Text = "مفعل / غير مفعل"
        CheckBox1.UseVisualStyleBackColor = True
        ' 
        ' CustomerAccountNumber
        ' 
        CustomerAccountNumber.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        CustomerAccountNumber.Location = New Point(470, 147)
        CustomerAccountNumber.Margin = New Padding(2, 3, 2, 3)
        CustomerAccountNumber.Name = "CustomerAccountNumber"
        CustomerAccountNumber.Size = New Size(174, 27)
        CustomerAccountNumber.TabIndex = 21
        ' 
        ' AccountNumber
        ' 
        AccountNumber.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        AccountNumber.Location = New Point(250, 85)
        AccountNumber.Margin = New Padding(2, 3, 2, 3)
        AccountNumber.Name = "AccountNumber"
        AccountNumber.Size = New Size(191, 27)
        AccountNumber.TabIndex = 20
        ' 
        ' identityNB
        ' 
        identityNB.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        identityNB.Location = New Point(466, 85)
        identityNB.Margin = New Padding(2, 3, 2, 3)
        identityNB.Name = "identityNB"
        identityNB.Size = New Size(179, 27)
        identityNB.TabIndex = 19
        ' 
        ' Label19
        ' 
        Label19.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label19.AutoSize = True
        Label19.Location = New Point(342, 787)
        Label19.Margin = New Padding(2, 0, 2, 0)
        Label19.Name = "Label19"
        Label19.Size = New Size(77, 20)
        Label19.TabIndex = 18
        Label19.Text = "رقم المرجع"
        ' 
        ' Label18
        ' 
        Label18.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label18.AutoSize = True
        Label18.Location = New Point(335, 733)
        Label18.Margin = New Padding(2, 0, 2, 0)
        Label18.Name = "Label18"
        Label18.Size = New Size(86, 20)
        Label18.TabIndex = 17
        Label18.Text = " رقم الفاكس"
        ' 
        ' Label17
        ' 
        Label17.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label17.AutoSize = True
        Label17.Location = New Point(567, 787)
        Label17.Margin = New Padding(2, 0, 2, 0)
        Label17.Name = "Label17"
        Label17.Size = New Size(80, 20)
        Label17.TabIndex = 16
        Label17.Text = " رقم الهاتف"
        ' 
        ' Label16
        ' 
        Label16.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label16.AutoSize = True
        Label16.Location = New Point(558, 735)
        Label16.Margin = New Padding(2, 0, 2, 0)
        Label16.Name = "Label16"
        Label16.Size = New Size(91, 20)
        Label16.TabIndex = 15
        Label16.Text = " رقم الجوال 2"
        ' 
        ' Label15
        ' 
        Label15.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label15.AutoSize = True
        Label15.Location = New Point(559, 683)
        Label15.Margin = New Padding(2, 0, 2, 0)
        Label15.Name = "Label15"
        Label15.Size = New Size(91, 20)
        Label15.TabIndex = 14
        Label15.Text = " رقم الجوال 1"
        ' 
        ' Label14
        ' 
        Label14.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label14.AutoSize = True
        Label14.Location = New Point(394, 603)
        Label14.Margin = New Padding(2, 0, 2, 0)
        Label14.Name = "Label14"
        Label14.Size = New Size(55, 20)
        Label14.TabIndex = 13
        Label14.Text = "الايميل "
        ' 
        ' Label13
        ' 
        Label13.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label13.AutoSize = True
        Label13.Location = New Point(535, 603)
        Label13.Margin = New Padding(2, 0, 2, 0)
        Label13.Name = "Label13"
        Label13.Size = New Size(97, 20)
        Label13.TabIndex = 12
        Label13.Text = "الرقم الضريبي"
        ' 
        ' Label11
        ' 
        Label11.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label11.AutoSize = True
        Label11.Location = New Point(546, 445)
        Label11.Margin = New Padding(2, 0, 2, 0)
        Label11.Name = "Label11"
        Label11.Size = New Size(97, 20)
        Label11.TabIndex = 10
        Label11.Text = "الاسم الرسمي"
        ' 
        ' Label10
        ' 
        Label10.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label10.AutoSize = True
        Label10.Location = New Point(574, 379)
        Label10.Margin = New Padding(2, 0, 2, 0)
        Label10.Name = "Label10"
        Label10.Size = New Size(72, 20)
        Label10.TabIndex = 9
        Label10.Text = "رقم المدير"
        ' 
        ' Label9
        ' 
        Label9.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label9.AutoSize = True
        Label9.Location = New Point(373, 273)
        Label9.Margin = New Padding(2, 0, 2, 0)
        Label9.Name = "Label9"
        Label9.Size = New Size(36, 20)
        Label9.TabIndex = 8
        Label9.Text = "البلد"
        ' 
        ' Label8
        ' 
        Label8.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label8.AutoSize = True
        Label8.Location = New Point(345, 340)
        Label8.Margin = New Padding(2, 0, 2, 0)
        Label8.Name = "Label8"
        Label8.Size = New Size(67, 20)
        Label8.TabIndex = 7
        Label8.Text = "المنطقــه"
        ' 
        ' Label7
        ' 
        Label7.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label7.AutoSize = True
        Label7.Location = New Point(568, 275)
        Label7.Margin = New Padding(2, 0, 2, 0)
        Label7.Name = "Label7"
        Label7.Size = New Size(79, 20)
        Label7.TabIndex = 6
        Label7.Text = " (ID) المدير"
        ' 
        ' Label6
        ' 
        Label6.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label6.AutoSize = True
        Label6.Location = New Point(597, 325)
        Label6.Margin = New Padding(2, 0, 2, 0)
        Label6.Name = "Label6"
        Label6.Size = New Size(46, 20)
        Label6.TabIndex = 5
        Label6.Text = "المدير"
        ' 
        ' Label5
        ' 
        Label5.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label5.AutoSize = True
        Label5.Location = New Point(178, 63)
        Label5.Margin = New Padding(2, 0, 2, 0)
        Label5.Name = "Label5"
        Label5.Size = New Size(69, 20)
        Label5.TabIndex = 4
        Label5.Text = "العــنـــوان"
        ' 
        ' Label4
        ' 
        Label4.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label4.AutoSize = True
        Label4.Location = New Point(538, 184)
        Label4.Margin = New Padding(2, 0, 2, 0)
        Label4.Name = "Label4"
        Label4.Size = New Size(106, 20)
        Label4.TabIndex = 3
        Label4.Text = "الاسم بالانجليزي"
        ' 
        ' Label3
        ' 
        Label3.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label3.AutoSize = True
        Label3.Location = New Point(522, 123)
        Label3.Margin = New Padding(2, 0, 2, 0)
        Label3.Name = "Label3"
        Label3.Size = New Size(121, 20)
        Label3.TabIndex = 2
        Label3.Text = "رقم حساب العميل"
        ' 
        ' Label2
        ' 
        Label2.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label2.AutoSize = True
        Label2.Location = New Point(382, 63)
        Label2.Margin = New Padding(2, 0, 2, 0)
        Label2.Name = "Label2"
        Label2.Size = New Size(81, 20)
        Label2.TabIndex = 1
        Label2.Text = "رقم السجيل"
        ' 
        ' Label1
        ' 
        Label1.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label1.AutoSize = True
        Label1.Location = New Point(568, 63)
        Label1.Margin = New Padding(2, 0, 2, 0)
        Label1.Name = "Label1"
        Label1.Size = New Size(74, 20)
        Label1.TabIndex = 0
        Label1.Text = "رقم الهويه"
        ' 
        ' GroupBox5
        ' 
        GroupBox5.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        GroupBox5.Controls.Add(PictureBox1)
        GroupBox5.Location = New Point(5, -1)
        GroupBox5.Margin = New Padding(3, 4, 3, 4)
        GroupBox5.Name = "GroupBox5"
        GroupBox5.Padding = New Padding(3, 4, 3, 4)
        GroupBox5.RightToLeft = RightToLeft.Yes
        GroupBox5.Size = New Size(1608, 73)
        GroupBox5.TabIndex = 1
        GroupBox5.TabStop = False
        ' 
        ' PictureBox1
        ' 
        PictureBox1.BackColor = Color.Transparent
        PictureBox1.BackgroundImageLayout = ImageLayout.None
        PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), Image)
        PictureBox1.ImageLocation = ""
        PictureBox1.Location = New Point(19, 28)
        PictureBox1.Margin = New Padding(3, 4, 3, 4)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(31, 29)
        PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
        PictureBox1.TabIndex = 8
        PictureBox1.TabStop = False
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
        GroupBox2.Location = New Point(1552, 125)
        GroupBox2.Margin = New Padding(3, 4, 3, 4)
        GroupBox2.Name = "GroupBox2"
        GroupBox2.Padding = New Padding(3, 4, 3, 4)
        GroupBox2.RightToLeft = RightToLeft.Yes
        GroupBox2.Size = New Size(62, 469)
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
        Delegations.Location = New Point(13, 336)
        Delegations.Margin = New Padding(3, 4, 3, 4)
        Delegations.Name = "Delegations"
        Delegations.Size = New Size(40, 43)
        Delegations.SizeMode = PictureBoxSizeMode.StretchImage
        Delegations.TabIndex = 8
        Delegations.TabStop = False
        ' 
        ' DocumentsSettings
        ' 
        DocumentsSettings.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        DocumentsSettings.BackColor = Color.Transparent
        DocumentsSettings.Image = CType(resources.GetObject("DocumentsSettings.Image"), Image)
        DocumentsSettings.Location = New Point(14, 244)
        DocumentsSettings.Margin = New Padding(3, 4, 3, 4)
        DocumentsSettings.Name = "DocumentsSettings"
        DocumentsSettings.Size = New Size(40, 45)
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
        PictureBox3.Location = New Point(11, 20)
        PictureBox3.Margin = New Padding(3, 4, 3, 4)
        PictureBox3.Name = "PictureBox3"
        PictureBox3.Size = New Size(39, 41)
        PictureBox3.SizeMode = PictureBoxSizeMode.StretchImage
        PictureBox3.TabIndex = 7
        PictureBox3.TabStop = False
        ' 
        ' AddAttachmentsBT
        ' 
        AddAttachmentsBT.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        AddAttachmentsBT.BackColor = Color.Transparent
        AddAttachmentsBT.Image = CType(resources.GetObject("AddAttachmentsBT.Image"), Image)
        AddAttachmentsBT.Location = New Point(11, 188)
        AddAttachmentsBT.Margin = New Padding(3, 4, 3, 4)
        AddAttachmentsBT.Name = "AddAttachmentsBT"
        AddAttachmentsBT.Size = New Size(40, 43)
        AddAttachmentsBT.SizeMode = PictureBoxSizeMode.StretchImage
        AddAttachmentsBT.TabIndex = 5
        AddAttachmentsBT.TabStop = False
        ' 
        ' PictureBox5
        ' 
        PictureBox5.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        PictureBox5.Image = CType(resources.GetObject("PictureBox5.Image"), Image)
        PictureBox5.Location = New Point(11, 72)
        PictureBox5.Margin = New Padding(3, 4, 3, 4)
        PictureBox5.Name = "PictureBox5"
        PictureBox5.Size = New Size(39, 45)
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
        ' Customers
        ' 
        AutoScaleDimensions = New SizeF(8.0F, 20.0F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = SystemColors.ButtonHighlight
        ClientSize = New Size(1622, 968)
        Controls.Add(GroupBox2)
        Controls.Add(GroupBox5)
        Controls.Add(GroupBox1)
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Margin = New Padding(3, 4, 3, 4)
        MaximizeBox = False
        MinimizeBox = False
        Name = "Customers"
        StartPosition = FormStartPosition.CenterScreen
        Text = "الـعــمــــــــلاء"
        GroupBox1.ResumeLayout(False)
        GroupBox6.ResumeLayout(False)
        CType(CurrencyDGV, ComponentModel.ISupportInitialize).EndInit()
        GroupBox4.ResumeLayout(False)
        CType(BranchesInfoDGV, ComponentModel.ISupportInitialize).EndInit()
        GroupBox3.ResumeLayout(False)
        GroupBox3.PerformLayout()
        GroupBox5.ResumeLayout(False)
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        GroupBox2.ResumeLayout(False)
        CType(Delegations, ComponentModel.ISupportInitialize).EndInit()
        CType(DocumentsSettings, ComponentModel.ISupportInitialize).EndInit()
        CType(PictureBox3, ComponentModel.ISupportInitialize).EndInit()
        CType(AddAttachmentsBT, ComponentModel.ISupportInitialize).EndInit()
        CType(PictureBox5, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Delegations As PictureBox
    Friend WithEvents DocumentsSettings As PictureBox
    Friend WithEvents PictureBox3 As PictureBox
    Friend WithEvents AddAttachmentsBT As PictureBox
    Friend WithEvents PictureBox5 As PictureBox
    Friend WithEvents NotifyIcon1 As NotifyIcon
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents Label14 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents CustomerAccountNumber As TextBox
    Friend WithEvents AccountNumber As TextBox
    Friend WithEvents identityNB As TextBox
    Friend WithEvents Label19 As Label
    Friend WithEvents Label18 As Label
    Friend WithEvents Label17 As Label
    Friend WithEvents Label16 As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents CheckBox1 As CheckBox
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents CheckBox2 As CheckBox
    Friend WithEvents TextBox9 As TextBox
    Friend WithEvents Label20 As Label
    Friend WithEvents TextBox8 As TextBox
    Friend WithEvents AreaCB As ComboBox
    Friend WithEvents CountryCB As ComboBox
    Friend WithEvents TextBox7 As TextBox
    Friend WithEvents TextBox6 As TextBox
    Friend WithEvents TextBox5 As TextBox
    Friend WithEvents RichTextBox1 As RichTextBox
    Friend WithEvents TextBox16 As TextBox
    Friend WithEvents TextBox15 As TextBox
    Friend WithEvents TextBox14 As TextBox
    Friend WithEvents TextBox13 As TextBox
    Friend WithEvents TextBox12 As TextBox
    Friend WithEvents TextBox11 As TextBox
    Friend WithEvents TextBox10 As TextBox
    Friend WithEvents ComboBox2 As ComboBox
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents GroupBox6 As GroupBox
    Friend WithEvents CurrencyDGV As DataGridView
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents BranchesInfoDGV As DataGridView

End Class
