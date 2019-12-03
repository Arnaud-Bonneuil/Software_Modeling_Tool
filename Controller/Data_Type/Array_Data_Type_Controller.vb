'=================================================================================================='
Public Class Array_Data_Type_Controller

    Inherits Typed_Data_Type_Controller

    Private My_Array As Array_Data_Type
    Private My_View As Array_Data_Type_View

    Public Overrides Function Get_Element() As Software_Element
        Return My_Array
    End Function

    Public Overrides Function Get_Typed_Element() As Typed_Data_Type
        Return My_Array
    End Function

    Public Overrides Function Get_View() As Software_Element_View
        Return My_View
    End Function

    Public Sub New(a_type As Array_Data_Type,
            parent_ctrl As Software_Element_Controller, parent_view As View)
        My_Array = a_type
        Set_Parenthood(parent_ctrl)
        My_View = New Array_Data_Type_View(Me, My_Array.Name, parent_view)
    End Sub

    Public Overrides Function Create_Edition_Form() As Software_Element_Edition_Form

        ' Compute the list of possible Base_Data_Type_Ref
        Me.Update_Possible_Base_Data_Type_Ref()

        Dim edit_form As New Array_Data_Type_Edition_Form(
            Me,
            My_Array.Name,
            My_Array.UUID,
            My_Array.Description,
            Get_Current_Base_Data_Type_Path,
            Me.Get_Possible_Base_Data_Type_Path_List,
            My_Array.Multiplicity)

        Return edit_form
    End Function

    Public Overrides Function Treat_Edition_Form_Data(edition_form As Edition_Form) As Boolean
        Dim array_is_ok As Boolean = Me.Analyze_Edition_Form_Common_Data(edition_form)

        Dim my_edit_form As Array_Data_Type_Edition_Form
        my_edit_form = CType(edition_form, Array_Data_Type_Edition_Form)

        Dim multiplicity As UInteger = CUInt(my_edit_form.Multiplicity_TextBox.Text)
        If Not Array_Data_Type.Is_Multiplicity_Valid(multiplicity) Then
            array_is_ok = False
            My_View.Display_Multiplicity_Is_Invalid()
        End If

        If array_is_ok = True Then
            Me.Apply_Edition_Form_Common_Data(edition_form)
            Me.Apply_Base_Data_Type(edition_form)
            My_Array.Multiplicity = multiplicity
        End If

        Return array_is_ok
    End Function

End Class
