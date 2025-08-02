Public Class DocumentUpdateForm

    Private dbConn As New DBconnections()
    Public Property DocumentId As Integer
    Public Property UpdateSuccess As Boolean = False
    Private selectedFileContent As Byte() = Nothing
    Private originalFilePath As String = ""

    Private Sub DocumentUpdateForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadAttachments()
        If DocumentId > 0 Then
            LoadDocumentData()
            ' Show that we're in edit mode - file update is optional
            BrowseFileButton.Text = "تغيير الملف (اختياري)"
        Else
            BrowseFileButton.Text = "استعراض..."
        End If
    End Sub

    Private Sub LoadAttachments()
        ' Load attachments into ComboBox
        Try
            Dim dt As DataTable = dbConn.GetAttachmnts()
            AttachmentComboBox.DataSource = dt
            AttachmentComboBox.DisplayMember = "Name"
            AttachmentComboBox.ValueMember = "Id"
        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل المرفقات: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadDocumentData()
        ' Load existing document data for editing
        Try
            Dim dt As DataTable = dbConn.GetDocuments()
            Dim documentRow = dt.Select($"Id = {DocumentId}").FirstOrDefault()

            If documentRow IsNot Nothing Then
                originalFilePath = documentRow("FilePath").ToString()
                FilePathTextBox.Text = originalFilePath
                UploadDatePicker.Value = Convert.ToDateTime(documentRow("UploadingDate"))
                ExpireDatePicker.Value = Convert.ToDateTime(documentRow("ExpireDate"))
                AttachmentComboBox.SelectedValue = Convert.ToInt32(documentRow("Attachment_ID"))
            End If
        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل بيانات الوثيقة: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BrowseFileButton_Click(sender As Object, e As EventArgs) Handles BrowseFileButton.Click
        ' Open file dialog to select document
        Dim openFileDialog As New OpenFileDialog()
        openFileDialog.Filter = "All Files (*.*)|*.*|PDF Files (*.pdf)|*.pdf|Word Documents (*.docx;*.doc)|*.docx;*.doc|Excel Files (*.xlsx;*.xls)|*.xlsx;*.xls|Image Files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp"
        openFileDialog.FilterIndex = 1
        openFileDialog.Title = "اختر الوثيقة"

        If openFileDialog.ShowDialog() = DialogResult.OK Then
            FilePathTextBox.Text = openFileDialog.FileName
            ' Read file content into memory
            Try
                selectedFileContent = System.IO.File.ReadAllBytes(openFileDialog.FileName)
                BrowseFileButton.Text = "تم اختيار ملف جديد"
                BrowseFileButton.BackColor = Color.Green
                MessageBox.Show($"تم تحميل الملف الجديد بنجاح ({selectedFileContent.Length:N0} بايت)", "تم التحميل", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("خطأ في قراءة الملف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                selectedFileContent = Nothing
                BrowseFileButton.Text = "استعراض..."
                BrowseFileButton.BackColor = SystemColors.Control
            End Try
        End If
    End Sub

    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click
        ' Basic validation - only check essential fields
        If AttachmentComboBox.SelectedValue Is Nothing Then
            MessageBox.Show("يرجى اختيار المرفق", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            AttachmentComboBox.Focus()
            Return
        End If

        If ExpireDatePicker.Value <= UploadDatePicker.Value Then
            MessageBox.Show("تاريخ الانتهاء يجب أن يكون بعد تاريخ الرفع", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            ExpireDatePicker.Focus()
            Return
        End If

        ' Determine what to update
        Dim filePathToUpdate As String = FilePathTextBox.Text.Trim()
        If String.IsNullOrWhiteSpace(filePathToUpdate) Then
            filePathToUpdate = originalFilePath ' Keep original file path if empty
        End If

        ' Update document
        Try
            Dim success As Boolean = dbConn.UpdateDocument(
                DocumentId,
                filePathToUpdate,
                UploadDatePicker.Value,
                ExpireDatePicker.Value,
                Convert.ToInt32(AttachmentComboBox.SelectedValue),
                selectedFileContent ' This can be Nothing - method will handle it
            )

            If success Then
                ' Show what was updated
                Dim updatedFields As New List(Of String)
                If selectedFileContent IsNot Nothing Then updatedFields.Add("الملف")
                updatedFields.Add("التواريخ والمرفق")

                Dim message As String = "تم تحديث الوثيقة بنجاح"
                If updatedFields.Count > 0 Then
                    message &= " (" & String.Join("، ", updatedFields) & ")"
                End If

                MessageBox.Show(message, "نجح التحديث", MessageBoxButtons.OK, MessageBoxIcon.Information)
                UpdateSuccess = True
                Me.Close()
            Else
                MessageBox.Show("فشل في تحديث الوثيقة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MessageBox.Show("خطأ في تحديث الوثيقة: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        Me.Close()
    End Sub

    Private Sub ViewFileButton_Click(sender As Object, e As EventArgs) Handles ViewFileButton.Click
        ' Preview the file - either from file system or from database
        Try
            Dim fileContentToView As Byte() = Nothing
            Dim fileName As String = ""

            ' Check if we have newly selected file content
            If selectedFileContent IsNot Nothing AndAlso selectedFileContent.Length > 0 Then
                fileContentToView = selectedFileContent
                fileName = System.IO.Path.GetFileName(FilePathTextBox.Text)
            ElseIf DocumentId > 0 Then
                ' Try to get file content from database
                fileContentToView = dbConn.GetDocumentFileContent(DocumentId)
                fileName = System.IO.Path.GetFileName(originalFilePath)
            End If

            If fileContentToView IsNot Nothing AndAlso fileContentToView.Length > 0 Then
                ' Create temporary file and open it
                ViewFileFromDatabase(fileContentToView, fileName)
            ElseIf Not String.IsNullOrWhiteSpace(FilePathTextBox.Text) AndAlso System.IO.File.Exists(FilePathTextBox.Text) Then
                ' Fallback to file system
                System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo() With {
                    .FileName = FilePathTextBox.Text,
                    .UseShellExecute = True
                })
            Else
                MessageBox.Show("لا يوجد ملف للعرض", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            MessageBox.Show("خطأ في عرض الملف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ViewFileFromDatabase(fileContent As Byte(), fileName As String)
        Try
            ' Create temp directory if not exists
            Dim tempDir As String = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "DocumentViewer")
            If Not System.IO.Directory.Exists(tempDir) Then
                System.IO.Directory.CreateDirectory(tempDir)
            End If

            ' Create temporary file
            Dim tempFilePath As String = System.IO.Path.Combine(tempDir, fileName)
            System.IO.File.WriteAllBytes(tempFilePath, fileContent)

            ' Open with default application
            System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo() With {
                .FileName = tempFilePath,
                .UseShellExecute = True
            })
        Catch ex As Exception
            MessageBox.Show("خطأ في إنشاء الملف المؤقت: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Add method to reset file selection and keep original file
    Private Sub ResetFileSelection()
        selectedFileContent = Nothing
        FilePathTextBox.Text = originalFilePath
        BrowseFileButton.Text = "تغيير الملف (اختياري)"
        BrowseFileButton.BackColor = SystemColors.Control
    End Sub

    ' Handle double-click on browse button to reset
    Private Sub BrowseFileButton_DoubleClick(sender As Object, e As EventArgs) Handles BrowseFileButton.DoubleClick
        If DocumentId > 0 AndAlso selectedFileContent IsNot Nothing Then
            Dim result As DialogResult = MessageBox.Show("هل تريد الاحتفاظ بالملف الأصلي؟", "إعادة تعيين", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = DialogResult.Yes Then
                ResetFileSelection()
                MessageBox.Show("تم إعادة تعيين الملف للأصلي", "تم الإعادة", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub
End Class