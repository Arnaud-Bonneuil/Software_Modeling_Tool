'=================================================================================================='

'=================================================================================================='
Public MustInherit Class Software_Element_Controller

    Inherits Controller

    Public Parent_Controller As Software_Element_Controller = Nothing

    Public Children_Controller As List(Of Software_Element_Controller) = _
                                                            New List(Of Software_Element_Controller)

    Public MustOverride Function Get_Element() As Software_Element
    Public MustOverride Function Get_View() As Software_Element_View

    Public Overridable Sub Delete_My_View()
    End Sub

    Public Overridable Sub View_Element_Context_Menu_Clicked()
        Dim element As Software_Element = Get_Element()
        Dim view As Software_Element_View = Get_View()
        view.Display_Element(element.Name, element.UUID, element.Description)
    End Sub

    Public Overridable Sub View_Predefined_Element_Context_Menu_Clicked()
        Dim element As Software_Element = Get_Element()
        Dim view As Software_Element_View = Get_View()
        view.Display_Predefined_Element(element.Name, element.UUID, element.Description)
    End Sub

    Public Overrides Sub Edit_Context_Menu_Clicked()
        Dim element As Software_Element = Get_Element()
        Dim edit_form As New Software_Element_Edition_Form(Me, element.Name, element.Description)
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
            'Me.Display_Name_Is_Invalid()
        End If

        ' Treat the new Description
        If element.Is_Description_Valid(new_description) Then
            element.Description = new_description
            view.Update_All_Description_Views(new_description)
        Else
            'Me.Display_Description_Is_Invalid()
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

End Class