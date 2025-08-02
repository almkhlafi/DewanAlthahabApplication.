<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class DocumentUpdateForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        GroupBox1 = New GroupBox()
        ViewFileButton = New Button()
        BrowseFileButton = New Button()
        FilePathTextBox = New TextBox()
        Label1 = New Label()
        GroupBox2 = New GroupBox()
        AttachmentComboBox = New ComboBox()
        Label4 = New Label()
        ExpireDatePicker = New DateTimePicker()
        Label3 = New Label()
        UploadDatePicker = New DateTimePicker()
        Label2 = New Label()
        SaveButton = New Button()
        CancelButton = New Button()
        GroupBox1.SuspendLayout()
        GroupBox2.SuspendLayout()
        SuspendLayout()
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Controls.Add(ViewFileButton)
        GroupBox1.Controls.Add(BrowseFileButton)
        GroupBox1.Controls.Add(FilePathTextBox)
        GroupBox1.Controls.Add(Label1)
        GroupBox1.Font = New Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        GroupBox1.Location = New Point(16, 18)
        GroupBox1.Margin = New Padding(4, 5, 4, 5)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Padding = New Padding(4, 5, 4, 5)
        GroupBox1.RightToLeft = RightToLeft.Yes
        GroupBox1.Size = New Size(747, 154)
        GroupBox1.TabIndex = 0
        GroupBox1.TabStop = False
        GroupBox1.Text = "معلومات الملف"
        ' 
        ' ViewFileButton
        ' 
        ViewFileButton.Location = New Point(30, 98)
        ViewFileButton.Margin = New Padding(4, 5, 4, 5)
        ViewFileButton.Name = "ViewFileButton"
        ViewFileButton.Size = New Size(133, 46)
        ViewFileButton.TabIndex = 3
        ViewFileButton.Text = "عرض الملف"
        ViewFileButton.UseVisualStyleBackColor = True
        ' 
        ' BrowseFileButton
        ' 
        BrowseFileButton.Location = New Point(30, 38)
        BrowseFileButton.Margin = New Padding(4, 5, 4, 5)
        BrowseFileButton.Name = "BrowseFileButton"
        BrowseFileButton.Size = New Size(133, 46)
        BrowseFileButton.TabIndex = 2
        BrowseFileButton.Text = "استعراض..."
        BrowseFileButton.UseVisualStyleBackColor = True
        ' 
        ' FilePathTextBox
        ' 
        FilePathTextBox.Location = New Point(200, 48)
        FilePathTextBox.Margin = New Padding(4, 5, 4, 5)
        FilePathTextBox.Name = "FilePathTextBox"
        FilePathTextBox.Size = New Size(425, 26)
        FilePathTextBox.TabIndex = 1
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(640, 51)
        Label1.Margin = New Padding(4, 0, 4, 0)
        Label1.Name = "Label1"
        Label1.Size = New Size(79, 20)
        Label1.TabIndex = 0
        Label1.Text = "مسار الملف:"
        ' 
        ' GroupBox2
        ' 
        GroupBox2.Controls.Add(AttachmentComboBox)
        GroupBox2.Controls.Add(Label4)
        GroupBox2.Controls.Add(ExpireDatePicker)
        GroupBox2.Controls.Add(Label3)
        GroupBox2.Controls.Add(UploadDatePicker)
        GroupBox2.Controls.Add(Label2)
        GroupBox2.Font = New Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        GroupBox2.Location = New Point(16, 200)
        GroupBox2.Margin = New Padding(4, 5, 4, 5)
        GroupBox2.Name = "GroupBox2"
        GroupBox2.Padding = New Padding(4, 5, 4, 5)
        GroupBox2.RightToLeft = RightToLeft.Yes
        GroupBox2.Size = New Size(747, 231)
        GroupBox2.TabIndex = 1
        GroupBox2.TabStop = False
        GroupBox2.Text = "معلومات الوثيقة"
        ' 
        ' AttachmentComboBox
        ' 
        AttachmentComboBox.DropDownStyle = ComboBoxStyle.DropDownList
        AttachmentComboBox.FormattingEnabled = True
        AttachmentComboBox.Location = New Point(200, 162)
        AttachmentComboBox.Margin = New Padding(4, 5, 4, 5)
        AttachmentComboBox.Name = "AttachmentComboBox"
        AttachmentComboBox.Size = New Size(399, 28)
        AttachmentComboBox.TabIndex = 5
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(640, 166)
        Label4.Margin = New Padding(4, 0, 4, 0)
        Label4.Name = "Label4"
        Label4.Size = New Size(52, 20)
        Label4.TabIndex = 4
        Label4.Text = "المرفق:"
        ' 
        ' ExpireDatePicker
        ' 
        ExpireDatePicker.Format = DateTimePickerFormat.Short
        ExpireDatePicker.Location = New Point(271, 109)
        ExpireDatePicker.Margin = New Padding(4, 5, 4, 5)
        ExpireDatePicker.Name = "ExpireDatePicker"
        ExpireDatePicker.Size = New Size(265, 26)
        ExpireDatePicker.TabIndex = 3
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(600, 115)
        Label3.Margin = New Padding(4, 0, 4, 0)
        Label3.Name = "Label3"
        Label3.Size = New Size(87, 20)
        Label3.TabIndex = 2
        Label3.Text = "تاريخ الانتهاء:"
        ' 
        ' UploadDatePicker
        ' 
        UploadDatePicker.Format = DateTimePickerFormat.Short
        UploadDatePicker.Location = New Point(271, 54)
        UploadDatePicker.Margin = New Padding(4, 5, 4, 5)
        UploadDatePicker.Name = "UploadDatePicker"
        UploadDatePicker.Size = New Size(265, 26)
        UploadDatePicker.TabIndex = 1
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(600, 60)
        Label2.Margin = New Padding(4, 0, 4, 0)
        Label2.Name = "Label2"
        Label2.Size = New Size(77, 20)
        Label2.TabIndex = 0
        Label2.Text = "تاريخ الرفع:"
        ' 
        ' SaveButton
        ' 
        SaveButton.BackColor = Color.FromArgb(CByte(0), CByte(122), CByte(204))
        SaveButton.FlatStyle = FlatStyle.Flat
        SaveButton.Font = New Font("Microsoft Sans Serif", 10F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        SaveButton.ForeColor = Color.White
        SaveButton.Location = New Point(600, 462)
        SaveButton.Margin = New Padding(4, 5, 4, 5)
        SaveButton.Name = "SaveButton"
        SaveButton.Size = New Size(160, 62)
        SaveButton.TabIndex = 2
        SaveButton.Text = "حفظ التحديث"
        SaveButton.UseVisualStyleBackColor = False
        ' 
        ' CancelButton
        ' 
        CancelButton.BackColor = Color.FromArgb(CByte(220), CByte(53), CByte(69))
        CancelButton.FlatStyle = FlatStyle.Flat
        CancelButton.Font = New Font("Microsoft Sans Serif", 10F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        CancelButton.ForeColor = Color.White
        CancelButton.Location = New Point(427, 462)
        CancelButton.Margin = New Padding(4, 5, 4, 5)
        CancelButton.Name = "CancelButton"
        CancelButton.Size = New Size(160, 62)
        CancelButton.TabIndex = 3
        CancelButton.Text = "إلغاء"
        CancelButton.UseVisualStyleBackColor = False
        ' 
        ' DocumentUpdateForm
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(779, 555)
        Controls.Add(CancelButton)
        Controls.Add(SaveButton)
        Controls.Add(GroupBox2)
        Controls.Add(GroupBox1)
        FormBorderStyle = FormBorderStyle.FixedDialog
        Margin = New Padding(4, 5, 4, 5)
        MaximizeBox = False
        MinimizeBox = False
        Name = "DocumentUpdateForm"
        RightToLeft = RightToLeft.Yes
        RightToLeftLayout = True
        StartPosition = FormStartPosition.CenterParent
        Text = "تحديث الوثيقة"
        GroupBox1.ResumeLayout(False)
        GroupBox1.PerformLayout()
        GroupBox2.ResumeLayout(False)
        GroupBox2.PerformLayout()
        ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents ViewFileButton As Button
    Friend WithEvents BrowseFileButton As Button
    Friend WithEvents FilePathTextBox As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents AttachmentComboBox As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents ExpireDatePicker As DateTimePicker
    Friend WithEvents Label3 As Label
    Friend WithEvents UploadDatePicker As DateTimePicker
    Friend WithEvents Label2 As Label
    Friend WithEvents SaveButton As Button
    Friend WithEvents CancelButton As Button
End Class