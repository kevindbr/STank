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






End Class
