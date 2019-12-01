'=================================================================================================='
Public Class Structured_Data_Type_Controller

    Inherits Software_Element_Controller

    Private My_Type As Structured_Data_Type
    Private My_View As Structured_Data_Type_View

    Public Overrides Function Get_Element() As Software_Element
        Return My_Type
    End Function

    Public Overrides Function Get_View() As Software_Element_View
        Return My_View
    End Function

    Public Sub New(a_type As Structured_Data_Type,
            parent_ctrl As Software_Element_Controller, parent_view As View)
        My_Type = a_type
        Set_Parenthood(parent_ctrl)
        My_View = New Structured_Data_Type_View(Me, My_Type.Name, parent_view)
        Create_Children_Controller()
    End Sub

    Public Sub Create_Children_Controller()
        If Not IsNothing(My_Type.Fields) Then
            Dim field As Structured_Data_Type_Field
            For Each field In My_Type.Fields
                Dim field_ctrl As New Structured_Data_Type_Field_Controller(field, Me, My_View)
            Next
        End If
    End Sub

    Public Sub Add_Field_Context_Menu_Clicked()
        ' Create new Field
        Dim new_field As New Structured_Data_Type_Field
        new_field.Name = "New_Structured_Data_Type_Field"
        new_field.Create_UUID()
        My_Type.Add_Field(new_field)

        ' Create its controller
        Dim field_ctrl As Structured_Data_Type_Field_Controller
        field_ctrl = New Structured_Data_Type_Field_Controller(new_field, Me, My_View)

        Me.Set_Top_Level_Package_Controller_Status_To_Modified()
    End Sub

End Class


'=================================================================================================='
Public Class Structured_Data_Type_Field_Controller

    Inherits Typed_Software_Element_Controller

    Private My_Field As Structured_Data_Type_Field
    Private My_View As Structured_Data_Type_Field_View

    Public Overrides Function Get_Element() As Software_Element
        Return My_Field
    End Function

    Public Overrides Function Get_Typed_Element() As Typed_Software_Element
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

    Public Overrides Sub View_Element_Context_Menu_Clicked()
        Dim element As Structured_Data_Type_Field
        element = CType(Get_Element(), Structured_Data_Type_Field)
        Dim edit_form As New Typed_Software_Element_Edition_Form(
            Me,
            element.Name,
            element.UUID,
            element.Description,
            Get_Current_Base_Data_Type_Path,
            Nothing)
        edit_form.Set_Read_Only()
        edit_form.ShowDialog()
    End Sub

    Public Overrides Sub Edit_Context_Menu_Clicked()

        Dim element As Structured_Data_Type_Field
        element = CType(Get_Element(), Structured_Data_Type_Field)

        ' Compute the list of possible Base_Data_Type_Ref
        Me.Update_Possible_Base_Data_Type_Ref(Me.Parent_Controller)

        Dim edit_form As New Typed_Software_Element_Edition_Form(
            Me,
            element.Name,
            element.UUID,
            element.Description,
            Get_Current_Base_Data_Type_Path,
            Me.Get_Possible_Base_Data_Type_Path_List)
        edit_form.ShowDialog()
    End Sub

    Public Overrides Sub Edition_Window_Apply_Button_Clicked(edit_win As Edition_Form)
        MyBase.Edition_Window_Apply_Button_Clicked(edit_win)
    End Sub

End Class
