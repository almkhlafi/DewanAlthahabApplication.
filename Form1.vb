Public Class Form1

    Private dbConn As New DBconnections()
    Private isUpdatingCustomer As Boolean = False
    Private selectedCustomerId As Integer = 0

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        tableHeaders()
        LoadCustomerData()
    End Sub



    Private Sub AddAttachmentBT_Click(sender As Object, e As EventArgs) Handles AddAttachmentsBT.Click
        AttachmentsManagement.ShowDialog()

    End Sub
    Private Sub LoadCustomerData()
        Dim dt As DataTable = dbConn.GetCustomers()

        CustomerTableDGV.Rows.Clear()

        For Each row As DataRow In dt.Rows
            CustomerTableDGV.Rows.Add(row("Id"), row("Name"))
        Next

        ' Prevent auto-selection after loading data
        CustomerTableDGV.ClearSelection()
    End Sub

    Private Sub tableHeaders()
        CustomerTableDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        CustomerTableDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        CustomerTableDGV.Columns.Clear()
        CustomerTableDGV.Columns.Add("CustomerID", "رقم")
        CustomerTableDGV.Columns.Add("CustomerName", "اسم العميل")
        CustomerTableDGV.ReadOnly = True
        CustomerTableDGV.AllowUserToAddRows = False
        CustomerTableDGV.AllowUserToDeleteRows = False

        ' Disable auto-selection
        CustomerTableDGV.ClearSelection()
    End Sub

    Private Sub AddCustomerBT_Click_1(sender As Object, e As EventArgs) Handles AddCustomerBT.Click
        ' التحقق من عدم وجود نص فارغ أو مسافات فقط
        If String.IsNullOrWhiteSpace(addUserTB.Text) Then
            MessageBox.Show("يرجى إدخال اسم العميل", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            addUserTB.Focus()
            Return
        End If

        ' التحقق من طول النص
        If addUserTB.Text.Trim().Length < 2 Then
            MessageBox.Show("يجب أن يكون اسم العميل أكثر من حرف واحد", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            addUserTB.Focus()
            Return
        End If

        ' التحقق من طول النص الأقصى
        If addUserTB.Text.Trim().Length > 100 Then
            MessageBox.Show("اسم العميل طويل جداً. الحد الأقصى 100 حرف", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            addUserTB.Focus()
            Return
        End If

        Try
            If isUpdatingCustomer Then
                ' تحديث العميل الموجود
                Dim updateSuccess As Boolean = dbConn.UpdateCustomer(selectedCustomerId, addUserTB.Text.Trim())
                If updateSuccess Then
                    MessageBox.Show("تم تحديث العميل بنجاح", "نجح التحديث", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("فشل في تحديث العميل. يرجى المحاولة مرة أخرى.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
                ' إعادة تعيين وضع التحديث
                isUpdatingCustomer = False
                selectedCustomerId = 0
            Else
                ' إضافة عميل جديد
                Dim newCustomerId As Integer = dbConn.AddCustomer(addUserTB.Text.Trim())

                If newCustomerId > 0 Then
                    ' إنشاء إدخالات وثائق للعميل الجديد لجميع المرفقات الموجودة
                    Dim documentsCreated As Boolean = dbConn.AddAttachmentDocumentsForNewCustomer(newCustomerId)

                    If documentsCreated Then
                        MessageBox.Show("تم إضافة العميل بنجاح وتم إنشاء إدخالات الوثائق لجميع المرفقات", "نجحت الإضافة", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        MessageBox.Show("تم إضافة العميل ولكن فشل في إنشاء إدخالات الوثائق", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If
                Else
                    MessageBox.Show("فشل في إضافة العميل", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If

            addUserTB.Clear()
            LoadCustomerData()
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء العملية: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub RemoveCustomerBT_Click(sender As Object, e As EventArgs) Handles RemoveCustomerBT.Click
        ' التحقق من تحديد صف في الجدول
        If CustomerTableDGV.SelectedRows.Count = 0 Then
            MessageBox.Show("يرجى تحديد عميل لحذفه", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' الحصول على معرف العميل المحدد
        Dim selectedRow As DataGridViewRow = CustomerTableDGV.SelectedRows(0)
        Dim customerId As Integer = Convert.ToInt32(selectedRow.Cells("CustomerID").Value)
        Dim customerName As String = selectedRow.Cells("CustomerName").Value.ToString()

        ' التأكيد قبل الحذف
        Dim result As DialogResult = MessageBox.Show(
            "هل أنت متأكد من حذف العميل: " & customerName & "؟" & vbCrLf & "لا يمكن التراجع عن هذا الإجراء.",
            "تأكيد الحذف",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question
        )

        If result = DialogResult.Yes Then
            ' حذف العميل من قاعدة البيانات
            Dim deleteSuccess As Boolean = dbConn.RemoveCustomer(customerId)

            If deleteSuccess Then
                MessageBox.Show("تم حذف العميل بنجاح", "نجح الحذف", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ' تحديث الجدول بعد الحذف
                LoadCustomerData()
            Else
                MessageBox.Show("فشل في حذف العميل. يرجى المحاولة مرة أخرى.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub

    Private Sub CustomerTableDGV_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles CustomerTableDGV.CellClick
        ' التأكد من أن المستخدم نقر على خلية صالحة وليس على الرأس
        If e.RowIndex >= 0 Then
            Dim selectedRow As DataGridViewRow = CustomerTableDGV.Rows(e.RowIndex)
            Dim customerId As Integer = Convert.ToInt32(selectedRow.Cells("CustomerID").Value)
            Dim currentName As String = selectedRow.Cells("CustomerName").Value.ToString()

            ' وضع البيانات في حقل النص
            addUserTB.Text = currentName

            ' تفعيل وضع التحديث
            isUpdatingCustomer = True
            selectedCustomerId = customerId

            ' إعطاء التركيز لحقل النص
            addUserTB.Focus()
            addUserTB.SelectAll()
        End If
    End Sub

    Private Sub ResetCustomerUpdateMode()
        isUpdatingCustomer = False
        selectedCustomerId = 0
    End Sub

    Private Sub addUserTB_TextChanged(sender As Object, e As EventArgs) Handles addUserTB.TextChanged
        ' إذا كان المستخدم يكتب في حقل فارغ، اجعل الزر للإضافة
        If String.IsNullOrEmpty(addUserTB.Text) And isUpdatingCustomer Then
            ResetCustomerUpdateMode()
        End If
    End Sub

    Private Sub CustomerTableDGV_Leave(sender As Object, e As EventArgs) Handles CustomerTableDGV.Leave
        ' لا نفعل شيء هنا، نترك الوضع كما هو
    End Sub

    ' إلغاء التحديد عند النقر على أي مكان في النموذج
    Private Sub Form1_Click(sender As Object, e As EventArgs) Handles Me.Click
        ClearCustomerSelection()
    End Sub

    ' إلغاء التحديد عند النقر على أي من عناصر التحكم الأخرى
    Private Sub ClearSelectionOnControlClick(sender As Object, e As EventArgs) Handles AddAttachmentsBT.Click
        ' Don't clear selection for controls that need it (removed addUserTB as it's used for updating selected customers)
        ClearCustomerSelection()
    End Sub

    ' طريقة مساعدة لإلغاء تحديد العميل
    Private Sub ClearCustomerSelection()
        If CustomerTableDGV.SelectedRows.Count > 0 Then
            CustomerTableDGV.ClearSelection()
            ResetCustomerUpdateMode()
            addUserTB.Clear()
        End If
    End Sub

    Private Sub DocumentsSettings_Click(sender As Object, e As EventArgs) Handles DocumentsSettings.Click
        Dim docMgmt As New DocumentsManagement

        ' Check if a customer is selected
        If CustomerTableDGV.SelectedRows.Count > 0 Then
            ' Customer selected - show customer-specific documents
            Dim selectedRow = CustomerTableDGV.SelectedRows(0)
            docMgmt.SelectedCustomerId = Convert.ToInt32(selectedRow.Cells("CustomerID").Value)
            docMgmt.SelectedCustomerName = selectedRow.Cells("CustomerName").Value.ToString
        Else
            ' No customer selected - show all documents for all customers
            docMgmt.SelectedCustomerId = 0 ' 0 means show all customers
            docMgmt.SelectedCustomerName = "جميع العملاء"
        End If

        docMgmt.ShowDialog()
        docMgmt.Dispose()
    End Sub

    Private Sub Delegations_Click(sender As Object, e As EventArgs) Handles Delegations.Click
        ' Check if a customer is selected
        If CustomerTableDGV.SelectedRows.Count > 0 Then
            ' Customer selected - create and show the delegations form
            Dim delegMgmt As New DelegationsManagement()
            Dim selectedRow As DataGridViewRow = CustomerTableDGV.SelectedRows(0)
            Dim customerId As Integer = Convert.ToInt32(selectedRow.Cells("CustomerID").Value)
            delegMgmt.CustomerIdToLoad = customerId

            delegMgmt.ShowDialog()
            delegMgmt.Dispose()
        Else
            ' No customer selected - show warning message
            MessageBox.Show("يرجى تحديد عميل أولاً لإدارة المفوضين.", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub
End Class