'=================================================================================================='
Public Class Structured_Data_Type_View

    Inherits Data_Types_View

    Private My_Controller As Structured_Data_Type_Controller

    Public Sub New(a_ctrl As Structured_Data_Type_Controller, a_name As String, parent_view As View)
        My_Controller = a_ctrl
        Me.Node = New TreeNode(a_name)
        parent_view.Get_Node.Nodes.Add(Me.Node)
        Me.Node.ImageKey = "Structured_Data_Type"
        Me.Node.SelectedImageKey = "Structured_Data_Type"
        Me.Node.Tag = My_Controller
        Node.ContextMenuStrip = CType(Node.TreeView, Model_Browser).Structure_CtxtMenu
    End Sub

End Class


'=================================================================================================='
Public Class Structured_Data_Type_Field_View

    Inherits Software_Element_View

    Private My_Controller As Structured_Data_Type_Field_Controller

    Public Sub New(
            a_ctrl As Structured_Data_Type_Field_Controller,
            a_name As String,
            parent_view As View)
        My_Controller = a_ctrl
        Me.Node = New TreeNode(a_name)
        parent_view.Get_Node.Nodes.Add(Me.Node)
        Me.Node.ImageKey = "Structured_Data_Type_Field"
        Me.Node.SelectedImageKey = "Structured_Data_Type_Field"
        Me.Node.Tag = My_Controller
        'Node.ContextMenuStrip = CType(Node.TreeView, Model_Browser).Elmt_CtxtMenu
    End Sub

    Public Overloads Sub Display_Element(
            a_name As String,
            a_uuid As Guid,
            a_description As String,
            a_base_type As String)
        MsgBox("To do.", MsgBoxStyle.OkOnly, "Field view")
    End Sub

End Class



'=================================================================================================='
Public Class Structured_Data_Type_Browser_Context_Menu

    Inherits Software_Element_Browser_Context_Menu

    Private WithEvents Sub_Context_Menu_Add_Field As ToolStripMenuItem

    Public Sub New(a_model_browser As Model_Browser)
        MyBase.New(a_model_browser)
        Me.Sub_Context_Menu_Add_Field = New ToolStripMenuItem

        Me.Sub_Context_Menu_Add_Field.Text = "Add field"

        Me.Items.AddRange(New ToolStripItem() {
            New ToolStripSeparator,
            Me.Sub_Context_Menu_Add_Field})
    End Sub

    Private Sub Add_Field(sender As Object, e As EventArgs) _
                                                         Handles Sub_Context_Menu_Add_Field.Click
        Dim ctrl As Structured_Data_Type_Controller
        ctrl = CType(Get_Controller(), Structured_Data_Type_Controller)
        If Not IsNothing(ctrl) Then
            'ctrl.Add_Field_Clicked()
        End If
    End Sub

End Class