Imports System.ComponentModel

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

End Class
