Imports System.Dynamic

Namespace Converter

    Public Class DynamicConverter

        Public Shared Function Convert(Of T As Class)(ByVal source As ExpandoObject) As T
            Dim dict As IDictionary(Of String, Object) = source
            Dim ctor = GetType(T).GetConstructors().Single(Function(c) c.GetParameters().Count() > 0)
            Dim parameters = ctor.GetParameters()
            Dim parameterValues = parameters.[Select](Function(p) dict(p.Name)).ToArray()
            Return CType(ctor.Invoke(parameterValues), T)
        End Function
    End Class

End Namespace