'=================================================================================================='

'=================================================================================================='
Public Class Software_Project_View

    Inherits View

    Private My_Controller As Software_Project_Controller
    Public Main_Window As Main_Window

    Public Sub New(a_ctrl As Software_Project_Controller)
        Me.My_Controller = a_ctrl
        Me.Main_Window = New Main_Window(Me.My_Controller)
    End Sub

    Public Sub Display_Main_Window()
        Application.EnableVisualStyles()
        Application.Run(Me.Main_Window)
    End Sub

    Public Function Display_Project_Selection_Form() As String
        Dim project_file_path As String = ""
        Dim dialog_box = New OpenFileDialog
        dialog_box.Title = "Select software model project file"
        dialog_box.Filter = "XML file|*.xml"
        dialog_box.CheckFileExists = True
        Dim result As DialogResult = dialog_box.ShowDialog
        If result = DialogResult.OK Then
            project_file_path = dialog_box.FileName
        End If
        Return project_file_path
    End Function

    Public Sub Display_Project_Creation_Form()
        Dim edit_form As New New_Project_Form(
            My_Controller,
            "New_Project",
            "A good description is always useful",
            ".xml")
        edit_form.ShowDialog()
    End Sub

    Public Sub Display_Project_File_Not_Found()
        MsgBox("The project file is not found !", MsgBoxStyle.Critical, "Error")
    End Sub

    Public Sub Display_Project_File_Is_Invalid()
        MsgBox("The project file is invalid !", MsgBoxStyle.Critical, "Error")
    End Sub

    Public Sub Display_Package_File_Not_Found(pkg_path_list As List(Of String))
        Dim pkg_path As String
        For Each pkg_path In pkg_path_list
            MsgBox("Folowing file path not found : " & vbCrLf & pkg_path,
                MsgBoxStyle.Exclamation,
                "Warning")
        Next
    End Sub

    Public Sub Display_Invalid_Package_File(pkg_path_list As List(Of String))
        Dim pkg_path As String
        For Each pkg_path In pkg_path_list
            MsgBox("Folowing package file content is invalid : " & vbCrLf & pkg_path,
                MsgBoxStyle.Critical,
                "Error")
        Next
    End Sub

    Public Function Display_Add_Existing_Package_Form() As String
        Dim choose_file_box As New OpenFileDialog
        choose_file_box.Title = "Choose package file"
        choose_file_box.CheckFileExists = True
        choose_file_box.CheckPathExists = True
        choose_file_box.DefaultExt = ".xml"
        choose_file_box.Filter = "Package file |*.xml"
        choose_file_box.Multiselect = False
        Dim xml_pkg_file_path As String
        Dim dialog_result As DialogResult
        dialog_result = choose_file_box.ShowDialog()
        xml_pkg_file_path = choose_file_box.SafeFileName
        Return xml_pkg_file_path
    End Function

    Public Sub Display_Add_New_Package_Form()
        Dim new_form As New New_Top_Level_Package_Form(
            My_Controller,
            "New_Package",
            "A good description is always useful.",
            ".xml")
        new_form.ShowDialog()
    End Sub

    Public Sub Clear_Main_Window()
        Me.Main_Window.Clear_Model_Browser()
        Me.Main_Window.Clear_Diagram_Area()
    End Sub

    Public Sub Initialize_Project_View(a_name As String)
        Node = New TreeNode(a_name)
        Main_Window.My_Model_Browser.Nodes.Add(Node)
        Node.ImageKey = "Model_Container"
        Node.SelectedImageKey = "Model_Container"
        Node.ContextMenuStrip = CType(Node.TreeView, Model_Browser).Model_Container_ContextMenu
        Node.Tag = My_Controller
    End Sub

    Public Overrides Sub Update_All_Name_Views(new_name As String)
        MyBase.Update_All_Name_Views(new_name)

    End Sub

    Public Overrides Sub Update_All_Description_Views(new_description As String)
        MyBase.Update_All_Description_Views(new_description)
    End Sub

    Sub Display_Is_Modified(name As String)
        Me.Node.Text = name & " *"
    End Sub

    Sub Display_Is_Saved(name As String)
        Me.Node.Text = name
    End Sub

    Sub Display_Name_Is_Invalid()
        MsgBox("The project name is invalid !", MsgBoxStyle.Critical, "Error")
    End Sub

    Sub Display_Description_Is_Invalid()
        MsgBox("The project description name is invalid !", MsgBoxStyle.Critical, "Error")
    End Sub

    Sub Display_New_Package_Name_Is_Invalid()
        MsgBox("The new Package name is invalid !", MsgBoxStyle.Critical, "Error")
    End Sub

    Sub Display_New_Package_Description_Is_Invalid()
        MsgBox("The new Package description is invalid !", MsgBoxStyle.Critical, "Error")
    End Sub

    Sub Display_New_Package_File_Path_Is_Invalid()
        MsgBox("The new Package file path is invalid !", MsgBoxStyle.Critical, "Error")
    End Sub

End Class


'=================================================================================================='

'=================================================================================================='
Public Class Project_Browser_Context_Menu

    Inherits Browser_Context_Menu

    Private WithEvents Sub_Context_Menu_Edit As ToolStripMenuItem
    Private WithEvents Sub_Context_Menu_Save As ToolStripMenuItem
    Private WithEvents Sub_Context_Menu_Add_Existing_Package As ToolStripMenuItem
    Private WithEvents Sub_Context_Menu_Add_New_Package As ToolStripMenuItem

    Public Sub New(a_model_browser As Model_Browser)
        MyBase.New(a_model_browser)

        Me.Sub_Context_Menu_Edit = New ToolStripMenuItem
        Me.Sub_Context_Menu_Edit.Text = "Edit"

        Me.Sub_Context_Menu_Save = New ToolStripMenuItem
        Me.Sub_Context_Menu_Save.Text = "Save"

        Me.Sub_Context_Menu_Add_Existing_Package = New ToolStripMenuItem
        Me.Sub_Context_Menu_Add_Existing_Package.Text = "Add existing Package"

        Me.Sub_Context_Menu_Add_New_Package = New ToolStripMenuItem
        Me.Sub_Context_Menu_Add_New_Package.Text = "Add new Package"

        Me.Items.AddRange(New ToolStripItem() {
            Me.Sub_Context_Menu_Edit,
            Me.Sub_Context_Menu_Save,
            New ToolStripSeparator,
            Sub_Context_Menu_Add_Existing_Package,
            Sub_Context_Menu_Add_New_Package})

    End Sub

    Private Sub Edit(sender As Object, e As EventArgs) Handles Sub_Context_Menu_Edit.Click
        Dim ctrl As Software_Project_Controller
        ctrl = CType(Get_Controller(), Software_Project_Controller)
        If Not IsNothing(ctrl) Then
            ctrl.Edit_Context_Menu_Clicked()
        End If
    End Sub

    Private Sub Save(sender As Object, e As EventArgs) Handles Sub_Context_Menu_Save.Click
        Dim ctrl As Software_Project_Controller
        ctrl = CType(Get_Controller(), Software_Project_Controller)
        If Not IsNothing(ctrl) Then
            ctrl.Save_Context_Menu_Clicked()
        End If
    End Sub

    Private Sub Add_Existing_Package(sender As Object, e As EventArgs) _
                                                 Handles Sub_Context_Menu_Add_Existing_Package.Click
        Dim ctrl As Software_Project_Controller
        ctrl = CType(Get_Controller(), Software_Project_Controller)
        If Not IsNothing(ctrl) Then
            ctrl.Add_Existing_Package_Context_Menu_Clicked()
        End If
    End Sub

    Private Sub Add_New_Package(sender As Object, e As EventArgs) _
                                                      Handles Sub_Context_Menu_Add_New_Package.Click
        Dim ctrl As Software_Project_Controller
        ctrl = CType(Get_Controller(), Software_Project_Controller)
        If Not IsNothing(ctrl) Then
            ctrl.Add_New_Package_Context_Menu_Clicked()
        End If
    End Sub

End Class


'=============================================================================================='
' Form for project/ top level package creation
'=============================================================================================='
Public MustInherit Class New_Top_Element_Form
    Inherits Form

    Protected My_Controller As Software_Project_Controller
    Protected Name_TextBox As TextBox
    Protected Description_TextBox As RichTextBox
    Protected XML_File_Path_TextBox As TextBox

    Protected WithEvents Create_Button As Button

    Shared Title_Size As Size = New Size(80, 20)

    Public Sub New(
        prj_ctrl As Software_Project_Controller,
        name_field As String,
        description_field As String,
        xml_file_path_field As String)

        Me.My_Controller = prj_ctrl

        Dim box_size = New Size(500, 300)
        Me.ClientSize = box_size
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MaximumSize = box_size
        Me.MinimumSize = box_size

        Dim name_title As New Label
        name_title.Text = "Name"
        name_title.Location = New Point(10, 20)
        name_title.Size = New_Top_Element_Form.Title_Size
        Me.Controls.Add(name_title)

        Me.Name_TextBox = New TextBox
        Me.Name_TextBox.Text = name_field
        Me.Name_TextBox.Location = New Point(100, 20)
        Me.Name_TextBox.Size = New Size(370, 20)
        Me.Controls.Add(Me.Name_TextBox)

        Dim description_title As New Label
        description_title.Text = "Description"
        description_title.Location = New Point(10, 60)
        description_title.Size = New_Top_Element_Form.Title_Size
        Me.Controls.Add(description_title)

        Me.Description_TextBox = New RichTextBox
        Me.Description_TextBox.Text = description_field
        Me.Description_TextBox.Location = New Point(100, 60)
        Me.Description_TextBox.Size = New Size(370, 100)
        Me.Controls.Add(Me.Description_TextBox)

        Dim xml_file_path_title As New Label
        xml_file_path_title.Text = "Path"
        xml_file_path_title.Location = New Point(10, 180)
        xml_file_path_title.Size = New_Top_Element_Form.Title_Size
        Me.Controls.Add(xml_file_path_title)

        Me.XML_File_Path_TextBox = New TextBox
        Me.XML_File_Path_TextBox.Text = xml_file_path_field
        Me.XML_File_Path_TextBox.Location = New Point(100, 180)
        Me.XML_File_Path_TextBox.Size = New Size(370, 20)
        Me.Controls.Add(Me.XML_File_Path_TextBox)

        Me.Create_Button = New Button
        Me.Create_Button.Text = "Create"
        Me.Create_Button.Location = New Point(212, 220)
        Me.Controls.Add(Create_Button)

        Me.Name_TextBox.Select()

    End Sub

End Class


'=============================================================================================='
Public Class New_Project_Form

    Inherits New_Top_Element_Form

    Public Sub New(
        prj_ctrl As Software_Project_Controller,
        name_field As String,
        description_field As String,
        xml_file_path_field As String)
        MyBase.New(prj_ctrl, name_field, description_field, xml_file_path_field)

        Me.Text = "Create a new project"

    End Sub

    Private Sub Create_Pressed(sender As Object, e As EventArgs) Handles Create_Button.Click
        My_Controller.New_Project_Create_Button_Clicked(
            Me.Name_TextBox.Text,
            Me.Description_TextBox.Text,
            Me.XML_File_Path_TextBox.Text)
        Me.Close()
    End Sub

End Class


'=============================================================================================='
Public Class New_Top_Level_Package_Form

    Inherits New_Top_Element_Form

    Public Sub New(
        prj_ctrl As Software_Project_Controller,
        name_field As String,
        description_field As String,
        xml_file_path_field As String)
        MyBase.New(prj_ctrl, name_field, description_field, xml_file_path_field)

        Me.Text = "Create a new package"

    End Sub

    Private Sub Create_Pressed(sender As Object, e As EventArgs) Handles Create_Button.Click
        My_Controller.Add_New_Package_Create_Button_Clicked(
            Me.Name_TextBox.Text,
            Me.Description_TextBox.Text,
            Me.XML_File_Path_TextBox.Text)
        Me.Close()
    End Sub

End Class