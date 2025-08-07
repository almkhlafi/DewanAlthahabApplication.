Imports System.Data
Imports Microsoft.Data.SqlClient

Public Class DBconnections

    Dim connpath As String = "Data Source=ABDULRAHMAN;Initial Catalog=EmployeesDB;Integrated Security=True;TrustServerCertificate=True"
    Dim connpath2 As String = "Data Source=192.168.15.56;Initial Catalog=CMGADB2024;Persist Security Info=True;User ID=AR;Pooling=False;Multiple Active Result Sets=False;Encrypt=True;Trust Server Certificate=True;Application Name=""SQL Server Management Studio"";Command Timeout=30;Password=123456"

    ' Helper method to truncate strings to prevent database field length errors
    Private Function TruncateString(value As String, maxLength As Integer) As Object
        If String.IsNullOrEmpty(value) Then
            Return DBNull.Value
        End If

        If value.Length <= maxLength Then
            Return value
        Else
            ' Log which field is being truncated for debugging
            System.Diagnostics.Debug.WriteLine($"Truncating field from {value.Length} to {maxLength} characters: {value.Substring(0, Math.Min(50, value.Length))}...")
            Return value.Substring(0, maxLength)
        End If
    End Function

    '===============Reuse the connection=============
    ' Reusable method to get a new connection (default - first connection)
    Public Function GetConnection() As SqlConnection
        Return New SqlConnection(connpath)
    End Function

    ' Reusable method to get a connection using the second connection string
    Public Function GetConnection2() As SqlConnection
        Return New SqlConnection(connpath2)
    End Function

    ' Reusable method to get a connection by choice (1 or 2)
    Public Function GetConnection(connectionNumber As Integer) As SqlConnection
        If connectionNumber = 2 Then
            Return New SqlConnection(connpath2)
        Else
            Return New SqlConnection(connpath)
        End If
    End Function




    ' =====================List Customers========================
    Public Function GetCustomers() As DataTable
        Dim dt As New DataTable()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim da As SqlDataAdapter = Nothing

        Try
            conn = New SqlConnection(connpath)
            conn.Open()

            Dim query As String = "SELECT Id, Name FROM Customers"
            cmd = New SqlCommand(query, conn)
            da = New SqlDataAdapter(cmd)
            da.Fill(dt)

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل العملاء: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If da IsNot Nothing Then da.Dispose()
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Dispose()
            End If
        End Try

        Return dt
    End Function


    '=====================Add a Customer========================
    Public Function AddCustomer(name As String) As Integer
        Dim localConn As SqlConnection = Nothing
        Dim localCmd As SqlCommand = Nothing
        Dim newCustomerId As Integer = 0

        Try
            localConn = New SqlConnection(connpath)
            localConn.Open()

            Dim query As String = "INSERT INTO Customers (Name) VALUES (@Name); SELECT SCOPE_IDENTITY();"
            localCmd = New SqlCommand(query, localConn)
            localCmd.Parameters.AddWithValue("@Name", name)

            Dim result = localCmd.ExecuteScalar()
            If result IsNot Nothing Then
                newCustomerId = Convert.ToInt32(result)
            End If

        Catch ex As Exception
            MessageBox.Show("خطأ في إضافة العميل: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If localCmd IsNot Nothing Then localCmd.Dispose()
            If localConn IsNot Nothing AndAlso localConn.State = ConnectionState.Open Then
                localConn.Close()
                localConn.Dispose()
            End If
        End Try

        Return newCustomerId
    End Function

    '=====================Remove a Customer========================
    Public Function RemoveCustomer(id As Integer) As Boolean
        Dim localConn As SqlConnection = Nothing
        Dim localCmd As SqlCommand = Nothing
        Dim transaction As SqlTransaction = Nothing
        Dim success As Boolean = False

        Try
            localConn = New SqlConnection(connpath)
            localConn.Open()
            transaction = localConn.BeginTransaction()

            ' Delete related records in correct order to avoid foreign key conflicts

            ' 1. Delete Delegators_Permissions for delegators belonging to this customer
            Dim deletePermissionsQuery As String = "DELETE FROM Delegators_Permissions WHERE DelegatorId IN (SELECT Id FROM Delegators WHERE CustomerId = @CustomerId)"
            localCmd = New SqlCommand(deletePermissionsQuery, localConn, transaction)
            localCmd.Parameters.AddWithValue("@CustomerId", id)
            localCmd.ExecuteNonQuery()
            localCmd.Dispose()

            ' 2. Delete Delegators for this customer
            Dim deleteDelegatorsQuery As String = "DELETE FROM Delegators WHERE CustomerId = @CustomerId"
            localCmd = New SqlCommand(deleteDelegatorsQuery, localConn, transaction)
            localCmd.Parameters.AddWithValue("@CustomerId", id)
            localCmd.ExecuteNonQuery()
            localCmd.Dispose()

            ' 3. Delete Documents for this customer
            Dim deleteDocumentsQuery As String = "DELETE FROM Customers_Attachmnets_Documnets WHERE Customer_ID = @CustomerId"
            localCmd = New SqlCommand(deleteDocumentsQuery, localConn, transaction)
            localCmd.Parameters.AddWithValue("@CustomerId", id)
            localCmd.ExecuteNonQuery()
            localCmd.Dispose()

            ' 4. Finally delete the customer
            Dim deleteCustomerQuery As String = "DELETE FROM Customers WHERE Id = @CustomerId"
            localCmd = New SqlCommand(deleteCustomerQuery, localConn, transaction)
            localCmd.Parameters.AddWithValue("@CustomerId", id)
            Dim rowsAffected As Integer = localCmd.ExecuteNonQuery()

            ' Commit transaction if customer was deleted
            If rowsAffected > 0 Then
                transaction.Commit()
                success = True
            Else
                transaction.Rollback()
                success = False
            End If

        Catch ex As Exception
            If transaction IsNot Nothing Then
                transaction.Rollback()
            End If
            MessageBox.Show("خطأ في حذف العميل: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
            success = False
        Finally
            If localCmd IsNot Nothing Then localCmd.Dispose()
            If transaction IsNot Nothing Then transaction.Dispose()
            If localConn IsNot Nothing AndAlso localConn.State = ConnectionState.Open Then
                localConn.Close()
                localConn.Dispose()
            End If
        End Try

        Return success
    End Function

    ' =====================List ِAttachments========================
    Public Function GetAttachmnts() As DataTable
        Dim dt As New DataTable()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim da As SqlDataAdapter = Nothing

        Try
            conn = New SqlConnection(connpath)
            conn.Open()

            Dim query As String = "SELECT Id, Name FROM Attachments"
            cmd = New SqlCommand(query, conn)
            da = New SqlDataAdapter(cmd)
            da.Fill(dt)

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل المرفقات: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If da IsNot Nothing Then da.Dispose()
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Dispose()
            End If
        End Try

        Return dt
    End Function

    '=====================Add an Attachment========================

    Public Function AddAttachment(name As String) As Integer
        Dim localConn As SqlConnection = Nothing
        Dim localCmd As SqlCommand = Nothing
        Dim newAttachmentId As Integer = 0

        Try
            localConn = New SqlConnection(connpath)
            localConn.Open()

            Dim query As String = "INSERT INTO Attachments (Name) VALUES (@Name); SELECT SCOPE_IDENTITY();"
            localCmd = New SqlCommand(query, localConn)
            localCmd.Parameters.AddWithValue("@Name", name)

            Dim result = localCmd.ExecuteScalar()
            If result IsNot Nothing Then
                newAttachmentId = Convert.ToInt32(result)
            End If

        Catch ex As Exception
            MessageBox.Show("خطأ في إضافة المرفق: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If localCmd IsNot Nothing Then localCmd.Dispose()
            If localConn IsNot Nothing AndAlso localConn.State = ConnectionState.Open Then
                localConn.Close()
                localConn.Dispose()
            End If
        End Try

        Return newAttachmentId
    End Function

    '=====================Remove an Attachment========================
    Public Function RemoveAttachment(id As Integer) As Boolean
        Dim localConn As SqlConnection = Nothing
        Dim localCmd As SqlCommand = Nothing
        Dim transaction As SqlTransaction = Nothing
        Dim success As Boolean = False

        Try
            localConn = New SqlConnection(connpath)
            localConn.Open()
            transaction = localConn.BeginTransaction()

            ' حذف جميع الوثائق المرتبطة بهذا المرفق أولاً
            Dim deleteDocsQuery As String = "DELETE FROM Customers_Attachmnets_Documnets WHERE Attachment_ID = @AttachmentId"
            localCmd = New SqlCommand(deleteDocsQuery, localConn, transaction)
            localCmd.Parameters.AddWithValue("@AttachmentId", id)
            localCmd.ExecuteNonQuery()
            localCmd.Dispose()

            ' ثم حذف المرفق نفسه
            Dim deleteAttachmentQuery As String = "DELETE FROM Attachments WHERE Id = @Id"
            localCmd = New SqlCommand(deleteAttachmentQuery, localConn, transaction)
            localCmd.Parameters.AddWithValue("@Id", id)
            Dim rowsAffected As Integer = localCmd.ExecuteNonQuery()

            If rowsAffected > 0 Then
                transaction.Commit()
                success = True
            Else
                transaction.Rollback()
                success = False
            End If

        Catch ex As Exception
            If transaction IsNot Nothing Then
                transaction.Rollback()
            End If
            MessageBox.Show("خطأ في حذف المرفق: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
            success = False
        Finally
            If localCmd IsNot Nothing Then localCmd.Dispose()
            If transaction IsNot Nothing Then transaction.Dispose()
            If localConn IsNot Nothing AndAlso localConn.State = ConnectionState.Open Then
                localConn.Close()
                localConn.Dispose()
            End If
        End Try

        Return success
    End Function

    '=====================Update a Customer========================
    Public Function UpdateCustomer(id As Integer, name As String) As Boolean
        Dim localConn As SqlConnection = Nothing
        Dim localCmd As SqlCommand = Nothing
        Dim success As Boolean = False

        Try
            localConn = New SqlConnection(connpath)
            localConn.Open()

            Dim query As String = "UPDATE Customers SET Name = @Name WHERE Id = @Id"
            localCmd = New SqlCommand(query, localConn)
            localCmd.Parameters.AddWithValue("@Name", name)
            localCmd.Parameters.AddWithValue("@Id", id)

            Dim rowsAffected As Integer = localCmd.ExecuteNonQuery()
            success = (rowsAffected > 0)

        Catch ex As Exception
            MessageBox.Show("خطأ في تحديث العميل: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
            success = False
        Finally
            If localCmd IsNot Nothing Then localCmd.Dispose()
            If localConn IsNot Nothing AndAlso localConn.State = ConnectionState.Open Then
                localConn.Close()
                localConn.Dispose()
            End If
        End Try

        Return success
    End Function

    '=====================Update an Attachment========================
    Public Function UpdateAttachment(id As Integer, name As String) As Boolean
        Dim localConn As SqlConnection = Nothing
        Dim localCmd As SqlCommand = Nothing
        Dim success As Boolean = False

        Try
            localConn = New SqlConnection(connpath)
            localConn.Open()

            Dim query As String = "UPDATE Attachments SET Name = @Name WHERE Id = @Id"
            localCmd = New SqlCommand(query, localConn)
            localCmd.Parameters.AddWithValue("@Name", name)
            localCmd.Parameters.AddWithValue("@Id", id)

            Dim rowsAffected As Integer = localCmd.ExecuteNonQuery()
            success = (rowsAffected > 0)

        Catch ex As Exception
            MessageBox.Show("خطأ في تحديث المرفق: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
            success = False
        Finally
            If localCmd IsNot Nothing Then localCmd.Dispose()
            If localConn IsNot Nothing AndAlso localConn.State = ConnectionState.Open Then
                localConn.Close()
                localConn.Dispose()
            End If
        End Try

        Return success
    End Function

    '=====================Get Documents with Attachment Names========================
    Public Function GetDocuments() As DataTable
        Dim dt As New DataTable()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim da As SqlDataAdapter = Nothing

        Try
            conn = New SqlConnection(connpath)
            conn.Open()

            Dim query As String = "SELECT d.Id, a.Name AS AttachmentName, d.FilePath, d.UploadingDate, d.ExpireDate, d.fileAttached, d.Attachment_ID " &
                                "FROM Customers_Attachmnets_Documnets d " &
                                "INNER JOIN Attachments a ON d.Attachment_ID = a.Id " &
                                "ORDER BY d.UploadingDate DESC"
            cmd = New SqlCommand(query, conn)
            da = New SqlDataAdapter(cmd)
            da.Fill(dt)

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل الوثائق: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If da IsNot Nothing Then da.Dispose()
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Dispose()
            End If
        End Try

        Return dt
    End Function

    ' Get documents for a specific customer
    Public Function GetDocumentsByCustomer(customerId As Integer) As DataTable
        Dim dt As New DataTable()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim da As SqlDataAdapter = Nothing

        Try
            conn = New SqlConnection(connpath)
            conn.Open()

            Dim query As String = "SELECT d.Id, a.Name AS AttachmentName, d.FilePath, d.UploadingDate, d.ExpireDate, d.fileAttached, d.Attachment_ID " &
                                "FROM Customers_Attachmnets_Documnets d " &
                                "INNER JOIN Attachments a ON d.Attachment_ID = a.Id " &
                                "WHERE d.Customer_ID = @CustomerId " &
                                "ORDER BY d.UploadingDate DESC"
            cmd = New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@CustomerId", customerId)
            da = New SqlDataAdapter(cmd)
            da.Fill(dt)

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل وثائق العميل: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If da IsNot Nothing Then da.Dispose()
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Dispose()
            End If
        End Try

        Return dt
    End Function

    ' Get customer name by ID
    Public Function GetCustomerName(customerId As Integer) As String
        Dim customerName As String = ""
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing

        Try
            conn = New SqlConnection(connpath)
            conn.Open()

            Dim query As String = "SELECT Name FROM Customers WHERE Id = @CustomerId"
            cmd = New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@CustomerId", customerId)

            Dim result = cmd.ExecuteScalar()
            If result IsNot Nothing Then
                customerName = result.ToString()
            End If

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل اسم العميل: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Dispose()
            End If
        End Try

        Return customerName
    End Function

    ' Get documents that are about to expire (within 30 days)
    Public Function GetAboutToExpireDocuments() As DataTable
        Dim dt As New DataTable()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim da As SqlDataAdapter = Nothing

        Try
            conn = New SqlConnection(connpath)
            conn.Open()

            Dim query As String = "SELECT d.Id, a.Name AS AttachmentName, d.FilePath, d.UploadingDate, d.ExpireDate, d.fileAttached, d.Attachment_ID " &
                                "FROM Customers_Attachmnets_Documnets d " &
                                "INNER JOIN Attachments a ON d.Attachment_ID = a.Id " &
                                "WHERE d.ExpireDate > GETDATE() AND d.ExpireDate <= DATEADD(DAY, 30, GETDATE()) " &
                                "ORDER BY d.ExpireDate ASC"
            cmd = New SqlCommand(query, conn)
            da = New SqlDataAdapter(cmd)
            da.Fill(dt)

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل الوثائق المنتهية الصلاحية قريباً: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If da IsNot Nothing Then da.Dispose()
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Dispose()
            End If
        End Try

        Return dt
    End Function

    ' Get documents that have already expired
    Public Function GetExpiredDocuments() As DataTable
        Dim dt As New DataTable()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim da As SqlDataAdapter = Nothing

        Try
            conn = New SqlConnection(connpath)
            conn.Open()

            Dim query As String = "SELECT d.Id, a.Name AS AttachmentName, d.FilePath, d.UploadingDate, d.ExpireDate, d.fileAttached, d.Attachment_ID " &
                                "FROM Customers_Attachmnets_Documnets d " &
                                "INNER JOIN Attachments a ON d.Attachment_ID = a.Id " &
                                "WHERE d.ExpireDate < GETDATE() " &
                                "ORDER BY d.ExpireDate DESC"
            cmd = New SqlCommand(query, conn)
            da = New SqlDataAdapter(cmd)
            da.Fill(dt)

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل الوثائق المنتهية الصلاحية: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If da IsNot Nothing Then da.Dispose()
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Dispose()
            End If
        End Try

        Return dt
    End Function

    ' Get documents that are about to expire for a specific customer (within 30 days)
    Public Function GetAboutToExpireDocumentsByCustomer(customerId As Integer) As DataTable
        Dim dt As New DataTable()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim da As SqlDataAdapter = Nothing

        Try
            conn = New SqlConnection(connpath)
            conn.Open()

            Dim query As String = "SELECT d.Id, a.Name AS AttachmentName, d.FilePath, d.UploadingDate, d.ExpireDate, d.fileAttached, d.Attachment_ID " &
                                "FROM Customers_Attachmnets_Documnets d " &
                                "INNER JOIN Attachments a ON d.Attachment_ID = a.Id " &
                                "WHERE d.Customer_ID = @CustomerId AND d.ExpireDate > GETDATE() AND d.ExpireDate <= DATEADD(DAY, 30, GETDATE()) " &
                                "ORDER BY d.ExpireDate ASC"
            cmd = New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@CustomerId", customerId)
            da = New SqlDataAdapter(cmd)
            da.Fill(dt)

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل الوثائق المنتهية الصلاحية قريباً للعميل: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If da IsNot Nothing Then da.Dispose()
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Dispose()
            End If
        End Try

        Return dt
    End Function

    ' Get expired documents for a specific customer
    Public Function GetExpiredDocumentsByCustomer(customerId As Integer) As DataTable
        Dim dt As New DataTable()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim da As SqlDataAdapter = Nothing

        Try
            conn = New SqlConnection(connpath)
            conn.Open()

            Dim query As String = "SELECT d.Id, a.Name AS AttachmentName, d.FilePath, d.UploadingDate, d.ExpireDate, d.fileAttached, d.Attachment_ID " &
                                "FROM Customers_Attachmnets_Documnets d " &
                                "INNER JOIN Attachments a ON d.Attachment_ID = a.Id " &
                                "WHERE d.Customer_ID = @CustomerId AND d.ExpireDate < GETDATE() " &
                                "ORDER BY d.ExpireDate DESC"
            cmd = New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@CustomerId", customerId)
            da = New SqlDataAdapter(cmd)
            da.Fill(dt)

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل الوثائق المنتهية الصلاحية للعميل: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If da IsNot Nothing Then da.Dispose()
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Dispose()
            End If
        End Try

        Return dt
    End Function

    ' Update document information with optional file content
    Public Function UpdateDocument(id As Integer, filePath As String, uploadingDate As DateTime, expireDate As DateTime, attachmentId As Integer, fileContent As Byte()) As Boolean
        Dim localConn As SqlConnection = Nothing
        Dim localCmd As SqlCommand = Nothing
        Dim success As Boolean = False

        Try
            localConn = New SqlConnection(connpath)
            localConn.Open()

            ' Build query based on whether file content should be updated
            Dim query As String
            If fileContent IsNot Nothing AndAlso fileContent.Length > 0 Then
                ' Update including file content
                query = "UPDATE Customers_Attachmnets_Documnets SET FilePath = @FilePath, UploadingDate = @UploadingDate, ExpireDate = @ExpireDate, Attachment_ID = @AttachmentId, fileAttached = @FileContent WHERE Id = @Id"
            Else
                ' Update without changing file content
                query = "UPDATE Customers_Attachmnets_Documnets SET FilePath = @FilePath, UploadingDate = @UploadingDate, ExpireDate = @ExpireDate, Attachment_ID = @AttachmentId WHERE Id = @Id"
            End If

            localCmd = New SqlCommand(query, localConn)
            localCmd.Parameters.AddWithValue("@FilePath", filePath)
            localCmd.Parameters.AddWithValue("@UploadingDate", uploadingDate)
            localCmd.Parameters.AddWithValue("@ExpireDate", expireDate)
            localCmd.Parameters.AddWithValue("@AttachmentId", attachmentId)
            localCmd.Parameters.AddWithValue("@Id", id)

            ' Add file content parameter only if we're updating it
            If fileContent IsNot Nothing AndAlso fileContent.Length > 0 Then
                localCmd.Parameters.Add("@FileContent", SqlDbType.VarBinary, -1).Value = fileContent
            End If

            Dim rowsAffected As Integer = localCmd.ExecuteNonQuery()
            success = (rowsAffected > 0)

        Catch ex As Exception
            MessageBox.Show("خطأ في تحديث الوثيقة: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
            success = False
        Finally
            If localCmd IsNot Nothing Then localCmd.Dispose()
            If localConn IsNot Nothing AndAlso localConn.State = ConnectionState.Open Then
                localConn.Close()
                localConn.Dispose()
            End If
        End Try

        Return success
    End Function

    ' Add new document with file content
    Public Function AddDocument(filePath As String, uploadingDate As DateTime, expireDate As DateTime, customerId As Integer, attachmentId As Integer, fileContent As Byte()) As Boolean
        Dim localConn As SqlConnection = Nothing
        Dim localCmd As SqlCommand = Nothing
        Dim success As Boolean = False

        Try
            localConn = New SqlConnection(connpath)
            localConn.Open()

            Dim query As String = "INSERT INTO Customers_Attachmnets_Documnets (FilePath, UploadingDate, ExpireDate, Customer_ID, Attachment_ID, fileAttached) VALUES (@FilePath, @UploadingDate, @ExpireDate, @CustomerId, @AttachmentId, @FileContent)"
            localCmd = New SqlCommand(query, localConn)
            localCmd.Parameters.AddWithValue("@FilePath", filePath)
            localCmd.Parameters.AddWithValue("@UploadingDate", uploadingDate)
            localCmd.Parameters.AddWithValue("@ExpireDate", expireDate)
            localCmd.Parameters.AddWithValue("@CustomerId", customerId)
            localCmd.Parameters.AddWithValue("@AttachmentId", attachmentId)

            ' Handle file content
            If fileContent IsNot Nothing AndAlso fileContent.Length > 0 Then
                localCmd.Parameters.Add("@FileContent", SqlDbType.VarBinary, -1).Value = fileContent
            Else
                localCmd.Parameters.Add("@FileContent", SqlDbType.VarBinary, -1).Value = DBNull.Value
            End If

            Dim rowsAffected As Integer = localCmd.ExecuteNonQuery()
            success = (rowsAffected > 0)

        Catch ex As Exception
            MessageBox.Show("خطأ في إضافة الوثيقة: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
            success = False
        Finally
            If localCmd IsNot Nothing Then localCmd.Dispose()
            If localConn IsNot Nothing AndAlso localConn.State = ConnectionState.Open Then
                localConn.Close()
                localConn.Dispose()
            End If
        End Try

        Return success
    End Function

    ' Get document file content by ID
    Public Function GetDocumentFileContent(documentId As Integer) As Byte()
        Dim localConn As SqlConnection = Nothing
        Dim localCmd As SqlCommand = Nothing
        Dim fileContent As Byte() = Nothing

        Try
            localConn = New SqlConnection(connpath)
            localConn.Open()

            Dim query As String = "SELECT fileAttached FROM Customers_Attachmnets_Documnets WHERE Id = @Id"
            localCmd = New SqlCommand(query, localConn)
            localCmd.Parameters.AddWithValue("@Id", documentId)

            Dim result = localCmd.ExecuteScalar()
            If result IsNot Nothing AndAlso result IsNot DBNull.Value Then
                fileContent = CType(result, Byte())
            End If

        Catch ex As Exception
            MessageBox.Show("خطأ في استرجاع محتوى الملف: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If localCmd IsNot Nothing Then localCmd.Dispose()
            If localConn IsNot Nothing AndAlso localConn.State = ConnectionState.Open Then
                localConn.Close()
                localConn.Dispose()
            End If
        End Try

        Return fileContent
    End Function

    ' Add attachment placeholder document entries for all customers
    Public Function AddAttachmentDocumentsForAllCustomers(attachmentId As Integer, attachmentName As String) As Boolean
        Dim localConn As SqlConnection = Nothing
        Dim localCmd As SqlCommand = Nothing
        Dim transaction As SqlTransaction = Nothing
        Dim success As Boolean = False

        Try
            localConn = New SqlConnection(connpath)
            localConn.Open()
            transaction = localConn.BeginTransaction()

            ' Get all customer IDs
            Dim getCustomersQuery As String = "SELECT Id FROM Customers"
            localCmd = New SqlCommand(getCustomersQuery, localConn, transaction)
            Dim customersTable As New DataTable()
            Dim adapter As New SqlDataAdapter(localCmd)
            adapter.Fill(customersTable)
            adapter.Dispose()
            localCmd.Dispose()

            ' Insert document entry for each customer
            Dim insertQuery As String = "INSERT INTO Customers_Attachmnets_Documnets (FilePath, UploadingDate, ExpireDate, Customer_ID, Attachment_ID, fileAttached) VALUES (@FilePath, @UploadingDate, @ExpireDate, @CustomerId, @AttachmentId, @FileContent)"
            localCmd = New SqlCommand(insertQuery, localConn, transaction)

            ' Add parameters with explicit types to prevent data corruption
            localCmd.Parameters.Add("@FilePath", SqlDbType.NVarChar, 500)
            localCmd.Parameters.Add("@UploadingDate", SqlDbType.DateTime2)
            localCmd.Parameters.Add("@ExpireDate", SqlDbType.DateTime2)
            localCmd.Parameters.Add("@CustomerId", SqlDbType.Int)
            localCmd.Parameters.Add("@AttachmentId", SqlDbType.Int)
            localCmd.Parameters.Add("@FileContent", SqlDbType.VarBinary, -1)

            For Each customerRow As DataRow In customersTable.Rows
                Dim customerId As Integer = Convert.ToInt32(customerRow("Id"))

                ' Set parameter values for each iteration
                localCmd.Parameters("@FilePath").Value = $"مرفق جديد: {attachmentName}"
                localCmd.Parameters("@UploadingDate").Value = DateTime.Now
                localCmd.Parameters("@ExpireDate").Value = DateTime.Now.AddYears(1)
                localCmd.Parameters("@CustomerId").Value = customerId
                localCmd.Parameters("@AttachmentId").Value = attachmentId
                localCmd.Parameters("@FileContent").Value = DBNull.Value

                localCmd.ExecuteNonQuery()
            Next

            transaction.Commit()
            success = True

        Catch ex As Exception
            If transaction IsNot Nothing Then
                transaction.Rollback()
            End If
            MessageBox.Show("خطأ في إنشاء إدخالات الوثائق للمرفق الجديد: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
            success = False
        Finally
            If localCmd IsNot Nothing Then localCmd.Dispose()
            If transaction IsNot Nothing Then transaction.Dispose()
            If localConn IsNot Nothing AndAlso localConn.State = ConnectionState.Open Then
                localConn.Close()
                localConn.Dispose()
            End If
        End Try

        Return success
    End Function

    ' Add attachment placeholder documents for a new customer
    Public Function AddAttachmentDocumentsForNewCustomer(customerId As Integer) As Boolean
        Dim localConn As SqlConnection = Nothing
        Dim localCmd As SqlCommand = Nothing
        Dim transaction As SqlTransaction = Nothing
        Dim success As Boolean = False

        Try
            localConn = New SqlConnection(connpath)
            localConn.Open()
            transaction = localConn.BeginTransaction()

            ' Get all attachment IDs and names
            Dim getAttachmentsQuery As String = "SELECT Id, Name FROM Attachments"
            localCmd = New SqlCommand(getAttachmentsQuery, localConn, transaction)
            Dim attachmentsTable As New DataTable()
            Dim adapter As New SqlDataAdapter(localCmd)
            adapter.Fill(attachmentsTable)
            adapter.Dispose()
            localCmd.Dispose()

            ' Insert document entry for each attachment
            Dim insertQuery As String = "INSERT INTO Customers_Attachmnets_Documnets (FilePath, UploadingDate, ExpireDate, Customer_ID, Attachment_ID, fileAttached) VALUES (@FilePath, @UploadingDate, @ExpireDate, @CustomerId, @AttachmentId, @FileContent)"
            localCmd = New SqlCommand(insertQuery, localConn, transaction)

            ' Add parameters with explicit types to prevent data corruption
            localCmd.Parameters.Add("@FilePath", SqlDbType.NVarChar, 500)
            localCmd.Parameters.Add("@UploadingDate", SqlDbType.DateTime2)
            localCmd.Parameters.Add("@ExpireDate", SqlDbType.DateTime2)
            localCmd.Parameters.Add("@CustomerId", SqlDbType.Int)
            localCmd.Parameters.Add("@AttachmentId", SqlDbType.Int)
            localCmd.Parameters.Add("@FileContent", SqlDbType.VarBinary, -1)

            For Each attachmentRow As DataRow In attachmentsTable.Rows
                Dim attachmentId As Integer = Convert.ToInt32(attachmentRow("Id"))
                Dim attachmentName As String = attachmentRow("Name").ToString()

                ' Set parameter values for each iteration
                localCmd.Parameters("@FilePath").Value = $"مرفق: {attachmentName}"
                localCmd.Parameters("@UploadingDate").Value = DateTime.Now
                localCmd.Parameters("@ExpireDate").Value = DateTime.Now.AddYears(1)
                localCmd.Parameters("@CustomerId").Value = customerId
                localCmd.Parameters("@AttachmentId").Value = attachmentId
                localCmd.Parameters("@FileContent").Value = DBNull.Value

                localCmd.ExecuteNonQuery()
            Next

            transaction.Commit()
            success = True

        Catch ex As Exception
            If transaction IsNot Nothing Then
                transaction.Rollback()
            End If
            MessageBox.Show("خطأ في إنشاء إدخالات الوثائق للعميل الجديد: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
            success = False
        Finally
            If localCmd IsNot Nothing Then localCmd.Dispose()
            If transaction IsNot Nothing Then transaction.Dispose()
            If localConn IsNot Nothing AndAlso localConn.State = ConnectionState.Open Then
                localConn.Close()
                localConn.Dispose()
            End If
        End Try

        Return success
    End Function

    ' Update document with scanned content only
    Public Function UpdateDocumentWithScannedContent(documentId As Integer, scannedContent As Byte()) As Boolean
        Dim localConn As SqlConnection = Nothing
        Dim localCmd As SqlCommand = Nothing
        Dim success As Boolean = False

        Try
            localConn = New SqlConnection(connpath)
            localConn.Open()

            ' Update only the file content and upload date
            Dim query As String = "UPDATE Customers_Attachmnets_Documnets SET fileAttached = @FileContent, UploadingDate = @UploadingDate WHERE Id = @Id"
            localCmd = New SqlCommand(query, localConn)
            localCmd.Parameters.AddWithValue("@Id", documentId)
            localCmd.Parameters.AddWithValue("@UploadingDate", DateTime.Now)
            localCmd.Parameters.Add("@FileContent", SqlDbType.VarBinary, -1).Value = scannedContent

            Dim rowsAffected As Integer = localCmd.ExecuteNonQuery()
            success = (rowsAffected > 0)

        Catch ex As Exception
            MessageBox.Show("خطأ في حفظ الوثيقة الممسوحة: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
            success = False
        Finally
            If localCmd IsNot Nothing Then localCmd.Dispose()
            If localConn IsNot Nothing AndAlso localConn.State = ConnectionState.Open Then
                localConn.Close()
                localConn.Dispose()
            End If
        End Try

        Return success
    End Function

    ' =====================Get Delegators by Customer========================
    Public Function GetDelegatorsByCustomer(customerId As Integer) As DataTable
        Dim dt As New DataTable()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim da As SqlDataAdapter = Nothing

        Try
            conn = New SqlConnection(connpath)
            conn.Open()

            ' First, ensure the required tables exist
            EnsureDelegatorsTableExists(conn)
            EnsureDelegatorsPermissionsTableExists(conn)

            ' Simple query to get delegators for a specific customer
            Dim query As String = "
SELECT 
    Id, 
    Name, 
    CustomerId, 
    ISNULL([Identity], '') AS [Identity], 
    ISNULL(Nationality, '') AS Nationality, 
    ISNULL(PhoneNumber, '') AS PhoneNumber, 
    ISNULL([Type], '') AS [Type], 
    ISNULL(dateOfDelegation, GETDATE()) AS dateOfDelegation, 
    ISNULL(expireOfDelegation, DATEADD(YEAR, 1, GETDATE())) AS expireOfDelegation, 
    ISNULL(dateOfPermision, GETDATE()) AS dateOfPermision, 
    ISNULL(expireOfPermision, DATEADD(YEAR, 1, GETDATE())) AS expireOfPermision,
    ISNULL(dateOfDelegation, GETDATE()) AS AssignedDate
FROM Delegators 
WHERE CustomerId = @CustomerId 
ORDER BY Name"

            cmd = New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@CustomerId", customerId)
            da = New SqlDataAdapter(cmd)
            da.Fill(dt)

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل المفوضين: " & ex.Message & vbCrLf & "Stack Trace: " & ex.StackTrace, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If da IsNot Nothing Then da.Dispose()
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Dispose()
            End If
        End Try

        Return dt
    End Function

    ' =====================Ensure Delegators Table Exists========================
    Private Sub EnsureDelegatorsTableExists(conn As SqlConnection)
        Try
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Delegators'"
            Dim checkCmd As New SqlCommand(checkTableQuery, conn)
            Dim tableExists As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
            checkCmd.Dispose()

            If tableExists = 0 Then
                ' Create the Delegators table
                Dim createTableQuery As String = "CREATE TABLE Delegators (" &
                    "Id INT IDENTITY(1,1) PRIMARY KEY," &
                    "Name NVARCHAR(255) NOT NULL," &
                    "CustomerId INT NOT NULL," &
                    "[Identity] NVARCHAR(50)," &
                    "Nationality NVARCHAR(100)," &
                    "PhoneNumber NVARCHAR(20)," &
                    "[Type] NVARCHAR(100)," &
                    "dateOfDelegation DATETIME," &
                    "expireOfDelegation DATETIME," &
                    "dateOfPermision DATETIME," &
                    "expireOfPermision DATETIME," &
                    "FOREIGN KEY (CustomerId) REFERENCES Customers(Id) ON DELETE CASCADE" &
                    ")"
                Dim createCmd As New SqlCommand(createTableQuery, conn)
                createCmd.ExecuteNonQuery()
                createCmd.Dispose()
            End If
        Catch ex As Exception
            ' Table creation failed, but we can still continue
        End Try
    End Sub

    ' =====================Ensure Delegators_Permissions Bridge Table Exists========================
    Private Sub EnsureDelegatorsPermissionsTableExists(conn As SqlConnection)
        Try
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Delegators_Permissions'"
            Dim checkCmd As New SqlCommand(checkTableQuery, conn)
            Dim tableExists As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
            checkCmd.Dispose()

            If tableExists = 0 Then
                ' Create the Delegators_Permissions bridge table with cascade constraints
                Dim createTableQuery As String = "CREATE TABLE Delegators_Permissions (" &
                    "Id INT IDENTITY(1,1) PRIMARY KEY," &
                    "PermissionId INT NOT NULL," &
                    "DelegatorId INT NOT NULL," &
                    "AssignedDate DATETIME DEFAULT GETDATE()," &
                    "FOREIGN KEY (DelegatorId) REFERENCES Delegators(Id) ON DELETE CASCADE," &
                    "FOREIGN KEY (PermissionId) REFERENCES Permissions(Id) ON DELETE CASCADE" &
                    ")"
                Dim createCmd As New SqlCommand(createTableQuery, conn)
                createCmd.ExecuteNonQuery()
                createCmd.Dispose()
            End If
        Catch ex As Exception
            ' Table creation failed, but we can still continue
        End Try
    End Sub

    ' =====================Add Delegator========================
    Public Function AddDelegator(name As String, customerId As Integer, identity As String, nationality As String, phoneNumber As String, type As String, dateOfDelegation As DateTime, expireOfDelegation As DateTime, dateOfPermision As DateTime, expireOfPermision As DateTime) As Integer
        Dim localConn As SqlConnection = Nothing
        Dim localCmd As SqlCommand = Nothing
        Dim newDelegatorId As Integer = 0

        Try
            localConn = New SqlConnection(connpath)
            localConn.Open()

            Dim query As String = "INSERT INTO Delegators (Name, CustomerId, [Identity], Nationality, PhoneNumber, [Type], dateOfDelegation, expireOfDelegation, dateOfPermision, expireOfPermision) VALUES (@Name, @CustomerId, @Identity, @Nationality, @PhoneNumber, @Type, @DateOfDelegation, @ExpireOfDelegation, @DateOfPermision, @ExpireOfPermision); SELECT SCOPE_IDENTITY();"
            localCmd = New SqlCommand(query, localConn)
            localCmd.Parameters.AddWithValue("@Name", name)
            localCmd.Parameters.AddWithValue("@CustomerId", customerId)
            localCmd.Parameters.AddWithValue("@Identity", identity)
            localCmd.Parameters.AddWithValue("@Nationality", nationality)
            localCmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber)
            localCmd.Parameters.AddWithValue("@Type", type)
            localCmd.Parameters.AddWithValue("@DateOfDelegation", dateOfDelegation)
            localCmd.Parameters.AddWithValue("@ExpireOfDelegation", expireOfDelegation)
            localCmd.Parameters.AddWithValue("@DateOfPermision", dateOfPermision)
            localCmd.Parameters.AddWithValue("@ExpireOfPermision", expireOfPermision)

            Dim result = localCmd.ExecuteScalar()
            If result IsNot Nothing Then
                newDelegatorId = Convert.ToInt32(result)

                ' Check if permission ID 1 exists before creating bridge entry
                localCmd.Dispose()
                Dim checkPermissionQuery As String = "SELECT COUNT(*) FROM Permissions WHERE Id = 1"
                localCmd = New SqlCommand(checkPermissionQuery, localConn)
                Dim permissionExists As Integer = Convert.ToInt32(localCmd.ExecuteScalar())
                localCmd.Dispose()

                ' Only create bridge entry if permission ID 1 exists
                If permissionExists > 0 Then
                    Dim bridgeQuery As String = "INSERT INTO Delegators_Permissions (PermissionId, DelegatorId, AssignedDate) VALUES (1, @DelegatorId, @AssignedDate)"
                    localCmd = New SqlCommand(bridgeQuery, localConn)
                    localCmd.Parameters.AddWithValue("@DelegatorId", newDelegatorId)
                    localCmd.Parameters.AddWithValue("@AssignedDate", dateOfDelegation)
                    localCmd.ExecuteNonQuery()
                End If
            End If

        Catch ex As Exception
            MessageBox.Show("خطأ في إضافة المفوض: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If localCmd IsNot Nothing Then localCmd.Dispose()
            If localConn IsNot Nothing AndAlso localConn.State = ConnectionState.Open Then
                localConn.Close()
                localConn.Dispose()
            End If
        End Try

        Return newDelegatorId
    End Function

    ' =====================Get All Delegators for Testing========================
    Public Function GetAllDelegators() As DataTable
        Dim dt As New DataTable()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim da As SqlDataAdapter = Nothing

        Try
            conn = New SqlConnection(connpath)
            conn.Open()

            ' First, ensure the required tables exist
            EnsureDelegatorsTableExists(conn)
            EnsureDelegatorsPermissionsTableExists(conn)

            ' Query to get all delegators to test if table has data
            Dim query As String = "
SELECT 
    Id, 
    Name, 
    CustomerId, 
    ISNULL([Identity], '') AS [Identity], 
    ISNULL(Nationality, '') AS Nationality, 
    ISNULL(PhoneNumber, '') AS PhoneNumber, 
    ISNULL([Type], '') AS [Type], 
    ISNULL(dateOfDelegation, GETDATE()) AS dateOfDelegation, 
    ISNULL(expireOfDelegation, DATEADD(YEAR, 1, GETDATE())) AS expireOfDelegation, 
    ISNULL(dateOfPermision, GETDATE()) AS dateOfPermision, 
    ISNULL(expireOfPermision, DATEADD(YEAR, 1, GETDATE())) AS expireOfPermision,
    ISNULL(dateOfDelegation, GETDATE()) AS AssignedDate
FROM Delegators 
ORDER BY Name"

            cmd = New SqlCommand(query, conn)
            da = New SqlDataAdapter(cmd)
            da.Fill(dt)

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل جميع المفوضين: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If da IsNot Nothing Then da.Dispose()
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Dispose()
            End If
        End Try

        Return dt
    End Function

    ' =====================Update Delegator========================
    Public Function UpdateDelegator(id As Integer, name As String, customerId As Integer, identity As String, nationality As String, phoneNumber As String, type As String, dateOfDelegation As DateTime, expireOfDelegation As DateTime, dateOfPermision As DateTime, expireOfPermision As DateTime) As Boolean
        Dim localConn As SqlConnection = Nothing
        Dim localCmd As SqlCommand = Nothing
        Dim success As Boolean = False

        Try
            localConn = New SqlConnection(connpath)
            localConn.Open()

            Dim query As String = "UPDATE Delegators SET Name = @Name, CustomerId = @CustomerId, [Identity] = @Identity, Nationality = @Nationality, PhoneNumber = @PhoneNumber, [Type] = @Type, dateOfDelegation = @DateOfDelegation, expireOfDelegation = @ExpireOfDelegation, dateOfPermision = @DateOfPermision, expireOfPermision = @ExpireOfPermision WHERE Id = @Id"
            localCmd = New SqlCommand(query, localConn)
            localCmd.Parameters.AddWithValue("@Name", name)
            localCmd.Parameters.AddWithValue("@CustomerId", customerId)
            localCmd.Parameters.AddWithValue("@Identity", identity)
            localCmd.Parameters.AddWithValue("@Nationality", nationality)
            localCmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber)
            localCmd.Parameters.AddWithValue("@Type", type)
            localCmd.Parameters.AddWithValue("@DateOfDelegation", dateOfDelegation)
            localCmd.Parameters.AddWithValue("@ExpireOfDelegation", expireOfDelegation)
            localCmd.Parameters.AddWithValue("@DateOfPermision", dateOfPermision)
            localCmd.Parameters.AddWithValue("@ExpireOfPermision", expireOfPermision)
            localCmd.Parameters.AddWithValue("@Id", id)

            Dim rowsAffected As Integer = localCmd.ExecuteNonQuery()
            success = (rowsAffected > 0)

        Catch ex As Exception
            MessageBox.Show("خطأ في تحديث المفوض: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
            success = False
        Finally
            If localCmd IsNot Nothing Then localCmd.Dispose()
            If localConn IsNot Nothing AndAlso localConn.State = ConnectionState.Open Then
                localConn.Close()
                localConn.Dispose()
            End If
        End Try

        Return success
    End Function

    ' =====================Remove Delegator========================
    Public Function RemoveDelegator(id As Integer) As Boolean
        Dim localConn As SqlConnection = Nothing
        Dim localCmd As SqlCommand = Nothing
        Dim transaction As SqlTransaction = Nothing
        Dim success As Boolean = False

        Try
            localConn = New SqlConnection(connpath)
            localConn.Open()
            transaction = localConn.BeginTransaction()

            ' Delete delegator permissions first
            Dim deletePermissionsQuery As String = "DELETE FROM Delegators_Permissions WHERE DelegatorId = @DelegatorId"
            localCmd = New SqlCommand(deletePermissionsQuery, localConn, transaction)
            localCmd.Parameters.AddWithValue("@DelegatorId", id)
            localCmd.ExecuteNonQuery()
            localCmd.Dispose()

            ' Then delete the delegator
            Dim deleteDelegatorQuery As String = "DELETE FROM Delegators WHERE Id = @Id"
            localCmd = New SqlCommand(deleteDelegatorQuery, localConn, transaction)
            localCmd.Parameters.AddWithValue("@Id", id)
            Dim rowsAffected As Integer = localCmd.ExecuteNonQuery()

            If rowsAffected > 0 Then
                transaction.Commit()
                success = True
            Else
                transaction.Rollback()
                success = False
            End If

        Catch ex As Exception
            If transaction IsNot Nothing Then
                transaction.Rollback()
            End If
            MessageBox.Show("خطأ في حذف المفوض: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
            success = False
        Finally
            If localCmd IsNot Nothing Then localCmd.Dispose()
            If transaction IsNot Nothing Then transaction.Dispose()
            If localConn IsNot Nothing AndAlso localConn.State = ConnectionState.Open Then
                localConn.Close()
                localConn.Dispose()
            End If
        End Try

        Return success
    End Function

    ' =====================Get All Permissions========================
    Public Function GetAllPermissions() As DataTable
        Dim dt As New DataTable()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim da As SqlDataAdapter = Nothing

        Try
            conn = New SqlConnection(connpath)
            conn.Open()

            ' Ensure Permissions table exists
            EnsurePermissionsTableExists(conn)

            Dim query As String = "SELECT Id, Name FROM Permissions ORDER BY Name"
            cmd = New SqlCommand(query, conn)
            da = New SqlDataAdapter(cmd)
            da.Fill(dt)

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل الصلاحيات: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If da IsNot Nothing Then da.Dispose()
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Dispose()
            End If
        End Try

        Return dt
    End Function

    ' =====================Get Delegator Permissions========================
    Public Function GetDelegatorPermissions(delegatorId As Integer) As DataTable
        Dim dt As New DataTable()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim da As SqlDataAdapter = Nothing

        Try
            conn = New SqlConnection(connpath)
            conn.Open()

            Dim query As String = "
SELECT 
    p.Id,
    p.Name,
    dp.AssignedDate
FROM Permissions p
INNER JOIN Delegators_Permissions dp ON p.Id = dp.PermissionId
WHERE dp.DelegatorId = @DelegatorId
ORDER BY p.Name"

            cmd = New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@DelegatorId", delegatorId)
            da = New SqlDataAdapter(cmd)
            da.Fill(dt)

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل صلاحيات المفوض: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If da IsNot Nothing Then da.Dispose()
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Dispose()
            End If
        End Try

        Return dt
    End Function

    ' =====================Get Available Permissions for Delegator========================
    Public Function GetAvailablePermissionsForDelegator(delegatorId As Integer) As DataTable
        Dim dt As New DataTable()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim da As SqlDataAdapter = Nothing

        Try
            conn = New SqlConnection(connpath)
            conn.Open()

            Dim query As String = "
SELECT 
    p.Id,
    p.Name
FROM Permissions p
WHERE p.Id NOT IN (
    SELECT dp.PermissionId 
    FROM Delegators_Permissions dp 
    WHERE dp.DelegatorId = @DelegatorId
)
ORDER BY p.Name"

            cmd = New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@DelegatorId", delegatorId)
            da = New SqlDataAdapter(cmd)
            da.Fill(dt)

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل الصلاحيات المتاحة: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If da IsNot Nothing Then da.Dispose()
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Dispose()
            End If
        End Try

        Return dt
    End Function

    ' =====================Add New Permission========================
    Public Function AddPermission(permissionName As String) As Integer
        Dim localConn As SqlConnection = Nothing
        Dim localCmd As SqlCommand = Nothing
        Dim newPermissionId As Integer = 0

        Try
            localConn = New SqlConnection(connpath)
            localConn.Open()

            ' Ensure Permissions table exists
            EnsurePermissionsTableExists(localConn)

            ' Check if permission already exists
            Dim checkQuery As String = "SELECT COUNT(*) FROM Permissions WHERE Name = @Name"
            localCmd = New SqlCommand(checkQuery, localConn)
            localCmd.Parameters.AddWithValue("@Name", permissionName.Trim())

            Dim count As Integer = Convert.ToInt32(localCmd.ExecuteScalar())
            localCmd.Dispose()

            If count = 0 Then
                ' Insert new permission
                Dim insertQuery As String = "INSERT INTO Permissions (Name) VALUES (@Name); SELECT SCOPE_IDENTITY();"
                localCmd = New SqlCommand(insertQuery, localConn)
                localCmd.Parameters.AddWithValue("@Name", permissionName.Trim())

                Dim result = localCmd.ExecuteScalar()
                If result IsNot Nothing Then
                    newPermissionId = Convert.ToInt32(result)
                End If
            Else
                ' Permission already exists - return 0 to indicate duplication
                newPermissionId = -1
            End If

        Catch ex As Exception
            MessageBox.Show("خطأ في إضافة الصلاحية: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
            newPermissionId = 0
        Finally
            If localCmd IsNot Nothing Then localCmd.Dispose()
            If localConn IsNot Nothing AndAlso localConn.State = ConnectionState.Open Then
                localConn.Close()
                localConn.Dispose()
            End If
        End Try

        Return newPermissionId
    End Function

    ' =====================Update Permission========================
    Public Function UpdatePermission(permissionId As Integer, permissionName As String) As Boolean
        Dim localConn As SqlConnection = Nothing
        Dim localCmd As SqlCommand = Nothing
        Dim success As Boolean = False

        Try
            localConn = New SqlConnection(connpath)
            localConn.Open()

            ' Check if another permission with the same name exists (excluding current one)
            Dim checkQuery As String = "SELECT COUNT(*) FROM Permissions WHERE Name = @Name AND Id <> @Id"
            localCmd = New SqlCommand(checkQuery, localConn)
            localCmd.Parameters.AddWithValue("@Name", permissionName.Trim())
            localCmd.Parameters.AddWithValue("@Id", permissionId)

            Dim count As Integer = Convert.ToInt32(localCmd.ExecuteScalar())
            localCmd.Dispose()

            If count = 0 Then
                ' Update permission
                Dim updateQuery As String = "UPDATE Permissions SET Name = @Name WHERE Id = @Id"
                localCmd = New SqlCommand(updateQuery, localConn)
                localCmd.Parameters.AddWithValue("@Name", permissionName.Trim())
                localCmd.Parameters.AddWithValue("@Id", permissionId)

                Dim rowsAffected As Integer = localCmd.ExecuteNonQuery()
                success = (rowsAffected > 0)
            Else
                MessageBox.Show("يوجد صلاحية أخرى بنفس الاسم", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                success = False
            End If

        Catch ex As Exception
            MessageBox.Show("خطأ في تحديث الصلاحية: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
            success = False
        Finally
            If localCmd IsNot Nothing Then localCmd.Dispose()
            If localConn IsNot Nothing AndAlso localConn.State = ConnectionState.Open Then
                localConn.Close()
                localConn.Dispose()
            End If
        End Try

        Return success
    End Function

    ' =====================Remove Permission========================
    Public Function RemovePermission(permissionId As Integer) As Boolean
        Dim localConn As SqlConnection = Nothing
        Dim localCmd As SqlCommand = Nothing
        Dim transaction As SqlTransaction = Nothing
        Dim success As Boolean = False

        Try
            localConn = New SqlConnection(connpath)
            localConn.Open()

            ' Ensure the DelegatorsPermissions table exists (before transaction)
            EnsureDelegatorsPermissionsTableExists(localConn)

            transaction = localConn.BeginTransaction()

            ' First, manually delete related records from Delegators_Permissions 
            ' (in case the existing foreign key doesn't have CASCADE DELETE)
            Dim deleteRelationsQuery As String = "DELETE FROM Delegators_Permissions WHERE PermissionId = @PermissionId"
            localCmd = New SqlCommand(deleteRelationsQuery, localConn, transaction)
            localCmd.Parameters.AddWithValue("@PermissionId", permissionId)
            localCmd.ExecuteNonQuery()
            localCmd.Dispose()

            ' Then delete the permission itself
            Dim deletePermissionQuery As String = "DELETE FROM Permissions WHERE Id = @Id"
            localCmd = New SqlCommand(deletePermissionQuery, localConn, transaction)
            localCmd.Parameters.AddWithValue("@Id", permissionId)
            Dim rowsAffected As Integer = localCmd.ExecuteNonQuery()

            If rowsAffected > 0 Then
                transaction.Commit()
                success = True
            Else
                transaction.Rollback()
                success = False
            End If

        Catch ex As Exception
            If transaction IsNot Nothing Then
                transaction.Rollback()
            End If
            MessageBox.Show("خطأ في حذف الصلاحية: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
            success = False
        Finally
            If localCmd IsNot Nothing Then localCmd.Dispose()
            If transaction IsNot Nothing Then transaction.Dispose()
            If localConn IsNot Nothing AndAlso localConn.State = ConnectionState.Open Then
                localConn.Close()
                localConn.Dispose()
            End If
        End Try

        Return success
    End Function

    ' =====================Get All Delegator Dates========================
    Public Function GetDelegatorDates(delegatorId As Integer) As (startDate As DateTime, endDate As DateTime, permissionStartDate As DateTime, permissionEndDate As DateTime)
        Dim localConn As SqlConnection = Nothing
        Dim localCmd As SqlCommand = Nothing
        Dim startDate As DateTime = DateTime.Now
        Dim endDate As DateTime = DateTime.Now.AddYears(1)
        Dim permissionStartDate As DateTime = DateTime.Now
        Dim permissionEndDate As DateTime = DateTime.Now.AddYears(1)

        Try
            localConn = New SqlConnection(connpath)
            localConn.Open()

            Dim query As String = "SELECT ISNULL(dateOfDelegation, GETDATE()) AS startDate, " &
                                 "ISNULL(expireOfDelegation, DATEADD(YEAR, 1, GETDATE())) AS endDate, " &
                                 "ISNULL(dateOfPermision, GETDATE()) AS permissionStartDate, " &
                                 "ISNULL(expireOfPermision, DATEADD(YEAR, 1, GETDATE())) AS permissionEndDate " &
                                 "FROM Delegators WHERE Id = @Id"
            localCmd = New SqlCommand(query, localConn)
            localCmd.Parameters.AddWithValue("@Id", delegatorId)

            Dim reader = localCmd.ExecuteReader()
            If reader.Read() Then
                startDate = Convert.ToDateTime(reader("startDate"))
                endDate = Convert.ToDateTime(reader("endDate"))
                permissionStartDate = Convert.ToDateTime(reader("permissionStartDate"))
                permissionEndDate = Convert.ToDateTime(reader("permissionEndDate"))
            End If
            reader.Close()

        Catch ex As Exception
            ' If there's an error, use default dates
            startDate = DateTime.Now
            endDate = DateTime.Now.AddYears(1)
            permissionStartDate = DateTime.Now
            permissionEndDate = DateTime.Now.AddYears(1)
        Finally
            If localCmd IsNot Nothing Then localCmd.Dispose()
            If localConn IsNot Nothing AndAlso localConn.State = ConnectionState.Open Then
                localConn.Close()
                localConn.Dispose()
            End If
        End Try

        Return (startDate, endDate, permissionStartDate, permissionEndDate)
    End Function

    ' =====================Grant Permission to Delegator========================
    Public Function GrantPermissionToDelegator(delegatorId As Integer, permissionId As Integer) As Boolean
        Dim localConn As SqlConnection = Nothing
        Dim localCmd As SqlCommand = Nothing
        Dim success As Boolean = False

        Try
            localConn = New SqlConnection(connpath)
            localConn.Open()

            ' Check if permission already exists
            Dim checkQuery As String = "SELECT COUNT(*) FROM Delegators_Permissions WHERE DelegatorId = @DelegatorId AND PermissionId = @PermissionId"
            localCmd = New SqlCommand(checkQuery, localConn)
            localCmd.Parameters.AddWithValue("@DelegatorId", delegatorId)
            localCmd.Parameters.AddWithValue("@PermissionId", permissionId)

            Dim count As Integer = Convert.ToInt32(localCmd.ExecuteScalar())
            localCmd.Dispose()

            If count = 0 Then
                ' Insert new permission
                Dim insertQuery As String = "INSERT INTO Delegators_Permissions (DelegatorId, PermissionId, AssignedDate) VALUES (@DelegatorId, @PermissionId, @AssignedDate)"
                localCmd = New SqlCommand(insertQuery, localConn)
                localCmd.Parameters.AddWithValue("@DelegatorId", delegatorId)
                localCmd.Parameters.AddWithValue("@PermissionId", permissionId)
                localCmd.Parameters.AddWithValue("@AssignedDate", DateTime.Now)

                Dim rowsAffected As Integer = localCmd.ExecuteNonQuery()
                success = (rowsAffected > 0)
            Else
                success = True ' Already exists
            End If

        Catch ex As Exception
            MessageBox.Show("خطأ في منح الصلاحية: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
            success = False
        Finally
            If localCmd IsNot Nothing Then localCmd.Dispose()
            If localConn IsNot Nothing AndAlso localConn.State = ConnectionState.Open Then
                localConn.Close()
                localConn.Dispose()
            End If
        End Try

        Return success
    End Function

    ' =====================Revoke Permission from Delegator========================
    Public Function RevokePermissionFromDelegator(delegatorId As Integer, permissionId As Integer) As Boolean
        Dim localConn As SqlConnection = Nothing
        Dim localCmd As SqlCommand = Nothing
        Dim success As Boolean = False

        Try
            localConn = New SqlConnection(connpath)
            localConn.Open()

            Dim query As String = "DELETE FROM Delegators_Permissions WHERE DelegatorId = @DelegatorId AND PermissionId = @PermissionId"
            localCmd = New SqlCommand(query, localConn)
            localCmd.Parameters.AddWithValue("@DelegatorId", delegatorId)
            localCmd.Parameters.AddWithValue("@PermissionId", permissionId)

            Dim rowsAffected As Integer = localCmd.ExecuteNonQuery()
            success = (rowsAffected > 0)

        Catch ex As Exception
            MessageBox.Show("خطأ في إلغاء الصلاحية: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
            success = False
        Finally
            If localCmd IsNot Nothing Then localCmd.Dispose()
            If localConn IsNot Nothing AndAlso localConn.State = ConnectionState.Open Then
                localConn.Close()
                localConn.Dispose()
            End If
        End Try

        Return success
    End Function

    ' =====================Ensure Permissions Table Exists========================
    Private Sub EnsurePermissionsTableExists(conn As SqlConnection)
        Try
            Dim checkTableQuery As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Permissions'"
            Dim checkCmd As New SqlCommand(checkTableQuery, conn)
            Dim tableExists As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
            checkCmd.Dispose()

            If tableExists = 0 Then
                ' Create the Permissions table
                Dim createTableQuery As String = "CREATE TABLE Permissions (" &
                    "Id INT IDENTITY(1,1) PRIMARY KEY," &
                    "Name NVARCHAR(255) NOT NULL UNIQUE" &
                    ")"
                Dim createCmd As New SqlCommand(createTableQuery, conn)
                createCmd.ExecuteNonQuery()
                createCmd.Dispose()

                ' Insert default permissions
                Dim insertDefaults As String = "INSERT INTO Permissions (Name) VALUES " &
                    "('إدارة الوثائق'), ('إضافة مرفقات'), ('حذف مرفقات'), ('تحديث بيانات العملاء'), ('عرض التقارير')"
                Dim insertCmd As New SqlCommand(insertDefaults, conn)
                insertCmd.ExecuteNonQuery()
                insertCmd.Dispose()
            End If
        Catch ex As Exception
            ' Table creation failed, but we can still continue
        End Try
    End Sub


    ' Methods for UsersLogin_Documents table (for PDF document access)
    Public Function IsUserActive(userCode As String) As Boolean
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing

        Try
            conn = New SqlConnection(connpath)
            conn.Open()

            Dim query As String = "SELECT Active FROM UsersLogin_Documents WHERE userId = @userId"
            cmd = New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@userId", userCode)

            Dim result As Object = cmd.ExecuteScalar()
            If result IsNot Nothing Then
                Return Convert.ToBoolean(result)
            Else
                Return False
            End If

        Catch ex As Exception
            Throw New Exception("Error checking user status: " & ex.Message)
        Finally
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Function

    Public Function GetUserPassword(userCode As String) As String
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing

        Try
            conn = New SqlConnection(connpath)
            conn.Open()

            Dim query As String = "SELECT password FROM UsersLogin_Documents WHERE userId = @userId AND Active = 1"
            cmd = New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@userId", userCode)

            Dim result As Object = cmd.ExecuteScalar()
            If result IsNot Nothing Then
                Return result.ToString()
            Else
                Return Nothing
            End If

        Catch ex As Exception
            Throw New Exception("Error retrieving user password: " & ex.Message)
        Finally
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Function

    Public Function GetActiveDocumentUsers() As DataTable
        Dim dt As New DataTable()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim da As SqlDataAdapter = Nothing

        Try
            conn = New SqlConnection(connpath)
            conn.Open()

            Dim query As String = "SELECT userId, password FROM UsersLogin_Documents WHERE Active = 1"
            cmd = New SqlCommand(query, conn)
            da = New SqlDataAdapter(cmd)
            da.Fill(dt)

        Catch ex As Exception
            Throw New Exception("Error retrieving active document users: " & ex.Message)
        Finally
            If da IsNot Nothing Then da.Dispose()
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try

        Return dt
    End Function

    ' Get all documents with customer names
    Public Function GetDocumentsWithCustomerNames() As DataTable
        Dim dt As New DataTable()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim da As SqlDataAdapter = Nothing

        Try
            conn = New SqlConnection(connpath)
            conn.Open()

            Dim query As String = "SELECT d.Id, c.Name AS CustomerName, a.Name AS AttachmentName, d.FilePath, d.UploadingDate, d.ExpireDate, d.fileAttached, d.Attachment_ID " &
                                "FROM Customers_Attachmnets_Documnets d " &
                                "INNER JOIN Attachments a ON d.Attachment_ID = a.Id " &
                                "INNER JOIN Customers c ON d.Customer_ID = c.Id " &
                                "ORDER BY c.Name, d.UploadingDate DESC"
            cmd = New SqlCommand(query, conn)
            da = New SqlDataAdapter(cmd)
            da.Fill(dt)

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل الوثائق مع أسماء العملاء: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If da IsNot Nothing Then da.Dispose()
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Dispose()
            End If
        End Try

        Return dt
    End Function

    ' Get about to expire documents with customer names (within 30 days)
    Public Function GetAboutToExpireDocumentsWithCustomerNames() As DataTable
        Dim dt As New DataTable()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim da As SqlDataAdapter = Nothing

        Try
            conn = New SqlConnection(connpath)
            conn.Open()

            Dim query As String = "SELECT d.Id, c.Name AS CustomerName, a.Name AS AttachmentName, d.FilePath, d.UploadingDate, d.ExpireDate, d.fileAttached, d.Attachment_ID " &
                                "FROM Customers_Attachmnets_Documnets d " &
                                "INNER JOIN Attachments a ON d.Attachment_ID = a.Id " &
                                "INNER JOIN Customers c ON d.Customer_ID = c.Id " &
                                "WHERE d.ExpireDate > GETDATE() AND d.ExpireDate <= DATEADD(DAY, 30, GETDATE()) " &
                                "ORDER BY d.ExpireDate ASC, c.Name"
            cmd = New SqlCommand(query, conn)
            da = New SqlDataAdapter(cmd)
            da.Fill(dt)

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل الوثائق المنتهية الصلاحية قريباً مع أسماء العملاء: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If da IsNot Nothing Then da.Dispose()
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Dispose()
            End If
        End Try

        Return dt
    End Function

    ' Get expired documents with customer names
    Public Function GetExpiredDocumentsWithCustomerNames() As DataTable
        Dim dt As New DataTable()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim da As SqlDataAdapter = Nothing

        Try
            conn = New SqlConnection(connpath)
            conn.Open()

            Dim query As String = "SELECT d.Id, c.Name AS CustomerName, a.Name AS AttachmentName, d.FilePath, d.UploadingDate, d.ExpireDate, d.fileAttached, d.Attachment_ID " &
                                "FROM Customers_Attachmnets_Documnets d " &
                                "INNER JOIN Attachments a ON d.Attachment_ID = a.Id " &
                                "INNER JOIN Customers c ON d.Customer_ID = c.Id " &
                                "WHERE d.ExpireDate < GETDATE() " &
                                "ORDER BY d.ExpireDate DESC, c.Name"
            cmd = New SqlCommand(query, conn)
            da = New SqlDataAdapter(cmd)
            da.Fill(dt)

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل الوثائق المنتهية الصلاحية مع أسماء العملاء: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If da IsNot Nothing Then da.Dispose()
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Dispose()
            End If
        End Try

        Return dt
    End Function

    ' =====================Get Countries from CustomerAccountsMaster Database========================
    Public Function GetCountries() As DataTable
        Dim dt As New DataTable()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim da As SqlDataAdapter = Nothing

        Try
            conn = New SqlConnection(connpath2)
            conn.Open()

            Dim query As String = "SELECT countrycode, countryName, contryarname FROM CountryMaster WHERE active = 'True' ORDER BY contryarname"
            cmd = New SqlCommand(query, conn)
            da = New SqlDataAdapter(cmd)
            da.Fill(dt)

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل البلدان: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If da IsNot Nothing Then da.Dispose()
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Dispose()
            End If
        End Try

        Return dt
    End Function

    ' =====================Get Branches from CustomerAccountsMaster Database========================
    Public Function GetBranches() As DataTable
        Dim dt As New DataTable()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim da As SqlDataAdapter = Nothing

        Try
            conn = New SqlConnection(connpath2)
            conn.Open()

            Dim query As String = "SELECT branch_code, branch_name, branch_arabic FROM Branchs ORDER BY branch_arabic"
            cmd = New SqlCommand(query, conn)
            da = New SqlDataAdapter(cmd)
            da.Fill(dt)

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل الفروع: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If da IsNot Nothing Then da.Dispose()
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Dispose()
            End If
        End Try

        Return dt
    End Function

    ' =====================Get Currency from CustomerAccountsMaster Database========================
    Public Function GetCurrency() As DataTable
        Dim dt As New DataTable()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim da As SqlDataAdapter = Nothing

        Try
            conn = New SqlConnection(connpath2)
            conn.Open()

            Dim query As String = "SELECT suffix_code, symbol, arabicname, decimal FROM CurrencyMaster ORDER BY arabicname"
            cmd = New SqlCommand(query, conn)
            da = New SqlDataAdapter(cmd)
            da.Fill(dt)

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل العملات: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If da IsNot Nothing Then da.Dispose()
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Dispose()
            End If
        End Try

        Return dt
    End Function

    ' =====================Update Branch Selection Data========================
    Public Function UpdateBranchSelection(branchCode As String, isSelected As Boolean, isActive As Boolean, isLocked As Boolean, refNo As String) As Boolean
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim success As Boolean = False

        Try
            conn = New SqlConnection(connpath2)
            conn.Open()

            ' Check if record exists
            Dim checkQuery As String = "SELECT COUNT(*) FROM CustomerAccountsMaster_BranchSelected WHERE fld_branch_code = @BranchCode"
            cmd = New SqlCommand(checkQuery, conn)
            cmd.Parameters.AddWithValue("@BranchCode", branchCode)
            Dim recordExists As Integer = Convert.ToInt32(cmd.ExecuteScalar())
            cmd.Dispose()

            If recordExists > 0 Then
                ' Update existing record
                Dim updateQuery As String = "UPDATE CustomerAccountsMaster_BranchSelected SET fld_select = @Selected, fld_active_branch = @Active, fld_loacked = @Locked, fld_ref_no_branch = @RefNo WHERE fld_branch_code = @BranchCode"
                cmd = New SqlCommand(updateQuery, conn)
            Else
                ' Insert new record
                Dim insertQuery As String = "INSERT INTO CustomerAccountsMaster_BranchSelected (fld_branch_code, fld_select, fld_active_branch, fld_loacked, fld_ref_no_branch) VALUES (@BranchCode, @Selected, @Active, @Locked, @RefNo)"
                cmd = New SqlCommand(insertQuery, conn)
            End If

            cmd.Parameters.AddWithValue("@BranchCode", branchCode)
            cmd.Parameters.AddWithValue("@Selected", isSelected)
            cmd.Parameters.AddWithValue("@Active", isActive)
            cmd.Parameters.AddWithValue("@Locked", isLocked)
            cmd.Parameters.AddWithValue("@RefNo", refNo)

            Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
            success = (rowsAffected > 0)

        Catch ex As Exception
            MessageBox.Show("خطأ في تحديث بيانات الفرع: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
            success = False
        Finally
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Dispose()
            End If
        End Try

        Return success
    End Function

    ' =====================Get Areas from CustomerAccountsMaster Database========================
    Public Function GetAreas(countryCode As String) As DataTable
        Dim dt As New DataTable()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim da As SqlDataAdapter = Nothing

        Try
            conn = New SqlConnection(connpath2)
            conn.Open()

            Dim query As String = "SELECT code, description, shortname, contryCode FROM [CMGADB2024].[dbo].[AreaMaster] WHERE contryCode = @CountryCode AND active = 'True' ORDER BY description"
            cmd = New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@CountryCode", countryCode)
            da = New SqlDataAdapter(cmd)
            da.Fill(dt)

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل المناطق: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If da IsNot Nothing Then da.Dispose()
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Dispose()
            End If
        End Try

        Return dt
    End Function

    ' =====================Get Market Data from CustomerAccountsMaster Database========================
    Public Function GetMarketData() As DataTable
        Dim dt As New DataTable()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim da As SqlDataAdapter = Nothing

        Try
            conn = New SqlConnection(connpath2)
            conn.Open()

            Dim query As String = "SELECT code, description, shortname, active, arabic_desc, fld_area_code FROM [CMGADB2024].[dbo].[CusTransactionMaster] WHERE active = 'True' ORDER BY description"
            cmd = New SqlCommand(query, conn)
            da = New SqlDataAdapter(cmd)
            da.Fill(dt)

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل بيانات السوق: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If da IsNot Nothing Then da.Dispose()
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Dispose()
            End If
        End Try

        Return dt
    End Function

    ' =====================Get Category Data from CustomerAccountsMaster Database========================
    Public Function GetCategoryData() As DataTable
        Dim dt As New DataTable()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim da As SqlDataAdapter = Nothing

        Try
            conn = New SqlConnection(connpath2)
            conn.Open()

            Dim query As String = "SELECT fld_code, fld_name, fld_arabic_name, fld_active FROM [CMGADB2024].[dbo].[CustomerCategory] WHERE fld_active = 'True' ORDER BY fld_arabic_name"
            cmd = New SqlCommand(query, conn)
            da = New SqlDataAdapter(cmd)
            da.Fill(dt)

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل بيانات الفئة: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If da IsNot Nothing Then da.Dispose()
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Dispose()
            End If
        End Try

        Return dt
    End Function

    ' =====================Get Groups Data from CustomerAccountsMaster Database========================
    Public Function GetGroupsData() As DataTable
        Dim dt As New DataTable()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim da As SqlDataAdapter = Nothing

        Try
            conn = New SqlConnection(connpath2)
            conn.Open()

            Dim query As String = "SELECT code, description, shortname, active, arabic_desc FROM [CMGADB2024].[dbo].[CusGradeMaster] WHERE active = 'True' ORDER BY description"
            cmd = New SqlCommand(query, conn)
            da = New SqlDataAdapter(cmd)
            da.Fill(dt)

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل بيانات المجموعات: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If da IsNot Nothing Then da.Dispose()
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Dispose()
            End If
        End Try

        Return dt
    End Function

    ' =====================Get Type Data from CustomerAccountsMaster Database========================
    Public Function GetTypeData() As DataTable
        Dim dt As New DataTable()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim da As SqlDataAdapter = Nothing

        Try
            conn = New SqlConnection(connpath2)
            conn.Open()

            Dim query As String = "SELECT fld_code, fld_name, fld_arabic_name, fld_active FROM [CMGADB2024].[dbo].[CustomerType] WHERE fld_active = 'True' ORDER BY fld_arabic_name"
            cmd = New SqlCommand(query, conn)
            da = New SqlDataAdapter(cmd)
            da.Fill(dt)

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل بيانات النوع: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If da IsNot Nothing Then da.Dispose()
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Dispose()
            End If
        End Try

        Return dt
    End Function

    ' =====================Customer/Supplier Management========================
    ' Generate next code for Customer or Supplier
    Public Function GenerateNextCode(isCustomer As Boolean) As String
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim nextNumber As Integer = 1

        Try
            conn = GetConnection2() ' Use CMGADB2024 database
            conn.Open()

            Dim prefix As String = If(isCustomer, "C", "S")
            Dim query As String = $"SELECT MAX(CAST(SUBSTRING(code, 2, 4) AS INT)) FROM [CustomerAccountsMaster] WHERE code LIKE '{prefix}%'"

            cmd = New SqlCommand(query, conn)
            Dim result = cmd.ExecuteScalar()

            If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                nextNumber = Convert.ToInt32(result) + 1
            End If

        Catch ex As Exception
            MessageBox.Show("خطأ في توليد الكود: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Dispose()
            End If
        End Try

        Dim prefix2 As String = If(isCustomer, "C", "S")
        Return $"{prefix2}{nextNumber.ToString("D4")}"
    End Function

    ' Save or Update Customer/Supplier
    Public Function SaveCustomerSupplier(customerData As CustomerSupplierData) As Boolean
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing

        Try
            conn = GetConnection2() ' Use CMGADB2024 database
            conn.Open()

            Dim query As String = ""

            If customerData.IsUpdate AndAlso Not String.IsNullOrEmpty(customerData.ExistingCode) Then
                ' Update existing record - include FK fields
                query = "UPDATE [CustomerAccountsMaster] SET " &
                       "c_type = @c_type, " &
                       "name = @name, " &
                       "fld_arabic_name = @fld_arabic_name, " &
                       "shortname = @shortname, " &
                       "bussiness_address = @bussiness_address, " &
                       "acc_manager = @acc_manager, " &
                       "bank_acc_name = @bank_acc_name, " &
                       "bank_acc_no = @bank_acc_no, " &
                       "fld_state = @fld_state, " &
                       "fld_dist = @fld_dist, " &
                       "vat_tin_no = @vat_tin_no, " &
                       "email = @email, " &
                       "fld_contry_code_mobile = @fld_contry_code_mobile, " &
                       "country = @country, " &
                       "fld_fax_no = @fld_fax_no, " &
                       "fld_ref_no = @fld_ref_no, " &
                       "fld_indvl_id_no = @fld_indvl_id_no, " &
                       "fld_cr_no = @fld_cr_no, " &
                       "sales_man = @sales_man, " &
                       "scrap_adj_code = @scrap_adj_code, " &
                       "type = @type, " &
                       "categoty = @categoty " &
                       "WHERE code = @code"
            Else
                ' Insert new record - include FK fields
                query = "INSERT INTO [CustomerAccountsMaster] " &
                       "(code, c_type, name, fld_arabic_name, shortname, bussiness_address, " &
                       "acc_manager, bank_acc_name, bank_acc_no, fld_state, fld_dist, " &
                       "vat_tin_no, email, fld_contry_code_mobile, country, fld_fax_no, " &
                       "fld_ref_no, fld_indvl_id_no, fld_cr_no, currency, active, introduced_date, " &
                       "sales_man, scrap_adj_code, type, categoty) " &
                       "VALUES (@code, @c_type, @name, @fld_arabic_name, @shortname, @bussiness_address, " &
                       "@acc_manager, @bank_acc_name, @bank_acc_no, @fld_state, @fld_dist, " &
                       "@vat_tin_no, @email, @fld_contry_code_mobile, @country, @fld_fax_no, " &
                       "@fld_ref_no, @fld_indvl_id_no, @fld_cr_no, @currency, @active, @introduced_date, " &
                       "@sales_man, @scrap_adj_code, @type, @categoty)"
            End If

            cmd = New SqlCommand(query, conn)

            ' Add parameters with debug output
            System.Diagnostics.Debug.WriteLine("=== Adding SQL Parameters ===")
            cmd.Parameters.AddWithValue("@code", TruncateString(customerData.Code, 10))
            System.Diagnostics.Debug.WriteLine($"@code: '{customerData.Code}' -> '{TruncateString(customerData.Code, 10)}'")
            
            cmd.Parameters.AddWithValue("@c_type", TruncateString(customerData.CustomerType, 10))
            System.Diagnostics.Debug.WriteLine($"@c_type: '{customerData.CustomerType}' -> '{TruncateString(customerData.CustomerType, 10)}'")
            
            cmd.Parameters.AddWithValue("@name", TruncateString(customerData.EnglishName, 50))
            System.Diagnostics.Debug.WriteLine($"@name: '{customerData.EnglishName}' (Length: {customerData.EnglishName?.Length})")
            
            cmd.Parameters.AddWithValue("@fld_arabic_name", TruncateString(customerData.ArabicName, 50))
            System.Diagnostics.Debug.WriteLine($"@fld_arabic_name: '{customerData.ArabicName}' (Length: {customerData.ArabicName?.Length})")
            
            cmd.Parameters.AddWithValue("@shortname", TruncateString(customerData.CommercialName, 30))
            System.Diagnostics.Debug.WriteLine($"@shortname: '{customerData.CommercialName}' (Length: {customerData.CommercialName?.Length})")
            
            cmd.Parameters.AddWithValue("@bussiness_address", TruncateString(customerData.Address, 200))
            System.Diagnostics.Debug.WriteLine($"@bussiness_address: Length: {customerData.Address?.Length}")
            
            cmd.Parameters.AddWithValue("@acc_manager", TruncateString(customerData.Manager, 50))
            cmd.Parameters.AddWithValue("@bank_acc_name", TruncateString(customerData.ManagerID, 30))
            cmd.Parameters.AddWithValue("@bank_acc_no", TruncateString(customerData.ManagerNumber, 30))
            cmd.Parameters.AddWithValue("@fld_state", TruncateString(customerData.Country, 100))
            cmd.Parameters.AddWithValue("@fld_dist", TruncateString(customerData.Area, 100))
            cmd.Parameters.AddWithValue("@vat_tin_no", TruncateString(customerData.VATNumber, 15))
            cmd.Parameters.AddWithValue("@email", TruncateString(customerData.Email, 50))
            cmd.Parameters.AddWithValue("@fld_contry_code_mobile", TruncateString(customerData.MobileCountryCode, 5))
            cmd.Parameters.AddWithValue("@country", TruncateString(customerData.CountryName, 500))
            cmd.Parameters.AddWithValue("@fld_fax_no", TruncateString(customerData.FaxNumber, 15))
            cmd.Parameters.AddWithValue("@fld_ref_no", TruncateString(customerData.ReferralNumber, 15))
            cmd.Parameters.AddWithValue("@fld_indvl_id_no", TruncateString(customerData.IndividualID, 15))
            cmd.Parameters.AddWithValue("@fld_cr_no", TruncateString(customerData.CommercialRecord, 15))
            
            ' Add FK parameters - apply to both INSERT and UPDATE
            cmd.Parameters.AddWithValue("@sales_man", TruncateString(customerData.SalesMan, 50))
            cmd.Parameters.AddWithValue("@scrap_adj_code", TruncateString(customerData.ScrapAdjCode, 50))
            cmd.Parameters.AddWithValue("@type", TruncateString(customerData.TypeCode, 50))
            cmd.Parameters.AddWithValue("@categoty", TruncateString(customerData.CategoryCode, 50))
            
            ' Add additional parameters for INSERT operations only
            If Not customerData.IsUpdate Then
                cmd.Parameters.AddWithValue("@currency", TruncateString("SAR", 10))  ' Default currency
                cmd.Parameters.AddWithValue("@active", True)  ' Default to active
                cmd.Parameters.AddWithValue("@introduced_date", DateTime.Now)  ' Current date
                System.Diagnostics.Debug.WriteLine("Added default values for new customer")
            End If
            
            System.Diagnostics.Debug.WriteLine("=== End SQL Parameters ===")

            Dim result As Integer = cmd.ExecuteNonQuery()
            Return result > 0

        Catch ex As Exception
            ' Log detailed error information for debugging
            System.Diagnostics.Debug.WriteLine($"Database error: {ex.Message}")
            System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}")

            Dim errorMsg As String = "خطأ في حفظ بيانات العميل/المورد: " & ex.Message

            ' Check if it's a truncation error and provide helpful message
            If ex.Message.Contains("truncated") OrElse ex.Message.Contains("String or binary data would be truncated") Then
                errorMsg = "خطأ في حفظ البيانات: أحد الحقول يحتوي على نص طويل جداً. تم تقليل طول النص إلى الحد المسموح. يرجى المحاولة مرة أخرى." & vbCrLf & vbCrLf & "التفاصيل التقنية: " & ex.Message
            End If

            MessageBox.Show(errorMsg, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        Finally
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Dispose()
            End If
        End Try
    End Function


    ' Get all customer/supplier codes for navigation
    Public Function GetAllCustomerSupplierCodes() As List(Of String)
        Dim codes As New List(Of String)()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing

        Try
            conn = GetConnection2() ' Use CMGADB2024 database
            conn.Open()

            Dim query As String = "SELECT code FROM [CustomerAccountsMaster] ORDER BY code"
            cmd = New SqlCommand(query, conn)
            reader = cmd.ExecuteReader()

            While reader.Read()
                If reader("code") IsNot DBNull.Value Then
                    Dim code As String = reader("code").ToString()
                    codes.Add(code)
                    System.Diagnostics.Debug.WriteLine($"Found customer code: '{code}' (Length: {code.Length})")
                End If
            End While
            
            System.Diagnostics.Debug.WriteLine($"Total customer codes loaded: {codes.Count}")

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل قائمة العملاء/الموردين: " & ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If reader IsNot Nothing Then reader.Close()
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Dispose()
            End If
        End Try

        Return codes
    End Function

    ' =====================Lookup Methods for ComboBoxes========================
    
    ' Load data from CusTransactionMaster table
    Public Function LoadCusTransactionMaster() As DataTable
        Dim dt As New DataTable()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        
        Try
            conn = New SqlConnection(connpath2)
            ' Create the basic structure
            dt.Columns.Add("fld_area_code", GetType(String))
            dt.Columns.Add("DisplayText", GetType(String))
            dt.Columns.Add("Description", GetType(String))
            
            conn.Open()
            
            ' Try to get data with description if available
            Dim query As String = "SELECT DISTINCT fld_area_code, 
                CASE 
                    WHEN EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CusTransactionMaster' AND COLUMN_NAME = 'description')
                    THEN COALESCE(description, fld_area_code)
                    ELSE fld_area_code
                END as description
                FROM CusTransactionMaster 
                WHERE fld_area_code IS NOT NULL 
                ORDER BY fld_area_code"
            
            cmd = New SqlCommand(query, conn)
            Dim reader As SqlDataReader = cmd.ExecuteReader()
            
            While reader.Read()
                Dim row As DataRow = dt.NewRow()
                Dim areaCode As String = If(reader("fld_area_code") Is DBNull.Value, "", reader("fld_area_code").ToString().Trim())
                Dim description As String = If(reader("description") Is DBNull.Value, areaCode, reader("description").ToString().Trim())
                
                row("fld_area_code") = areaCode
                row("Description") = description
                row("DisplayText") = If(String.IsNullOrEmpty(description) OrElse description = areaCode, areaCode, $"{areaCode} - {description}")
                dt.Rows.Add(row)
            End While
            
            reader.Close()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Error loading CusTransactionMaster: " & ex.Message)
            ' Add a default empty row to prevent binding errors
            If dt.Rows.Count = 0 Then
                Dim emptyRow As DataRow = dt.NewRow()
                emptyRow("fld_area_code") = ""
                emptyRow("Description") = ""
                emptyRow("DisplayText") = "لا توجد بيانات"
                dt.Rows.Add(emptyRow)
            End If
        Finally
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing Then
                If conn.State = ConnectionState.Open Then conn.Close()
                conn.Dispose()
            End If
        End Try
        
        Return dt
    End Function

    ' Load data from CusGradeMaster table
    Public Function LoadCusGradeMaster() As DataTable
        Dim dt As New DataTable()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        
        Try
            conn = New SqlConnection(connpath2)
            ' Create the basic structure
            dt.Columns.Add("code", GetType(String))
            dt.Columns.Add("DisplayText", GetType(String))
            dt.Columns.Add("Description", GetType(String))
            
            conn.Open()
            
            ' Try to get data with description if available
            Dim query As String = "SELECT DISTINCT code, 
                CASE 
                    WHEN EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CusGradeMaster' AND COLUMN_NAME = 'description')
                    THEN COALESCE(description, code)
                    ELSE code
                END as description
                FROM CusGradeMaster 
                WHERE code IS NOT NULL 
                ORDER BY code"
            
            cmd = New SqlCommand(query, conn)
            Dim reader As SqlDataReader = cmd.ExecuteReader()
            
            While reader.Read()
                Dim row As DataRow = dt.NewRow()
                Dim code As String = If(reader("code") Is DBNull.Value, "", reader("code").ToString().Trim())
                Dim description As String = If(reader("description") Is DBNull.Value, code, reader("description").ToString().Trim())
                
                row("code") = code
                row("Description") = description
                row("DisplayText") = If(String.IsNullOrEmpty(description) OrElse description = code, code, $"{code} - {description}")
                dt.Rows.Add(row)
            End While
            
            reader.Close()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Error loading CusGradeMaster: " & ex.Message)
            ' Add a default empty row to prevent binding errors
            If dt.Rows.Count = 0 Then
                Dim emptyRow As DataRow = dt.NewRow()
                emptyRow("code") = ""
                emptyRow("Description") = ""
                emptyRow("DisplayText") = "لا توجد بيانات"
                dt.Rows.Add(emptyRow)
            End If
        Finally
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing Then
                If conn.State = ConnectionState.Open Then conn.Close()
                conn.Dispose()
            End If
        End Try
        
        Return dt
    End Function

    ' Load data from CustomerType table (FK: type)
    Public Function LoadCustomerType() As DataTable
        Dim dt As New DataTable()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        
        Try
            conn = New SqlConnection(connpath2)
            ' Create the basic structure
            dt.Columns.Add("fld_code", GetType(String))
            dt.Columns.Add("DisplayText", GetType(String))
            dt.Columns.Add("Description", GetType(String))
            
            conn.Open()
            
            ' Load from CustomerType table - PK: fld_code -> FK: type
            Dim query As String = "SELECT DISTINCT fld_code FROM CustomerType WHERE fld_code IS NOT NULL ORDER BY fld_code"
            
            cmd = New SqlCommand(query, conn)
            Dim reader As SqlDataReader = cmd.ExecuteReader()
            
            While reader.Read()
                Dim row As DataRow = dt.NewRow()
                Dim fldCode As String = If(reader("fld_code") Is DBNull.Value, "", reader("fld_code").ToString().Trim())
                
                row("fld_code") = fldCode
                row("Description") = fldCode
                row("DisplayText") = fldCode
                dt.Rows.Add(row)
            End While
            
            reader.Close()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Error loading CustomerType: " & ex.Message)
            ' Add a default empty row to prevent binding errors
            If dt.Rows.Count = 0 Then
                Dim emptyRow As DataRow = dt.NewRow()
                emptyRow("fld_code") = ""
                emptyRow("Description") = ""
                emptyRow("DisplayText") = "لا توجد بيانات"
                dt.Rows.Add(emptyRow)
            End If
        Finally
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing Then
                If conn.State = ConnectionState.Open Then conn.Close()
                conn.Dispose()
            End If
        End Try
        
        Return dt
    End Function

    ' Load data from CustomerCategory table (FK: categoty)
    Public Function LoadCustomerCategory() As DataTable
        Dim dt As New DataTable()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        
        Try
            conn = New SqlConnection(connpath2)
            ' Create the basic structure
            dt.Columns.Add("fld_code", GetType(String))
            dt.Columns.Add("DisplayText", GetType(String))
            dt.Columns.Add("Description", GetType(String))
            
            conn.Open()
            
            ' Load from CustomerCategory table - PK: fld_code -> FK: categoty
            Dim query As String = "SELECT DISTINCT fld_code FROM CustomerCategory WHERE fld_code IS NOT NULL ORDER BY fld_code"
            
            cmd = New SqlCommand(query, conn)
            Dim reader As SqlDataReader = cmd.ExecuteReader()
            
            While reader.Read()
                Dim row As DataRow = dt.NewRow()
                Dim fldCode As String = If(reader("fld_code") Is DBNull.Value, "", reader("fld_code").ToString().Trim())
                
                row("fld_code") = fldCode
                row("Description") = fldCode
                row("DisplayText") = fldCode
                dt.Rows.Add(row)
            End While
            
            reader.Close()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Error loading CustomerCategory: " & ex.Message)
            ' Add a default empty row to prevent binding errors
            If dt.Rows.Count = 0 Then
                Dim emptyRow As DataRow = dt.NewRow()
                emptyRow("fld_code") = ""
                emptyRow("Description") = ""
                emptyRow("DisplayText") = "لا توجد بيانات"
                dt.Rows.Add(emptyRow)
            End If
        Finally
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing Then
                If conn.State = ConnectionState.Open Then conn.Close()
                conn.Dispose()
            End If
        End Try
        
        Return dt
    End Function


    ' Load data from AreaMaster table (if different from existing Area logic)
    Public Function LoadAreaMaster() As DataTable
        Dim dt As New DataTable()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim da As SqlDataAdapter = Nothing
        
        Try
            conn = New SqlConnection(connpath2)
            cmd = New SqlCommand("SELECT contryCode, contryCode AS DisplayText FROM AreaMaster ORDER BY contryCode", conn)
            da = New SqlDataAdapter(cmd)
            da.Fill(dt)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Error loading AreaMaster: " & ex.Message)
        Finally
            If da IsNot Nothing Then da.Dispose()
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing Then
                conn.Close()
                conn.Dispose()
            End If
        End Try
        
        Return dt
    End Function

    ' Load customer/supplier data by code including FK values from CustomerAccountsMaster
    Public Function GetCustomerSupplierByCode(customerCode As String) As CustomerSupplierData
        Dim customerData As New CustomerSupplierData()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        
        Try
            conn = New SqlConnection(connpath2)
            conn.Open()
            
            System.Diagnostics.Debug.WriteLine($"GetCustomerSupplierByCode called with code: '{customerCode}' (Length: {customerCode?.Length})")
            System.Diagnostics.Debug.WriteLine($"Connection string: {connpath2}")
            
            ' Query to get customer data with FK values from CustomerAccountsMaster
            ' Using actual column names from the database
            Dim query As String = "
                SELECT 
                    cam.code, cam.c_type, cam.name, cam.fld_arabic_name, 
                    cam.shortname, cam.bussiness_address, cam.acc_manager, cam.fld_indvl_id_no, 
                    cam.contact, cam.country, cam.area, cam.vat_tin_no, 
                    cam.email, cam.fld_contry_code_mobile, cam.fld_fax_no, cam.fld_ref_no,
                    cam.fld_indvl_id_no, cam.fld_cr_no, cam.c_type, cam.active,
                    cam.sales_man, cam.scrap_adj_code, cam.type, cam.categoty
                FROM CustomerAccountsMaster cam
                WHERE cam.code = @CustomerCode"
            
            cmd = New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@CustomerCode", customerCode.Trim())
            
            System.Diagnostics.Debug.WriteLine($"Executing query: {query}")
            System.Diagnostics.Debug.WriteLine($"Parameter @CustomerCode: '{customerCode}' (trimmed to '{customerCode.Trim()}')")
            
            Dim reader As SqlDataReader = cmd.ExecuteReader()
            System.Diagnostics.Debug.WriteLine("Query executed successfully")
            
            If reader.Read() Then
                System.Diagnostics.Debug.WriteLine($"Customer record found in database for code: {customerCode}")
                ' Map basic customer data - ensure Code is always set from parameter
                customerData.Code = customerCode.Trim() ' Use the search parameter, not the database value
                System.Diagnostics.Debug.WriteLine($"Forced Code to: '{customerData.Code}'")
                
                customerData.CustomerType = If(reader("c_type") Is DBNull.Value, "", reader("c_type").ToString())
                System.Diagnostics.Debug.WriteLine($"Mapped CustomerType: '{customerData.CustomerType}'")
                
                customerData.EnglishName = If(reader("name") Is DBNull.Value, "", reader("name").ToString())
                System.Diagnostics.Debug.WriteLine($"Mapped EnglishName: '{customerData.EnglishName}'")
                
                customerData.ArabicName = If(reader("fld_arabic_name") Is DBNull.Value, "", reader("fld_arabic_name").ToString())
                System.Diagnostics.Debug.WriteLine($"Mapped ArabicName: '{customerData.ArabicName}'")
                
                customerData.CommercialName = If(reader("shortname") Is DBNull.Value, "", reader("shortname").ToString())
                System.Diagnostics.Debug.WriteLine($"Mapped CommercialName: '{customerData.CommercialName}'")
                customerData.Address = If(reader("bussiness_address") Is DBNull.Value, "", reader("bussiness_address").ToString())
                customerData.Manager = If(reader("acc_manager") Is DBNull.Value, "", reader("acc_manager").ToString())
                customerData.ManagerID = If(reader("fld_indvl_id_no") Is DBNull.Value, "", reader("fld_indvl_id_no").ToString())
                customerData.ManagerNumber = If(reader("contact") Is DBNull.Value, "", reader("contact").ToString())
                customerData.Country = If(reader("country") Is DBNull.Value, "", reader("country").ToString())
                customerData.Area = If(reader("area") Is DBNull.Value, "", reader("area").ToString())
                customerData.VATNumber = If(reader("vat_tin_no") Is DBNull.Value, "", reader("vat_tin_no").ToString())
                customerData.Email = If(reader("email") Is DBNull.Value, "", reader("email").ToString())
                customerData.MobileCountryCode = If(reader("fld_contry_code_mobile") Is DBNull.Value, "", reader("fld_contry_code_mobile").ToString())
                customerData.FaxNumber = If(reader("fld_fax_no") Is DBNull.Value, "", reader("fld_fax_no").ToString())
                customerData.ReferralNumber = If(reader("fld_ref_no") Is DBNull.Value, "", reader("fld_ref_no").ToString())
                customerData.IndividualID = If(reader("fld_indvl_id_no") Is DBNull.Value, "", reader("fld_indvl_id_no").ToString())
                customerData.CommercialRecord = If(reader("fld_cr_no") Is DBNull.Value, "", reader("fld_cr_no").ToString())
                customerData.IdentityType = If(reader("c_type") Is DBNull.Value, "", reader("c_type").ToString())
                customerData.Active = If(reader("active") Is DBNull.Value, False, CBool(reader("active")))
                
                ' Map FK fields from CustomerAccountsMaster
                customerData.SalesMan = If(reader("sales_man") Is DBNull.Value, "", reader("sales_man").ToString())
                customerData.ScrapAdjCode = If(reader("scrap_adj_code") Is DBNull.Value, "", reader("scrap_adj_code").ToString())
                customerData.TypeCode = If(reader("type") Is DBNull.Value, "", reader("type").ToString())
                customerData.CategoryCode = If(reader("categoty") Is DBNull.Value, "", reader("categoty").ToString())
            Else
                System.Diagnostics.Debug.WriteLine($"No customer record found in database for code: {customerCode}")
            End If
            
            reader.Close()
            
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"Error loading customer/supplier '{customerCode}': {ex.Message}")
        Finally
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn IsNot Nothing Then
                If conn.State = ConnectionState.Open Then conn.Close()
                conn.Dispose()
            End If
        End Try
        
        Return customerData
    End Function

    ' Test method to verify database connectivity and customer data
    Public Function TestCustomerDataAccess() As String
        Dim result As New System.Text.StringBuilder()
        Dim conn As SqlConnection = Nothing
        
        Try
            conn = New SqlConnection(connpath2)
            conn.Open()
            result.AppendLine($"✓ Database connection successful")
            result.AppendLine($"Connection string: {connpath2}")
            
            ' Test 1: Count all records
            Dim countCmd As New SqlCommand("SELECT COUNT(*) FROM CustomerAccountsMaster", conn)
            Dim totalCount As Integer = CInt(countCmd.ExecuteScalar())
            result.AppendLine($"✓ Total records in CustomerAccountsMaster: {totalCount}")
            
            ' Test 2: Get first 5 codes
            Dim codesCmd As New SqlCommand("SELECT TOP 5 Code FROM CustomerAccountsMaster ORDER BY Code", conn)
            Dim reader As SqlDataReader = codesCmd.ExecuteReader()
            result.AppendLine("✓ First 5 customer codes:")
            While reader.Read()
                Dim code As String = If(reader("Code").ToString(), "[NULL]")
                result.AppendLine($"  - '{code}' (Length: {code.Length})")
            End While
            reader.Close()
            
            ' Test 3: Check for empty/null codes
            Dim nullCountCmd As New SqlCommand("SELECT COUNT(*) FROM CustomerAccountsMaster WHERE Code IS NULL OR Code = ''", conn)
            Dim nullCount As Integer = CInt(nullCountCmd.ExecuteScalar())
            result.AppendLine($"✓ Empty/NULL codes: {nullCount}")
            
            ' Test 4: Check specifically for C0002
            Dim c0002Cmd As New SqlCommand("SELECT Code, EnglishName, ArabicName FROM CustomerAccountsMaster WHERE Code = 'C0002'", conn)
            Dim c0002Reader As SqlDataReader = c0002Cmd.ExecuteReader()
            If c0002Reader.Read() Then
                result.AppendLine($"✓ C0002 found:")
                result.AppendLine($"  - Code: '{c0002Reader("Code")}'")
                result.AppendLine($"  - EnglishName: '{c0002Reader("EnglishName")}'")
                result.AppendLine($"  - ArabicName: '{c0002Reader("ArabicName")}'")
            Else
                result.AppendLine("✗ C0002 NOT found in database")
            End If
            c0002Reader.Close()
            
            ' Test 5: Check for codes containing '2'
            Dim codes2Cmd As New SqlCommand("SELECT Code FROM CustomerAccountsMaster WHERE Code LIKE '%2%' ORDER BY Code", conn)
            Dim codes2Reader As SqlDataReader = codes2Cmd.ExecuteReader()
            result.AppendLine("✓ Codes containing '2':")
            While codes2Reader.Read()
                result.AppendLine($"  - '{codes2Reader("Code")}'")
            End While
            codes2Reader.Close()
            
        Catch ex As Exception
            result.AppendLine($"✗ Error: {ex.Message}")
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
        
        Return result.ToString()
    End Function

    ' Quick method to check if C0002 exists
    Public Function DoesCustomerExist(customerCode As String) As Boolean
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(connpath2)
            conn.Open()
            Dim cmd As New SqlCommand("SELECT COUNT(*) FROM CustomerAccountsMaster WHERE Code = @Code", conn)
            cmd.Parameters.AddWithValue("@Code", customerCode.Trim())
            Dim count As Integer = CInt(cmd.ExecuteScalar())
            System.Diagnostics.Debug.WriteLine($"Customer '{customerCode}' exists in database: {count > 0} (Count: {count})")
            Return count > 0
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"Error checking if customer '{customerCode}' exists: {ex.Message}")
            Return False
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Function

    ' Debug method to get raw data for specific customer
    Public Function GetRawCustomerData(customerCode As String) As String
        Dim conn As SqlConnection = Nothing
        Dim result As New System.Text.StringBuilder()
        
        Try
            conn = New SqlConnection(connpath2)
            conn.Open()
            
            Dim query As String = "SELECT TOP 1 * FROM CustomerAccountsMaster WHERE Code = @Code"
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@Code", customerCode.Trim())
            
            Dim reader As SqlDataReader = cmd.ExecuteReader()
            
            If reader.Read() Then
                result.AppendLine($"=== RAW DATA for {customerCode} ===")
                For i As Integer = 0 To reader.FieldCount - 1
                    Dim fieldName As String = reader.GetName(i)
                    Dim fieldValue As Object = reader(i)
                    Dim displayValue As String = If(fieldValue Is DBNull.Value, "[NULL]", fieldValue.ToString())
                    result.AppendLine($"{fieldName}: '{displayValue}'")
                Next
                result.AppendLine("=== END RAW DATA ===")
            Else
                result.AppendLine($"No data found for customer {customerCode}")
            End If
            
            reader.Close()
            
        Catch ex As Exception
            result.AppendLine($"Error getting raw data for '{customerCode}': {ex.Message}")
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
        
        Return result.ToString()
    End Function

End Class