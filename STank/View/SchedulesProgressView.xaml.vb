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

        Dim panel = mMainViewModel.getProj.Panel
        Dim port = panel.Port

        Try
            'TODO: might make sense to move this logic into SchedulerReport...
            Dim fieldPanel As String = port.Login()

            Dim schedulerReport = panel.SchedulerReport
            Dim replacementValues = panel.NameChangeDocument.ReplacementValues
            'Dim commandName As String = replacementValues(schedulerReport.ZoneName)     'TODO: currently zone name is not in name change document
            Dim listOfScheduleIdsZoneNames As List(Of KeyValuePair(Of String, String)) = New List(Of KeyValuePair(Of String, String))
            Dim totalCount = 1


            For Each kvp As KeyValuePair(Of String, Tuple(Of String, String, String)) In panel.SchedulerReport.Schedules
                BaseMainViewModel.UpdateProgress(totalCount / panel.SchedulerReport.Schedules.Count)
                totalCount += 1
                Dim zoneName As String = kvp.Value.Item1
                Dim commandName As String = zoneName.Replace(".", "_")
                Dim scheduleId = ""

                If (listOfScheduleIdsZoneNames.Count > 0) Then
                    For Each schPair As KeyValuePair(Of String, String) In listOfScheduleIdsZoneNames
                        If (schPair.Key = zoneName) Then
                            scheduleId = schPair.Value
                        End If
                    Next
                End If


                If (scheduleId = "") Then
                    scheduleId = port.CreateSchedule(commandName + "_SchedCmd")
                    Dim scheduleZonePair = New KeyValuePair(Of String, String)(zoneName, scheduleId)

                    If Not (listOfScheduleIdsZoneNames.Contains(scheduleZonePair)) Then
                        listOfScheduleIdsZoneNames.Add(scheduleZonePair)
                    End If
                End If

                panel.SchedulerReport.ScheduleId = scheduleId   'not sure if this is still needed.  Maybe for later stepss

                'BaseMainViewModel.WriteLog(String.Format("Creating BACnet schedule object '{0}'", commandName + "_SchedCmd"))

                Dim weekday As String = kvp.Key
                Dim times As Tuple(Of String, String, String) = kvp.Value
                port.PopulateSchedule(scheduleId, weekday, times.Item2, times.Item3)

                Dim i As Integer = 1
                For Each kvp2 As KeyValuePair(Of String, String) In panel.PanelAttributesDocument.LenumPoints

                    Dim pointName As String = kvp2.Key
                    Dim stateTextTableId As String = kvp2.Value

                    Dim stateTextTable As StateTextDoc.StateTextTable = panel.StateTextDocument.GetStateTextByID(stateTextTableId)
                    Dim pointId As String = port.GetPointInstanceNumber(pointName)

                    Dim commandId As String = port.CreateBacnetCommand(commandName + If(i > 1, CStr(i), ""))     'need new command for each point
                    Dim commandEncodedName As String = port.CreateCommandActions(commandName, commandId, fieldPanel, stateTextTable, pointId)
                    port.LinkScheduleToCommand(scheduleId, commandEncodedName)
                Next
            Next

            ' mMainViewModel.getProj.Panel.SchedulerReport.ListOfScheduleIdsZoneNames = listOfScheduleIdsZoneNames
            BaseMainViewModel.UpdateProgress(1.0)
            e.Result = listOfScheduleIdsZoneNames

            port.Logout()

        Catch ex As Exception
            port.Logout()
            Throw New Exception(ex.Message + " schedules finished partially.")

        End Try

    End Sub


    Private Sub showDone(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs)
        Dim message = "Schedules have been converted.  Please refer to Schedules log file for panel output."

        If e.Cancelled = True Then
            message = "Operation cancelled."

        ElseIf e.Error IsNot Nothing Then
            message = "Error. " + e.Error.Message

        Else
            mMainViewModel.getProj.Panel.SchedulerReport.ListOfScheduleIdsZoneNames = e.Result


        End If

        Dim messageWindow As GeneralPopupView = New GeneralPopupView(message)
        doneButton.Content = "Done"
        doneButton.IsEnabled = True
        messageWindow.Show()
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
