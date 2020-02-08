Imports NetSettingsManager.Interfaces

Public Class SettingsCache
    Implements ISettingsCache(Of Object)

    Public Sub New()
        CachedNodes = New List(Of ISettingNode(Of Object))
        MaximumCacheSize = Integer.MaxValue
    End Sub

    Private ReadOnly Property CachedNodes As ICollection(Of ISettingNode(Of Object))

    Public Property MaximumCacheSize As Integer Implements ISettingsCache(Of Object).MaximumCacheSize

    Public Sub ClearCache() Implements ISettingsCache(Of Object).ClearCache
        CachedNodes.Clear()
    End Sub

    Public Sub AddOrUpdateNode(node As ISettingNode(Of Object)) Implements ISettingsCache(Of Object).AddOrUpdateNode
        If CachedNodes.Where(Function(x) x.Key = node.Key).Any Then
            Dim fNode = CachedNodes.Where(Function(x) x.Key = node.Key).Single
            With fNode
                .DefaultValue = node.DefaultValue
                .Description = node.Description
                .Key = node.Key
                .MaxValue = node.MaxValue
                .MinValue = node.MinValue
                .UnitString = node.UnitString
                .Value = node.Value
            End With
        Else
            CachedNodes.Add(node)
        End If
    End Sub

    Public Sub RemoveNode(key As String) Implements ISettingsCache(Of Object).RemoveNode
        CachedNodes.Remove(CachedNodes.Where(Function(x) x.Key = key).SingleOrDefault)
    End Sub

    Public Function GetNode(key As String) As ISettingNode(Of Object) Implements ISettingsCache(Of Object).GetNode
        Return CachedNodes.Where(Function(x) x.Key = key).SingleOrDefault
    End Function
End Class
