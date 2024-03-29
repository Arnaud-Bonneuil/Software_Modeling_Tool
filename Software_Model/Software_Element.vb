﻿Imports System.Xml
Imports System.Xml.Serialization
Imports System.Text.RegularExpressions



'=================================================================================================='
' Software_Element
'=================================================================================================='
Partial Public MustInherit Class Software_Element

    Public Name As String

    Public UUID As Guid

    Public Description As String

    <XmlIgnore()>
    Public Is_Read_Only As Boolean = False

    <XmlIgnore()>
    Public Parent As Software_Element = Nothing

    <XmlIgnore()>
    Public Children As List(Of Software_Element) = New List(Of Software_Element)

    Public MustOverride Function Get_Parent_MetaClass() As Type

    Public Overridable Sub Add_Element(new_child As Software_Element)
        ' Nothing to do if leaf element
    End Sub

    Public Overridable Sub Remove_Element(old_child As Software_Element)
        ' Nothing to do if leaf element
    End Sub

    Public Overridable Sub Post_Treat_After_Xml_Deserialization(is_read_only As Boolean)
        Me.Is_Read_Only = is_read_only
        Me.Parent.Children.Add(Me)
    End Sub

    Public Function Get_Path() As String
        Dim path As String = ""
        Dim sw_element As Software_Element
        sw_element = CType(Me, Software_Element)
        If Not IsNothing(sw_element.Parent) Then 'test if Me is a Top_Level_Package
            path = "/" & sw_element.Name
            sw_element = sw_element.Parent
            While Not IsNothing(sw_element.Parent)
                path = "/" & sw_element.Name & path
                sw_element = sw_element.Parent
            End While
            path = "/" & sw_element.Name & path
        Else
            path = "/" & sw_element.Name

        End If
        Return path
    End Function

    '----------------------------------------------------------------------------------------------'
    Public Sub Create_UUID()
        Me.UUID = Guid.NewGuid()
    End Sub

    '----------------------------------------------------------------------------------------------'
    ' Returns True if the given name is valid for Me
    ' A name is valid if it is a valid software symbol and that My parent does not already have a 
    ' child with this name.
    Public Overridable Function Is_Name_Valid(a_name As String) As Boolean
        Dim result As Boolean = True
        If Is_Software_Symbol_Valid(a_name) Then
            If Not IsNothing(Me.Parent.Children) Then
                Dim brother_list As New List(Of Software_Element)
                brother_list.AddRange(Me.Parent.Children)
                brother_list.Remove(Me)
                Dim brother As Software_Element
                For Each brother In brother_list
                    If a_name = brother.Name Then
                        result = False
                        Exit For
                    End If
                Next
            Else
                result = False
            End If
        Else
            result = False
        End If
        Return result
    End Function

    Public Overridable Function Is_Description_Valid(description As String) As Boolean
        Return True
    End Function

    Shared Function Is_Software_Symbol_Valid(symbol As String) As Boolean
        Dim result As Boolean = False
        If Regex.IsMatch(symbol, "^[a-zA-Z][a-zA-Z0-9_]+$") Then
            result = True
        End If
        Return result
    End Function

End Class


'=================================================================================================='
Partial Public MustInherit Class Typed_Software_Element
    Inherits Software_Element

    Public Base_Data_Type_Ref As Guid

End Class
