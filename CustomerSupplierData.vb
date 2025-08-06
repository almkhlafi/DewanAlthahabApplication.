' Data model class for Customer/Supplier information
Public Class CustomerSupplierData
    
    Public Property Code As String = ""
    Public Property CustomerType As String = "" ' Customer or Supplier
    Public Property EnglishName As String = ""
    Public Property ArabicName As String = ""
    Public Property CommercialName As String = ""
    Public Property Address As String = ""
    Public Property Manager As String = ""
    Public Property ManagerID As String = ""
    Public Property ManagerNumber As String = ""
    Public Property Country As String = ""
    Public Property Area As String = ""
    Public Property VATNumber As String = ""
    Public Property Email As String = ""
    Public Property MobileCountryCode As String = ""
    Public Property CountryName As String = ""
    Public Property FaxNumber As String = ""
    Public Property ReferralNumber As String = ""
    Public Property IndividualID As String = ""
    Public Property CommercialRecord As String = ""
    Public Property IdentityType As String = "" ' "فردي" or "تجاري"
    Public Property Active As Boolean = False ' Active status for VTRAppliedCKB
    
    ' Foreign Key fields for CustomerAccountsMaster
    Public Property SalesMan As String = "" ' FK from CusTransactionMaster.fld_area_code
    Public Property ScrapAdjCode As String = "" ' FK from CusGradeMaster.code
    Public Property TypeCode As String = "" ' FK from CustomerType.fld_code
    Public Property CategoryCode As String = "" ' FK from CustomerCategory.fld_code
    
    ' Control properties
    Public Property IsUpdate As Boolean = False
    Public Property ExistingCode As String = ""
    
    ' Constructor
    Public Sub New()
        ' Default values
    End Sub
    
    ' Helper method to check if this is a customer (vs supplier)
    Public ReadOnly Property IsCustomer As Boolean
        Get
            Return CustomerType = "Customer" OrElse Code.StartsWith("C")
        End Get
    End Property
    
    ' Helper method to check if this is a supplier (vs customer)
    Public ReadOnly Property IsSupplier As Boolean
        Get
            Return CustomerType = "Supplier" OrElse Code.StartsWith("S")
        End Get
    End Property
    
    ' Helper method to check if identity type is Individual
    Public ReadOnly Property IsIndividual As Boolean
        Get
            Return IdentityType = "فردي"
        End Get
    End Property
    
    ' Helper method to check if identity type is Commercial
    Public ReadOnly Property IsCommercial As Boolean
        Get
            Return IdentityType = "تجاري"
        End Get
    End Property
    
End Class