Public Class AttachmentsManagement

    Private dbConn As New DBconnections()
    Private isUpdatingAttachment As Boolean = False
    Private selectedAttachmentId As Integer = 0

    Private Sub AttachmentsManagement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        tableHeaders()
        LoadAttachmentData()
    End Sub

    Private Sub tableHeaders()
        AttachmetTableDGV.Columns.Clear()
        AttachmetTableDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect

        AttachmetTableDGV.MultiSelect = False
        AttachmetTableDGV.ReadOnly = True
        AttachmetTableDGV.AllowUserToAddRows = False
        AttachmetTableDGV.AllowUserToDeleteRows = False
        AttachmetTableDGV.RowHeadersVisible = False
        ' AttachmetTableDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        AttachmetTableDGV.Columns.Add("AttachmentID", "الرقم")
        AttachmetTableDGV.Columns.Add("AttachmentName", "اسم المرفق")
        AttachmetTableDGV.Columns("AttachmentName").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill


        ' Disable auto-selection
        AttachmetTableDGV.ClearSelection()
    End Sub

    Private Sub LoadAttachmentData()
        Dim dt As DataTable = dbConn.GetAttachmnts()

        AttachmetTableDGV.Rows.Clear()

        For Each row As DataRow In dt.Rows
            AttachmetTableDGV.Rows.Add(row("Id"), row("Name"))
        Next
    End Sub

    Private Sub AddAttachmentBT_Click(sender As Object, e As EventArgs) Handles AddAttachmentBT.Click
        ' التحقق من عدم وجود نص فارغ أو مسافات فقط
        If String.IsNullOrWhiteSpace(AttachmentTB.Text) Then
            MessageBox.Show("يرجى إدخال اسم المرفق", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            AttachmentTB.Focus()
            Return
        End If

        ' التحقق من طول النص
        If AttachmentTB.Text.Trim().Length < 2 Then
            MessageBox.Show("يجب أن يكون اسم المرفق أكثر من حرف واحد", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            AttachmentTB.Focus()
            Return
        End If

        ' التحقق من طول النص الأقصى
        If AttachmentTB.Text.Trim().Length > 100 Then
            MessageBox.Show("اسم المرفق طويل جداً. الحد الأقصى 100 حرف", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            AttachmentTB.Focus()
            Return
        End If

        Try
            If isUpdatingAttachment Then
                ' تحديث المرفق الموجود
                Dim updateSuccess As Boolean = dbConn.UpdateAttachment(selectedAttachmentId, AttachmentTB.Text.Trim())
                If updateSuccess Then
                    MessageBox.Show("تم تحديث المرفق بنجاح", "نجح التحديث", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("فشل في تحديث المرفق. يرجى المحاولة مرة أخرى.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
                ' إعادة تعيين وضع التحديث
                isUpdatingAttachment = False
                selectedAttachmentId = 0
            Else
                ' إضافة مرفق جديد
                Dim newAttachmentId As Integer = dbConn.AddAttachment(AttachmentTB.Text.Trim())

                If newAttachmentId > 0 Then
                    ' إنشاء إدخالات وثائق للمرفق الجديد لجميع العملاء
                    Dim documentsCreated As Boolean = dbConn.AddAttachmentDocumentsForAllCustomers(newAttachmentId, AttachmentTB.Text.Trim())

                    If documentsCreated Then
                        MessageBox.Show("تم إضافة المرفق بنجاح وتم إنشاء إدخالات الوثائق لجميع العملاء", "نجحت الإضافة", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        MessageBox.Show("تم إضافة المرفق ولكن فشل في إنشاء إدخالات الوثائق", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If
                Else
                    MessageBox.Show("فشل في إضافة المرفق", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If

            AttachmentTB.Clear()
            LoadAttachmentData()
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء العملية: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub RemoveAttachmentBT_Click(sender As Object, e As EventArgs) Handles RemoveAttachmentBT.Click
        ' التحقق من تحديد صف في الجدول
        If AttachmetTableDGV.CurrentRow Is Nothing Then
            MessageBox.Show("يرجى تحديد مرفق لحذفه", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' الحصول على معرف المرفق المحدد
        Dim selectedRow As DataGridViewRow = AttachmetTableDGV.CurrentRow
        Dim attachmentId As Integer = Convert.ToInt32(selectedRow.Cells("AttachmentID").Value)
        Dim attachmentName As String = selectedRow.Cells("AttachmentName").Value.ToString()

        ' التأكيد قبل الحذف
        Dim result As DialogResult = MessageBox.Show(
            "هل أنت متأكد من حذف المرفق: " & attachmentName & "؟" & vbCrLf & "لا يمكن التراجع عن هذا الإجراء.",
            "تأكيد الحذف",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question
        )

        If result = DialogResult.Yes Then
            ' حذف المرفق من قاعدة البيانات
            Dim deleteSuccess As Boolean = dbConn.RemoveAttachment(attachmentId)

            If deleteSuccess Then
                MessageBox.Show("تم حذف المرفق بنجاح", "نجح الحذف", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ' تحديث الجدول بعد الحذف
                LoadAttachmentData()
            Else
                MessageBox.Show("فشل في حذف المرفق. يرجى المحاولة مرة أخرى.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub

    Private Sub AttachmetTableDGV_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles AttachmetTableDGV.CellClick
        ' التأكد من أن المستخدم نقر على خلية صالحة وليس على الرأس
        If e.RowIndex >= 0 Then
            Dim selectedRow As DataGridViewRow = AttachmetTableDGV.Rows(e.RowIndex)
            Dim attachmentId As Integer = Convert.ToInt32(selectedRow.Cells("AttachmentID").Value)
            Dim currentName As String = selectedRow.Cells("AttachmentName").Value.ToString()

            ' وضع البيانات في حقل النص
            AttachmentTB.Text = currentName

            ' تفعيل وضع التحديث
            isUpdatingAttachment = True
            selectedAttachmentId = attachmentId


            ' إعطاء التركيز لحقل النص
            AttachmentTB.Focus()
            AttachmentTB.SelectAll()
        End If
    End Sub

    Private Sub ResetAttachmentUpdateMode()
        isUpdatingAttachment = False
        selectedAttachmentId = 0
    End Sub

    Private Sub AttachmentTB_TextChanged(sender As Object, e As EventArgs) Handles AttachmentTB.TextChanged
        ' إذا كان المستخدم يكتب في حقل فارغ، اجعل الزر للإضافة
        If String.IsNullOrEmpty(AttachmentTB.Text) And isUpdatingAttachment Then
            ResetAttachmentUpdateMode()
        End If
    End Sub

    Private Sub AttachmetTableDGV_Leave(sender As Object, e As EventArgs) Handles AttachmetTableDGV.Leave
        ' لا نفعل شيء هنا، نترك الوضع كما هو
    End Sub

    ' Clear selection when clicking outside the DataGridView
    Private Sub AttachmentsManagement_Click(sender As Object, e As EventArgs) Handles Me.Click
        ClearAttachmentSelection()
    End Sub

    ' Clear attachment selection helper method
    Private Sub ClearAttachmentSelection()
        If AttachmetTableDGV.CurrentRow IsNot Nothing Then
            AttachmetTableDGV.ClearSelection()
            ResetAttachmentUpdateMode()
        End If
    End Sub

    ' Clear selection when clicking on other controls (except those that need selection)
    Private Sub ClearSelectionOnControlClick(sender As Object, e As EventArgs) Handles AttachmentTB.Click
        ' Only clear on text box click if it's empty
        If sender Is AttachmentTB AndAlso String.IsNullOrEmpty(AttachmentTB.Text) Then
            ClearAttachmentSelection()
        End If
    End Sub

End Class
