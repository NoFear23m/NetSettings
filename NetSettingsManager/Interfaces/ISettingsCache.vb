Namespace Interfaces
    Public Interface ISettingsCache(Of T)

        Sub ClearCache()
        Property MaximumCacheSize As Integer

        Sub AddOrUpdateNode(node As ISettingNode(Of T))
        Function GetNode(key As String) As ISettingNode(Of T)
        Sub RemoveNode(key As String)


    End Interface
End Namespace