'=================================================================================================='
Public Class Basic_Data_Type_Controller

    Inherits Software_Element_Controller

    Private My_Type As Data_Type
    Private My_View As Basic_Data_Type_View

    Public Overrides Function Get_Element() As Software_Element
        Return My_Type
    End Function

    Public Overrides Function Get_View() As Software_Element_View
        Return My_View
    End Function

    Public Sub New(a_type As Data_Type,
            parent_ctrl As Software_Element_Controller, parent_view As View)
        My_Type = a_type
        Set_Parenthood(parent_ctrl)
        My_View = New Basic_Data_Type_View(Me, My_Type.Name, parent_view)
    End Sub

    Public Overrides Function Treat_Edition_Form_Data(edition_form As Edition_Form) As Boolean
        ' A Basic_Data_Type shall never be edited
        Return False
    End Function

End Class