Imports System.ComponentModel
Imports System.Array
Imports System.Text.RegularExpressions
Imports System.Data
Imports System.Threading

Public Class CommPort
    Implements INotifyPropertyChanged
    'CommPort object represents the connection logic for the seriel port associated with this panel 

    ' -------------
    ' Data Members
    ' -------------

    Private mPanel As Panel
    Private mType As String
    Private mHostString As String
    Private mServiceType As String
    Private mTcpPort As Integer
    Private mSshVersion As String
    Private mProtocol As String
    Private mPortName As String
    Private mUserName As String
    Private mPassword As String
    Private mLoginValid As Boolean


    Private sp As IO.Ports.SerialPort       'TODO: rename
    Private logger As BaseMainViewModel.Logger

    Private mLog As System.Windows.Controls.ListBox     'not great design practice, but need to direct log output from here


    Public Sub InitLogger(ByVal del As [Delegate])


    End Sub

    Private Sub AddToLog(ByVal line As String, ByVal logger As BaseMainViewModel.Logger)
        'Need this because this code doesn't run on UI thread but needs to modify UI elements

        logger(line)

        'Dispatcher.Invoke(Sub()
        '                      mMainViewModel.Log(log, line)
        '                  End Sub)
    End Sub


    Public Event PropertyChanged As PropertyChangedEventHandler _
  Implements INotifyPropertyChanged.PropertyChanged

    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub

    Public Property Type As String
        Get
            Return mType
        End Get

        Set(value As String)
            mType = value
            NotifyPropertyChanged("Type")
        End Set
    End Property

    Public Property HostString As String
        Get
            Return mHostString
        End Get

        Set(value As String)
            mHostString = value
            NotifyPropertyChanged("HostString")
        End Set
    End Property

    Public Property ServiceType As String
        Get
            Return mServiceType
        End Get

        Set(value As String)
            mServiceType = value
            NotifyPropertyChanged("ServiceType")
        End Set
    End Property

    Public Property TcpPort As Integer
        Get
            Return mTcpPort
        End Get

        Set(value As Integer)
            mTcpPort = value
            NotifyPropertyChanged("TcpPort")
        End Set
    End Property

    Public Property SshVersion As String
        Get
            Return mSshVersion
        End Get

        Set(value As String)
            mSshVersion = value
            NotifyPropertyChanged("SshVersion")
        End Set
    End Property

    Public Property Protocol As String
        Get
            Return mProtocol
        End Get

        Set(value As String)
            mProtocol = value
            NotifyPropertyChanged("Protocol")
        End Set
    End Property

    Public Property PortName As String
        Get
            Return mPortName
        End Get

        Set(value As String)
            mPortName = value
            NotifyPropertyChanged("PortName")
        End Set
    End Property

    Public Property UserName As String
        Get
            Return mUserName
        End Get

        Set(value As String)
            mUserName = value
            NotifyPropertyChanged("UserName")
        End Set
    End Property


    Public Property Password As String
        Get
            Return mPassword
        End Get

        Set(value As String)
            mPassword = value
            NotifyPropertyChanged("Password")
        End Set
    End Property

    Public Property LoginValid As Boolean
        Get
            Return mLoginValid
        End Get

        Set(value As Boolean)
            mLoginValid = value
            NotifyPropertyChanged("LoginValid")
        End Set
    End Property


    Sub IntializeData()
        mPortName = "null"
        mHostString = "null"
        mProtocol = "null"
        mServiceType = "null"
        mSshVersion = "null"
        mTcpPort = 0
        mType = "null"
        mUserName = "high"
        mPassword = "high1"
    End Sub






    Public Function GetPointInstanceNumber(ByVal pointName As String) As String

        Dim resp As String

        SendCommand("#")        'Top of menu
        SendCommand("p")        'Point
        SendCommand("d")        'Display
        SendCommand("d")        'Definition
        SendCommand("n")        'Name
        resp = SendCommand(pointName, True)   'Point name
        resp = SendCommand("", True)   'Field panel
        resp = SendCommand("", True)   'Here, Printer

        If resp.Contains("not found") Then Return ""

        If resp.Equals("") Then Return ""

        'Dim matches As MatchCollection = Regex.Matches(reRegex.Escape("DEFINE(") & "(.*)" & Regex.Escape(",""") & "(.*)" & """" & "(.*)")
        Dim instanceNumber = Regex.Matches(resp, "Instance Number\s+:\s+([0-9]+)").Item(0).Groups(1).ToString()

        Return instanceNumber


    End Function



    Public Sub DeletePoint(ByVal pointName As String)

        Dim resp As String

        SendCommand("#")        'Top of menu
        SendCommand("p")        'Point
        SendCommand("e")        'Edit
        SendCommand("d")        'Delete
        SendCommand(pointName, True)   'Point name
        resp = SendCommand("y")   'Are you sure (y/n)
        'check that response contains "Command successful"

    End Sub



    Sub CreatePoint(ByVal pointName As String, ByVal instanceNumber As String, ByVal stateTextTableId As String)

        Dim resp As String

        'better to have newLines, Command?

        SendCommand("#")        'Top of menu
        SendCommand("p")        'Point
        SendCommand("e")        'Edit
        SendCommand("a")        'Add
        SendCommand(pointName, True)   'Point system name
        SendCommand(instanceNumber, True)   'Instance Number     (will auto-assign if not specified)
        'can't send blank.  Needs to be a number?
        SendCommand("", True)   'Point name          (already gets filled in from system name, so just press enter)
        SendCommand("LENUM", True)   'Point type (12 = LENUM)      (enter LENUM)
        SendCommand("", True)   'Descriptor
        SendCommand(stateTextTableId, True)   'State text table.  Need to get ID for the one just added, based on State Text Table Log
        SendCommand("", True)   'Access group(s)
        SendCommand("n")   'Alarmable (Y/N)       NOTE: these y/n commands may not require space to be sent
        SendCommand("", True)   'Field panel
        SendCommand("v", True)   'Physical, Virtual Point address
        SendCommand("", True)   'Point


        resp = SendCommand("VAC", True)   'Relinquish Default   (pre-fills in 2, maybe just leave).  Must match state text table?
        'TODO: not sure what to send here


        'check that response contains "Command successful"

    End Sub



    Sub CreateStateTextTable(ByVal stateTextTable As StateTextDoc.StateTextTable)


        'TODO: first check if table exists (from state text table look report) - only create if it doesn't


        ' or delete and re-add....

        Dim resp As String

        SendCommand("#")        'Top of menu
        SendCommand("s")        'System
        SendCommand("t")        'Text
        SendCommand("e")        'Edit
        SendCommand("t")        'Table
        SendCommand("a")        'Add
        SendCommand("BAC_" + stateTextTable.tableName, True)   'State text table name
        SendCommand(stateTextTable.tableId, True)   'State text table ID

        For i As Integer = 0 To stateTextTable.namesList.Count - 1
            SendCommand(stateTextTable.valuesList(i), True)   'Value
            SendCommand(stateTextTable.namesList(i), True)   'State text
        Next


        resp = SendCommand("", True)    'Send blank line to indicate done adding states?
        'check that response contains "Command successful"

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
        'SendCommand(commandName, True)   'Command name
        'SendCommand("", True)   'Command instance
        'resp = SendCommand("", True)   'Description
        Dim commandInstance = ""
        If Not resp = "" Then
            commandInstance = Regex.Matches(resp, "([0-9]+)\s+" + commandName).Item(0).Groups(1).ToString()
        End If

        SendCommand("", True)

        Return commandInstance

    End Function


    Function CreateCommandActions(ByVal commandName As String, ByVal commandId As String,
                                  ByVal fieldPanel As String, ByVal stateTextTable As StateTextDoc.StateTextTable,
                                  ByVal pointId As String) As String
        'This ties the point to the command.  Even though the point knew about the state text on creation, it needs to now be controlled by
        'the schedule command.

        'Each schedule object controls one or more command object.  All actionlist items corresponding to each command object are executed according to the schedule.


        'For a new point, need a new action list, and hence also a new command.  Only one action list per command, and an action list is tied to a single point.


        Dim resp As String

        For i As Integer = 0 To stateTextTable.namesList.Count - 1
            SendCommand(stateTextTable.valuesList(i), True)   'Value
            SendCommand(stateTextTable.namesList(i), True)   'State text
            SendCommand("#")        'Top of menu
            SendCommand("a")        'Application
            SendCommand("b")        'BacNet
            SendCommand("m")        'coMmander
            SendCommand("a")        'Actionlists
            SendCommand("a")        'Add
            SendCommand("", True)   'Field panel
            SendCommand(commandId, True)   'Command instance
            SendCommand(stateTextTable.valuesList(i), True)        'Action list index
            SendCommand(stateTextTable.namesList(i), True)    'Action text
            SendCommand(stateTextTable.valuesList(i), True)    'Sequence #
            SendCommand(fieldPanel, True)    'Device Instance Number (get this from...?  Same as field panel number)
            'can we just send blank here?

            SendCommand("19", True)    'Object Type (19 = multistate-value)
            SendCommand(pointId, True)
            SendCommand("85", True)    'Property Id (85 = present-value)
            SendCommand("", True)    'Array Index
            SendCommand("u")    'Bool, Real, Enum, Unsigned
            SendCommand(stateTextTable.valuesList(i), True)    'Value
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




    Function CreateSchedule(ByVal scheduleName As String) As String
        Dim scheduleId = ""
        Try


            Dim resp As String

            resp = SendCommand("#")        'Top of menu
            resp = SendCommand("a")        'Application
            resp = SendCommand("b")        'BacNet
            resp = SendCommand("s")        'Schedule
            resp = SendCommand("e")        'Edit
            resp = SendCommand("a")        'Add
            resp = SendCommand("", True)   'Field panel
            resp = SendCommand(scheduleName, True)   'Schedule name
            resp = SendCommand("", True)        'Schedule ID (should be auto-assigned, then we get it below)
            resp = SendCommand("", True)   'Description
            resp = SendCommand("1", True)   'Default Value
            resp = SendCommand("u")    'Bool, Real, Enum, Unsigned
            resp = SendCommand("16", True)    'Priority for writing (1-16)
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
            SendCommand("", True)   'First Id
            resp = SendCommand("", True)   'Last Id
            scheduleId = Regex.Matches(resp, "([0-9]+)\s+" + scheduleName).Item(0).Groups(1).ToString()
            'mMainViewModel.getProj.Panel.SchedulerReport.ScheduleId = scheduleId
            SendCommand("", True)

        Catch ex As Exception

        End Try

        'mPanel.SchedulerReport.ScheduleId = scheduleId     'why is mPanel null here?

        Return scheduleId


    End Function



    Function PopulateSchedule(ByVal scheduleId As String, ByVal weekday As String, ByVal startTime As String, ByVal stopTime As String) As String

        Dim resp As String

        SendCommand("#")        'Top of menu
        SendCommand("a")        'Application
        SendCommand("b")        'BacNet
        SendCommand("s")        'Schedule
        SendCommand("w")        'Weekly
        SendCommand("a")        'Add
        SendCommand("", True)   'Field panel
        SendCommand(scheduleId, True)   'Schedule Id
        SendCommand(WeekdayAbbrev(weekday), True)   'Weekday (m,tu,w,th,f,sa,su)
        SendCommand(startTime + ":00", True)   'Time (HH:MM:SS)
        resp = SendCommand("2", True)   'Value (can be NULL, but not surle what this signifies)
        'TODO: not sure what to send here.  2 for start, 1 for stop?
        'check that response contains "Command successful"

        SendCommand("#")        'Top of menu
        SendCommand("a")        'Application
        SendCommand("b")        'BacNet
        SendCommand("s")        'Schedule
        SendCommand("w")        'Weekly
        SendCommand("a")        'Add
        SendCommand("", True)   'Field panel
        SendCommand(scheduleId, True)   'Schedule Id
        SendCommand(WeekdayAbbrev(weekday), True)   'Weekday (m,tu,w,th,f,sa,su)
        SendCommand(stopTime + ":00", True)   'Time (HH:MM:SS)
        resp = SendCommand("1", True)   'Value (can be NULL, but not surle what this signifies)
        'TODO: not sure what to send here.  2 for start, 1 for stop?
        'check that response contains "Command successful"

        Return ""
    End Function


    Function LinkScheduleToCommand(ByVal scheduleId As String, ByVal commandEncodedName As String) As String

        Dim resp As String

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







    Public Sub ConfigureSSTO(ByVal replacementValues As Dictionary(Of String, String), ByVal zoneData As Dictionary(Of String, String),
                             ByVal scheduleId As String)

        Dim resp As String

        BaseMainViewModel.UpdateProgress(0.2)

        'General SSTO settings
        SendCommand("#")        'Top of menu
        SendCommand("a")        'Application
        SendCommand("b")        'BacNet
        SendCommand("t")        'ssTo
        SendCommand("g")        'General
        SendCommand("a")        'Add
        SendCommand(scheduleId, True)   'Schedule id

        If (zoneData("Optimization") = "Enabled") Then

            SendCommand(If(zoneData("Start Optimization") = "Enabled", "y", "n"))   'Enable strt optimization (Y/N)
            SendCommand(If(zoneData("Stop Optimization") = "Enabled", "y", "n"))   'Enable stop optimization (Y/N)
            SendCommand(If(zoneData("Night Operation") = "Enabled", "y", "n"))   'Enable night operation (Y/N)
            SendCommand(GetOperationCommand(zoneData("Allowed Operation")))   'Allowed operation (N,H,C,hAc)
            SendCommand(GetOperationCommand(zoneData("Selected Operation")))   'Desired oper (N,H,C,hAc,R)  'Not sure about this R option....'

            If Not replacementValues Is Nothing Then
                SendCommand(replacementValues(zoneData("Outside Temperature")), True)   'Outside temperature
                SendCommand(replacementValues(zoneData("Zone Temperature")), True)   'Zone temperature
            End If


            SendCommand(zoneData("Heating Setpoint (OCC)"), True)   'Heating setpoint-occupancy
            SendCommand(zoneData("Heating Setpoint (VAC)"), True)   'Heating setpoint-vacancy
            SendCommand(zoneData("Cooling Setpoint (OCC)"), True)   'Cooling setpoint-occupancy
            resp = SendCommand(zoneData("Cooling Setpoint (VAC)"), True)   'Cooling setpoint-vacancy
            'check that response contains "Command successful"
        End If

        BaseMainViewModel.UpdateProgress(0.4)
        If (zoneData("Optimization") = "Enabled") Then
            'Start time optimization
            'TODO: only do this if enabled in report?
            If (zoneData("Start Optimization") = "Enabled") Then
                SendCommand("#")        'Top of menu
                SendCommand("a")        'Application
                SendCommand("b")        'BacNet
                SendCommand("t")        'ssTo
                SendCommand("s")        'Start
                SendCommand("a")        'Add
                SendCommand(scheduleId, True)   'Schedule id
                For Each mode As SstoMode In System.Enum.GetValues(GetType(SstoMode))
                    SendCommand(GetSstoData(mode, zoneData("Mode (Start)")), True)   'Start mode-htg
                    SendCommand(If(GetSstoData(mode, zoneData("If too early")) = "OCC1", "y", "n"))   'Early-next occ mode-htg (Y/N)    'not sure about this comparison
                    SendCommand(If(GetSstoData(mode, zoneData("If too late")) = "Use Next Mode", "y", "n"))   'Late-occ mode-htg (Y/N)    'not sure about this comparison
                    SendCommand(If(GetSstoData(mode, zoneData("Time Shift")) = "Inside and Outside", "a", "b"))   'Time shift-htg (Fxd,Bsc,Adv)    '3 options, how to map?
                    SendCommand(If(GetSstoData(mode, zoneData("Use Learning")) = "Yes", "y", "n"))   'Use learning-htg (Y/N)
                    SendCommand(GetSstoData(mode, zoneData("Outside Temp Limit")), True)   'Outside temp limit-htg
                    SendCommand(GetSstoData(mode, zoneData("Zone Temp Deviation")), True)   'Zone temp deviation-htg 
                    SendCommand(GetSstoData(mode, zoneData("Min Start Duration")), True)   'Min start duration-htg (min)
                    SendCommand(GetSstoData(mode, zoneData("Max Start Duration")), True)   'Max start duration-htg (min)
                    SendCommand(GetSstoData(mode, zoneData("Max Extension Time")), True)   'Max extension time-htg (min)

                    If mode = SstoMode.Heating Then
                        SendCommand(GetSstoData(mode, zoneData("Max Setpoint Offset")), True)   'Maximum setpoint offset-htg
                    End If

                Next
            End If

            BaseMainViewModel.UpdateProgress(0.6)

            If (zoneData("Night Operation") = "Enabled") Then
                SendCommand("#")        'Top of menu
                SendCommand("a")        'Application
                SendCommand("b")        'BacNet
                SendCommand("t")        'ssTo
                SendCommand("n")        'Night
                SendCommand("a")        'Add
                SendCommand(scheduleId, True)   'Schedule id
                SendCommand(GetSstoData(SstoMode.Heating, zoneData("Mode (Night)")), True)   'Night mode-htg
                SendCommand(GetSstoData(SstoMode.Cooling, zoneData("Mode (Night)")), True)   'Night mode-clg
                resp = SendCommand(zoneData("Differential"), True)   'Differential
                'check that response contains "Command successful"
            End If
        End If

        BaseMainViewModel.UpdateProgress(1.0)

        'TODO: probably don't need to implement stop time optimization, they said...it wasn't in the example report anyway

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




    Function CreateEnhancedAlarms(ByVal alarmsDataRow As DataRow) As String

        Dim resp As String

        'First need to create reference point

        SendCommand("#")        'Top of menu
        SendCommand("p")        'Point
        SendCommand("e")        'Edit
        SendCommand("a")        'Add
        SendCommand(alarmsDataRow.Item("SysName") + "V", True)   'Point system name
        SendCommand("", True)   'Instance Number     (will auto-assign if not specified)
        SendCommand("", True)   'Point name          (already gets filled in from system name, so just press enter)
        SendCommand("LAO", True)   'Point type (is this always the type we use?)
        SendCommand("", True)   'Descriptor
        SendCommand(alarmsDataRow.Item("Format").Substring(0, 1))   'Float, Integer, Time, Date, dAte-time
        SendCommand(alarmsDataRow.Item("Decimal"), True)   'Number of decimal places
        SendCommand(alarmsDataRow.Item("Eng units"), True)   'Engineering units    (has this already been replaced with BACnet?)
        SendCommand("", True)   'Access group(s)
        SendCommand("n")   'Alarmable (Y/N)       NOTE: these y/n commands may not require space to be sent
        SendCommand("", True)   'Field panel
        SendCommand("v", True)   'Physical, Virtual Point address
        SendCommand("", True)   'Point
        SendCommand("1", True)   'COV limit (is this always the value we send?)
        resp = SendCommand("56", True)   'Relinquish Default   (is this always the value we send?)

        BaseMainViewModel.UpdateProgress(0.5)


        SendCommand("#")        'Top of menu
        SendCommand("p")        'Point
        SendCommand("a")        'Alarm
        SendCommand("v")        'Event
        SendCommand("e")        'Edit
        SendCommand("a")        'Add
        SendCommand("", True)   'Field panel
        SendCommand(alarmsDataRow.Item("SysName") + "_EE", True)   'Event Enrollment Name
        SendCommand("", True)   'Event enrollment instance
        SendCommand("", True)   'Event enrollment description
        SendCommand("Y")   'Report as Alarm (Y/N)
        SendCommand("Y")   'OFFNORMAL event enabled (Y/N)
        SendCommand("Y")   'NORMAL event enabled (Y/N)
        SendCommand("Y")   'FAULT event enabled (Y/N)
        SendCommand("3", True)   'Notification ID
        SendCommand("0", True)   'Alarm Message number
        SendCommand(alarmsDataRow.Item("SysName"), True)   'Reference point name
        SendCommand("LIM", True)   'Event Type
        SendCommand(alarmsDataRow.Item("SysName") + "V", True)   'Setpoint reference
        SendCommand(alarmsDataRow.Item("D1 L2 Offset"), True)   'High diff limit          'TODO: not sure these are right.  Shouldn't these come from Pt Export?
        SendCommand(alarmsDataRow.Item("D1 L1 Offset").ToString.Trim("-"), True)   'Low diff limit (can't be negative?  How does this work?
        'SendCommand(alarmsDataRow.Item("Deadband"), True)   'Deadband (seems to be blank)
        SendCommand("", True)   'Deadband
        resp = SendCommand(alarmsDataRow.Item("Level delay"), True)   'TimeDelay (sec)
        'check that response contains "Command successful"

        BaseMainViewModel.UpdateProgress(1.0)


        Return ""

    End Function




    'PPCL replacement via HMI...not currently used, but may later

    Function RetrieveProgram() As String

        Dim returnStr = ""

        Login()

        SendCommand("a")   'Application
        SendCommand("p")   'Ppcl
        SendCommand("d")   'Display

        SendCommand("", True)  'Program name
        SendCommand("", True)  'Field panel
        SendCommand("", True)  'First line #
        SendCommand("", True)  'Last line #
        SendCommand("", True)  'Here, Printer

        Try
            sp.ReadLine() 'clear out buffer before program is read
        Catch ex As TimeoutException
        End Try

        returnStr = ReadLines(sp)

        Logout()

        Return returnStr


    End Function



    Sub ReplaceProgram(ByVal lines As List(Of String))


        Login()

        SendCommand("a")   'Application
        SendCommand("p")   'Ppcl
        SendCommand("e")   'Edit
        SendCommand("AHU5", True)  'Program name (shouldn't be hard-coded)
        SendCommand("d")   'Delete
        SendCommand("y")   'Yes
        SendCommand("y")   'Yes

        SendCommand("e")   'Edit
        SendCommand("AHU5", True)  'Program name (shouldn't be hard-coded)

        'How come sometimes it doesn't ask for these?
        SendCommand("", True)  'Field Panel
        SendCommand("16", True)  'Writing Priority


        For Each str As String In lines '.Take(20)
            SendCommand("a")   'Add
            SendCommand(str, True)
        Next

        Logout()


    End Sub




    Public Function TestLogin(Optional ByVal isFresh As Boolean = True) As Boolean

        If Not mPortName.Equals("No Active Comm Ports") Then

            Me.sp = My.Computer.Ports.OpenSerialPort(mPortName)     'is this set whenever changed in Connection View?

            If Not (sp Is Nothing) Then
                If (sp.IsOpen()) Then

                    sp.ReadTimeout = 200
                    sp.NewLine = vbCr
                    sp.BaudRate = 115200

                    If Not isFresh Then Return ""

                    Dim resp As String

                    SendCommand("", True)           'get initial response from panel
                    SendCommand("h")                'Hello
                    SendCommand(mUserName, True)       'Username
                    resp = SendCommand(mPassword, True)      'Password

                    Logout()

                    If resp.ToLower().Contains("invalid") Then
                        mLoginValid = False
                        Return False
                    End If

                    If resp.Equals("") Then
                        mLoginValid = False
                        Return False
                    End If

                    mLoginValid = True
                    Return True
                End If
            End If
        End If

        mLoginValid = False
        Return False

    End Function


    Public Function Login(Optional ByVal isFresh As Boolean = True) As String

        Dim fieldPanel = ""
        Try


            Me.sp = My.Computer.Ports.OpenSerialPort(mPortName)     'is this set whenever changed in Connection View?

            sp.ReadTimeout = 200
            sp.NewLine = vbCr
            sp.BaudRate = 115200

            If Not isFresh Then Return ""

            Dim resp As String

            SendCommand("", True)           'get initial response from panel
            Thread.Sleep(1000)
            SendCommand("h")                'Hello
            SendCommand(mUserName, True)       'Username
            resp = SendCommand(mPassword, True)      'Password

            'Dim matches As MatchCollection = Regex.Matches(reRegex.Escape("DEFINE(") & "(.*)" & Regex.Escape(",""") & "(.*)" & """" & "(.*)")
            fieldPanel = Regex.Matches(resp, "Field panel <([0-9]+)>").Item(0).Groups(1).ToString()

            SendCommand("", True)
        Catch ex As Exception

            fieldPanel = ex.Message

            If (ex.Message.ToLower().Contains("denied")) Then
                Logout()

            End If


            Throw New Exception(ex.Message + " ")


        End Try


        Return fieldPanel

    End Function


    Public Sub Logout()
        If Not (sp Is Nothing) Then
            If (sp.IsOpen()) Then

                SendCommand("#", True)           '
                SendCommand("b")                'Bye
                SendCommand("y")                'Yes

                sp.Close()
            End If
        End If
    End Sub




    Private Function SendCommands(ByVal sp As IO.Ports.SerialPort, ByVal commands As List(Of String), Optional ByVal sendLineBreaks As Boolean = False) As String

        'TODO: may be useful to build output from a series of commands?  Or just return the output of the last?
        Dim response As String = ""
        For Each command As String In commands
            response = SendCommand(command, sendLineBreaks)
        Next
        Return response

    End Function


    Public Function SendCommand(ByVal command As String, Optional ByVal sendLineBreaks As Boolean = False) As String

        Dim resp As String

        BaseMainViewModel.WriteLog(command)

        If sendLineBreaks Then
            sp.WriteLine(command)
        Else
            sp.Write(command)
        End If

        resp = ReadLines(sp)

        BaseMainViewModel.WriteLog(resp)

        Return resp

    End Function





    'Private Function SendCommand(ByVal command As String, Optional ByVal sendLineBreaks As Boolean = False) As String

    '    AddToLog(command)
    '    Dim cmdOutput As String = mMainViewModel.getProj.Panel.Port.SendCommand(command, sendLineBreaks)
    '    AddToLog(cmdOutput)
    '    'TODO: possibly insert sleep in here
    '    Return cmdOutput

    'End Function




    Function ReadLines(ByVal sp As IO.Ports.SerialPort) As String


        Dim returnStr As String = ""
        Dim str As String
        'sp.NewLine = "\n"

        Try
            If sp.BytesToRead > 0 Then
                Do
                    str = sp.ReadLine()
                    'str = sp.ReadExisting()
                    returnStr += str '+ vbCr
                    'file.WriteLine(str)
                Loop
            End If
        Catch ex As TimeoutException


        End Try

        Return returnStr



    End Function



End Class
