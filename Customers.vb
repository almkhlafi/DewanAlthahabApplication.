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

        ' Load additional ComboBoxes
        LoadMarketData()
        LoadCategoryData()
        LoadGroupsData()
        LoadTypeData()
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
            ' Temporarily remove CountryCB event handler to prevent interference
            RemoveHandler CountryCB.SelectedIndexChanged, AddressOf CountryCB_SelectedIndexChanged

            ' Load areas from CMGADB2024 database for specific country
            Dim areasTable As DataTable = dbConn.GetAreas(countryCode)

            ' Debug: Show exact results from database
            If areasTable.Rows.Count > 0 Then
                Dim firstRow = areasTable.Rows(0)
                MessageBox.Show($"Found {areasTable.Rows.Count} areas for '{countryCode}'{vbCrLf}First area: {firstRow("description")} - {firstRow("shortname")}", "Database Results", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

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
            AreaCB.ValueMember = "description"
            AreaCB.AutoCompleteMode = AutoCompleteMode.Suggest
            AreaCB.AutoCompleteSource = AutoCompleteSource.ListItems
            AreaCB.DropDownStyle = ComboBoxStyle.DropDown

            ' Create a new DataTable with combined display text
            Dim displayTable As New DataTable()
            displayTable.Columns.Add("description", GetType(String))
            displayTable.Columns.Add("DisplayText", GetType(String))

            ' Add areas to ComboBox with combined display format: description - shortname
            For Each row As DataRow In areasTable.Rows
                Dim newRow As DataRow = displayTable.NewRow()
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
        AddHandler CountryCB.KeyUp, AddressOf CountryCB_KeyUp
        AddHandler CountryCB.TextChanged, AddressOf CountryCB_TextChanged
    End Sub

    Private areaSearchSetup As Boolean = False

    Private Sub SetupAreaSearch()
        ' Enable advanced search functionality for AreaCB only once
        If Not areaSearchSetup Then
            AddHandler AreaCB.KeyUp, AddressOf AreaCB_KeyUp
            AddHandler AreaCB.TextChanged, AddressOf AreaCB_TextChanged
            AddHandler AreaCB.DropDown, AddressOf AreaCB_DropDown
            AddHandler AreaCB.SelectedIndexChanged, AddressOf AreaCB_SelectedIndexChanged
            AddHandler AreaCB.Click, AddressOf AreaCB_Click
            AddHandler AreaCB.Enter, AddressOf AreaCB_Enter
            AddHandler AreaCB.GotFocus, AddressOf AreaCB_GotFocus
            areaSearchSetup = True
        End If
    End Sub

    Private Sub SetupMarketSearch()
        ' Enable advanced search functionality for MarketCB
        AddHandler MarketCB.KeyUp, AddressOf MarketCB_KeyUp
        AddHandler MarketCB.TextChanged, AddressOf MarketCB_TextChanged
    End Sub

    Private Sub SetupCategorySearch()
        ' Enable advanced search functionality for CategoryCB
        AddHandler CatogeryCB.KeyUp, AddressOf CategoryCB_KeyUp
        AddHandler CatogeryCB.TextChanged, AddressOf CategoryCB_TextChanged
    End Sub

    Private Sub SetupGroupsSearch()
        ' Enable advanced search functionality for GroupsCB
        AddHandler GroupsCB.KeyUp, AddressOf GroupsCB_KeyUp
        AddHandler GroupsCB.TextChanged, AddressOf GroupsCB_TextChanged
    End Sub

    Private Sub SetupTypeSearch()
        ' Enable advanced search functionality for TypeCB
        AddHandler TypeCB.KeyUp, AddressOf TypeCB_KeyUp
        AddHandler TypeCB.TextChanged, AddressOf TypeCB_TextChanged
    End Sub

    Private Sub CountryCB_KeyUp(sender As Object, e As KeyEventArgs)
        ' Handle key navigation in dropdown
        If e.KeyCode = Keys.Enter Then
            CountryCB.DroppedDown = False
        ElseIf e.KeyCode = Keys.Down OrElse e.KeyCode = Keys.Up Then
            CountryCB.DroppedDown = True
        End If
    End Sub

    Private isUpdatingCountryData As Boolean = False

    Private Sub CountryCB_TextChanged(sender As Object, e As EventArgs)
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

    Private isUpdatingAreaData As Boolean = False

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
    Private isUpdatingMarketData As Boolean = False

    Private Sub MarketCB_KeyUp(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            MarketCB.DroppedDown = False
        ElseIf e.KeyCode = Keys.Down OrElse e.KeyCode = Keys.Up Then
            MarketCB.DroppedDown = True
        End If
    End Sub

    Private Sub MarketCB_TextChanged(sender As Object, e As EventArgs)
        If isUpdatingMarketData Then Return

        Try
            Dim searchText As String = MarketCB.Text.ToLower().Trim()
            Dim currentCursorPosition As Integer = MarketCB.SelectionStart

            Dim originalData As DataTable = CType(MarketCB.Tag, DataTable)
            If originalData Is Nothing Then Return

            If String.IsNullOrEmpty(searchText) Then
                isUpdatingMarketData = True
                MarketCB.DataSource = originalData
                MarketCB.Text = ""
                isUpdatingMarketData = False
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

            Dim reorderedTable As DataTable = originalData.Clone()
            For Each row In matchingRows
                reorderedTable.ImportRow(row)
            Next
            For Each row In nonMatchingRows
                reorderedTable.ImportRow(row)
            Next

            isUpdatingMarketData = True
            Dim userText As String = MarketCB.Text
            MarketCB.DataSource = reorderedTable
            MarketCB.Text = userText
            MarketCB.SelectionStart = currentCursorPosition
            MarketCB.SelectionLength = 0
            If MarketCB.Focused AndAlso Not String.IsNullOrEmpty(searchText) Then
                MarketCB.DroppedDown = True
            End If
            isUpdatingMarketData = False

        Catch ex As Exception
            isUpdatingMarketData = False
        End Try
    End Sub

    ' =====================CategoryCB Search Event Handlers========================
    Private isUpdatingCategoryData As Boolean = False

    Private Sub CategoryCB_KeyUp(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            CatogeryCB.DroppedDown = False
        ElseIf e.KeyCode = Keys.Down OrElse e.KeyCode = Keys.Up Then
            CatogeryCB.DroppedDown = True
        End If
    End Sub

    Private Sub CategoryCB_TextChanged(sender As Object, e As EventArgs)
        If isUpdatingCategoryData Then Return

        Try
            Dim searchText As String = CatogeryCB.Text.ToLower().Trim()
            Dim currentCursorPosition As Integer = CatogeryCB.SelectionStart

            Dim originalData As DataTable = CType(CatogeryCB.Tag, DataTable)
            If originalData Is Nothing Then Return

            If String.IsNullOrEmpty(searchText) Then
                isUpdatingCategoryData = True
                CatogeryCB.DataSource = originalData
                CatogeryCB.Text = ""
                isUpdatingCategoryData = False
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

            Dim reorderedTable As DataTable = originalData.Clone()
            For Each row In matchingRows
                reorderedTable.ImportRow(row)
            Next
            For Each row In nonMatchingRows
                reorderedTable.ImportRow(row)
            Next

            isUpdatingCategoryData = True
            Dim userText As String = CatogeryCB.Text
            CatogeryCB.DataSource = reorderedTable
            CatogeryCB.Text = userText
            CatogeryCB.SelectionStart = currentCursorPosition
            CatogeryCB.SelectionLength = 0
            If CatogeryCB.Focused AndAlso Not String.IsNullOrEmpty(searchText) Then
                CatogeryCB.DroppedDown = True
            End If
            isUpdatingCategoryData = False

        Catch ex As Exception
            isUpdatingCategoryData = False
        End Try
    End Sub

    ' =====================GroupsCB Search Event Handlers========================
    Private isUpdatingGroupsData As Boolean = False

    Private Sub GroupsCB_KeyUp(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            GroupsCB.DroppedDown = False
        ElseIf e.KeyCode = Keys.Down OrElse e.KeyCode = Keys.Up Then
            GroupsCB.DroppedDown = True
        End If
    End Sub

    Private Sub GroupsCB_TextChanged(sender As Object, e As EventArgs)
        If isUpdatingGroupsData Then Return

        Try
            Dim searchText As String = GroupsCB.Text.ToLower().Trim()
            Dim currentCursorPosition As Integer = GroupsCB.SelectionStart

            Dim originalData As DataTable = CType(GroupsCB.Tag, DataTable)
            If originalData Is Nothing Then Return

            If String.IsNullOrEmpty(searchText) Then
                isUpdatingGroupsData = True
                GroupsCB.DataSource = originalData
                GroupsCB.Text = ""
                isUpdatingGroupsData = False
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

            Dim reorderedTable As DataTable = originalData.Clone()
            For Each row In matchingRows
                reorderedTable.ImportRow(row)
            Next
            For Each row In nonMatchingRows
                reorderedTable.ImportRow(row)
            Next

            isUpdatingGroupsData = True
            Dim userText As String = GroupsCB.Text
            GroupsCB.DataSource = reorderedTable
            GroupsCB.Text = userText
            GroupsCB.SelectionStart = currentCursorPosition
            GroupsCB.SelectionLength = 0
            If GroupsCB.Focused AndAlso Not String.IsNullOrEmpty(searchText) Then
                GroupsCB.DroppedDown = True
            End If
            isUpdatingGroupsData = False

        Catch ex As Exception
            isUpdatingGroupsData = False
        End Try
    End Sub

    ' =====================TypeCB Search Event Handlers========================
    Private isUpdatingTypeData As Boolean = False

    Private Sub TypeCB_KeyUp(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            TypeCB.DroppedDown = False
        ElseIf e.KeyCode = Keys.Down OrElse e.KeyCode = Keys.Up Then
            TypeCB.DroppedDown = True
        End If
    End Sub

    Private Sub TypeCB_TextChanged(sender As Object, e As EventArgs)
        If isUpdatingTypeData Then Return

        Try
            Dim searchText As String = TypeCB.Text.ToLower().Trim()
            Dim currentCursorPosition As Integer = TypeCB.SelectionStart

            Dim originalData As DataTable = CType(TypeCB.Tag, DataTable)
            If originalData Is Nothing Then Return

            If String.IsNullOrEmpty(searchText) Then
                isUpdatingTypeData = True
                TypeCB.DataSource = originalData
                TypeCB.Text = ""
                isUpdatingTypeData = False
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

            Dim reorderedTable As DataTable = originalData.Clone()
            For Each row In matchingRows
                reorderedTable.ImportRow(row)
            Next
            For Each row In nonMatchingRows
                reorderedTable.ImportRow(row)
            Next

            isUpdatingTypeData = True
            Dim userText As String = TypeCB.Text
            TypeCB.DataSource = reorderedTable
            TypeCB.Text = userText
            TypeCB.SelectionStart = currentCursorPosition
            TypeCB.SelectionLength = 0
            If TypeCB.Focused AndAlso Not String.IsNullOrEmpty(searchText) Then
                TypeCB.DroppedDown = True
            End If
            isUpdatingTypeData = False

        Catch ex As Exception
            isUpdatingTypeData = False
        End Try
    End Sub

    Private isLoadingAreas As Boolean = False

    Private Sub CountryCB_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CountryCB.SelectedIndexChanged
        ' Prevent execution during area loading or country data updates
        If isLoadingAreas OrElse isUpdatingCountryData Then Return

        Try
            If CountryCB.SelectedItem IsNot Nothing Then
                Dim selectedCountry = CType(CountryCB.SelectedItem, DataRowView)
                Dim countryCode = selectedCountry("countrycode").ToString
                Dim displayText = selectedCountry("DisplayText").ToString


                ' Auto-populate AreaCB with areas/cities for any selected country
                isLoadingAreas = True
                LoadAreas(countryCode)
                isLoadingAreas = False
            Else
                MessageBox.Show("CountryCB.SelectedItem is Nothing", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            isLoadingAreas = False
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

    Private Sub LoadMarketData()
        Try
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

    Private Sub MarketCB_SelectedIndexChanged(sender As Object, e As EventArgs) Handles MarketCB.SelectedIndexChanged

    End Sub

    Private Sub CatogeryCB_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CatogeryCB.SelectedIndexChanged

    End Sub

    Private Sub GroupsCB_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GroupsCB.SelectedIndexChanged

    End Sub

    Private Sub TypeCB_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TypeCB.SelectedIndexChanged

    End Sub
End Class


