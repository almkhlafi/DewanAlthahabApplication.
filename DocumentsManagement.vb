Public Class DocumentsManagement

    Private dbConn As New DBconnections()
    Public Property SelectedCustomerId As Integer = 0
    Public Property SelectedCustomerName As String = ""
    Private currentDecryptionPassword As String = Nothing

    Private Sub DocumentsManagement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupDataGridView()
        LoadDocumentsData()


        ' label1.Parent = this
        ExpiredLB.BackColor = Color.Transparent
        AboutToExpireLB.BackColor = Color.Transparent

        ' Set form title based on customer
        If SelectedCustomerId = 0 Then
            ' Show all customers
            If String.IsNullOrEmpty(SelectedCustomerName) Then
                SelectedCustomerName = "جميع العملاء"
            End If
        ElseIf String.IsNullOrEmpty(SelectedCustomerName) Then
            SelectedCustomerName = dbConn.GetCustomerName(SelectedCustomerId)
        End If
        Me.Text = $"إدارة الوثائق - العميل: {SelectedCustomerName}"

        ' Update button notification counts
        UpdateButtonNotificationCounts()

        ' All UI elements remain visible - no hiding based on customer selection mode
    End Sub

    Private Sub SetupDataGridView()
        DocumnetsDGV.Columns.Clear()
        DocumnetsDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DocumnetsDGV.MultiSelect = False
        DocumnetsDGV.ReadOnly = True
        DocumnetsDGV.AllowUserToAddRows = False
        DocumnetsDGV.AllowUserToDeleteRows = False
        DocumnetsDGV.RowHeadersVisible = False
        DocumnetsDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        ' Increase row height for better readability
        DocumnetsDGV.RowTemplate.Height = 40
        DocumnetsDGV.DefaultCellStyle.Padding = New Padding(5, 8, 5, 8)

        ' إضافة الأعمدة
        DocumnetsDGV.Columns.Add("Id", "الرقم")
        DocumnetsDGV.Columns.Add("CustomerName", "اسم العميل")
        DocumnetsDGV.Columns.Add("AttachmentName", "اسم المرفق")
        DocumnetsDGV.Columns.Add("FilePath", "مسار الملف")
        DocumnetsDGV.Columns.Add("UploadingDate", "تاريخ الرفع")
        DocumnetsDGV.Columns.Add("ExpireDate", "تاريخ الانتهاء")

        ' إضافة عمود أيقونة للعرض
        Dim iconColumn As New DataGridViewImageColumn()
        iconColumn.Name = "ViewFile"
        iconColumn.HeaderText = "عرض الملف"
        iconColumn.Width = 60
        iconColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        'iconColumn.DefaultCellStyle.BackColor = Color.FromArgb(52, 152, 219)
        'iconColumn.DefaultCellStyle.SelectionBackColor = Color.FromArgb(41, 128, 185)

        ' Try to get the same image resource as AddAttachmentsBT
        Try
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Customers))
            Dim attachmentIcon As Image = CType(resources.GetObject("AddAttachmentsBT.Image"), Image)
            If attachmentIcon IsNot Nothing Then
                ' Resize icon to fit in cell
                Dim resizedIcon As New Bitmap(attachmentIcon, New Size(24, 24))
                iconColumn.Image = resizedIcon
                iconColumn.DefaultCellStyle.NullValue = resizedIcon
            Else
                ' Create a simple attachment icon if resource not found
                iconColumn.Image = CreateAttachmentIcon()
                iconColumn.DefaultCellStyle.NullValue = CreateAttachmentIcon()
            End If
        Catch ex As Exception
            ' Create a simple attachment icon if loading fails
            iconColumn.Image = CreateAttachmentIcon()
            iconColumn.DefaultCellStyle.NullValue = CreateAttachmentIcon()
        End Try

        DocumnetsDGV.Columns.Add(iconColumn)

        ' تنسيق أعمدة التاريخ
        DocumnetsDGV.Columns("UploadingDate").DefaultCellStyle.Format = "dd/MM/yyyy"
        DocumnetsDGV.Columns("ExpireDate").DefaultCellStyle.Format = "dd/MM/yyyy"

        ' إخفاء عمود الرقم أو جعله أصغر
        DocumnetsDGV.Columns("Id").Width = 50
    End Sub

    Private Sub LoadDocumentsData(Optional filterType As String = "All")
        Dim dt As DataTable

        Select Case filterType
            Case "AboutToExpire"
                If SelectedCustomerId = 0 Then
                    ' Show all customers' about to expire documents
                    dt = dbConn.GetAboutToExpireDocumentsWithCustomerNames()
                Else
                    dt = dbConn.GetAboutToExpireDocumentsByCustomer(SelectedCustomerId)
                End If
            Case "Expired"
                If SelectedCustomerId = 0 Then
                    ' Show all customers' expired documents
                    dt = dbConn.GetExpiredDocumentsWithCustomerNames()
                Else
                    dt = dbConn.GetExpiredDocumentsByCustomer(SelectedCustomerId)
                End If
            Case Else
                If SelectedCustomerId = 0 Then
                    ' Show all documents for all customers
                    dt = dbConn.GetDocumentsWithCustomerNames()
                Else
                    ' Load customer-specific documents
                    dt = dbConn.GetDocumentsByCustomer(SelectedCustomerId)
                End If
        End Select

        DocumnetsDGV.Rows.Clear()

        For Each row As DataRow In dt.Rows
            Dim customerName As String = ""
            If SelectedCustomerId = 0 Then
                ' Show customer name when displaying all customers
                customerName = If(row.Table.Columns.Contains("CustomerName"), row("CustomerName").ToString(), "")
            End If

            ' Add the row
            Dim rowIndex As Integer = DocumnetsDGV.Rows.Add(
                row("Id"),
                customerName,
                row("AttachmentName"),
                row("FilePath"),
                Convert.ToDateTime(row("UploadingDate")).ToString("dd/MM/yyyy"),
                Convert.ToDateTime(row("ExpireDate")).ToString("dd/MM/yyyy"),
                GetViewFileIcon() ' Add icon to each row
            )

            ' Color the row based on expiration status
            ColorRowByExpirationStatus(DocumnetsDGV.Rows(rowIndex), Convert.ToDateTime(row("ExpireDate")))
        Next

        ' Show/hide customer name column based on selection mode
        If SelectedCustomerId = 0 Then
            ' Show customer name column when displaying all customers
            DocumnetsDGV.Columns("CustomerName").Visible = True
        Else
            ' Hide customer name column when displaying single customer
            DocumnetsDGV.Columns("CustomerName").Visible = False
        End If

        ' Prevent auto-selection after loading data
        DocumnetsDGV.ClearSelection()

        ' Update button notification counts
        UpdateButtonNotificationCounts()

        ' Update form title based on filter
        Dim baseTitle As String = $"إدارة الوثائق - العميل: {SelectedCustomerName}"

        Select Case filterType
            Case "AboutToExpire"
                Me.Text = baseTitle & " - الوثائق المنتهية الصلاحية قريباً"
            Case "Expired"
                Me.Text = baseTitle & " - الوثائق المنتهية الصلاحية"
            Case Else
                Me.Text = baseTitle
        End Select
    End Sub

    Private Sub DocumnetsDGV_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DocumnetsDGV.CellContentClick
        ' التحقق من النقر على عمود الزر
        If e.ColumnIndex = DocumnetsDGV.Columns("ViewFile").Index AndAlso e.RowIndex >= 0 Then
            Dim filePath As String = DocumnetsDGV.Rows(e.RowIndex).Cells("FilePath").Value.ToString()
            OpenDocument(filePath)
        End If
    End Sub

    Private Sub OpenDocument(filePath As String)
        Try
            ' Always require authentication before opening any PDF
            If Not AuthenticateUserForPDFAccess() Then
                Return
            End If

            ' Get document ID from selected row for database retrieval
            Dim selectedRow As DataGridViewRow = DocumnetsDGV.SelectedRows(0)
            Dim documentId As Integer = Convert.ToInt32(selectedRow.Cells("Id").Value)

            ' Try to get file content from database first
            Dim fileContent As Byte() = dbConn.GetDocumentFileContent(documentId)

            If fileContent IsNot Nothing AndAlso fileContent.Length > 0 Then
                ' Check if document is password protected (for decryption)
                If IsDocumentPasswordProtected(fileContent) Then
                    ' Decrypt the document using stored password
                    fileContent = DecryptDocument(fileContent)
                End If

                ' Open file from database
                OpenDocumentFromDatabase(fileContent, System.IO.Path.GetFileName(filePath))
            ElseIf System.IO.File.Exists(filePath) Then
                ' Fallback to file system if database doesn't have content
                System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo() With {
                    .FileName = filePath,
                    .UseShellExecute = True
                })
            Else
                MessageBox.Show("الملف غير موجود في قاعدة البيانات أو في المسار المحدد: " & filePath, "ملف غير موجود", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            MessageBox.Show("خطأ في فتح الملف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub OpenDocumentFromDatabase(fileContent As Byte(), fileName As String)
        Try
            ' Create temp directory if not exists
            Dim tempDir As String = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "DocumentViewer")
            If Not System.IO.Directory.Exists(tempDir) Then
                System.IO.Directory.CreateDirectory(tempDir)
            End If

            ' Create temporary file with safe filename (no Arabic characters)
            Dim timestamp As String = DateTime.Now.ToString("yyyyMMdd_HHmmss")
            Dim fileExt As String = ".pdf" ' Default to PDF for scanned documents

            ' Detect file type from content if possible
            If fileContent.Length > 20 Then
                ' Check for PDF signature (both single and multi-page PDFs)
                If fileContent(0) = &H25 AndAlso fileContent(1) = &H50 AndAlso fileContent(2) = &H44 AndAlso fileContent(3) = &H46 Then
                    fileExt = ".pdf"
                    System.Diagnostics.Debug.WriteLine("Detected PDF format from signature")
                ElseIf System.Text.Encoding.ASCII.GetString(fileContent, 0, Math.Min(fileContent.Length, 15)).StartsWith("SCANNED_PDF_DOC") Then
                    fileExt = ".pdf" ' Legacy custom multi-page format
                    System.Diagnostics.Debug.WriteLine("Detected legacy custom PDF format")
                ElseIf System.Text.Encoding.ASCII.GetString(fileContent, 0, Math.Min(fileContent.Length, 17)).StartsWith("MULTI_SCAN_DOC_V1") Then
                    ' Our new custom multi-image format - extract and view as separate images
                    System.Diagnostics.Debug.WriteLine("Detected custom multi-image format")
                    ExtractAndViewMultiImageDocument(fileContent, fileName)
                    Return
                ElseIf System.Text.Encoding.ASCII.GetString(fileContent, 0, Math.Min(fileContent.Length, 19)).StartsWith("INDIVIDUAL_IMAGES_V1") Then
                    ' Our individual images format - extract and view as separate images
                    System.Diagnostics.Debug.WriteLine("Detected custom individual images format")
                    ExtractAndViewIndividualImagesDocument(fileContent, fileName)
                    Return
                Else
                    ' Default to PDF for scanned documents (assume all scans are now PDFs)
                    fileExt = ".pdf"
                    System.Diagnostics.Debug.WriteLine("Defaulting to PDF format for scanned document")
                End If
            End If

            ' Try to get original extension if available
            If Not String.IsNullOrEmpty(fileName) AndAlso fileName.Contains(".") Then
                Try
                    Dim originalExt As String = System.IO.Path.GetExtension(fileName)
                    If Not String.IsNullOrEmpty(originalExt) Then
                        fileExt = originalExt
                    End If
                Catch
                    ' Keep detected extension
                End Try
            End If

            ' Use safe filename without Arabic characters
            Dim tempFileName As String = $"Document_{timestamp}{fileExt}"
            Dim tempFilePath As String = System.IO.Path.Combine(tempDir, tempFileName)

            ' Write file content to temp file
            System.IO.File.WriteAllBytes(tempFilePath, fileContent)

            ' Verify file was created successfully
            If System.IO.File.Exists(tempFilePath) Then
                ' Open with default application
                System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo() With {
                    .FileName = tempFilePath,
                    .UseShellExecute = True
                })
            Else
                MessageBox.Show("فشل في إنشاء الملف المؤقت", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Catch ex As Exception
            MessageBox.Show("خطأ في إنشاء الملف المؤقت: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SacnDocumentBT_Click(sender As Object, e As EventArgs) Handles SacnDocumentBT.Click
        ' Authenticate user first
        If Not AuthenticateUserForPDFAccess() Then
            Return
        End If

        ' Check if a document is selected first
        If DocumnetsDGV.SelectedRows.Count = 0 Then
            MessageBox.Show("يرجى تحديد وثيقة لمسحها ضوئياً", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Check for connected scanners first
        Dim connectedScanners As List(Of String) = GetConnectedScanners()

        If connectedScanners.Count = 0 Then
            MessageBox.Show("لا يوجد ماسح ضوئي متصل" & vbCrLf & "يرجى التأكد من توصيل الماسح الضوئي وتشغيله.", "لا يوجد ماسح ضوئي", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        Else
            ' Show connected scanners
            Dim scannersMessage As String = "الماسحات الضوئية المتصلة:" & vbCrLf
            For i As Integer = 0 To connectedScanners.Count - 1
                scannersMessage += $"{i + 1}. {connectedScanners(i)}" & vbCrLf
            Next
            scannersMessage += vbCrLf & "سيتم استخدام الماسح الأول للمسح الضوئي."

            Dim result As DialogResult = MessageBox.Show(scannersMessage, "الماسحات الضوئية المتاحة", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            If result = DialogResult.Cancel Then
                Return
            End If
        End If

        Try
            ' Get selected document info
            Dim selectedRow = DocumnetsDGV.SelectedRows(0)
            Dim documentId = Convert.ToInt32(selectedRow.Cells("Id").Value)
            Dim attachmentName = selectedRow.Cells("AttachmentName").Value.ToString()

            ' Use fixed password or no password (depending on your logic)
            Dim selectedUsername As String = Nothing ' Or "admin123" if your logic needs a fixed one

            ' Start scanning process
            Dim scannedDocument As Byte() = ScanDocument(selectedUsername)

            If scannedDocument IsNot Nothing AndAlso scannedDocument.Length > 0 Then
                System.Diagnostics.Debug.WriteLine($"Scanned document size: {scannedDocument.Length} bytes")

                ' Check if it's a PDF (should start with %PDF)
                If scannedDocument.Length > 4 Then
                    Dim pdfCheck As String = System.Text.Encoding.ASCII.GetString(scannedDocument, 0, 4)
                    System.Diagnostics.Debug.WriteLine($"Document header: {pdfCheck}")
                End If

                ' Update the document with scanned content
                Dim success As Boolean = dbConn.UpdateDocumentWithScannedContent(documentId, scannedDocument)

                If success Then
                    MessageBox.Show($"تم مسح الوثيقة '{attachmentName}' وحفظها بنجاح كملف PDF ({scannedDocument.Length:N0} بايت)", "نجح المسح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    ' Refresh the view
                    LoadDocumentsData()
                Else
                    MessageBox.Show("فشل في حفظ الوثيقة الممسوحة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Else
                MessageBox.Show("فشل في إنشاء وثيقة ممسوحة صالحة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                System.Diagnostics.Debug.WriteLine($"Scanned document is null or empty: {scannedDocument?.Length}")
            End If

        Catch ex As Exception
            MessageBox.Show("خطأ في عملية المسح الضوئي: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Get list of connected scanners
    Private Function GetConnectedScanners() As List(Of String)
        Dim scanners As New List(Of String)()

        Try
            ' Create WIA DeviceManager
            Dim deviceManager As Object = CreateObject("WIA.DeviceManager")
            Dim deviceInfos As Object = deviceManager.DeviceInfos

            System.Diagnostics.Debug.WriteLine($"Found {deviceInfos.Count} WIA devices")

            ' Check each device
            For i As Integer = 1 To deviceInfos.Count
                Try
                    Dim deviceInfo As Object = deviceInfos(i)
                    Dim deviceType As Integer = CInt(deviceInfo.Properties("Type").Value)

                    ' Check if it's a scanner (Type 1 = Scanner, Type 2 = Camera)
                    If deviceType = 1 Then
                        Dim deviceName As String = deviceInfo.Properties("Name").Value.ToString()
                        Dim deviceDescription As String = ""

                        Try
                            deviceDescription = deviceInfo.Properties("Description").Value.ToString()
                        Catch
                            deviceDescription = "ماسح ضوئي"
                        End Try

                        scanners.Add($"{deviceName} - {deviceDescription}")
                        System.Diagnostics.Debug.WriteLine($"Found scanner: {deviceName}")
                    End If

                Catch ex As Exception
                    System.Diagnostics.Debug.WriteLine($"Error checking device {i}: {ex.Message}")
                End Try
            Next

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"Error getting scanner list: {ex.Message}")
            ' If WIA is not available, return empty list
        End Try

        Return scanners
    End Function

    ' مسح الوثائق ضوئياً باستخدام WIA
    Private Function ScanDocument(Optional username As String = Nothing) As Byte()
        Try
            ' Create WIA objects
            Dim deviceManager As Object = CreateObject("WIA.DeviceManager")
            Dim device As Object = Nothing

            ' Get available devices
            Dim deviceInfos As Object = deviceManager.DeviceInfos

            If deviceInfos.Count = 0 Then
                MessageBox.Show("لم يتم العثور على أجهزة مسح ضوئي متصلة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return Nothing
            End If

            ' Use the first available scanner
            device = deviceInfos(1).Connect()

            ' Configure scanner settings
            Dim item As Object = device.Items(1)

            ' Set scan properties with more conservative approach
            Try
                ' Try to set basic properties first
                ' Set format to BMP (most universally supported)
                SetScanProperty(item, 6153, "{B96B3CAB-0728-11D3-9D7B-0000F81EF32E}") ' WIA_IPA_FORMAT - BMP

                ' Set color mode to 24-bit RGB (more compatible than color intent)
                SetScanProperty(item, 6146, 1) ' WIA_IPS_CUR_INTENT - Text/Photo

                ' Set bit depth
                SetScanProperty(item, 6150, 24) ' WIA_IPA_BITS_PER_PIXEL - 24 bit color

                ' Set moderate resolution (150 DPI for reliability)
                SetScanProperty(item, 6147, 150) ' WIA_IPS_XRES
                SetScanProperty(item, 6148, 150) ' WIA_IPS_YRES

                ' Set A4 page size (210mm x 297mm)
                SetScanProperty(item, 6151, 8268) ' WIA_IPS_XEXTENT (A4 width: 210mm = 8.268 inches = 8268/1000)
                SetScanProperty(item, 6152, 11693) ' WIA_IPS_YEXTENT (A4 height: 297mm = 11.693 inches = 11693/1000)

                System.Diagnostics.Debug.WriteLine("Scanner properties configured successfully")

            Catch ex As Exception
                ' Continue with default settings if property setting fails
                System.Diagnostics.Debug.WriteLine($"Scanner property setting failed: {ex.Message}")

                ' Try minimal configuration
                Try
                    SetScanProperty(item, 6153, "{B96B3CAB-0728-11D3-9D7B-0000F81EF32E}") ' Force BMP format
                Catch
                    ' Use scanner defaults
                End Try
            End Try

            ' Scan all pages
            Dim allPagesData As New List(Of Byte())
            Dim pageNumber As Integer = 1
            Dim hasMorePages As Boolean = True
            Dim maxPages As Integer = 50 ' Safety limit to prevent infinite loops
            Dim consecutiveEmptyPages As Integer = 0

            ' Show progress message
            Dim progressForm As New Form()
            Dim progressLabel As New Label()
            progressLabel.Text = "جاري المسح الضوئي..."
            progressLabel.Dock = DockStyle.Fill
            progressLabel.TextAlign = ContentAlignment.MiddleCenter
            progressForm.Controls.Add(progressLabel)
            progressForm.Text = "مسح ضوئي"
            progressForm.Size = New Size(300, 100)
            progressForm.StartPosition = FormStartPosition.CenterParent
            progressForm.Show()
            Application.DoEvents()

            While hasMorePages AndAlso pageNumber <= maxPages AndAlso consecutiveEmptyPages < 3
                Try
                    progressLabel.Text = $"جاري مسح الصفحة {pageNumber}..."
                    Application.DoEvents()

                    ' Add small delay to ensure scanner is ready
                    Threading.Thread.Sleep(500)

                    ' Scan the current page
                    Dim imageObject As Object = item.Transfer()

                    If imageObject IsNot Nothing Then
                        progressLabel.Text = $"جاري تحويل الصفحة {pageNumber}..."
                        Application.DoEvents()

                        ' Convert image to byte array
                        Dim imageBytes As Byte() = ConvertImageToByteArray(imageObject)

                        If imageBytes IsNot Nothing AndAlso imageBytes.Length > 100 Then ' Lowered threshold
                            allPagesData.Add(imageBytes)
                            progressLabel.Text = $"تم مسح الصفحة {pageNumber} بنجاح ({imageBytes.Length:N0} بايت)"
                            Application.DoEvents()
                            pageNumber += 1
                            consecutiveEmptyPages = 0 ' Reset empty page counter

                            ' Continue scanning automatically - try to scan next page
                            If pageNumber <= maxPages Then
                                progressLabel.Text = $"جاري محاولة مسح الصفحة {pageNumber} تلقائياً..."
                                Application.DoEvents()
                                hasMorePages = True ' Continue scanning
                            Else
                                hasMorePages = False
                            End If
                        Else
                            progressLabel.Text = $"الصفحة {pageNumber} فارغة أو تالفة (الحجم: {If(imageBytes?.Length, 0)} بايت)"
                            Application.DoEvents()
                            consecutiveEmptyPages += 1

                            ' Show debug info for empty pages
                            If imageObject IsNot Nothing Then
                                Try
                                    Dim debugInfo As String = $"نوع الكائن: {imageObject.GetType().Name}"
                                    If imageObject.GetType().GetProperty("FormatID") IsNot Nothing Then
                                        debugInfo += $", تنسيق: {imageObject.FormatID}"
                                    End If
                                    System.Diagnostics.Debug.WriteLine($"Empty page debug: {debugInfo}")
                                Catch
                                    ' Ignore debug errors
                                End Try
                            End If

                            ' Stop scanning when we get empty pages - this means no more documents
                            hasMorePages = False
                        End If
                    Else
                        progressLabel.Text = $"فشل في مسح الصفحة {pageNumber}"
                        Application.DoEvents()
                        consecutiveEmptyPages += 1
                        hasMorePages = False ' Stop on scan failure
                    End If

                Catch ex As Exception
                    progressLabel.Text = $"خطأ في مسح الصفحة {pageNumber}: {ex.Message}"
                    Application.DoEvents()
                    System.Diagnostics.Debug.WriteLine($"Scan error on page {pageNumber}: {ex.Message}")
                    hasMorePages = False
                End Try
            End While

            progressForm.Close()

            ' Provide feedback about scanning results
            If pageNumber > maxPages Then
                MessageBox.Show($"تم إيقاف المسح الضوئي عند الحد الأقصى ({maxPages} صفحة). تم مسح {allPagesData.Count} صفحات بنجاح.", "تم إنهاء المسح", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ElseIf consecutiveEmptyPages >= 3 Then
                MessageBox.Show($"تم إيقاف المسح الضوئي بسبب عدة صفحات فارغة متتالية. تم مسح {allPagesData.Count} صفحات بنجاح.", "تم إنهاء المسح", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ElseIf allPagesData.Count > 1 Then
                MessageBox.Show($"تم مسح {allPagesData.Count} صفحات تلقائياً وسيتم حفظها في ملف PDF واحد.", "مسح متعدد الصفحات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show($"تم مسح صفحة واحدة وسيتم حفظها كملف PDF.", "مسح صفحة واحدة", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            ' Combine all pages into a single PDF or multi-page TIFF
            If allPagesData.Count > 0 Then
                System.Diagnostics.Debug.WriteLine($"Combining {allPagesData.Count} pages into document")
                For i As Integer = 0 To allPagesData.Count - 1
                    System.Diagnostics.Debug.WriteLine($"Page {i + 1}: {allPagesData(i).Length} bytes")
                Next

                Dim combinedDocument As Byte() = CombinePagesIntoDocument(allPagesData, username)
                If combinedDocument IsNot Nothing Then
                    System.Diagnostics.Debug.WriteLine($"Final combined document: {combinedDocument.Length} bytes")
                Else
                    System.Diagnostics.Debug.WriteLine("Combined document is null!")
                End If
                Return combinedDocument
            Else
                MessageBox.Show("لم يتم مسح أي صفحات صحيحة", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return Nothing
            End If

        Catch ex As Exception
            MessageBox.Show("خطأ في الوصول إلى جهاز المسح الضوئي: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        End Try
    End Function

    ' تعيين خصائص المسح الضوئي
    Private Sub SetScanProperty(item As Object, propertyId As Integer, value As Object)
        Try
            Dim properties As Object = item.Properties
            Dim prop As Object = properties(propertyId)
            prop.Value = value
        Catch ex As Exception
            ' Ignore property setting errors
        End Try
    End Sub

    ' تحويل الصورة إلى مصفوفة بايت
    Private Function ConvertImageToByteArray(imageObject As Object) As Byte()
        Try
            System.Diagnostics.Debug.WriteLine($"Processing WIA object type: {imageObject.GetType().Name}")

            ' Method 1: Try FileData.BinaryData directly (for IImageFile interface)
            Try
                Dim fileData As Object = imageObject.FileData
                If fileData IsNot Nothing Then
                    Dim binaryData As Byte() = DirectCast(fileData.BinaryData, Byte())
                    If binaryData IsNot Nothing AndAlso binaryData.Length > 100 Then
                        System.Diagnostics.Debug.WriteLine($"Method 1 success: {binaryData.Length} bytes")
                        Return binaryData
                    End If
                End If
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Method 1 (FileData.BinaryData) failed: {ex.Message}")
            End Try

            ' Method 2: Try accessing Vector properties
            Try
                Dim vectorData As Object = imageObject.FileData
                If vectorData IsNot Nothing Then
                    ' Try Vector.BinaryData
                    Dim bytes As Byte() = DirectCast(vectorData.BinaryData, Byte())
                    If bytes IsNot Nothing AndAlso bytes.Length > 100 Then
                        System.Diagnostics.Debug.WriteLine($"Method 2 success: {bytes.Length} bytes")
                        Return bytes
                    End If
                End If
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Method 2 (Vector) failed: {ex.Message}")
            End Try

            ' Method 3: Save to temporary file approach (most reliable fallback)
            Dim tempPath As String = System.IO.Path.GetTempFileName()
            System.IO.File.Delete(tempPath) ' Remove the temp file created by GetTempFileName

            ' Try BMP format first (most compatible)
            Try
                Dim bmpPath As String = tempPath & ".bmp"
                imageObject.SaveFile(bmpPath)

                If System.IO.File.Exists(bmpPath) Then
                    Dim fileInfo As New System.IO.FileInfo(bmpPath)
                    If fileInfo.Length > 100 Then
                        Dim bytes As Byte() = System.IO.File.ReadAllBytes(bmpPath)
                        System.IO.File.Delete(bmpPath)
                        System.Diagnostics.Debug.WriteLine($"Method 3 (BMP) success: {bytes.Length} bytes")
                        Return bytes
                    End If
                    System.IO.File.Delete(bmpPath)
                End If
            Catch bmpEx As Exception
                System.Diagnostics.Debug.WriteLine($"BMP save failed: {bmpEx.Message}")
            End Try

            ' Try JPEG format
            Try
                Dim jpgPath As String = tempPath & ".jpg"
                imageObject.SaveFile(jpgPath)

                If System.IO.File.Exists(jpgPath) Then
                    Dim fileInfo As New System.IO.FileInfo(jpgPath)
                    If fileInfo.Length > 100 Then
                        Dim bytes As Byte() = System.IO.File.ReadAllBytes(jpgPath)
                        System.IO.File.Delete(jpgPath)
                        System.Diagnostics.Debug.WriteLine($"Method 3 (JPEG) success: {bytes.Length} bytes")
                        Return bytes
                    End If
                    System.IO.File.Delete(jpgPath)
                End If
            Catch jpgEx As Exception
                System.Diagnostics.Debug.WriteLine($"JPEG save failed: {jpgEx.Message}")
            End Try

            ' Try PNG format
            Try
                Dim pngPath As String = tempPath & ".png"
                imageObject.SaveFile(pngPath)

                If System.IO.File.Exists(pngPath) Then
                    Dim fileInfo As New System.IO.FileInfo(pngPath)
                    If fileInfo.Length > 100 Then
                        Dim bytes As Byte() = System.IO.File.ReadAllBytes(pngPath)
                        System.IO.File.Delete(pngPath)
                        System.Diagnostics.Debug.WriteLine($"Method 3 (PNG) success: {bytes.Length} bytes")
                        Return bytes
                    End If
                    System.IO.File.Delete(pngPath)
                End If
            Catch pngEx As Exception
                System.Diagnostics.Debug.WriteLine($"PNG save failed: {pngEx.Message}")
            End Try

            System.Diagnostics.Debug.WriteLine("All conversion methods failed")
            Return Nothing

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"ConvertImageToByteArray error: {ex.Message}")
            Return Nothing
        End Try
    End Function

    ' دمج الصفحات في وثيقة واحدة (PDF للمتعدد الصفحات)
    Private Function CombinePagesIntoDocument(pages As List(Of Byte()), Optional username As String = Nothing) As Byte()
        Try
            Dim userPassword As String = username ' Use the username as password directly

            If pages.Count = 1 Then
                ' Single page - create single page PDF
                System.Diagnostics.Debug.WriteLine("Single page document, creating single page PDF")
                Return CreatePDFFromSingleImage(pages(0), userPassword)
            Else
                ' Multiple pages - create multi-page PDF
                System.Diagnostics.Debug.WriteLine("Multiple pages detected, creating multi-page PDF")
                Return CreateProperPDFFromImages(pages, userPassword)
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"CombinePagesIntoDocument error: {ex.Message}")
            ' Fallback: Create single page PDF from first image
            Return CreatePDFFromSingleImage(pages(0))
        End Try
    End Function

    ' إنشاء ملف PDF من الصور المتعددة باستخدام نهج محسن
    Private Function CreatePDFFromImages(imagePages As List(Of Byte())) As Byte()
        Try
            ' Create temporary files for processing
            Dim tempDir As String = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "ScanPDF")
            If Not System.IO.Directory.Exists(tempDir) Then
                System.IO.Directory.CreateDirectory(tempDir)
            End If

            ' Try different PDF creation methods
            Dim pdfBytes As Byte() = Nothing

            ' Method 1: Try using multi-page TIFF (most reliable for scanned documents)
            pdfBytes = CreateMultiPageTIFF(imagePages)
            If pdfBytes IsNot Nothing Then Return pdfBytes

            ' Method 2: Try using Windows built-in approach
            pdfBytes = CreatePDFViaPowerShell(imagePages, tempDir)
            If pdfBytes IsNot Nothing Then Return pdfBytes

            ' Fallback: Return first page if all methods fail
            Return imagePages(0)

        Catch ex As Exception
            ' If all PDF creation methods fail, return first page
            Return imagePages(0)
        End Try
    End Function

    ' إنشاء PDF باستخدام PowerShell و Windows Print to PDF
    Private Function CreatePDFViaPowerShell(imagePages As List(Of Byte()), tempDir As String) As Byte()
        Try
            ' Save images to temporary files
            Dim imagePaths As New List(Of String)()
            For i As Integer = 0 To imagePages.Count - 1
                Dim tempImagePath As String = System.IO.Path.Combine(tempDir, $"scan_page_{i + 1:00}.jpg")
                System.IO.File.WriteAllBytes(tempImagePath, imagePages(i))
                imagePaths.Add(tempImagePath)
            Next

            ' Create HTML file that contains all images
            Dim htmlPath As String = System.IO.Path.Combine(tempDir, "document.html")
            Dim pdfPath As String = System.IO.Path.Combine(tempDir, "scanned_document.pdf")

            CreateHTMLFromImages(imagePaths, htmlPath)

            ' Try to convert HTML to PDF using simple concatenation approach
            If ConvertHTMLToPDFSimple(imagePaths, pdfPath) Then
                If System.IO.File.Exists(pdfPath) Then
                    Dim pdfBytes As Byte() = System.IO.File.ReadAllBytes(pdfPath)

                    ' Clean up temp files
                    CleanupTempFiles(tempDir)

                    Return pdfBytes
                End If
            End If

            ' If PDF creation fails, create a simple multi-image format
            Return CreateSimpleMultiImageDocument(imagePages)

        Catch ex As Exception
            Return CreateSimpleMultiImageDocument(imagePages)
        End Try
    End Function

    ' إنشاء ملف HTML من الصور
    Private Sub CreateHTMLFromImages(imagePaths As List(Of String), htmlPath As String)
        Try
            Using writer As New System.IO.StreamWriter(htmlPath, False, System.Text.Encoding.UTF8)
                writer.WriteLine("<!DOCTYPE html>")
                writer.WriteLine("<html><head><title>Scanned Document</title></head><body>")

                For Each imagePath In imagePaths
                    writer.WriteLine($"<div style='page-break-after: always;'>")
                    writer.WriteLine($"<img src='file:///{imagePath.Replace("\", "/")}' style='width: 100%; height: auto;' />")
                    writer.WriteLine("</div>")
                Next

                writer.WriteLine("</body></html>")
            End Using
        Catch ex As Exception
            ' Ignore HTML creation errors
        End Try
    End Sub

    ' تحويل HTML إلى PDF باستخدام نهج بسيط
    Private Function ConvertHTMLToPDFSimple(imagePaths As List(Of String), pdfPath As String) As Boolean
        Try
            ' Create a basic PDF structure manually
            ' This creates a simplified PDF-like document

            Using pdfStream As New System.IO.FileStream(pdfPath, System.IO.FileMode.Create)
                Using writer As New System.IO.BinaryWriter(pdfStream)
                    ' Write PDF header
                    Dim header As String = "%PDF-1.4" & vbLf
                    writer.Write(System.Text.Encoding.ASCII.GetBytes(header))

                    ' Write basic PDF structure for multi-page document
                    Dim catalog As String = "1 0 obj" & vbLf & "<<" & vbLf & "/Type /Catalog" & vbLf & "/Pages 2 0 R" & vbLf & ">>" & vbLf & "endobj" & vbLf
                    writer.Write(System.Text.Encoding.ASCII.GetBytes(catalog))

                    ' Write pages object
                    Dim pages As String = "2 0 obj" & vbLf & "<<" & vbLf & "/Type /Pages" & vbLf & $"/Count {imagePaths.Count}" & vbLf & "/Kids ["
                    For i As Integer = 0 To imagePaths.Count - 1
                        pages += $" {i + 3} 0 R"
                    Next
                    pages += " ]" & vbLf & ">>" & vbLf & "endobj" & vbLf
                    writer.Write(System.Text.Encoding.ASCII.GetBytes(pages))

                    ' Write individual page objects (simplified)
                    For i As Integer = 0 To imagePaths.Count - 1
                        Dim pageObj As String = $"{i + 3} 0 obj" & vbLf & "<<" & vbLf & "/Type /Page" & vbLf & "/Parent 2 0 R" & vbLf & "/MediaBox [0 0 612 792]" & vbLf & ">>" & vbLf & "endobj" & vbLf
                        writer.Write(System.Text.Encoding.ASCII.GetBytes(pageObj))
                    Next

                    ' Write basic trailer
                    Dim trailer As String = "xref" & vbLf & "trailer" & vbLf & "<<" & vbLf & "/Root 1 0 R" & vbLf & ">>" & vbLf & "%%EOF"
                    writer.Write(System.Text.Encoding.ASCII.GetBytes(trailer))
                End Using
            End Using

            Return System.IO.File.Exists(pdfPath)

        Catch ex As Exception
            Return False
        End Try
    End Function

    ' إنشاء وثيقة متعددة الصور بسيطة
    Private Function CreateSimpleMultiImageDocument(imagePages As List(Of Byte())) As Byte()
        Try
            System.Diagnostics.Debug.WriteLine($"Creating simple multi-image document with {imagePages.Count} pages")

            ' Create a container with all images
            Dim combinedDocument As New List(Of Byte)()

            ' Add custom header for multi-image document
            Dim header As String = "MULTI_SCAN_DOC_V1"
            combinedDocument.AddRange(System.Text.Encoding.UTF8.GetBytes(header))
            combinedDocument.AddRange(BitConverter.GetBytes(imagePages.Count)) ' Page count

            System.Diagnostics.Debug.WriteLine($"Header added: {header}, Page count: {imagePages.Count}")

            ' Add each image with size prefix
            For i As Integer = 0 To imagePages.Count - 1
                Dim pageSize As Integer = imagePages(i).Length
                combinedDocument.AddRange(BitConverter.GetBytes(pageSize))
                combinedDocument.AddRange(imagePages(i))
                System.Diagnostics.Debug.WriteLine($"Added page {i + 1}: {pageSize} bytes")
            Next

            Dim result As Byte() = combinedDocument.ToArray()
            System.Diagnostics.Debug.WriteLine($"Simple multi-image document created: {result.Length} bytes")
            Return result

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"CreateSimpleMultiImageDocument error: {ex.Message}")
            Return Nothing
        End Try
    End Function

    ' إنشاء وثيقة من ملفات صور منفردة
    Private Function CreateIndividualImagesDocument(imagePages As List(Of Byte())) As Byte()
        Try
            System.Diagnostics.Debug.WriteLine($"Creating individual images document with {imagePages.Count} pages")

            ' Create a ZIP-like container with individual image files
            Dim combinedDocument As New List(Of Byte)()

            ' Add header
            Dim header As String = "INDIVIDUAL_IMAGES_V1"
            combinedDocument.AddRange(System.Text.Encoding.UTF8.GetBytes(header))
            combinedDocument.AddRange(BitConverter.GetBytes(imagePages.Count)) ' Page count

            ' Add each image as a separate "file" entry
            For i As Integer = 0 To imagePages.Count - 1
                ' Add file header
                Dim fileName As String = $"page_{i + 1:00}.bmp"
                Dim fileNameBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(fileName)

                ' Add filename length and filename
                combinedDocument.AddRange(BitConverter.GetBytes(fileNameBytes.Length))
                combinedDocument.AddRange(fileNameBytes)

                ' Add file size and content
                combinedDocument.AddRange(BitConverter.GetBytes(imagePages(i).Length))
                combinedDocument.AddRange(imagePages(i))

                System.Diagnostics.Debug.WriteLine($"Added individual image {i + 1}: {fileName}, {imagePages(i).Length} bytes")
            Next

            Dim result As Byte() = combinedDocument.ToArray()
            System.Diagnostics.Debug.WriteLine($"Individual images document created: {result.Length} bytes")
            Return result

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"CreateIndividualImagesDocument error: {ex.Message}")
            Return Nothing
        End Try
    End Function

    ' تنظيف الملفات المؤقتة
    Private Sub CleanupTempFiles(tempDir As String)
        Try
            If System.IO.Directory.Exists(tempDir) Then
                System.IO.Directory.Delete(tempDir, True)
            End If
        Catch ex As Exception
            ' Ignore cleanup errors
        End Try
    End Sub

    ' إنشاء ملف TIFF متعدد الصفحات (أفضل للوثائق الممسوحة ضوئياً)
    Private Function CreateMultiPageTIFF(imagePages As List(Of Byte())) As Byte()
        Try
            System.Diagnostics.Debug.WriteLine($"Creating multi-page TIFF from {imagePages.Count} pages")
            Dim tempTiffPath As String = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"scanned_doc_{DateTime.Now:yyyyMMddHHmmss}.tiff")

            Using firstImage As System.Drawing.Image = ByteArrayToImage(imagePages(0))
                If firstImage Is Nothing Then
                    System.Diagnostics.Debug.WriteLine("Failed to create image from first page")
                    Return Nothing
                End If

                System.Diagnostics.Debug.WriteLine($"First image: {firstImage.Width}x{firstImage.Height}")

                ' Create encoder parameters for multi-page TIFF
                Dim encoderParams As New System.Drawing.Imaging.EncoderParameters(2)
                Dim encoder As System.Drawing.Imaging.ImageCodecInfo = GetEncoderInfo("image/tiff")

                If encoder Is Nothing Then
                    System.Diagnostics.Debug.WriteLine("TIFF encoder not found")
                    Return Nothing
                End If

                ' Set compression and save flag
                encoderParams.Param(0) = New System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Compression, System.Drawing.Imaging.EncoderValue.CompressionLZW)
                encoderParams.Param(1) = New System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, System.Drawing.Imaging.EncoderValue.MultiFrame)

                ' Save first page
                firstImage.Save(tempTiffPath, encoder, encoderParams)
                System.Diagnostics.Debug.WriteLine("First page saved to TIFF")

                ' Add remaining pages
                For i As Integer = 1 To imagePages.Count - 1
                    Using nextImage As System.Drawing.Image = ByteArrayToImage(imagePages(i))
                        If nextImage IsNot Nothing Then
                            System.Diagnostics.Debug.WriteLine($"Adding page {i + 1}: {nextImage.Width}x{nextImage.Height}")
                            encoderParams.Param(1) = New System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, System.Drawing.Imaging.EncoderValue.FrameDimensionPage)
                            firstImage.SaveAdd(nextImage, encoderParams)
                        Else
                            System.Diagnostics.Debug.WriteLine($"Failed to create image from page {i + 1}")
                        End If
                    End Using
                Next

                ' Finalize the multi-page TIFF
                encoderParams.Param(1) = New System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, System.Drawing.Imaging.EncoderValue.Flush)
                firstImage.SaveAdd(encoderParams)
                System.Diagnostics.Debug.WriteLine("Multi-page TIFF finalized")
            End Using

            ' Read the created TIFF file
            If System.IO.File.Exists(tempTiffPath) Then
                Dim fileInfo As New System.IO.FileInfo(tempTiffPath)
                System.Diagnostics.Debug.WriteLine($"TIFF file created: {fileInfo.Length} bytes")
                Dim tiffBytes As Byte() = System.IO.File.ReadAllBytes(tempTiffPath)
                System.IO.File.Delete(tempTiffPath) ' Clean up
                Return tiffBytes
            Else
                System.Diagnostics.Debug.WriteLine("TIFF file was not created")
            End If

            Return Nothing

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"CreateMultiPageTIFF error: {ex.Message}")
            Return Nothing
        End Try
    End Function

    ' تحويل مصفوفة البايت إلى صورة
    Private Function ByteArrayToImage(imageBytes As Byte()) As System.Drawing.Image
        Try
            If imageBytes Is Nothing OrElse imageBytes.Length = 0 Then
                System.Diagnostics.Debug.WriteLine("ByteArrayToImage: Empty or null byte array")
                Return Nothing
            End If

            System.Diagnostics.Debug.WriteLine($"ByteArrayToImage: Processing {imageBytes.Length} bytes")

            Using ms As New System.IO.MemoryStream(imageBytes)
                Dim image As System.Drawing.Image = System.Drawing.Image.FromStream(ms)
                System.Diagnostics.Debug.WriteLine($"ByteArrayToImage: Created image {image.Width}x{image.Height}")
                Return image
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"ByteArrayToImage error: {ex.Message}")
            Return Nothing
        End Try
    End Function

    ' الحصول على معلومات التشفير للصورة
    Private Function GetEncoderInfo(mimeType As String) As System.Drawing.Imaging.ImageCodecInfo
        Dim codecs As System.Drawing.Imaging.ImageCodecInfo() = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders()
        For Each codec As System.Drawing.Imaging.ImageCodecInfo In codecs
            If codec.MimeType = mimeType Then
                Return codec
            End If
        Next
        Return Nothing
    End Function

    Private Sub UpdateDocumenrBT_Click(sender As Object, e As EventArgs) Handles UpdateDocumenrBT.Click
        ' Authenticate user first
        If Not AuthenticateUserForPDFAccess() Then
            Return
        End If

        ' Check if a row is selected
        If DocumnetsDGV.SelectedRows.Count = 0 Then
            MessageBox.Show("يرجى تحديد وثيقة للتحديث", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Get selected document ID
        Dim selectedRow = DocumnetsDGV.SelectedRows(0)
        Dim documentId = Convert.ToInt32(selectedRow.Cells("Id").Value)

        ' Open the document update form
        Dim updateForm As New DocumentUpdateForm
        updateForm.DocumentId = documentId

        If updateForm.ShowDialog = DialogResult.OK OrElse updateForm.UpdateSuccess Then
            ' Refresh the current view (maintain current filter if any)
            If Text.Contains("قريباً") Then
                LoadDocumentsData("AboutToExpire")
            ElseIf Text.Contains("المنتهية الصلاحية") And Not Text.Contains("قريباً") Then
                LoadDocumentsData("Expired")
            Else
                LoadDocumentsData() ' Load all documents
            End If
        End If

        updateForm.Dispose()
    End Sub

    Private Sub AboutToExpireBT_Click(sender As Object, e As EventArgs) Handles AboutToExpireBT.Click
        ' Load documents that are about to expire (within 30 days)
        LoadDocumentsData("AboutToExpire")
    End Sub

    Private Sub ExpiredBT_Click(sender As Object, e As EventArgs) Handles ExpiredBT.Click
        ' Load documents that have already expired
        LoadDocumentsData("Expired")
    End Sub


    ' Create a simple attachment icon if resources are not available
    Private Function CreateAttachmentIcon() As Bitmap
        Dim bitmap As New Bitmap(24, 24)
        Using g As Graphics = Graphics.FromImage(bitmap)
            g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            g.Clear(Color.Transparent)

            ' Draw paperclip-like icon
            Using pen As New Pen(Color.White, 2)
                ' Draw paperclip shape
                g.DrawRectangle(pen, 6, 8, 12, 10)
                g.DrawRectangle(pen, 8, 6, 8, 14)
            End Using
        End Using
        Return bitmap
    End Function

    ' Get the view file icon (tries to use Form1 resource, falls back to custom icon)
    Private Function GetViewFileIcon() As Image
        Try
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Customers))
            Dim attachmentIcon As Image = CType(resources.GetObject("AddAttachmentsBT.Image"), Image)
            If attachmentIcon IsNot Nothing Then
                Return New Bitmap(attachmentIcon, New Size(24, 24))
            End If
        Catch ex As Exception
            ' Ignore error and use fallback
        End Try
        Return CreateAttachmentIcon()
    End Function

    Private Sub RefreshShowAllDocuments_Click(sender As Object, e As EventArgs) Handles RefreshShowAllDocuments.Click
        ' Show all documents and reset form title
        LoadDocumentsData("All")
    End Sub

    ' Clear selection when clicking outside the DataGridView
    Private Sub DocumentsManagement_Click(sender As Object, e As EventArgs) Handles Me.Click
        ClearDocumentSelection()
    End Sub

    ' Clear document selection helper method
    Private Sub ClearDocumentSelection()
        If DocumnetsDGV.SelectedRows.Count > 0 Then
            DocumnetsDGV.ClearSelection()
        End If
    End Sub

    ' Clear selection when clicking on other controls
    Private Sub ClearSelectionOnControlClick(sender As Object, e As EventArgs) Handles AboutToExpireBT.Click, ExpiredBT.Click, RefreshShowAllDocuments.Click
        ' Removed SacnDocumentBT and UpdateDocumenrBT as they might need document selection
    End Sub

    ' استخراج وعرض وثيقة متعددة الصور
    Private Sub ExtractAndViewMultiImageDocument(fileContent As Byte(), fileName As String)
        Try
            System.Diagnostics.Debug.WriteLine("Extracting multi-image document")

            ' Skip header
            Dim headerLength As Integer = System.Text.Encoding.UTF8.GetBytes("MULTI_SCAN_DOC_V1").Length
            Dim offset As Integer = headerLength

            ' Read page count
            Dim pageCount As Integer = BitConverter.ToInt32(fileContent, offset)
            offset += 4

            System.Diagnostics.Debug.WriteLine($"Found {pageCount} pages in multi-image document")

            ' Create temp directory
            Dim tempDir As String = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"MultiScanView_{DateTime.Now:yyyyMMddHHmmss}")
            System.IO.Directory.CreateDirectory(tempDir)

            ' Extract each page
            For i As Integer = 0 To pageCount - 1
                Dim pageSize As Integer = BitConverter.ToInt32(fileContent, offset)
                offset += 4

                Dim pageData As Byte() = New Byte(pageSize - 1) {}
                Array.Copy(fileContent, offset, pageData, 0, pageSize)
                offset += pageSize

                ' Save page to temp file
                Dim pageFileName As String = $"Page_{i + 1:00}.bmp"
                Dim pageFilePath As String = System.IO.Path.Combine(tempDir, pageFileName)
                System.IO.File.WriteAllBytes(pageFilePath, pageData)

                System.Diagnostics.Debug.WriteLine($"Extracted page {i + 1}: {pageSize} bytes -> {pageFileName}")
            Next

            ' Open the temp directory to show all images
            System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo() With {
                .FileName = "explorer.exe",
                .Arguments = tempDir,
                .UseShellExecute = True
            })

        Catch ex As Exception
            MessageBox.Show($"خطأ في استخراج الوثيقة متعددة الصور: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' استخراج وعرض وثيقة الصور المنفردة
    Private Sub ExtractAndViewIndividualImagesDocument(fileContent As Byte(), fileName As String)
        Try
            System.Diagnostics.Debug.WriteLine("Extracting individual images document")

            ' Skip header
            Dim headerLength As Integer = System.Text.Encoding.UTF8.GetBytes("INDIVIDUAL_IMAGES_V1").Length
            Dim offset As Integer = headerLength

            ' Read page count
            Dim pageCount As Integer = BitConverter.ToInt32(fileContent, offset)
            offset += 4

            System.Diagnostics.Debug.WriteLine($"Found {pageCount} images in individual images document")

            ' Create temp directory
            Dim tempDir As String = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"IndividualImagesView_{DateTime.Now:yyyyMMddHHmmss}")
            System.IO.Directory.CreateDirectory(tempDir)

            ' Extract each image
            For i As Integer = 0 To pageCount - 1
                ' Read filename length and filename
                Dim fileNameLength As Integer = BitConverter.ToInt32(fileContent, offset)
                offset += 4

                Dim originalFileName As String = System.Text.Encoding.UTF8.GetString(fileContent, offset, fileNameLength)
                offset += fileNameLength

                ' Read file size and content
                Dim fileSize As Integer = BitConverter.ToInt32(fileContent, offset)
                offset += 4

                Dim imageData As Byte() = New Byte(fileSize - 1) {}
                Array.Copy(fileContent, offset, imageData, 0, fileSize)
                offset += fileSize

                ' Save image to temp file
                Dim imageFilePath As String = System.IO.Path.Combine(tempDir, originalFileName)
                System.IO.File.WriteAllBytes(imageFilePath, imageData)

                System.Diagnostics.Debug.WriteLine($"Extracted image {i + 1}: {originalFileName}, {fileSize} bytes")
            Next

            ' Open the temp directory to show all images
            System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo() With {
                .FileName = "explorer.exe",
                .Arguments = tempDir,
                .UseShellExecute = True
            })

        Catch ex As Exception
            MessageBox.Show($"خطأ في استخراج وثيقة الصور المنفردة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' إنشاء ملف PDF صحيح من صورة واحدة
    Private Function CreatePDFFromSingleImage(imageBytes As Byte(), Optional userPassword As String = Nothing) As Byte()
        Try
            System.Diagnostics.Debug.WriteLine($"Creating single page PDF from {imageBytes.Length} bytes")

            ' Create temporary image file
            Dim tempImagePath As String = System.IO.Path.GetTempFileName() & ".bmp"
            System.IO.File.WriteAllBytes(tempImagePath, imageBytes)

            Using image As System.Drawing.Image = System.Drawing.Image.FromFile(tempImagePath)
                ' Use A4 page size (595.28 x 841.89 points)
                Dim pageWidth As Double = 595.28  ' A4 width in points (210mm)
                Dim pageHeight As Double = 841.89 ' A4 height in points (297mm)

                System.Diagnostics.Debug.WriteLine($"Single page A4 dimensions: {pageWidth:F1} x {pageHeight:F1} points")
                System.Diagnostics.Debug.WriteLine($"Image dimensions: {image.Width} x {image.Height} pixels")

                ' Create PDF using manual PDF structure with A4 page size
                Dim pdfBytes As Byte() = CreateManualPDF(imageBytes, pageWidth, pageHeight, False, userPassword)

                ' Clean up temp file
                Try
                    System.IO.File.Delete(tempImagePath)
                Catch
                    ' Ignore cleanup errors
                End Try

                If pdfBytes IsNot Nothing AndAlso pdfBytes.Length > 0 Then
                    System.Diagnostics.Debug.WriteLine($"Single page PDF created successfully: {pdfBytes.Length} bytes")
                    Return pdfBytes
                Else
                    System.Diagnostics.Debug.WriteLine("PDF creation returned null or empty")
                    Return Nothing
                End If
            End Using

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"CreatePDFFromSingleImage error: {ex.Message}")
            System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}")
            Return Nothing
        End Try
    End Function

    ' إنشاء ملف PDF صحيح من صور متعددة
    Private Function CreateProperPDFFromImages(imagePages As List(Of Byte()), Optional userPassword As String = Nothing) As Byte()
        Try
            System.Diagnostics.Debug.WriteLine($"Creating multi-page PDF from {imagePages.Count} images")

            ' Create temporary image files and get dimensions
            Dim tempImagePaths As New List(Of String)()
            Dim imageDimensions As New List(Of SizeF)()

            For i As Integer = 0 To imagePages.Count - 1
                Dim tempImagePath As String = System.IO.Path.GetTempFileName() & ".bmp"
                System.IO.File.WriteAllBytes(tempImagePath, imagePages(i))
                tempImagePaths.Add(tempImagePath)

                Using image As System.Drawing.Image = System.Drawing.Image.FromFile(tempImagePath)
                    ' Use A4 page size for all pages (595.28 x 841.89 points)
                    Dim pageWidth As Single = 595.28F  ' A4 width in points (210mm)
                    Dim pageHeight As Single = 841.89F ' A4 height in points (297mm)
                    imageDimensions.Add(New SizeF(pageWidth, pageHeight))

                    System.Diagnostics.Debug.WriteLine($"Image {i + 1}: {image.Width}x{image.Height} -> A4 PDF page {pageWidth:F1}x{pageHeight:F1} points")
                End Using
            Next

            ' Create multi-page PDF
            Dim pdfBytes As Byte() = CreateManualMultiPagePDF(imagePages, imageDimensions, userPassword)

            ' Clean up temp files
            For Each tempPath As String In tempImagePaths
                Try
                    System.IO.File.Delete(tempPath)
                Catch
                    ' Ignore cleanup errors
                End Try
            Next

            System.Diagnostics.Debug.WriteLine($"Multi-page PDF created: {pdfBytes?.Length} bytes")
            Return pdfBytes

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"CreateProperPDFFromImages error: {ex.Message}")
            Return Nothing
        End Try
    End Function

    ' إنشاء ملف PDF يدوياً بهيكل صحيح
    Private Function CreateManualPDF(imageBytes As Byte(), pageWidth As Double, pageHeight As Double, isMultiPage As Boolean, Optional userPassword As String = Nothing) As Byte()
        Try
            Using ms As New System.IO.MemoryStream()
                Using writer As New System.IO.BinaryWriter(ms)
                    ' PDF Header
                    Dim header As String = "%PDF-1.4" & vbLf
                    writer.Write(System.Text.Encoding.ASCII.GetBytes(header))

                    ' Object positions for xref table
                    Dim objectPositions As New List(Of Long)()

                    ' Object 1: Catalog
                    objectPositions.Add(ms.Position)
                    Dim catalog As String = "1 0 obj" & vbLf & "<<" & vbLf & "/Type /Catalog" & vbLf & "/Pages 2 0 R" & vbLf & ">>" & vbLf & "endobj" & vbLf
                    writer.Write(System.Text.Encoding.ASCII.GetBytes(catalog))

                    ' Object 2: Pages
                    objectPositions.Add(ms.Position)
                    Dim pages As String = "2 0 obj" & vbLf & "<<" & vbLf & "/Type /Pages" & vbLf & "/Kids [3 0 R]" & vbLf & "/Count 1" & vbLf & ">>" & vbLf & "endobj" & vbLf
                    writer.Write(System.Text.Encoding.ASCII.GetBytes(pages))

                    ' Object 3: Page
                    objectPositions.Add(ms.Position)
                    Dim page As String = $"3 0 obj" & vbLf & "<<" & vbLf & "/Type /Page" & vbLf & "/Parent 2 0 R" & vbLf & $"/MediaBox [0 0 {pageWidth:F2} {pageHeight:F2}]" & vbLf & "/Resources << /XObject << /Im0 4 0 R >> >>" & vbLf & "/Contents 5 0 R" & vbLf & ">>" & vbLf & "endobj" & vbLf
                    writer.Write(System.Text.Encoding.ASCII.GetBytes(page))

                    ' Object 4: Image
                    objectPositions.Add(ms.Position)

                    ' Convert BMP to JPEG if needed for better PDF compatibility
                    Dim jpegBytes As Byte() = ConvertToJPEG(imageBytes)
                    Dim finalImageBytes As Byte() = If(jpegBytes, imageBytes)

                    System.Diagnostics.Debug.WriteLine($"Image conversion: Original {imageBytes.Length} -> Final {finalImageBytes.Length} bytes")

                    Dim imageObj As String = $"4 0 obj" & vbLf & "<<" & vbLf & "/Type /XObject" & vbLf & "/Subtype /Image" & vbLf & $"/Width {1700}" & vbLf & $"/Height {2800}" & vbLf & "/ColorSpace /DeviceRGB" & vbLf & "/BitsPerComponent 8" & vbLf & "/Filter /DCTDecode" & vbLf & $"/Length {finalImageBytes.Length}" & vbLf & ">>" & vbLf & "stream" & vbLf
                    writer.Write(System.Text.Encoding.ASCII.GetBytes(imageObj))
                    writer.Write(finalImageBytes)

                    Dim endStream As String = vbLf & "endstream" & vbLf & "endobj" & vbLf
                    writer.Write(System.Text.Encoding.ASCII.GetBytes(endStream))

                    ' Object 5: Content stream (scale image to fit A4 page)
                    objectPositions.Add(ms.Position)
                    ' Scale and center image on A4 page while maintaining aspect ratio
                    Dim contentStream As String = $"q {pageWidth:F2} 0 0 {pageHeight:F2} 0 0 cm /Im0 Do Q"
                    Dim content As String = $"5 0 obj" & vbLf & "<<" & vbLf & $"/Length {contentStream.Length}" & vbLf & ">>" & vbLf & "stream" & vbLf & contentStream & vbLf & "endstream" & vbLf & "endobj" & vbLf
                    writer.Write(System.Text.Encoding.ASCII.GetBytes(content))

                    ' xref table
                    Dim xrefPos As Long = ms.Position
                    Dim xref As String = "xref" & vbLf & $"0 {objectPositions.Count + 1}" & vbLf & "0000000000 65535 f " & vbLf
                    For Each pos As Long In objectPositions
                        xref += $"{pos:D10} 00000 n " & vbLf
                    Next
                    writer.Write(System.Text.Encoding.ASCII.GetBytes(xref))

                    ' Trailer
                    Dim trailer As String = "trailer" & vbLf & "<<" & vbLf & $"/Size {objectPositions.Count + 1}" & vbLf & "/Root 1 0 R" & vbLf & ">>" & vbLf & "startxref" & vbLf & xrefPos.ToString() & vbLf & "%%EOF"
                    writer.Write(System.Text.Encoding.ASCII.GetBytes(trailer))

                    Return ms.ToArray()
                End Using
            End Using

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"CreateManualPDF error: {ex.Message}")
            Return Nothing
        End Try
    End Function

    ' إنشاء ملف PDF متعدد الصفحات يدوياً
    Private Function CreateManualMultiPagePDF(imagePages As List(Of Byte()), imageDimensions As List(Of SizeF), Optional userPassword As String = Nothing) As Byte()
        Try
            Using ms As New System.IO.MemoryStream()
                Using writer As New System.IO.BinaryWriter(ms)
                    ' PDF Header
                    Dim header As String = "%PDF-1.4" & vbLf
                    writer.Write(System.Text.Encoding.ASCII.GetBytes(header))

                    Dim objectPositions As New List(Of Long)()

                    ' Object 1: Catalog
                    objectPositions.Add(ms.Position)
                    Dim catalog As String = "1 0 obj" & vbLf & "<<" & vbLf & "/Type /Catalog" & vbLf & "/Pages 2 0 R" & vbLf & ">>" & vbLf & "endobj" & vbLf
                    writer.Write(System.Text.Encoding.ASCII.GetBytes(catalog))

                    ' Object 2: Pages (parent)
                    objectPositions.Add(ms.Position)
                    Dim pageRefs As String = ""
                    For i As Integer = 0 To imagePages.Count - 1
                        pageRefs += $" {3 + i * 3} 0 R"
                    Next
                    Dim pages As String = $"2 0 obj" & vbLf & "<<" & vbLf & "/Type /Pages" & vbLf & $"/Kids [{pageRefs}]" & vbLf & $"/Count {imagePages.Count}" & vbLf & ">>" & vbLf & "endobj" & vbLf
                    writer.Write(System.Text.Encoding.ASCII.GetBytes(pages))

                    ' Create objects for each page (3 objects per page: Page, Image, Content)
                    For i As Integer = 0 To imagePages.Count - 1
                        Dim pageObjNum As Integer = 3 + i * 3
                        Dim imageObjNum As Integer = 4 + i * 3
                        Dim contentObjNum As Integer = 5 + i * 3

                        Dim pageWidth As Single = imageDimensions(i).Width
                        Dim pageHeight As Single = imageDimensions(i).Height

                        ' Page object
                        objectPositions.Add(ms.Position)
                        Dim page As String = $"{pageObjNum} 0 obj" & vbLf & "<<" & vbLf & "/Type /Page" & vbLf & "/Parent 2 0 R" & vbLf & $"/MediaBox [0 0 {pageWidth:F2} {pageHeight:F2}]" & vbLf & $"/Resources << /XObject << /Im{i} {imageObjNum} 0 R >> >>" & vbLf & $"/Contents {contentObjNum} 0 R" & vbLf & ">>" & vbLf & "endobj" & vbLf
                        writer.Write(System.Text.Encoding.ASCII.GetBytes(page))

                        ' Image object
                        objectPositions.Add(ms.Position)
                        Dim jpegBytes As Byte() = ConvertToJPEG(imagePages(i))
                        If jpegBytes Is Nothing Then jpegBytes = imagePages(i)

                        Dim imageObj As String = $"{imageObjNum} 0 obj" & vbLf & "<<" & vbLf & "/Type /XObject" & vbLf & "/Subtype /Image" & vbLf & "/Width 1700" & vbLf & "/Height 2800" & vbLf & "/ColorSpace /DeviceRGB" & vbLf & "/BitsPerComponent 8" & vbLf & "/Filter /DCTDecode" & vbLf & $"/Length {jpegBytes.Length}" & vbLf & ">>" & vbLf & "stream" & vbLf
                        writer.Write(System.Text.Encoding.ASCII.GetBytes(imageObj))
                        writer.Write(jpegBytes)
                        Dim endStream As String = vbLf & "endstream" & vbLf & "endobj" & vbLf
                        writer.Write(System.Text.Encoding.ASCII.GetBytes(endStream))

                        ' Content stream (scale image to fit A4 page)
                        objectPositions.Add(ms.Position)
                        Dim contentStream As String = $"q {pageWidth:F2} 0 0 {pageHeight:F2} 0 0 cm /Im{i} Do Q"
                        Dim content As String = $"{contentObjNum} 0 obj" & vbLf & "<<" & vbLf & $"/Length {contentStream.Length}" & vbLf & ">>" & vbLf & "stream" & vbLf & contentStream & vbLf & "endstream" & vbLf & "endobj" & vbLf
                        writer.Write(System.Text.Encoding.ASCII.GetBytes(content))
                    Next

                    ' xref table
                    Dim xrefPos As Long = ms.Position
                    Dim xref As String = "xref" & vbLf & $"0 {objectPositions.Count + 1}" & vbLf & "0000000000 65535 f " & vbLf
                    For Each pos As Long In objectPositions
                        xref += $"{pos:D10} 00000 n " & vbLf
                    Next
                    writer.Write(System.Text.Encoding.ASCII.GetBytes(xref))

                    ' Trailer
                    Dim trailer As String = "trailer" & vbLf & "<<" & vbLf & $"/Size {objectPositions.Count + 1}" & vbLf & "/Root 1 0 R" & vbLf & ">>" & vbLf & "startxref" & vbLf & xrefPos.ToString() & vbLf & "%%EOF"
                    writer.Write(System.Text.Encoding.ASCII.GetBytes(trailer))

                    Return ms.ToArray()
                End Using
            End Using

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"CreateManualMultiPagePDF error: {ex.Message}")
            Return Nothing
        End Try
    End Function

    ' تحويل الصورة إلى JPEG لتحسين التوافق مع PDF
    Private Function ConvertToJPEG(imageBytes As Byte()) As Byte()
        Try
            Using ms As New System.IO.MemoryStream(imageBytes)
                Using image As System.Drawing.Image = System.Drawing.Image.FromStream(ms)
                    Using outputMs As New System.IO.MemoryStream()
                        ' Get JPEG codec
                        Dim jpegCodec As System.Drawing.Imaging.ImageCodecInfo = GetEncoderInfo("image/jpeg")
                        If jpegCodec IsNot Nothing Then
                            ' Set high quality
                            Dim encoderParams As New System.Drawing.Imaging.EncoderParameters(1)
                            encoderParams.Param(0) = New System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L)

                            image.Save(outputMs, jpegCodec, encoderParams)
                            Return outputMs.ToArray()
                        Else
                            image.Save(outputMs, System.Drawing.Imaging.ImageFormat.Jpeg)
                            Return outputMs.ToArray()
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"ConvertToJPEG error: {ex.Message}")
            Return Nothing
        End Try
    End Function


    ' تطبيق حماية كلمة المرور على PDF (نهج بسيط)
    Private Function ApplyPasswordProtection(pdfBytes As Byte(), userPassword As String) As Byte()
        Try
            If String.IsNullOrEmpty(userPassword) OrElse pdfBytes Is Nothing Then
                Return pdfBytes
            End If

            System.Diagnostics.Debug.WriteLine($"Applying password protection with user password")

            ' Simple encryption approach - XOR with password hash
            ' Note: This is a basic demonstration. For production use a proper PDF encryption library
            Dim passwordHash As Byte() = System.Text.Encoding.UTF8.GetBytes(userPassword)
            Dim protectedBytes As Byte() = New Byte(pdfBytes.Length - 1) {}

            ' Keep PDF header unencrypted (first 20 bytes)
            For i As Integer = 0 To Math.Min(19, pdfBytes.Length - 1)
                protectedBytes(i) = pdfBytes(i)
            Next

            ' Apply simple XOR encryption to the rest
            For i As Integer = 20 To pdfBytes.Length - 1
                Dim keyIndex As Integer = (i - 20) Mod passwordHash.Length
                protectedBytes(i) = CByte(pdfBytes(i) Xor passwordHash(keyIndex))
            Next

            ' Add password protection marker at the end
            Dim marker As String = vbLf & "% Password Protected PDF" & vbLf
            Dim markerBytes As Byte() = System.Text.Encoding.ASCII.GetBytes(marker)

            Dim finalBytes As Byte() = New Byte(protectedBytes.Length + markerBytes.Length - 1) {}
            Array.Copy(protectedBytes, 0, finalBytes, 0, protectedBytes.Length)
            Array.Copy(markerBytes, 0, finalBytes, protectedBytes.Length, markerBytes.Length)

            System.Diagnostics.Debug.WriteLine($"Password protection applied to PDF ({finalBytes.Length} bytes)")
            Return finalBytes

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"Error applying password protection: {ex.Message}")
            Return pdfBytes
        End Try
    End Function

    ' مصادقة المستخدم لفتح أي PDF
    Private Function AuthenticateUserForPDFAccess() As Boolean
        Try
            ' Check if already authenticated in current session
            If Session.IsDocumentAccessAuthenticated Then
                System.Diagnostics.Debug.WriteLine("Using cached document authentication")
                currentDecryptionPassword = Session.DocumentAccessPassword
                Return True
            End If

            ' Create a simple input dialog for document access password
            Dim passwordForm As New Form()
            passwordForm.Text = "مصادقة الوثيقة"
            passwordForm.Size = New Size(400, 200)
            passwordForm.StartPosition = FormStartPosition.CenterParent
            passwordForm.FormBorderStyle = FormBorderStyle.FixedDialog
            passwordForm.MaximizeBox = False
            passwordForm.MinimizeBox = False

            ' Add label
            Dim label As New Label()
            label.Text = "أدخل كلمة مرور الوثيقة:"
            label.Location = New Point(10, 20)
            label.Size = New Size(350, 30)
            label.TextAlign = ContentAlignment.MiddleRight
            passwordForm.Controls.Add(label)

            ' Add password textbox
            Dim passwordTB As New TextBox()
            passwordTB.Location = New Point(10, 60)
            passwordTB.Size = New Size(350, 25)
            passwordTB.UseSystemPasswordChar = True
            passwordTB.RightToLeft = RightToLeft.No
            passwordForm.Controls.Add(passwordTB)

            ' Add buttons
            Dim okButton As New Button()
            okButton.Text = "موافق"
            okButton.Location = New Point(210, 120)
            okButton.Size = New Size(75, 30)
            okButton.DialogResult = DialogResult.OK
            passwordForm.Controls.Add(okButton)

            Dim cancelButton As New Button()
            cancelButton.Text = "إلغاء"
            cancelButton.Location = New Point(290, 120)
            cancelButton.Size = New Size(75, 30)
            cancelButton.DialogResult = DialogResult.Cancel
            passwordForm.Controls.Add(cancelButton)

            passwordForm.AcceptButton = okButton
            passwordForm.CancelButton = cancelButton

            ' Show dialog and validate against UsersLogin_Documents table
            If passwordForm.ShowDialog() = DialogResult.OK Then
                Dim enteredPassword As String = passwordTB.Text.Trim()

                If String.IsNullOrEmpty(enteredPassword) Then
                    MessageBox.Show("يرجى إدخال كلمة المرور", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return False
                End If

                ' Check if password exists in UsersLogin_Documents and user is active
                Dim activeUsers As DataTable = dbConn.GetActiveDocumentUsers()
                For Each row As DataRow In activeUsers.Rows
                    Dim userId As String = row("userId").ToString()
                    Dim userPassword As String = row("password").ToString()

                    If userPassword = enteredPassword Then
                        System.Diagnostics.Debug.WriteLine($"Document access authenticated for user ID: {userId}")
                        currentDecryptionPassword = enteredPassword

                        ' Store credentials in session for reuse
                        Session.SetDocumentAccess(userId, enteredPassword)

                        Return True
                    End If
                Next

                MessageBox.Show("كلمة المرور غير صحيحة أو المستخدم غير نشط", "خطأ في المصادقة", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            Return False

        Catch ex As Exception
            MessageBox.Show($"خطأ في عملية المصادقة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ' فحص ما إذا كانت الوثيقة محمية بكلمة مرور
    Private Function IsDocumentPasswordProtected(fileContent As Byte()) As Boolean
        Try
            If fileContent Is Nothing OrElse fileContent.Length < 50 Then
                Return False
            End If

            ' Check for password protection marker
            Dim contentString As String = System.Text.Encoding.ASCII.GetString(fileContent)
            Return contentString.Contains("% Password Protected PDF")

        Catch ex As Exception
            Return False
        End Try
    End Function


    ' فك تشفير الوثيقة
    Private Function DecryptDocument(encryptedContent As Byte()) As Byte()
        Try
            If String.IsNullOrEmpty(currentDecryptionPassword) Then
                System.Diagnostics.Debug.WriteLine("No decryption password available")
                Return encryptedContent
            End If

            ' Remove the password protection marker
            Dim markerBytes As Byte() = System.Text.Encoding.ASCII.GetBytes(vbLf & "% Password Protected PDF" & vbLf)
            Dim contentWithoutMarker As Byte() = New Byte(encryptedContent.Length - markerBytes.Length - 1) {}
            Array.Copy(encryptedContent, 0, contentWithoutMarker, 0, contentWithoutMarker.Length)

            ' Decrypt using XOR with password
            Dim passwordHash As Byte() = System.Text.Encoding.UTF8.GetBytes(currentDecryptionPassword)
            Dim decryptedBytes As Byte() = New Byte(contentWithoutMarker.Length - 1) {}

            ' Keep PDF header unencrypted (first 20 bytes)
            For i As Integer = 0 To Math.Min(19, contentWithoutMarker.Length - 1)
                decryptedBytes(i) = contentWithoutMarker(i)
            Next

            ' Decrypt the rest using XOR
            For i As Integer = 20 To contentWithoutMarker.Length - 1
                Dim keyIndex As Integer = (i - 20) Mod passwordHash.Length
                decryptedBytes(i) = CByte(contentWithoutMarker(i) Xor passwordHash(keyIndex))
            Next

            ' Clear the password for security
            currentDecryptionPassword = Nothing

            System.Diagnostics.Debug.WriteLine("Document decrypted successfully")
            Return decryptedBytes

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"Error decrypting document: {ex.Message}")
            currentDecryptionPassword = Nothing
            Return encryptedContent
        End Try
    End Function

    ' Color row based on document expiration status
    Private Sub ColorRowByExpirationStatus(row As DataGridViewRow, expireDate As DateTime)
        Try
            Dim today As DateTime = DateTime.Now.Date
            Dim daysUntilExpiry As Integer = (expireDate.Date - today).Days

            If daysUntilExpiry < 0 Then
                ' Expired documents - Red background
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 200, 200) ' Light red
                row.DefaultCellStyle.ForeColor = Color.DarkRed
            ElseIf daysUntilExpiry <= 30 Then
                ' About to expire (within 30 days) - Orange/Yellow background
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 245, 150) ' Light orange/yellow
                row.DefaultCellStyle.ForeColor = Color.DarkOrange
            Else
                ' Valid documents - Light green background
                row.DefaultCellStyle.BackColor = Color.FromArgb(200, 255, 200) ' Light green
                row.DefaultCellStyle.ForeColor = Color.DarkGreen
            End If

        Catch ex As Exception
            ' If there's an error, use default colors
            row.DefaultCellStyle.BackColor = Color.White
            row.DefaultCellStyle.ForeColor = Color.Black
        End Try
    End Sub

    ' Update button notification counts
    Private Sub UpdateButtonNotificationCounts()
        Try
            Dim expiredCount As Integer = 0
            Dim aboutToExpireCount As Integer = 0

            If SelectedCustomerId = 0 Then
                ' Get counts for all customers
                Dim expiredDt = dbConn.GetExpiredDocumentsWithCustomerNames()
                Dim aboutToExpireDt = dbConn.GetAboutToExpireDocumentsWithCustomerNames()
                expiredCount = expiredDt.Rows.Count
                aboutToExpireCount = aboutToExpireDt.Rows.Count
            Else
                ' Get counts for specific customer
                Dim expiredDt = dbConn.GetExpiredDocumentsByCustomer(SelectedCustomerId)
                Dim aboutToExpireDt = dbConn.GetAboutToExpireDocumentsByCustomer(SelectedCustomerId)
                expiredCount = expiredDt.Rows.Count
                aboutToExpireCount = aboutToExpireDt.Rows.Count
            End If



            ' Update labels with numbers only
            If expiredCount > 0 Then
                ExpiredLB.Text = expiredCount.ToString()
                ExpiredLB.Visible = True
            Else
                ExpiredLB.Text = ""
                ExpiredLB.Visible = False
            End If

            If aboutToExpireCount > 0 Then
                AboutToExpireLB.Text = aboutToExpireCount.ToString()
                AboutToExpireLB.Visible = True
            Else
                AboutToExpireLB.Text = ""
                AboutToExpireLB.Visible = False
            End If

        Catch ex As Exception
            ' If there's an error, use defaults
            '  ExpiredBT.Text = "المنتهية الصلاحية"
            ' AboutToExpireBT.Text = "منتهية الصلاحية قريباً"
            ExpiredLB.Text = ""
            ExpiredLB.Visible = False
            AboutToExpireLB.Text = ""
            AboutToExpireLB.Visible = False
        End Try
    End Sub
End Class