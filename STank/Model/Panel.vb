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
    Private mNameChangeDoc As NameChangeDoc
    Private mPanelAttributesDoc As PanelAttributesDoc
    Private mPpcl As Ppcl

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

    Public Property Ppcl As Ppcl
        Get
            Return mPpcl
        End Get

        Set(value As Ppcl)
            mPpcl = value
            NotifyPropertyChanged("Ppcl")
        End Set
    End Property

    Public Property NameChangeDocument As NameChangeDoc
        Get
            Return mNameChangeDoc
        End Get

        Set(value As NameChangeDoc)
            mNameChangeDoc = value
            NotifyPropertyChanged("NameChangeDocument")
        End Set
    End Property

    Public Property PanelAttributesDocument As PanelAttributesDoc
        Get
            Return mPanelAttributesDoc
        End Get

        Set(value As PanelAttributesDoc)
            mPanelAttributesDoc = value
            NotifyPropertyChanged("PanelAttributesDocument")
        End Set
    End Property

    Public Sub InitializeData()
        mName = "New Panel"
        mDatabase = New PanelDatabase()
        mDatabase.IntializeData()
        mCommPort = New CommPort()
        mCommPort.IntializeData()

        mPpcl = New Ppcl()
        mPpcl.Path = "No PPCL path Specified"
        'mPpcl.Path = "C:\Users\Axios\Desktop\testWorkingDir\PPCL_MBC45.pcl"

        mNameChangeDoc = New NameChangeDoc()
        mNameChangeDoc.Path = "No Name Change Document Path Specified"
        'mNameChangeDoc.Path = "C:\Users\Axios\Desktop\testWorkingDir\EPMAHU05_NewNames.csv"

        mPanelAttributesDoc = New PanelAttributesDoc()
        mPanelAttributesDoc.Path = "No Panel Attributes Document Specified"

    End Sub



End Class
