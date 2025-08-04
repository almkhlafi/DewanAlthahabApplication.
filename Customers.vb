Imports System.Linq

Public Class Customers

    Private dbConn As New DBconnections()

    ' Variables to track selected customer from search dialog
    Private currentSelectedCustomerId As Integer = 0
    Private currentSelectedCustomerName As String = ""

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Form initialization
        ' Setup NotifyIcon
        NotifyIcon1.Icon = Me.Icon
        NotifyIcon1.Visible = True
        NotifyIcon1.Text = "العملاء"

        ' Load countries into CountryCB
        LoadCountries()

        ' Setup and load branches DataGridView
        SetupBranchesDataGridView()
        LoadBranches()

        ' Setup and load currency DataGridView
        SetupCurrencyDataGridView()
        LoadCurrency()
    End Sub

    Private Sub LoadCountries()
        Try
            ' Load countries from CMGADB2024 database
            Dim countriesTable As DataTable = dbConn.GetCountries()

            ' Clear existing items
            CountryCB.Items.Clear()

            ' Set up CountryCB properties for enhanced search functionality
            CountryCB.DisplayMember = "DisplayText"
            CountryCB.ValueMember = "countrycode"
            CountryCB.AutoCompleteMode = AutoCompleteMode.Suggest
            CountryCB.AutoCompleteSource = AutoCompleteSource.ListItems
            CountryCB.DropDownStyle = ComboBoxStyle.DropDown

            ' Create a new DataTable with combined display text
            Dim displayTable As New DataTable()
            displayTable.Columns.Add("countrycode", GetType(String))
            displayTable.Columns.Add("DisplayText", GetType(String))

            ' Add countries with combined display format
            For Each row As DataRow In countriesTable.Rows
                Dim newRow As DataRow = displayTable.NewRow()
                newRow("countrycode") = row("countrycode").ToString()
                newRow("DisplayText") = $"{row("countrycode")} - {row("contryarname")} - {row("countryName")}"
                displayTable.Rows.Add(newRow)
            Next

            ' Bind to ComboBox
            CountryCB.DataSource = displayTable

            ' Store original data for filtering
            CountryCB.Tag = displayTable

            ' Set up additional search functionality
            SetupCountrySearch()

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل البلدان: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadAreas(countryCode As String)
        Try
            ' Load areas from CMGADB2024 database for specific country
            Dim areasTable As DataTable = dbConn.GetAreas(countryCode)

            ' Clear existing items
            AreaCB.Items.Clear()

            ' Set up AreaCB properties for enhanced search functionality
            AreaCB.DisplayMember = "DisplayText"
            AreaCB.ValueMember = "areacode"
            AreaCB.AutoCompleteMode = AutoCompleteMode.Suggest
            AreaCB.AutoCompleteSource = AutoCompleteSource.ListItems
            AreaCB.DropDownStyle = ComboBoxStyle.DropDown

            ' Create a new DataTable with combined display text
            Dim displayTable As New DataTable()
            displayTable.Columns.Add("areacode", GetType(String))
            displayTable.Columns.Add("DisplayText", GetType(String))

            ' Add areas to ComboBox with combined display format
            For Each row As DataRow In areasTable.Rows
                Dim newRow As DataRow = displayTable.NewRow()
                newRow("areacode") = row("areacode").ToString()
                
                ' Create display text with area code, description, and short name
                Dim displayText As String = $"{row("areacode")} - {row("description")}"
                If Not String.IsNullOrEmpty(row("shortname").ToString()) Then
                    displayText += $" - {row("shortname")}"
                End If
                
                newRow("DisplayText") = displayText
                displayTable.Rows.Add(newRow)
            Next

            ' Bind to ComboBox
            AreaCB.DataSource = displayTable

            ' Store original data for filtering
            AreaCB.Tag = displayTable

            ' Set up additional search functionality
            SetupAreaSearch()

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل المناطق: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SetupCountrySearch()
        ' Enable advanced search functionality for CountryCB
        AddHandler CountryCB.KeyUp, AddressOf CountryCB_KeyUp
        AddHandler CountryCB.TextChanged, AddressOf CountryCB_TextChanged
        AddHandler CountryCB.Leave, AddressOf CountryCB_Leave
    End Sub

    Private Sub SetupAreaSearch()
        ' Enable advanced search functionality for AreaCB
        AddHandler AreaCB.KeyUp, AddressOf AreaCB_KeyUp
        AddHandler AreaCB.TextChanged, AddressOf AreaCB_TextChanged
        AddHandler AreaCB.Leave, AddressOf AreaCB_Leave
    End Sub

    Private Sub CountryCB_KeyUp(sender As Object, e As KeyEventArgs)
        ' Handle key navigation in dropdown
        If e.KeyCode = Keys.Enter Then
            CountryCB.DroppedDown = False
        ElseIf e.KeyCode = Keys.Down OrElse e.KeyCode = Keys.Up Then
            CountryCB.DroppedDown = True
        End If
    End Sub

    Private Sub CountryCB_TextChanged(sender As Object, e As EventArgs)
        ' Enhanced search functionality - filter items based on typed text
        Try
            Dim searchText As String = CountryCB.Text.ToLower()
            If String.IsNullOrEmpty(searchText) Then Return

            ' Get the original data
            Dim originalData As DataTable = CType(CountryCB.Tag, DataTable)
            If originalData Is Nothing Then Return

            ' Filter the data based on search text
            Dim filteredRows = originalData.AsEnumerable().Where(Function(row)
                Dim displayText As String = row("DisplayText").ToString().ToLower()
                Return displayText.Contains(searchText)
            End Function)

            If filteredRows.Any() Then
                ' Create filtered DataTable
                Dim filteredTable As DataTable = originalData.Clone()
                For Each row In filteredRows
                    filteredTable.ImportRow(row)
                Next

                ' Update ComboBox with filtered data
                CountryCB.DataSource = filteredTable
                CountryCB.DroppedDown = True

                ' Move cursor to end of text
                CountryCB.SelectionStart = CountryCB.Text.Length
                CountryCB.SelectionLength = 0
            End If

        Catch ex As Exception
            ' Ignore any errors during text change
        End Try
    End Sub

    Private Sub AreaCB_KeyUp(sender As Object, e As KeyEventArgs)
        ' Handle key navigation in dropdown
        If e.KeyCode = Keys.Enter Then
            AreaCB.DroppedDown = False
        ElseIf e.KeyCode = Keys.Down OrElse e.KeyCode = Keys.Up Then
            AreaCB.DroppedDown = True
        End If
    End Sub

    Private Sub AreaCB_TextChanged(sender As Object, e As EventArgs)
        ' Enhanced search functionality - filter items based on typed text
        Try
            Dim searchText As String = AreaCB.Text.ToLower()
            If String.IsNullOrEmpty(searchText) Then Return

            ' Get the original data
            Dim originalData As DataTable = CType(AreaCB.Tag, DataTable)
            If originalData Is Nothing Then Return

            ' Filter the data based on search text
            Dim filteredRows = originalData.AsEnumerable().Where(Function(row)
                Dim displayText As String = row("DisplayText").ToString().ToLower()
                Return displayText.Contains(searchText)
            End Function)

            If filteredRows.Any() Then
                ' Create filtered DataTable
                Dim filteredTable As DataTable = originalData.Clone()
                For Each row In filteredRows
                    filteredTable.ImportRow(row)
                Next

                ' Update ComboBox with filtered data
                AreaCB.DataSource = filteredTable
                AreaCB.DroppedDown = True

                ' Move cursor to end of text
                AreaCB.SelectionStart = AreaCB.Text.Length
                AreaCB.SelectionLength = 0
            End If

        Catch ex As Exception
            ' Ignore any errors during text change
        End Try
    End Sub

    Private Sub CountryCB_Leave(sender As Object, e As EventArgs)
        ' Restore full list when leaving the control
        Try
            Dim originalData As DataTable = CType(CountryCB.Tag, DataTable)
            If originalData IsNot Nothing Then
                CountryCB.DataSource = originalData
            End If
        Catch ex As Exception
            ' Ignore errors
        End Try
    End Sub

    Private Sub AreaCB_Leave(sender As Object, e As EventArgs)
        ' Restore full list when leaving the control
        Try
            Dim originalData As DataTable = CType(AreaCB.Tag, DataTable)
            If originalData IsNot Nothing Then
                AreaCB.DataSource = originalData
            End If
        Catch ex As Exception
            ' Ignore errors
        End Try
    End Sub

    Private Sub CountryCB_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CountryCB.SelectedIndexChanged
        Try
            If CountryCB.SelectedItem IsNot Nothing Then
                Dim selectedCountry As DataRowView = CType(CountryCB.SelectedItem, DataRowView)
                Dim countryCode As String = selectedCountry("countrycode").ToString()
                Dim displayText As String = selectedCountry("DisplayText").ToString()

                ' Check if Saudi Arabia is selected (looking for SA or السعودية in the display text)
                If countryCode = "SA" OrElse displayText.Contains("السعودية") OrElse displayText.Contains("Saudi") Then
                    ' Auto-populate AreaCB with areas from database for Saudi Arabia
                    LoadAreas(countryCode)
                Else
                    ' Clear AreaCB for other countries
                    AreaCB.DataSource = Nothing
                    AreaCB.Items.Clear()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("خطأ في تحديد البلد: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SetupBranchesDataGridView()
        ' Clear existing columns
        BranchesInfoDGV.Columns.Clear()

        ' Set basic properties
        BranchesInfoDGV.AllowUserToAddRows = False
        BranchesInfoDGV.AllowUserToDeleteRows = False
        BranchesInfoDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        BranchesInfoDGV.MultiSelect = True
        BranchesInfoDGV.ReadOnly = False
        BranchesInfoDGV.RowHeadersVisible = False
        BranchesInfoDGV.RightToLeft = RightToLeft.Yes

        ' Add checkbox column for selection
        Dim selectColumn As New DataGridViewCheckBoxColumn()
        selectColumn.Name = "Select"
        selectColumn.HeaderText = "الكل"
        selectColumn.Width = 50
        selectColumn.ReadOnly = False
        BranchesInfoDGV.Columns.Add(selectColumn)

        ' Add branch name arabic column
        Dim branchArabicColumn As New DataGridViewTextBoxColumn()
        branchArabicColumn.Name = "BranchArabic"
        branchArabicColumn.HeaderText = "اسم الفرع عربي"
        branchArabicColumn.ReadOnly = True
        branchArabicColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        BranchesInfoDGV.Columns.Add(branchArabicColumn)

        ' Add branch name english column
        Dim branchEnglishColumn As New DataGridViewTextBoxColumn()
        branchEnglishColumn.Name = "BranchEnglish"
        branchEnglishColumn.HeaderText = "اسم الفرع انجليزي"
        branchEnglishColumn.ReadOnly = True
        branchEnglishColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        BranchesInfoDGV.Columns.Add(branchEnglishColumn)

        ' Add branch code column
        Dim branchCodeColumn As New DataGridViewTextBoxColumn()
        branchCodeColumn.Name = "BranchCode"
        branchCodeColumn.HeaderText = "الكود الاساسي"
        branchCodeColumn.ReadOnly = True
        branchCodeColumn.Width = 100
        BranchesInfoDGV.Columns.Add(branchCodeColumn)

        ' Add active status column with checkbox
        Dim activeColumn As New DataGridViewCheckBoxColumn()
        activeColumn.Name = "Active"
        activeColumn.HeaderText = "مفعل / غير مفعل"
        activeColumn.ReadOnly = False
        activeColumn.Width = 120
        activeColumn.TrueValue = True
        activeColumn.FalseValue = False
        BranchesInfoDGV.Columns.Add(activeColumn)

        ' Add ref number column (placeholder for now)
        Dim refNoColumn As New DataGridViewTextBoxColumn()
        refNoColumn.Name = "RefNo"
        refNoColumn.HeaderText = "Ref NO"
        refNoColumn.ReadOnly = True
        refNoColumn.Width = 80
        BranchesInfoDGV.Columns.Add(refNoColumn)

        ' Add locked column with checkbox
        Dim lockedColumn As New DataGridViewCheckBoxColumn()
        lockedColumn.Name = "Locked"
        lockedColumn.HeaderText = "Locked"
        lockedColumn.ReadOnly = False
        lockedColumn.Width = 80
        lockedColumn.TrueValue = True
        lockedColumn.FalseValue = False
        BranchesInfoDGV.Columns.Add(lockedColumn)

        ' Add header checkbox for select all
        AddHandler BranchesInfoDGV.CellContentClick, AddressOf BranchesInfoDGV_CellContentClick
        AddHandler BranchesInfoDGV.ColumnHeaderMouseClick, AddressOf BranchesInfoDGV_ColumnHeaderMouseClick
    End Sub

    Private Sub LoadBranches()
        Try
            ' Load branches from CMGADB2024 database
            Dim branchesTable As DataTable = dbConn.GetBranches()

            ' Clear existing rows
            BranchesInfoDGV.Rows.Clear()

            ' Add branches to DataGridView with default values
            For Each row As DataRow In branchesTable.Rows
                Dim newRow As Object() = {
                    False, ' Select checkbox (default unchecked)
                    row("branch_arabic").ToString(), ' Arabic name
                    row("branch_name").ToString(), ' English name
                    row("branch_code").ToString(), ' Branch code
                    True, ' Active status (default checked - will be saved to fld_active_branch)
                    "", ' Ref NO (empty default - will be saved to fld_ref_no_branch)
                    False ' Locked status (default unchecked - will be saved to fld_loacked)
                }
                BranchesInfoDGV.Rows.Add(newRow)
            Next

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل الفروع: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BranchesInfoDGV_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
        ' Handle checkbox clicks in checkbox columns
        If e.RowIndex >= 0 Then
            Dim columnName As String = BranchesInfoDGV.Columns(e.ColumnIndex).Name
            If columnName = "Select" OrElse columnName = "Active" OrElse columnName = "Locked" Then
                BranchesInfoDGV.EndEdit()
            End If
        End If
    End Sub

    Private Sub BranchesInfoDGV_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs)
        ' Handle "select all" functionality when clicking on the header checkbox
        If e.ColumnIndex = 0 Then
            Dim selectAll As Boolean = True

            ' Check if any row is currently selected to determine toggle behavior
            For Each row As DataGridViewRow In BranchesInfoDGV.Rows
                If CBool(row.Cells(0).Value) = True Then
                    selectAll = False
                    Exit For
                End If
            Next

            ' Set all checkboxes to the determined state
            For Each row As DataGridViewRow In BranchesInfoDGV.Rows
                row.Cells(0).Value = selectAll
            Next
        End If
    End Sub

    ' Helper method to get selected branches
    Public Function GetSelectedBranches() As List(Of String)
        Dim selectedBranches As New List(Of String)()

        For Each row As DataGridViewRow In BranchesInfoDGV.Rows
            If CBool(row.Cells("Select").Value) = True Then
                selectedBranches.Add(row.Cells("BranchCode").Value.ToString())
            End If
        Next

        Return selectedBranches
    End Function

    ' Helper method to get active branches
    Public Function GetActiveBranches() As List(Of String)
        Dim activeBranches As New List(Of String)()

        For Each row As DataGridViewRow In BranchesInfoDGV.Rows
            If CBool(row.Cells("Active").Value) = True Then
                activeBranches.Add(row.Cells("BranchCode").Value.ToString())
            End If
        Next

        Return activeBranches
    End Function

    ' Helper method to get locked branches
    Public Function GetLockedBranches() As List(Of String)
        Dim lockedBranches As New List(Of String)()

        For Each row As DataGridViewRow In BranchesInfoDGV.Rows
            If CBool(row.Cells("Locked").Value) = True Then
                lockedBranches.Add(row.Cells("BranchCode").Value.ToString())
            End If
        Next

        Return lockedBranches
    End Function

    ' Method to save branch changes to database
    Public Sub SaveBranchChanges()
        Try
            For Each row As DataGridViewRow In BranchesInfoDGV.Rows
                Dim branchCode As String = row.Cells("BranchCode").Value.ToString()
                Dim isSelected As Boolean = CBool(row.Cells("Select").Value)
                Dim isActive As Boolean = CBool(row.Cells("Active").Value)
                Dim isLocked As Boolean = CBool(row.Cells("Locked").Value)
                Dim refNo As String = row.Cells("RefNo").Value.ToString()

                dbConn.UpdateBranchSelection(branchCode, isSelected, isActive, isLocked, refNo)
            Next

            MessageBox.Show("تم حفظ تغييرات الفروع بنجاح!", "نجح الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("خطأ في حفظ تغييرات الفروع: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SetupCurrencyDataGridView()
        ' Clear existing columns
        CurrencyDGV.Columns.Clear()

        ' Set basic properties
        CurrencyDGV.AllowUserToAddRows = False
        CurrencyDGV.AllowUserToDeleteRows = False
        CurrencyDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        CurrencyDGV.MultiSelect = True
        CurrencyDGV.ReadOnly = False
        CurrencyDGV.RowHeadersVisible = False
        CurrencyDGV.RightToLeft = RightToLeft.Yes

        ' Add checkbox column for selection
        Dim selectColumn As New DataGridViewCheckBoxColumn()
        selectColumn.Name = "Select"
        selectColumn.HeaderText = "الكل"
        selectColumn.Width = 50
        selectColumn.ReadOnly = False
        CurrencyDGV.Columns.Add(selectColumn)

        ' Add currency code column
        Dim currencyCodeColumn As New DataGridViewTextBoxColumn()
        currencyCodeColumn.Name = "CurrencyCode"
        currencyCodeColumn.HeaderText = "الرمز"
        currencyCodeColumn.ReadOnly = True
        currencyCodeColumn.Width = 80
        CurrencyDGV.Columns.Add(currencyCodeColumn)

        ' Add currency name column
        Dim currencyNameColumn As New DataGridViewTextBoxColumn()
        currencyNameColumn.Name = "CurrencyName"
        currencyNameColumn.HeaderText = "الاسم"
        currencyNameColumn.ReadOnly = True
        currencyNameColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        CurrencyDGV.Columns.Add(currencyNameColumn)

        ' Add currency decimal/price column
        Dim currencyDecimalColumn As New DataGridViewTextBoxColumn()
        currencyDecimalColumn.Name = "CurrencyDecimal"
        currencyDecimalColumn.HeaderText = "السعر"
        currencyDecimalColumn.ReadOnly = True
        currencyDecimalColumn.Width = 100
        CurrencyDGV.Columns.Add(currencyDecimalColumn)

        ' Add event handlers
        AddHandler CurrencyDGV.CellContentClick, AddressOf DataGridView1_CellContentClick
        AddHandler CurrencyDGV.ColumnHeaderMouseClick, AddressOf DataGridView1_ColumnHeaderMouseClick
    End Sub

    Private Sub LoadCurrency()
        Try
            ' Load currency from CMGADB2024 database
            Dim currencyTable As DataTable = dbConn.GetCurrency()

            ' Clear existing rows
            CurrencyDGV.Rows.Clear()

            ' Add currency to DataGridView
            For Each row As DataRow In currencyTable.Rows
                Dim newRow As Object() = {
                    False, ' Select checkbox (unchecked by default)
                    row("suffix_code").ToString(), ' Currency code
                    row("arabicname").ToString(), ' Arabic name
                    row("decimal").ToString() ' Decimal/Price value
                }
                CurrencyDGV.Rows.Add(newRow)
            Next

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل العملات: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
        ' Handle checkbox clicks in the Select column
        If e.RowIndex >= 0 AndAlso e.ColumnIndex = 0 Then
            CurrencyDGV.EndEdit()
        End If
    End Sub

    Private Sub DataGridView1_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs)
        ' Handle "select all" functionality when clicking on the header checkbox
        If e.ColumnIndex = 0 Then
            Dim selectAll As Boolean = True

            ' Check if any row is currently selected to determine toggle behavior
            For Each row As DataGridViewRow In CurrencyDGV.Rows
                If CBool(row.Cells(0).Value) = True Then
                    selectAll = False
                    Exit For
                End If
            Next

            ' Set all checkboxes to the determined state
            For Each row As DataGridViewRow In CurrencyDGV.Rows
                row.Cells(0).Value = selectAll
            Next
        End If
    End Sub

    ' Helper method to get selected currencies
    Public Function GetSelectedCurrencies() As List(Of String)
        Dim selectedCurrencies As New List(Of String)()

        For Each row As DataGridViewRow In CurrencyDGV.Rows
            If CBool(row.Cells("Select").Value) = True Then
                selectedCurrencies.Add(row.Cells("CurrencyCode").Value.ToString())
            End If
        Next

        Return selectedCurrencies
    End Function


    Private Sub AddAttachmentsBT_Click(sender As Object, e As EventArgs) Handles AddAttachmentsBT.Click
        ' Check if a customer is selected - if not, show search dialog first
        '  If currentSelectedCustomerId = 0 Then
        ' No customer selected - show customer search dialog
        '  Dim searchForm As New CustomerSearchForm

        ' If searchForm.ShowDialog = DialogResult.OK Then
        ' Customer was selected from search - store customer info
        '     currentSelectedCustomerId = searchForm.SelectedCustomerId
        '    currentSelectedCustomerName = searchForm.SelectedCustomerName
        '  End If
        '
        ' searchForm.Dispose()
        '  End If

        ' Open attachments management (regardless of customer selection)
        AttachmentsManagement.ShowDialog()
    End Sub

    Private Sub DocumentsSettings_Click(sender As Object, e As EventArgs) Handles DocumentsSettings.Click
        Dim docMgmt As New DocumentsManagement

        ' Check if a customer is selected
        If currentSelectedCustomerId > 0 Then
            ' Customer selected - show customer-specific documents
            docMgmt.SelectedCustomerId = currentSelectedCustomerId
            docMgmt.SelectedCustomerName = currentSelectedCustomerName
        Else
            ' No customer selected - show all documents for all customers
            docMgmt.SelectedCustomerId = 0 ' 0 means show all customers
            docMgmt.SelectedCustomerName = "جميع العملاء"
        End If

        docMgmt.ShowDialog()
        docMgmt.Dispose()
    End Sub

    Private Sub Delegations_Click_1(sender As Object, e As EventArgs) Handles Delegations.Click
        ' Check if a customer is selected
        If currentSelectedCustomerId > 0 Then
            ' Customer selected - create and show the delegations form
            Dim delegMgmt As New DelegationsManagement
            delegMgmt.CustomerIdToLoad = currentSelectedCustomerId

            delegMgmt.ShowDialog()
            delegMgmt.Dispose()
        Else
            ' No customer selected - show customer search dialog
            Dim searchForm As New CustomerSearchForm

            If searchForm.ShowDialog = DialogResult.OK Then
                ' Customer was selected from search - store and use selected customer
                currentSelectedCustomerId = searchForm.SelectedCustomerId
                currentSelectedCustomerName = searchForm.SelectedCustomerName

                Dim delegMgmt As New DelegationsManagement
                delegMgmt.CustomerIdToLoad = currentSelectedCustomerId

                delegMgmt.ShowDialog()
                delegMgmt.Dispose()
            End If

            searchForm.Dispose()
        End If
    End Sub

    Private Sub SearchForCustomer_Click(sender As Object, e As EventArgs) Handles PictureBox5.Click
        Dim searchForm As New CustomerSearchForm

        If searchForm.ShowDialog = DialogResult.OK Then
            ' Customer was selected - store customer info
            currentSelectedCustomerId = searchForm.SelectedCustomerId
            currentSelectedCustomerName = searchForm.SelectedCustomerName

            MessageBox.Show($"تم اختيار العميل: {currentSelectedCustomerName}", "تم الاختيار", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        searchForm.Dispose()
    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        ' Clear customer selection
        currentSelectedCustomerId = 0
        currentSelectedCustomerName = ""

        ' Show NotifyIcon balloon
        NotifyIcon1.BalloonTipTitle = "تم بنجاح"
        NotifyIcon1.BalloonTipText = "تم مسح اختيار العميل بنجاح!"
        NotifyIcon1.BalloonTipIcon = ToolTipIcon.Info
        NotifyIcon1.ShowBalloonTip(1000)

        ' Optional: Auto-closing popup window (like MessageBox)
        Dim toast As New AutoCloseMessageForm("تم مسح اختيار العميل بنجاح  ↻", 1000)
        toast.StartPosition = FormStartPosition.CenterScreen
        toast.Show()

    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Dim result As DialogResult = MessageBox.Show("هل أنت متأكد من تسجيل الخروج؟", "تسجيل الخروج", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            ' Clear all session data including document access credentials
            Session.Clear()

            ' Close current form and show login form
            Me.Hide()

            Dim loginForm As New Login()
            If loginForm.ShowDialog() = DialogResult.OK Then
                ' User logged in successfully, show main form again
                Me.Show()
            Else
                ' User cancelled login, exit application
                Application.Exit()
            End If
        End If
    End Sub


End Class


