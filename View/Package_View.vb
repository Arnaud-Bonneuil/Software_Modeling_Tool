'=================================================================================================='
Public Class Package_View

    Inherits Software_Element_View

    Private My_Controller As Package_Controller

    Public Sub New(
            a_controller As Package_Controller,
            a_name As String,
            parent_view As View,
            is_read_only As Boolean)
        My_Controller = a_controller
        Me.Node = New TreeNode(a_name)
        parent_view.Get_Node.Nodes.Add(Me.Node)
        Me.Node.ImageKey = "Package"
        Me.Node.SelectedImageKey = "Package"
        Me.Node.Tag = My_Controller
        Node.ContextMenuStrip = CType(Node.TreeView, Model_Browser).Pkg_CtxtMenu
    End Sub

End Class


'=================================================================================================='
Public Class Package_Browser_Context_Menu

    Inherits Software_Element_Browser_Context_Menu

    Private WithEvents Sub_Context_Menu_Add_Package As ToolStripMenuItem
    Private WithEvents Sub_Context_Menu_Add_Diagram As ToolStripMenuItem
    Private WithEvents Sub_Context_Menu_Add_Enum As ToolStripMenuItem
    Private WithEvents Sub_Context_Menu_Add_Array As ToolStripMenuItem
    Private WithEvents Sub_Context_Menu_Add_Phy As ToolStripMenuItem
    Private WithEvents Sub_Context_Menu_Add_Struct As ToolStripMenuItem


    Public Sub New(a_model_browser As Model_Browser)
        MyBase.New(a_model_browser)

        Me.Sub_Context_Menu_Add_Package = New ToolStripMenuItem
        Me.Sub_Context_Menu_Add_Package.Text = "Add Package"

        Me.Sub_Context_Menu_Add_Diagram = New ToolStripMenuItem
        Me.Sub_Context_Menu_Add_Diagram.Text = "Add Diagram"

        Me.Sub_Context_Menu_Add_Enum = New ToolStripMenuItem
        Me.Sub_Context_Menu_Add_Enum.Text = "Add Enumerated_Data_Type"

        Me.Sub_Context_Menu_Add_Array = New ToolStripMenuItem
        Me.Sub_Context_Menu_Add_Array.Text = "Add Array_Data_Type"

        Me.Sub_Context_Menu_Add_Phy = New ToolStripMenuItem
        Me.Sub_Context_Menu_Add_Phy.Text = "Add Physical_Data_Type"

        Me.Sub_Context_Menu_Add_Struct = New ToolStripMenuItem
        Me.Sub_Context_Menu_Add_Struct.Text = "Add Structured_Data_Type"

        Me.Items.AddRange(New ToolStripItem() {
            New ToolStripSeparator,
            Me.Sub_Context_Menu_Add_Diagram,
            New ToolStripSeparator,
            Me.Sub_Context_Menu_Add_Package,
            New ToolStripSeparator,
            Me.Sub_Context_Menu_Add_Enum,
            Me.Sub_Context_Menu_Add_Array,
            Me.Sub_Context_Menu_Add_Phy,
            Me.Sub_Context_Menu_Add_Struct})
    End Sub

    Private Sub Add_Package(sender As Object, e As EventArgs) _
                                                          Handles Sub_Context_Menu_Add_Package.Click
        Dim ctrl As Package_Controller = CType(Get_Controller(), Package_Controller)
        If Not IsNothing(ctrl) Then
            ctrl.Add_Package_Context_Menu_Clicked()
        End If
    End Sub

    Private Sub Add_Diagram(sender As Object, e As EventArgs) _
                                                      Handles Sub_Context_Menu_Add_Diagram.Click
        Dim ctrl As Package_Controller = CType(Get_Controller(), Package_Controller)
        If Not IsNothing(ctrl) Then
            'ctrl.Add_Diagram_Clicked()
        End If
    End Sub

    Private Sub Add_Enum(sender As Object, e As EventArgs) Handles Sub_Context_Menu_Add_Enum.Click
        Dim ctrl As Package_Controller = CType(Get_Controller(), Package_Controller)
        If Not IsNothing(ctrl) Then
            ctrl.Add_Enum_Context_Menu_Clicked()
        End If
    End Sub

    Private Sub Add_Array(sender As Object, e As EventArgs) Handles Sub_Context_Menu_Add_Array.Click
        Dim ctrl As Package_Controller = CType(Get_Controller(), Package_Controller)
        If Not IsNothing(ctrl) Then
            ctrl.Add_Array_Context_Menu_Clicked()
        End If
    End Sub

    Private Sub Add_Phys(sender As Object, e As EventArgs) Handles Sub_Context_Menu_Add_Phy.Click
        Dim ctrl As Package_Controller = CType(Get_Controller(), Package_Controller)
        If Not IsNothing(ctrl) Then
            ctrl.Add_Physical_Context_Menu_Clicked()
        End If
    End Sub

    Private Sub Add_Struct(sender As Object, e As EventArgs) _
                                                           Handles Sub_Context_Menu_Add_Struct.Click
        Dim ctrl As Package_Controller = CType(Get_Controller(), Package_Controller)
        If Not IsNothing(ctrl) Then
            ctrl.Add_Struct_Context_Menu_Clicked()
        End If
    End Sub

End Class