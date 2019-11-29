'=================================================================================================='
Public Class Enumerated_Data_Type_View

    Inherits Data_Types_View

    Private My_Controller As Enumerated_Data_Type_Controller

    Public Sub New(a_ctrl As Enumerated_Data_Type_Controller, a_name As String, parent_view As View)
        My_Controller = a_ctrl
        Me.Node = New TreeNode(a_name)
        parent_view.Get_Node.Nodes.Add(Me.Node)
        Me.Node.ImageKey = "Enumerated_Data_Type"
        Me.Node.SelectedImageKey = "Enumerated_Data_Type"
        Me.Node.Tag = My_Controller
        Node.ContextMenuStrip = CType(Node.TreeView, Model_Browser).Enum_CtxtMenu
    End Sub

End Class


'=================================================================================================='
Public Class Enumerated_Data_Type_Enumeral_View

    Inherits Software_Element_View

    Private My_Controller As Enumerated_Data_Type_Enumeral_Controller

    Public Sub New(
            a_ctrl As Enumerated_Data_Type_Enumeral_Controller,
            a_name As String,
            parent_view As View)
        My_Controller = a_ctrl
        Me.Node = New TreeNode(a_name)
        parent_view.Get_Node.Nodes.Add(Me.Node)
        Me.Node.ImageKey = "Enumerated_Data_Type_Enumeral"
        Me.Node.SelectedImageKey = "Enumerated_Data_Type_Enumeral"
        Me.Node.Tag = My_Controller
        Node.ContextMenuStrip = CType(Node.TreeView, Model_Browser).Elmt_CtxtMenu
    End Sub

    Public Overloads Sub Display_Element(
            a_name As String,
            a_uuid As Guid,
            a_description As String,
            a_value As UInteger)
        Dim message_box_text As String
        message_box_text = "Name : " & a_name & vbCrLf & vbCrLf & _
            "UUID : " & a_uuid.ToString & vbCrLf & vbCrLf & _
            "Description : " & a_description & vbCrLf & vbCrLf & _
            "Value : " & a_value.ToString
        MsgBox(message_box_text, MsgBoxStyle.OkOnly, "Enumeral view")
    End Sub

End Class


'=================================================================================================='
Public Class Enumerated_Data_Type_Browser_Context_Menu

    Inherits Software_Element_Browser_Context_Menu

    Private WithEvents Sub_Context_Menu_Add_Enumeral As ToolStripMenuItem

    Public Sub New(a_model_browser As Model_Browser)
        MyBase.New(a_model_browser)
        Me.Sub_Context_Menu_Add_Enumeral = New ToolStripMenuItem

        Me.Sub_Context_Menu_Add_Enumeral.Text = "Add enumeral"

        Me.Items.AddRange(New ToolStripItem() {
            New ToolStripSeparator,
            Me.Sub_Context_Menu_Add_Enumeral})
    End Sub

    Private Sub Add_Enumeral(sender As Object, e As EventArgs) _
                                                         Handles Sub_Context_Menu_Add_Enumeral.Click
        Dim ctrl As Enumerated_Data_Type_Controller
        ctrl = CType(Get_Controller(), Enumerated_Data_Type_Controller)
        If Not IsNothing(ctrl) Then
            ' ctrl.Add_Enumeral_Clicked()
        End If
    End Sub

End Class