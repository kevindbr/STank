Imports System.ComponentModel
Imports System.Data
Imports System.Windows.Automation
Imports System.Windows.Automation.Provider
Imports System.Windows.Automation.Peers
Imports System.Windows.Forms

Public Class DefineView

    Private mMainViewModel As MainViewModel

    ''' <summary>
    ''' Bring in mainViewModel to update and change project data
    ''' </summary>
    ''' <param name="mainViewModel"></param>
    ''' <remarks></remarks>
    Sub New(ByRef mainViewModel As MainViewModel)
        mMainViewModel = mainViewModel
        InitializeComponent()

        'AddHandler mMainViewModel.getProj.Panel.Ppcl.PropertyChanged, AddressOf updateDefineGrid
        'but this handler isn't registered until after path has been set anyway
        'mMainViewModel.getProj.Panel.NameChangeDocument.Path = mMainViewModel.getProj.Panel.NameChangeDocument.Path
        'Come back to this later.  Updating on window show is OK as long as they are prompted to do this before running FnR.

    End Sub

    Private Sub IntializeWindow()
        updateDefineGrid()
    End Sub


    Private Sub CellValueChanged(ByVal sender As Object, _
        ByVal e As DataGridCellEditEndingEventArgs) _
        Handles defineGrid.CellEditEnding

        Dim editedCell As System.Windows.Controls.TextBox = CType(e.EditingElement, System.Windows.Controls.TextBox)
        Dim newVal = editedCell.Text
        Dim variable = e.Row.Item(0)

        mMainViewModel.getProj.Panel.Ppcl.NewVariables(variable) = newVal



        'Dim r = e.Row.Item(2)

        'Dim variables As New Dictionary(Of String, String)


        'For Each row As DataRowView In defineGrid.Items   'will this reflect the new values?
        '    Dim variable = row.Item(0)
        '    Dim val = If(row = r, val2, row.Item(2))

        '    variables.Add(variable, val)
        'Next


        'mMainViewModel.getProj.Panel.Ppcl.NewVariables = variables


        ' Update the balance column whenever the values of any cell changes.
        'UpdateBalance()
    End Sub


    Private Sub updateDefineGrid()
        Dim dt As New DataTable

        dt.Columns.Add("Variable")
        dt.Columns.Add("Current Def")
        dt.Columns.Add("New Def")

        'Dim variables As New Dictionary(Of String, String)
        For Each kvp As KeyValuePair(Of String, String) In mMainViewModel.getProj.Panel.Ppcl.Variables  'which got set when Path was changed
            dt.Rows.Add(kvp.Key, kvp.Value, kvp.Value)
            'variables.Add(kvp.Key, kvp.Value)
        Next

        mMainViewModel.getProj.Panel.Ppcl.NewVariables = New Dictionary(Of String, String)(mMainViewModel.getProj.Panel.Ppcl.Variables)
        'or could do this in setter for Variables

        'For Each row As DataRowView In defineGrid.Items   'will this reflect the new values?
        '    Dim variable = row.Item(0)
        '    Dim val = If(row = r, val2, row.Item(2))

        '    variables.Add(variable, val)
        'Next


        Dispatcher.Invoke(Sub()
                              defineGrid.ItemsSource = dt.AsDataView
                              defineGrid.Columns(0).IsReadOnly = True
                              defineGrid.Columns(1).IsReadOnly = True
                              defineGrid.Columns(2).IsReadOnly = False
                          End Sub)
    End Sub

    Private Sub exitView(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub

End Class
