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

        Dim panel = mMainViewModel.getProj.Panel


        Dim fieldPanel As String = panel.Port.Login()


        panel.Port.ConfigureSSTO(panel.NameChangeDocument.ReplacementValues, panel.ZoneDefinitionReport.ZoneData, panel.SchedulerReport.ScheduleId)

        panel.Port.Logout()


        BaseMainViewModel.ResetUI()


    End Sub




    Private Sub exitView(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub



End Class
