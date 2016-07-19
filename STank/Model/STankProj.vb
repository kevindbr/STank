Imports System.ComponentModel

Public Class STankProj
    Implements INotifyPropertyChanged

    'The STankProj object will hold 1 to many panel objects.  We will also use this object to save/load projects

    ' -------------
    ' Data Members
    ' -------------
    ' mName, name of STankProj
    ' mPanels, list of Panel objects

    Private mPanel As Panel

    Private mName As String
    Private mPanels As List(Of Panel)
    Private mWorkingDirectory As WorkingDirectory
    Private mNameChangeStatus As String
    Private mEngineeringUnitsStatus As String
    Private mStateTextStatus As String
    Private mEnhancedAlarmsStatus As String
    Private mStartStopStatus As String
    Private mSchedulesStatus As String


    Public Event PropertyChanged As PropertyChangedEventHandler _
      Implements INotifyPropertyChanged.PropertyChanged

    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub

    Public Property NameChangeStatus As String
        Get
            Return mNameChangeStatus
        End Get

        Set(value As String)
            mNameChangeStatus = value
            NotifyPropertyChanged("NameChangeStatus")
        End Set
    End Property

    Public Property EngineeringUnitsStatus As String
        Get
            Return mEngineeringUnitsStatus
        End Get

        Set(value As String)
            mEngineeringUnitsStatus = value
            NotifyPropertyChanged("EngineeringUnitsStatus")
        End Set
    End Property

    Public Property StateTextStatus As String
        Get
            Return mStateTextStatus
        End Get

        Set(value As String)
            mStateTextStatus = value
            NotifyPropertyChanged("StateTextStatus")
        End Set
    End Property


    Public Property EnhancedAlarmsStatus As String
        Get
            Return mEnhancedAlarmsStatus
        End Get

        Set(value As String)
            mEnhancedAlarmsStatus = value
            NotifyPropertyChanged("EnhancedAlarmsStatus")
        End Set
    End Property

    Public Property StartStopStatus As String
        Get
            Return mStartStopStatus
        End Get

        Set(value As String)
            mStartStopStatus = value
            NotifyPropertyChanged("StartStopStatus")
        End Set
    End Property

    Public Property SchedulesStatus As String
        Get
            Return mSchedulesStatus
        End Get

        Set(value As String)
            mSchedulesStatus = value
            NotifyPropertyChanged("SchedulesStatus")
        End Set
    End Property

    Public Property Panel As Panel
        Get
            Return mPanel
        End Get

        Set(value As Panel)
            mPanel = value
            NotifyPropertyChanged("Panel")
        End Set
    End Property

    'For now just assume a single panel - was having trouble with forms data binding using multiple panels

    Public Property Panels As List(Of Panel)
        Get
            Return mPanels
        End Get

        Set(value As List(Of Panel))
            mPanels = value
            NotifyPropertyChanged("Panels")
        End Set
    End Property

    Public Property Name As String
        Get
            Return mName
        End Get

        Set(value As String)
            mName = value
            NotifyPropertyChanged("Name")
        End Set
    End Property


    'Public Property Directory As WorkingDirectory
    '    Get
    '        Return mWorkingDirectory
    '    End Get

    '    Set(value As WorkingDirectory)
    '        mWorkingDirectory = value
    '        NotifyPropertyChanged("Directory")
    '    End Set
    'End Property

    Sub InitializeData()
        mName = "New Project"
        Panels = New List(Of Panel)

        mNameChangeStatus = "incomplete"
        mEngineeringUnitsStatus = "incomplete"
        mStateTextStatus = "incomplete"
        mEnhancedAlarmsStatus = "incomplete"
        mStartStopStatus = "incomplete"
        mSchedulesStatus = "incomplete"

    End Sub


End Class
