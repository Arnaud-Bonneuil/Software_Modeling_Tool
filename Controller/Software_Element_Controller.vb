
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

    Sub Delete_Context_Menu_Clicked()
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



End Class