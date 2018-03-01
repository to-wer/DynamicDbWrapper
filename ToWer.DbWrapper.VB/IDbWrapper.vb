

Public Interface IDbWrapper

    Function ReadSingle(Of T As Class)(ByVal connectionString As String, ByVal procedureName As String) As T

    Function ReadSingle(Of T As Class)(ByVal connectionString As String, ByVal procedureName As String, ByVal parameters As Dictionary(Of String, Object)) As T

    Function ReadList(Of T As Class)(ByVal connectionString As String, ByVal procedureName As String) As List(Of T)

    Function ReadList(Of T As Class)(ByVal connectionString As String, ByVal procedureName As String, ByVal parameters As Dictionary(Of String, Object)) As List(Of T)

    Sub ExecuteNonQuery(ByVal connectionString As String, ByVal procedureName As String, ByVal parameters As Dictionary(Of String, Object))

    Sub ExecuteNonQuery(Of T As Class)(ByVal connectionString As String, ByVal procedureName As String, ByVal item As T)

End Interface
