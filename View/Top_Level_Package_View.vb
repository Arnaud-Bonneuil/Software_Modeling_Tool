'=================================================================================================='
Public Class Top_Level_Package_View

    Inherits Package_View

    Private My_Controller As Top_Level_Package_Controller

    Public Sub New(
            a_controller As Package_Controller,
            a_name As String,
            parent_view As View,
            is_read_only As Boolean)
        MyBase.New(a_controller, a_name, parent_view, is_read_only)
        If is_read_only = True Then
            Node.ContextMenuStrip = CType(Node.TreeView, Model_Browser).Predefined_Elmt_CtxtMenu
        Else
            Node.ContextMenuStrip = CType(Node.TreeView, Model_Browser).Top_Level_Pkg_CtxtMenu
        End If
    End Sub


    Public Sub Display_Is_Modified(package_name As String)
        ' Modify model browser
        Me.Node.Text = package_name & " *"

        ' Modify diagram views

    End Sub

    Public Sub Display_Is_Saved(package_name As String)
        ' Modify model browser
        Me.Node.Text = package_name

        ' Modify diagram views

    End Sub

End Class


'=================================================================================================='
Public Class Top_Level_Package_Browser_Context_Menu

    Inherits Package_Browser_Context_Menu
    Private WithEvents Sub_Context_Menu_Save As ToolStripMenuItem
    Private WithEvents Sub_Context_Menu_Remove As ToolStripMenuItem

    Public Sub New(a_model_browser As Model_Browser)
        MyBase.New(a_model_browser)

        Me.Sub_Context_Menu_Save = New ToolStripMenuItem
        Me.Sub_Context_Menu_Save.Text = "Save"

        Me.Sub_Context_Menu_Remove = New ToolStripMenuItem
        Me.Sub_Context_Menu_Remove.Text = "Remove"

        ' Remove the "Delete" menu
        Me.Items.RemoveAt(3)

        ' Remove the "Move" menu
        Me.Items.RemoveAt(2)

        Me.Items.Insert(0, Me.Sub_Context_Menu_Save)
        Me.Items.Insert(3, Me.Sub_Context_Menu_Remove)
    End Sub

    Private Sub Save(sender As Object, e As EventArgs) Handles Sub_Context_Menu_Save.Click
        Dim ctrl As Top_Level_Package_Controller
        ctrl = CType(Get_Controller(), Top_Level_Package_Controller)
        If Not IsNothing(ctrl) Then
            ctrl.Save_Context_Menu_Clicked()
        End If
    End Sub

    Private Sub Remove(sender As Object, e As EventArgs) Handles Sub_Context_Menu_Remove.Click
        Dim ctrl As Top_Level_Package_Controller
        ctrl = CType(Get_Controller(), Top_Level_Package_Controller)
        If Not IsNothing(ctrl) Then
            ctrl.Remove_Context_Menu_Clicked()
        End If
    End Sub
End Class
