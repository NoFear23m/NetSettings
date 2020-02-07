Namespace Interfaces
    Public Interface ISettingsProvider

        Property ProviderName As String
        Property ProviderDescription As String

        Sub SaveNode(Of T)(node As ISettingNode(Of T))
        Function GetNode(Of T)(key As String) As ISettingNode(Of T)

    End Interface
End Namespace