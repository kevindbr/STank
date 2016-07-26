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
        bw.WorkerReportsProgress = True
        bw.WorkerSupportsCancellation = True
        AddHandler bw.DoWork, AddressOf bw_RunFindAndReplace
        bw.RunWorkerAsync()
    End Sub

    Private Sub bw_RunFindAndReplace(ByVal sender As Object, ByVal e As DoWorkEventArgs)

        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)

        ConfigureSSTO()     'get scheduleID from previous step...


    End Sub


    'TODO: still not
    Private Sub UpdateProgress(ByVal fraction As Integer)

        Dispatcher.Invoke(Sub(ByRef i As Integer)
                              progressBar.Value = i * 100
                          End Sub, fraction)


    End Sub


    Private Function SendCommand(ByVal command As String, Optional ByVal sendLineBreaks As Boolean = False) As String

        AddToLog(command)
        Dim cmdOutput As String = mMainViewModel.getProj.Panel.Port.SendCommand(command, sendLineBreaks)
        AddToLog(cmdOutput)
        'TODO: possibly insert sleep in here
        Return cmdOutput

    End Function


    Private Sub ConfigureSSTO()

        Dim resp As String

        Dim newNames As Dictionary(Of String, String) = mMainViewModel.getProj.Panel.NameChangeDocument.ReplacementValues     'TODO: make sure name change is done first
        Dim zoneData As Dictionary(Of String, String) = mMainViewModel.getProj.Panel.ZoneDefinitionReport.ZoneData
        Dim scheduleId As String = mMainViewModel.getProj.Panel.SchedulerReport.ScheduleId


        Dim fieldPanel As String = mMainViewModel.getProj.Panel.Port.Login(False)       'already have connection


        'General SSTO settings
        SendCommand("#")        'Top of menu
        SendCommand("a")        'Application
        SendCommand("b")        'BacNet
        SendCommand("t")        'ssTo
        SendCommand("g")        'General
        SendCommand("a")        'Add
        SendCommand(scheduleId, True)   'Schedule id
        SendCommand(If(zoneData("Start Optimization") = "Enabled", "y", "n"))   'Enable strt optimization (Y/N)
        SendCommand(If(zoneData("Stop Optimization") = "Enabled", "y", "n"))   'Enable stop optimization (Y/N)
        SendCommand(If(zoneData("Night Operation") = "Enabled", "y", "n"))   'Enable night operation (Y/N)
        SendCommand(GetOperationCommand(zoneData("Allowed Operation")))   'Allowed operation (N,H,C,hAc)
        SendCommand(GetOperationCommand(zoneData("Selected Operation")))   'Desired oper (N,H,C,hAc,R)  'Not sure about this R option....'
        SendCommand(newNames(zoneData("Outside Temperature")), True)   'Outside temperature
        SendCommand(newNames(zoneData("Zone Temperature")), True)   'Zone temperature
        SendCommand(zoneData("Heating Setpoint (OCC)"), True)   'Heating setpoint-occupancy
        SendCommand(zoneData("Heating Setpoint (VAC)"), True)   'Heating setpoint-vacancy
        SendCommand(zoneData("Cooling Setpoint (OCC)"), True)   'Cooling setpoint-occupancy
        resp = SendCommand(zoneData("Cooling Setpoint (VAC)"), True)   'Cooling setpoint-vacancy
        'check that response contains "Command successful"

        'Start time optimization
        SendCommand("#")        'Top of menu
        SendCommand("a")        'Application
        SendCommand("b")        'BacNet
        SendCommand("t")        'ssTo
        SendCommand("s")        'Start
        SendCommand("a")        'Add
        SendCommand(scheduleId, True)   'Schedule id
        For Each mode As SstoMode In System.Enum.GetValues(GetType(SstoMode))
            SendCommand(GetSstoData(mode, zoneData("Mode")), True)   'Start mode-htg
            SendCommand(If(GetSstoData(mode, zoneData("If too early")) = "OCC1", "y", "n"))   'Early-next occ mode-htg (Y/N)    'not sure about this comparison
            SendCommand(If(GetSstoData(mode, zoneData("If too late")) = "Use Next Mode", "y", "n"))   'Late-occ mode-htg (Y/N)    'not sure about this comparison
            SendCommand(If(GetSstoData(mode, zoneData("Time Shift")) = "Inside and Outside", "a", "b"))   'Time shift-htg (Fxd,Bsc,Adv)    '3 options, how to map?
            SendCommand(If(GetSstoData(mode, zoneData("Use Learning")) = "Yes", "y", "n"))   'Use learning-htg (Y/N)
            SendCommand(GetSstoData(mode, zoneData("Outside Temp Limit")), True)   'Outside temp limit-htg
            SendCommand(GetSstoData(mode, zoneData("Zone Temp Deviation")), True)   'Zone temp deviation-htg 
            SendCommand(GetSstoData(mode, zoneData("Min Start Duration")), True)   'Min start duration-htg (min)
            SendCommand(GetSstoData(mode, zoneData("Max Start Duration")), True)   'Max start duration-htg (min)
            SendCommand(GetSstoData(mode, zoneData("Max Extension Time")), True)   'Max extension time-htg (min)

            'missed a couple other options here...offset?

        Next

        'TODO: probably don't need to implement stop time optimization, they said...it wasn't in the example report anyway
        'but may need to do night

        mMainViewModel.getProj.Panel.Port.Logout()


    End Sub



    Private Function GetOperationCommand(ByVal zoneDefOperation As String)
        Select Case zoneDefOperation
            Case "Heating and Cooling"
                Return "a"
            Case "Heating"
                Return "h"
            Case "Cooling"
                Return "c"
            Case Else
                Return "n"
        End Select
    End Function


    Private Enum SstoMode
        Heating
        Cooling
    End Enum

    Private Function GetSstoData(ByVal mode As SstoMode, ByVal zoneDataValue As String)
        Return Regex.Split(zoneDataValue, "\s{2,}")(mode)
    End Function




    Private Sub AddToLog(ByVal line As String)
        'Need this because this code doesn't run on UI thread but needs to modify UI elements
        Dispatcher.Invoke(Sub()
                              mMainViewModel.Log(log, line)
                          End Sub)
    End Sub

    Private Sub exitView(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub



End Class
