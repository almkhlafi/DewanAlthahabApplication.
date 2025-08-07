Public Class CustomersInfo


    Private dbConn As New DBconnections()

        ' Variables to track selected customer from search dialog
        Private currentSelectedCustomerId As Integer = 0
        Private currentSelectedCustomerName As String = ""

        ' Helper method to find controls by name to avoid Designer variable shadowing issues
        Private Function FindControlByName(Of T As Control)(controlName As String) As T
            Try
                Dim controls As Control() = Me.Controls.Find(controlName, True)
                If controls.Length > 0 AndAlso TypeOf controls(0) Is T Then
                    Return CType(controls(0), T)
                End If
            Catch ex As Exception
                ' Control not found or wrong type
            End Try
            Return Nothing
        End Function

        ' Variables for customer navigation
        Private customerList As List(Of String) = New List(Of String)()
        Private currentCustomerIndex As Integer = -1
        Private isNavigating As Boolean = False

        ' Control update flags to prevent recursive events
        Private isLoadingAreas As Boolean = False
        Private isUpdatingCountryData As Boolean = False
        Private isUpdatingAreaData As Boolean = False
        Private isUpdatingMarketData As Boolean = False
        Private isUpdatingCategoryData As Boolean = False
        Private isUpdatingGroupsData As Boolean = False
        Private isUpdatingTypeData As Boolean = False

        Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ' Form initialization
            ' Setup NotifyIcon


            ' Setup and load branches DataGridView
            SetupBranchesDataGridView()
            LoadBranches()

            ' Setup and load currency DataGridView
            SetupCurrencyDataGridView()
            LoadCurrency()

            ' Set up ComboBox search functionality (but don't load data yet)
            SetupAreaSearch()
            SetupCountrySearch()
            SetupMarketSearch()
            SetupCategorySearch()
            SetupGroupsSearch()
            SetupTypeSearch()

            ' Initialize Customer/Supplier ComboBoxes with enhanced logic
            InitializeCustomerSupplierComboBoxes()

            ' Initialize customer list for navigation (but don't load first customer data)
            InitializeNavigationOnly()

            ' Debug: Test database access
            Try
                Dim testResult As String = dbConn.TestCustomerDataAccess()
                System.Diagnostics.Debug.WriteLine("=== DATABASE ACCESS TEST ===")
                System.Diagnostics.Debug.WriteLine(testResult)
                System.Diagnostics.Debug.WriteLine("=== END TEST ===")
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Database test failed: {ex.Message}")
            End Try
        End Sub

        Private Sub LoadCountries()
            Try
                ' Safety check - ensure CountryCB is initialized
                If CountryCB Is Nothing Then
                    MessageBox.Show("CountryCB is not initialized in LoadCountries!", "Control Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If

                ' Load countries from CMGADB2024 database
                Dim countriesTable As DataTable = dbConn.GetCountries()

                ' Clear existing DataSource first (not Items)
                CountryCB.DataSource = Nothing
                CountryCB.Tag = Nothing

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

                ' Set Saudi Arabia as default selection
                For i As Integer = 0 To displayTable.Rows.Count - 1
                    If displayTable.Rows(i)("countrycode").ToString() = "00966" Then
                        CountryCB.SelectedIndex = i
                        ' Manually trigger areas loading for Saudi Arabia
                        LoadAreas("00966")
                        Exit For
                    End If
                Next

                ' Set up additional search functionality
                SetupCountrySearch()

            Catch ex As Exception
                MessageBox.Show("خطأ في تحميل البلدان: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub LoadAreas(countryCode As String)
            Try
                ' Temporarily remove CountryCB event handler to prevent interference
                RemoveHandler CountryCB.SelectedIndexChanged, AddressOf CountryCB_SelectedIndexChanged

                ' Load areas from CMGADB2024 database for specific country
                Dim areasTable As DataTable = dbConn.GetAreas(countryCode)


                ' Check if areas were found
                If areasTable.Rows.Count = 0 Then
                    ' Clear AreaCB if no areas found for this country
                    isUpdatingAreaData = True
                    isUpdatingCountryData = True
                    AreaCB.DataSource = Nothing
                    AreaCB.Tag = Nothing
                    AreaCB.Text = ""
                    isUpdatingAreaData = False
                    isUpdatingCountryData = False
                    Return
                End If

                ' Clear existing data completely
                isUpdatingAreaData = True
                isUpdatingCountryData = True
                AreaCB.DataSource = Nothing
                AreaCB.Tag = Nothing
                AreaCB.Text = ""

                ' Set up AreaCB properties for enhanced search functionality
                AreaCB.DisplayMember = "DisplayText"
                AreaCB.ValueMember = "code"
                AreaCB.AutoCompleteMode = AutoCompleteMode.Suggest
                AreaCB.AutoCompleteSource = AutoCompleteSource.ListItems
                AreaCB.DropDownStyle = ComboBoxStyle.DropDown

                ' Create a new DataTable with combined display text
                Dim displayTable As New DataTable()
                displayTable.Columns.Add("code", GetType(String))
                displayTable.Columns.Add("description", GetType(String))
                displayTable.Columns.Add("DisplayText", GetType(String))

                ' Add areas to ComboBox with combined display format: description - shortname
                For Each row As DataRow In areasTable.Rows
                    Dim newRow As DataRow = displayTable.NewRow()
                    newRow("code") = row("code").ToString()
                    newRow("description") = row("description").ToString()

                    ' Create display text with description and short name
                    Dim displayText As String = row("description").ToString()
                    If Not String.IsNullOrEmpty(row("shortname").ToString()) Then
                        displayText += $" - {row("shortname")}"
                    End If

                    newRow("DisplayText") = displayText
                    displayTable.Rows.Add(newRow)
                Next

                ' Store original data for filtering before binding
                AreaCB.Tag = displayTable

                ' Bind to ComboBox
                AreaCB.DataSource = displayTable


                isUpdatingAreaData = False
                isUpdatingCountryData = False

                ' Set up additional search functionality
                SetupAreaSearch()

                ' Re-add CountryCB event handler
                AddHandler CountryCB.SelectedIndexChanged, AddressOf CountryCB_SelectedIndexChanged

            Catch ex As Exception
                ' Ensure event handler is re-added even on error
                AddHandler CountryCB.SelectedIndexChanged, AddressOf CountryCB_SelectedIndexChanged
                isUpdatingAreaData = False
                isUpdatingCountryData = False
                MessageBox.Show("خطأ في تحميل المناطق: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub SetupCountrySearch()
            ' Enable advanced search functionality for CountryCB
            If CountryCB IsNot Nothing Then
                Try
                    AddHandler CountryCB.KeyUp, AddressOf CountryCB_KeyUp
                    AddHandler CountryCB.TextChanged, AddressOf CountryCB_TextChanged
                Catch ex As Exception
                    MessageBox.Show("خطأ في إعداد البحث للبلدان: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End Try
            Else
                MessageBox.Show("CountryCB control is not initialized!", "Control Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Sub

        Private areaSearchSetups As Boolean = False

        Private Sub SetupAreaSearch()
            ' Enable advanced search functionality for AreaCB only once
            If Not areaSearchSetups AndAlso AreaCB IsNot Nothing Then
                Try
                    AddHandler AreaCB.KeyUp, AddressOf AreaCB_KeyUp
                    AddHandler AreaCB.TextChanged, AddressOf AreaCB_TextChanged
                    AddHandler AreaCB.DropDown, AddressOf AreaCB_DropDown
                    AddHandler AreaCB.SelectedIndexChanged, AddressOf AreaCB_SelectedIndexChanged
                    AddHandler AreaCB.Click, AddressOf AreaCB_Click
                    AddHandler AreaCB.Enter, AddressOf AreaCB_Enter
                    AddHandler AreaCB.GotFocus, AddressOf AreaCB_GotFocus
                    areaSearchSetups = True
                Catch ex As Exception
                    ' Handle any errors during event handler setup
                    MessageBox.Show("خطأ في إعداد البحث للمناطق: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End Try
            ElseIf AreaCB Is Nothing Then
                MessageBox.Show("AreaCB control is not initialized!", "Control Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Sub

        Private Sub SetupMarketSearch()
            ' Market search is now handled in LoadLookupData method with enhanced search
            ' No additional setup needed
        End Sub

        Private Sub SetupCategorySearch()
            ' Category search is now handled in LoadLookupData method with enhanced search
            ' No additional setup needed
        End Sub

        Private Sub SetupGroupsSearch()
            ' Groups search is now handled in LoadLookupData method with enhanced search
            ' No additional setup needed
        End Sub

        Private Sub SetupTypeSearch()
            ' Type search is now handled in LoadLookupData method with enhanced search
            ' No additional setup needed
        End Sub

        Private Sub CountryCB_KeyUp(sender As Object, e As KeyEventArgs)
            ' Safety check - ensure CountryCB is initialized and form is loaded
            If CountryCB Is Nothing OrElse Not Me.IsHandleCreated Then Return

            ' Handle key navigation in dropdown
            If e.KeyCode = Keys.Enter Then
                CountryCB.DroppedDown = False
            ElseIf e.KeyCode = Keys.Down OrElse e.KeyCode = Keys.Up Then
                CountryCB.DroppedDown = True
            End If
        End Sub


        Private Sub CountryCB_TextChanged(sender As Object, e As EventArgs)
            ' Safety check - ensure CountryCB is initialized and form is loaded
            If CountryCB Is Nothing OrElse Not Me.IsHandleCreated Then Return

            ' Prevent recursive calls when updating data
            If isUpdatingCountryData Then Return

            ' Enhanced search functionality - move matching items to top
            Try
                Dim searchText As String = CountryCB.Text.ToLower().Trim()
                Dim currentCursorPosition As Integer = CountryCB.SelectionStart

                ' Get the original data
                Dim originalData As DataTable = CType(CountryCB.Tag, DataTable)
                If originalData Is Nothing Then Return

                ' If search text is empty, restore full list
                If String.IsNullOrEmpty(searchText) Then
                    isUpdatingCountryData = True
                    CountryCB.DataSource = originalData
                    CountryCB.Text = ""
                    isUpdatingCountryData = False
                    Return
                End If

                ' Sort data: matching items first, then non-matching items
                ' Search by country code OR country name (Arabic or English)
                Dim matchingRows = originalData.AsEnumerable().Where(Function(row)
                                                                         Try
                                                                             Dim countryCode As String = If(row("countrycode") IsNot Nothing AndAlso row("countrycode") IsNot DBNull.Value, row("countrycode").ToString().ToLower(), "")
                                                                             Dim displayText As String = If(row("DisplayText") IsNot Nothing AndAlso row("DisplayText") IsNot DBNull.Value, row("DisplayText").ToString().ToLower(), "")

                                                                             ' Match by country code OR by any part of the display text
                                                                             Return countryCode.Contains(searchText) OrElse displayText.Contains(searchText)
                                                                         Catch
                                                                             Return False
                                                                         End Try
                                                                     End Function).OrderBy(Function(row) If(row("DisplayText") IsNot Nothing, row("DisplayText").ToString(), ""))

                Dim nonMatchingRows = originalData.AsEnumerable().Where(Function(row)
                                                                            Try
                                                                                Dim countryCode As String = If(row("countrycode") IsNot Nothing AndAlso row("countrycode") IsNot DBNull.Value, row("countrycode").ToString().ToLower(), "")
                                                                                Dim displayText As String = If(row("DisplayText") IsNot Nothing AndAlso row("DisplayText") IsNot DBNull.Value, row("DisplayText").ToString().ToLower(), "")

                                                                                ' No match by country code OR display text
                                                                                Return Not (countryCode.Contains(searchText) OrElse displayText.Contains(searchText))
                                                                            Catch
                                                                                Return True
                                                                            End Try
                                                                        End Function).OrderBy(Function(row) If(row("DisplayText") IsNot Nothing, row("DisplayText").ToString(), ""))

                ' Create reordered DataTable with matches first
                Dim reorderedTable As DataTable = originalData.Clone()

                ' Add matching rows first
                For Each row In matchingRows
                    reorderedTable.ImportRow(row)
                Next

                ' Add non-matching rows after
                For Each row In nonMatchingRows
                    reorderedTable.ImportRow(row)
                Next

                ' Update ComboBox with reordered data without changing text
                If CountryCB IsNot Nothing Then
                    isUpdatingCountryData = True
                    Dim userText As String = CountryCB.Text
                    CountryCB.DataSource = reorderedTable
                    CountryCB.Text = userText
                    CountryCB.SelectionStart = currentCursorPosition
                    CountryCB.SelectionLength = 0
                    ' Only open dropdown if CountryCB has focus and user is actively typing
                    If CountryCB.Focused AndAlso Not String.IsNullOrEmpty(searchText) Then
                        CountryCB.DroppedDown = True
                    End If
                    isUpdatingCountryData = False
                End If

            Catch ex As Exception
                isUpdatingCountryData = False
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
            ' Prevent recursive calls when updating data
            If isUpdatingAreaData Then Return

            ' Enhanced search functionality - move matching items to top
            Try
                Dim searchText As String = AreaCB.Text.ToLower().Trim()
                Dim currentCursorPosition As Integer = AreaCB.SelectionStart

                ' Get the original data
                Dim originalData As DataTable = CType(AreaCB.Tag, DataTable)
                If originalData Is Nothing Then Return

                ' If search text is empty, restore full list
                If String.IsNullOrEmpty(searchText) Then
                    isUpdatingAreaData = True
                    AreaCB.DataSource = originalData
                    AreaCB.Text = ""
                    isUpdatingAreaData = False
                    Return
                End If

                ' Sort data: matching items first, then non-matching items
                Dim matchingRows = originalData.AsEnumerable().Where(Function(row)
                                                                         Dim displayText As String = row("DisplayText").ToString().ToLower()
                                                                         Return displayText.Contains(searchText)
                                                                     End Function).OrderBy(Function(row) row("DisplayText").ToString())

                Dim nonMatchingRows = originalData.AsEnumerable().Where(Function(row)
                                                                            Dim displayText As String = row("DisplayText").ToString().ToLower()
                                                                            Return Not displayText.Contains(searchText)
                                                                        End Function).OrderBy(Function(row) row("DisplayText").ToString())

                ' Create reordered DataTable with matches first
                Dim reorderedTable As DataTable = originalData.Clone()

                ' Add matching rows first
                For Each row In matchingRows
                    reorderedTable.ImportRow(row)
                Next

                ' Add non-matching rows after
                For Each row In nonMatchingRows
                    reorderedTable.ImportRow(row)
                Next

                ' Update ComboBox with reordered data without changing text
                isUpdatingAreaData = True
                Dim userText As String = AreaCB.Text
                AreaCB.DataSource = reorderedTable
                AreaCB.Text = userText
                AreaCB.SelectionStart = currentCursorPosition
                AreaCB.SelectionLength = 0
                ' Only open dropdown if AreaCB has focus and user is actively typing
                If AreaCB.Focused AndAlso Not String.IsNullOrEmpty(searchText) Then
                    AreaCB.DroppedDown = True
                End If
                isUpdatingAreaData = False

            Catch ex As Exception
                isUpdatingAreaData = False
                ' Ignore any errors during text change
            End Try
        End Sub



        Private Sub AreaCB_DropDown(sender As Object, e As EventArgs)
            ' Prevent CountryCB changes when AreaCB dropdown opens
            Try
                isLoadingAreas = True
                isUpdatingCountryData = True
            Catch ex As Exception
                ' Ignore errors
            End Try
        End Sub

        Private Sub AreaCB_SelectedIndexChanged(sender As Object, e As EventArgs)
            ' Prevent CountryCB changes when AreaCB selection changes
            Try
                If Not isUpdatingAreaData Then
                    ' Simply reset the flags after area selection
                    isLoadingAreas = False
                    isUpdatingCountryData = False
                End If
            Catch ex As Exception
                isLoadingAreas = False
                isUpdatingCountryData = False
            End Try
        End Sub

        Private Sub AreaCB_Click(sender As Object, e As EventArgs)
            ' Completely remove CountryCB event handler when AreaCB is clicked
            Try
                RemoveHandler CountryCB.SelectedIndexChanged, AddressOf CountryCB_SelectedIndexChanged
                isUpdatingCountryData = True
                isLoadingAreas = True
            Catch ex As Exception
                ' Ignore errors
            End Try
        End Sub

        Private Sub AreaCB_Enter(sender As Object, e As EventArgs)
            ' Completely remove CountryCB event handler when AreaCB gets focus
            Try
                RemoveHandler CountryCB.SelectedIndexChanged, AddressOf CountryCB_SelectedIndexChanged
                isUpdatingCountryData = True
                isLoadingAreas = True
            Catch ex As Exception
                ' Ignore errors
            End Try
        End Sub

        Private Sub AreaCB_GotFocus(sender As Object, e As EventArgs)
            ' Completely remove CountryCB event handler when AreaCB gets focus
            Try
                RemoveHandler CountryCB.SelectedIndexChanged, AddressOf CountryCB_SelectedIndexChanged
                isUpdatingCountryData = True
                isLoadingAreas = True
            Catch ex As Exception
                ' Ignore errors
            End Try
        End Sub

        ' =====================MarketCB Search Event Handlers========================

        ' Additional form methods and navigation functionality

        Private Sub CountryCB_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CountryCB.SelectedIndexChanged
            ' Prevent execution during area loading or country data updates
            If isLoadingAreas OrElse isUpdatingCountryData OrElse isNavigating Then Return

            Try
                If CountryCB.SelectedItem IsNot Nothing Then
                    Dim selectedCountry = CType(CountryCB.SelectedItem, DataRowView)
                    Dim countryCode = selectedCountry("countrycode").ToString
                    Dim displayText = selectedCountry("DisplayText").ToString

                    ' Auto-populate AreaCB with areas/cities for any selected country
                    isLoadingAreas = True
                    LoadAreas(countryCode)
                    isLoadingAreas = False

                    System.Diagnostics.Debug.WriteLine($"Selected country: {displayText} with code: {countryCode}")
                Else
                    ' Country is empty - clear areas silently without showing error message
                    isLoadingAreas = True
                    isUpdatingAreaData = True
                    If AreaCB IsNot Nothing Then
                        AreaCB.DataSource = Nothing
                        AreaCB.Tag = Nothing
                        AreaCB.Text = ""
                    End If
                    isUpdatingAreaData = False
                    isLoadingAreas = False
                End If
            Catch ex As Exception
                isLoadingAreas = False
                MessageBox.Show("خطأ في تحديد البلد: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub SetupBranchesDataGridView()
            ' Safety check - ensure BranchesInfoDGV is initialized
            If BranchesInfoDGV Is Nothing Then
                MessageBox.Show("BranchesInfoDGV is not initialized!", "Control Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

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
                ' Safety check - ensure BranchesInfoDGV is initialized
                If BranchesInfoDGV Is Nothing Then
                    MessageBox.Show("BranchesInfoDGV is not initialized in LoadBranches!", "Control Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If

                ' Load branches from CMGADB2024 database
                Dim branchesTable As DataTable = dbConn.GetBranches()
                BranchesInfoDGV.Rows.Clear()
                ' Add branches to DataGridView - only populate Arabic and English names from database
                ' Other columns (Select, Active, RefNo, Locked) will be filled by users later
                For Each row As DataRow In branchesTable.Rows
                    Dim newRow As Object() = {
                    False, ' Select checkbox (default unchecked - user will set)
                    row("branch_arabic").ToString(), ' Arabic name from database
                    row("branch_name").ToString(), ' English name from database
                    "", ' Branch code (empty - user will fill)
                    False, ' Active status (default unchecked - user will set)
                    "", ' Ref NO (empty - user will fill)
                    False ' Locked status (default unchecked - user will set)
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
            ' Find CurrencyDGV control by name to avoid Designer variable shadowing issues
            Dim currencyDGV As DataGridView = FindControlByName(Of DataGridView)("CurrencyDGV")
            If currencyDGV Is Nothing Then
                MessageBox.Show("CurrencyDGV control not found!", "Control Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            ' Clear existing columns
            currencyDGV.Columns.Clear()

            ' Set basic properties
            currencyDGV.AllowUserToAddRows = False
            currencyDGV.AllowUserToDeleteRows = False
            currencyDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            currencyDGV.MultiSelect = True
            currencyDGV.ReadOnly = False
            currencyDGV.RowHeadersVisible = False
            currencyDGV.RightToLeft = RightToLeft.Yes

            ' Add checkbox column for selection
            Dim selectColumn As New DataGridViewCheckBoxColumn()
            selectColumn.Name = "Select"
            selectColumn.HeaderText = "الكل"
            selectColumn.Width = 50
            selectColumn.ReadOnly = False
            currencyDGV.Columns.Add(selectColumn)

            ' Add currency code column
            Dim currencyCodeColumn As New DataGridViewTextBoxColumn()
            currencyCodeColumn.Name = "CurrencyCode"
            currencyCodeColumn.HeaderText = "الرمز"
            currencyCodeColumn.ReadOnly = True
            currencyCodeColumn.Width = 80
            currencyDGV.Columns.Add(currencyCodeColumn)

            ' Add currency name column
            Dim currencyNameColumn As New DataGridViewTextBoxColumn()
            currencyNameColumn.Name = "CurrencyName"
            currencyNameColumn.HeaderText = "الاسم"
            currencyNameColumn.ReadOnly = True
            currencyNameColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            currencyDGV.Columns.Add(currencyNameColumn)

            ' Add currency decimal/price column
            Dim currencyDecimalColumn As New DataGridViewTextBoxColumn()
            currencyDecimalColumn.Name = "CurrencyDecimal"
            currencyDecimalColumn.HeaderText = "السعر"
            currencyDecimalColumn.ReadOnly = True
            currencyDecimalColumn.Width = 100
            currencyDGV.Columns.Add(currencyDecimalColumn)

            ' Add event handlers
            AddHandler currencyDGV.CellContentClick, AddressOf DataGridView1_CellContentClick
            AddHandler currencyDGV.ColumnHeaderMouseClick, AddressOf DataGridView1_ColumnHeaderMouseClick
        End Sub

        Private Sub LoadCurrency()
            Try
                ' Find CurrencyDGV control by name to avoid Designer variable shadowing issues
                Dim currencyDGV As DataGridView = FindControlByName(Of DataGridView)("CurrencyDGV")
                If currencyDGV Is Nothing Then
                    MessageBox.Show("CurrencyDGV control not found in LoadCurrency!", "Control Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If

                ' Load currency from CMGADB2024 database
                Dim currencyTable As DataTable = dbConn.GetCurrency()

                ' Clear existing rows
                currencyDGV.Rows.Clear()

                ' Add currency to DataGridView
                For Each row As DataRow In currencyTable.Rows
                    Dim newRow As Object() = {
                    False, ' Select checkbox (unchecked by default)
                    row("suffix_code").ToString(), ' Currency code
                    row("arabicname").ToString(), ' Arabic name
                    row("decimal").ToString() ' Decimal/Price value
                }
                    currencyDGV.Rows.Add(newRow)
                Next

            Catch ex As Exception
                MessageBox.Show("خطأ في تحميل العملات: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub LoadMarketData()
            Try
                ' Safety check - ensure MarketCB is initialized
                If MarketCB Is Nothing Then
                    MessageBox.Show("MarketCB is not initialized!", "Control Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If

                ' Load market data from CMGADB2024 database
                Dim marketTable As DataTable = dbConn.GetMarketData()

                ' Set up MarketCB properties for enhanced search functionality
                MarketCB.DisplayMember = "DisplayText"
                MarketCB.ValueMember = "code"

                MarketCB.AutoCompleteMode = AutoCompleteMode.Suggest
                MarketCB.AutoCompleteSource = AutoCompleteSource.ListItems
                MarketCB.DropDownStyle = ComboBoxStyle.DropDown

                ' Create a new DataTable with combined display text
                Dim displayTable As New DataTable()
                displayTable.Columns.Add("code", GetType(String))
                displayTable.Columns.Add("DisplayText", GetType(String))

                ' Add market data with combined display format: code - description - shortname
                For Each row As DataRow In marketTable.Rows
                    Dim newRow As DataRow = displayTable.NewRow()
                    newRow("code") = row("code").ToString()

                    Dim displayText As String = $"{row("code")} - {row("description")}"
                    If Not String.IsNullOrEmpty(row("shortname").ToString()) Then
                        displayText += $" - {row("shortname")}"
                    End If

                    newRow("DisplayText") = displayText
                    displayTable.Rows.Add(newRow)
                Next

                ' Bind to ComboBox and store original data for search
                MarketCB.DataSource = displayTable
                MarketCB.Tag = displayTable

                ' Set up search functionality
                SetupMarketSearch()

            Catch ex As Exception
                MessageBox.Show("خطأ في تحميل بيانات السوق: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub LoadCategoryData()
            Try
                ' Safety check - ensure CatogeryCB is initialized
                If CatogeryCB Is Nothing Then
                    MessageBox.Show("CatogeryCB is not initialized!", "Control Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If

                ' Load category data from CMGADB2024 database
                Dim categoryTable As DataTable = dbConn.GetCategoryData()

                ' Set up CategoryCB properties for enhanced search functionality
                CatogeryCB.DisplayMember = "DisplayText"
                CatogeryCB.ValueMember = "fld_code"
                CatogeryCB.AutoCompleteMode = AutoCompleteMode.Suggest
                CatogeryCB.AutoCompleteSource = AutoCompleteSource.ListItems
                CatogeryCB.DropDownStyle = ComboBoxStyle.DropDown

                ' Create a new DataTable with combined display text
                Dim displayTable As New DataTable()
                displayTable.Columns.Add("fld_code", GetType(String))
                displayTable.Columns.Add("DisplayText", GetType(String))

                ' Add category data with combined display format: fld_code - fld_name - fld_arabic_name
                For Each row As DataRow In categoryTable.Rows
                    Dim newRow As DataRow = displayTable.NewRow()
                    newRow("fld_code") = row("fld_code").ToString()

                    Dim displayText As String = $"{row("fld_code")} - {row("fld_name")}"
                    If Not String.IsNullOrEmpty(row("fld_arabic_name").ToString()) Then
                        displayText += $" - {row("fld_arabic_name")}"
                    End If

                    newRow("DisplayText") = displayText
                    displayTable.Rows.Add(newRow)
                Next

                ' Bind to ComboBox and store original data for search
                CatogeryCB.DataSource = displayTable
                CatogeryCB.Tag = displayTable

                ' Set up search functionality
                SetupCategorySearch()

            Catch ex As Exception
                MessageBox.Show("خطأ في تحميل بيانات الفئة: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub LoadGroupsData()
            Try
                ' Safety check - ensure GroupsCB is initialized
                If GroupsCB Is Nothing Then
                    MessageBox.Show("GroupsCB is not initialized!", "Control Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If

                ' Load groups data from CMGADB2024 database
                Dim groupsTable As DataTable = dbConn.GetGroupsData()

                ' Set up GroupsCB properties for enhanced search functionality
                GroupsCB.DisplayMember = "DisplayText"
                GroupsCB.ValueMember = "code"
                GroupsCB.AutoCompleteMode = AutoCompleteMode.Suggest
                GroupsCB.AutoCompleteSource = AutoCompleteSource.ListItems
                GroupsCB.DropDownStyle = ComboBoxStyle.DropDown

                ' Create a new DataTable with combined display text
                Dim displayTable As New DataTable()
                displayTable.Columns.Add("code", GetType(String))
                displayTable.Columns.Add("DisplayText", GetType(String))

                ' Add groups data with combined display format: code - description - shortname
                For Each row As DataRow In groupsTable.Rows
                    Dim newRow As DataRow = displayTable.NewRow()
                    newRow("code") = row("code").ToString()

                    Dim displayText As String = $"{row("code")} - {row("description")}"
                    If Not String.IsNullOrEmpty(row("shortname").ToString()) Then
                        displayText += $" - {row("shortname")}"
                    End If

                    newRow("DisplayText") = displayText
                    displayTable.Rows.Add(newRow)
                Next

                ' Bind to ComboBox and store original data for search
                GroupsCB.DataSource = displayTable
                GroupsCB.Tag = displayTable

                ' Set up search functionality
                SetupGroupsSearch()

            Catch ex As Exception
                MessageBox.Show("خطأ في تحميل بيانات المجموعات: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub LoadTypeData()
            Try
                ' Safety check - ensure TypeCB is initialized
                If TypeCB Is Nothing Then
                    MessageBox.Show("TypeCB is not initialized!", "Control Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If

                ' Load type data from CMGADB2024 database
                Dim typeTable As DataTable = dbConn.GetTypeData()

                ' Set up TypeCB properties for enhanced search functionality
                TypeCB.DisplayMember = "DisplayText"
                TypeCB.ValueMember = "fld_code"
                TypeCB.AutoCompleteMode = AutoCompleteMode.Suggest
                TypeCB.AutoCompleteSource = AutoCompleteSource.ListItems
                TypeCB.DropDownStyle = ComboBoxStyle.DropDown

                ' Create a new DataTable with combined display text
                Dim displayTable As New DataTable()
                displayTable.Columns.Add("fld_code", GetType(String))
                displayTable.Columns.Add("DisplayText", GetType(String))

                ' Add type data with combined display format: fld_code - fld_name - fld_arabic_name
                For Each row As DataRow In typeTable.Rows
                    Dim newRow As DataRow = displayTable.NewRow()
                    newRow("fld_code") = row("fld_code").ToString()

                    Dim displayText As String = $"{row("fld_code")} - {row("fld_name")}"
                    If Not String.IsNullOrEmpty(row("fld_arabic_name").ToString()) Then
                        displayText += $" - {row("fld_arabic_name")}"
                    End If

                    newRow("DisplayText") = displayText
                    displayTable.Rows.Add(newRow)
                Next

                ' Bind to ComboBox and store original data for search
                TypeCB.DataSource = displayTable
                TypeCB.Tag = displayTable

                ' Set up search functionality
                SetupTypeSearch()

            Catch ex As Exception
                MessageBox.Show("خطأ في تحميل بيانات النوع: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
            ' Handle checkbox clicks in the Select column
            If e.RowIndex >= 0 AndAlso e.ColumnIndex = 0 Then
                Dim currencyDGV As DataGridView = FindControlByName(Of DataGridView)("CurrencyDGV")
                If currencyDGV IsNot Nothing Then
                    currencyDGV.EndEdit()
                End If
            End If
        End Sub

        Private Sub DataGridView1_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs)
            ' Handle "select all" functionality when clicking on the header checkbox
            If e.ColumnIndex = 0 Then
                Dim currencyDGV As DataGridView = FindControlByName(Of DataGridView)("CurrencyDGV")
                If currencyDGV Is Nothing Then Return

                Dim selectAll As Boolean = True

                ' Check if any row is currently selected to determine toggle behavior
                For Each row As DataGridViewRow In currencyDGV.Rows
                    If CBool(row.Cells(0).Value) = True Then
                        selectAll = False
                        Exit For
                    End If
                Next

                ' Set all checkboxes to the determined state
                For Each row As DataGridViewRow In currencyDGV.Rows
                    row.Cells(0).Value = selectAll
                Next
            End If
        End Sub

        ' Helper method to get selected currencies
        Public Function GetSelectedCurrencies() As List(Of String)
            Dim selectedCurrencies As New List(Of String)()
            Dim currencyDGV As DataGridView = FindControlByName(Of DataGridView)("CurrencyDGV")
            If currencyDGV Is Nothing Then Return selectedCurrencies

            For Each row As DataGridViewRow In currencyDGV.Rows
                If CBool(row.Cells("Select").Value) = True Then
                    selectedCurrencies.Add(row.Cells("CurrencyCode").Value.ToString())
                End If
            Next

            Return selectedCurrencies
        End Function




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



        Private Sub CatogeryCB_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CatogeryCB.SelectedIndexChanged

        End Sub

        Private Sub GroupsCB_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GroupsCB.SelectedIndexChanged

        End Sub

        Private Sub TypeCB_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TypeCB.SelectedIndexChanged

        End Sub

        ' =====================Customer/Supplier Management========================

        Private Sub InitializeCustomerSupplierComboBoxes()
            Try
                ' Initialize CustomerSupplierCB with enhanced logic
                If CustomerSupplierCB IsNot Nothing Then
                    CustomerSupplierCB.Items.Clear()
                    CustomerSupplierCB.Items.Add("Customer")
                    CustomerSupplierCB.Items.Add("Supplier")
                    CustomerSupplierCB.SelectedIndex = 0 ' Default to Customer

                    ' Generate and display next available code
                    GenerateAndDisplayNextCode()
                End If

                ' Initialize IdentityCommercialNameOptionCB with enhanced logic
                If IdentityCommercialNameOptionCB IsNot Nothing Then
                    IdentityCommercialNameOptionCB.Items.Clear()
                    IdentityCommercialNameOptionCB.Items.Add("فردي") ' Individual
                    IdentityCommercialNameOptionCB.Items.Add("تجاري") ' Commercial
                    IdentityCommercialNameOptionCB.SelectedIndex = 0 ' Default to Individual

                    ' Set initial label
                    UpdateIdentityLabel("فردي")
                End If

                ' Load lookup data from database for ComboBoxes
                LoadLookupData()

            Catch ex As Exception
                MessageBox.Show("خطأ في تهيئة قوائم العميل/المورد: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        ' Load lookup data from database for ComboBoxes
        Private Sub LoadLookupData()
            Try
                ' Load MarketCB from CusTransactionMaster
                If MarketCB IsNot Nothing Then
                    Try
                        ' Reset ComboBox completely
                        MarketCB.DataSource = Nothing
                        MarketCB.Items.Clear()
                        MarketCB.DisplayMember = Nothing
                        MarketCB.ValueMember = Nothing

                        Dim marketData As DataTable = dbConn.LoadCusTransactionMaster()
                        If marketData IsNot Nothing AndAlso marketData.Rows.Count > 0 Then
                            ' Verify required columns exist
                            If marketData.Columns.Contains("fld_area_code") AndAlso marketData.Columns.Contains("DisplayText") Then
                                MarketCB.DataSource = marketData
                                MarketCB.DisplayMember = "DisplayText"
                                MarketCB.ValueMember = "fld_area_code"
                                MarketCB.SelectedIndex = -1
                                MarketCB.Tag = marketData.Copy() ' Store original data for search

                                ' Enable enhanced search functionality
                                MarketCB.AutoCompleteMode = AutoCompleteMode.None
                                MarketCB.AutoCompleteSource = AutoCompleteSource.None
                                MarketCB.DropDownStyle = ComboBoxStyle.DropDown

                                ' Setup enhanced search events
                                AddHandler MarketCB.TextChanged, AddressOf MarketCB_TextChanged
                                AddHandler MarketCB.KeyUp, AddressOf MarketCB_KeyUp
                            Else
                                System.Diagnostics.Debug.WriteLine("MarketCB: Required columns not found")
                            End If
                        Else
                            System.Diagnostics.Debug.WriteLine("MarketCB: No data returned")
                        End If
                    Catch ex As Exception
                        System.Diagnostics.Debug.WriteLine("Error loading MarketCB: " & ex.Message)
                        ' Create fallback empty ComboBox
                        MarketCB.DataSource = Nothing
                        MarketCB.Items.Clear()
                        MarketCB.Items.Add("لا توجد بيانات")
                        MarketCB.SelectedIndex = -1
                    End Try
                End If

                ' Load GroupsCB from CusGradeMaster
                If GroupsCB IsNot Nothing Then
                    Try
                        ' Reset ComboBox completely
                        GroupsCB.DataSource = Nothing
                        GroupsCB.Items.Clear()
                        GroupsCB.DisplayMember = Nothing
                        GroupsCB.ValueMember = Nothing

                        Dim groupsData As DataTable = dbConn.LoadCusGradeMaster()
                        If groupsData IsNot Nothing AndAlso groupsData.Rows.Count > 0 Then
                            If groupsData.Columns.Contains("code") AndAlso groupsData.Columns.Contains("DisplayText") Then
                                GroupsCB.DataSource = groupsData
                                GroupsCB.DisplayMember = "DisplayText"
                                GroupsCB.ValueMember = "code"
                                GroupsCB.SelectedIndex = -1
                                GroupsCB.Tag = groupsData.Copy() ' Store original data for search

                                ' Enable enhanced search functionality
                                GroupsCB.AutoCompleteMode = AutoCompleteMode.None
                                GroupsCB.AutoCompleteSource = AutoCompleteSource.None
                                GroupsCB.DropDownStyle = ComboBoxStyle.DropDown

                                ' Setup enhanced search events
                                AddHandler GroupsCB.TextChanged, AddressOf GroupsCB_TextChanged
                                AddHandler GroupsCB.KeyUp, AddressOf GroupsCB_KeyUp
                            End If
                        End If
                    Catch ex As Exception
                        System.Diagnostics.Debug.WriteLine("Error loading GroupsCB: " & ex.Message)
                        GroupsCB.DataSource = Nothing
                        GroupsCB.Items.Clear()
                        GroupsCB.Items.Add("لا توجد بيانات")
                        GroupsCB.SelectedIndex = -1
                    End Try
                End If

                ' Load TypeCB from CustomerType
                If TypeCB IsNot Nothing Then
                    Try
                        ' Reset ComboBox completely
                        TypeCB.DataSource = Nothing
                        TypeCB.Items.Clear()
                        TypeCB.DisplayMember = Nothing
                        TypeCB.ValueMember = Nothing

                        Dim typeData As DataTable = dbConn.LoadCustomerType()
                        If typeData IsNot Nothing AndAlso typeData.Rows.Count > 0 Then
                            If typeData.Columns.Contains("fld_code") AndAlso typeData.Columns.Contains("DisplayText") Then
                                TypeCB.DataSource = typeData
                                TypeCB.DisplayMember = "DisplayText"
                                TypeCB.ValueMember = "fld_code"
                                TypeCB.SelectedIndex = -1
                                TypeCB.Tag = typeData.Copy() ' Store original data for search

                                ' Enable enhanced search functionality
                                TypeCB.AutoCompleteMode = AutoCompleteMode.None
                                TypeCB.AutoCompleteSource = AutoCompleteSource.None
                                TypeCB.DropDownStyle = ComboBoxStyle.DropDown

                                ' Setup enhanced search events
                                AddHandler TypeCB.TextChanged, AddressOf TypeCB_TextChanged
                                AddHandler TypeCB.KeyUp, AddressOf TypeCB_KeyUp
                            End If
                        End If
                    Catch ex As Exception
                        System.Diagnostics.Debug.WriteLine("Error loading TypeCB: " & ex.Message)
                        TypeCB.DataSource = Nothing
                        TypeCB.Items.Clear()
                        TypeCB.Items.Add("لا توجد بيانات")
                        TypeCB.SelectedIndex = -1
                    End Try
                End If

                ' Load CatogeryCB from CustomerCategory
                If CatogeryCB IsNot Nothing Then
                    Try
                        ' Reset ComboBox completely
                        CatogeryCB.DataSource = Nothing
                        CatogeryCB.Items.Clear()
                        CatogeryCB.DisplayMember = Nothing
                        CatogeryCB.ValueMember = Nothing

                        Dim categoryData As DataTable = dbConn.LoadCustomerCategory()
                        If categoryData IsNot Nothing AndAlso categoryData.Rows.Count > 0 Then
                            If categoryData.Columns.Contains("fld_code") AndAlso categoryData.Columns.Contains("DisplayText") Then
                                CatogeryCB.DataSource = categoryData
                                CatogeryCB.DisplayMember = "DisplayText"
                                CatogeryCB.ValueMember = "fld_code"
                                CatogeryCB.SelectedIndex = -1
                                CatogeryCB.Tag = categoryData.Copy() ' Store original data for search

                                ' Enable enhanced search functionality
                                CatogeryCB.AutoCompleteMode = AutoCompleteMode.None
                                CatogeryCB.AutoCompleteSource = AutoCompleteSource.None
                                CatogeryCB.DropDownStyle = ComboBoxStyle.DropDown

                                ' Setup enhanced search events
                                AddHandler CatogeryCB.TextChanged, AddressOf CatogeryCB_TextChanged
                                AddHandler CatogeryCB.KeyUp, AddressOf CatogeryCB_KeyUp
                            End If
                        End If
                    Catch ex As Exception
                        System.Diagnostics.Debug.WriteLine("Error loading CatogeryCB: " & ex.Message)
                        CatogeryCB.DataSource = Nothing
                        CatogeryCB.Items.Clear()
                        CatogeryCB.Items.Add("لا توجد بيانات")
                        CatogeryCB.SelectedIndex = -1
                    End Try
                End If

                ' Load AreaCB from AreaMaster
                If AreaCB IsNot Nothing Then
                    Try
                        ' Reset ComboBox completely
                        AreaCB.DataSource = Nothing
                        AreaCB.Items.Clear()
                        AreaCB.DisplayMember = Nothing
                        AreaCB.ValueMember = Nothing

                        Dim areaData As DataTable = dbConn.LoadAreaMaster()
                        If areaData IsNot Nothing AndAlso areaData.Rows.Count > 0 Then
                            If areaData.Columns.Contains("contryCode") AndAlso areaData.Columns.Contains("DisplayText") Then
                                AreaCB.DataSource = areaData
                                AreaCB.DisplayMember = "DisplayText"
                                AreaCB.ValueMember = "contryCode"
                                AreaCB.SelectedIndex = -1

                                ' Keep existing enhanced search functionality for AreaCB
                                AreaCB.AutoCompleteMode = AutoCompleteMode.Suggest
                                AreaCB.AutoCompleteSource = AutoCompleteSource.ListItems
                                AreaCB.DropDownStyle = ComboBoxStyle.DropDown
                            End If
                        End If
                    Catch ex As Exception
                        System.Diagnostics.Debug.WriteLine("Error loading AreaCB: " & ex.Message)
                        AreaCB.DataSource = Nothing
                        AreaCB.Items.Clear()
                        AreaCB.Items.Add("لا توجد بيانات")
                        AreaCB.SelectedIndex = -1
                    End Try
                End If

                ' Load CountryCB from CountryMaster
                If CountryCB IsNot Nothing Then
                    Try
                        ' Reset ComboBox completely
                        CountryCB.DataSource = Nothing
                        CountryCB.Items.Clear()
                        CountryCB.DisplayMember = Nothing
                        CountryCB.ValueMember = Nothing

                        Dim countryData As DataTable = dbConn.GetCountries()
                        If countryData IsNot Nothing AndAlso countryData.Rows.Count > 0 Then
                            If countryData.Columns.Contains("countrycode") AndAlso countryData.Columns.Contains("contryarname") Then
                                ' Create display text column for countries
                                If Not countryData.Columns.Contains("DisplayText") Then
                                    countryData.Columns.Add("DisplayText", GetType(String))
                                    For Each row As DataRow In countryData.Rows
                                        Dim countryName As String = If(row("contryarname").ToString(), "")
                                        Dim countryCode As String = If(row("countrycode").ToString(), "")
                                        row("DisplayText") = $"{countryName} ({countryCode})"
                                    Next
                                End If

                                CountryCB.DataSource = countryData
                                CountryCB.DisplayMember = "DisplayText"
                                CountryCB.ValueMember = "countrycode"
                                CountryCB.SelectedIndex = -1
                            End If
                        End If
                    Catch ex As Exception
                        System.Diagnostics.Debug.WriteLine("Error loading CountryCB: " & ex.Message)
                        CountryCB.DataSource = Nothing
                        CountryCB.Items.Clear()
                        CountryCB.Items.Add("لا توجد بيانات")
                        CountryCB.SelectedIndex = -1
                    End Try
                End If

            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("General error in LoadLookupData: " & ex.Message)
            End Try
        End Sub

        ' Set lookup ComboBoxes from loaded customer data FK fields
        Private Sub SetLookupComboBoxes(customerData As CustomerSupplierData)
            Try
                ' Set MarketCB from SalesMan FK
                If MarketCB IsNot Nothing AndAlso Not String.IsNullOrEmpty(customerData.SalesMan) AndAlso MarketCB.DataSource IsNot Nothing Then
                    Dim marketTable As DataTable = CType(MarketCB.DataSource, DataTable)
                    For i As Integer = 0 To marketTable.Rows.Count - 1
                        If marketTable.Rows(i)("fld_area_code").ToString() = customerData.SalesMan Then
                            MarketCB.SelectedIndex = i
                            Exit For
                        End If
                    Next
                End If

                ' Set GroupsCB from ScrapAdjCode FK
                If GroupsCB IsNot Nothing AndAlso Not String.IsNullOrEmpty(customerData.ScrapAdjCode) AndAlso GroupsCB.DataSource IsNot Nothing Then
                    Dim groupsTable As DataTable = CType(GroupsCB.DataSource, DataTable)
                    For i As Integer = 0 To groupsTable.Rows.Count - 1
                        If groupsTable.Rows(i)("code").ToString() = customerData.ScrapAdjCode Then
                            GroupsCB.SelectedIndex = i
                            Exit For
                        End If
                    Next
                End If

                ' Set TypeCB from TypeCode FK
                If TypeCB IsNot Nothing AndAlso Not String.IsNullOrEmpty(customerData.TypeCode) AndAlso TypeCB.DataSource IsNot Nothing Then
                    Dim typeTable As DataTable = CType(TypeCB.DataSource, DataTable)
                    For i As Integer = 0 To typeTable.Rows.Count - 1
                        If typeTable.Rows(i)("fld_code").ToString() = customerData.TypeCode Then
                            TypeCB.SelectedIndex = i
                            Exit For
                        End If
                    Next
                End If

                ' Set CatogeryCB from CategoryCode FK
                If CatogeryCB IsNot Nothing AndAlso Not String.IsNullOrEmpty(customerData.CategoryCode) AndAlso CatogeryCB.DataSource IsNot Nothing Then
                    Dim categoryTable As DataTable = CType(CatogeryCB.DataSource, DataTable)
                    For i As Integer = 0 To categoryTable.Rows.Count - 1
                        If categoryTable.Rows(i)("fld_code").ToString() = customerData.CategoryCode Then
                            CatogeryCB.SelectedIndex = i
                            Exit For
                        End If
                    Next
                End If

            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error setting lookup ComboBoxes: " & ex.Message)
            End Try
        End Sub

        Private Sub CustomerSupplierCB_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CustomerSupplierCB.SelectedIndexChanged
            ' Handle customer/supplier type selection with code generation
            Try
                If CustomerSupplierCB.SelectedItem IsNot Nothing AndAlso Not isNavigating Then
                    ' Generate new code when customer/supplier type changes
                    GenerateAndDisplayNextCode()
                End If
            Catch ex As Exception
                MessageBox.Show("خطأ في تحديد نوع العميل/المورد: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub IdentityCommercialNameOptionCB_SelectedIndexChanged(sender As Object, e As EventArgs) Handles IdentityCommercialNameOptionCB.SelectedIndexChanged
            ' Handle identity type selection and update label dynamically
            Try
                If IdentityCommercialNameOptionCB.SelectedItem IsNot Nothing Then
                    Dim selectedType As String = IdentityCommercialNameOptionCB.SelectedItem.ToString()
                    UpdateIdentityLabel(selectedType)
                End If
            Catch ex As Exception
                MessageBox.Show("خطأ في تحديد نوع الهوية: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub UpdateIdentityLabel(identityType As String)
            Try
                If CommercialRecordAndIdentityLB IsNot Nothing Then
                    If identityType = "فردي" Then
                        CommercialRecordAndIdentityLB.Text = "رقم الهوية"
                    ElseIf identityType = "تجاري" Then
                        CommercialRecordAndIdentityLB.Text = "رقم السجل"
                    End If
                End If
            Catch ex As Exception
                MessageBox.Show("خطأ في تحديث تسمية الهوية: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        ' Handle ActiveNoActiveCKB checkbox state change
        Private Sub ActiveNoActiveCKB_CheckedChanged(sender As Object, e As EventArgs) Handles ActiveNoActiveCKB.CheckedChanged
            Try
                If CustomerAccountNumberTB IsNot Nothing Then
                    ' Enable/disable CustomerAccountNumberTB based on ActiveNoActiveCKB state
                    CustomerAccountNumberTB.Enabled = ActiveNoActiveCKB.Checked

                    If Not ActiveNoActiveCKB.Checked Then
                        ' Clear the text when disabled
                        CustomerAccountNumberTB.Text = ""
                    End If

                    CustomerAccountNumberTB.Refresh()
                End If
            Catch ex As Exception
                MessageBox.Show("خطأ في تحديث حالة رقم الحساب: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        ' Handle VTRAppliedCKB checkbox state change
        Private Sub VTRAppliedCKB_CheckedChanged(sender As Object, e As EventArgs) Handles VTRAppliedCKB.CheckedChanged
            Try
                If VTRnumberTB IsNot Nothing Then
                    ' Disable VTRnumberTB when VTRAppliedCKB is checked
                    VTRnumberTB.Enabled = Not VTRAppliedCKB.Checked

                    If VTRAppliedCKB.Checked Then
                        ' Clear the text when disabled
                        VTRnumberTB.Text = ""
                    End If

                    VTRnumberTB.Refresh()
                End If
            Catch ex As Exception
                MessageBox.Show("خطأ في تحديث حالة رقم الضريبة: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        ' Generate and display next available customer/supplier code
        Private Sub GenerateAndDisplayNextCode()
            Try
                If CustomerSupplierCB IsNot Nothing AndAlso CustomerSupplierCB.SelectedItem IsNot Nothing Then
                    Dim isCustomer As Boolean = CustomerSupplierCB.SelectedItem.ToString() = "Customer"
                    Dim nextCode As String = dbConn.GenerateNextCode(isCustomer)

                    ' Display the generated code in a suitable control (assuming there's a code display textbox)
                    ' If there's no specific code display control, we could show it in the form title or a label
                    Me.Text = $"العملاء - الكود التالي: {nextCode}"
                End If
            Catch ex As Exception
                MessageBox.Show("خطأ في توليد الكود: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub


        ' Load existing customer/supplier data by code
        Public Sub LoadCustomerSupplierData(code As String)
            Try
                Dim customerData As CustomerSupplierData = dbConn.GetCustomerSupplierByCode(code)

                If Not String.IsNullOrEmpty(customerData.Code) Then
                    ' Populate form with loaded data
                    If CustomerSupplierCB IsNot Nothing Then
                        CustomerSupplierCB.SelectedItem = customerData.CustomerType
                    End If

                    If NameInEnglishTB IsNot Nothing Then NameInEnglishTB.Text = customerData.EnglishName
                    If FormalNameTB IsNot Nothing Then FormalNameTB.Text = customerData.ArabicName
                    If CommercialNameTB IsNot Nothing Then CommercialNameTB.Text = customerData.CommercialName
                    If AddressTA IsNot Nothing Then AddressTA.Text = customerData.Address
                    If ManagerTB IsNot Nothing Then ManagerTB.Text = customerData.Manager
                    If ManagerIDTB IsNot Nothing Then ManagerIDTB.Text = customerData.ManagerID
                    If MangerNumberTB IsNot Nothing Then MangerNumberTB.Text = customerData.ManagerNumber
                    If VTRnumberTB IsNot Nothing Then VTRnumberTB.Text = customerData.VATNumber
                    If emailTB IsNot Nothing Then emailTB.Text = customerData.Email
                    If phoneNumber1ZipCodeTB IsNot Nothing Then phoneNumber1ZipCodeTB.Text = customerData.MobileCountryCode
                    If FaxNumberTB IsNot Nothing Then FaxNumberTB.Text = customerData.FaxNumber
                    If ReferralNumberTB IsNot Nothing Then ReferralNumberTB.Text = customerData.ReferralNumber

                    ' Set identity type and corresponding field
                    If IdentityCommercialNameOptionCB IsNot Nothing Then
                        IdentityCommercialNameOptionCB.SelectedItem = customerData.IdentityType
                        UpdateIdentityLabel(customerData.IdentityType)

                        If CommercialRecordAndIdentityTB IsNot Nothing Then
                            If customerData.IsIndividual Then
                                CommercialRecordAndIdentityTB.Text = customerData.IndividualID
                            ElseIf customerData.IsCommercial Then
                                CommercialRecordAndIdentityTB.Text = customerData.CommercialRecord
                            End If
                        End If
                    End If

                    ' TODO: Set CountryCB and AreaCB based on loaded data
                    ' This would require searching the ComboBox items for matching values

                    MessageBox.Show($"تم تحميل بيانات {If(customerData.IsCustomer, "العميل", "المورد")} بنجاح!", "تم التحميل", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("لم يتم العثور على بيانات بالكود المحدد.", "غير موجود", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If

            Catch ex As Exception
                MessageBox.Show("خطأ في تحميل بيانات العميل/المورد: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        ' Clear form (helper method)
        Private Sub ClearForm()
            Try
                ' Clear all text boxes
                If NameInEnglishTB IsNot Nothing Then NameInEnglishTB.Clear()
                If FormalNameTB IsNot Nothing Then FormalNameTB.Clear()
                If CommercialNameTB IsNot Nothing Then CommercialNameTB.Clear()
                If AddressTA IsNot Nothing Then AddressTA.Clear()
                If ManagerTB IsNot Nothing Then ManagerTB.Clear()
                If ManagerIDTB IsNot Nothing Then ManagerIDTB.Clear()
                If MangerNumberTB IsNot Nothing Then MangerNumberTB.Clear()
                If VTRnumberTB IsNot Nothing Then VTRnumberTB.Clear()
                If emailTB IsNot Nothing Then emailTB.Clear()
                If phoneNumber1ZipCodeTB IsNot Nothing Then phoneNumber1ZipCodeTB.Clear()
                If FaxNumberTB IsNot Nothing Then FaxNumberTB.Clear()
                If ReferralNumberTB IsNot Nothing Then ReferralNumberTB.Clear()
                If CommercialRecordAndIdentityTB IsNot Nothing Then CommercialRecordAndIdentityTB.Clear()

                ' Reset ComboBoxes to default
                If CustomerSupplierCB IsNot Nothing Then CustomerSupplierCB.SelectedIndex = 0
                If IdentityCommercialNameOptionCB IsNot Nothing Then
                    IdentityCommercialNameOptionCB.SelectedIndex = 0
                    UpdateIdentityLabel("فردي")
                End If

            Catch ex As Exception
                MessageBox.Show("خطأ في مسح النموذج: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        ' =====================Navigation and Save Methods========================

        Private Sub SaveInfo_Click(sender As Object, e As EventArgs) Handles SaveInfo.Click
            ' Save current customer/supplier data
            SaveCustomerSupplierData()

            ' Refresh customer list after save to include any new records (without loading ComboBox data)
            RefreshCustomerListOnly()
        End Sub

        Private Sub LoadCustomerListForNavigation()
            Try
                ' Load all customer/supplier codes from database
                System.Diagnostics.Debug.WriteLine("Loading customer list from database...")
                customerList = dbConn.GetAllCustomerSupplierCodes()
                System.Diagnostics.Debug.WriteLine($"Loaded {customerList?.Count} customers from database")

                ' If we have customers, load the first one
                If customerList IsNot Nothing AndAlso customerList.Count > 0 Then
                    currentCustomerIndex = 0
                    LoadCustomerByIndex(currentCustomerIndex)
                Else
                    currentCustomerIndex = -1
                    ClearForm()
                    MessageBox.Show("لا توجد عملاء/موردين في قاعدة البيانات", "لا توجد بيانات", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If

            Catch ex As Exception
                MessageBox.Show("خطأ في تحميل قائمة العملاء: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        ' Refresh customer list without loading ComboBox data or changing current customer
        Private Sub RefreshCustomerListOnly()
            Try
                ' Store current customer code
                Dim currentCode As String = ""
                If currentCustomerIndex >= 0 AndAlso currentCustomerIndex < customerList.Count Then
                    currentCode = customerList(currentCustomerIndex)
                End If

                ' Reload customer list from database
                customerList = dbConn.GetAllCustomerSupplierCodes()

                ' Try to find the current customer in the updated list
                If Not String.IsNullOrEmpty(currentCode) Then
                    For i As Integer = 0 To customerList.Count - 1
                        If customerList(i) = currentCode Then
                            currentCustomerIndex = i
                            Exit For
                        End If
                    Next

                    ' Update form title
                    Me.Text = $"العملاء - {currentCustomerIndex + 1} من {customerList.Count} - {currentCode}"
                End If

            Catch ex As Exception
                MessageBox.Show("خطأ في تحديث قائمة العملاء: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        ' Initialize navigation without loading first customer data or ComboBox data
        Private Sub InitializeNavigationOnly()
            Try
                ' Load all customer/supplier codes from database
                System.Diagnostics.Debug.WriteLine("Initializing navigation - loading customer list...")
                customerList = dbConn.GetAllCustomerSupplierCodes()
                System.Diagnostics.Debug.WriteLine($"Navigation initialized with {customerList?.Count} customers")

                ' Set navigation index but don't load customer data yet
                If customerList IsNot Nothing AndAlso customerList.Count > 0 Then
                    currentCustomerIndex = 0
                    ' Show navigation info without loading data
                    Me.Text = $"العملاء - {currentCustomerIndex + 1} من {customerList.Count} - (استخدم أزرار التنقل لتحميل البيانات)"
                    System.Diagnostics.Debug.WriteLine($"Navigation ready: {currentCustomerIndex + 1} of {customerList.Count} customers")
                Else
                    currentCustomerIndex = -1
                    Me.Text = "العملاء - لا توجد بيانات"
                    System.Diagnostics.Debug.WriteLine("No customers found in database")
                    MessageBox.Show("لا توجد عملاء/موردين في قاعدة البيانات", "لا توجد بيانات", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If

            Catch ex As Exception
                MessageBox.Show("خطأ في تهيئة قائمة العملاء: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        ' Load all ComboBox data (called only during navigation)
        Private Sub LoadAllComboBoxData()
            Try
                ' Load all lookup data including countries, markets, categories, and groups
                LoadLookupData()
                LoadTypeData()

            Catch ex As Exception
                MessageBox.Show("خطأ في تحميل بيانات القوائم المنسدلة: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub NavigateToNextCustomer()
            Try
                ' Ensure customer list is loaded
                If customerList Is Nothing OrElse customerList.Count = 0 Then
                    System.Diagnostics.Debug.WriteLine("Customer list is empty, loading customer list...")
                    LoadCustomerListForNavigation()
                    If customerList.Count = 0 Then
                        MessageBox.Show("لا توجد سجلات عملاء/موردين في قاعدة البيانات.", "لا توجد سجلات", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return
                    End If
                End If

                ' Move to next customer (with wrapping)
                currentCustomerIndex += 1
                If currentCustomerIndex >= customerList.Count Then
                    currentCustomerIndex = 0 ' Wrap to first customer
                End If

                System.Diagnostics.Debug.WriteLine($"Navigating to next customer: {currentCustomerIndex + 1} of {customerList.Count}")
                LoadCustomerByIndex(currentCustomerIndex)

            Catch ex As Exception
                MessageBox.Show("خطأ في الانتقال للعميل التالي: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub NavigateToPreviousCustomer()
            Try
                ' Ensure customer list is loaded
                If customerList Is Nothing OrElse customerList.Count = 0 Then
                    System.Diagnostics.Debug.WriteLine("Customer list is empty, loading customer list...")
                    LoadCustomerListForNavigation()
                    If customerList.Count = 0 Then
                        MessageBox.Show("لا توجد سجلات عملاء/موردين في قاعدة البيانات.", "لا توجد سجلات", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return
                    End If
                End If

                ' Move to previous customer (with wrapping)
                currentCustomerIndex -= 1
                If currentCustomerIndex < 0 Then
                    currentCustomerIndex = customerList.Count - 1 ' Wrap to last customer
                End If

                System.Diagnostics.Debug.WriteLine($"Navigating to previous customer: {currentCustomerIndex + 1} of {customerList.Count}")
                LoadCustomerByIndex(currentCustomerIndex)

            Catch ex As Exception
                MessageBox.Show("خطأ في الانتقال للعميل السابق: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub LoadCustomerByIndex(index As Integer)
            Try
                If index < 0 OrElse index >= customerList.Count Then
                    System.Diagnostics.Debug.WriteLine($"Invalid index: {index}, customer list count: {customerList?.Count}")
                    Return
                End If

                isNavigating = True
                Dim customerCode As String = customerList(index)

                ' Ensure ComboBoxes are loaded before populating customer data
                System.Diagnostics.Debug.WriteLine("Loading ComboBox data for navigation...")
                Try
                    LoadAllComboBoxData()
                Catch comboEx As Exception
                    System.Diagnostics.Debug.WriteLine($"Warning: ComboBox loading failed: {comboEx.Message}")
                End Try

                ' Load customer data
                System.Diagnostics.Debug.WriteLine($"Loading customer data for code: {customerCode}")
                Dim customerData As CustomerSupplierData = dbConn.GetCustomerSupplierByCode(customerCode)
                System.Diagnostics.Debug.WriteLine($"Customer data loaded - Code: {customerData?.Code}, IsNull: {customerData Is Nothing}")

                If customerData IsNot Nothing AndAlso Not String.IsNullOrEmpty(customerData.Code) Then
                    System.Diagnostics.Debug.WriteLine($"Customer data found! Starting form population for: {customerData.Code}")
                    ' Populate form with loaded data - force UI update
                    Try
                        ' Clear all fields first
                        ClearForm()

                        ' Populate Customer/Supplier Type
                        If CustomerSupplierCB IsNot Nothing AndAlso Not String.IsNullOrEmpty(customerData.CustomerType) Then
                            For i As Integer = 0 To CustomerSupplierCB.Items.Count - 1
                                If CustomerSupplierCB.Items(i).ToString() = customerData.CustomerType Then
                                    CustomerSupplierCB.SelectedIndex = i
                                    Exit For
                                End If
                            Next
                        End If

                        ' Analyze code prefix and set CustomerSupplierCB accordingly
                        If Not String.IsNullOrEmpty(customerData.Code) Then
                            If customerData.Code.ToUpper().StartsWith("C") Then
                                ' Code starts with C - set to Customer (first item, index 0)
                                If CustomerSupplierCB IsNot Nothing AndAlso CustomerSupplierCB.Items.Count > 0 Then
                                    CustomerSupplierCB.SelectedIndex = 0
                                End If
                            ElseIf customerData.Code.ToUpper().StartsWith("S") Then
                                ' Code starts with S - set to Supplier (second item, index 1)
                                If CustomerSupplierCB IsNot Nothing AndAlso CustomerSupplierCB.Items.Count > 1 Then
                                    CustomerSupplierCB.SelectedIndex = 1
                                End If
                            End If
                        End If

                        ' Handle ActiveNoActiveCKB checkbox and CustomerAccountNumberTB state
                        If ActiveNoActiveCKB IsNot Nothing Then
                            ' Set ActiveNoActiveCKB based on database active column
                            ActiveNoActiveCKB.Checked = customerData.Active
                            ActiveNoActiveCKB.Refresh()

                            ' Enable/disable CustomerAccountNumberTB based on Active status
                            If CustomerAccountNumberTB IsNot Nothing Then
                                CustomerAccountNumberTB.Enabled = customerData.Active
                                If customerData.Active Then
                                    ' Populate the CustomerAccountNumberTB if Active is true
                                    CustomerAccountNumberTB.Text = If(String.IsNullOrEmpty(customerData.CommercialRecord), "", customerData.CommercialRecord)
                                Else
                                    ' Clear the field if Active is false
                                    CustomerAccountNumberTB.Text = ""
                                End If
                                CustomerAccountNumberTB.Refresh()
                            End If
                        End If

                        ' Handle VTRAppliedCKB checkbox enablement based on CustomerAccountNumberTB data
                        If VTRAppliedCKB IsNot Nothing Then
                            ' Check if CustomerAccountNumberTB contains data to determine if VTRAppliedCKB should be enabled
                            Dim hasCustomerAccountData As Boolean = False
                            If CustomerAccountNumberTB IsNot Nothing Then
                                hasCustomerAccountData = Not String.IsNullOrEmpty(CustomerAccountNumberTB.Text.Trim())
                            End If

                            If hasCustomerAccountData Then
                                ' Enable VTRAppliedCKB when CustomerAccountNumberTB has data
                                VTRAppliedCKB.Enabled = True
                            Else
                                ' Disable VTRAppliedCKB if CustomerAccountNumberTB has no data
                                VTRAppliedCKB.Enabled = False
                                VTRAppliedCKB.Checked = False
                            End If
                            VTRAppliedCKB.Refresh()

                            ' Handle VTRnumberTB state based on VTRAppliedCKB
                            If VTRnumberTB IsNot Nothing Then
                                ' Disable VTRnumberTB when VTRAppliedCKB is checked
                                VTRnumberTB.Enabled = Not VTRAppliedCKB.Checked
                                VTRnumberTB.Refresh()
                            End If
                        End If

                        ' Populate all text fields with data
                        ' Debug: Show customer data details
                        System.Diagnostics.Debug.WriteLine($"Customer Data Details:")
                        System.Diagnostics.Debug.WriteLine($"  Code: '{customerData.Code}'")
                        System.Diagnostics.Debug.WriteLine($"  English Name: '{customerData.EnglishName}'")
                        System.Diagnostics.Debug.WriteLine($"  Arabic Name: '{customerData.ArabicName}'")
                        System.Diagnostics.Debug.WriteLine($"  Active: {customerData.Active}")

                        System.Diagnostics.Debug.WriteLine("Starting to populate text fields...")
                        If NameInEnglishTB IsNot Nothing Then
                            NameInEnglishTB.Text = If(String.IsNullOrEmpty(customerData.EnglishName), "", customerData.EnglishName)
                            NameInEnglishTB.Refresh()
                            System.Diagnostics.Debug.WriteLine($"English Name set to: '{NameInEnglishTB.Text}'")
                        End If

                        If FormalNameTB IsNot Nothing Then
                            FormalNameTB.Text = If(String.IsNullOrEmpty(customerData.ArabicName), "", customerData.ArabicName)
                            FormalNameTB.Refresh()
                        End If

                        If CommercialNameTB IsNot Nothing Then
                            CommercialNameTB.Text = If(String.IsNullOrEmpty(customerData.CommercialName), "", customerData.CommercialName)
                            CommercialNameTB.Refresh()
                        End If

                        If AddressTA IsNot Nothing Then
                            AddressTA.Text = If(String.IsNullOrEmpty(customerData.Address), "", customerData.Address)
                            AddressTA.Refresh()
                        End If

                        If ManagerTB IsNot Nothing Then
                            ManagerTB.Text = If(String.IsNullOrEmpty(customerData.Manager), "", customerData.Manager)
                            ManagerTB.Refresh()
                        End If

                        If ManagerIDTB IsNot Nothing Then
                            ManagerIDTB.Text = If(String.IsNullOrEmpty(customerData.ManagerID), "", customerData.ManagerID)
                            ManagerIDTB.Refresh()
                        End If

                        If MangerNumberTB IsNot Nothing Then
                            MangerNumberTB.Text = If(String.IsNullOrEmpty(customerData.ManagerNumber), "", customerData.ManagerNumber)
                            MangerNumberTB.Refresh()
                        End If

                        If VTRnumberTB IsNot Nothing Then
                            VTRnumberTB.Text = If(String.IsNullOrEmpty(customerData.VATNumber), "", customerData.VATNumber)
                            VTRnumberTB.Refresh()
                        End If

                        If emailTB IsNot Nothing Then
                            emailTB.Text = If(String.IsNullOrEmpty(customerData.Email), "", customerData.Email)
                            emailTB.Refresh()
                        End If

                        If phoneNumber1ZipCodeTB IsNot Nothing Then
                            phoneNumber1ZipCodeTB.Text = If(String.IsNullOrEmpty(customerData.MobileCountryCode), "", customerData.MobileCountryCode)
                            phoneNumber1ZipCodeTB.Refresh()
                        End If

                        If FaxNumberTB IsNot Nothing Then
                            FaxNumberTB.Text = If(String.IsNullOrEmpty(customerData.FaxNumber), "", customerData.FaxNumber)
                            FaxNumberTB.Refresh()
                        End If

                        If ReferralNumberTB IsNot Nothing Then
                            ReferralNumberTB.Text = If(String.IsNullOrEmpty(customerData.ReferralNumber), "", customerData.ReferralNumber)
                            ReferralNumberTB.Refresh()
                        End If

                        ' Add missing fields from the field mapping specification
                        If CustomerAccountNumberTB IsNot Nothing Then
                            CustomerAccountNumberTB.Text = If(String.IsNullOrEmpty(customerData.CommercialRecord), "", customerData.CommercialRecord)
                            CustomerAccountNumberTB.Refresh()
                        End If

                        ' Add all the missing controls you mentioned
                        If ActiveNoActiveCKB IsNot Nothing Then
                            ActiveNoActiveCKB.Checked = False ' Default value, no field mapping specified
                            ActiveNoActiveCKB.Refresh()

                            ' Ensure CustomerAccountNumberTB is disabled when ActiveNoActiveCKB is unchecked
                            If CustomerAccountNumberTB IsNot Nothing Then
                                CustomerAccountNumberTB.Enabled = False
                            End If
                        End If

                        If VTRAppliedCKB IsNot Nothing Then
                            VTRAppliedCKB.Checked = False ' Default value, no field mapping specified
                            VTRAppliedCKB.Enabled = False ' Default disabled until CustomerAccountNumberTB has data
                            VTRAppliedCKB.Refresh()

                            ' Ensure VTRnumberTB is enabled when VTRAppliedCKB is unchecked (default state)
                            If VTRnumberTB IsNot Nothing Then
                                VTRnumberTB.Enabled = Not VTRAppliedCKB.Checked ' True since VTRAppliedCKB is unchecked by default
                            End If
                        End If

                        If phoneNumber1TB IsNot Nothing Then
                            phoneNumber1TB.Text = "" ' No direct mapping specified in your table
                            phoneNumber1TB.Refresh()
                        End If

                        If phoneNumber2TB IsNot Nothing Then
                            phoneNumber2TB.Text = "" ' No direct mapping specified in your table
                            phoneNumber2TB.Refresh()
                        End If

                        If telephoneNumberTB IsNot Nothing Then
                            telephoneNumberTB.Text = "" ' No direct mapping specified in your table
                            telephoneNumberTB.Refresh()
                        End If

                        If telephoneNumberZipcodeTB IsNot Nothing Then
                            telephoneNumberZipcodeTB.Text = "" ' Maps to pld_limit but no data available yet
                            telephoneNumberZipcodeTB.Refresh()
                        End If

                        ' Set identity type and corresponding field
                        If IdentityCommercialNameOptionCB IsNot Nothing AndAlso Not String.IsNullOrEmpty(customerData.IdentityType) Then
                            For i As Integer = 0 To IdentityCommercialNameOptionCB.Items.Count - 1
                                If IdentityCommercialNameOptionCB.Items(i).ToString() = customerData.IdentityType Then
                                    IdentityCommercialNameOptionCB.SelectedIndex = i
                                    Exit For
                                End If
                            Next
                            UpdateIdentityLabel(customerData.IdentityType)

                            If CommercialRecordAndIdentityTB IsNot Nothing Then
                                If customerData.IsIndividual Then
                                    CommercialRecordAndIdentityTB.Text = If(String.IsNullOrEmpty(customerData.IndividualID), "", customerData.IndividualID)
                                ElseIf customerData.IsCommercial Then
                                    CommercialRecordAndIdentityTB.Text = If(String.IsNullOrEmpty(customerData.CommercialRecord), "", customerData.CommercialRecord)
                                End If
                                CommercialRecordAndIdentityTB.Refresh()
                            End If
                        End If

                        ' Set country ComboBox
                        If Not String.IsNullOrEmpty(customerData.Country) AndAlso CountryCB IsNot Nothing AndAlso CountryCB.DataSource IsNot Nothing Then
                            Dim countryTable As DataTable = CType(CountryCB.DataSource, DataTable)
                            Dim countryFound As Boolean = False
                            For i As Integer = 0 To countryTable.Rows.Count - 1
                                If countryTable.Rows(i)("countrycode").ToString() = customerData.Country Then
                                    CountryCB.SelectedIndex = i
                                    ' Load areas for this country
                                    LoadAreas(customerData.Country)
                                    countryFound = True
                                    Exit For
                                End If
                            Next

                            ' If country not found in the list, clear the selection
                            If Not countryFound Then
                                CountryCB.SelectedIndex = -1
                                CountryCB.Text = ""
                            End If
                        Else
                            ' Country is empty or null - clear country and area selections
                            If CountryCB IsNot Nothing Then
                                CountryCB.SelectedIndex = -1
                                CountryCB.Text = ""
                            End If
                            If AreaCB IsNot Nothing Then
                                AreaCB.DataSource = Nothing
                                AreaCB.Tag = Nothing
                                AreaCB.Text = ""
                            End If
                        End If

                        ' Set area ComboBox  
                        If Not String.IsNullOrEmpty(customerData.Area) AndAlso AreaCB IsNot Nothing AndAlso AreaCB.DataSource IsNot Nothing Then
                            Dim areaTable As DataTable = CType(AreaCB.DataSource, DataTable)
                            Dim areaFound As Boolean = False
                            For i As Integer = 0 To areaTable.Rows.Count - 1
                                If areaTable.Rows(i)("code").ToString() = customerData.Area Then
                                    AreaCB.SelectedIndex = i
                                    areaFound = True
                                    Exit For
                                End If
                            Next

                            ' If area not found in the list, clear the selection
                            If Not areaFound Then
                                AreaCB.SelectedIndex = -1
                                AreaCB.Text = ""
                            End If
                        Else
                            ' Area is empty - clear area selection if no country was set above
                            If AreaCB IsNot Nothing AndAlso String.IsNullOrEmpty(customerData.Country) Then
                                AreaCB.SelectedIndex = -1
                                AreaCB.Text = ""
                            End If
                        End If

                        ' Set lookup ComboBoxes from FK fields
                        SetLookupComboBoxes(customerData)

                        ' Force form refresh
                        Me.Refresh()
                        Application.DoEvents()

                        System.Diagnostics.Debug.WriteLine($"Form population completed successfully for customer: {customerData.Code}")

                    Catch fieldEx As Exception
                        System.Diagnostics.Debug.WriteLine($"Error populating form fields: {fieldEx.Message}")
                        MessageBox.Show("خطأ في تعبئة الحقول: " & fieldEx.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End Try
                Else
                    System.Diagnostics.Debug.WriteLine($"Customer not found or empty data for code: {customerCode}")
                    MessageBox.Show($"لم يتم العثور على بيانات العميل: {customerCode}" & Environment.NewLine &
                               $"تأكد من وجود العميل في قاعدة البيانات", "خطأ - C0004", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If

                ' Update current selected customer info
                currentSelectedCustomerName = customerCode

                ' Show navigation info in title
                Me.Text = $"العملاء - {index + 1} من {customerList.Count} - {customerCode}"

                isNavigating = False

            Catch ex As Exception
                isNavigating = False
                MessageBox.Show("خطأ في تحميل بيانات العميل: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        ' Override the existing SaveCustomerSupplierData method to handle navigation context
        Public Shadows Sub SaveCustomerSupplierData()
            Try
                ' Create customer data object
                Dim customerData As New CustomerSupplierData()

                ' Check if we're updating an existing customer or creating new one
                If currentCustomerIndex >= 0 AndAlso currentCustomerIndex < customerList.Count Then
                    ' We're updating an existing customer
                    customerData.ExistingCode = customerList(currentCustomerIndex)
                    customerData.IsUpdate = True
                    customerData.Code = customerData.ExistingCode
                Else
                    ' We're creating a new customer with C/S prefix auto-increment
                    Dim isCustomer As Boolean = CustomerSupplierCB.SelectedItem?.ToString() = "Customer"
                    customerData.Code = dbConn.GenerateNextCode(isCustomer) ' This will generate C0001, C0002... or S0001, S0002...
                    customerData.IsUpdate = False
                End If

                ' Map form data to customer data object (same as before)
                customerData.CustomerType = If(CustomerSupplierCB.SelectedItem IsNot Nothing, CustomerSupplierCB.SelectedItem.ToString(), "Customer")
                customerData.EnglishName = If(NameInEnglishTB IsNot Nothing, NameInEnglishTB.Text.Trim(), "")
                customerData.ArabicName = If(FormalNameTB IsNot Nothing, FormalNameTB.Text.Trim(), "")
                customerData.CommercialName = If(CommercialNameTB IsNot Nothing, CommercialNameTB.Text.Trim(), "")
                customerData.Address = If(AddressTA IsNot Nothing, AddressTA.Text.Trim(), "")
                customerData.Manager = If(ManagerTB IsNot Nothing, ManagerTB.Text.Trim(), "")
                customerData.ManagerID = If(ManagerIDTB IsNot Nothing, ManagerIDTB.Text.Trim(), "")
                customerData.ManagerNumber = If(MangerNumberTB IsNot Nothing, MangerNumberTB.Text.Trim(), "")

                ' Get selected country and area
                If CountryCB.SelectedItem IsNot Nothing Then
                    Dim selectedCountry = CType(CountryCB.SelectedItem, DataRowView)
                    customerData.Country = selectedCountry("countrycode").ToString()
                    customerData.CountryName = selectedCountry("DisplayText").ToString()
                End If

                If AreaCB.SelectedItem IsNot Nothing Then
                    Dim selectedArea = CType(AreaCB.SelectedItem, DataRowView)
                    customerData.Area = selectedArea("code").ToString()
                End If

                customerData.VATNumber = If(VTRnumberTB IsNot Nothing, VTRnumberTB.Text.Trim(), "")
                customerData.Email = If(emailTB IsNot Nothing, emailTB.Text.Trim(), "")
                customerData.MobileCountryCode = If(phoneNumber1ZipCodeTB IsNot Nothing, phoneNumber1ZipCodeTB.Text.Trim(), "")
                customerData.FaxNumber = If(FaxNumberTB IsNot Nothing, FaxNumberTB.Text.Trim(), "")
                customerData.ReferralNumber = If(ReferralNumberTB IsNot Nothing, ReferralNumberTB.Text.Trim(), "")

                ' Handle identity type and corresponding field mapping per specification
                If IdentityCommercialNameOptionCB.SelectedItem IsNot Nothing Then
                    customerData.IdentityType = IdentityCommercialNameOptionCB.SelectedItem.ToString()

                    If customerData.IdentityType = "فردي" Then
                        ' Individual - save CommercialRecordAndIdentityTB value to fld_indvl_id_no field
                        customerData.IndividualID = If(CommercialRecordAndIdentityTB IsNot Nothing, CommercialRecordAndIdentityTB.Text.Trim(), "")
                        ' Clear commercial record field for individual type
                        customerData.CommercialRecord = ""
                    ElseIf customerData.IdentityType = "تجاري" Then
                        ' Commercial - save CommercialRecordAndIdentityTB value to fld_cr_no field
                        customerData.CommercialRecord = If(CommercialRecordAndIdentityTB IsNot Nothing, CommercialRecordAndIdentityTB.Text.Trim(), "")
                        ' Clear individual ID field for commercial type
                        customerData.IndividualID = ""
                    End If
                End If

                ' Handle Active status based on ActiveNoActiveCKB checkbox
                customerData.Active = If(ActiveNoActiveCKB IsNot Nothing, ActiveNoActiveCKB.Checked, False)

                ' Map ComboBox selections to FK fields for CustomerAccountsMaster
                ' SalesMan from MarketCB (CusTransactionMaster.fld_area_code)
                If MarketCB IsNot Nothing AndAlso MarketCB.SelectedValue IsNot Nothing Then
                    customerData.SalesMan = MarketCB.SelectedValue.ToString()
                End If

                ' ScrapAdjCode from GroupsCB (CusGradeMaster.code)
                If GroupsCB IsNot Nothing AndAlso GroupsCB.SelectedValue IsNot Nothing Then
                    customerData.ScrapAdjCode = GroupsCB.SelectedValue.ToString()
                End If

                ' TypeCode from TypeCB (CustomerType.fld_code)
                If TypeCB IsNot Nothing AndAlso TypeCB.SelectedValue IsNot Nothing Then
                    customerData.TypeCode = TypeCB.SelectedValue.ToString()
                End If

                ' CategoryCode from CatogeryCB (CustomerCategory.fld_code)
                If CatogeryCB IsNot Nothing AndAlso CatogeryCB.SelectedValue IsNot Nothing Then
                    customerData.CategoryCode = CatogeryCB.SelectedValue.ToString()
                End If

                ' Handle VTRAppliedCKB enablement based on CustomerAccountNumberTB data when inserting
                If VTRAppliedCKB IsNot Nothing And CustomerAccountNumberTB IsNot Nothing Then
                    Dim hasCustomerAccountData As Boolean = Not String.IsNullOrEmpty(CustomerAccountNumberTB.Text.Trim())
                    If Not hasCustomerAccountData Then
                        ' If no customer account data, disable VTRAppliedCKB
                        VTRAppliedCKB.Enabled = False
                        VTRAppliedCKB.Checked = False
                    Else
                        ' If customer account data exists, enable VTRAppliedCKB
                        VTRAppliedCKB.Enabled = True
                    End If
                End If

                ' Save to database
                Dim success As Boolean = dbConn.SaveCustomerSupplier(customerData)

                If success Then
                    MessageBox.Show($"تم حفظ بيانات {If(customerData.IsCustomer, "العميل", "المورد")} بنجاح!" & vbCrLf & $"الكود: {customerData.Code}", "نجح الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    ' If this was a new customer, add it to the list and navigate to it
                    If Not customerData.IsUpdate Then
                        customerList.Add(customerData.Code)
                        currentCustomerIndex = customerList.Count - 1
                        Me.Text = $"العملاء - {currentCustomerIndex + 1} من {customerList.Count} - {customerData.Code}"
                    End If
                Else
                    MessageBox.Show("فشل في حفظ البيانات. يرجى المحاولة مرة أخرى.", "خطأ في الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If

            Catch ex As Exception
                MessageBox.Show("خطأ في حفظ بيانات العميل/المورد: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub


        Private Sub AddAttachmentsBT_Click(sender As Object, e As EventArgs) Handles AddAttachmentsBT.Click
            AttachmentsManagement.Show()

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

        End Sub



        Private Sub UpCustomersPB_Click(sender As Object, e As EventArgs) Handles UpCustomersPB.Click
            NavigateToNextCustomer()
        End Sub

        Private Sub downCustomersPB_Click(sender As Object, e As EventArgs) Handles downCustomersPB.Click
            NavigateToPreviousCustomer()

        End Sub

        Private Sub refreshPB_Click(sender As Object, e As EventArgs) Handles refreshPB.Click

            IdentityCommercialNameOptionCB.Text = ""
            CustomerAccountNumberTB.Text = ""
            NameInEnglishTB.Text = ""
            AddressTA.Text = ""
            ManagerTB.Text = ""
            ManagerIDTB.Text = ""
            FormalNameTB.Text = ""
            CommercialNameTB.Text = ""
            VTRnumberTB.Text = ""
            emailTB.Text = ""
            phoneNumber1TB.Text = ""
            phoneNumber1ZipCodeTB.Text = ""
            phoneNumber2TB.Text = ""
            telephoneNumberTB.Text = ""
            telephoneNumberZipcodeTB.Text = ""
            FaxNumberTB.Text = ""
            ReferralNumberTB.Text = ""

            ' CheckBoxes
            ActiveNoActiveCKB.Checked = False
            VTRAppliedCKB.Checked = False
            VTRAppliedCKB.Enabled = False ' Disable until CustomerAccountNumberTB has data

            ' Ensure CustomerAccountNumberTB is disabled when form is cleared
            If CustomerAccountNumberTB IsNot Nothing Then
                CustomerAccountNumberTB.Enabled = False
            End If

            ' Ensure VTRnumberTB is enabled when form is cleared (since VTRAppliedCKB is unchecked)
            If VTRnumberTB IsNot Nothing Then
                VTRnumberTB.Enabled = True
            End If

            ' ComboBoxes - reset to defaults
            If CustomerSupplierCB IsNot Nothing Then CustomerSupplierCB.SelectedIndex = 0 ' Default to Customer
            If IdentityCommercialNameOptionCB IsNot Nothing Then
                IdentityCommercialNameOptionCB.SelectedIndex = 0 ' Default to فردي
                UpdateIdentityLabel("فردي")
            End If
            CountryCB.SelectedIndex = -1
            AreaCB.SelectedIndex = -1
        End Sub

        ' Enhanced search event handlers for ComboBoxes
        Private Sub MarketCB_TextChanged(sender As Object, e As EventArgs)
            PerformEnhancedSearch(MarketCB, "MarketCB")
        End Sub

        Private Sub MarketCB_KeyUp(sender As Object, e As KeyEventArgs)
            If e.KeyCode = Keys.Escape Then
                MarketCB.DroppedDown = False
            ElseIf e.KeyCode <> Keys.Up AndAlso e.KeyCode <> Keys.Down Then
                MarketCB.DroppedDown = True
            End If
        End Sub

        Private Sub GroupsCB_TextChanged(sender As Object, e As EventArgs)
            PerformEnhancedSearch(GroupsCB, "GroupsCB")
        End Sub

        Private Sub GroupsCB_KeyUp(sender As Object, e As KeyEventArgs)
            If e.KeyCode = Keys.Escape Then
                GroupsCB.DroppedDown = False
            ElseIf e.KeyCode <> Keys.Up AndAlso e.KeyCode <> Keys.Down Then
                GroupsCB.DroppedDown = True
            End If
        End Sub

        Private Sub TypeCB_TextChanged(sender As Object, e As EventArgs)
            PerformEnhancedSearch(TypeCB, "TypeCB")
        End Sub

        Private Sub TypeCB_KeyUp(sender As Object, e As KeyEventArgs)
            If e.KeyCode = Keys.Escape Then
                TypeCB.DroppedDown = False
            ElseIf e.KeyCode <> Keys.Up AndAlso e.KeyCode <> Keys.Down Then
                TypeCB.DroppedDown = True
            End If
        End Sub

        Private Sub CatogeryCB_TextChanged(sender As Object, e As EventArgs)
            PerformEnhancedSearch(CatogeryCB, "CatogeryCB")
        End Sub

        Private Sub CatogeryCB_KeyUp(sender As Object, e As KeyEventArgs)
            If e.KeyCode = Keys.Escape Then
                CatogeryCB.DroppedDown = False
            ElseIf e.KeyCode <> Keys.Up AndAlso e.KeyCode <> Keys.Down Then
                CatogeryCB.DroppedDown = True
            End If
        End Sub

        ' Enhanced search function that searches any letter, not just the first
        Private Sub PerformEnhancedSearch(comboBox As ComboBox, comboName As String)
            Try
                If comboBox Is Nothing OrElse Not Me.IsHandleCreated Then Return

                Dim searchText As String = comboBox.Text.ToLower().Trim()
                Dim currentCursorPosition As Integer = comboBox.SelectionStart

                ' Get original data from Tag
                Dim originalData As DataTable = CType(comboBox.Tag, DataTable)
                If originalData Is Nothing Then
                    ' Store original data in Tag if not already stored
                    If comboBox.DataSource IsNot Nothing Then
                        originalData = CType(comboBox.DataSource, DataTable)
                        comboBox.Tag = originalData.Copy()
                        originalData = CType(comboBox.Tag, DataTable)
                    Else
                        Return
                    End If
                End If

                If String.IsNullOrEmpty(searchText) Then
                    ' Restore original data if search is empty
                    comboBox.DataSource = originalData
                    comboBox.Text = ""
                    Return
                End If

                ' Filter data based on search text - search any letter in DisplayText
                Dim filteredData As New DataTable()
                filteredData = originalData.Clone()

                For Each row As DataRow In originalData.Rows
                    Dim displayText As String = If(row("DisplayText").ToString().ToLower(), "")
                    ' Search for any occurrence of search text, not just at the beginning
                    If displayText.Contains(searchText) Then
                        filteredData.ImportRow(row)
                    End If
                Next

                ' Reorder results to prioritize matches that start with search text
                Dim reorderedTable As New DataTable()
                reorderedTable = filteredData.Clone()

                ' First add rows that start with the search text
                For Each row As DataRow In filteredData.Rows
                    Dim displayText As String = If(row("DisplayText").ToString().ToLower(), "")
                    If displayText.StartsWith(searchText) Then
                        reorderedTable.ImportRow(row)
                    End If
                Next

                ' Then add rows that contain the search text but don't start with it
                For Each row As DataRow In filteredData.Rows
                    Dim displayText As String = If(row("DisplayText").ToString().ToLower(), "")
                    If displayText.Contains(searchText) AndAlso Not displayText.StartsWith(searchText) Then
                        reorderedTable.ImportRow(row)
                    End If
                Next

                ' Update ComboBox with filtered data
                If comboBox IsNot Nothing Then
                    Dim userText As String = comboBox.Text
                    comboBox.DataSource = reorderedTable
                    comboBox.Text = userText
                    comboBox.SelectionStart = currentCursorPosition
                    comboBox.SelectionLength = 0

                    ' Only open dropdown if ComboBox has focus and user is actively typing
                    If comboBox.Focused AndAlso Not String.IsNullOrEmpty(searchText) AndAlso reorderedTable.Rows.Count > 0 Then
                        comboBox.DroppedDown = True
                    End If
                End If

            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Error in enhanced search for {comboName}: {ex.Message}")
            End Try
        End Sub


    End Class
