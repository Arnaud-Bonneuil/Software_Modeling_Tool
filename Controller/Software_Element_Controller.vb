'=================================================================================================='

'=================================================================================================='
Public MustInherit Class Software_Element_Controller

    Inherits Controller

    Public Parent_Controller As Software_Element_Controller = Nothing
    Public Children_Controller As List(Of Software_Element_Controller) = _
                                                            New List(Of Software_Element_Controller)

    Protected Is_Under_Creation As Boolean ' Indicates that I am temporarily created

    Public MustOverride Function Get_Element() As Software_Element
    Public MustOverride Function Get_View() As Software_Element_View

    '=============================================================================================='
    ' Public methods
    '=============================================================================================='
    Public Sub Set_Parenthood(parent_ctrl As Software_Element_Controller)
        Me.Parent_Controller = parent_ctrl
        If Not IsNothing(parent_ctrl) Then
            parent_ctrl.Children_Controller.Add(Me)
        End If
        Me.Get_Controller_Dico_By_Element_UUID.Add(Get_Element.UUID, Me)
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

    Public Function Get_Controller_By_Element_UUID(element_uuid As Guid) _
                                                                      As Software_Element_Controller

        Dim dico As Dictionary(Of Guid, Software_Element_Controller)
        dico = Get_Controller_Dico_By_Element_UUID()

        If dico.ContainsKey(element_uuid) Then
            Return dico(element_uuid)
        Else
            Return Nothing
        End If
    End Function

    ' Return the list of a the controller of a Data_Type within the project
    Public Function Get_Controllers_Of_Data_Type() As List(Of Software_Element_Controller)

        Dim ctrl_list As New List(Of Software_Element_Controller)

        ' Get project controller
        Dim project_ctrl As Software_Project_Controller
        project_ctrl = Me.Get_Top_Level_Package_Controller.My_Project_Controller

        Dim top_pkg_ctrl As Top_Level_Package_Controller
        For Each top_pkg_ctrl In project_ctrl.My_Top_Level_Package_Controllers_List
            top_pkg_ctrl.Get_All_Ctrl_By_Elmt_MetaClass(ctrl_list, GetType(Enumerated_Data_Type))
            top_pkg_ctrl.Get_All_Ctrl_By_Elmt_MetaClass(ctrl_list, GetType(Array_Data_Type))
            top_pkg_ctrl.Get_All_Ctrl_By_Elmt_MetaClass(ctrl_list, GetType(Physical_Data_Type))
            top_pkg_ctrl.Get_All_Ctrl_By_Elmt_MetaClass(ctrl_list, GetType(Structured_Data_Type))
            top_pkg_ctrl.Get_All_Ctrl_By_Elmt_MetaClass(ctrl_list, GetType(Basic_Integer_Type))
            top_pkg_ctrl.Get_All_Ctrl_By_Elmt_MetaClass(ctrl_list, GetType(Basic_Boolean_Type))
            top_pkg_ctrl.Get_All_Ctrl_By_Elmt_MetaClass(ctrl_list,
                                                                 GetType(Basic_Floating_Point_Type))
        Next

        Return ctrl_list
    End Function

    Public Sub Set_Top_Level_Package_Controller_Status_To_Modified()
        Dim top_ctrl As Top_Level_Package_Controller
        top_ctrl = Me.Get_Top_Level_Package_Controller
        top_ctrl.Set_Is_Modified()
    End Sub

    ' Basic edition form creation for element without specific attributes
    ' It shall be overriden by the elements with specific attributes
    Public Overridable Function Create_Edition_Form() As Software_Element_Edition_Form
        Dim software_element As Software_Element = Get_Element()
        Dim edit_form As New Software_Element_Edition_Form(
            Me,
            software_element.Name,
            software_element.UUID,
            software_element.Description)
        Return edit_form
    End Function

    ' Basic treatment procedure for element without specific attributes
    ' It shall be overriden by the elements with specific attributes
    Public Overridable Function Treat_Edition_Form_Data(edition_form As Edition_Form) As Boolean
        Dim element_is_ok As Boolean = Me.Analyze_Edition_Form_Common_Data(edition_form)

        If element_is_ok = True Then
            Me.Apply_Edition_Form_Common_Data(edition_form)
        End If

        Return element_is_ok
    End Function

    Public Function Analyze_Edition_Form_Common_Data(edition_form As Edition_Form) As Boolean

        Dim element_is_ok As Boolean = True
        Dim element As Software_Element = Get_Element()
        Dim view As Software_Element_View = Get_View()

        ' Treat the new Name
        Dim new_name As String = edition_form.Name_TextBox.Text
        If Not element.Is_Name_Valid(new_name) Then
            element_is_ok = False
            view.Display_Name_Is_Invalid()
        End If

        ' Treat the new Description
        Dim new_description As String = edition_form.Description_TextBox.Text
        If Not element.Is_Description_Valid(new_description) Then
            element_is_ok = False
            view.Display_Description_Is_Invalid()
        End If

        ' Update the element data if all is ok
        If element_is_ok = True Then

        End If

        Return element_is_ok
    End Function

    Public Sub Apply_Edition_Form_Common_Data(edition_form As Edition_Form)
        Dim element As Software_Element = Get_Element()
        Dim view As Software_Element_View = Get_View()

        Dim new_name As String = edition_form.Name_TextBox.Text
        element.Name = new_name
        view.Update_All_Name_Views(new_name)

        Dim new_description As String = edition_form.Description_TextBox.Text
        element.Description = new_description
        view.Update_All_Description_Views(new_description)
    End Sub

    ' Shall be call by a parent controller to indicate that Me is temporarily created, waiting for 
    ' valid data from the user.
    Public Sub Set_Is_Under_Creation()
        Is_Under_Creation = True
    End Sub


    '=============================================================================================='
    ' Methods for model browser contextual menu
    '=============================================================================================='
    Public Sub View_Element_Context_Menu_Clicked()
        Dim edit_form As Software_Element_Edition_Form
        edit_form = Create_Edition_Form()
        edit_form.Set_Read_Only()
        edit_form.ShowDialog()
    End Sub

    Public Overrides Sub Edit_Context_Menu_Clicked()
        Dim edit_form As Software_Element_Edition_Form
        edit_form = Create_Edition_Form()
        edit_form.ShowDialog()
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

        ' Once moving window is closed, check the destination
        Dim new_parent_path As String = move_form.Get_Destination
        ' If the choosen Parent is with the list of possible
        If parent_ctrl_dic.ContainsKey(new_parent_path) Then

            Dim new_parent_ctrl As Software_Element_Controller = parent_ctrl_dic(new_parent_path)

            ' Check that the new prent has not already a child with the name as the Software_Element
            Dim name_is_unique As Boolean = True
            For Each ctrl In new_parent_ctrl.Children_Controller
                If ctrl.Get_Element.Name = moved_element.Name Then
                    name_is_unique = False
                    Exit For
                End If
            Next

            If name_is_unique = False Then
                Me.Get_View.Display_New_Parent_Has_A_Child_With_Same_Name()
            Else
                ' Move the element

                ' Remove Me from my current Parent
                Me.Set_Top_Level_Package_Controller_Status_To_Modified()
                Dim view As Software_Element_View = Get_View()
                moved_element.Parent.Remove_Element(moved_element)
                Me.Parent_Controller.Children_Controller.Remove(Me)

                ' Add Me to my new Parent
                Dim new_parent As Software_Element = new_parent_ctrl.Get_Element
                new_parent.Add_Element(moved_element)
                new_parent_ctrl.Set_Top_Level_Package_Controller_Status_To_Modified()

                ' Updte model browser
                view.Update_Model_Browser(Me.Parent_Controller, new_parent_ctrl)
            End If
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

    Public Overrides Sub Edition_Window_Apply_Button_Clicked(edit_win As Edition_Form)

        Dim element_is_ok As Boolean
        element_is_ok = Treat_Edition_Form_Data(edit_win)

        If element_is_ok = True Then
            Me.Set_Top_Level_Package_Controller_Status_To_Modified()

            ' Eventually indicates that the creation is ok (if edition form has been opened from 
            ' a creation function)
            Me.Is_Under_Creation = False

            edit_win.Close()
        End If

    End Sub

    Public Overrides Sub Edition_Window_Closing(edition_form As Edition_Form)
        If Me.Is_Under_Creation = True Then
            ' The edition window has been opened to set the data of a new element
            ' It is closed without successful 'Apply' : the new element must be destroyed

            ' Delete the view
            Dim view As Software_Element_View = Get_View()
            view.Delete_All_View()

            ' Delete the element and its children
            Dim element As Software_Element = Get_Element()
            element.Parent.Remove_Element(element)

            ' Delete the controller and its children

        End If
    End Sub


    '=============================================================================================='
    ' Methods for model diagrams
    '=============================================================================================='
    Public Overridable Sub Draw_Figure(page As TabPage, diagram_elmt As Model_Diagram_Element)

    End Sub

    Public Sub Name_Edited_Within_Diagram(new_name As String)

        Dim element As Software_Element = Get_Element()
        Dim view As Software_Element_View = Get_View()

        ' Analyze the new Name
        If Not element.Is_Name_Valid(new_name) Then
            view.Display_Name_Is_Invalid()
        Else
            ' Apply the new name
            If element.Name <> new_name Then
                element.Name = new_name
                Me.Set_Top_Level_Package_Controller_Status_To_Modified()
            End If
        End If
        view.Update_All_Name_Views(element.Name)

    End Sub

    Public Sub Description_Edited_Within_Diagram(new_description As String)

        Dim element As Software_Element = Get_Element()
        Dim view As Software_Element_View = Get_View()

        ' Analyze the new description
        If Not element.Is_Description_Valid(new_description) Then
            view.Display_Description_Is_Invalid()
        Else
            ' Apply the new description
            If element.Description <> new_description Then
                element.Description = new_description
                Me.Set_Top_Level_Package_Controller_Status_To_Modified()
            End If
        End If
        view.Update_All_Description_Views(element.Description)

    End Sub


    '=============================================================================================='
    ' Private methods
    '=============================================================================================='
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
            top_pkg_ctrl.Get_All_Ctrl_By_Elmt_MetaClass(ctrl_list, sw_elmt_type)
        Next

        Return ctrl_list
    End Function

    Private Sub Get_All_Ctrl_By_Elmt_MetaClass(
        ByRef ctrl_list As List(Of Software_Element_Controller),
        meta_class As Type)
        Dim child As Software_Element_Controller
        For Each child In Me.Children_Controller
            If child.Get_Element.GetType = meta_class Then
                ctrl_list.Add(child)
            End If
            child.Get_All_Ctrl_By_Elmt_MetaClass(ctrl_list, meta_class)
        Next
    End Sub



End Class



'=================================================================================================='
' This class shall be used by each Software_Element which has a Base_Data_Type_Ref
'=================================================================================================='
Public Class Based_Data_Type_Manager

    Private Possible_Base_Data_Type_Ctrl As New Dictionary(Of String, Software_Element_Controller)

    Public Function Get_Current_Base_Data_Type_Path(
            ctrl As Software_Element_Controller,
            data_type_ref As Guid) As String

        ' Initialize the returned path
        Dim base_data_type_path As String = "Unresolved"

        ' Get the dictionary of Controller by Software_Element UUID
        Dim ctrl_dico As Dictionary(Of Guid, Software_Element_Controller)
        ctrl_dico = ctrl.Get_Controller_Dico_By_Element_UUID

        ' Test if the referenced Data_Type is known
        If ctrl_dico.ContainsKey(data_type_ref) Then

            ' Get the controller of the referenced Data_Type
            Dim base_data_type_ctrl As Software_Element_Controller
            base_data_type_ctrl = ctrl_dico(data_type_ref)

            ' Get the path of the referenced Data_Type
            base_data_type_path = base_data_type_ctrl.Get_Element.Get_Path
        End If

        Return base_data_type_path
    End Function

    Public Sub Update_Possible_Base_Data_Type_Ref(ctrl_to_remove As Software_Element_Controller)
        ' Initialize the dictionary
        Me.Possible_Base_Data_Type_Ctrl.Clear()

        ' Get the list of controller of Data_Type
        Dim base_data_type_ctrl_list As List(Of Software_Element_Controller)
        base_data_type_ctrl_list = ctrl_to_remove.Get_Controllers_Of_Data_Type()
        base_data_type_ctrl_list.Remove(ctrl_to_remove)

        ' Build the dictionary
        For Each ctrl In base_data_type_ctrl_list
            Possible_Base_Data_Type_Ctrl.Add(ctrl.Get_Element.Get_Path, ctrl)
        Next
    End Sub

    Public Function Get_Possible_Base_Data_Type_Path_List() As List(Of String)
        Return Me.Possible_Base_Data_Type_Ctrl.Keys.ToList
    End Function

    Public Function Check_Base_Data_Type_Validity(data_type_path As String) As Boolean
        If Me.Get_Possible_Base_Data_Type_Path_List.Contains(data_type_path) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function Get_New_Base_Data_Type_Ref(data_type_path As String) As Guid
        Dim base_data_type_ctrl As Software_Element_Controller
        base_data_type_ctrl = Me.Possible_Base_Data_Type_Ctrl(data_type_path)
        Dim base_data_type_uuid = base_data_type_ctrl.Get_Element.UUID
        Return base_data_type_uuid
    End Function

End Class