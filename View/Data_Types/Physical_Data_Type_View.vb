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

    Sub Display_Resolution_Is_Invalid()
        MsgBox("New resolution is invalid !", MsgBoxStyle.Critical, "Error")
    End Sub

    Sub Display_Offset_Is_Invalid()
        MsgBox("New offset is invalid !", MsgBoxStyle.Critical, "Error")
    End Sub

End Class

'=================================================================================================='
Public Class Physical_Data_Type_Edition_Form

    Inherits Typed_Software_Element_Edition_Form

    Public Unit_TextBox As TextBox
    Public Resolution_TextBox As TextBox
    Public Offset_TextBox As TextBox

    Public Sub New(
        a_controller As Controller,
        name_field As String,
        uuid_field As Guid,
        description_field As String,
        base_type_ref_path As String,
        data_type_path_list As List(Of String),
        unit_field As String,
        resol_field As Decimal,
        offset_field As Decimal)

        MyBase.New(
            a_controller, _
            name_field, uuid_field, description_field, _
            base_type_ref_path, data_type_path_list)

        Dim unit_title As New Label
        unit_title.Text = "Unit"
        unit_title.Location = New Point(Left_Margin, 260)
        unit_title.Size = Edition_Form.Title_Size
        Me.Controls.Add(unit_title)

        Me.Unit_TextBox = New TextBox
        Me.Unit_TextBox.Text = CStr(unit_field)
        Me.Unit_TextBox.Location = New Point(Field_Abscissa, 260)
        Me.Unit_TextBox.Size = New Size(Field_Width, Title_Height)
        Me.Controls.Add(Me.Unit_TextBox)

        Dim resol_title As New Label
        resol_title.Text = "Resolution"
        resol_title.Location = New Point(Left_Margin, 300)
        resol_title.Size = Edition_Form.Title_Size
        Me.Controls.Add(resol_title)

        Me.Resolution_TextBox = New TextBox
        Me.Resolution_TextBox.Text = CStr(resol_field)
        Me.Resolution_TextBox.Location = New Point(Field_Abscissa, 300)
        Me.Resolution_TextBox.Size = New Size(Field_Width, Title_Height)
        Me.Controls.Add(Me.Resolution_TextBox)

        Dim offset_title As New Label
        offset_title.Text = "Offset"
        offset_title.Location = New Point(Left_Margin, 340)
        offset_title.Size = Edition_Form.Title_Size
        Me.Controls.Add(offset_title)

        Me.Offset_TextBox = New TextBox
        Me.Offset_TextBox.Text = CStr(offset_field)
        Me.Offset_TextBox.Location = New Point(Field_Abscissa, 340)
        Me.Offset_TextBox.Size = New Size(Field_Width, Title_Height)
        Me.Controls.Add(Me.Offset_TextBox)

        Me.Set_Height(480)
    End Sub

    Public Overrides Sub Set_Read_Only()
        MyBase.Set_Read_Only()
        Unit_TextBox.ReadOnly = True
        Resolution_TextBox.ReadOnly = True
        Offset_TextBox.ReadOnly = True
    End Sub

End Class