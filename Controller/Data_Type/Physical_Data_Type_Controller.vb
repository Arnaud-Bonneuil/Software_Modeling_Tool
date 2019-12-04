'=================================================================================================='
Public Class Physical_Data_Type_Controller

    Inherits Software_Element_Controller

    Private My_Type As Physical_Data_Type
    Private My_View As Physical_Data_Type_View

    Private My_Data_Type_Mngr As New Based_Data_Type_Manager

    Public Overrides Function Get_Element() As Software_Element
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
        My_Data_Type_Mngr.Update_Possible_Base_Data_Type_Ref(Me)

        Dim edit_form As New Physical_Data_Type_Edition_Form(
            Me,
            My_Type.Name,
            My_Type.UUID,
            My_Type.Description,
            My_Data_Type_Mngr.Get_Current_Base_Data_Type_Path(Me, My_Type.Base_Data_Type_Ref),
            My_Data_Type_Mngr.Get_Possible_Base_Data_Type_Path_List,
            My_Type.Unit, My_Type.Resolution, My_Type.Offset)

        Return edit_form
    End Function

    Public Overrides Function Treat_Edition_Form_Data(edition_form As Edition_Form) As Boolean
        Dim phys_is_ok As Boolean = Me.Analyze_Edition_Form_Common_Data(edition_form)

        Dim my_edit_form As Physical_Data_Type_Edition_Form
        my_edit_form = CType(edition_form, Physical_Data_Type_Edition_Form)

        Dim new_base_data_type_path = my_edit_form.Base_Type_Ref_ComboBox.Text
        If Not Me.My_Data_Type_Mngr.Check_Base_Data_Type_Validity(new_base_data_type_path) Then
            phys_is_ok = False
            My_View.Display_Base_Data_Type_Is_Invalid()
        End If

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
            My_Type.Base_Data_Type_Ref = _
                My_Data_Type_Mngr.Get_New_Base_Data_Type_Ref(new_base_data_type_path)
            My_Type.Unit = my_edit_form.Unit_TextBox.Text
            My_Type.Resolution = resolution
            My_Type.Offset = offset
        End If

        Return phys_is_ok
    End Function

End Class
