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
        bw.WorkerReportsProgress = True
        bw.WorkerSupportsCancellation = True
        AddHandler bw.DoWork, AddressOf bw_RunFindAndReplace
        bw.RunWorkerAsync()
    End Sub


    Private Sub bw_RunFindAndReplace(ByVal sender As Object, ByVal e As DoWorkEventArgs)

        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)

        ConfigureSchedules()     'get scheduleID from previous step...


    End Sub


    Private Sub UpdateProgress(ByVal fraction As Integer)

        Dispatcher.Invoke(Sub(ByRef i As Integer)
                              progressBar.Value = i * 100
                          End Sub, fraction)


    End Sub



    Public Sub ConfigureSchedules()

        'Dim sp As IO.Ports.SerialPort = My.Computer.Ports.OpenSerialPort(mPortName)

        Dim resp As String

        Dim fieldPanel As String = mMainViewModel.getProj.Panel.Port.Login()

        Dim commandName As String = "FPL01_BacCmd"
        Dim commandInstance As String = CreateBacnetCommand(commandName)

        Dim stateTextTableId As String = "3003" 'arbitrary, but needs to be consistent
        Dim stateTextTable As Dictionary(Of String, String) = New Dictionary(Of String, String) From {{"1", "VAC"}, {"2", "OCC"}}

        'TODO: check instance number from report


        Dim commandEncodedName As String = CreateCommandActions(commandName, commandInstance, fieldPanel, stateTextTable)

        CreateStateTextTable("BAC_AHU_LENUM", stateTextTableId, stateTextTable)
        CreatePoint("FPL01_LENUM", stateTextTableId)

        Dim scheduleId As String = CreateSchedule("FPL01_SchedCmd", commandEncodedName)


        mMainViewModel.getProj.Panel.Port.Logout()

    End Sub




    Function CreateBacnetCommand(ByVal commandName As String) As String

        Dim resp As String

        SendCommand("#")        'Top of menu
        SendCommand("a")        'Application
        SendCommand("b")        'BacNet
        SendCommand("m")        'coMmander
        SendCommand("e")        'Edit
        SendCommand("a")        'Add
        SendCommand("", True)   'Field panel
        SendCommand(commandName, True)   'Command name
        SendCommand("", True)   'Command instance
        resp = SendCommand("", True)   'Description
        'check that response contains "Command successful"

        SendCommand("#")        'Top of menu
        SendCommand("a")        'Application
        SendCommand("b")        'BacNet
        SendCommand("m")        'coMmander
        SendCommand("l")        'Log
        SendCommand("", True)   'Field panel
        SendCommand("", True)   'First Id
        resp = SendCommand("", True)   'Last Id
        SendCommand(commandName, True)   'Command name
        SendCommand("", True)   'Command instance
        resp = SendCommand("", True)   'Description
        Dim commandInstance = Regex.Matches(resp, "([0-9]+)\s+" + commandName).Item(0).Groups(1).ToString()

        SendCommand("", True)

        Return commandInstance

    End Function


    Function CreateCommandActions(ByVal commandName As String, ByVal commandId As String,
                                  ByVal deviceInstanceNumber As String, ByVal stateTextTable As Dictionary(Of String, String)) As String

        Dim resp As String

        For Each kvp As KeyValuePair(Of String, String) In stateTextTable
            Dim value As String = kvp.Key
            Dim stateText As String = kvp.Value

            SendCommand("#")        'Top of menu
            SendCommand("a")        'Application
            SendCommand("b")        'BacNet
            SendCommand("m")        'coMmander
            SendCommand("a")        'Actionlists
            SendCommand("a")        'Add
            SendCommand("", True)   'Field panel
            SendCommand(commandId, True)   'Command instance
            SendCommand(value, True)        'Action list index
            SendCommand(stateText, True)    'Action text
            SendCommand(value, True)    'Sequence #
            SendCommand(deviceInstanceNumber, True)    'Device Instance Number (get this from...?  Same as field panel number)
            SendCommand("19", True)    'Object Type (19 = multistate-value)
            SendCommand("1", True)    'Object Instance Number     'TODO: how do we know this?  Some screenshots say 2.... doesn't panel assign this ?
            SendCommand("85", True)    'Property Id (85 = present-value)
            SendCommand("", True)    'Array Index
            SendCommand("u")    'Bool, Real, Enum, Unsigned
            SendCommand(value, True)    'Value
            SendCommand("16", True)    'Priority for writing (1-16)
            SendCommand("0", True)    'Post delay
            resp = SendCommand("n", True)    'Quit on failure (Y/N)     NOTE: y/n may not require space...like other single-letter commands
            'check that response contains "Command successful"

        Next

        SendCommand("#")        'Top of menu
        SendCommand("a")        'Application
        SendCommand("b")        'BacNet
        SendCommand("m")        'coMmander
        SendCommand("a")        'Actionlists
        SendCommand("l")        'Log
        resp = SendCommand("", True)   'Field panel
        Dim commandEncodedName = Regex.Matches(resp, "(\S+)\s+" + commandName).Item(0).Groups(1).ToString()

        Return commandEncodedName


    End Function



    Sub CreateStateTextTable(ByVal tableName As String, ByVal tableId As String, ByVal stateTextTable As Dictionary(Of String, String))

        Dim resp As String

        SendCommand("#")        'Top of menu
        SendCommand("s")        'System
        SendCommand("t")        'Text
        SendCommand("e")        'Edit
        SendCommand("t")        'Table
        SendCommand("a")        'Add
        SendCommand(tableName, True)   'State text table name
        SendCommand(tableId, True)   'State text table ID (arbitrary, but will need to use later)

        For Each kvp As KeyValuePair(Of String, String) In stateTextTable
            Dim value As String = kvp.Key
            Dim stateText As String = kvp.Value
            SendCommand(value, True)   'Value
            SendCommand(stateText, True)   'State text
        Next

        resp = SendCommand("", True)    'Send blank line to indicate done adding states?
        'check that response contains "Command successful"

    End Sub



    Sub CreatePoint(ByVal pointName As String, ByVal stateTextTableId As String)

        Dim resp As String

        'better to have newLines, Command?

        SendCommand("#")        'Top of menu
        SendCommand("p")        'Point
        SendCommand("e")        'Edit
        SendCommand("a")        'Add
        SendCommand(pointName, True)   'Point system name
        SendCommand("", True)   'Instance Number     (does this refer back to command action?  Probably just linked by state text table)
        SendCommand(pointName, True)   'Point name
        SendCommand("12", True)   'Point type (12 = LENUM)
        SendCommand("", True)   'Descriptor
        SendCommand(stateTextTableId, True)   'State text table.  Need to get ID for the one just added, based on State Text Table Log
        SendCommand("", True)   'Access group(s)
        SendCommand("n", True)   'Alarmable (Y/N)       NOTE: these y/n commands may not require space to be sent
        SendCommand("", True)   'Field panel
        SendCommand("v", True)   'Physical, Virtual Point address
        SendCommand("", True)   'Point
        resp = SendCommand("VAC", True)   'Relinquish Default
        'check that response contains "Command successful"

    End Sub



    Function CreateSchedule(ByVal scheduleName As String, ByVal commandEncodedName As String) As String

        Dim resp As String

        SendCommand("#")        'Top of menu
        SendCommand("a")        'Application
        SendCommand("b")        'BacNet
        SendCommand("s")        'Schedule
        SendCommand("e")        'Edit
        SendCommand("a")        'Add
        SendCommand("", True)   'Field panel
        SendCommand(scheduleName, True)   'Schedule name
        SendCommand("", True)        'Schedule ID (should be auto-assigned, then we get it below)
        SendCommand("", True)   'Description
        SendCommand("1", True)   'Default Value
        SendCommand("u")    'Bool, Real, Enum, Unsigned
        SendCommand("16", True)    'Priority for writing (1-16)
        SendCommand("", True)   'Start date (MM/DD/YYYY) - would assume blank means forever
        SendCommand("", True)   'Weekday (m,tu,w,th,f,sa,su,*)   - not sure what this means, but leave blank for now?
        SendCommand("", True)   'Stop date (MM/DD/YYYY) - would assume blank means forever
        SendCommand("", True)   'Weekday (m,tu,w,th,f,sa,su,*)   - not sure what this means, but leave blank for now?
        SendCommand("n")   'Out of service (y/n)
        SendCommand("y")   'SSTO enabled (y/n)
        resp = SendCommand("y")   'SI unit (y/n)
        'check that response contains "Command successful"

        SendCommand("#")        'Top of menu
        SendCommand("a")        'Application
        SendCommand("b")        'BacNet
        SendCommand("s")        'Schedule
        SendCommand("l")        'Log    (or 'd' to Display - Schedule report - lets us see what was done above)
        SendCommand("", True)   'Field panel
        Dim scheduleId = Regex.Matches(resp, "([0-9]+)\s+" + scheduleName).Item(0).Groups(1).ToString()
        mMainViewModel.getProj.Panel.SchedulerReport.ScheduleId = scheduleId
        SendCommand("", True)

        For Each kvp As KeyValuePair(Of String, Tuple(Of String, String)) In mMainViewModel.getProj.Panel.SchedulerReport.Schedules
            Dim weekday As String = kvp.Key
            Dim v2 As Tuple(Of String, String) = kvp.Value

            SendCommand("#")        'Top of menu
            SendCommand("a")        'Application
            SendCommand("b")        'BacNet
            SendCommand("s")        'Schedule
            SendCommand("w")        'Weekly
            SendCommand("a")        'Add
            SendCommand("", True)   'Field panel
            SendCommand(scheduleId, True)   'Schedule Id
            SendCommand(WeekdayAbbrev(weekday), True)   'Weekday (m,tu,w,th,f,sa,su)
            SendCommand("05:00:00", True)   'Time (HH:MM:SS)
            resp = SendCommand("2", True)   'Value (can be NULL, but not sure what this signifies)
            'check that response contains "Command successful"

            'Add target to link schedule object to command object.  One schedule can control one or more commands....
            SendCommand("#")        'Top of menu
            SendCommand("a")        'Application
            SendCommand("b")        'BacNet
            SendCommand("s")        'Schedule
            SendCommand("t")        'Targets
            SendCommand("a")        'Add
            SendCommand("", True)   'Field panel
            SendCommand(scheduleId, True)   'Schedule Id
            SendCommand(commandEncodedName, True)   'Encoded Name
            SendCommand("85", True)   'Property Id
            resp = SendCommand("", True)   'Array Index
            'check that response contains "Command successful"


            SendCommand("m", True)   'Weekday (m,tu,w,th,f,sa,su)
            SendCommand("05:00:00", True)   'Time (HH:MM:SS)
            resp = SendCommand("2", True)   'Value (can be NULL, but not sure what this signifies)
            'check that response contains "Command successful"

            'Do whatever you want with v2:
            'If v2.ImageID = .... Then
        Next




        Return ""


    End Function



    Private Function WeekdayAbbrev(ByVal fullString) As String

        Select Case fullString
            Case "Monday"
                Return "m"
            Case "Tuesday"
                Return "tu"
            Case "Wednesday"
                Return "w"
            Case "Thursday"
                Return "th"
            Case "Friday"
                Return "f"
            Case "Saturday"
                Return "sa"
            Case "Sunday"
                Return "su"
            Case Else
                Return ""
        End Select

    End Function



    'TODO: doesn't schedule have to be done before SSTO?




    Private Function SendCommand(ByVal command As String, Optional ByVal sendLineBreaks As Boolean = False) As String

        AddToLog(command)
        Dim cmdOutput As String = mMainViewModel.getProj.Panel.Port.SendCommand(command, sendLineBreaks)
        AddToLog(cmdOutput)
        'TODO: possibly insert sleep in here
        Return cmdOutput

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
