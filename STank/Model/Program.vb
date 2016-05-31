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
    Private mNewText As String

    Private mNewLines As New List(Of String)



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


    Public Property NewText As String
        Get
            Return mNewText
        End Get

        Set(value As String)
            mNewText = value
            NotifyPropertyChanged("NewText")
        End Set

    End Property


    Public Property NewLines As List(Of String)
        Get
            Return mNewLines
        End Get

        Set(value As List(Of String))
            mNewLines = value
            NotifyPropertyChanged("NewLines")
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

    Public Sub changeNames(ByVal nameChangeVals As Dictionary(Of String, String))


        NewText = mText

        For Each kvp As KeyValuePair(Of String, String) In mVariables
            mNewText = mNewText.Replace("%" + kvp.Key + "%", kvp.Value)
        Next

        For Each kvp As KeyValuePair(Of String, String) In nameChangeVals
            mNewText = mNewText.Replace(kvp.Key, kvp.Value)
        Next

        StripLines()

    End Sub


    Public Sub StripLines()


        Dim strs As String() = mNewText.Split(vbNullChar)

        Dim strsLst = strs.ToList()

        strsLst.RemoveAt(strsLst.Count - 1)
        strsLst.RemoveAt(strsLst.Count - 1)

        strsLst.RemoveAt(0)
        strsLst.RemoveAt(0)
        strsLst.RemoveAt(0)


        'Dim lst As List(Of String) = New List(Of String)


        'Dim lines As New List(Of KeyValuePair(Of String, String))

        'Dim lines As New List(Of String)


        For Each str As String In strsLst





            'Dim matches As MatchCollection = Regex.Matches(returnStr, "\s*" + "([a-zA-Z]+)" + "\s*" + "\d+" + ".*")

            Dim matches As MatchCollection = Regex.Matches(str, "^\s*" + "([a-zA-Z]+)" + "\s*" + "(.*)")

            'Dim matches As MatchCollection = Regex.Matches(str, "\s*" + "([a-zA-Z]+)" + "\s*" + "([0-9]+)" + "\s*" + "(.*)")


            If (matches.Count = 2) Then

                Dim sss = "blah"

            End If



            'Dim matches As MatchCollection = Regex.Matches(str, "(.*)")
            'Dim variables As New Dictionary(Of String, String)()
            For Each m As Match In matches

                'lines.Add(New KeyValuePair(Of String, String)(m.Groups(2).ToString, m.Groups(3).ToString))


                mNewLines.Add(m.Groups(2).ToString)

                'For Each g As Group In m.Groups

                '    Dim ssss = g.ToString
                '    Dim s2 = ""

                'Next


                'Dim str1 = m.Groups(1).ToString()
                'Dim str2 = m.Groups(2).ToString()
            Next m

        Next








    End Sub





End Class
