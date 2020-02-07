Imports NetSettingsManager.Interfaces

Public Class BoolSettingNode
    Implements ISettingNode(Of Boolean)

    Public Sub New(keyName As String, settingValue As Boolean)
        Me.New(keyName, settingValue, Nothing, Nothing, Nothing, "", "")
    End Sub
    Public Sub New(keyName As String, settingValue As Boolean, defValue As Boolean, min As Boolean, max As Boolean, desc As String, title As String)
        Key = keyName : Value = settingValue : DefaultValue = defValue : MinValue = min : MaxValue = max : Me.Title = title
    End Sub

    Public Property Key As String Implements ISettingNode(Of Boolean).Key

    Public Property Value As Boolean Implements ISettingNode(Of Boolean).Value

    Public Property DefaultValue As Boolean Implements ISettingNode(Of Boolean).DefaultValue

    Public Property MinValue As Boolean Implements ISettingNode(Of Boolean).MinValue

    Public Property MaxValue As Boolean Implements ISettingNode(Of Boolean).MaxValue

    Public Property Description As String Implements ISettingNode(Of Boolean).Description

    Public Property Title As String Implements ISettingNode(Of Boolean).Title
End Class
