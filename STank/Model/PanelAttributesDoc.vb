Imports System.ComponentModel

Public Class PanelAttributesDoc
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

            If isValidDocument(mPath) Then

            End If

            NotifyPropertyChanged("Path")
        End Set
    End Property

    ''' <summary>
    ''' Check file extension here
    ''' </summary>
    ''' <param name="mPath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function isValidDocument(mPath As String) As Boolean
        Dim isValidFile = True

        If mPath = "No Panel Attributes Document Specified" Then
            isValidFile = False
        End If

        Return isValidFile
    End Function

End Class
