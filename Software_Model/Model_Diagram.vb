Imports System.Xml
Imports System.Xml.Serialization

Public Class Model_Diagram

    Inherits Software_Element

    <XmlArrayItemAttribute(GetType(Square_Model_Diagram_Element)), _
    XmlArray("Model_Diagram_Elements")>
    Public Model_Diagram_Elements As List(Of Model_Diagram_Element)

    Public Overrides Function Get_Parent_MetaClass() As Type
        Return GetType(Package)
    End Function

End Class

Public MustInherit Class Model_Diagram_Element

    Public Software_Element_Ref As Guid

End Class


Public Class Square_Model_Diagram_Element

    Inherits Model_Diagram_Element

    Public Abscissa As Integer
    Public Ordinate As Integer
    Public Width As Integer
    Public Height As Integer

End Class