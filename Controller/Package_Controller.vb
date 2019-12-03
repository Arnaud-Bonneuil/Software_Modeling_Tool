Imports System.IO

'=================================================================================================='
Public Class Package_Controller

    Inherits Software_Element_Controller

    Protected My_Package As Package
    Protected My_Package_View As View

    Public Sub New(a_package As Package,
            parent_ctrl As Software_Element_Controller, parent_view As View)
        My_Package = a_package
        Set_Parenthood(parent_ctrl)
        My_Package_View = New Package_View(Me, My_Package.Name, parent_view, a_package.Is_Read_Only)
        Create_Children_Controller()
    End Sub

    Public Sub New()
    End Sub

    Public Overrides Function Get_Element() As Software_Element
        Return My_Package
    End Function

    Public Overrides Function Get_View() As Software_Element_View
        Return CType(My_Package_View, Software_Element_View)
    End Function

    Public Sub Create_Children_Controller()
        If Not IsNothing(My_Package.Packages) Then
            Dim pkg As Package
            For Each pkg In My_Package.Packages
                Dim pkg_ctrl As New Package_Controller(pkg, Me, My_Package_View)
            Next
        End If
        If Not IsNothing(My_Package.Data_Types) Then
            Dim type As Data_Type
            For Each type In My_Package.Data_Types
                Dim type_ctrl As Software_Element_Controller = Nothing
                Select Case type.GetType
                    Case GetType(Basic_Integer_Type), _
                         GetType(Basic_Boolean_Type), _
                         GetType(Basic_Floating_Point_Type)
                        type_ctrl = New Basic_Data_Type_Controller(type, Me, My_Package_View)
                    Case GetType(Enumerated_Data_Type)
                        type_ctrl = New Enumerated_Data_Type_Controller( _
                                             CType(type, Enumerated_Data_Type), Me, My_Package_View)
                    Case GetType(Array_Data_Type)
                        type_ctrl = New Array_Data_Type_Controller( _
                                                  CType(type, Array_Data_Type), Me, My_Package_View)
                    Case GetType(Physical_Data_Type)
                        type_ctrl = New Physical_Data_Type_Controller( _
                                               CType(type, Physical_Data_Type), Me, My_Package_View)
                    Case GetType(Structured_Data_Type)
                        type_ctrl = New Structured_Data_Type_Controller( _
                                             CType(type, Structured_Data_Type), Me, My_Package_View)
                End Select
            Next
        End If
    End Sub

    Public Sub Add_Package_Context_Menu_Clicked()
        ' Create a new Package
        Dim new_package As New Package
        new_package.Name = "New_Package"
        new_package.Create_UUID()
        My_Package.Add_Package(new_package)

        ' Create its controller
        Dim new_ctrl As New Package_Controller(new_package, Me, My_Package_View)
        new_ctrl.Set_Is_Under_Creation()
        Dim edit_form As Software_Element_Edition_Form = new_ctrl.Create_Edition_Form()
        edit_form.ShowDialog()

    End Sub

    Public Sub Add_Enum_Context_Menu_Clicked()
        ' Create a new Enum
        Dim new_enum As New Enumerated_Data_Type
        new_enum.Name = "New_Enum"
        new_enum.Create_UUID()
        My_Package.Add_Data_Type(new_enum)

        ' Create its controller
        Dim new_ctrl As New Enumerated_Data_Type_Controller(new_enum, Me, My_Package_View)
        new_ctrl.Set_Is_Under_Creation()
        Dim edit_form As Software_Element_Edition_Form = new_ctrl.Create_Edition_Form()
        edit_form.ShowDialog()

    End Sub

    Sub Add_Array_Context_Menu_Clicked()
        ' Create a new Array
        Dim new_array As New Array_Data_Type
        new_array.Name = "New_Array"
        new_array.Create_UUID()
        My_Package.Add_Data_Type(new_array)

        ' Create its controller
        Dim new_ctrl As New Array_Data_Type_Controller(new_array, Me, My_Package_View)
        new_ctrl.Set_Is_Under_Creation()
        Dim edit_form As Software_Element_Edition_Form = new_ctrl.Create_Edition_Form()
        edit_form.ShowDialog()
    End Sub

    Sub Add_Physical_Context_Menu_Clicked()
        ' Create a new Physical_Data_Type
        Dim new_phys As New Physical_Data_Type
        new_phys.Name = "New_Phys"
        new_phys.Create_UUID()
        My_Package.Add_Data_Type(new_phys)

        ' Create its controller
        Dim new_ctrl As New Physical_Data_Type_Controller(new_phys, Me, My_Package_View)
        new_ctrl.Set_Is_Under_Creation()
        Dim edit_form As Software_Element_Edition_Form = new_ctrl.Create_Edition_Form()
        edit_form.ShowDialog()
    End Sub

    Sub Add_Struct_Context_Menu_Clicked()
        ' Create a new Struct
        Dim new_struct As New Structured_Data_Type
        new_struct.Name = "New_Struct"
        new_struct.Create_UUID()
        My_Package.Add_Data_Type(new_struct)

        ' Create its controller
        Dim new_ctrl As New Structured_Data_Type_Controller(new_struct, Me, My_Package_View)
        new_ctrl.Set_Is_Under_Creation()
        Dim edit_form As Software_Element_Edition_Form = new_ctrl.Create_Edition_Form()
        edit_form.ShowDialog()
    End Sub

End Class
