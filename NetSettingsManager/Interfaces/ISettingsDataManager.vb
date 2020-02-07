Namespace Interfaces
    Public Interface ISettingsDataManager

        Property Provider As ISettingsProvider
        Function GetValue(Of T)(key As String, Optional bypassCache As Boolean = False) As ISettingNode(Of T)
        Function GetValues(keys As List(Of String), Optional bypassCache As Boolean = False) As List(Of ISettingNode(Of Object))
        Function GetValues(ParamArray keys() As String) As List(Of ISettingNode(Of Object))
        Sub SetValue(Of T)(key As String, value As T)
        Sub SetValue(Of T)(node As ISettingNode(Of T))
        'Sub SetValues(keys As Tuple(Of String, Object))
        Sub SetValues(Of T)(nodes As List(Of ISettingNode(Of T)))

    End Interface
End Namespace