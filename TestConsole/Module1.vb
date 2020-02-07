Module Module1

    Sub Main()
        Dim xmlPath = My.Application.Info.DirectoryPath & "\test1.xml"
        Dim dm As New NetSettingsManager.SettingsDataManager(New SettingsXmlProvider.SimpleXmlProvider(xmlPath))
        dm.SetValue(Of String)(New NetSettingsManager.SettingNode(Of String)("TestKey1", "TestÜValue1", "Hallo", "min", "max", "Beschreibung", "Titel"))
        dm.SetValue(Of String)(New NetSettingsManager.SettingNode(Of String)("TestKey2", "TestValue2"))
        dm.SetValue(Of String)(New NetSettingsManager.SettingNode(Of String)("TestKey3", "TestValue2"))
        dm.SetValue(Of String)(New NetSettingsManager.SettingNode(Of String)("TestKey4", "TestValue2"))
        dm.SetValue(Of String)(New NetSettingsManager.SettingNode(Of String)("TestKey5", "TestValue2"))
        dm.SetValue(Of String)(New NetSettingsManager.SettingNode(Of String)("TestKey6", "TestValue2"))
        dm.SetValue(Of String)(New NetSettingsManager.SettingNode(Of String)("TestKey7", "TestValue2"))
        dm.SetValue(Of String)(New NetSettingsManager.SettingNode(Of String)("TestKey2", "TestValue2Neu"))
        dm.SetValue(New NetSettingsManager.SettingNode(Of Boolean)("MyBoolTest1", True))


        Dim val = dm.GetValue(Of String)("TestKey1")
        Dim b = dm.GetValue(Of Boolean)("MyBoolTest1")
    End Sub

End Module
