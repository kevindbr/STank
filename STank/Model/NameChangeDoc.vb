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


    Public Function getReplacementValues() As Dictionary(Of String, String)
        Dim mReplacementValues = New Dictionary(Of String, String)
        Dim lines() As String = File.ReadAllLines(mPath)
        For Each line As String In lines
            Dim vals() As String = line.Split(","c)
            If (vals(0) <> "" And vals(1) <> "") Then
                Try
                    mReplacementValues.Add(vals(0), vals(1))
                Catch
                    MsgBox("Please check columns in csv file")
                End Try
            End If
        Next line
        Return mReplacementValues
    End Function

End Class
