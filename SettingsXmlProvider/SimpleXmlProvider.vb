Imports System.IO
Imports System.Xml
Imports NetSettingsManager
Imports NetSettingsManager.Interfaces

Public Class SimpleXmlProvider
    Implements ISettingsProvider


    Public Sub New(path As String, Optional createFileIfNotExist As Boolean = True)
        If Not Directory.Exists(IO.Path.GetDirectoryName(path)) Then
            Directory.CreateDirectory(IO.Path.GetDirectoryName(path))
        End If
        'If createFileIfNotExist AndAlso Not IO.File.Exists(path) Then
        '    SaveChanges()
        'End If

        FilePath = path
        ProviderDescription = "A Simple XML Provider"
        ProviderName = "SimpleXml"
    End Sub


    Public Property ProviderName As String Implements ISettingsProvider.ProviderName
    Public Property ProviderDescription As String Implements ISettingsProvider.ProviderDescription

    Public ReadOnly Property FilePath As String



    Public Sub SaveNode(Of T)(node As ISettingNode(Of T)) Implements ISettingsProvider.SaveNode
        Dim existingNode As Boolean
        If IO.File.Exists(FilePath) Then
            existingNode = GetNode(Of T)(node.Key) IsNot Nothing
        Else
            Dim xml1 As New XDocument(New XDeclaration("1.0", "UTF-8", "yes"),
                               New XElement(ProviderName, New Object() {
                                                        New XAttribute("Description", ProviderDescription), New XElement("Node")}))

            Dim sw As New StringWriter()
            xml1.Save(sw)
        End If
        If existingNode Then



            Dim root As XElement = XElement.Load(FilePath)
            Dim xNode = From x In root.<Node> Where x.@Key = node.Key Select x

            xNode.Attributes("Value").First.Value = node.Value.ToString
            xNode.Attributes("Value").First.Value = node.Value.ToString
            xNode.Attributes("Value").First.Value = node.Value.ToString
            xNode.Attributes("Value").First.Value = node.Value.ToString
            xNode.Attributes("Value").First.Value = node.Value.ToString
            xNode.Attributes("Value").First.Value = node.Value.ToString
            xNode.Attributes("Value").First.Value = node.Value.ToString
            root.Save(FilePath)
        Else
            If node.Description Is Nothing Then node.Description = ""
            If node.Title Is Nothing Then node.Title = ""



            Dim xElemFile As XElement = XElement.Load(FilePath)
            Dim xElem = (From x In xElemFile.<Node>).First.Parent

            xElem.Nodes.First.AddAfterSelf(New XElement("Node", New Object() {
                                                    New XAttribute("Key", node.Key.ToString),
                                                        New XAttribute("Title", node.Title),
                                                        New XAttribute("Description", If(node.Description Is Nothing, "{null}", node.Description.ToString)),
                                                        New XAttribute("MinValue", If(node.MinValue Is Nothing, "{null}", node.MinValue.ToString)),
                                                        New XAttribute("MaxValue", If(node.MaxValue Is Nothing, "{null}", node.MaxValue.ToString)),
                                                        New XAttribute("DefaultValue", If(node.DefaultValue Is Nothing, "{null}", node.DefaultValue.ToString)),
                                                        New XAttribute("Unit", If(node.UnitString Is Nothing, "{null}", node.UnitString.ToString)),
                                                        New XAttribute("Value", If(node.Value Is Nothing, "{null}", node.Value.ToString))}))
            xElem.Save(FilePath)
        End If

    End Sub

    Public Function GetNode(Of T)(key As String) As ISettingNode(Of T) Implements ISettingsProvider.GetNode
        Dim root As XElement = XElement.Load(FilePath)
        Dim xNode = From x In root.<Node> Where x.@Key = key Select x

        Dim node As New SettingNode(Of T)
        node.Key = key
        If Not xNode.Any Then Return Nothing 'gibt es den Knoten nicht ist es das erste mal das dieser Key gepsiechert wird.
        node.Value = CType(Convert.ChangeType(xNode.Attributes("Value").First.Value, GetType(T)), T)
        If xNode.Attributes("MinValue").Any Then node.MinValue = If(xNode.Attributes("MinValue").First.Value = "{null}", Nothing, xNode.Attributes("MinValue").First.Value)
        If xNode.Attributes("MaxValue").Any Then node.MaxValue = If(xNode.Attributes("MaxValue").First.Value = "{null}", Nothing, xNode.Attributes("MaxValue").First.Value)
        If xNode.Attributes("DefaultValue").Any Then node.DefaultValue = If(xNode.Attributes("DefaultValue").First.Value = "{null}", Nothing, xNode.Attributes("DefaultValue").First.Value)
        If xNode.Attributes("Description").Any Then node.Description = If(xNode.Attributes("Description").First.Value = "{null}", Nothing, xNode.Attributes("Description").First.Value)
        If xNode.Attributes("Unit").Any Then node.UnitString = If(xNode.Attributes("Unit").First.Value = "{null}", Nothing, xNode.Attributes("Unit").First.Value)
        If xNode.Attributes("Title").Any Then node.Title = If(xNode.Attributes("Title").First.Value = "{null}", Nothing, xNode.Attributes("Title").First.Value)
        Return CType(node, ISettingNode(Of T))
    End Function



End Class
