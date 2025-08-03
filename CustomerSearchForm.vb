Public Class CustomerSearchForm
    Private dbConn As New DBconnections()
    Public Property SelectedCustomerId As Integer = 0
    Public Property SelectedCustomerName As String = ""
    
    ' Variables for customer management
    Private isUpdatingCustomer As Boolean = False
    Private selectedCustomerIdForUpdate As Integer = 0

    Private Sub CustomerSearchForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupTable()
        LoadCustomerData()
    End Sub

    Private Sub SetupTable()
        CustomerSearchDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        CustomerSearchDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        CustomerSearchDGV.MultiSelect = False

        CustomerSearchDGV.Columns.Clear()
        CustomerSearchDGV.Columns.Add("CustomerID", "رقم")
        CustomerSearchDGV.Columns.Add("CustomerName", "اسم العميل")
        CustomerSearchDGV.ReadOnly = True
        CustomerSearchDGV.AllowUserToAddRows = False
        CustomerSearchDGV.AllowUserToDeleteRows = False

        CustomerSearchDGV.ClearSelection()
    End Sub

    Private Sub LoadCustomerData(Optional searchText As String = "")
        Dim dt As DataTable = dbConn.GetCustomers()
        CustomerSearchDGV.Rows.Clear()

        For Each row As DataRow In dt.Rows
            Dim customerName As String = row("Name").ToString()

            If String.IsNullOrEmpty(searchText) OrElse
               customerName.ToLower().Contains(searchText.ToLower()) OrElse
               row("Id").ToString().Contains(searchText) Then
                CustomerSearchDGV.Rows.Add(row("Id"), customerName)
            End If
        Next

        CustomerSearchDGV.ClearSelection()
    End Sub

    Private Sub SearchTB_TextChanged(sender As Object, e As EventArgs) Handles SearchTB.TextChanged
        LoadCustomerData(SearchTB.Text.Trim())
    End Sub

    Private Sub CustomerSearchDGV_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles CustomerSearchDGV.CellDoubleClick
        SelectCustomer()
    End Sub

    Private Sub SelectBT_Click(sender As Object, e As EventArgs) Handles SelectBT.Click
        SelectCustomer()
    End Sub

    Private Sub SelectCustomer()
        If CustomerSearchDGV.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = CustomerSearchDGV.SelectedRows(0)
            SelectedCustomerId = Convert.ToInt32(selectedRow.Cells("CustomerID").Value)
            SelectedCustomerName = selectedRow.Cells("CustomerName").Value.ToString()
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Else
            MessageBox.Show("يرجى تحديد عميل", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub CancelBT_Click(sender As Object, e As EventArgs) Handles CancelBT.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub AddCustomerBT_Click(sender As Object, e As EventArgs) Handles AddCustomerBT.Click
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
                Dim updateSuccess As Boolean = dbConn.UpdateCustomer(selectedCustomerIdForUpdate, addUserTB.Text.Trim())
                If updateSuccess Then
                    MessageBox.Show("تم تحديث العميل بنجاح", "نجح التحديث", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("فشل في تحديث العميل. يرجى المحاولة مرة أخرى.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
                ' إعادة تعيين وضع التحديث
                ResetCustomerUpdateMode()
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
            LoadCustomerData(SearchTB.Text.Trim()) ' Refresh the list
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء العملية: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub RemoveCustomerBT_Click(sender As Object, e As EventArgs) Handles RemoveCustomerBT.Click
        ' التحقق من تحديد صف في الجدول
        If CustomerSearchDGV.SelectedRows.Count = 0 Then
            MessageBox.Show("يرجى تحديد عميل لحذفه", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' الحصول على معرف العميل المحدد
        Dim selectedRow As DataGridViewRow = CustomerSearchDGV.SelectedRows(0)
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
                LoadCustomerData(SearchTB.Text.Trim())
            Else
                MessageBox.Show("فشل في حذف العميل. يرجى المحاولة مرة أخرى.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub

    Private Sub CustomerSearchDGV_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles CustomerSearchDGV.CellClick
        ' التأكد من أن المستخدم نقر على خلية صالحة وليس على الرأس
        If e.RowIndex >= 0 Then
            Dim selectedRow As DataGridViewRow = CustomerSearchDGV.Rows(e.RowIndex)
            Dim customerId As Integer = Convert.ToInt32(selectedRow.Cells("CustomerID").Value)
            Dim currentName As String = selectedRow.Cells("CustomerName").Value.ToString()

            ' وضع البيانات في حقل النص
            addUserTB.Text = currentName

            ' تفعيل وضع التحديث
            isUpdatingCustomer = True
            selectedCustomerIdForUpdate = customerId

            ' إعطاء التركيز لحقل النص
            addUserTB.Focus()
            addUserTB.SelectAll()
        End If
    End Sub

    Private Sub ResetCustomerUpdateMode()
        isUpdatingCustomer = False
        selectedCustomerIdForUpdate = 0
    End Sub

    Private Sub addUserTB_TextChanged(sender As Object, e As EventArgs) Handles addUserTB.TextChanged
        ' إذا كان المستخدم يكتب في حقل فارغ، اجعل الزر للإضافة
        If String.IsNullOrEmpty(addUserTB.Text) And isUpdatingCustomer Then
            ResetCustomerUpdateMode()
        End If
    End Sub
End Class