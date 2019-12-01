'=================================================================================================='

'=================================================================================================='
Public MustInherit Class Software_Element_Controller

    Inherits Controller

    Public Parent_Controller As Software_Element_Controller = Nothing

    Public Children_Controller As List(Of Software_Element_Controller) = _
                                                            New List(Of Software_Element_Controller)

    Public MustOverride Function Get_Element() As Software_Element
    Public MustOverride Function Get_View() As Software_Element_View

    Public Sub Set_Parenthood(parent_ctrl As Software_Element_Controller)
        Me.Parent_Controller = parent_ctrl
        If Not IsNothing(parent_ctrl) Then
            parent_ctrl.Children_Controller.Add(Me)
        End If
        Me.Get_Controller_Dico_By_Element_UUID.Add(Get_Element.UUID, Me)
    End Sub

    Public Overridable Sub Delete_My_View()
    End Sub

    Public Overridable Sub View_Element_Context_Menu_Clicked()
        Dim element As Software_Element = Get_Element()
        Dim edit_form As New Software_Element_Edition_Form(
            Me,
            element.Name,
            element.UUID,
            element.Description)
        edit_form.Set_Read_Only()
        edit_form.ShowDialog()
    End Sub

    Public Overridable Sub View_Predefined_Element_Context_Menu_Clicked()
        Dim element As Software_Element = Get_Element()
        Dim edit_form As New Software_Element_Edition_Form(
            Me,
            element.Name,
            element.UUID,
            element.Description)
        edit_form.Set_Read_Only()
        edit_form.ShowDialog()
    End Sub

    Public Overrides Sub Edit_Context_Menu_Clicked()
        Dim element As Software_Element = Get_Element()
        Dim edit_form As New Software_Element_Edition_Form(
            Me,
            element.Name,
            element.UUID,
            element.Description)
        edit_form.ShowDialog()
    End Sub

    Public Overrides Sub Edition_Window_Apply_Button_Clicked(edit_win As Edition_Form)
        Dim element As Software_Element = Get_Element()
        Dim view As Software_Element_View = Get_View()
        Dim new_name As String = edit_win.Name_TextBox.Text
        Dim new_description As String = edit_win.Description_TextBox.Text

        ' Treat the new Name
        If element.Is_Name_Valid(new_name) Then
            element.Name = new_name
            view.Update_All_Name_Views(new_name)
        Else
            view.Display_Name_Is_Invalid()
        End If

        ' Treat the new Description
        If element.Is_Description_Valid(new_description) Then
            element.Description = new_description
            view.Update_All_Description_Views(new_description)
        Else
            view.Display_Description_Is_Invalid()
        End If

        Me.Set_Top_Level_Package_Controller_Status_To_Modified()

    End Sub

    Public Sub Move_Context_Menu_Clicked()

        Dim moved_element As Software_Element = Me.Get_Element()

        'Build the list of possible new parent controller
        Dim parent_ctrl_list As List(Of Software_Element_Controller)
        Dim parent_type As Type = moved_element.Get_Parent_MetaClass
        parent_ctrl_list = Get_Controllers_By_Element_MetaClass(parent_type)

        Dim parent_ctrl_dic As New Dictionary(Of String, Software_Element_Controller)
        Dim ctrl As Software_Element_Controller
        For Each ctrl In parent_ctrl_list
            Dim element As Software_Element = ctrl.Get_Element()
            If ctrl.Get_Element.Is_Read_Only = False Then
                parent_ctrl_dic.Add(element.Get_Path(), ctrl)
            End If
        Next
        parent_ctrl_dic.Remove(moved_element.Parent.Get_Path)

        ' Show moving window
        Dim move_form As New Move_Window(parent_ctrl_dic.Keys.ToList)
        move_form.ShowDialog()

        ' Once moving window is closed, move the element
        Dim new_parent_path As String = move_form.Get_Destination
        ' If the choosen Parent is with the list of possible
        If parent_ctrl_dic.ContainsKey(new_parent_path) Then

            ' Remove Me from my current Parent
            Me.Set_Top_Level_Package_Controller_Status_To_Modified()
            Dim view As Software_Element_View = Get_View()
            moved_element.Parent.Remove_Element(moved_element)
            Me.Parent_Controller.Children_Controller.Remove(Me)

            ' Add Me to my new Parent
            Dim new_parent_ctrl As Software_Element_Controller = parent_ctrl_dic(new_parent_path)
            Dim new_parent As Software_Element = new_parent_ctrl.Get_Element
            new_parent.Add_Element(moved_element)
            new_parent_ctrl.Set_Top_Level_Package_Controller_Status_To_Modified()

            ' Updte model browser
            view.Update_Model_Browser(Me.Parent_Controller, new_parent_ctrl)
        End If

    End Sub

    Public Sub Delete_Context_Menu_Clicked()
        ' Delete the view
        Dim view As Software_Element_View = Get_View()
        view.Delete_All_View()

        ' Delete the element and its children
        Dim element As Software_Element = Get_Element()
        element.Parent.Remove_Element(element)

        ' Delete the controller and its children
        Me.Set_Top_Level_Package_Controller_Status_To_Modified()
    End Sub

    Public Function Get_Top_Level_Package_Controller() As Top_Level_Package_Controller
        Dim top_ctrl As Top_Level_Package_Controller
        Dim parent As Software_Element_Controller = Me.Parent_Controller
        If IsNothing(parent) Then
            top_ctrl = CType(Me, Top_Level_Package_Controller)
        Else
            While Not IsNothing(parent.Parent_Controller)
                parent = parent.Parent_Controller
            End While
            top_ctrl = CType(parent, Top_Level_Package_Controller)
        End If
        Return top_ctrl
    End Function

    Public Function Get_Controller_Dico_By_Element_UUID() As  _
                                                    Dictionary(Of Guid, Software_Element_Controller)
        Dim top_ctrl As Top_Level_Package_Controller
        top_ctrl = Get_Top_Level_Package_Controller()
        Dim proj_ctrl As Software_Project_Controller
        proj_ctrl = top_ctrl.My_Project_Controller
        Return proj_ctrl.Controller_Dico_By_Element_UUID
    End Function

    Public Sub Set_Top_Level_Package_Controller_Status_To_Modified()
        Dim top_ctrl As Top_Level_Package_Controller
        top_ctrl = Me.Get_Top_Level_Package_Controller
        top_ctrl.Set_Is_Modified()
    End Sub

    Private Function Get_Controllers_By_Element_MetaClass(sw_elmt_type As Type) _
                                                             As List(Of Software_Element_Controller)
        Dim ctrl_list As New List(Of Software_Element_Controller)

        ' Get project controller
        Dim project_ctrl As Software_Project_Controller
        project_ctrl = Me.Get_Top_Level_Package_Controller.My_Project_Controller

        Dim top_pkg_ctrl As Top_Level_Package_Controller
        For Each top_pkg_ctrl In project_ctrl.My_Top_Level_Package_Controllers_List
            If sw_elmt_type = GetType(Package) Then
                ctrl_list.Add(top_pkg_ctrl)
            End If
            top_pkg_ctrl.Get_All_Controllers_By_Element_MetaClass(ctrl_list, sw_elmt_type)
        Next

        Return ctrl_list
    End Function

    Private Sub Get_All_Controllers_By_Element_MetaClass(
        ByRef ctrl_list As List(Of Software_Element_Controller),
        meta_class As Type)
        Dim child As Software_Element_Controller
        For Each child In Me.Children_Controller
            If child.Get_Element.GetType = meta_class Then
                ctrl_list.Add(child)
            End If
            child.Get_All_Controllers_By_Element_MetaClass(ctrl_list, meta_class)
        Next
    End Sub

    Public Function Get_Controllers_Of_Data_Type() As List(Of Software_Element_Controller)

        Dim ctrl_list As New List(Of Software_Element_Controller)

        ' Get project controller
        Dim project_ctrl As Software_Project_Controller
        project_ctrl = Me.Get_Top_Level_Package_Controller.My_Project_Controller

        Dim top_pkg_ctrl As Top_Level_Package_Controller
        For Each top_pkg_ctrl In project_ctrl.My_Top_Level_Package_Controllers_List
            top_pkg_ctrl.Get_All_Controllers_By_Element_MetaClass(ctrl_list, GetType(Enumerated_Data_Type))
            top_pkg_ctrl.Get_All_Controllers_By_Element_MetaClass(ctrl_list, GetType(Array_Data_Type))
            top_pkg_ctrl.Get_All_Controllers_By_Element_MetaClass(ctrl_list, GetType(Physical_Data_Type))
            top_pkg_ctrl.Get_All_Controllers_By_Element_MetaClass(ctrl_list, GetType(Structured_Data_Type))
            top_pkg_ctrl.Get_All_Controllers_By_Element_MetaClass(ctrl_list, GetType(Basic_Integer_Type))
            top_pkg_ctrl.Get_All_Controllers_By_Element_MetaClass(ctrl_list, GetType(Basic_Boolean_Type))
            top_pkg_ctrl.Get_All_Controllers_By_Element_MetaClass(ctrl_list, GetType(Basic_Floating_Point_Type))
        Next

        Return ctrl_list
    End Function

End Class



'=================================================================================================='

'=================================================================================================='
Public MustInherit Class Typed_Software_Element_Controller

    Inherits Software_Element_Controller

    Private Possible_Base_Data_Type_Ctrl As New Dictionary(Of String, Software_Element_Controller)

    Public MustOverride Function Get_Typed_Element() As Typed_Software_Element

    Protected Function Get_Current_Base_Data_Type_Path() As String
        Dim element As Typed_Software_Element
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

    Protected Sub Update_Possible_Base_Data_Type_Ref(ctrl_to_remove As Software_Element_Controller)
        ' Initialize the dictionary
        Me.Possible_Base_Data_Type_Ctrl.Clear()

        ' Get the list of controller of Data_Type
        Dim base_data_type_ctrl_list As List(Of Software_Element_Controller)
        base_data_type_ctrl_list = Get_Controllers_Of_Data_Type()
        base_data_type_ctrl_list.Remove(ctrl_to_remove)

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