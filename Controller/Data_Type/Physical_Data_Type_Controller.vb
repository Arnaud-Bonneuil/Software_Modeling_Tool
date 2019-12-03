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

    Public Overrides Function Create_Edition_Form() As Software_Element_Edition_Form

        ' Compute the list of possible Base_Data_Type_Ref
        Me.Update_Possible_Base_Data_Type_Ref()

        Dim edit_form As New Physical_Data_Type_Edition_Form(
            Me,
            My_Type.Name,
            My_Type.UUID,
            My_Type.Description,
            Get_Current_Base_Data_Type_Path, Me.Get_Possible_Base_Data_Type_Path_List,
            My_Type.Unit, My_Type.Resolution, My_Type.Offset)

        Return edit_form
    End Function

    Public Overrides Function Treat_Edition_Form_Data(edition_form As Edition_Form) As Boolean
        Dim phys_is_ok As Boolean = Me.Analyze_Edition_Form_Common_Data(edition_form)

        Dim my_edit_form As Physical_Data_Type_Edition_Form
        my_edit_form = CType(edition_form, Physical_Data_Type_Edition_Form)

        Dim resolution As Decimal = CDec(my_edit_form.Resolution_TextBox.Text)
        If Not Physical_Data_Type.Is_Resolution_Valid(resolution) Then
            phys_is_ok = False
            My_View.Display_Resolution_Is_Invalid()
        End If

        Dim offset As Decimal = CDec(my_edit_form.Offset_TextBox.Text)
        If Not Physical_Data_Type.Is_Offset_Valid(offset) Then
            phys_is_ok = False
            My_View.Display_Offset_Is_Invalid()
        End If

        If phys_is_ok = True Then
            Me.Apply_Edition_Form_Common_Data(edition_form)
            Me.Apply_Base_Data_Type(edition_form)
            My_Type.Unit = my_edit_form.Unit_TextBox.Text
            My_Type.Resolution = resolution
            My_Type.Offset = offset
        End If

        Return phys_is_ok
    End Function

End Class
