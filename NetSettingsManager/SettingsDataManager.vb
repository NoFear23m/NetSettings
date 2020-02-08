Imports NetSettingsManager.Interfaces

Public Class SettingsDataManager
    Implements ISettingsDataManager

    Public Sub New(provider As ISettingsProvider)
        Me.Provider = provider
        Cache = New SettingsCache()
    End Sub

    Public Property Provider As ISettingsProvider Implements ISettingsDataManager.Provider
    Friend Property Cache As ISettingsCache(Of Object)


    Public Sub SetValue(Of T)(key As String, value As T) Implements ISettingsDataManager.SetValue
        Dim val = GetValue(Of T)(key)
        val.Value = value
        SetValue(val)
    End Sub
    Public Sub SetValues(Of T)(nodes As List(Of ISettingNode(Of T))) Implements ISettingsDataManager.SetValues
        nodes.ForEach(Sub(x) SetValue(x))
    End Sub
    Public Sub SetValue(Of T)(node As ISettingNode(Of T)) Implements ISettingsDataManager.SetValue
        Provider.SaveNode(Of T)(node)
        'Im Cache auch Updaten
        Dim sv As New SettingNode(Of Object) With {.Key = node.Key, .DefaultValue = node.DefaultValue, .Description = node.Description,
            .MaxValue = node.MaxValue, .MinValue = node.MinValue, .Title = node.Title, .UnitString = node.UnitString, .Value = node.Value}

        Cache.AddOrUpdateNode(sv)

    End Sub





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
    Public Function GetValue(Of T)(key As String, Optional bypassCache As Boolean = False) As ISettingNode(Of T) Implements ISettingsDataManager.GetValue
        If Not bypassCache Then
            'versuchen von Cache zu holen
            Dim sett = Cache.GetNode(key)
            If sett IsNot Nothing Then
                Dim rn As New SettingNode(Of T)
                rn.Key = sett.Key
                rn.Description = sett.Description
                rn.DefaultValue = CType(Convert.ChangeType(sett.DefaultValue, GetType(T)), T)
                rn.MaxValue = CType(Convert.ChangeType(sett.MaxValue, GetType(T)), T)
                rn.MinValue = CType(Convert.ChangeType(sett.MinValue, GetType(T)), T)
                rn.Title = sett.Title
                rn.UnitString = sett.UnitString
                rn.Value = CType(Convert.ChangeType(sett.Value, GetType(T)), T)

                Return rn
            End If
        End If

        Return Provider.GetNode(Of T)(key)
    End Function


End Class
