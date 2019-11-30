Imports System.IO

'=================================================================================================='
Public Class Top_Level_Package_Controller

    Inherits Package_Controller

    Public My_Project_Controller As Software_Project_Controller

    Private Is_Modified As Boolean = False

    Public Sub New(
            a_prj_ctrl As Software_Project_Controller,
            a_package As Package,
            parent_view As View)
        Me.Parent_Controller = Nothing ' by definition
        My_Project_Controller = a_prj_ctrl

        My_Package = a_package
        My_Package_View = New Top_Level_Package_View(Me, a_package.Name,
                                                                parent_view, a_package.Is_Read_Only)
        Create_Children_Controller()
    End Sub

    Public Sub Save()
        If Me.Is_Modified = True Then
            Dim top_pkg As Top_Level_Package = CType(My_Package, Top_Level_Package)
            Dim xml_file_stream As New FileStream(top_pkg.Xml_File_Path, FileMode.Create)
            top_pkg.Serialize_Package(xml_file_stream)
            xml_file_stream.Close()
            Set_Is_Saved()
        End If
    End Sub

    Public Sub Set_Is_Modified()
        If Me.Is_Modified = False Then
            ' Change package status
            Is_Modified = True

            ' Change views
            CType(My_Package_View, Top_Level_Package_View).Display_Is_Modified(My_Package.Name)
        End If
    End Sub

    Private Sub Set_Is_Saved()
        If Me.Is_Modified = True Then
            ' Change package status
            Is_Modified = False

            ' Change views
            CType(My_Package_View, Top_Level_Package_View).Display_Is_Saved(My_Package.Name)
        End If
    End Sub

    Public Sub Save_Context_Menu_Clicked()
        Me.Save()
    End Sub

    Sub Remove_Context_Menu_Clicked()
        CType(My_Package_View, Top_Level_Package_View).Delete_All_View()
        My_Project_Controller.Remove_Top_Level_Package(Me)
    End Sub

End Class
