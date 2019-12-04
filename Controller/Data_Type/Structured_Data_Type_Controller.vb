'=================================================================================================='
Public Class Structured_Data_Type_Controller

    Inherits Software_Element_Controller

    Private My_Struct As Structured_Data_Type
    Private My_View As Structured_Data_Type_View

    Public Overrides Function Get_Element() As Software_Element
        Return My_Struct
    End Function

    Public Overrides Function Get_View() As Software_Element_View
        Return My_View
    End Function

    Public Sub New(a_type As Structured_Data_Type,
            parent_ctrl As Software_Element_Controller, parent_view As View)
        My_Struct = a_type
        Set_Parenthood(parent_ctrl)
        My_View = New Structured_Data_Type_View(Me, My_Struct.Name, parent_view)
        Create_Children_Controller()
    End Sub

    Public Sub Create_Children_Controller()
        If Not IsNothing(My_Struct.Fields) Then
            Dim field As Structured_Data_Type_Field
            For Each field In My_Struct.Fields
                Dim field_ctrl As New Structured_Data_Type_Field_Controller(field, Me, My_View)
            Next
        End If
    End Sub

    Public Sub Add_Field_Context_Menu_Clicked()
        ' Create new Field
        Dim new_field As New Structured_Data_Type_Field
        new_field.Name = "New_Structured_Data_Type_Field"
        new_field.Create_UUID()
        My_Struct.Add_Field(new_field)

        ' Create its controller
        Dim field_ctrl As Structured_Data_Type_Field_Controller
        field_ctrl = New Structured_Data_Type_Field_Controller(new_field, Me, My_View)
        field_ctrl.Set_Is_Under_Creation()
        Dim edit_form As Software_Element_Edition_Form = field_ctrl.Create_Edition_Form()
        edit_form.ShowDialog()
    End Sub

End Class


'=================================================================================================='
Public Class Structured_Data_Type_Field_Controller

    Inherits Software_Element_Controller

    Private My_Field As Structured_Data_Type_Field
    Private My_View As Structured_Data_Type_Field_View

    Private My_Data_Type_Mngr As New Based_Data_Type_Manager

    Public Overrides Function Get_Element() As Software_Element
        Return My_Field
    End Function


    Public Overrides Function Get_View() As Software_Element_View
        Return My_View
    End Function

    Public Sub New(a_field As Structured_Data_Type_Field,
            parent_ctrl As Software_Element_Controller, parent_view As View)
        My_Field = a_field
        Set_Parenthood(parent_ctrl)
        My_View = New Structured_Data_Type_Field_View(Me, My_Field.Name, parent_view)
    End Sub

    Public Overrides Function Create_Edition_Form() As Software_Element_Edition_Form

        ' Compute the list of possible Base_Data_Type_Ref
        My_Data_Type_Mngr.Update_Possible_Base_Data_Type_Ref(Me.Parent_Controller)

        Dim edit_form As New Typed_Software_Element_Edition_Form(
            Me,
            My_Field.Name,
            My_Field.UUID,
            My_Field.Description,
            My_Data_Type_Mngr.Get_Current_Base_Data_Type_Path(Me, My_Field.Base_Data_Type_Ref),
            My_Data_Type_Mngr.Get_Possible_Base_Data_Type_Path_List)

        Return edit_form
    End Function

    Public Overrides Function Treat_Edition_Form_Data(edition_form As Edition_Form) As Boolean
        Dim field_is_ok As Boolean = Me.Analyze_Edition_Form_Common_Data(edition_form)

        Dim my_edit_form As Typed_Software_Element_Edition_Form
        my_edit_form = CType(edition_form, Typed_Software_Element_Edition_Form)

        Dim new_base_data_type_path = my_edit_form.Base_Type_Ref_ComboBox.Text
        If Not Me.My_Data_Type_Mngr.Check_Base_Data_Type_Validity(new_base_data_type_path) Then
            field_is_ok = False
            My_View.Display_Base_Data_Type_Is_Invalid()
        End If

        If field_is_ok = True Then
            Me.Apply_Edition_Form_Common_Data(edition_form)
            My_Field.Base_Data_Type_Ref = _
                My_Data_Type_Mngr.Get_New_Base_Data_Type_Ref(new_base_data_type_path)
        End If

        Return field_is_ok
    End Function

End Class
