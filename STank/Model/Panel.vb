Imports System.ComponentModel

Public Class Panel
    Implements INotifyPropertyChanged
    'The Panel object can be thought of as responsible for representing the hardware panels we connect to

    ' -------------
    ' Data Members
    ' -------------
    ' mName, name of STankProj
    ' mPanels, list of Panel objects

    Private mName As String
    Private mDatabase As PanelDatabase
    Private mCommPort As CommPort
    Private mProgram As Program

    Public Event PropertyChanged As PropertyChangedEventHandler _
Implements INotifyPropertyChanged.PropertyChanged

    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub

    Public Property Name As String
        Get
            Return mName
        End Get

        Set(value As String)
            mName = value
            NotifyPropertyChanged("Name")
        End Set
    End Property

    Public Property Database As PanelDatabase
        Get
            Return mDatabase
        End Get

        Set(value As PanelDatabase)
            mDatabase = value
            NotifyPropertyChanged("Database")
        End Set
    End Property

    Public Property Port As CommPort
        Get
            Return mCommPort
        End Get

        Set(value As CommPort)
            mCommPort = value
            NotifyPropertyChanged("Port")
        End Set
    End Property

    Public Property Program As Program
        Get
            Return mProgram
        End Get

        Set(value As Program)
            mProgram = value
            NotifyPropertyChanged("Program")
        End Set
    End Property





End Class
