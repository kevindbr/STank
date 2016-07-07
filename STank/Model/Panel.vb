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
    Private mStateTextDoc As StateTextDoc
    Private mPpcl As Ppcl
    Private mSchedulerReport As SchedulerReport
    Private mZoneDefinitionReport As ZoneDefinitionReport

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


    Public Property StateTextDocument As StateTextDoc
        Get
            Return mStateTextDoc
        End Get

        Set(value As StateTextDoc)
            mStateTextDoc = value
            NotifyPropertyChanged("StateTextDocument")
        End Set
    End Property


    Public Property SchedulerReport As SchedulerReport
        Get
            Return mSchedulerReport
        End Get

        Set(value As SchedulerReport)
            mSchedulerReport = value
            NotifyPropertyChanged("SchedulerReport")
        End Set
    End Property


    Public Property ZoneDefinitionReport As ZoneDefinitionReport
        Get
            Return mZoneDefinitionReport
        End Get

        Set(value As ZoneDefinitionReport)
            mZoneDefinitionReport = value
            NotifyPropertyChanged("ZoneDefinitionReport")
        End Set
    End Property


    Public Sub InitializeData()
        mName = "New Panel"
        mDatabase = New PanelDatabase()
        mDatabase.IntializeData()
        mCommPort = New CommPort()
        mCommPort.IntializeData()

        mPpcl = New Ppcl()
        mPpcl.Path = Ppcl.EmptyPath
        'mPpcl.Path = "C:\Users\Axios\Desktop\testWorkingDir\PPCL_MBC45.pcl"

        mNameChangeDoc = New NameChangeDoc()
        mNameChangeDoc.Path = NameChangeDoc.EmptyPath
        'mNameChangeDoc.Path = "C:\Users\Axios\Desktop\testWorkingDir\EPMAHU05_NewNames.csv"

        mPanelAttributesDoc = New PanelAttributesDoc()
        mPanelAttributesDoc.Path = PanelAttributesDoc.EmptyPath

        mStateTextDoc = New StateTextDoc()
        mStateTextDoc.Path = StateTextDoc.EmptyPath

        mSchedulerReport = New SchedulerReport()
        mSchedulerReport.Path = SchedulerReport.EmptyPath

        mZoneDefinitionReport = New ZoneDefinitionReport()
        mZoneDefinitionReport.Path = ZoneDefinitionReport.EmptyPath



    End Sub



End Class
