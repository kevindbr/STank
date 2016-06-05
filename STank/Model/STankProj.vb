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

    Public Event PropertyChanged As PropertyChangedEventHandler _
      Implements INotifyPropertyChanged.PropertyChanged

    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub

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

End Class
