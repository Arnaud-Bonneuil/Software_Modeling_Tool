Imports System.Xml
Imports System.Xml.Serialization

 '=================================================================================================='
Partial Public MustInherit Class Data_Type
    Inherits Software_Element
    'Public MustOverride Function Is_Value_Valid(data_value As String) As Boolean

    Public Overrides Function Get_Parent_MetaClass() As Type
        Return GetType(Package)
    End Function

End Class


'=================================================================================================='
Partial Public Class Basic_Integer_Type
    Inherits Data_Type
End Class


'=================================================================================================='
Partial Public Class Basic_Boolean_Type
    Inherits Data_Type
End Class


'=================================================================================================='
Partial Public Class Basic_Floating_Point_Type
    Inherits Data_Type
End Class


'=================================================================================================='
Partial Public Class Enumerated_Data_Type

    Inherits Data_Type

    <XmlArrayItemAttribute(GetType(Enumerated_Data_Type_Enumeral)), XmlArray("Enumerals")>
    Public Enumerals As ArrayList

    Public Overrides Sub Post_Treat_After_Xml_Deserialization(is_read_only As Boolean)
        MyBase.Post_Treat_After_Xml_Deserialization(is_read_only)
        Dim enumeral As Enumerated_Data_Type_Enumeral
        For Each enumeral In Me.Enumerals
            enumeral.Parent = Me
            enumeral.Post_Treat_After_Xml_Deserialization(is_read_only)
        Next
    End Sub

    Public Overrides Sub Add_Element(new_child As Software_Element)
        Add_Enumeral(CType(new_child, Enumerated_Data_Type_Enumeral))
    End Sub

    Public Overrides Sub Remove_Element(old_child As Software_Element)
        Me.Enumerals.Remove(old_child)
        Me.Children.Remove(old_child)
        old_child.Parent = Nothing
    End Sub

    Public Sub Add_Enumeral(new_enumeral As Enumerated_Data_Type_Enumeral)
        If IsNothing(Me.Enumerals) Then
            Me.Enumerals = New ArrayList
        End If
        new_enumeral.Parent = Me
        Me.Enumerals.Add(new_enumeral)
        Me.Children.Add(new_enumeral)
    End Sub

End Class


'=================================================================================================='
Partial Public Class Enumerated_Data_Type_Enumeral

    Inherits Software_Element

    Public Value As UInteger

    Public Overrides Function Get_Parent_MetaClass() As Type
        Return GetType(Enumerated_Data_Type)
    End Function

    Public Function Is_Value_Valid(value As UInteger) As Boolean
        Return True
    End Function

End Class



'=================================================================================================='
Partial Public MustInherit Class Typed_Data_Type

    Inherits Data_Type

    Public Base_Data_Type_Ref As Guid

End Class


'=================================================================================================='
Partial Public Class Array_Data_Type

    Inherits Typed_Data_Type

    Public Multiplicity As UInteger

    Public Shared Function Is_Multiplicity_Valid(multiplicity As UInteger) As Boolean
        Dim result As Boolean = True
        If multiplicity = 0 Or multiplicity = 1 Then
            result = False
        End If
        Return result
    End Function

End Class


'=================================================================================================='
Partial Public Class Physical_Data_Type

    Inherits Typed_Data_Type

    Public Unit As String

    Public Resolution As Decimal

    Public Offset As Decimal

    Public Shared Function Is_Resolution_Valid(resol As Decimal) As Boolean
        Dim result As Boolean = True
        'Dim dummy As Decimal = 0
        'Decimal.TryParse(resol, _
        '                System.Globalization.NumberStyles.Any, _
        '                System.Globalization.CultureInfo.InvariantCulture, _
        '                dummy)
        'If dummy = 0 Then
        '    result = False
        'End If
        Return result
    End Function

    Public Shared Function Is_Offset_Valid(offset As Decimal) As Boolean
        Dim result As Boolean = True
        'Dim dummy As Decimal = 0
        'result = Decimal.TryParse(offset, _
        '                System.Globalization.NumberStyles.Any, _
        '                System.Globalization.CultureInfo.InvariantCulture, _
        '                dummy)
        Return result
    End Function

End Class


'=================================================================================================='
Partial Public Class Structured_Data_Type
    Inherits Data_Type

    <XmlArrayItemAttribute(GetType(Structured_Data_Type_Field)), XmlArray("Fields")>
    Public Fields As List(Of Structured_Data_Type_Field)

    Public Overrides Sub Post_Treat_After_Xml_Deserialization(is_read_only As Boolean)
        MyBase.Post_Treat_After_Xml_Deserialization(is_read_only)
        Dim field As Structured_Data_Type_Field
        For Each field In Me.Fields
            field.Parent = Me
            field.Post_Treat_After_Xml_Deserialization(is_read_only)
        Next
    End Sub

    Public Sub Add_Field(new_field As Structured_Data_Type_Field)
        If IsNothing(Me.Fields) Then
            Me.Fields = New List(Of Structured_Data_Type_Field)
        End If
        new_field.Parent = Me
        Me.Fields.Add(new_field)
        Me.Children.Add(new_field)
    End Sub

    Public Function Contains_Field(field_name As String) As Boolean
        Dim field_exist As Boolean = False
        Dim current_field As Structured_Data_Type_Field
        If Not IsNothing(Me.Fields) Then
            For Each current_field In Me.Fields
                If current_field.Name = field_name Then
                    field_exist = True
                    Exit For
                End If
            Next
        End If
        Return field_exist
    End Function

    Public Overrides Sub Add_Element(new_child As Software_Element)
        Me.Add_Field(CType(new_child, Structured_Data_Type_Field))
    End Sub

    Public Overrides Sub Remove_Element(old_child As Software_Element)
        Me.Fields.Remove(CType(old_child, Structured_Data_Type_Field))
        Me.Children.Remove(old_child)
        old_child.Parent = Nothing
    End Sub
End Class


'=================================================================================================='
Partial Public Class Structured_Data_Type_Field
    Inherits Typed_Software_Element

    Public Overrides Function Get_Parent_MetaClass() As Type
        Return GetType(Structured_Data_Type)
    End Function

End Class
