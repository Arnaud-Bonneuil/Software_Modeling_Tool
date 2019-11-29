'=================================================================================================='

'=================================================================================================='
Public MustInherit Class View

    Protected Node As TreeNode

    Public Function Get_Node() As TreeNode
        Return Me.Node
    End Function

    Public Overridable Sub Update_All_Name_Views(new_name As String)
        ' Update Model_Browser
        Me.Node.Text = new_name

    End Sub

    Public Overridable Sub Update_All_Description_Views(new_description As String)

    End Sub

    Public Overridable Sub Delete_All_View()
        ' Remove model browser view
        Me.Node.Remove()

        ' Remove diagram views

    End Sub

End Class


'=================================================================================================='
' Abstract form that gathers shared elements : Name + Description
'=================================================================================================='
Public MustInherit Class Edition_Form

    Inherits Form

    Protected My_Controller As Controller

    Private WithEvents Apply_Button As Button

    Public Name_TextBox As TextBox
    Public Description_TextBox As RichTextBox

    Protected Shared Left_Margin As Integer = 10
    Protected Shared Title_Height As Integer = 25
    Protected Shared Title_Size As Size = New Size(100, Title_Height)
    Protected Shared Field_Abscissa As Integer = 120
    Protected Shared Field_Width As Integer = 350

    Public Sub New(
        a_controller As Controller,
        name_field As String,
        description_field As String)

        My_Controller = a_controller

        Me.Text = "Edit Software_Element"
        Dim box_size = New Size(500, 600)
        Me.ClientSize = box_size
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MaximumSize = box_size
        Me.MinimumSize = box_size

        Dim name_title As New Label
        name_title.Text = "Name"
        name_title.Location = New Point(Left_Margin, 20)
        name_title.Size = Edition_Form.Title_Size
        Me.Controls.Add(name_title)

        Me.Name_TextBox = New TextBox
        Me.Name_TextBox.Text = name_field
        Me.Name_TextBox.Location = New Point(Field_Abscissa, 20)
        Me.Name_TextBox.Size = New Size(Field_Width, Title_Height)
        Me.Controls.Add(Me.Name_TextBox)

        Dim description_title As New Label
        description_title.Text = "Description"
        description_title.Location = New Point(Left_Margin, 60)
        description_title.Size = Edition_Form.Title_Size
        Me.Controls.Add(description_title)

        Me.Description_TextBox = New RichTextBox
        Me.Description_TextBox.Text = description_field
        Me.Description_TextBox.Location = New Point(Field_Abscissa, 60)
        Me.Description_TextBox.Size = New Size(Field_Width, 100)
        Me.Controls.Add(Me.Description_TextBox)

        Me.Apply_Button = New Button
        Me.Apply_Button.Text = "Apply"
        Me.Apply_Button.Location = New Point(200, 500)
        Me.Controls.Add(Apply_Button)

        Me.Name_TextBox.Select()

    End Sub

    Private Sub Apply_Pressed(sender As Object, e As EventArgs) Handles Apply_Button.Click
        My_Controller.Edition_Window_Apply_Button_Clicked(Me)
        Me.Close()
    End Sub
End Class


'=================================================================================================='
' Abstract class defining the basic content of a contextual menu
'=================================================================================================='
Public MustInherit Class Browser_Context_Menu

    Inherits ContextMenuStrip

    Protected My_Model_Browser As Model_Browser

    Public Sub New(a_model_browser As Model_Browser)
        My_Model_Browser = a_model_browser
    End Sub

    Protected Function Get_Controller() As Controller
        Dim ctrl As Controller = Nothing
        Dim selected_node As TreeNode
        selected_node = My_Model_Browser.SelectedNode
        If Not IsNothing(selected_node) Then
            ctrl = CType(selected_node.Tag, Controller)
        End If
        Return ctrl
    End Function

End Class