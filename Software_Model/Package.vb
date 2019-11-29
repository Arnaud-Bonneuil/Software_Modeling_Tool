Imports System.Xml
Imports System.Xml.Serialization


Public Class Package

    Inherits Software_Element

    <XmlArrayItemAttribute(GetType(Package)), XmlArray("Packages")>
    Public Packages As List(Of Package)

    <XmlArrayItemAttribute(GetType(Enumerated_Data_Type)), _
    XmlArrayItemAttribute(GetType(Array_Data_Type)), _
    XmlArrayItemAttribute(GetType(Physical_Data_Type)), _
    XmlArrayItemAttribute(GetType(Structured_Data_Type)), _
    XmlArrayItemAttribute(GetType(Basic_Integer_Type)), _
    XmlArrayItemAttribute(GetType(Basic_Boolean_Type)), _
    XmlArrayItemAttribute(GetType(Basic_Floating_Point_Type)), _
    XmlArray("Data_Types")>
    Public Data_Types As List(Of Data_Type)

    <XmlArrayItemAttribute(GetType(Model_Diagram)), XmlArray("Model_Diagrams")>
    Public Model_Diagrams As List(Of Model_Diagram)


    Public Overrides Function Get_Parent_MetaClass() As Type
        Return GetType(Package)
    End Function

    Public Overrides Sub Add_Element(new_child As Software_Element)
        Select Case new_child.GetType
            Case GetType(Package)
                Add_Package(CType(new_child, Package))
            Case GetType(Model_Diagram)
                Add_Diagram(CType(new_child, Model_Diagram))
            Case GetType(Enumerated_Data_Type), GetType(Array_Data_Type), _
                 GetType(Physical_Data_Type), GetType(Structured_Data_Type)
                Add_Data_Type(CType(new_child, Data_Type))
        End Select
    End Sub

    Public Overrides Sub Remove_Element(old_child As Software_Element)
        Select Case old_child.GetType
            Case GetType(Package)
                Me.Packages.Remove(CType(old_child, Package))
            Case GetType(Model_Diagram)
                Me.Model_Diagrams.Remove(CType(old_child, Model_Diagram))
            Case GetType(Enumerated_Data_Type), GetType(Array_Data_Type), _
                 GetType(Physical_Data_Type), GetType(Structured_Data_Type)
                Me.Data_Types.Remove(CType(old_child, Data_Type))
        End Select
        Me.Children.Remove(old_child)
        old_child.Parent = Nothing
    End Sub

    Public Overrides Sub Post_Treat_After_Xml_Deserialization(is_read_only As Boolean)
        Me.Is_Read_Only = is_read_only
        Me.Add_To_Project_Elements_List()

        If Not IsNothing(Me.Packages) Then
            Dim pkg As Package
            For Each pkg In Me.Packages
                pkg.Parent = Me
                pkg.Post_Treat_After_Xml_Deserialization(is_read_only)
            Next
        End If

        If Not IsNothing(Me.Model_Diagrams) Then
            Dim diag As Model_Diagram
            For Each diag In Me.Model_Diagrams
                diag.Parent = Me
                diag.Post_Treat_After_Xml_Deserialization(is_read_only)
            Next
        End If

        If Not IsNothing(Me.Data_Types) Then
            Dim type As Data_Type
            For Each type In Me.Data_Types
                type.Parent = Me
                type.Post_Treat_After_Xml_Deserialization(is_read_only)
            Next
        End If

    End Sub

    Public Sub Add_Package(new_package As Package)
        If IsNothing(Me.Packages) Then
            Me.Packages = New List(Of Package)
        End If
        new_package.Parent = Me
        Me.Packages.Add(new_package)
        Me.Children.Add(new_package)
    End Sub

    Public Sub Add_Diagram(new_diagram As Model_Diagram)
        If IsNothing(Me.Model_Diagrams) Then
            Me.Model_Diagrams = New List(Of Model_Diagram)
        End If
        new_diagram.Parent = Me
        Me.Model_Diagrams.Add(new_diagram)
        Me.Children.Add(new_diagram)
    End Sub

    Public Sub Add_Data_Type(new_data_type As Data_Type)
        If IsNothing(Me.Data_Types) Then
            Me.Data_Types = New List(Of Data_Type)
        End If
        new_data_type.Parent = Me
        Me.Data_Types.Add(new_data_type)
        Me.Children.Add(new_data_type)
    End Sub

End Class

