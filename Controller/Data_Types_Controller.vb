'=================================================================================================='
Public MustInherit Class Data_Types_Controller
    Inherits Software_Element_Controller
End Class


'=================================================================================================='
Public Class Basic_Data_Type_Controller

    Inherits Data_Types_Controller

    Private My_Type As Data_Type
    Private My_View As Basic_Data_Type_View

    Public Overrides Function Get_Element() As Software_Element
        Return My_Type
    End Function

    Public Overrides Function Get_View() As Software_Element_View
        Return My_View
    End Function

    Public Sub New(a_type As Data_Type, parent_view As View)
        My_Type = a_type
        My_View = New Basic_Data_Type_View(Me, My_Type.Name, parent_view)
    End Sub

    Public Overrides Sub Edit_Context_Menu_Clicked()
        ' Basic_Data_Type shall not be edited
    End Sub

    Public Overrides Sub Edition_Window_Apply_Button_Clicked(edit_win As Edition_Form)
        ' Basic_Data_Type shall not be edited
    End Sub

End Class


'=================================================================================================='
Public Class Enumerated_Data_Type_Controller

    Inherits Data_Types_Controller

    Private My_Type As Enumerated_Data_Type
    Private My_View As Enumerated_Data_Type_View

    Public Overrides Function Get_Element() As Software_Element
        Return My_Type
    End Function

    Public Overrides Function Get_View() As Software_Element_View
        Return My_View
    End Function

    Public Sub New(a_type As Enumerated_Data_Type, parent_view As View)
        My_Type = a_type
        My_View = New Enumerated_Data_Type_View(Me, My_Type.Name, parent_view)
        Create_Children_Controller()
    End Sub

    Public Sub Create_Children_Controller()
        If Not IsNothing(My_Type.Enumerals) Then
            Dim enumeral As Enumerated_Data_Type_Enumeral
            For Each enumeral In My_Type.Enumerals
                Dim enumeral_ctrl As New Enumerated_Data_Type_Enumeral_Controller(enumeral, My_View)
                enumeral_ctrl.Parent_Controller = Me
                Me.Children_Controller.Add(enumeral_ctrl)
            Next
        End If
    End Sub

End Class


'=================================================================================================='
Public Class Enumerated_Data_Type_Enumeral_Controller

    Inherits Software_Element_Controller

    Private My_Enumeral As Enumerated_Data_Type_Enumeral
    Private My_View As Enumerated_Data_Type_Enumeral_View

    Public Overrides Function Get_Element() As Software_Element
        Return My_Enumeral
    End Function

    Public Overrides Function Get_View() As Software_Element_View
        Return My_View
    End Function

    Public Sub New(a_enumeral As Enumerated_Data_Type_Enumeral, parent_view As View)
        My_Enumeral = a_enumeral
        My_View = New Enumerated_Data_Type_Enumeral_View(Me, My_Enumeral.Name, parent_view)
    End Sub

    Public Overrides Sub View_Element_Context_Menu_Clicked()
        My_View.Display_Element(
            My_Enumeral.Name,
            My_Enumeral.UUID,
            My_Enumeral.Description,
            My_Enumeral.Value)
    End Sub

End Class


'=================================================================================================='
Public Class Array_Data_Type_Controller

    Inherits Data_Types_Controller

    Private My_Type As Array_Data_Type
    Private My_View As Array_Data_Type_View

    Public Overrides Function Get_Element() As Software_Element
        Return My_Type
    End Function

    Public Overrides Function Get_View() As Software_Element_View
        Return My_View
    End Function

    Public Sub New(a_type As Array_Data_Type, parent_view As View)
        My_Type = a_type
        My_View = New Array_Data_Type_View(Me, My_Type.Name, parent_view)
    End Sub

End Class


'=================================================================================================='
Public Class Physical_Data_Type_Controller

    Inherits Data_Types_Controller

    Private My_Type As Physical_Data_Type
    Private My_View As Physical_Data_Type_View

    Public Overrides Function Get_Element() As Software_Element
        Return My_Type
    End Function

    Public Overrides Function Get_View() As Software_Element_View
        Return My_View
    End Function

    Public Sub New(a_type As Physical_Data_Type, parent_view As View)
        My_Type = a_type
        My_View = New Physical_Data_Type_View(Me, My_Type.Name, parent_view)
    End Sub

End Class


'=================================================================================================='
Public Class Structured_Data_Type_Controller

    Inherits Data_Types_Controller

    Private My_Type As Structured_Data_Type
    Private My_View As Structured_Data_Type_View

    Public Overrides Function Get_Element() As Software_Element
        Return My_Type
    End Function

    Public Overrides Function Get_View() As Software_Element_View
        Return My_View
    End Function

    Public Sub New(a_type As Structured_Data_Type, parent_view As View)
        My_Type = a_type
        My_View = New Structured_Data_Type_View(Me, My_Type.Name, parent_view)
        Create_Children_Controller()
    End Sub

    Public Sub Create_Children_Controller()
        If Not IsNothing(My_Type.Fields) Then
            Dim field As Structured_Data_Type_Field
            For Each field In My_Type.Fields
                Dim field_ctrl As New Structured_Data_Type_Field_Controller(field, My_View)
                field_ctrl.Parent_Controller = Me
                Me.Children_Controller.Add(field_ctrl)
            Next
        End If
    End Sub

End Class


'=================================================================================================='
Public Class Structured_Data_Type_Field_Controller

    Inherits Software_Element_Controller

    Private My_Field As Structured_Data_Type_Field
    Private My_View As Structured_Data_Type_Field_View

    Public Overrides Function Get_Element() As Software_Element
        Return My_Field
    End Function

    Public Overrides Function Get_View() As Software_Element_View
        Return My_View
    End Function

    Public Sub New(a_field As Structured_Data_Type_Field, parent_view As View)
        My_Field = a_field
        My_View = New Structured_Data_Type_Field_View(Me, My_Field.Name, parent_view)
    End Sub

    Public Overrides Sub View_Element_Context_Menu_Clicked()
        My_View.Display_Element(
            My_Field.Name,
            My_Field.UUID,
            My_Field.Description,
            "")
    End Sub

End Class
