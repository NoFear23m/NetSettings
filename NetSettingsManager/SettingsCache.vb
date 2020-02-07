Imports NetSettingsManager.Interfaces

Public Class SettingsCache
    Implements ISettingsCache(Of String)

    Public Sub New()
        CachedNodes = New List(Of ISettingNode(Of String))
    End Sub

    Public Property CacheValues As Boolean Implements ISettingsCache(Of String).CacheValues

    Public Property CachedNodes As ICollection(Of ISettingNode(Of String)) Implements ISettingsCache(Of String).CachedNodes


    Public Sub ClearCache() Implements ISettingsCache(Of String).ClearCache
        CachedNodes.Clear()
    End Sub


End Class
