Imports NetSettingsManager
Imports NetSettingsManager.Interfaces
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Xml

<Assembly: InternalsVisibleTo("NetSettingsManager.Tests")>
Public Class SimpleXmlProvider
    Implements ISettingsProvider

    Private Const XmlVersion As String = "1.0"
    Private Const XmlEncoding As String = "UTF-8"
    Private Const XmlStandalone As String = "yes"


    Public Sub New(path As String, Optional createFileIfNotExists As Boolean = True)
        If Not Directory.Exists(IO.Path.GetDirectoryName(path)) Then
            Directory.CreateDirectory(IO.Path.GetDirectoryName(path))
        End If


        FilePath = path
        ProviderDescription = "A Simple XML Provider"
        ProviderName = "SimpleXml"
        If createFileIfNotExists AndAlso Not File.Exists(FilePath) Then
            CreateXml()
        End If
    End Sub


    Public Property ProviderName As String Implements ISettingsProvider.ProviderName
    Public Property ProviderDescription As String Implements ISettingsProvider.ProviderDescription

    Public ReadOnly Property FilePath As String



    Public Sub SaveNode(Of T)(node As ISettingNode(Of T)) Implements ISettingsProvider.SaveNode
        SaveNode(Of T)(node, New FileStream(FilePath, FileMode.OpenOrCreate))
    End Sub

    Public Sub RemoveNode(key As String) Implements ISettingsProvider.RemoveNode
        RemoveNode(key, New FileStream(FilePath, FileMode.OpenOrCreate))
    End Sub

    Friend Sub SaveNode(Of T)(node As ISettingNode(Of T), s As Stream)
        If ExistNode(node.Key, s) Then
            s.Position = 0
            Dim root As XElement = XElement.Load(s)
            Dim xNode = From x In root.<Node> Where x.@Key = node.Key Select x

            'xNode.Attributes("Title").First.Value = node.Value.ToString
            'xNode.Attributes("Description").First.Value = node.Description.ToString
            'xNode.Attributes("MinValue").First.Value = node.MinValue.ToString
            'xNode.Attributes("MaxValue").First.Value = node.MaxValue.ToString
            'xNode.Attributes("DefaultValue").First.Value = node.DefaultValue.ToString
            'xNode.Attributes("Unit").First.Value = node.UnitString.ToString
            'xNode.Attributes("Value").First.Value = node.Value.ToString
            FormatAttributes(Of T)(xNode, CType(node, SettingNode(Of T)))
            SaveXml(root, s)
        Else
            If node.Description Is Nothing Then node.Description = ""
            If node.Title Is Nothing Then node.Title = ""

            s.Position = 0
            Dim xElemFile As XElement = XElement.Load(s)

            Dim xElem = (From x In xElemFile.<ProviderName>).FirstOrDefault
            If xElem Is Nothing Then

                xElemFile.Add(New XElement("Node", New Object() {
                                                        New XAttribute("Key", node.Key.ToString),
                                                            New XAttribute("Title", node.Title),
                                                            New XAttribute("Description", If(node.Description Is Nothing, "{null}", node.Description.ToString)),
                                                            New XAttribute("MinValue", If(node.MinValue Is Nothing, "{null}", node.MinValue.ToString)),
                                                            New XAttribute("MaxValue", If(node.MaxValue Is Nothing, "{null}", node.MaxValue.ToString)),
                                                            New XAttribute("DefaultValue", If(node.DefaultValue Is Nothing, "{null}", node.DefaultValue.ToString)),
                                                            New XAttribute("Unit", If(node.UnitString Is Nothing, "{null}", node.UnitString.ToString)),
                                                            New XAttribute("Value", If(node.Value Is Nothing, "{null}", node.Value.ToString))}))
            Else
                xElem.Nodes.First.AddAfterSelf(New XElement("Node", New Object() {
                                                        New XAttribute("Key", node.Key.ToString),
                                                            New XAttribute("Title", node.Title),
                                                            New XAttribute("Description", If(node.Description Is Nothing, "{null}", node.Description.ToString)),
                                                            New XAttribute("MinValue", If(node.MinValue Is Nothing, "{null}", node.MinValue.ToString)),
                                                            New XAttribute("MaxValue", If(node.MaxValue Is Nothing, "{null}", node.MaxValue.ToString)),
                                                            New XAttribute("DefaultValue", If(node.DefaultValue Is Nothing, "{null}", node.DefaultValue.ToString)),
                                                            New XAttribute("Unit", If(node.UnitString Is Nothing, "{null}", node.UnitString.ToString)),
                                                            New XAttribute("Value", If(node.Value Is Nothing, "{null}", node.Value.ToString))}))
            End If

            SaveXml(xElemFile, s)

        End If
        s.Close()
        s.Dispose()
    End Sub

    Friend Sub SaveXml(elem As XElement, fs As Stream)
        fs.Position = 0
        elem.Save(fs)
    End Sub


    Public Function GetNode(Of T)(key As String) As ISettingNode(Of T) Implements ISettingsProvider.GetNode
        Return GetNode(Of T)(key, New FileStream(FilePath, FileMode.OpenOrCreate))
    End Function

    Friend Function GetNode(Of T)(key As String, s As Stream, Optional closeStream As Boolean = True) As ISettingNode(Of T)
        Dim root As XElement = XElement.Load(s)
        Dim xNode = From x In root.<Node> Where x.@Key = key Select x

        Dim node As New SettingNode(Of T)() With {.Key = key}

        If Not xNode.Any Then Return Nothing 'gibt es den Knoten nicht ist es das erste mal das dieser Key gespeichert wird.
        'node.Value = CType(Convert.ChangeType(xNode.Attributes("Value").First.Value, GetType(T)), T)
        'If xNode.Attributes("MinValue").Any Then
        '    node.MinValue = If(xNode.Attributes("MinValue").First.Value = "{null}", Nothing, CType(Convert.ChangeType(xNode.Attributes("MinValue").First.Value, GetType(T)), T))
        'End If
        'If xNode.Attributes("MaxValue").Any Then
        '    node.MaxValue = If(xNode.Attributes("MaxValue").First.Value = "{null}", Nothing, CType(Convert.ChangeType(xNode.Attributes("MaxValue").First.Value, GetType(T)), T))
        'End If
        'If xNode.Attributes("DefaultValue").Any Then
        '    node.DefaultValue = If(xNode.Attributes("DefaultValue").First.Value = "{null}", Nothing, CType(Convert.ChangeType(xNode.Attributes("DefaultValue").First.Value, GetType(T)), T))
        'End If
        'If xNode.Attributes("Description").Any Then
        '    node.Description = If(xNode.Attributes("Description").First.Value = "{null}", Nothing, xNode.Attributes("Description").First.Value)
        'End If
        'If xNode.Attributes("Unit").Any Then
        '    node.UnitString = If(xNode.Attributes("Unit").First.Value = "{null}", Nothing, xNode.Attributes("Unit").First.Value)
        'End If
        'If xNode.Attributes("Title").Any Then
        '    node.Title = If(xNode.Attributes("Title").First.Value = "{null}", Nothing, xNode.Attributes("Title").First.Value)
        'End If
        FormatAttributes(Of T)(xNode, node)

        If closeStream Then
            s.Close()
            s.Dispose()
        End If
        Return CType(node, ISettingNode(Of T))
    End Function

    Private Function ExistNode(key As String, s As Stream) As Boolean
        Return GetNode(Of Object)(key, s, False) IsNot Nothing
    End Function

    Private Sub CreateXml()
        Dim xml1 As New XDocument(New XDeclaration(XmlVersion, XmlEncoding, XmlStandalone),
                               New XElement(ProviderName, New Object() {New XAttribute("Description", ProviderDescription)}))
        xml1.Save(FilePath)
    End Sub

    Private Sub FormatAttributes(Of T)(xn As IEnumerable(Of XElement), ByRef n As SettingNode(Of T))
        n.Value = CType(Convert.ChangeType(xn.Attributes("Value").First.Value, GetType(T)), T)
        If xn.Attributes("MinValue").Any Then
            n.MinValue = If(xn.Attributes("MinValue").First.Value = "{null}", Nothing, CType(Convert.ChangeType(xn.Attributes("MinValue").First.Value, GetType(T)), T))
        End If
        If xn.Attributes("MaxValue").Any Then
            n.MaxValue = If(xn.Attributes("MaxValue").First.Value = "{null}", Nothing, CType(Convert.ChangeType(xn.Attributes("MaxValue").First.Value, GetType(T)), T))
        End If
        If xn.Attributes("DefaultValue").Any Then
            n.DefaultValue = If(xn.Attributes("DefaultValue").First.Value = "{null}", Nothing, CType(Convert.ChangeType(xn.Attributes("DefaultValue").First.Value, GetType(T)), T))
        End If
        If xn.Attributes("Description").Any Then
            n.Description = If(xn.Attributes("Description").First.Value = "{null}", Nothing, xn.Attributes("Description").First.Value)
        End If
        If xn.Attributes("Unit").Any Then
            n.UnitString = If(xn.Attributes("Unit").First.Value = "{null}", Nothing, xn.Attributes("Unit").First.Value)
        End If
        If xn.Attributes("Title").Any Then
            n.Title = If(xn.Attributes("Title").First.Value = "{null}", Nothing, xn.Attributes("Title").First.Value)
        End If
    End Sub

End Class
