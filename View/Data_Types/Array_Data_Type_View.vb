'=================================================================================================='
Public Class Array_Data_Type_View

    Inherits Data_Types_View

    Private My_Controller As Array_Data_Type_Controller

    Public Sub New(a_ctrl As Array_Data_Type_Controller, a_name As String, parent_view As View)
        My_Controller = a_ctrl
        Me.Node = New TreeNode(a_name)
        parent_view.Get_Node.Nodes.Add(Me.Node)
        Me.Node.ImageKey = "Array_Data_Type"
        Me.Node.SelectedImageKey = "Array_Data_Type"
        Me.Node.Tag = My_Controller
        Node.ContextMenuStrip = CType(Node.TreeView, Model_Browser).Elmt_CtxtMenu
    End Sub

    Sub Display_Multiplicity_Is_Invalid()
        MsgBox("New multiplicity is invalid !", MsgBoxStyle.Critical, "Error")
    End Sub

End Class


'=================================================================================================='
Public Class Array_Data_Type_Edition_Form

    Inherits Typed_Software_Element_Edition_Form

    Public Multiplicity_TextBox As TextBox

    Public Sub New(
        a_controller As Controller,
        name_field As String,
        uuid_field As Guid,
        description_field As String,
        base_type_ref_path As String,
        data_type_path_list As List(Of String),
        multiplicity_field As UInteger)

        MyBase.New(
            a_controller, _
            name_field, uuid_field, description_field, _
            base_type_ref_path, data_type_path_list)

        Dim multiplicity_title As New Label
        multiplicity_title.Text = "Multiplicity"
        multiplicity_title.Location = New Point(Left_Margin, 260)
        multiplicity_title.Size = Edition_Form.Title_Size
        Me.Controls.Add(multiplicity_title)

        Me.Multiplicity_TextBox = New TextBox
        Me.Multiplicity_TextBox.Text = CStr(multiplicity_field)
        Me.Multiplicity_TextBox.Location = New Point(Field_Abscissa, 260)
        Me.Multiplicity_TextBox.Size = New Size(Field_Width, Title_Height)
        Me.Controls.Add(Me.Multiplicity_TextBox)

        Me.Set_Height(400)
    End Sub

    Public Overrides Sub Set_Read_Only()
        MyBase.Set_Read_Only()
        Multiplicity_TextBox.ReadOnly = True
    End Sub

End Class
