
Imports System.Data.SqlClient
Imports System.Dynamic
Imports ToWer.DbWrapper.VB.Converter

Namespace MsSql


    Public Class MsSqlWrapper
        Implements IDbWrapper

        Public Function ReadSingle(Of T As Class)(ByVal connectionString As String, ByVal procedureName As String) As T Implements IDbWrapper.ReadSingle
            Return ReadSingle(Of T)(connectionString, procedureName, Nothing)
        End Function

        Public Function ReadSingle(Of T As Class)(ByVal connectionString As String, ByVal procedureName As String, ByVal parameters As Dictionary(Of String, Object)) As T Implements IDbWrapper.ReadSingle
            Dim result = ExecuteReader(connectionString, procedureName, parameters)
            Dim item = result.FirstOrDefault()
            If item Is Nothing Then Return Nothing
            Return DynamicConverter.Convert(Of T)(item)
        End Function

        Public Function ReadList(Of T As Class)(ByVal connectionString As String, ByVal procedureName As String) As List(Of T) Implements IDbWrapper.ReadList
            Return ReadList(Of T)(connectionString, procedureName, Nothing)
        End Function

        Public Function ReadList(Of T As Class)(ByVal connectionString As String, ByVal procedureName As String, ByVal parameters As Dictionary(Of String, Object)) As List(Of T) Implements IDbWrapper.ReadList
            Dim items = ExecuteReader(connectionString, procedureName, parameters)
            Dim result = New List(Of T)()
            For Each item In items
                result.Add(DynamicConverter.Convert(Of T)(item))
            Next

            Return result
        End Function

        Public Sub ExecuteNonQuery(ByVal connectionString As String, ByVal procedureName As String, ByVal parameters As Dictionary(Of String, Object)) Implements IDbWrapper.ExecuteNonQuery
            Using con = New SqlConnection(connectionString)
                Using cmd = New SqlCommand()
                    cmd.Connection = con
                    cmd.CommandText = procedureName
                    cmd.CommandType = System.Data.CommandType.StoredProcedure
                    If parameters IsNot Nothing Then
                        For Each param In parameters
                            cmd.Parameters.AddWithValue("@" & param.Key, param.Value)
                        Next
                    End If

                    Try
                        con.Open()
                        cmd.ExecuteNonQuery()
                    Catch
                        Throw
                    End Try
                End Using
            End Using
        End Sub

        Public Sub ExecuteNonQuery(Of T As Class)(ByVal connectionString As String, ByVal procedureName As String, ByVal item As T) Implements IDbWrapper.ExecuteNonQuery
            Throw New NotImplementedException()
        End Sub

        Private Function ExecuteReader(ByVal connectionString As String, ByVal procedureName As String, ByVal parameters As Dictionary(Of String, Object)) As List(Of Object)
            Using con = New SqlConnection(connectionString)
                Using cmd = New SqlCommand()
                    cmd.Connection = con
                    cmd.CommandText = procedureName
                    cmd.CommandType = System.Data.CommandType.StoredProcedure
                    If parameters IsNot Nothing Then
                        For Each param In parameters
                            cmd.Parameters.AddWithValue("@" & param.Key, param.Value)
                        Next
                    End If

                    Try
                        con.Open()
                        Using reader = cmd.ExecuteReader()
                            Dim result = New List(Of Object)()
                            While reader.Read()
                                Dim newItem = TryCast(New ExpandoObject(), IDictionary(Of String, Object))
                                For i As Integer = 0 To reader.FieldCount - 1
                                    Dim type = reader(i).[GetType]()
                                    newItem.Add(reader.GetName(i), If(type <> GetType(DBNull), reader(i), Nothing))
                                Next

                                result.Add(newItem)
                            End While

                            Return result
                        End Using
                    Catch
                        Throw
                    End Try
                End Using
            End Using
        End Function
    End Class

End Namespace