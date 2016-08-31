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
    Private Property logFName = "Schedules"

    ''' <summary>
    ''' Bring in mainViewModel to update and change project data
    ''' </summary>
    ''' <param name="mainViewModel"></param>
    ''' <remarks></remarks>
    Sub New(ByRef mainViewModel As MainViewModel)
        mMainViewModel = mainViewModel
        InitializeComponent()
        doneButton.IsEnabled = False
    End Sub

    Private Sub IntializeWindow()
        ' updateDefineGrid()
        BaseMainViewModel.InitUI(Dispatcher.CurrentDispatcher, log, progressBar, logFName, mMainViewModel.getProj.LogPath)
        bw.WorkerReportsProgress = True
        bw.WorkerSupportsCancellation = True
        AddHandler bw.DoWork, AddressOf bw_RunFindAndReplace
        AddHandler bw.RunWorkerCompleted, AddressOf showDone
        bw.RunWorkerAsync()
    End Sub


    Private Sub bw_RunFindAndReplace(ByVal sender As Object, ByVal e As DoWorkEventArgs)

        'TODO: might make sense to move this logic into SchedulerReport...


        Dim panel = mMainViewModel.getProj.Panel
        Dim port = panel.Port

        Dim fieldPanel As String = port.Login()

        Dim schedulerReport = panel.SchedulerReport
        Dim replacementValues = panel.NameChangeDocument.ReplacementValues
        'Dim commandName As String = replacementValues(schedulerReport.ZoneName)     'TODO: currently zone name is not in name change document
        Dim commandName As String = schedulerReport.ZoneName.Replace(".", "_")


        Dim scheduleId As String = port.CreateSchedule(commandName + "_SchedCmd")
        panel.SchedulerReport.ScheduleId = scheduleId   'not sure if this is still needed.  Maybe for later stepss

        'BaseMainViewModel.WriteLog(String.Format("Creating BACnet schedule object '{0}'", commandName + "_SchedCmd"))
        BaseMainViewModel.UpdateProgress(0.1)


        For Each kvp As KeyValuePair(Of String, Tuple(Of String, String, String)) In panel.SchedulerReport.Schedules
            Dim weekday As String = kvp.Key
            Dim times As Tuple(Of String, String, String) = kvp.Value
            port.PopulateSchedule(scheduleId, weekday, times.Item2, times.Item3)
        Next

        BaseMainViewModel.UpdateProgress(0.2)


        Dim i As Integer = 1
        For Each kvp As KeyValuePair(Of String, String) In panel.PanelAttributesDocument.LenumPoints

            Dim pointName As String = kvp.Key
            Dim stateTextTableId As String = kvp.Value

            Dim stateTextTable As StateTextDoc.StateTextTable = panel.StateTextDocument.GetStateTextByID(stateTextTableId)
            Dim pointId As String = port.GetPointInstanceNumber(pointName)

            Dim commandId As String = port.CreateBacnetCommand(commandName + If(i > 1, CStr(i), ""))     'need new command for each point
            BaseMainViewModel.UpdateProgress(0.4)

            Dim commandEncodedName As String = port.CreateCommandActions(commandName, commandId, fieldPanel, stateTextTable, pointId)

            BaseMainViewModel.UpdateProgress(0.6)
            port.LinkScheduleToCommand(scheduleId, commandEncodedName)
        Next

        BaseMainViewModel.UpdateProgress(1.0)

        port.Logout()



    End Sub

    Private Sub showDone()
        Dim message As GeneralPopupView = New GeneralPopupView("Schedules have been converted.  Please refer to ssto log file for panel output.")
        doneButton.Content = "Done"
        doneButton.IsEnabled = True
        message.Show()
        BaseMainViewModel.WriteFile()
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
