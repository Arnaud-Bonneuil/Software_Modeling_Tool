'=================================================================================================='
Public Class Physical_Data_Type_Controller

    Inherits Typed_Data_Type_Controller

    Private My_Type As Physical_Data_Type
    Private My_View As Physical_Data_Type_View

    Public Overrides Function Get_Element() As Software_Element
        Return My_Type
    End Function

    Public Overrides Function Get_Typed_Element() As Typed_Data_Type
        Return My_Type
    End Function

    Public Overrides Function Get_View() As Software_Element_View
        Return My_View
    End Function

    Public Sub New(a_type As Physical_Data_Type,
           parent_ctrl As Software_Element_Controller, parent_view As View)
        My_Type = a_type
        Set_Parenthood(parent_ctrl)
        My_View = New Physical_Data_Type_View(Me, My_Type.Name, parent_view)
    End Sub

    Public Overrides Sub View_Element_Context_Menu_Clicked()
        Dim element As Physical_Data_Type
        element = CType(Get_Element(), Physical_Data_Type)
        Dim edit_form As New Physical_Data_Type_Edition_Form(
            Me,
            element.Name,
            element.UUID,
            element.Description,
            Get_Current_Base_Data_Type_Path,
            Nothing,
            element.Unit, element.Resolution, element.Offset)
        edit_form.Set_Read_Only()
        edit_form.ShowDialog()
    End Sub

    Public Overrides Sub Edit_Context_Menu_Clicked()

        Dim element As Physical_Data_Type
        element = CType(Get_Element(), Physical_Data_Type)

        ' Compute the list of possible Base_Data_Type_Ref
        Me.Update_Possible_Base_Data_Type_Ref()

        Dim edit_form As New Physical_Data_Type_Edition_Form(
            Me,
            element.Name,
            element.UUID,
            element.Description,
            Get_Current_Base_Data_Type_Path, Me.Get_Possible_Base_Data_Type_Path_List,
            element.Unit, element.Resolution, element.Offset)
        edit_form.ShowDialog()
    End Sub

    Public Overrides Sub Edition_Window_Apply_Button_Clicked(edit_win As Edition_Form)

        MyBase.Edition_Window_Apply_Button_Clicked(edit_win)

        Dim my_edit_window As Physical_Data_Type_Edition_Form
        my_edit_window = CType(edit_win, Physical_Data_Type_Edition_Form)

        My_Type.Unit = my_edit_window.Unit_TextBox.Text

        Dim resolution As Decimal = CDec(my_edit_window.Resolution_TextBox.Text)
        If Physical_Data_Type.Is_Resolution_Valid(resolution) Then
            My_Type.Resolution = resolution
        Else
            My_View.Display_Resolution_Is_Invalid()
        End If

        Dim offset As Decimal = CDec(my_edit_window.Offset_TextBox.Text)
        If Physical_Data_Type.Is_Offset_Valid(offset) Then
            My_Type.Offset = offset
        Else
            My_View.Display_Offset_Is_Invalid()
        End If



    End Sub

End Class
