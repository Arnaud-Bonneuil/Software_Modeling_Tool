'=================================================================================================='
Public Class Enumerated_Data_Type_Controller

    Inherits Software_Element_Controller

    Private My_Enum As Enumerated_Data_Type
    Private My_View As Enumerated_Data_Type_View


    Public Overrides Function Get_Element() As Software_Element
        Return My_Enum
    End Function

    Public Overrides Function Get_View() As Software_Element_View
        Return My_View
    End Function

    Public Sub New(a_type As Enumerated_Data_Type,
            parent_ctrl As Software_Element_Controller, parent_view As View)
        My_Enum = a_type
        Set_Parenthood(parent_ctrl)
        My_View = New Enumerated_Data_Type_View(Me, My_Enum.Name, parent_view)
        Create_Children_Controller()
    End Sub

    Public Sub Create_Children_Controller()
        If Not IsNothing(My_Enum.Enumerals) Then
            Dim enumeral As Enumerated_Data_Type_Enumeral
            For Each enumeral In My_Enum.Enumerals
                Dim enumeral_ctrl As Enumerated_Data_Type_Enumeral_Controller
                enumeral_ctrl = New Enumerated_Data_Type_Enumeral_Controller(enumeral, Me, My_View)
            Next
        End If
    End Sub

    Public Sub Add_Enumeral_Context_Menu_Clicked()
        ' Create new Enumeral
        Dim new_enumeral As New Enumerated_Data_Type_Enumeral
        new_enumeral.Name = "New_Enumerated_Data_Type_Enumeral"
        new_enumeral.Create_UUID()
        My_Enum.Add_Enumeral(new_enumeral)

        ' Create its controller
        Dim enumeral_ctrl As Enumerated_Data_Type_Enumeral_Controller
        enumeral_ctrl = New Enumerated_Data_Type_Enumeral_Controller(new_enumeral, Me, My_View)
        enumeral_ctrl.Set_Is_Under_Creation()
        Dim edit_form As Software_Element_Edition_Form = enumeral_ctrl.Create_Edition_Form()
        edit_form.ShowDialog()

    End Sub

    Public Overrides Sub Draw_Figure(page As TabPage, diagram_elmt As Model_Diagram_Element)

        ' Build list of enumeral name/value/ctrl
        Dim enumeral_name_list As New List(Of String)
        Dim enumeral_value_list As New List(Of UInteger)
        For Each ctrl In Me.Children_Controller
            Dim element As Enumerated_Data_Type_Enumeral
            element = CType(ctrl.Get_Element, Enumerated_Data_Type_Enumeral)
            enumeral_name_list.Add(element.Name)
            enumeral_value_list.Add(element.Value)
        Next

        Dim new_fig As Software_Element_Square_Figure
        new_fig = My_View.Draw_On_Diagram_Page(
            page,
            CType(diagram_elmt, Square_Model_Diagram_Element),
            My_Enum.Name, My_Enum.Description)
    End Sub

End Class


'=================================================================================================='
Public Class Enumerated_Data_Type_Enumeral_Controller

    Inherits Software_Element_Controller

    Private My_Enumeral As Enumerated_Data_Type_Enumeral
    Private My_View As Enumerated_Data_Type_Enumeral_View

    Public Overrides Function Get_Element() As Software_Element
        Return My_Enumeral
    End Function

    Public Overrides Function Get_View() As Software_Element_View
        Return My_View
    End Function

    Public Sub New(a_enumeral As Enumerated_Data_Type_Enumeral,
            parent_ctrl As Software_Element_Controller, parent_view As View)
        My_Enumeral = a_enumeral
        Set_Parenthood(parent_ctrl)
        My_View = New Enumerated_Data_Type_Enumeral_View(Me, My_Enumeral.Name, parent_view)
    End Sub

    Public Overrides Function Create_Edition_Form() As Software_Element_Edition_Form
        Dim edit_form As New Enumerated_Data_Type_Enumeral_Edition_Form(
            Me,
            My_Enumeral.Name,
            My_Enumeral.UUID,
            My_Enumeral.Description,
            My_Enumeral.Value)
        Return edit_form
    End Function

    Public Overrides Function Treat_Edition_Form_Data(edition_form As Edition_Form) As Boolean
        Dim enumeral_is_ok As Boolean = Me.Analyze_Edition_Form_Common_Data(edition_form)

        Dim my_edit_form As Enumerated_Data_Type_Enumeral_Edition_Form
        my_edit_form = CType(edition_form, Enumerated_Data_Type_Enumeral_Edition_Form)
        Dim value As UInteger = CUInt(my_edit_form.Value_TextBox.Text)
        If Not My_Enumeral.Is_Value_Valid(value) Then
            enumeral_is_ok = False
            My_View.Display_Value_Is_Invalid()
        End If

        If enumeral_is_ok = True Then
            Me.Apply_Edition_Form_Common_Data(edition_form)
            My_Enumeral.Value = value
        End If

        Return enumeral_is_ok
    End Function

End Class

