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

    Function Draw_On_Diagram_Page(
            page As TabPage,
            a_diagram_element As Square_Model_Diagram_Element,
            a_name As String,
            a_description As String) As Software_Element_Square_Figure

        Dim fig As New Enumerated_Data_Type_Figure(My_Controller, a_diagram_element, a_name, a_description)
        page.Controls.Add(fig)

        Return fig

    End Function

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

    Public Sub Display_Value_Is_Invalid()
        MsgBox("New value is invalid !", MsgBoxStyle.Critical, "Error")
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
            ctrl.Add_Enumeral_Context_Menu_Clicked()
        End If
    End Sub

End Class


'=================================================================================================='
Public Class Enumerated_Data_Type_Enumeral_Edition_Form

    Inherits Software_Element_Edition_Form

    Public Value_TextBox As TextBox

    Public Sub New(
        a_controller As Controller,
        name_field As String,
        uuid_field As Guid,
        description_field As String,
        value_field As UInteger)
        MyBase.New(a_controller, name_field, uuid_field, description_field)

        Dim value_title As New Label
        value_title.Text = "Value"
        value_title.Location = New Point(Left_Margin, 220)
        value_title.Size = Edition_Form.Title_Size
        Me.Controls.Add(value_title)

        Me.Value_TextBox = New TextBox
        Me.Value_TextBox.Text = CStr(value_field)
        Me.Value_TextBox.Location = New Point(Field_Abscissa, 220)
        Me.Value_TextBox.Size = New Size(Field_Width, Title_Height)
        Me.Controls.Add(Me.Value_TextBox)

        Me.Set_Height(340)
    End Sub

    Public Overrides Sub Set_Read_Only()
        MyBase.Set_Read_Only()
        Value_TextBox.ReadOnly = True
    End Sub

End Class

'=================================================================================================='
Public Class Enumerated_Data_Type_Figure
    Inherits Software_Element_Square_Figure

    Public Sub New(a_ctrl As Software_Element_Controller,
            a_diagram_element As Square_Model_Diagram_Element,
            a_name As String,
            a_description As String)

        MyBase.New(a_ctrl, a_diagram_element, a_name, a_description)

        Me.Border_Color = Pens.DarkRed

    End Sub

End Class
