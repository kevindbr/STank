Imports System.ComponentModel
Imports System.IO

Public Class NameChangeDoc
    Implements INotifyPropertyChanged

    Private mPanel As Panel
    Private mPath As String
    'Private mReplacementValues As Dictionary(Of String, String)



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


    'TODO: does it make sense to store these, or retrieve them from the file every time?
    Public Function getReplacementValues() As Dictionary(Of String, String)
        Dim replacementValues = New Dictionary(Of String, String)
        Dim lines() As String = File.ReadAllLines(mPath)
        For Each line As String In lines
            Dim vals() As String = line.Split(","c)
            If (vals(0) <> "" And vals(1) <> "") Then
                Try
                    replacementValues.Add(vals(0), vals(1))
                Catch
                    MsgBox("Please check columns in csv file")
                End Try
            End If
        Next line
        Return replacementValues
    End Function

End Class
