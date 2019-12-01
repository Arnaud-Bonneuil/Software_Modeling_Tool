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

    Public Overrides Sub Edit_Context_Menu_Clicked()
        ' Basic_Data_Type shall not be edited
    End Sub

    Public Overrides Sub Edition_Window_Apply_Button_Clicked(edit_win As Edition_Form)
        ' Basic_Data_Type shall not be edited
    End Sub

End Class


'=================================================================================================='
Public MustInherit Class Typed_Data_Type_Controller

    Inherits Software_Element_Controller

    Private Possible_Base_Data_Type_Ctrl As New Dictionary(Of String, Software_Element_Controller)

    Public Overrides Function Get_Element() As Software_Element
        Return Nothing
    End Function

    Public Overrides Function Get_View() As Software_Element_View
        Return Nothing
    End Function

    Public MustOverride Function Get_Typed_Element() As Typed_Data_Type

    Protected Function Get_Current_Base_Data_Type_Path() As String
        Dim element As Typed_Data_Type
        element = Get_Typed_Element()
        Dim base_data_type_path As String = "Unresolved"
        Dim ctrl_dico As Dictionary(Of Guid, Software_Element_Controller)
        ctrl_dico = Me.Get_Controller_Dico_By_Element_UUID
        If ctrl_dico.ContainsKey(element.Base_Data_Type_Ref) Then
            Dim base_data_type_ctrl As Software_Element_Controller
            base_data_type_ctrl = ctrl_dico(element.Base_Data_Type_Ref)
            base_data_type_path = base_data_type_ctrl.Get_Element.Get_Path
        End If
        Return base_data_type_path
    End Function

    Protected Sub Update_Possible_Base_Data_Type_Ref()
        ' Initialize the dictionary
        Me.Possible_Base_Data_Type_Ctrl.Clear()

        ' Get the list of controller of Data_Type
        Dim base_data_type_ctrl_list As List(Of Software_Element_Controller)
        base_data_type_ctrl_list = Get_Controllers_Of_Data_Type()
        base_data_type_ctrl_list.Remove(Me)

        ' Build the dictionary
        For Each ctrl In base_data_type_ctrl_list
            Possible_Base_Data_Type_Ctrl.Add(ctrl.Get_Element.Get_Path, ctrl)
        Next
    End Sub

    Protected Function Get_Possible_Base_Data_Type_Ctrl(data_type_path As String) As Software_Element_Controller
        If Me.Possible_Base_Data_Type_Ctrl.ContainsKey(data_type_path) Then
            Return Possible_Base_Data_Type_Ctrl(data_type_path)
        Else
            Return Nothing
        End If
    End Function

    Protected Function Get_Possible_Base_Data_Type_Path_List() As List(Of String)
        Return Me.Possible_Base_Data_Type_Ctrl.Keys.ToList
    End Function

    Public Overrides Sub Edition_Window_Apply_Button_Clicked(edit_win As Edition_Form)

        MyBase.Edition_Window_Apply_Button_Clicked(edit_win)

        Dim my_edit_window As Typed_Software_Element_Edition_Form
        my_edit_window = CType(edit_win, Typed_Software_Element_Edition_Form)

        Dim new_base_data_type_path As String = my_edit_window.Base_Type_Ref_ComboBox.Text
        If Me.Get_Possible_Base_Data_Type_Path_List.Contains(new_base_data_type_path) Then
            Dim base_data_type_ctrl As Software_Element_Controller
            base_data_type_ctrl = Me.Get_Possible_Base_Data_Type_Ctrl(new_base_data_type_path)
            Dim base_data_type_uuid = base_data_type_ctrl.Get_Element.UUID
            Get_Typed_Element.Base_Data_Type_Ref = base_data_type_uuid
        End If
    End Sub

End Class