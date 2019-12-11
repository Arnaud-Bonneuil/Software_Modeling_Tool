'=================================================================================================='

'=================================================================================================='
Public MustInherit Class Software_Element_View

    Inherits View

    Private Name_Diagram_Boxes_List As New List(Of TextBox)
    Private Description_Diagram_Boxes_List As New List(Of RichTextBox)

    Public Sub Add_Name_Diagram_Box(new_name_textbox As TextBox)
        Name_Diagram_Boxes_List.Add(new_name_textbox)
    End Sub

    Public Sub Add_Description_Diagram_Box(new_description_textbox As RichTextBox)
        Description_Diagram_Boxes_List.Add(new_description_textbox)
    End Sub

    Public Overrides Sub Update_All_Name_Views(new_name As String)
        MyBase.Update_All_Name_Views(new_name)

        Dim text_box As TextBox
        For Each text_box In Me.Name_Diagram_Boxes_List
            text_box.Text = new_name
        Next
    End Sub

    Public Overrides Sub Update_All_Description_Views(new_description As String)
        MyBase.Update_All_Description_Views(new_description)

        Dim text_box As RichTextBox
        For Each text_box In Me.Description_Diagram_Boxes_List
            text_box.Text = new_description
        Next
    End Sub

    Public Sub Update_Model_Browser(
            old_parent_ctrl As Software_Element_Controller,
            new_parent_ctrl As Software_Element_Controller)
        old_parent_ctrl.Get_View.Node.Nodes.Remove(Me.Node)
        new_parent_ctrl.Get_View.Node.Nodes.Add(Me.Node)
    End Sub

    Public Sub Display_Name_Is_Invalid()
        MsgBox("New name is invalid !", MsgBoxStyle.Critical, "Error")
    End Sub

    Public Sub Display_Description_Is_Invalid()
        MsgBox("New description is invalid !", MsgBoxStyle.Critical, "Error")
    End Sub

    Public Sub Display_Base_Data_Type_Is_Invalid()
        MsgBox("New base data type is invalid !", MsgBoxStyle.Critical, "Error")
    End Sub

    Sub Display_New_Parent_Has_A_Child_With_Same_Name()
        MsgBox("New parent has a child with the same name as the moved element !",
               MsgBoxStyle.Critical, "Error")
    End Sub

End Class


'=================================================================================================='
' Edition form
'=================================================================================================='
Public Class Software_Element_Edition_Form

    Inherits Edition_Form

    Public Sub New(
        a_controller As Controller,
        name_field As String,
        uuid_field As Guid,
        description_field As String)
        MyBase.New(a_controller, name_field, uuid_field, description_field)
    End Sub

    Public Overridable Sub Set_Read_Only()
        Me.Text = "View element"
        Me.Name_TextBox.ReadOnly = True
        Me.Description_TextBox.ReadOnly = True
        Me.Apply_Button.Hide()
    End Sub

End Class


'=================================================================================================='

'=================================================================================================='
Public Class Typed_Software_Element_Edition_Form

    Inherits Software_Element_Edition_Form

    Public Base_Type_Ref_ComboBox As ComboBox

    Public Sub New(
        a_controller As Controller,
        name_field As String,
        uuid_field As Guid,
        description_field As String,
        base_type_ref_path As String,
        data_type_path_list As List(Of String))
        MyBase.New(a_controller, name_field, uuid_field, description_field)

        Dim base_type_title As New Label
        base_type_title.Text = "Base Data_Type"
        base_type_title.Location = New Point(Left_Margin, 220)
        base_type_title.Size = Edition_Form.Title_Size
        Me.Controls.Add(base_type_title)

        Me.Base_Type_Ref_ComboBox = New ComboBox
        Base_Type_Ref_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList
        If Not IsNothing(data_type_path_list) Then
            Dim data_type_path As String
            For Each data_type_path In data_type_path_list
                Me.Base_Type_Ref_ComboBox.Items.Add(data_type_path)
            Next
        Else
            Me.Base_Type_Ref_ComboBox.Items.Add(base_type_ref_path)
        End If
        Me.Base_Type_Ref_ComboBox.Text = base_type_ref_path
        Me.Base_Type_Ref_ComboBox.Location = New Point(Field_Abscissa, 220)
        Me.Base_Type_Ref_ComboBox.Size = New Size(Field_Width, Title_Height)
        Me.Controls.Add(Me.Base_Type_Ref_ComboBox)

        Me.Set_Height(340)

    End Sub

End Class


'=================================================================================================='
' Contextual menu for predefined (read only) element.
'=================================================================================================='
Public Class Predefined_Element_Browser_Context_Menu

    Inherits Browser_Context_Menu

    Private WithEvents Sub_Context_Menu_View As ToolStripMenuItem

    Public Sub New(a_model_browser As Model_Browser)
        MyBase.New(a_model_browser)

        Me.Sub_Context_Menu_View = New ToolStripMenuItem
        Me.Sub_Context_Menu_View.Text = "View"

        Me.Items.AddRange(New ToolStripItem() {
            Me.Sub_Context_Menu_View})
    End Sub

    Private Sub View(sender As Object, e As EventArgs) Handles Sub_Context_Menu_View.Click
        Dim ctrl As Software_Element_Controller
        ctrl = CType(Get_Controller(), Software_Element_Controller)
        If Not IsNothing(ctrl) Then
            ctrl.View_Element_Context_Menu_Clicked()
        End If
    End Sub

End Class


'=================================================================================================='
' Default contextual menu for standard Software_Elements.
' Shall be inherited by all other contextual menus.
'=================================================================================================='
Public Class Software_Element_Browser_Context_Menu

    Inherits Browser_Context_Menu

    Private WithEvents Sub_Context_Menu_View As ToolStripMenuItem
    Private WithEvents Sub_Context_Menu_Edit As ToolStripMenuItem
    Private WithEvents Sub_Context_Menu_Move As ToolStripMenuItem
    Private WithEvents Sub_Context_Menu_Delete As ToolStripMenuItem

    Public Sub New(a_model_browser As Model_Browser)
        MyBase.New(a_model_browser)

        Me.Sub_Context_Menu_View = New ToolStripMenuItem
        Me.Sub_Context_Menu_View.Text = "View"

        Me.Sub_Context_Menu_Edit = New ToolStripMenuItem
        Me.Sub_Context_Menu_Edit.Text = "Edit"

        Me.Sub_Context_Menu_Move = New ToolStripMenuItem
        Me.Sub_Context_Menu_Move.Text = "Move"

        Me.Sub_Context_Menu_Delete = New ToolStripMenuItem
        Me.Sub_Context_Menu_Delete.Text = "Delete"

        Me.Items.AddRange(New ToolStripItem() {
            Me.Sub_Context_Menu_View,
            Me.Sub_Context_Menu_Edit,
            Me.Sub_Context_Menu_Move,
            Me.Sub_Context_Menu_Delete})

    End Sub

    Private Sub View_Element(sender As Object, e As EventArgs) Handles Sub_Context_Menu_View.Click
        Dim ctrl As Software_Element_Controller
        ctrl = CType(Get_Controller(), Software_Element_Controller)
        If Not IsNothing(ctrl) Then
            ctrl.View_Element_Context_Menu_Clicked()
        End If
    End Sub


    Private Sub Edit_Element(sender As Object, e As EventArgs) Handles Sub_Context_Menu_Edit.Click
        Dim ctrl As Controller = Get_Controller()
        If Not IsNothing(ctrl) Then
            ctrl.Edit_Context_Menu_Clicked()
        End If
    End Sub

    Private Sub Move_Element(sender As Object, e As EventArgs) Handles Sub_Context_Menu_Move.Click
        Dim ctrl As Software_Element_Controller
        ctrl = CType(Get_Controller(), Software_Element_Controller)
        If Not IsNothing(ctrl) Then
            ctrl.Move_Context_Menu_Clicked()
        End If
    End Sub

    Private Sub Delete_Element(sender As Object, e As EventArgs) _
                                                               Handles Sub_Context_Menu_Delete.Click
        Dim ctrl As Software_Element_Controller
        ctrl = CType(Get_Controller(), Software_Element_Controller)
        If Not IsNothing(ctrl) Then
            ctrl.Delete_Context_Menu_Clicked()
        End If
    End Sub

End Class


'=================================================================================================='

'=================================================================================================='
Public Class Move_Window
    Inherits Form

    Private WithEvents Move_Button As Button

    Private Destination_Element As ComboBox

    Public Sub New(list As List(Of String))

        Me.Text = "Select new parent :"
        Dim box_size = New Size(300, 200)
        Me.ClientSize = box_size
        Me.FormBorderStyle = FormBorderStyle.Sizable
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MaximumSize = New Size(800, 200)
        Me.MinimumSize = box_size

        Me.Destination_Element = New ComboBox
        Dim path As String
        For Each path In list
            Me.Destination_Element.Items.Add(path)
        Next
        Me.Destination_Element.Anchor = AnchorStyles.Right Or AnchorStyles.Left
        Me.Destination_Element.Location = New Point(20, 20)
        Me.Destination_Element.Size = New Size(240, 25)
        Me.Controls.Add(Me.Destination_Element)

        Me.Move_Button = New Button
        Me.Move_Button.Text = "Move"
        Me.Move_Button.Location = New Point(110, 100)
        Me.Controls.Add(Me.Move_Button)

    End Sub

    Private Sub Move_Pressed(sender As Object, e As EventArgs) Handles Move_Button.Click
        Me.Close()
    End Sub

    Public Function Get_Destination() As String
        Return Me.Destination_Element.Text
    End Function

End Class


'=================================================================================================='

'=================================================================================================='
Public MustInherit Class Software_Element_Square_Figure

    Inherits Panel

    Protected Drawn_Element_Ctrl As Software_Element_Controller
    Protected Diagram_Element As Square_Model_Diagram_Element

    Private WithEvents Meta_Class_Box As Label
    Private WithEvents Name_Box As TextBox
    Private WithEvents Description_Box As RichTextBox

    Protected Border_Color As Pen = Pens.Black
    Private Const Minimun_Width As Integer = 200
    Private Const Minimun_Height As Integer = 200
    Protected Const Box_Height As Integer = 20
    Protected Const Horizontal_Marge As Integer = 4
    Protected Const Double_Horizontal_Marge As Integer = Horizontal_Marge * 2
    Protected Vertical_Pos As Integer = 0
    Protected Const Vertical_Marge As Integer = 3

    Private Splitter_1_Ordinate As Integer = 0
    Private Splitter_2_Ordinate As Integer = 0
    Private Splitter_3_Ordinate As Integer = 0

    Private X_Cliqued As Integer = 0
    Private Width_Clicked As Integer = 0
    Private Is_Resizing_Width As Boolean = False

    Private Y_Cliqued As Integer = 0
    Private Height_Clicked As Integer = 0
    Private Is_Resizing_Height As Boolean = False

    Private Loc_X_Cliqued As Integer = 0
    Private Loc_Y_Cliqued As Integer = 0
    Private Is_Moving As Boolean = False

    Public Sub New(a_ctrl As Software_Element_Controller,
            a_diagram_element As Square_Model_Diagram_Element,
            a_name As String,
            a_description As String)

        Me.SuspendLayout()

        Me.ResizeRedraw = True

        Me.Drawn_Element_Ctrl = a_ctrl
        Me.Diagram_Element = a_diagram_element

        Me.Dock = DockStyle.None
        Me.Size = New Size(Me.Diagram_Element.Width, Me.Diagram_Element.Height)
        Me.Location = New Point(Diagram_Element.Abscissa, Diagram_Element.Ordinate)

        ' Add top line of the square
        Vertical_Pos = 1 + Vertical_Marge

        ' Add the display of the meta-class of the element
        Me.Meta_Class_Box = New Label
        Me.Controls.Add(Me.Meta_Class_Box)
        Me.Meta_Class_Box.Location = New Point(Horizontal_Marge, Vertical_Pos)
        Me.Meta_Class_Box.Width = Me.Diagram_Element.Width - Double_Horizontal_Marge
        Me.Meta_Class_Box.Height = Box_Height
        Me.Meta_Class_Box.BorderStyle = Windows.Forms.BorderStyle.None
        Me.Meta_Class_Box.BackColor = Color.White
        Me.Meta_Class_Box.Text = "<< " & " >>"
        Me.Meta_Class_Box.TextAlign = ContentAlignment.MiddleCenter
        Dim font = New Font(Me.Meta_Class_Box.Font, FontStyle.Italic)
        Me.Meta_Class_Box.Font = font
        Vertical_Pos += Box_Height + Vertical_Marge

        ' Add horizontal separator between meta-class and name
        Splitter_1_Ordinate = Vertical_Pos
        Vertical_Pos += 1 + Vertical_Marge

        ' Add the display of the name of the element
        Me.Name_Box = New TextBox
        Me.Controls.Add(Me.Name_Box)
        Me.Name_Box.Location = New Point(Horizontal_Marge, Vertical_Pos)
        Me.Name_Box.Width = Me.Diagram_Element.Width - Double_Horizontal_Marge
        Me.Name_Box.Height = Box_Height
        Me.Name_Box.BorderStyle = Windows.Forms.BorderStyle.None
        Me.Name_Box.Text = a_name
        Me.Name_Box.Font = New Font("Microsoft Sans Serif", 12)
        Me.Name_Box.TextAlign = HorizontalAlignment.Center
        Vertical_Pos += Box_Height + Vertical_Marge
        Me.Drawn_Element_Ctrl.Get_View.Add_Name_Diagram_Box(Me.Name_Box)

        ' Add horizontal separator between name and description
        Splitter_2_Ordinate = Vertical_Pos
        Vertical_Pos += 1 + Vertical_Marge

        ' Add the display of the description of the element
        Me.Description_Box = New RichTextBox
        Me.Controls.Add(Me.Description_Box)
        Me.Description_Box.Location = New Point(Horizontal_Marge, Vertical_Pos)
        Me.Description_Box.Width = Me.Diagram_Element.Width - Double_Horizontal_Marge
        Me.Description_Box.Height = 4 * Box_Height
        Me.Description_Box.BorderStyle = Windows.Forms.BorderStyle.None
        Me.Description_Box.Text = a_description
        Vertical_Pos += 4 * Box_Height + Vertical_Marge
        Me.Drawn_Element_Ctrl.Get_View.Add_Description_Diagram_Box(Me.Description_Box)

        ' Add horizontal separator between description and children/attributes
        Splitter_3_Ordinate = Vertical_Pos
        Vertical_Pos += 1 + Vertical_Marge

        Me.ResumeLayout()
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        ' To "delete" previous lines
        e.Graphics.FillRectangle(New SolidBrush(Color.White), _
                                 Horizontal_Marge, 1, Size.Width - 1, Size.Height - 1)

        ' External rectangle
        e.Graphics.DrawRectangle(Border_Color, 0, 0, Size.Width - 1, Size.Height - 1)

        ' Horizontal line between meta-class and name
        e.Graphics.DrawLine(Border_Color, 1, Splitter_1_Ordinate, Size.Width, Splitter_1_Ordinate)

        ' Horizontal line between name and description
        e.Graphics.DrawLine(Border_Color, 1, Splitter_2_Ordinate, Size.Width, Splitter_2_Ordinate)

        ' Horizontal line between description and children/attributes
        e.Graphics.DrawLine(Border_Color, 1, Splitter_3_Ordinate, Size.Width, Splitter_3_Ordinate)
    End Sub

    Private Sub Me_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) _
            Handles Me.MouseDown, Name_Box.MouseDown, Description_Box.MouseDown, Meta_Class_Box.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Then
            If e.X >= Me.Width - 5 And e.X <= Me.Width Then
                Is_Resizing_Height = False
                Is_Resizing_Width = True
                Is_Moving = False
                Width_Clicked = Me.Width
                X_Cliqued = e.X
            ElseIf e.Y >= Me.Height - 5 And e.Y <= Me.Height Then
                Is_Resizing_Width = False
                Is_Resizing_Height = True
                Is_Moving = False
                Height_Clicked = Me.Height
                Y_Cliqued = e.Y
            ElseIf e.X >= 0 And e.X <= Me.Width And e.Y >= 0 And e.Y <= 20 Then
                Is_Moving = True
                Loc_X_Cliqued = Me.Location.X
                X_Cliqued = e.X
                Loc_Y_Cliqued = Me.Location.Y
                Y_Cliqued = e.Y
            Else
                Is_Resizing_Height = False
                Is_Resizing_Width = False
                Is_Moving = False
            End If
        End If
    End Sub

    Private Sub Me_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) _
            Handles Me.MouseUp, Name_Box.MouseUp, Description_Box.MouseUp, Meta_Class_Box.MouseUp
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Is_Resizing_Width = False
            Is_Resizing_Height = False
            Is_Moving = False
        End If
    End Sub

    Private Sub Me_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) _
            Handles Me.MouseMove, Name_Box.MouseMove, _
                    Description_Box.MouseMove, Meta_Class_Box.MouseMove

        If Is_Resizing_Width = True Then
            Dim requested_new_width As Integer
            requested_new_width = Width_Clicked + e.X - X_Cliqued
            Dim new_width As Integer
            new_width = Math.Max(Minimun_Width, requested_new_width)
            Me.Resize_Figure_Width(new_width)

        ElseIf Is_Resizing_Height = True Then
            Dim requested_new_height As Integer
            requested_new_height = Height_Clicked + e.Y - Y_Cliqued
            Dim new_height As Integer
            new_height = Math.Max(Minimun_Height, requested_new_height)
            Me.Resize_Figure_Heigth(new_height)

        ElseIf Is_Moving = True Then
            Dim new_loc_x As Integer = Math.Max(0, Loc_X_Cliqued + e.X - X_Cliqued)
            Dim new_loc_y As Integer = Math.Max(0, Loc_Y_Cliqued + e.Y - Y_Cliqued)
            Me.Location = New Point(new_loc_x, new_loc_y)
            Me.Diagram_Element.Abscissa = new_loc_x
            Me.Diagram_Element.Ordinate = new_loc_y
            Me.Drawn_Element_Ctrl.Set_Top_Level_Package_Controller_Status_To_Modified()
        End If

    End Sub

    Protected Overridable Sub Resize_Figure_Width(new_width As Integer)
        Me.Meta_Class_Box.Width = new_width - Double_Horizontal_Marge
        Me.Name_Box.Width = new_width - Double_Horizontal_Marge
        Me.Description_Box.Width = new_width - Double_Horizontal_Marge
        Me.Width = new_width
        Me.Diagram_Element.Width = new_width
        Me.Drawn_Element_Ctrl.Set_Top_Level_Package_Controller_Status_To_Modified()
    End Sub

    Protected Overridable Sub Resize_Figure_Heigth(new_height As Integer)
        Me.Diagram_Element.Height = new_height
        Me.Drawn_Element_Ctrl.Set_Top_Level_Package_Controller_Status_To_Modified()
        Me.Height = new_height
    End Sub

    Private Sub Name_Edited(ByVal sender As Object, ByVal e As EventArgs) Handles Name_Box.Leave
        Me.Drawn_Element_Ctrl.Name_Edited_Within_Diagram(Me.Name_Box.Text)
    End Sub

    Private Sub Description_Edited(ByVal sender As Object, ByVal e As EventArgs) _
                                                                       Handles Description_Box.Leave
        Me.Drawn_Element_Ctrl.Description_Edited_Within_Diagram(Me.Description_Box.Text)
    End Sub

End Class