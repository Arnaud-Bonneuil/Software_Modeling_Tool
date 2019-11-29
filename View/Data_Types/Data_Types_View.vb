'=================================================================================================='
Public MustInherit Class Data_Types_View
    Inherits Software_Element_View
End Class


'=================================================================================================='
Public Class Basic_Data_Type_View

    Inherits Data_Types_View

    Private My_Controller As Basic_Data_Type_Controller

    Public Sub New(a_ctrl As Basic_Data_Type_Controller, a_name As String, parent_view As View)
        My_Controller = a_ctrl
        Me.Node = New TreeNode(a_name)
        parent_view.Get_Node.Nodes.Add(Me.Node)
        Me.Node.ImageKey = "Basic_Type"
        Me.Node.SelectedImageKey = "Basic_Type"
        Me.Node.Tag = My_Controller
        Node.ContextMenuStrip = CType(Node.TreeView, Model_Browser).Predefined_Elmt_CtxtMenu
    End Sub

End Class