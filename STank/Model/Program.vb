Imports System.ComponentModel
Imports System.Text.RegularExpressions

Public Class Program
    Implements INotifyPropertyChanged

    Sub New(ByVal text As String)
        mText = text
        getVariables()
        getDefineStrings()
    End Sub


    Private mText As String
    Private mVariables As New Dictionary(Of String, String)
    Private mDefineStrings As New Collection

    Public Event PropertyChanged As PropertyChangedEventHandler _
Implements INotifyPropertyChanged.PropertyChanged

    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub

    Public Property Text As String
        Get
            Return mText
        End Get

        Set(value As String)
            mText = value
            NotifyPropertyChanged("Text")
        End Set
    End Property


    Public Sub getVariables()
        Dim matches As MatchCollection = Regex.Matches(mText, Regex.Escape("DEFINE(") & "(.*)" & Regex.Escape(",""") & "(.*)" & """" & "(.*)")
        'Dim variables As New Dictionary(Of String, String)()
        For Each m As Match In matches
            mVariables.Add(m.Groups(1).ToString(), m.Groups(2).ToString())
        Next m
        'Return variables

    End Sub

    Public Sub getDefineStrings()
        Dim matches As MatchCollection = Regex.Matches(mText, Regex.Escape("DEFINE(") & "(.*)" & Regex.Escape(",""") & "(.*)" & """" & "(.*)")
        'Dim defineStrings As New Collection
        Dim x, y As Integer
        For Each m As Match In matches
            x = InStrRev(mText, m.ToString)
            y = InStrRev(mText, vbNewLine, x)
            mDefineStrings.Add(mText.ToString.Substring(y, x - y + Len(m.ToString)))
        Next m
        'Return defineStrings
    End Sub




End Class
