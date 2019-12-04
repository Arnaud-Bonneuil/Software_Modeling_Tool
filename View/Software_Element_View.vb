'=================================================================================================='

'=================================================================================================='
Public MustInherit Class Software_Element_View

    Inherits View

    Public Overrides Sub Update_All_Name_Views(new_name As String)
        MyBase.Update_All_Name_Views(new_name)
    End Sub

    Public Overrides Sub Update_All_Description_Views(new_description As String)
        MyBase.Update_All_Description_Views(new_description)
    End Sub

    Public Sub Update_Model_Browser(
            old_parent_ctrl As Software_Element_Controller,
            new_parent_ctrl As Software_Element_Controller)
        old_parent_ctrl.Get_View.Node.Nodes.Remove(Me.Node)
        new_parent_ctrl.Get_View.Node.Nodes.Add(Me.Node)
    End Sub

    Public Sub Display_Name_Is_Invalid()
        MsgBox("New name is invalid !", MsgBoxStyle.Critical, "Error")
    End Sub

    Public Sub Display_Description_Is_Invalid()
        MsgBox("New description is invalid !", MsgBoxStyle.Critical, "Error")
    End Sub

    Public Sub Display_Base_Data_Type_Is_Invalid()
        MsgBox("New base data type is invalid !", MsgBoxStyle.Critical, "Error")
    End Sub

End Class


'=================================================================================================='
' Edition form
'=================================================================================================='
Public Class Software_Element_Edition_Form

    Inherits Edition_Form

    Public Sub New(
        a_controller As Controller,
        name_field As String,
        uuid_field As Guid,
        description_field As String)
        MyBase.New(a_controller, name_field, uuid_field, description_field)
    End Sub

    Public Overridable Sub Set_Read_Only()
        Me.Text = "View element"
        Me.Name_TextBox.ReadOnly = True
        Me.Description_TextBox.ReadOnly = True
        Me.Apply_Button.Hide()
    End Sub

End Class


Public Class Typed_Software_Element_Edition_Form

    Inherits Software_Element_Edition_Form

    Public Base_Type_Ref_ComboBox As ComboBox

    Public Sub New(
        a_controller As Controller,
        name_field As String,
        uuid_field As Guid,
        description_field As String,
        base_type_ref_path As String,
        data_type_path_list As List(Of String))
        MyBase.New(a_controller, name_field, uuid_field, description_field)

        Dim base_type_title As New Label
        base_type_title.Text = "Base Data_Type"
        base_type_title.Location = New Point(Left_Margin, 220)
        base_type_title.Size = Edition_Form.Title_Size
        Me.Controls.Add(base_type_title)

        Me.Base_Type_Ref_ComboBox = New ComboBox
        Base_Type_Ref_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList
        If Not IsNothing(data_type_path_list) Then
            Dim data_type_path As String
            For Each data_type_path In data_type_path_list
                Me.Base_Type_Ref_ComboBox.Items.Add(data_type_path)
            Next
        Else
            Me.Base_Type_Ref_ComboBox.Items.Add(base_type_ref_path)
        End If
        Me.Base_Type_Ref_ComboBox.Text = base_type_ref_path
        Me.Base_Type_Ref_ComboBox.Location = New Point(Field_Abscissa, 220)
        Me.Base_Type_Ref_ComboBox.Size = New Size(Field_Width, Title_Height)
        Me.Controls.Add(Me.Base_Type_Ref_ComboBox)

        Me.Set_Height(340)

    End Sub

End Class


'=================================================================================================='
' Contextual menu for predefined (read only) element.
'=================================================================================================='
Public Class Predefined_Element_Browser_Context_Menu

    Inherits Browser_Context_Menu

    Private WithEvents Sub_Context_Menu_View As ToolStripMenuItem

    Public Sub New(a_model_browser As Model_Browser)
        MyBase.New(a_model_browser)

        Me.Sub_Context_Menu_View = New ToolStripMenuItem
        Me.Sub_Context_Menu_View.Text = "View"

        Me.Items.AddRange(New ToolStripItem() {
            Me.Sub_Context_Menu_View})
    End Sub

    Private Sub View(sender As Object, e As EventArgs) Handles Sub_Context_Menu_View.Click
        Dim ctrl As Software_Element_Controller
        ctrl = CType(Get_Controller(), Software_Element_Controller)
        If Not IsNothing(ctrl) Then
            ctrl.View_Element_Context_Menu_Clicked()
        End If
    End Sub

End Class


'=================================================================================================='
' Default contextual menu for standard Software_Elements.
' Shall be inherited by all other contextual menus.
'=================================================================================================='
Public Class Software_Element_Browser_Context_Menu

    Inherits Browser_Context_Menu

    Private WithEvents Sub_Context_Menu_View As ToolStripMenuItem
    Private WithEvents Sub_Context_Menu_Edit As ToolStripMenuItem
    Private WithEvents Sub_Context_Menu_Move As ToolStripMenuItem
    Private WithEvents Sub_Context_Menu_Delete As ToolStripMenuItem

    Public Sub New(a_model_browser As Model_Browser)
        MyBase.New(a_model_browser)

        Me.Sub_Context_Menu_View = New ToolStripMenuItem
        Me.Sub_Context_Menu_View.Text = "View"

        Me.Sub_Context_Menu_Edit = New ToolStripMenuItem
        Me.Sub_Context_Menu_Edit.Text = "Edit"

        Me.Sub_Context_Menu_Move = New ToolStripMenuItem
        Me.Sub_Context_Menu_Move.Text = "Move"

        Me.Sub_Context_Menu_Delete = New ToolStripMenuItem
        Me.Sub_Context_Menu_Delete.Text = "Delete"

        Me.Items.AddRange(New ToolStripItem() {
            Me.Sub_Context_Menu_View,
            Me.Sub_Context_Menu_Edit,
            Me.Sub_Context_Menu_Move,
            Me.Sub_Context_Menu_Delete})

    End Sub

    Private Sub View_Element(sender As Object, e As EventArgs) Handles Sub_Context_Menu_View.Click
        Dim ctrl As Software_Element_Controller
        ctrl = CType(Get_Controller(), Software_Element_Controller)
        If Not IsNothing(ctrl) Then
            ctrl.View_Element_Context_Menu_Clicked()
        End If
    End Sub


    Private Sub Edit_Element(sender As Object, e As EventArgs) Handles Sub_Context_Menu_Edit.Click
        Dim ctrl As Controller = Get_Controller()
        If Not IsNothing(ctrl) Then
            ctrl.Edit_Context_Menu_Clicked()
        End If
    End Sub

    Private Sub Move_Element(sender As Object, e As EventArgs) Handles Sub_Context_Menu_Move.Click
        Dim ctrl As Software_Element_Controller
        ctrl = CType(Get_Controller(), Software_Element_Controller)
        If Not IsNothing(ctrl) Then
            ctrl.Move_Context_Menu_Clicked()
        End If
    End Sub

    Private Sub Delete_Element(sender As Object, e As EventArgs) _
                                                               Handles Sub_Context_Menu_Delete.Click
        Dim ctrl As Software_Element_Controller
        ctrl = CType(Get_Controller(), Software_Element_Controller)
        If Not IsNothing(ctrl) Then
            ctrl.Delete_Context_Menu_Clicked()
        End If
    End Sub

End Class


'=================================================================================================='
Public Class Move_Window
    Inherits Form

    Private WithEvents Move_Button As Button

    Private Destination_Element As ComboBox

    Public Sub New(list As List(Of String))

        Me.Text = "Select new parent :"
        Dim box_size = New Size(300, 200)
        Me.ClientSize = box_size
        Me.FormBorderStyle = FormBorderStyle.Sizable
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MaximumSize = New Size(800, 200)
        Me.MinimumSize = box_size

        Me.Destination_Element = New ComboBox
        Dim path As String
        For Each path In list
            Me.Destination_Element.Items.Add(path)
        Next
        Me.Destination_Element.Anchor = AnchorStyles.Right Or AnchorStyles.Left
        Me.Destination_Element.Location = New Point(20, 20)
        Me.Destination_Element.Size = New Size(240, 25)
        Me.Controls.Add(Me.Destination_Element)

        Me.Move_Button = New Button
        Me.Move_Button.Text = "Move"
        Me.Move_Button.Location = New Point(110, 100)
        Me.Controls.Add(Me.Move_Button)

    End Sub

    Private Sub Move_Pressed(sender As Object, e As EventArgs) Handles Move_Button.Click
        Me.Close()
    End Sub

    Public Function Get_Destination() As String
        Return Me.Destination_Element.Text
    End Function

End Class