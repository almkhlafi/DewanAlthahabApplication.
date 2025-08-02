Public Class DelegationsManagement

    Private dbConn As New DBconnections()
    Private isUpdatingDelegator As Boolean = False
    Private selectedDelegatorId As Integer = 0
    Private selectedCustomerId As Integer = 0
    Private isUpdatingPermission As Boolean = False
    Private selectedPermissionId As Integer = 0

    ' Public property to set customer ID before showing the form
    Public Property CustomerIdToLoad As Integer
        Get
            Return selectedCustomerId
        End Get
        Set(value As Integer)
            selectedCustomerId = value
        End Set
    End Property

    Private Sub DelegationsManagement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupDataGridView()
        SetupArabicDatePickers()
        ' Load data if customer ID was already set
        If selectedCustomerId > 0 Then
            LoadDelegatorsData(selectedCustomerId)
            UpdateCustomerNameLabel()
        End If

        ' Load all permissions initially (no delegator selected)
        LoadDelegatorPermissions(0)
        
        ' Show all permissions in dataGDDelegations grid initially
        LoadAllPermissionsIntoGrid()
    End Sub

    Private Sub SetupDataGridView()
        dataGDlistAllDelegators.Columns.Clear()
        dataGDlistAllDelegators.SelectionMode = DataGridViewSelectionMode.FullRowSelect

        dataGDlistAllDelegators.MultiSelect = False
        dataGDlistAllDelegators.ReadOnly = True
        dataGDlistAllDelegators.AllowUserToAddRows = False
        dataGDlistAllDelegators.AllowUserToDeleteRows = False
        dataGDlistAllDelegators.RowHeadersVisible = False
        dataGDlistAllDelegators.ColumnHeadersVisible = True
        dataGDlistAllDelegators.EnableHeadersVisualStyles = True
        dataGDlistAllDelegators.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        ' تنسيق رؤوس الأعمدة - خط عريض
        dataGDlistAllDelegators.ColumnHeadersHeight = 40
        dataGDlistAllDelegators.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray
        dataGDlistAllDelegators.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
        dataGDlistAllDelegators.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 10, FontStyle.Bold)
        dataGDlistAllDelegators.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        ' تنسيق البيانات - خط عادي
        dataGDlistAllDelegators.RowTemplate.Height = 35
        dataGDlistAllDelegators.DefaultCellStyle.Font = New Font("Arial", 9, FontStyle.Regular)
        dataGDlistAllDelegators.DefaultCellStyle.Padding = New Padding(5, 5, 5, 5)
        dataGDlistAllDelegators.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

        ' إضافة الأعمدة
        dataGDlistAllDelegators.Columns.Add("Id", "رقم")
        dataGDlistAllDelegators.Columns.Add("Name", "اسم المفوض")
        dataGDlistAllDelegators.Columns.Add("CustomerId", "رقم العميل")
        dataGDlistAllDelegators.Columns.Add("Identity", "رقم الهوية")
        dataGDlistAllDelegators.Columns.Add("Nationality", "الجنسية")
        dataGDlistAllDelegators.Columns.Add("PhoneNumber", "رقم الهاتف")
        dataGDlistAllDelegators.Columns.Add("Type", "نوع التفويض")
        dataGDlistAllDelegators.Columns.Add("dateOfDelegation", "تاريخ بداية التفويض")
        dataGDlistAllDelegators.Columns.Add("expireOfDelegation", "تاريخ انتهاء التفويض")
        dataGDlistAllDelegators.Columns.Add("dateOfPermision", "تاريخ بداية الترخيص")
        dataGDlistAllDelegators.Columns.Add("expireOfPermision", "تاريخ انتهاء الترخيص")
        dataGDlistAllDelegators.Columns.Add("AssignedDate", "تاريخ تعيين الصلاحية")

        ' تنسيق أعمدة التاريخ
        dataGDlistAllDelegators.Columns("dateOfDelegation").DefaultCellStyle.Format = "dd/MM/yyyy"
        dataGDlistAllDelegators.Columns("expireOfDelegation").DefaultCellStyle.Format = "dd/MM/yyyy"
        dataGDlistAllDelegators.Columns("dateOfPermision").DefaultCellStyle.Format = "dd/MM/yyyy"
        dataGDlistAllDelegators.Columns("expireOfPermision").DefaultCellStyle.Format = "dd/MM/yyyy"
        dataGDlistAllDelegators.Columns("AssignedDate").DefaultCellStyle.Format = "dd/MM/yyyy"

        ' إخفاء عمود الرقم أو جعله أصغر
        dataGDlistAllDelegators.Columns("Id").Width = 50
        dataGDlistAllDelegators.Columns("CustomerId").Width = 70
    End Sub

    Private Sub SetupArabicDatePickers()
        ' Configure Arabic culture for date display
        Dim arabicCulture As New System.Globalization.CultureInfo("ar-SA")

        ' Configure dateOfPermisionTimePicker for Arabic display
        If dateOfPermisionTimePicker IsNot Nothing Then
            dateOfPermisionTimePicker.Format = DateTimePickerFormat.Custom
            dateOfPermisionTimePicker.CustomFormat = "dd/MM/yyyy"
            dateOfPermisionTimePicker.RightToLeft = RightToLeft.Yes
            dateOfPermisionTimePicker.RightToLeftLayout = True
        End If

        ' Configure dateOfDelegateTimePicker for Arabic display
        If dateOfDelegateTimePicker IsNot Nothing Then
            dateOfDelegateTimePicker.Format = DateTimePickerFormat.Custom
            dateOfDelegateTimePicker.CustomFormat = "dd/MM/yyyy"
            dateOfDelegateTimePicker.RightToLeft = RightToLeft.Yes
            dateOfDelegateTimePicker.RightToLeftLayout = True
        End If

        ' Configure ExpireDatePicker for Arabic display (if it exists)
        Try
            If Me.Controls("ExpireDatePicker") IsNot Nothing Then
                Dim expirePicker As DateTimePicker = DirectCast(Me.Controls("ExpireDatePicker"), DateTimePicker)
                expirePicker.Format = DateTimePickerFormat.Custom
                expirePicker.CustomFormat = "dd/MM/yyyy"
                expirePicker.RightToLeft = RightToLeft.Yes
                expirePicker.RightToLeftLayout = True
            End If
        Catch
            ' ExpireDatePicker doesn't exist, skip
        End Try

        ' Configure UploadDatePicker for Arabic display (if it exists)
        Try
            If Me.Controls("UploadDatePicker") IsNot Nothing Then
                Dim uploadPicker As DateTimePicker = DirectCast(Me.Controls("UploadDatePicker"), DateTimePicker)
                uploadPicker.Format = DateTimePickerFormat.Custom
                uploadPicker.CustomFormat = "dd/MM/yyyy"
                uploadPicker.RightToLeft = RightToLeft.Yes
                uploadPicker.RightToLeftLayout = True
            End If
        Catch
            ' UploadDatePicker doesn't exist, skip
        End Try
    End Sub

    Public Sub LoadDelegatorsData(Optional customerId As Integer = 0)
        ' Always refresh the DataGridView setup
        SetupDataGridView()

        Me.selectedCustomerId = customerId
        Dim dt As DataTable

        If customerId > 0 Then
            Me.Text = $"إدارة المفوضين - العميل رقم: {customerId}"
            dt = dbConn.GetDelegatorsByCustomer(customerId)

            ' Update customer name label
            UpdateCustomerNameLabel()

            ' If no delegators found for this customer, keep empty table (don't show all delegators)
            If dt.Rows.Count = 0 Then
                ' Keep the empty DataTable - don't load all delegators
                Me.Text = $"إدارة المفوضين - العميل رقم: {customerId} (لا يوجد مفوضين)"
            End If
        Else
            ' If no specific customer, show empty table
            dt = New DataTable()
            Me.Text = "إدارة المفوضين - لم يتم تحديد عميل"
            CustomerNameLable.Text = "لم يتم تحديد عميل"
        End If

        ' Clear existing data and refresh
        dataGDlistAllDelegators.Rows.Clear()
        dataGDlistAllDelegators.Refresh()

        For Each row As DataRow In dt.Rows
            dataGDlistAllDelegators.Rows.Add(
                row("Id"),
                row("Name"),
                row("CustomerId"),
                If(row("Identity") Is DBNull.Value, "", row("Identity").ToString()),
                If(row("Nationality") Is DBNull.Value, "", row("Nationality").ToString()),
                If(row("PhoneNumber") Is DBNull.Value, "", row("PhoneNumber").ToString()),
                If(row("Type") Is DBNull.Value, "", row("Type").ToString()),
                If(row("dateOfDelegation") Is DBNull.Value, DateTime.Now, Convert.ToDateTime(row("dateOfDelegation"))).ToString("dd/MM/yyyy"),
                If(row("expireOfDelegation") Is DBNull.Value, DateTime.Now.AddYears(1), Convert.ToDateTime(row("expireOfDelegation"))).ToString("dd/MM/yyyy"),
                If(row("dateOfPermision") Is DBNull.Value, DateTime.Now, Convert.ToDateTime(row("dateOfPermision"))).ToString("dd/MM/yyyy"),
                If(row("expireOfPermision") Is DBNull.Value, DateTime.Now.AddYears(1), Convert.ToDateTime(row("expireOfPermision"))).ToString("dd/MM/yyyy"),
                If(row("AssignedDate") Is DBNull.Value, DateTime.Now, Convert.ToDateTime(row("AssignedDate"))).ToString("dd/MM/yyyy")
            )
        Next

        ' Clear form fields when loading new data
        ClearFormFields()

        ' Force DataGridView to update display
        dataGDlistAllDelegators.Invalidate()
        dataGDlistAllDelegators.Update()
    End Sub

    Private Sub dataGDlistAllDelegators_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dataGDlistAllDelegators.CellClick
        If e.RowIndex >= 0 Then
            Dim selectedRow As DataGridViewRow = dataGDlistAllDelegators.Rows(e.RowIndex)

            ' Fill form fields with selected delegator data
            selectedDelegatorId = Convert.ToInt32(selectedRow.Cells("Id").Value)
            DelegatorNameTB.Text = selectedRow.Cells("Name").Value.ToString()
            DelegatorIdentityBT.Text = selectedRow.Cells("Identity").Value.ToString()
            DelegatorNationalityTB.Text = selectedRow.Cells("Nationality").Value.ToString()
            DelegatorPhoneNumberTB.Text = selectedRow.Cells("PhoneNumber").Value.ToString()
            DelegatorTypeTB.Text = selectedRow.Cells("Type").Value.ToString()

            ' Parse dates
            Try
                dateOfDelegateTimePicker.Value = Convert.ToDateTime(selectedRow.Cells("dateOfDelegation").Value)
                dateOfPermisionTimePicker.Value = Convert.ToDateTime(selectedRow.Cells("dateOfPermision").Value)
            Catch ex As Exception
                dateOfDelegateTimePicker.Value = DateTime.Now
                dateOfPermisionTimePicker.Value = DateTime.Now
            End Try

            isUpdatingDelegator = True

            ' Load permissions for the selected delegator
            LoadDelegatorPermissions(selectedDelegatorId)
        End If
    End Sub

    Private Sub AddDelegatorBT_Click(sender As Object, e As EventArgs) Handles AddDelegatorBT.Click
        ' Validation
        If String.IsNullOrWhiteSpace(DelegatorNameTB.Text) Then
            MessageBox.Show("يرجى إدخال اسم المفوض", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            DelegatorNameTB.Focus()
            Return
        End If

        If selectedCustomerId <= 0 Then
            MessageBox.Show("يرجى تحديد عميل أولاً من النموذج الرئيسي", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            ' Calculate expiry dates (1 year from delegation/permission dates)
            Dim expireOfDelegation As DateTime = dateOfDelegateTimePicker.Value.AddYears(1)
            Dim expireOfPermision As DateTime = dateOfPermisionTimePicker.Value.AddYears(1)

            If isUpdatingDelegator Then
                ' Update existing delegator
                Dim updateSuccess As Boolean = dbConn.UpdateDelegator(
                    selectedDelegatorId,
                    DelegatorNameTB.Text.Trim(),
                    selectedCustomerId,
                    DelegatorIdentityBT.Text.Trim(),
                    DelegatorNationalityTB.Text.Trim(),
                    DelegatorPhoneNumberTB.Text.Trim(),
                    DelegatorTypeTB.Text.Trim(),
                    dateOfDelegateTimePicker.Value,
                    expireOfDelegation,
                    dateOfPermisionTimePicker.Value,
                    expireOfPermision
                )

                If updateSuccess Then
                    MessageBox.Show("تم تحديث المفوض بنجاح", "نجح التحديث", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("فشل في تحديث المفوض", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If

                isUpdatingDelegator = False
                selectedDelegatorId = 0
            Else
                ' Add new delegator
                Dim newDelegatorId As Integer = dbConn.AddDelegator(
                    DelegatorNameTB.Text.Trim(),
                    selectedCustomerId,
                    DelegatorIdentityBT.Text.Trim(),
                    DelegatorNationalityTB.Text.Trim(),
                    DelegatorPhoneNumberTB.Text.Trim(),
                    DelegatorTypeTB.Text.Trim(),
                    dateOfDelegateTimePicker.Value,
                    expireOfDelegation,
                    dateOfPermisionTimePicker.Value,
                    expireOfPermision
                )

                If newDelegatorId > 0 Then
                    MessageBox.Show("تم إضافة المفوض بنجاح", "نجحت الإضافة", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("فشل في إضافة المفوض", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If

            ClearFormFields()
            LoadDelegatorsData(selectedCustomerId)

        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء العملية: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub RemoveDelegatorBT_Click(sender As Object, e As EventArgs) Handles RemoveDelegatorBT.Click
        If dataGDlistAllDelegators.CurrentRow Is Nothing Then
            MessageBox.Show("يرجى تحديد مفوض لحذفه", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim selectedRow As DataGridViewRow = dataGDlistAllDelegators.CurrentRow
        Dim delegatorId As Integer = Convert.ToInt32(selectedRow.Cells("Id").Value)
        Dim delegatorName As String = selectedRow.Cells("Name").Value.ToString()

        Dim result As DialogResult = MessageBox.Show(
            "هل أنت متأكد من حذف المفوض: " & delegatorName & "؟" & vbCrLf & "لا يمكن التراجع عن هذا الإجراء.",
            "تأكيد الحذف",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question
        )

        If result = DialogResult.Yes Then
            Dim deleteSuccess As Boolean = dbConn.RemoveDelegator(delegatorId)

            If deleteSuccess Then
                MessageBox.Show("تم حذف المفوض بنجاح", "نجح الحذف", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadDelegatorsData(selectedCustomerId)
                ClearFormFields()
            Else
                MessageBox.Show("فشل في حذف المفوض", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub

    Private Sub ClearFormFields()
        DelegatorNameTB.Clear()
        DelegatorIdentityBT.Clear()
        DelegatorNationalityTB.Clear()
        DelegatorPhoneNumberTB.Clear()
        DelegatorTypeTB.Clear()
        dateOfDelegateTimePicker.Value = DateTime.Now
        dateOfPermisionTimePicker.Value = DateTime.Now
        isUpdatingDelegator = False
        selectedDelegatorId = 0
        dataGDlistAllDelegators.ClearSelection()

        ' Load all permissions when no delegator is selected
        LoadDelegatorPermissions(0)
    End Sub

    ' =====================Permission Management Methods========================
    Private Sub LoadDelegatorPermissions(delegatorId As Integer)
        Try
            If delegatorId > 0 Then
                ' Load permissions already assigned to this delegator
                Dim assignedPermissions As DataTable = dbConn.GetDelegatorPermissions(delegatorId)
                LoadPermissionsIntoGrid(dataGDOfDivenPermissions, assignedPermissions, True)

                ' Load permissions available to grant to this delegator
                Dim availablePermissions As DataTable = dbConn.GetAvailablePermissionsForDelegator(delegatorId)
                LoadPermissionsIntoGrid(dataGDDelegations, availablePermissions, False)
            Else
                ' No delegator selected - show all permissions in the available list
                ClearPermissionGrids()
                Dim allPermissions As DataTable = dbConn.GetAllPermissions()
                LoadPermissionsIntoGrid(dataGDDelegations, allPermissions, False)

                ' Clear the assigned permissions grid
                LoadPermissionsIntoGrid(dataGDOfDivenPermissions, New DataTable(), True)
            End If

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل الصلاحيات: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadPermissionsIntoGrid(grid As DataGridView, dt As DataTable, includeAssignedDate As Boolean)
        ' Setup grid columns
        grid.Columns.Clear()
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        grid.MultiSelect = True  ' Enable multi-select for permission grids
        grid.ReadOnly = True
        grid.AllowUserToAddRows = False
        grid.AllowUserToDeleteRows = False
        grid.RowHeadersVisible = False
        grid.ColumnHeadersVisible = True
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        ' Style headers
        grid.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 10, FontStyle.Bold)
        grid.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray
        grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        ' Style data
        grid.DefaultCellStyle.Font = New Font("Arial", 9, FontStyle.Regular)
        grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

        ' Add columns
        grid.Columns.Add("Id", "رقم")
        grid.Columns.Add("Name", "اسم الصلاحية")
        If includeAssignedDate Then
            grid.Columns.Add("AssignedDate", "تاريخ المنح")
        End If

        ' Hide ID column
        grid.Columns("Id").Width = 50
        grid.Columns("Id").Visible = False

        ' Load data
        grid.Rows.Clear()
        For Each row As DataRow In dt.Rows
            If includeAssignedDate Then
                grid.Rows.Add(
                    row("Id"),
                    row("Name"),
                    If(row("AssignedDate") Is DBNull.Value, "", Convert.ToDateTime(row("AssignedDate")).ToString("dd/MM/yyyy"))
                )
            Else
                grid.Rows.Add(
                    row("Id"),
                    row("Name")
                )
            End If
        Next

        grid.Refresh()
    End Sub

    Private Sub ClearPermissionGrids()
        If dataGDOfDivenPermissions IsNot Nothing Then
            dataGDOfDivenPermissions.Rows.Clear()
        End If
        If dataGDDelegations IsNot Nothing Then
            dataGDDelegations.Rows.Clear()
        End If
    End Sub

    ' =====================Grant Permission Button Event========================
    Private Sub DelegateToCustomerBT_Click(sender As Object, e As EventArgs) Handles DelegateToCustomerBT.Click
        ' Check if a delegator is selected
        If selectedDelegatorId <= 0 Then
            MessageBox.Show("يرجى تحديد مفوض أولاً", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Check if permissions are selected from available permissions grid
        If dataGDDelegations.SelectedRows.Count = 0 Then
            MessageBox.Show("يرجى تحديد صلاحية أو أكثر من قائمة الصلاحيات المتاحة", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Dim successCount As Integer = 0
            Dim failCount As Integer = 0
            Dim permissionNames As New List(Of String)

            ' Process each selected permission
            For Each selectedRow As DataGridViewRow In dataGDDelegations.SelectedRows
                Dim permissionId As Integer = Convert.ToInt32(selectedRow.Cells("Id").Value)
                Dim permissionName As String = selectedRow.Cells("Name").Value.ToString()
                permissionNames.Add(permissionName)

                ' Grant the permission
                Dim success As Boolean = dbConn.GrantPermissionToDelegator(selectedDelegatorId, permissionId)
                If success Then
                    successCount += 1
                Else
                    failCount += 1
                End If
            Next

            ' Show result message
            If successCount > 0 And failCount = 0 Then
                If successCount = 1 Then
                    MessageBox.Show($"تم منح الصلاحية '{permissionNames(0)}' بنجاح", "نجح المنح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show($"تم منح {successCount} صلاحيات بنجاح", "نجح المنح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            ElseIf successCount > 0 And failCount > 0 Then
                MessageBox.Show($"تم منح {successCount} صلاحيات بنجاح، وفشل في منح {failCount} صلاحيات", "منح جزئي", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Else
                MessageBox.Show("فشل في منح جميع الصلاحيات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

            ' Refresh permission grids
            LoadDelegatorPermissions(selectedDelegatorId)

        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء منح الصلاحيات: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =====================Revoke Permission Button Event========================
    Private Sub unDelegateFromCustomerBT_Click(sender As Object, e As EventArgs) Handles unDelegateFromCustomerBT.Click
        ' Check if a delegator is selected
        If selectedDelegatorId <= 0 Then
            MessageBox.Show("يرجى تحديد مفوض أولاً", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Check if permissions are selected from assigned permissions grid
        If dataGDOfDivenPermissions.SelectedRows.Count = 0 Then
            MessageBox.Show("يرجى تحديد صلاحية أو أكثر من قائمة الصلاحيات الممنوحة", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Dim permissionNames As New List(Of String)
            For Each selectedRow As DataGridViewRow In dataGDOfDivenPermissions.SelectedRows
                permissionNames.Add(selectedRow.Cells("Name").Value.ToString())
            Next

            ' Confirm revocation
            Dim message As String
            If permissionNames.Count = 1 Then
                message = $"هل أنت متأكد من إلغاء الصلاحية '{permissionNames(0)}'؟"
            Else
                message = $"هل أنت متأكد من إلغاء {permissionNames.Count} صلاحيات؟"
            End If

            Dim result As DialogResult = MessageBox.Show(message, "تأكيد الإلغاء", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = DialogResult.Yes Then
                Dim successCount As Integer = 0
                Dim failCount As Integer = 0

                ' Process each selected permission
                For Each selectedRow As DataGridViewRow In dataGDOfDivenPermissions.SelectedRows
                    Dim permissionId As Integer = Convert.ToInt32(selectedRow.Cells("Id").Value)

                    ' Revoke the permission
                    Dim success As Boolean = dbConn.RevokePermissionFromDelegator(selectedDelegatorId, permissionId)
                    If success Then
                        successCount += 1
                    Else
                        failCount += 1
                    End If
                Next

                ' Show result message
                If successCount > 0 And failCount = 0 Then
                    If successCount = 1 Then
                        MessageBox.Show($"تم إلغاء الصلاحية '{permissionNames(0)}' بنجاح", "نجح الإلغاء", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        MessageBox.Show($"تم إلغاء {successCount} صلاحيات بنجاح", "نجح الإلغاء", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                ElseIf successCount > 0 And failCount > 0 Then
                    MessageBox.Show($"تم إلغاء {successCount} صلاحيات بنجاح، وفشل في إلغاء {failCount} صلاحيات", "إلغاء جزئي", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Else
                    MessageBox.Show("فشل في إلغاء جميع الصلاحيات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If

                ' Refresh permission grids
                LoadDelegatorPermissions(selectedDelegatorId)
            End If

        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء إلغاء الصلاحيات: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =====================Customer Name Label Update========================
    Private Sub UpdateCustomerNameLabel()
        Try
            If selectedCustomerId > 0 Then
                Dim customerName As String = dbConn.GetCustomerName(selectedCustomerId)
                If Not String.IsNullOrEmpty(customerName) Then
                    CustomerNameLable.Text = $"العميل: {customerName}"
                Else
                    CustomerNameLable.Text = $"العميل رقم: {selectedCustomerId}"
                End If
            Else
                CustomerNameLable.Text = "لم يتم تحديد عميل"
            End If
        Catch ex As Exception
            CustomerNameLable.Text = "خطأ في تحميل اسم العميل"
        End Try
    End Sub

    ' =====================Permission Management Button Event========================
    Private Sub AddPermissionBT_Click(sender As Object, e As EventArgs) Handles AddPermissionBT.Click
        ' Validation
        If String.IsNullOrWhiteSpace(AddPermissionTB.Text) Then
            MessageBox.Show("يرجى إدخال اسم الصلاحية", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            AddPermissionTB.Focus()
            Return
        End If

        ' Check minimum length
        If AddPermissionTB.Text.Trim().Length < 2 Then
            MessageBox.Show("يجب أن يكون اسم الصلاحية أكثر من حرف واحد", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            AddPermissionTB.Focus()
            Return
        End If

        Try
            If isUpdatingPermission Then
                ' Update existing permission
                Dim updateSuccess As Boolean = dbConn.UpdatePermission(selectedPermissionId, AddPermissionTB.Text.Trim())

                If updateSuccess Then
                    MessageBox.Show("تم تحديث الصلاحية بنجاح", "نجح التحديث", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    ' Reset update mode
                    isUpdatingPermission = False
                    selectedPermissionId = 0
                    AddPermissionBT.Text = "إضافة صلاحية"
                Else
                    MessageBox.Show("فشل في تحديث الصلاحية", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Else
                ' Add new permission
                Dim newPermissionId As Integer = dbConn.AddPermission(AddPermissionTB.Text.Trim())

                If newPermissionId > 0 Then
                    MessageBox.Show("تم إضافة الصلاحية بنجاح", "نجحت الإضافة", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ElseIf newPermissionId = -1 Then
                    MessageBox.Show("هذه الصلاحية موجودة مسبقاً", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    AddPermissionTB.Focus()
                    AddPermissionTB.SelectAll()
                    Return
                Else
                    MessageBox.Show("فشل في إضافة الصلاحية", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If

            ' Clear the textbox and refresh permission grids
            AddPermissionTB.Clear()

            ' Refresh permission grids
            LoadDelegatorPermissions(selectedDelegatorId)

        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء العملية: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =====================Permission Grid Click Events for Editing========================
    Private Sub dataGDDelegations_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dataGDDelegations.CellDoubleClick
        If e.RowIndex >= 0 Then
            EditSelectedPermission(dataGDDelegations.Rows(e.RowIndex))
        End If
    End Sub

    Private Sub dataGDOfDivenPermissions_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dataGDOfDivenPermissions.CellDoubleClick
        If e.RowIndex >= 0 Then
            EditSelectedPermission(dataGDOfDivenPermissions.Rows(e.RowIndex))
        End If
    End Sub

    Private Sub EditSelectedPermission(selectedRow As DataGridViewRow)
        Try
            selectedPermissionId = Convert.ToInt32(selectedRow.Cells("Id").Value)
            Dim permissionName As String = selectedRow.Cells("Name").Value.ToString()

            ' Fill the textbox with the permission name for editing
            AddPermissionTB.Text = permissionName

            ' Set update mode
            isUpdatingPermission = True
            AddPermissionBT.Text = "تحديث الصلاحية"

            ' Focus on the textbox
            AddPermissionTB.Focus()
            AddPermissionTB.SelectAll()

        Catch ex As Exception
            MessageBox.Show("خطأ في تحديد الصلاحية للتحديث: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =====================Clear Permission Update Mode========================
    Private Sub AddPermissionTB_TextChanged(sender As Object, e As EventArgs) Handles AddPermissionTB.TextChanged
        ' If textbox is cleared, reset update mode
        If String.IsNullOrEmpty(AddPermissionTB.Text) And isUpdatingPermission Then
            isUpdatingPermission = False
            selectedPermissionId = 0
            AddPermissionBT.Text = "إضافة صلاحية"
        End If
    End Sub

    ' =====================Remove Permission Button Event========================
    Private Sub RemovePermissionBT_Click(sender As Object, e As EventArgs) Handles RemovePermissionBT.Click
        ' Check if permissions are selected from either grid
        Dim selectedPermissions As New List(Of DataGridViewRow)

        ' Collect selected permissions from both grids
        For Each selectedRow As DataGridViewRow In dataGDDelegations.SelectedRows
            selectedPermissions.Add(selectedRow)
        Next

        For Each selectedRow As DataGridViewRow In dataGDOfDivenPermissions.SelectedRows
            selectedPermissions.Add(selectedRow)
        Next

        If selectedPermissions.Count = 0 Then
            MessageBox.Show("يرجى تحديد صلاحية أو أكثر لحذفها من أي من الجدولين", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            ' Collect permission names for confirmation message
            Dim permissionNames As New List(Of String)
            For Each selectedRow As DataGridViewRow In selectedPermissions
                permissionNames.Add(selectedRow.Cells("Name").Value.ToString())
            Next

            ' Confirm deletion
            Dim message As String
            If permissionNames.Count = 1 Then
                message = $"هل أنت متأكد من حذف الصلاحية '{permissionNames(0)}'؟" & vbCrLf &
                         "سيؤدي هذا إلى إزالتها من جميع المفوضين أيضاً." & vbCrLf &
                         "لا يمكن التراجع عن هذا الإجراء."
            Else
                message = $"هل أنت متأكد من حذف {permissionNames.Count} صلاحيات؟" & vbCrLf &
                         "سيؤدي هذا إلى إزالتها من جميع المفوضين أيضاً." & vbCrLf &
                         "لا يمكن التراجع عن هذا الإجراء."
            End If

            Dim result As DialogResult = MessageBox.Show(message, "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = DialogResult.Yes Then
                Dim successCount As Integer = 0
                Dim failCount As Integer = 0

                ' Process each selected permission
                For Each selectedRow As DataGridViewRow In selectedPermissions
                    Dim permissionId As Integer = Convert.ToInt32(selectedRow.Cells("Id").Value)

                    ' Remove the permission
                    Dim success As Boolean = dbConn.RemovePermission(permissionId)
                    If success Then
                        successCount += 1
                    Else
                        failCount += 1
                    End If
                Next

                ' Show result message
                If successCount > 0 And failCount = 0 Then
                    If successCount = 1 Then
                        MessageBox.Show($"تم حذف الصلاحية '{permissionNames(0)}' بنجاح", "نجح الحذف", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        MessageBox.Show($"تم حذف {successCount} صلاحيات بنجاح", "نجح الحذف", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                ElseIf successCount > 0 And failCount > 0 Then
                    MessageBox.Show($"تم حذف {successCount} صلاحيات بنجاح، وفشل في حذف {failCount} صلاحيات", "حذف جزئي", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Else
                    MessageBox.Show("فشل في حذف جميع الصلاحيات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If

                ' Clear update mode if we were editing one of the deleted permissions
                If isUpdatingPermission Then
                    For Each selectedRow As DataGridViewRow In selectedPermissions
                        Dim permissionId As Integer = Convert.ToInt32(selectedRow.Cells("Id").Value)
                        If permissionId = selectedPermissionId Then
                            ' Reset update mode
                            isUpdatingPermission = False
                            selectedPermissionId = 0
                            AddPermissionBT.Text = "إضافة صلاحية"
                            AddPermissionTB.Clear()
                            Exit For
                        End If
                    Next
                End If

                ' Refresh permission grids
                LoadDelegatorPermissions(selectedDelegatorId)
            End If

        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء حذف الصلاحيات: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Clear selection when clicking outside the DataGridViews
    Private Sub DelegationsManagement_Click(sender As Object, e As EventArgs) Handles Me.Click
        ClearAllSelections()
        ClearFormFieldsAndResetPermissions()
    End Sub

    ' Clear all DataGridView selections
    Private Sub ClearAllSelections()
        If dataGDlistAllDelegators.CurrentRow IsNot Nothing Then
            dataGDlistAllDelegators.ClearSelection()
        End If
        If dataGDDelegations.SelectedRows.Count > 0 Then
            dataGDDelegations.ClearSelection()
        End If
        If dataGDOfDivenPermissions.SelectedRows.Count > 0 Then
            dataGDOfDivenPermissions.ClearSelection()
        End If
    End Sub

    ' Handle when user clicks outside of dataGDlistAllDelegators
    Private Sub dataGDlistAllDelegators_SelectionChanged(sender As Object, e As EventArgs) Handles dataGDlistAllDelegators.SelectionChanged
        ' If no rows are selected (user clicked outside), clear everything
        If dataGDlistAllDelegators.SelectedRows.Count = 0 AndAlso dataGDlistAllDelegators.CurrentRow Is Nothing Then
            ClearFormFieldsAndResetPermissions()
        End If
    End Sub

    ' Clear selection when clicking on other controls (except those that need selection)
    Private Sub ClearSelectionOnControlClick(sender As Object, e As EventArgs) Handles DelegatorNameTB.Click, DelegatorIdentityBT.Click, DelegatorNationalityTB.Click, DelegatorPhoneNumberTB.Click, DelegatorTypeTB.Click, AddPermissionTB.Click
        ' Clear selections and reset when clicking on input controls
        If dataGDlistAllDelegators.SelectedRows.Count > 0 OrElse dataGDlistAllDelegators.CurrentRow IsNot Nothing Then
            dataGDlistAllDelegators.ClearSelection()
            ClearFormFieldsAndResetPermissions()
        End If
    End Sub

    ' Clear form fields and reset permission grids when clicking outside delegators grid
    Private Sub ClearFormFieldsAndResetPermissions()
        ' Clear form fields
        ClearFormFields()
        
        ' Clear dataGDOfDivenPermissions
        If dataGDOfDivenPermissions IsNot Nothing Then
            dataGDOfDivenPermissions.Rows.Clear()
            dataGDOfDivenPermissions.Refresh()
        End If
        
        ' Load all permissions into dataGDDelegations (restore original functionality)
        LoadAllPermissionsIntoGrid()
    End Sub

    ' Load all permissions into dataGDDelegations grid
    Private Sub LoadAllPermissionsIntoGrid()
        Try
            Dim allPermissions As DataTable = dbConn.GetAllPermissions()
            LoadPermissionsIntoGrid(dataGDDelegations, allPermissions, False)
        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل الصلاحيات: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

End Class