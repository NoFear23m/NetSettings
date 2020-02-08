Imports NetSettingsManager.Interfaces

Public Class SettingNode(Of T)
    Implements Interfaces.ISettingNode(Of T)

    Public Sub New()
    End Sub
    Public Sub New(keyName As String, settingValue As T)
        Me.New(keyName, settingValue, Nothing, Nothing, Nothing, "", "")
    End Sub
    Public Sub New(keyName As String, settingValue As T, defValue As T, min As T, max As T, desc As String, title As String)
        Key = keyName : Value = settingValue : DefaultValue = defValue : MinValue = min : MaxValue = max : Me.Title = title
        Description = desc
    End Sub

    Public Property Key As String Implements ISettingNode(Of T).Key
    Public Property Value As T Implements ISettingNode(Of T).Value
    Public Property DefaultValue As T Implements ISettingNode(Of T).DefaultValue
    Public Property MinValue As T Implements ISettingNode(Of T).MinValue
    Public Property MaxValue As T Implements ISettingNode(Of T).MaxValue
    Public Property Description As String Implements ISettingNode(Of T).Description
    Public Property Title As String Implements ISettingNode(Of T).Title
    Public Property UnitString As String Implements ISettingNode(Of T).UnitString

End Class
