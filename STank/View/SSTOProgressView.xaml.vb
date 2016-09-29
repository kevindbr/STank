Imports System.ComponentModel
Imports System.Data
Imports System.Windows.Automation
Imports System.Windows.Automation.Provider
Imports System.Windows.Automation.Peers
Imports System.Windows.Threading
Imports System.Data.OleDb
Imports System.Text.RegularExpressions

Public Class SSTOProgressView

    Private mMainViewModel As MainViewModel
    Private bw As BackgroundWorker = New BackgroundWorker
    Private Property logFName = "SSTO"

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
        Dim fieldPanel As String = panel.Port.Login()

        ' We have a list of Zones and their corresponding zone data list
        ' We have a list of Zones and their corresponding schedule id, we match on those and configure ssto with those data
        For Each zoneDataList In panel.ZoneDefinitionReport.ZoneData
            For Each scheduleZoneIdPair In panel.SchedulerReport.ListOfScheduleIdsZoneNames
                If (zoneDataList.Key = scheduleZoneIdPair.Key) Then
                    panel.Port.ConfigureSSTO(panel.NameChangeDocument.ReplacementValues, zoneDataList.Value, scheduleZoneIdPair.Value)
                End If
            Next
        Next
        panel.Port.Logout()

    End Sub

    Private Sub showDone(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs)
        Dim message = "Start Stop Times have been converted.  Please refer to ssto log file for panel output."

        If e.Cancelled = True Then
            message = "Operation cancelled."

        ElseIf e.Error IsNot Nothing Then
            message = "Error. " + e.Error.Message

        Else

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



End Class
