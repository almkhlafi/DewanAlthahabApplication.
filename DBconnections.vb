Imports System.Data
Imports Microsoft.Data.SqlClient

Public Class DBconnections

    Dim connpath As String = "Data Source=ABDULRAHMAN;Initial Catalog=EmployeesDB;Integrated Security=True;TrustServerCertificate=True"
    Dim connpath2 As String = "Data Source=192.168.15.56;Initial Catalog=CMGADB2024;Persist Security Info=True;User ID=AR;Pooling=False;Multiple Active Result Sets=False;Encrypt=True;Trust Server Certificate=True;Application Name=""SQL Server Management Studio"";Command Timeout=30;Password=123456"

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

    ' =====================Get Countries from CMGADB2024 Database========================
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

    ' =====================Get Branches from CMGADB2024 Database========================
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

    ' =====================Get Currency from CMGADB2024 Database========================
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

    ' =====================Get Areas from CMGADB2024 Database========================
    Public Function GetAreas(countryCode As String) As DataTable
        Dim dt As New DataTable()
        Dim conn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim da As SqlDataAdapter = Nothing

        Try
            conn = New SqlConnection(connpath2)
            conn.Open()

            Dim query As String = "SELECT contryCode, description, shortname, areacode FROM [CMGADB2024].[dbo].[AreaMaster] WHERE contryCode = @CountryCode ORDER BY description"
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


End Class