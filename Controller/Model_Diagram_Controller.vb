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
        Dim my_diagram_area As Diagram_Area
        Dim prj_ctrl As Software_Project_Controller
        prj_ctrl = Me.Get_Top_Level_Package_Controller.My_Project_Controller
        my_diagram_area = prj_ctrl.Get_Diagram_Area

        ' Create a new page on the diagram area
        Dim new_page As TabPage
        new_page = my_diagram_area.Add_New_Diagram_Page(My_Diagram.Name)

        ' Associate the page with my view
        My_View.Set_Page(new_page)

        ' Draw all the elements already contained by the model diagram
        Dim diagram_elmt As Model_Diagram_Element
        For Each diagram_elmt In My_Diagram.Model_Diagram_Elements
            Dim drawn_sw_ctrl As Software_Element_Controller
            drawn_sw_ctrl = Me.Get_Controller_By_Element_UUID(diagram_elmt.Software_Element_Ref)
            drawn_sw_ctrl.Draw_Figure(new_page, diagram_elmt)
        Next
    End Sub

End Class
