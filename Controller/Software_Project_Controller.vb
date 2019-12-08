Imports System.IO


Public Class Software_Project_Controller

    Inherits Controller

    Private My_Project As Software_Project ' Reference to the loaded Software_Project
    Private My_View As New Software_Project_View(Me) ' Associated View (bidirectional link)
    Private Is_Project_Loaded As Boolean
    Private Loaded_Project_Xml_File As String
    Private Is_Project_Modified As Boolean
    Public My_Top_Level_Package_Controllers_List As List(Of Top_Level_Package_Controller)

    Public Controller_Dico_By_Element_UUID As New Dictionary(Of Guid, Software_Element_Controller)

    Public Sub Run()
        My_View.Display_Main_Window()
    End Sub


    '=============================================================================================='
    ' Methods for menu bar
    '=============================================================================================='
    Public Sub Load_Project_Clicked()
        ' Save and initialize all stuff if a project is already loaded
        If Me.Is_Project_Loaded = True Then
            Me.Save_All_And_Close()
            My_View.Clear_Main_Window()
        End If
        ' Load and display new project
        Dim project_file_path As String
        project_file_path = My_View.Display_Project_Selection_Form()
        If project_file_path <> "" Then ' User did not chose "Cancel" button
            Me.Load_And_Display_Project(project_file_path)
        Else
            ' Nothing to do
        End If
    End Sub

    Public Sub New_Project_Clicked()
        ' Save and initialize all stuff if a project is already loaded
        If Me.Is_Project_Loaded = True Then
            Me.Save_All_And_Close()
            My_View.Clear_Main_Window()
        End If
        ' Display a window for project creation
        My_View.Display_Project_Creation_Form()
    End Sub

    Public Sub New_Project_Create_Button_Clicked(
            prj_name As String,
            prj_description As String,
            prj_file_path As String)
        ' Initialize the Software_Project
        My_Project = New Software_Project
        My_Project.Name = prj_name
        My_Project.Description = prj_description
        My_Project.Packages_References = New List(Of Software_Package_Reference)
        My_Project.Tol_Level_Packages = New List(Of Top_Level_Package)

        Dim new_prj_file_stream As New FileStream(prj_file_path, FileMode.CreateNew)
        My_Project.Serialize_Project(new_prj_file_stream)
        new_prj_file_stream.Close()

        ' Set my (Software_Project_Controller) data
        Me.Loaded_Project_Xml_File = prj_file_path
        Me.Load_And_Display_Project(Me.Loaded_Project_Xml_File)

    End Sub


    '=============================================================================================='
    ' Methods for model browser contextual menu
    '=============================================================================================='
    Public Overrides Sub Edit_Context_Menu_Clicked()
        Dim edit_form As New Software_Element_Edition_Form(
            Me, My_Project.Name, Guid.Empty, My_Project.Description)
        edit_form.ShowDialog()
    End Sub

    Public Overrides Sub Edition_Window_Apply_Button_Clicked(edit_win As Edition_Form)
        Dim new_name As String = edit_win.Name_TextBox.Text
        Dim new_description As String = edit_win.Description_TextBox.Text

        ' Treat the new Name
        If My_Project.Is_Name_Valid(new_name) Then
            My_Project.Name = new_name
            My_View.Update_All_Name_Views(new_name)
        Else
            My_View.Display_Name_Is_Invalid()
        End If

        ' Treat the new Description
        If My_Project.Is_Description_Valid(new_description) Then
            My_Project.Description = new_description
            My_View.Update_All_Description_Views(new_description)
        Else
            My_View.Display_Description_Is_Invalid()
        End If

        Is_Project_Modified = True
        My_View.Display_Is_Modified(My_Project.Name)

    End Sub

    Public Overrides Sub Edition_Window_Closing(edition_form As Edition_Form)

    End Sub

    Public Sub Save_Context_Menu_Clicked()
        Save_Project()
        Save_All_Top_Level_Packages()
        My_View.Display_Is_Saved(My_Project.Name)
    End Sub

    Public Sub Add_Existing_Package_Context_Menu_Clicked()
        Dim new_pkg_file_path As String = ""
        new_pkg_file_path = My_View.Display_Add_Existing_Package_Form()
        If new_pkg_file_path <> "" Then

            ' Update project data
            Dim pkg_ref As New Software_Package_Reference
            pkg_ref.Path = new_pkg_file_path
            My_Project.Packages_References.Add(pkg_ref)
            Me.Is_Project_Modified = True

            ' Add package to model
            Dim new_pkg As Top_Level_Package

            Dim new_pkg_file_stream As New FileStream(new_pkg_file_path, FileMode.Open)
            new_pkg = Top_Level_Package.Deserialize_Package(new_pkg_file_stream)
            new_pkg_file_stream.Close()
            new_pkg.Xml_File_Path = new_pkg_file_path
            My_Project.Tol_Level_Packages.Add(new_pkg)
            new_pkg.Parent = Nothing
            new_pkg.Post_Treat_After_Xml_Deserialization(False)

            ' Update model browser
            My_View.Display_Is_Modified(My_Project.Name)
            Dim pkg_ctrl As New Top_Level_Package_Controller(Me, new_pkg, My_View)
            My_Top_Level_Package_Controllers_List.Add(pkg_ctrl)

        End If
    End Sub

    Public Sub Add_New_Package_Context_Menu_Clicked()
        My_View.Display_Add_New_Package_Form()
    End Sub

    Public Sub Add_New_Package_Create_Button_Clicked(
        pkg_name As String,
        pkg_description As String,
        pkg_file_path As String)

        Dim new_pkg As New Top_Level_Package
        new_pkg.Parent = Nothing

        ' TODO : test if the name of the package is unique among top level package of the project
        If Not new_pkg.Is_Name_Valid(pkg_name) Then
            My_View.Display_New_Package_Name_Is_Invalid()
        Else
            new_pkg.Name = pkg_name

            If new_pkg.Is_Description_Valid(pkg_description) Then
                new_pkg.Description = pkg_description
            Else
                My_View.Display_New_Package_Description_Is_Invalid()
            End If

            If Not new_pkg.Is_File_Path_Valid(pkg_file_path) Then
                My_View.Display_New_Package_File_Path_Is_Invalid()
            Else

                new_pkg.Create_UUID()
                new_pkg.Xml_File_Path = pkg_file_path

                ' Update project data
                Dim pkg_ref As New Software_Package_Reference
                pkg_ref.Path = pkg_file_path
                My_Project.Packages_References.Add(pkg_ref)
                Me.Is_Project_Modified = True

                ' Add package to model
                My_Project.Tol_Level_Packages.Add(new_pkg)
                new_pkg.Is_Read_Only = False

                ' Create Package file
                Dim new_pkg_file_stream As New FileStream(pkg_file_path, FileMode.CreateNew)
                ' TODO : catch exception if file already exit
                new_pkg.Serialize_Package(new_pkg_file_stream)
                new_pkg_file_stream.Close()

                ' Create controller
                My_View.Display_Is_Modified(My_Project.Name)
                Dim pkg_ctrl As New Top_Level_Package_Controller(Me, new_pkg, My_View)
                My_Top_Level_Package_Controllers_List.Add(pkg_ctrl)

            End If

        End If

    End Sub


    '=============================================================================================='
    ' Public methods
    '=============================================================================================='
    Public Sub Remove_Top_Level_Package(ctrl_to_remove As Top_Level_Package_Controller)
        ' Remove views
        ctrl_to_remove.Get_View.Delete_All_View()

        ' Remove package
        Dim pkg_to_remove As Top_Level_Package
        pkg_to_remove = CType(ctrl_to_remove.Get_Element, Top_Level_Package)
        My_Project.Tol_Level_Packages.Remove(pkg_to_remove)

        ' Update project
        My_Top_Level_Package_Controllers_List.Remove(ctrl_to_remove)
        Is_Project_Modified = True
        My_Project.Remove_Package_Ref(pkg_to_remove.Xml_File_Path)
        My_View.Display_Is_Modified(My_Project.Name)

    End Sub

    Public Function Add_New_Diagram_Page(diagram_name As String) As TabPage
        Return My_View.Add_New_Diagram_Page(diagram_name)
    End Function


    '=============================================================================================='
    ' Private methods
    '=============================================================================================='
    Private Sub Load_And_Display_Project(project_file_path As String)

        '------------------------------------------------------------------------------------------'
        ' Test prj_xml_file_path
        If Not File.Exists(project_file_path) Then
            My_View.Display_Project_File_Not_Found()
            Exit Sub
        End If

        '------------------------------------------------------------------------------------------'
        ' Load project data
        Dim prj_file_stream As New FileStream(project_file_path, FileMode.Open)
        Try
            My_Project = Software_Project.Deserialize_Project(prj_file_stream)
            Me.Is_Project_Loaded = True
            Me.Loaded_Project_Xml_File = project_file_path
            prj_file_stream.Close()
        Catch
            My_View.Display_Project_File_Is_Invalid()
            prj_file_stream.Close()
            Exit Sub
        End Try

        '------------------------------------------------------------------------------------------'
        ' Load top level packages
        Dim not_found_pkg_file_list As New List(Of String)
        Dim invalid_pkg_file_list As New List(Of String)
        My_Project.Load_Packages(not_found_pkg_file_list, invalid_pkg_file_list)
        If not_found_pkg_file_list.Count > 0 Then
            My_View.Display_Package_File_Not_Found(not_found_pkg_file_list)
        End If

        If invalid_pkg_file_list.Count > 0 Then
            My_View.Display_Invalid_Package_File(invalid_pkg_file_list)
        End If

        '------------------------------------------------------------------------------------------'
        ' Create_Sub_Contollers_And_Views
        My_Top_Level_Package_Controllers_List = New List(Of Top_Level_Package_Controller)
        My_View.Initialize_Project_View(My_Project.Name)
        Dim pkg As Package
        For Each pkg In My_Project.Tol_Level_Packages
            Dim pkg_ctrl As New Top_Level_Package_Controller(Me, pkg, My_View)
            My_Top_Level_Package_Controllers_List.Add(pkg_ctrl)
        Next

    End Sub

    Private Sub Save_Project()
        If Me.Is_Project_Modified = True Then
            Dim xml_file_stream As New FileStream(Me.Loaded_Project_Xml_File, FileMode.Create)
            My_Project.Serialize_Project(xml_file_stream)
            xml_file_stream.Close()
            Me.Is_Project_Modified = False
        End If
    End Sub

    Private Sub Save_All_Top_Level_Packages()
        Dim pkg_ctrl As Top_Level_Package_Controller
        For Each pkg_ctrl In My_Top_Level_Package_Controllers_List
            pkg_ctrl.Save()
        Next
    End Sub

    Private Sub Save_All_And_Close()
        Save_Project()
        Save_All_Top_Level_Packages()
        My_Project = Nothing
        My_View.Clear_Main_Window()
        Is_Project_Loaded = False
        Loaded_Project_Xml_File = ""
        Is_Project_Modified = False
        My_Top_Level_Package_Controllers_List = Nothing
    End Sub



End Class
