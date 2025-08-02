Public Class Session



    '======================Reusablity==========================
    '   Set values On login:

    'Session.LoggedInUserId = reader.GetInt32(0)
    'Session.LoggedInUsername = username
    ' Session.LoggedInName = reader.GetString(1)
    '------------------------------------------------------------------------
    'Access anywhere

    'Dim name As String = session.LoggedInName
    'Dim username As String = session.LoggedInUsername
    'Dim userId As Integer = session.LoggedInUserId
    '--------------------------------------------------------------------
    'Clear session On logout:

    'Session.Clear()
    '======================END OF Reusablity==========================
    Private Shared _loggedInUserId As Integer = -1
    Private Shared _loggedInUsername As String = String.Empty
    Private Shared _loggedInName As String = String.Empty

    Public Shared Property LoggedInUserId As Integer
        Get
            Return _loggedInUserId
        End Get
        Set(value As Integer)
            _loggedInUserId = value
        End Set
    End Property

    Public Shared Property LoggedInUsername As String
        Get
            Return _loggedInUsername
        End Get
        Set(value As String)
            _loggedInUsername = value
        End Set
    End Property

    Public Shared Property LoggedInName As String
        Get
            Return _loggedInName
        End Get
        Set(value As String)
            _loggedInName = value
        End Set
    End Property

    ''' <summary>
    ''' Clears all session info — call on logout.
    ''' </summary>
    Public Shared Sub Clear()
        _loggedInUserId = -1
        _loggedInUsername = String.Empty
        _loggedInName = String.Empty
    End Sub
End Class
