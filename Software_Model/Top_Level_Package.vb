Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Text

Public Class Top_Level_Package
    Inherits Package

    <XmlIgnore>
    Public Xml_File_Path As String

    <XmlIgnore>
    Shared Pkg_Serializer As XmlSerializer = New XmlSerializer(GetType(Top_Level_Package))

    Public Shared Function Deserialize_Package(pkg_file_stream As FileStream) As Top_Level_Package
        Dim new_pkg As New Top_Level_Package
        new_pkg = CType(Top_Level_Package.Pkg_Serializer.Deserialize(pkg_file_stream), Top_Level_Package)
        Return new_pkg
    End Function

    Public Sub Serialize_Package(pkg_file_stream As FileStream)
        ' Initialize XML writer
        Dim writer As XmlTextWriter
        writer = New XmlTextWriter(pkg_file_stream, Encoding.UTF8)
        writer.Indentation = 2
        writer.IndentChar = " "c
        writer.Formatting = Formatting.Indented

        ' Serialize Package
        Top_Level_Package.Pkg_Serializer.Serialize(writer, Me)

        ' Close writter
        writer.Close()
    End Sub

    Public Overrides Function Is_Name_Valid(new_name As String) As Boolean
        Return Software_Element.Is_Software_Symbol_Valid(new_name)
    End Function

    Public Function Is_File_Path_Valid(path As String) As Boolean
        Return True
    End Function
End Class