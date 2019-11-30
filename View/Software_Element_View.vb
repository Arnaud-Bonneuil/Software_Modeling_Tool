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

    Public Overridable Sub Display_Predefined_Element(
            a_name As String,
            a_uuid As Guid,
            a_descripttion As String)
        Me.Display_Element(a_name, a_uuid, a_descripttion)
    End Sub

    Public Overridable Sub Display_Element(
            a_name As String,
            a_uuid As Guid,
            a_descripttion As String)
        Dim message_box_text As String
        message_box_text = "Name : " & a_name & vbCrLf & vbCrLf & _
            "UUID : " & a_uuid.ToString & vbCrLf & vbCrLf & _
            "Description : " & a_descripttion
        MsgBox(message_box_text, MsgBoxStyle.OkOnly, "Element view")
    End Sub

    Public Sub Update_Model_Browser(
            old_parent_ctrl As Software_Element_Controller,
            new_parent_ctrl As Software_Element_Controller)
        old_parent_ctrl.Get_View.Node.Nodes.Remove(Me.Node)
        new_parent_ctrl.Get_View.Node.Nodes.Add(Me.Node)
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
        description_field As String)
        MyBase.New(a_controller, name_field, description_field)
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
            ctrl.View_Predefined_Element_Context_Menu_Clicked()
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