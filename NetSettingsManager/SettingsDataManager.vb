Imports NetSettingsManager.Interfaces

Public Class SettingsDataManager
    Implements ISettingsDataManager

    Public Sub New(provider As ISettingsProvider)
        Me.Provider = provider
        Cache = New SettingsCache()
    End Sub

    Public Property Provider As ISettingsProvider Implements ISettingsDataManager.Provider
    Friend Property Cache As ISettingsCache(Of String)

    Public Sub SetValue(Of T)(key As String, value As T) Implements ISettingsDataManager.SetValue
        Dim val = GetValue(Of T)(key)
        val.Value = value
        Provider.SaveNode(Of T)(val)
    End Sub

    Public Sub SetValue(Of T)(node As ISettingNode(Of T)) Implements ISettingsDataManager.SetValue
        Provider.SaveNode(Of T)(node)
    End Sub



    Public Sub SetValues(Of T)(nodes As List(Of ISettingNode(Of T))) Implements ISettingsDataManager.SetValues
        nodes.ForEach(Sub(x) Provider.SaveNode(Of T)(x))
    End Sub

    Public Function GetValue(Of T)(key As String, Optional bypassCache As Boolean = False) As ISettingNode(Of T) Implements ISettingsDataManager.GetValue
        If Not bypassCache Then
            'versuchen von Cache zu holen
            Dim sett = Cache.CachedNodes.Where(Function(x) x.Key = key).SingleOrDefault
            If sett IsNot Nothing Then Return CType(sett, ISettingNode(Of T))
        End If

        Return Provider.GetNode(Of T)(key)
    End Function

    Public Function GetValues(keys As List(Of String), Optional bypassCache As Boolean = False) As List(Of ISettingNode(Of Object)) Implements ISettingsDataManager.GetValues
        Dim retList As New List(Of ISettingNode(Of Object))
        For Each k In keys
            GetValue(Of Object)(k, bypassCache)
        Next
        Return retList
    End Function

    Public Function GetValues(ParamArray keys() As String) As List(Of ISettingNode(Of Object)) Implements ISettingsDataManager.GetValues
        Return GetValues(keys.ToList())
    End Function
End Class
