Public Class Main_Window

    Inherits Form

    Private My_Controller As Software_Project_Controller

    Private WithEvents Vertical_Split_Container As SplitContainer
    Public WithEvents My_Model_Browser As Model_Browser
    Private WithEvents My_Diagram_Area As Diagram_Area
    Private WithEvents Main_Menu As MenuStrip
    Private WithEvents Project_Menu As ToolStripMenuItem
    Private WithEvents Sub_Menu_Load_Existing_Poject As ToolStripMenuItem
    Private WithEvents Sub_Menu_New_Project As ToolStripMenuItem

    Public Sub New(a_ctrl As Software_Project_Controller)
        Me.My_Controller = a_ctrl
        Initialize()
    End Sub

    Private Sub Initialize()

        Me.Vertical_Split_Container = New SplitContainer
        My_Diagram_Area = New Diagram_Area
        Me.My_Model_Browser = New Model_Browser
        Me.Main_Menu = New MenuStrip
        Me.Project_Menu = New ToolStripMenuItem
        Me.Sub_Menu_Load_Existing_Poject = New ToolStripMenuItem
        Me.Sub_Menu_New_Project = New ToolStripMenuItem
        SuspendLayout()

        '------------------------------------------------------------------------------------------'
        ' Form
        '------------------------------------------------------------------------------------------'
        Me.Name = "SMT_Main_Form"
        Me.Text = "Software Modeling Tool"
        Me.ClientSize = New System.Drawing.Size(1000, 750)


        '------------------------------------------------------------------------------------------'
        ' Menu bar
        '------------------------------------------------------------------------------------------'
        Me.Controls.Add(Me.Main_Menu)
        Me.Main_Menu.Items.AddRange(New ToolStripItem() {Me.Project_Menu})
        Me.Project_Menu.Text = "Project"
        Me.Project_Menu.DropDownItems.AddRange( _
                    New ToolStripItem() {Me.Sub_Menu_Load_Existing_Poject, Me.Sub_Menu_New_Project})
        Me.Sub_Menu_Load_Existing_Poject.Text = "Load existing project"
        Me.Sub_Menu_New_Project.Text = "New project"


        '------------------------------------------------------------------------------------------'
        ' Vertical split
        '------------------------------------------------------------------------------------------'
        Me.Controls.Add(Me.Vertical_Split_Container)
        Me.Vertical_Split_Container.Name = "Vertical_Split_Container"
        Me.Vertical_Split_Container.Dock = DockStyle.Fill
        ' Left Panel
        Me.Vertical_Split_Container.Panel1.Controls.Add(My_Model_Browser)
        ' Right Panel
        Me.Vertical_Split_Container.Panel2.Controls.Add(My_Diagram_Area)


        '------------------------------------------------------------------------------------------'
        ' Model browser
        '------------------------------------------------------------------------------------------'
        My_Model_Browser.Dock = DockStyle.Fill
        My_Model_Browser.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or _
                                           AnchorStyles.Left Or AnchorStyles.Right
        My_Model_Browser.Location = New Point(0, 25)


        '------------------------------------------------------------------------------------------'
        ' Diagram
        '------------------------------------------------------------------------------------------'
        My_Diagram_Area.Dock = DockStyle.Fill
        My_Diagram_Area.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or _
                                 AnchorStyles.Left Or AnchorStyles.Right
        My_Diagram_Area.Location = New Point(0, 25)


        ResumeLayout(False)

    End Sub

    Public Sub Clear_Model_Browser()
        My_Model_Browser.Nodes.Clear()
    End Sub

    Function Add_New_Diagram_Page(diagram_name As String) As TabPage
        Dim new_page As New TabPage
        new_page.Text = diagram_name
        My_Diagram_Area.Controls.Add(new_page)
        Return new_page
    End Function

    Public Sub Clear_Diagram_Area()

    End Sub



    Private Sub Load_Project_Clicked(sender As Object, e As EventArgs) _
                                                         Handles Sub_Menu_Load_Existing_Poject.Click
        Me.My_Controller.Load_Project_Clicked()
    End Sub

    Private Sub New_Project_Clicked(sender As Object, e As EventArgs) _
                                                                  Handles Sub_Menu_New_Project.Click
        Me.My_Controller.New_Project_Clicked()
    End Sub

    Private Sub Node_Clicked(sender As Object, e As TreeNodeMouseClickEventArgs) Handles _
        My_Model_Browser.NodeMouseClick

        ' Set the right clicked node as the selected node
        My_Model_Browser.SelectedNode = e.Node

        If e.Button = Right Then
            Dim pt As New Point(e.Node.Bounds.X, e.Node.Bounds.Y)
            e.Node.ContextMenuStrip.Show(My_Model_Browser, pt)
        End If

    End Sub

End Class


Public Class Model_Browser

    Inherits TreeView

    Public Model_Container_ContextMenu As Project_Browser_Context_Menu
    Public Predefined_Elmt_CtxtMenu As Predefined_Element_Browser_Context_Menu
    Public Elmt_CtxtMenu As Software_Element_Browser_Context_Menu
    Public Top_Level_Pkg_CtxtMenu As Top_Level_Package_Browser_Context_Menu
    Public Pkg_CtxtMenu As Package_Browser_Context_Menu
    Public Model_Diagram_CtxtMenu As Model_Diagram_Context_Menu
    Public Enum_CtxtMenu As Enumerated_Data_Type_Browser_Context_Menu
    Public Structure_CtxtMenu As Structured_Data_Type_Browser_Context_Menu

    Public Sub New()

        Dim icon_list As New ImageList
        Dim my_icon As Icon
        Try
            my_icon = New Icon("Default_Icon.ico")
            icon_list.Images.Add("Software_Element", my_icon)

            my_icon = New Icon("Project.ico")
            icon_list.Images.Add("Model_Container", my_icon)

            my_icon = New Icon("Blue_Package.ico")
            icon_list.Images.Add("Package", my_icon)

            my_icon = New Icon("Diagram.ico")
            icon_list.Images.Add("Diagram", my_icon)

            my_icon = New Icon("Basic_Type.ico")
            icon_list.Images.Add("Basic_Type", my_icon)

            my_icon = New Icon("Enumerated_Data_Type.ico")
            icon_list.Images.Add("Enumerated_Data_Type", my_icon)

            my_icon = New Icon("Enumerated_Data_Type_Enumeral.ico")
            icon_list.Images.Add("Enumerated_Data_Type_Enumeral", my_icon)

            my_icon = New Icon("Array_Data_Type.ico")
            icon_list.Images.Add("Array_Data_Type", my_icon)

            my_icon = New Icon("Physical_Data_Type.ico")
            icon_list.Images.Add("Physical_Data_Type", my_icon)

            my_icon = New Icon("Structured_Data_Type.ico")
            icon_list.Images.Add("Structured_Data_Type", my_icon)

            ImageList = icon_list
        Catch

        End Try

        Model_Container_ContextMenu = New Project_Browser_Context_Menu(Me)
        Predefined_Elmt_CtxtMenu = New Predefined_Element_Browser_Context_Menu(Me)
        Elmt_CtxtMenu = New Software_Element_Browser_Context_Menu(Me)
        Top_Level_Pkg_CtxtMenu = New Top_Level_Package_Browser_Context_Menu(Me)
        Model_Diagram_CtxtMenu = New Model_Diagram_Context_Menu(Me)
        Enum_CtxtMenu = New Enumerated_Data_Type_Browser_Context_Menu(Me)
        Structure_CtxtMenu = New Structured_Data_Type_Browser_Context_Menu(Me)
        Pkg_CtxtMenu = New Package_Browser_Context_Menu(Me)
    End Sub

End Class


Public Class Diagram_Area

    Inherits TabControl



End Class
