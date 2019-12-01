'=================================================================================================='
Public Class Array_Data_Type_Controller

    Inherits Typed_Data_Type_Controller

    Private My_Type As Array_Data_Type
    Private My_View As Array_Data_Type_View

    Public Overrides Function Get_Element() As Software_Element
        Return My_Type
    End Function

    Public Overrides Function Get_Typed_Element() As Typed_Data_Type
        Return My_Type
    End Function

    Public Overrides Function Get_View() As Software_Element_View
        Return My_View
    End Function

    Public Sub New(a_type As Array_Data_Type,
            parent_ctrl As Software_Element_Controller, parent_view As View)
        My_Type = a_type
        Set_Parenthood(parent_ctrl)
        My_View = New Array_Data_Type_View(Me, My_Type.Name, parent_view)
    End Sub

    Public Overrides Sub View_Element_Context_Menu_Clicked()
        Dim element As Array_Data_Type
        element = CType(Get_Element(), Array_Data_Type)
        Dim edit_form As New Array_Data_Type_Edition_Form(
            Me,
            element.Name,
            element.UUID,
            element.Description,
            Get_Current_Base_Data_Type_Path,
            Nothing,
            element.Multiplicity)
        edit_form.Set_Read_Only()
        edit_form.ShowDialog()
    End Sub

    Public Overrides Sub Edit_Context_Menu_Clicked()

        Dim element As Array_Data_Type
        element = CType(Get_Element(), Array_Data_Type)

        ' Compute the list of possible Base_Data_Type_Ref
        Me.Update_Possible_Base_Data_Type_Ref()

        Dim edit_form As New Array_Data_Type_Edition_Form(
            Me,
            element.Name,
            element.UUID,
            element.Description,
            Get_Current_Base_Data_Type_Path,
            Me.Get_Possible_Base_Data_Type_Path_List,
            element.Multiplicity)
        edit_form.ShowDialog()
    End Sub

    Public Overrides Sub Edition_Window_Apply_Button_Clicked(edit_win As Edition_Form)

        MyBase.Edition_Window_Apply_Button_Clicked(edit_win)

        Dim my_edit_window As Array_Data_Type_Edition_Form
        my_edit_window = CType(edit_win, Array_Data_Type_Edition_Form)

        Dim multiplicity As UInteger = CUInt(my_edit_window.Multiplicity_TextBox.Text)
        If Array_Data_Type.Is_Multiplicity_Valid(multiplicity) Then
            My_Type.Multiplicity = multiplicity
        Else
            My_View.Display_Multiplicity_Is_Invalid()
        End If

    End Sub

End Class

