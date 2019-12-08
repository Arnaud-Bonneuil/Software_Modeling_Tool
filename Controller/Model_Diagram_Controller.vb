Public Class Model_Diagram_Controller

    Inherits Software_Element_Controller

    Private My_Diagram As Model_Diagram
    Private My_View As Model_Diagram_View

    Public Overrides Function Get_Element() As Software_Element
        Return My_Diagram
    End Function

    Public Overrides Function Get_View() As Software_Element_View
        Return My_View
    End Function

    Public Sub New(a_diagram As Model_Diagram,
            parent_ctrl As Software_Element_Controller, parent_view As View)
        My_Diagram = a_diagram
        Set_Parenthood(parent_ctrl)
        My_View = New Model_Diagram_View(Me, My_Diagram.Name, parent_view)
    End Sub

    Public Sub Draw_Context_Menu_Clicked()
        ' Get the diagram area
        Dim prj_ctrl As Software_Project_Controller
        prj_ctrl = Me.Get_Top_Level_Package_Controller.My_Project_Controller

        ' Create a new page on the diagram area
        Dim new_page As TabPage
        new_page = prj_ctrl.Add_New_Diagram_Page(My_Diagram.Name)

        ' Associate the page with my view
        My_View.Set_Page(new_page)

        ' Draw all the elements already contained by the model diagram
    End Sub

End Class
