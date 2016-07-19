Imports System.ComponentModel
Imports System.Array
Imports System.Text.RegularExpressions

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



    Sub IntializeData()
        mPortName = "null"
        mHostString = "null"
        mProtocol = "null"
        mServiceType = "null"
        mSshVersion = "null"
        mTcpPort = 0
        mType = "null"
    End Sub


    Function ReadLines(ByVal sp As IO.Ports.SerialPort) As String


        Dim returnStr As String = ""
        Dim str As String

        Try
            Do
                str = sp.ReadLine()
                returnStr += str '+ vbCr
                'file.WriteLine(str)
            Loop
        Catch ex As TimeoutException
        End Try

        Return returnStr



    End Function



    Function GetLines(ByVal com1 As IO.Ports.SerialPort) As String

        'TODO: combine w/readLines, but use return value differently


        'Dim filename As String = "test_" + DateTime.Now.ToString("MMddyyyyhhmmss") + ".pcl"
        'System.IO.File.WriteAllText(filename, Str)

        'Dim file As System.IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(folder + Path.DirectorySeparatorChar + filename, True)

        Dim returnStr As String = ""
        Dim str As String

        Try
            Do
                str = com1.ReadLine()
                returnStr += str '+ vbCr
                'file.WriteLine(str)
            Loop
        Catch ex As TimeoutException
        End Try


        'Dim matches As MatchCollection = Regex.Matches(returnStr, "\s*" + "[a-zA-Z]+" + "\s*" + "\d+" + ".*" + vbNullChar)

        'Dim matches As MatchCollection = Regex.Matches(returnStr, ".*" + vbNullChar)


        Return returnStr


    End Function




    Function RetrieveProgram() As String

        Dim returnStr = ""

        Dim sp As IO.Ports.SerialPort = My.Computer.Ports.OpenSerialPort(mPortName)
        sp.ReadTimeout = 200
        sp.NewLine = vbCr
        sp.BaudRate = 115200

        'Console.WriteLine(sp.ReadLine)

        sp.WriteLine("")  'get initial response from panel
        ReadLines(sp)
        sp.Write("h")     'Hello
        ReadLines(sp)
        sp.WriteLine("high")  'Username
        ReadLines(sp)
        sp.WriteLine("high1") 'Password
        ReadLines(sp)
        sp.WriteLine("")
        ReadLines(sp)
        sp.Write("a")   'Application
        ReadLines(sp)
        sp.Write("p")   'Ppcl
        ReadLines(sp)
        sp.Write("d")   'Display
        ReadLines(sp)

        sp.WriteLine("")  'Program name
        ReadLines(sp)
        sp.WriteLine("")  'Field panel
        ReadLines(sp)
        sp.WriteLine("")  'First line #
        ReadLines(sp)
        sp.WriteLine("")  'Last line #
        ReadLines(sp)
        sp.WriteLine("")  'Here, Printer

        Try

            sp.ReadLine() 'clear out buffer before program is read

        Catch ex As TimeoutException
        End Try



        returnStr = GetLines(sp)


        'getVariables(st)

        'getDefineStrings(st)


        sp.Write("q")     'Quit
        ReadLines(sp)
        sp.Write("q")     'Quit
        ReadLines(sp)
        sp.Write("b")     'Bye
        ReadLines(sp)
        sp.Write("Y")     'Yes
        ReadLines(sp)

        sp.Close()

        Return returnStr


    End Function



    Sub ReplaceProgram(ByVal lines As List(Of String))


        Dim sp As IO.Ports.SerialPort = My.Computer.Ports.OpenSerialPort(mPortName)
        sp.ReadTimeout = 200
        sp.NewLine = vbCr
        sp.BaudRate = 115200

        'Console.WriteLine(sp.ReadLine)

        sp.WriteLine("")  'get initial response from panel
        ReadLines(sp)
        sp.Write("h")     'Hello
        ReadLines(sp)
        sp.WriteLine("high")  'Username
        ReadLines(sp)
        sp.WriteLine("high1") 'Password
        ReadLines(sp)
        sp.WriteLine("")
        ReadLines(sp)
        sp.Write("a")   'Application
        ReadLines(sp)
        sp.Write("p")   'Ppcl
        ReadLines(sp)

        sp.Write("e")   'Edit
        ReadLines(sp)
        sp.WriteLine("AHU5")  'Program name (shouldn't be hard-coded)
        ReadLines(sp)
        sp.Write("d")   'Delete
        ReadLines(sp)
        sp.Write("y")   'Yes
        ReadLines(sp)
        sp.Write("y")   'Yes
        ReadLines(sp)

        sp.Write("e")   'Edit
        ReadLines(sp)
        sp.WriteLine("AHU5")  'Program name (shouldn't be hard-coded)
        ReadLines(sp)

        'How come sometimes it doesn't ask for these?
        sp.WriteLine("")  'Field Panel
        ReadLines(sp)
        sp.WriteLine("16")  'Writing Priority
        ReadLines(sp)



        For Each str As String In lines '.Take(20)

            sp.Write("a")   'Add
            'Dim st2 = ReadLines(sp)
            sp.WriteLine(str)
            Dim st = ReadLines(sp)
        Next


        sp.Write("q")     'Quit
        ReadLines(sp)
        sp.Write("q")     'Quit
        ReadLines(sp)
        sp.Write("q")     'Quit
        ReadLines(sp)
        sp.Write("b")     'Bye
        ReadLines(sp)
        sp.Write("Y")     'Yes
        ReadLines(sp)


        sp.Close()



    End Sub



    Public Sub Step7()

        Dim sp As IO.Ports.SerialPort = My.Computer.Ports.OpenSerialPort(mPortName)

        Dim resp As String

        Dim fieldPanel As String = Login(sp)

        Dim commandName As String = "FPL01_BacCmd"
        Dim commandInstance As String = CreateBacnetCommand(sp, commandName)

        Dim stateTextTableId As String = "3003" 'arbitrary, but needs to be consistent
        Dim stateTextTable As Dictionary(Of String, String) = New Dictionary(Of String, String) From {{"1", "VAC"}, {"2", "OCC"}}

        'TODO: check instance number from report


        Dim commandEncodedName As String = CreateCommandActions(sp, commandName, commandInstance, fieldPanel, stateTextTable)

        CreateStateTextTable(sp, "BAC_AHU_LENUM", stateTextTableId, stateTextTable)
        CreatePoint(sp, "FPL01_LENUM", stateTextTableId)

        Dim scheduleId As String = CreateSchedule(sp, "FPL01_SchedCmd", commandEncodedName)

        ConfigureSSTO(sp, scheduleId)


        sp.Close()

    End Sub




    Function Login(ByVal sp As IO.Ports.SerialPort) As String

        'Dim sp As IO.Ports.SerialPort = My.Computer.Ports.OpenSerialPort(mPortName)
        sp.ReadTimeout = 200
        sp.NewLine = vbCr
        sp.BaudRate = 115200

        Dim resp As String

        SendCommand(sp, "", True)           'get initial response from panel
        SendCommand(sp, "h")                'Hello
        SendCommand(sp, "high", True)       'Username
        resp = SendCommand(sp, "high1", True)      'Password

        'Dim matches As MatchCollection = Regex.Matches(resp, Regex.Escape("DEFINE(") & "(.*)" & Regex.Escape(",""") & "(.*)" & """" & "(.*)")
        Dim fieldPanel = Regex.Matches(resp, "Field panel <([0-9]+)>").Item(0).Groups(1).ToString()

        SendCommand(sp, "", True)

        Return fieldPanel


    End Function



    Function CreateBacnetCommand(ByVal sp As IO.Ports.SerialPort, ByVal commandName As String) As String

        Dim resp As String

        SendCommand(sp, "#")        'Top of menu
        SendCommand(sp, "a")        'Application
        SendCommand(sp, "b")        'BacNet
        SendCommand(sp, "m")        'coMmander
        SendCommand(sp, "e")        'Edit
        SendCommand(sp, "a")        'Add
        SendCommand(sp, "", True)   'Field panel
        SendCommand(sp, commandName, True)   'Command name
        SendCommand(sp, "", True)   'Command instance
        resp = SendCommand(sp, "", True)   'Description
        'check that response contains "Command successful"

        SendCommand(sp, "#")        'Top of menu
        SendCommand(sp, "a")        'Application
        SendCommand(sp, "b")        'BacNet
        SendCommand(sp, "m")        'coMmander
        SendCommand(sp, "l")        'Log
        SendCommand(sp, "", True)   'Field panel
        SendCommand(sp, "", True)   'First Id
        resp = SendCommand(sp, "", True)   'Last Id
        SendCommand(sp, commandName, True)   'Command name
        SendCommand(sp, "", True)   'Command instance
        resp = SendCommand(sp, "", True)   'Description
        Dim commandInstance = Regex.Matches(resp, "([0-9]+)\s+" + commandName).Item(0).Groups(1).ToString()

        SendCommand(sp, "", True)

        Return commandInstance

    End Function


    Function CreateCommandActions(ByVal sp As IO.Ports.SerialPort, ByVal commandName As String, ByVal commandId As String,
                                  ByVal deviceInstanceNumber As String, ByVal stateTextTable As Dictionary(Of String, String)) As String

        Dim resp As String

        For Each kvp As KeyValuePair(Of String, String) In stateTextTable
            Dim value As String = kvp.Key
            Dim stateText As String = kvp.Value

            SendCommand(sp, "#")        'Top of menu
            SendCommand(sp, "a")        'Application
            SendCommand(sp, "b")        'BacNet
            SendCommand(sp, "m")        'coMmander
            SendCommand(sp, "a")        'Actionlists
            SendCommand(sp, "a")        'Add
            SendCommand(sp, "", True)   'Field panel
            SendCommand(sp, commandId, True)   'Command instance
            SendCommand(sp, value, True)        'Action list index
            SendCommand(sp, stateText, True)    'Action text
            SendCommand(sp, value, True)    'Sequence #
            SendCommand(sp, deviceInstanceNumber, True)    'Device Instance Number (get this from...?  Same as field panel number)
            SendCommand(sp, "19", True)    'Object Type (19 = multistate-value)
            SendCommand(sp, "1", True)    'Object Instance Number     'TODO: how do we know this?  Some screenshots say 2.... doesn't panel assign this ?
            SendCommand(sp, "85", True)    'Property Id (85 = present-value)
            SendCommand(sp, "", True)    'Array Index
            SendCommand(sp, "u")    'Bool, Real, Enum, Unsigned
            SendCommand(sp, value, True)    'Value
            SendCommand(sp, "16", True)    'Priority for writing (1-16)
            SendCommand(sp, "0", True)    'Post delay
            resp = SendCommand(sp, "n", True)    'Quit on failure (Y/N)     NOTE: y/n may not require space...like other single-letter commands
            'check that response contains "Command successful"

        Next

        SendCommand(sp, "#")        'Top of menu
        SendCommand(sp, "a")        'Application
        SendCommand(sp, "b")        'BacNet
        SendCommand(sp, "m")        'coMmander
        SendCommand(sp, "a")        'Actionlists
        SendCommand(sp, "l")        'Log
        resp = SendCommand(sp, "", True)   'Field panel
        Dim commandEncodedName = Regex.Matches(resp, "(\S+)\s+" + commandName).Item(0).Groups(1).ToString()

        Return commandEncodedName


    End Function



    Sub CreateStateTextTable(ByVal sp As IO.Ports.SerialPort, ByVal tableName As String, ByVal tableId As String, ByVal stateTextTable As Dictionary(Of String, String))

        Dim resp As String

        SendCommand(sp, "#")        'Top of menu
        SendCommand(sp, "s")        'System
        SendCommand(sp, "t")        'Text
        SendCommand(sp, "e")        'Edit
        SendCommand(sp, "t")        'Table
        SendCommand(sp, "a")        'Add
        SendCommand(sp, tableName, True)   'State text table name
        SendCommand(sp, tableId, True)   'State text table ID (arbitrary, but will need to use later)

        For Each kvp As KeyValuePair(Of String, String) In stateTextTable
            Dim value As String = kvp.Key
            Dim stateText As String = kvp.Value
            SendCommand(sp, value, True)   'Value
            SendCommand(sp, stateText, True)   'State text
        Next

        resp = SendCommand(sp, "", True)    'Send blank line to indicate done adding states?
        'check that response contains "Command successful"

    End Sub



    Sub CreatePoint(ByVal sp As IO.Ports.SerialPort, ByVal pointName As String, ByVal stateTextTableId As String)

        Dim resp As String

        'better to have sp, newLines, Command?

        SendCommand(sp, "#")        'Top of menu
        SendCommand(sp, "p")        'Point
        SendCommand(sp, "e")        'Edit
        SendCommand(sp, "a")        'Add
        SendCommand(sp, pointName, True)   'Point system name
        SendCommand(sp, "", True)   'Instance Number     (does this refer back to command action?  Probably just linked by state text table)
        SendCommand(sp, pointName, True)   'Point name
        SendCommand(sp, "12", True)   'Point type (12 = LENUM)
        SendCommand(sp, "", True)   'Descriptor
        SendCommand(sp, stateTextTableId, True)   'State text table.  Need to get ID for the one just added, based on State Text Table Log
        SendCommand(sp, "", True)   'Access group(s)
        SendCommand(sp, "n", True)   'Alarmable (Y/N)       NOTE: these y/n commands may not require space to be sent
        SendCommand(sp, "", True)   'Field panel
        SendCommand(sp, "v", True)   'Physical, Virtual Point address
        SendCommand(sp, "", True)   'Point
        resp = SendCommand(sp, "VAC", True)   'Relinquish Default
        'check that response contains "Command successful"

    End Sub



    Function CreateSchedule(ByVal sp As IO.Ports.SerialPort, ByVal scheduleName As String, ByVal commandEncodedName As String) As String

        Dim resp As String

        SendCommand(sp, "#")        'Top of menu
        SendCommand(sp, "a")        'Application
        SendCommand(sp, "b")        'BacNet
        SendCommand(sp, "s")        'Schedule
        SendCommand(sp, "e")        'Edit
        SendCommand(sp, "a")        'Add
        SendCommand(sp, "", True)   'Field panel
        SendCommand(sp, scheduleName, True)   'Schedule name
        SendCommand(sp, "", True)        'Schedule ID (should be auto-assigned, then we get it below)
        SendCommand(sp, "", True)   'Description
        SendCommand(sp, "1", True)   'Default Value
        SendCommand(sp, "u")    'Bool, Real, Enum, Unsigned
        SendCommand(sp, "16", True)    'Priority for writing (1-16)
        SendCommand(sp, "", True)   'Start date (MM/DD/YYYY) - would assume blank means forever
        SendCommand(sp, "", True)   'Weekday (m,tu,w,th,f,sa,su,*)   - not sure what this means, but leave blank for now?
        SendCommand(sp, "", True)   'Stop date (MM/DD/YYYY) - would assume blank means forever
        SendCommand(sp, "", True)   'Weekday (m,tu,w,th,f,sa,su,*)   - not sure what this means, but leave blank for now?
        SendCommand(sp, "n")   'Out of service (y/n)
        SendCommand(sp, "y")   'SSTO enabled (y/n)
        resp = SendCommand(sp, "y")   'SI unit (y/n)
        'check that response contains "Command successful"

        SendCommand(sp, "#")        'Top of menu
        SendCommand(sp, "a")        'Application
        SendCommand(sp, "b")        'BacNet
        SendCommand(sp, "s")        'Schedule
        SendCommand(sp, "l")        'Log    (or 'd' to Display - Schedule report - lets us see what was done above)
        SendCommand(sp, "", True)   'Field panel
        Dim scheduleId = Regex.Matches(resp, "([0-9]+)\s+" + scheduleName).Item(0).Groups(1).ToString()
        SendCommand(sp, "", True)

        'TODO: do this for each item in weekly schedule report.

        SendCommand(sp, "#")        'Top of menu
        SendCommand(sp, "a")        'Application
        SendCommand(sp, "b")        'BacNet
        SendCommand(sp, "s")        'Schedule
        SendCommand(sp, "w")        'Weekly
        SendCommand(sp, "a")        'Add
        SendCommand(sp, "", True)   'Field panel
        SendCommand(sp, scheduleId, True)   'Schedule Id
        SendCommand(sp, "m", True)   'Weekday (m,tu,w,th,f,sa,su)
        SendCommand(sp, "05:00:00", True)   'Time (HH:MM:SS)
        resp = SendCommand(sp, "2", True)   'Value (can be NULL, but not sure what this signifies)
        'check that response contains "Command successful"

        'Add target to link schedule object to command object.  One schedule can control one or more commands....
        SendCommand(sp, "#")        'Top of menu
        SendCommand(sp, "a")        'Application
        SendCommand(sp, "b")        'BacNet
        SendCommand(sp, "s")        'Schedule
        SendCommand(sp, "t")        'Targets
        SendCommand(sp, "a")        'Add
        SendCommand(sp, "", True)   'Field panel
        SendCommand(sp, scheduleId, True)   'Schedule Id
        SendCommand(sp, commandEncodedName, True)   'Encoded Name
        SendCommand(sp, "85", True)   'Property Id
        resp = SendCommand(sp, "", True)   'Array Index
        'check that response contains "Command successful"


        SendCommand(sp, "m", True)   'Weekday (m,tu,w,th,f,sa,su)
        SendCommand(sp, "05:00:00", True)   'Time (HH:MM:SS)
        resp = SendCommand(sp, "2", True)   'Value (can be NULL, but not sure what this signifies)
        'check that response contains "Command successful"



        Return ""


    End Function

    'TODO: doesn't schedule have to be done before SSTO?

    Private Function ConfigureSSTO(ByVal sp As IO.Ports.SerialPort, ByVal scheduleId As String)

        Dim resp As String

        Dim newNames As Dictionary(Of String, String) = mPanel.NameChangeDocument.ReplacementValues
        Dim zoneData As Dictionary(Of String, String) = mPanel.ZoneDefinitionReport.ZoneData

        'General SSTO settings
        SendCommand(sp, "#")        'Top of menu
        SendCommand(sp, "a")        'Application
        SendCommand(sp, "b")        'BacNet
        SendCommand(sp, "t")        'ssTo
        SendCommand(sp, "g")        'General
        SendCommand(sp, "a")        'Add
        SendCommand(sp, scheduleId, True)   'Schedule id
        SendCommand(sp, If(zoneData("Start Optimization") = "Enabled", "y", "n"))   'Enable strt optimization (Y/N)
        SendCommand(sp, If(zoneData("Stop Optimization") = "Enabled", "y", "n"))   'Enable stop optimization (Y/N)
        SendCommand(sp, If(zoneData("Night Operation") = "Enabled", "y", "n"))   'Enable night operation (Y/N)
        SendCommand(sp, GetOperationCommand(zoneData("Allowed Operation")))   'Allowed operation (N,H,C,hAc)
        SendCommand(sp, GetOperationCommand(zoneData("Selected Operation")))   'Desired oper (N,H,C,hAc,R)  'Not sure about this R option....'
        SendCommand(sp, newNames(zoneData("Outside Temperature")), True)   'Outside temperature
        SendCommand(sp, newNames(zoneData("Zone Temperature")), True)   'Zone temperature
        SendCommand(sp, zoneData("Heating Setpoint (OCC)"), True)   'Heating setpoint-occupancy
        SendCommand(sp, zoneData("Heating Setpoint (VAC)"), True)   'Heating setpoint-vacancy
        SendCommand(sp, zoneData("Cooling Setpoint (OCC)"), True)   'Cooling setpoint-occupancy
        resp = SendCommand(sp, zoneData("Cooling Setpoint (VAC)"), True)   'Cooling setpoint-vacancy
        'check that response contains "Command successful"

        'Start time optimization
        SendCommand(sp, "#")        'Top of menu
        SendCommand(sp, "a")        'Application
        SendCommand(sp, "b")        'BacNet
        SendCommand(sp, "t")        'ssTo
        SendCommand(sp, "s")        'Start
        SendCommand(sp, "a")        'Add
        SendCommand(sp, scheduleId, True)   'Schedule id
        For Each mode As SstoMode In System.Enum.GetValues(GetType(SstoMode))
            SendCommand(sp, GetSstoData(mode, zoneData("Mode")), True)   'Start mode-htg
            SendCommand(sp, If(GetSstoData(mode, zoneData("If too early")) = "OCC1", "y", "n"))   'Early-next occ mode-htg (Y/N)    'not sure about this comparison
            SendCommand(sp, If(GetSstoData(mode, zoneData("If too late")) = "Use Next Mode", "y", "n"))   'Late-occ mode-htg (Y/N)    'not sure about this comparison
            SendCommand(sp, If(GetSstoData(mode, zoneData("Time Shift")) = "Inside and Outside", "a", "b"))   'Time shift-htg (Fxd,Bsc,Adv)    '3 options, how to map?
            SendCommand(sp, If(GetSstoData(mode, zoneData("Use Learning")) = "Yes", "y", "n"))   'Use learning-htg (Y/N)
            SendCommand(sp, GetSstoData(mode, zoneData("Outside Temp Limit")), True)   'Outside temp limit-htg
            SendCommand(sp, GetSstoData(mode, zoneData("Zone Temp Deviation")), True)   'Zone temp deviation-htg 
            SendCommand(sp, GetSstoData(mode, zoneData("Min Start Duration")), True)   'Min start duration-htg (min)
            SendCommand(sp, GetSstoData(mode, zoneData("Max Start Duration")), True)   'Max start duration-htg (min)
            SendCommand(sp, GetSstoData(mode, zoneData("Max Extension Time")), True)   'Max extension time-htg (min)
        Next

        'TODO: probably don't need to implement stop time optimization, they said...it wasn't in the example report anyway


    End Function



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


    Private Function SendCommands(ByVal sp As IO.Ports.SerialPort, ByVal commands As List(Of String), Optional ByVal sendLineBreaks As Boolean = False) As String

        'TODO: may be useful to build output from a series of commands?  Or just return the output of the last?
        Dim response As String = ""
        For Each command As String In commands
            response = SendCommand(sp, command, sendLineBreaks)
        Next
        Return response

    End Function


    Private Function SendCommand(ByVal sp As IO.Ports.SerialPort, ByVal command As String, Optional ByVal sendLineBreaks As Boolean = False) As String

        If sendLineBreaks Then
            sp.Write(command)
        Else
            sp.WriteLine(command)
        End If
        Return ReadLines(sp)

    End Function



End Class
