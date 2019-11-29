Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Text

Public Class Software_Project

    Public Name As String

    Public Description As String

    <XmlArrayItemAttribute(GetType(Software_Package_Reference)), XmlArray("Packages_References")>
    Public Packages_References As List(Of Software_Package_Reference)

    <XmlIgnore()>
    Public Tol_Level_Packages As List(Of Top_Level_Package)

    <XmlIgnore()>
    Shared Project_Data_Serializer As New XmlSerializer(GetType(Software_Project))


    Public Shared Function Deserialize_Project(prj_file_stream As FileStream) As Software_Project
        Dim new_sw_proj As Software_Project
        new_sw_proj = CType(Project_Data_Serializer.Deserialize(prj_file_stream), Software_Project)
        Return new_sw_proj
    End Function

    Public Sub Serialize_Project(prj_file_stream As FileStream)
        ' Initialize XML writer
        Dim writer As XmlTextWriter
        writer = New XmlTextWriter(prj_file_stream, Encoding.UTF8)
        writer.Indentation = 4
        writer.IndentChar = " "c
        writer.Formatting = Formatting.Indented

        ' Serialize Package
        Software_Project.Project_Data_Serializer.Serialize(writer, Me)

        ' Close writter
        writer.Close()
    End Sub

    Public Sub Load_Packages(
            ByRef not_found_pkg_file_list As List(Of String),
            ByRef invalid_pkg_file_list As List(Of String))

        '------------------------------------------------------------------------------------------'
        ' Initialize the Model_Container
        Tol_Level_Packages = New List(Of Top_Level_Package)

        '------------------------------------------------------------------------------------------'
        ' Load Packages
        Load_Basic_Types_Package()
        ' Load project's packages
        Dim pkg_xml_file_stream As FileStream
        Dim new_pkg As Top_Level_Package
        If Me.Packages_References.Count > 0 Then
            Dim pkg_ref As Software_Package_Reference
            For Each pkg_ref In Me.Packages_References
                If File.Exists(pkg_ref.Path) Then
                    ' Deserialize the Package
                    pkg_xml_file_stream = New FileStream(pkg_ref.Path, FileMode.Open)
                    new_pkg = Top_Level_Package.Deserialize_Package(pkg_xml_file_stream)
                    new_pkg.Xml_File_Path = pkg_ref.Path

                    ' Link the package to the container
                    Tol_Level_Packages.Add(new_pkg)
                    new_pkg.Parent = Nothing ' by definition
                    new_pkg.Post_Treat_After_Xml_Deserialization(False)

                    pkg_xml_file_stream.Close()
                Else
                    not_found_pkg_file_list.Add(pkg_ref.Path)
                End If
            Next
        End If
    End Sub

    Private Sub Load_Basic_Types_Package()
        Dim pkg_xml_file_stream As FileStream
        Dim new_pkg As Top_Level_Package
        pkg_xml_file_stream = New FileStream("Basic_Types.xml", FileMode.Open)
        new_pkg = Top_Level_Package.Deserialize_Package(pkg_xml_file_stream)
        Tol_Level_Packages.Add(new_pkg)
        new_pkg.Parent = Nothing ' by definition
        new_pkg.Post_Treat_After_Xml_Deserialization(True)
        pkg_xml_file_stream.Close()
    End Sub

    Function Is_Name_Valid(new_name As String) As Boolean
        Return Software_Element.Is_Software_Symbol_Valid(new_name)
    End Function

    Function Is_Description_Valid(new_description As String) As Boolean
        Return True
    End Function

    Sub Remove_Package_Ref(xml_file_path As String)
        Dim pkg_ref As Software_Package_Reference
        For Each pkg_ref In Me.Packages_References
            If pkg_ref.Path = xml_file_path Then
                Packages_References.Remove(pkg_ref)
                Exit Sub
            End If
        Next
    End Sub

End Class


Partial Public Class Software_Package_Reference

    <XmlAttribute(DataType:="string")>
    Public Path As String

End Class