Public Class Model_Diagram_View

    Inherits Software_Element_View

    Private My_Controller As Model_Diagram_Controller

    Private My_Diagram_Page As TabPage

    Public Sub New(a_ctrl As Model_Diagram_Controller, a_name As String, parent_view As View)
        My_Controller = a_ctrl
        Me.Node = New TreeNode(a_name)
        parent_view.Get_Node.Nodes.Add(Me.Node)
        Me.Node.ImageKey = "Diagram"
        Me.Node.SelectedImageKey = "Diagram"
        Me.Node.Tag = My_Controller
        Node.ContextMenuStrip = CType(Node.TreeView, Model_Browser).Model_Diagram_CtxtMenu
    End Sub

    Public Overrides Sub Update_All_Name_Views(new_name As String)
        MyBase.Update_All_Name_Views(new_name)
        ' If the diagram is drawn
        If Not IsNothing(My_Diagram_Page) Then
            My_Diagram_Page.Text = new_name
        End If
    End Sub

    Sub Set_Page(new_page As TabPage)
        My_Diagram_Page = new_page
    End Sub

End Class


Public Class Model_Diagram_Context_Menu

    Inherits Software_Element_Browser_Context_Menu

    Private WithEvents Sub_Context_Menu_Draw As ToolStripMenuItem

    Public Sub New(a_model_browser As Model_Browser)
        MyBase.New(a_model_browser)
        Me.Sub_Context_Menu_Draw = New ToolStripMenuItem

        Me.Sub_Context_Menu_Draw.Text = "Draw"

        Me.Items.AddRange(New ToolStripItem() {
            New ToolStripSeparator,
            Me.Sub_Context_Menu_Draw})
    End Sub

    Private Sub Draw(sender As Object, e As EventArgs) Handles Sub_Context_Menu_Draw.Click
        Dim ctrl As Model_Diagram_Controller
        ctrl = CType(Get_Controller(), Model_Diagram_Controller)
        If Not IsNothing(ctrl) Then
            ctrl.Draw_Context_Menu_Clicked()
        End If
    End Sub

End Class
