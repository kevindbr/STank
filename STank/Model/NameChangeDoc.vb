Imports System.ComponentModel

Public Class NameChangeDoc
    Implements INotifyPropertyChanged

    Private mPanel As Panel
    Private mPath As String


    Public Event PropertyChanged As PropertyChangedEventHandler _
  Implements INotifyPropertyChanged.PropertyChanged

    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub


    Public Property Path As String
        Get
            Return mPath
        End Get

        Set(value As String)
            mPath = value
            NotifyPropertyChanged("Path")
        End Set
    End Property

End Class
