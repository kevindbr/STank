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
        Dim scheduleId As String = ""


        Dim fieldPanel As String = mMainViewModel.getProj.Panel.Port.Login()


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




    'TODO: put this in public place
    Private Sub AddToLog(ByVal line As String)

        If line = "" Then Return 'when stderr is non-empty, stdout will be

        'Need this because this code doesn't run on UI thread but needs to modify UI elements
        Dispatcher.Invoke(Sub()

                              log.Items.Add(line)

                              If log.Items.Count > 10 Then
                                  log.Items.RemoveAt(0)
                              End If

                              'log.SelectedIndex = log.Items.Count - 1
                              'log.SelectedIndex = -1

                              'Scroll to last entry
                              Dim svAutomation As ListBoxAutomationPeer = ScrollViewerAutomationPeer.CreatePeerForElement(log)
                              Dim scrollInterface As IScrollProvider = svAutomation.GetPattern(PatternInterface.Scroll)
                              Dim scrollVertical As ScrollAmount = ScrollAmount.LargeIncrement
                              Dim scrollHorizontal As ScrollAmount = ScrollAmount.NoAmount
                              If scrollInterface.VerticallyScrollable Then
                                  scrollInterface.Scroll(scrollHorizontal, scrollVertical)
                              End If

                              'og.ScrollIntoView(log.Items.Item(log.Items.Count - 1))   'doesn't work for duplicate entries

                          End Sub)


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
