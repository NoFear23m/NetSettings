Namespace Interfaces
    Public Interface ISettingNode(Of T)

        Property Key As String
        Property Value As T
        Property DefaultValue As T
        Property MinValue As T
        Property MaxValue As T
        Property Description As String
        Property Title As String
        Property UnitString As String

    End Interface
End Namespace