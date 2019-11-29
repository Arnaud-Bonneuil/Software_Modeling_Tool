'=================================================================================================='
Public Class Physical_Data_Type_View

    Inherits Data_Types_View

    Private My_Controller As Physical_Data_Type_Controller

    Public Sub New(a_ctrl As Physical_Data_Type_Controller, a_name As String, parent_view As View)
        My_Controller = a_ctrl
        Me.Node = New TreeNode(a_name)
        parent_view.Get_Node.Nodes.Add(Me.Node)
        Me.Node.ImageKey = "Physical_Data_Type"
        Me.Node.SelectedImageKey = "Physical_Data_Type"
        Me.Node.Tag = My_Controller
        Node.ContextMenuStrip = CType(Node.TreeView, Model_Browser).Elmt_CtxtMenu
    End Sub

End Class
