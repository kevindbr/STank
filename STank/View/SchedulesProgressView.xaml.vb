Imports System.ComponentModel
Imports System.Data
Imports System.Windows.Automation
Imports System.Windows.Automation.Provider
Imports System.Windows.Automation.Peers
Imports System.Windows.Threading
Imports System.Data.OleDb
Imports System.Text.RegularExpressions

Public Class SchedulesProgressView

    Private mMainViewModel As MainViewModel
    Private bw As BackgroundWorker = New BackgroundWorker

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
        ' updateDefineGrid()
        BaseMainViewModel.InitUI(Windows.Threading.Dispatcher.CurrentDispatcher, log, progressBar)
        bw.WorkerReportsProgress = True
        bw.WorkerSupportsCancellation = True
        AddHandler bw.DoWork, AddressOf bw_RunFindAndReplace
        bw.RunWorkerAsync()
    End Sub


    Private Sub bw_RunFindAndReplace(ByVal sender As Object, ByVal e As DoWorkEventArgs)

        'TODO: might make sense to move this logic into SchedulerReport...


        Dim panel = mMainViewModel.getProj.Panel
        Dim port = panel.Port

        Dim fieldPanel As String = port.Login()

        Dim schedulerReport = mMainViewModel.getProj.Panel.SchedulerReport
        Dim replacementValues = mMainViewModel.getProj.Panel.NameChangeDocument.ReplacementValues
        'Dim commandName As String = replacementValues(schedulerReport.ZoneName)     'TODO: currently zone name is not in name change document
        Dim commandName As String = schedulerReport.ZoneName.Replace(".", "_")


        Dim scheduleId As String = port.CreateSchedule(commandName + "_SchedCmd")

        For Each kvp As KeyValuePair(Of String, Tuple(Of String, String)) In panel.SchedulerReport.Schedules
            Dim weekday As String = kvp.Key
            Dim times As Tuple(Of String, String) = kvp.Value
            port.PopulateSchedule(scheduleId, weekday, times.Item1, times.Item2)
        Next


        Dim i As Integer = 1
        For Each kvp As KeyValuePair(Of String, String) In panel.PanelAttributesDocument.LenumPoints

            Dim pointName As String = kvp.Key
            Dim stateTextTableId As String = kvp.Value

            Dim stateTextTable As StateTextDoc.StateTextTable = panel.StateTextDocument.GetStateTextByID(stateTextTableId)
            Dim pointId As String = port.GetPointInstanceNumber(pointName)

            Dim commandId As String = port.CreateBacnetCommand(commandName + If(i > 1, CStr(i), ""))     'need new command for each point
            Dim commandEncodedName As String = port.CreateCommandActions(commandName, commandId, fieldPanel, stateTextTable, pointId)

            'can we not get this until actions are created?

            'TODO: what if the LENUMs use the same state text table?  Don't want to create twice....Check if exists before creating.
            'OR...won't it complain if we try to use same instance number?

            'should be fine...since the point instance number will be different in actionlist
            'BUT...wouldn't the commandName be the same?
            'Check if schedule already exists...?

            port.LinkScheduleToCommand(scheduleId, commandEncodedName)



            'same schedule, but add a target for each command...

            'can one schedule command multiple points?  Yes...Just need one schedule per state text/action list.



        Next


        port.Logout()

        BaseMainViewModel.ResetUI()

    End Sub



    Private Sub exitView(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub

    'Private Sub updateDefineGrid()
    '    Dim dt As New DataTable

    '    dt.Columns.Add("Variable")
    '    dt.Columns.Add("Current Def")
    '    dt.Columns.Add("New Def")

    '    For Each kvp As KeyValuePair(Of String, String) In mMainViewModel.getProj.Panel.Ppcl.Variables
    '        dt.Rows.Add(kvp.Key, kvp.Value, kvp.Value)
    '    Next


    '    'defineGrid.DataContext = data.DefaultView

    '    Dispatcher.Invoke(Sub()
    '                          defineGrid.ItemsSource = dt.AsDataView

    '                          defineGrid.Columns(0).IsReadOnly = True
    '                          defineGrid.Columns(1).IsReadOnly = True
    '                          defineGrid.Columns(2).IsReadOnly = False
    '                      End Sub)
    'End Sub

End Class
