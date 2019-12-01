'=================================================================================================='
Public Class Enumerated_Data_Type_Controller

    Inherits Software_Element_Controller

    Private My_Type As Enumerated_Data_Type
    Private My_View As Enumerated_Data_Type_View

    Public Overrides Function Get_Element() As Software_Element
        Return My_Type
    End Function

    Public Overrides Function Get_View() As Software_Element_View
        Return My_View
    End Function

    Public Sub New(a_type As Enumerated_Data_Type,
            parent_ctrl As Software_Element_Controller, parent_view As View)
        My_Type = a_type
        Set_Parenthood(parent_ctrl)
        My_View = New Enumerated_Data_Type_View(Me, My_Type.Name, parent_view)
        Create_Children_Controller()
    End Sub

    Public Sub Create_Children_Controller()
        If Not IsNothing(My_Type.Enumerals) Then
            Dim enumeral As Enumerated_Data_Type_Enumeral
            For Each enumeral In My_Type.Enumerals
                Dim enumeral_ctrl As Enumerated_Data_Type_Enumeral_Controller
                enumeral_ctrl = New Enumerated_Data_Type_Enumeral_Controller(enumeral, Me, My_View)
            Next
        End If
    End Sub

    Public Sub Add_Enumeral_Context_Menu_Clicked()
        ' Create new Enumeral
        Dim new_enumeral As New Enumerated_Data_Type_Enumeral
        new_enumeral.Name = "New_Enumerated_Data_Type_Enumeral"
        new_enumeral.Create_UUID()
        My_Type.Add_Enumeral(new_enumeral)

        ' Create its controller
        Dim enumeral_ctrl As Enumerated_Data_Type_Enumeral_Controller
        enumeral_ctrl = New Enumerated_Data_Type_Enumeral_Controller(new_enumeral, Me, My_View)

        Me.Set_Top_Level_Package_Controller_Status_To_Modified()
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

    Public Sub New(a_enumeral As Enumerated_Data_Type_Enumeral,
            parent_ctrl As Software_Element_Controller, parent_view As View)
        My_Enumeral = a_enumeral
        Set_Parenthood(parent_ctrl)
        My_View = New Enumerated_Data_Type_Enumeral_View(Me, My_Enumeral.Name, parent_view)
    End Sub

    Public Overrides Sub View_Element_Context_Menu_Clicked()
        Dim element As Enumerated_Data_Type_Enumeral
        element = CType(Get_Element(), Enumerated_Data_Type_Enumeral)
        Dim edit_form As New Enumerated_Data_Type_Enumeral_Edition_Form(
            Me,
            element.Name,
            element.UUID,
            element.Description,
            element.Value)
        edit_form.Set_Read_Only()
        edit_form.ShowDialog()
    End Sub

    Public Overrides Sub Edit_Context_Menu_Clicked()
        Dim element As Enumerated_Data_Type_Enumeral
        element = CType(Get_Element(), Enumerated_Data_Type_Enumeral)
        Dim edit_form As New Enumerated_Data_Type_Enumeral_Edition_Form(
            Me,
            element.Name,
            element.UUID,
            element.Description,
            element.Value)
        edit_form.ShowDialog()
    End Sub

    Public Overrides Sub Edition_Window_Apply_Button_Clicked(edit_win As Edition_Form)

        MyBase.Edition_Window_Apply_Button_Clicked(edit_win)

        Dim my_edit_window As Enumerated_Data_Type_Enumeral_Edition_Form
        my_edit_window = CType(edit_win, Enumerated_Data_Type_Enumeral_Edition_Form)

        Dim value As UInteger = CUInt(my_edit_window.Value_TextBox.Text)
        If My_Enumeral.Is_Value_Valid(value) Then
            My_Enumeral.Value = value
        Else
            My_View.Display_Value_Is_Invalid()
        End If
    End Sub

End Class

