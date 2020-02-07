Namespace Interfaces
    Public Interface ISettingsCache(Of T)

        Property CacheValues As Boolean
        Property CachedNodes As ICollection(Of ISettingNode(Of T))
        Sub ClearCache()

    End Interface
End Namespace