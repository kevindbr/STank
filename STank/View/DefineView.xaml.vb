Imports System.ComponentModel
Imports System.Data
Imports System.Windows.Automation
Imports System.Windows.Automation.Provider
Imports System.Windows.Automation.Peers

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

    End Sub

    Private Sub IntializeWindow()
        updateDefineGrid()
    End Sub

    Private Sub updateDefineGrid()
        Dim dt As New DataTable

        dt.Columns.Add("Variable")
        dt.Columns.Add("Current Def")
        dt.Columns.Add("New Def")

        For Each kvp As KeyValuePair(Of String, String) In mMainViewModel.getProj.Panel.Ppcl.Variables
            dt.Rows.Add(kvp.Key, kvp.Value, kvp.Value)
        Next

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
