Imports System.ComponentModel
Imports System.Text.RegularExpressions

Public Class Ppcl
    Implements INotifyPropertyChanged



    Private mText As String
    Private mNewText As String
    Private mNewLines As New List(Of String)
    Private mVariables As New Dictionary(Of String, String)
    Private mDefineStrings As New Collection

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

        'Remove last 2 lines (not part of the program)
        strsLst.RemoveAt(strsLst.Count - 1)
        strsLst.RemoveAt(strsLst.Count - 1)

        'Remove first 3 lines (not part of the program)
        strsLst.RemoveAt(0)
        strsLst.RemoveAt(0)
        strsLst.RemoveAt(0)


        For Each str As String In strsLst

            'Dim matches As MatchCollection = Regex.Matches(returnStr, "\s*" + "([a-zA-Z]+)" + "\s*" + "\d+" + ".*")
            'Dim matches As MatchCollection = Regex.Matches(str, "^\s*" + "([a-zA-Z]+)" + "\s*" + "(.*\s*.*)")
            Dim matches As MatchCollection = Regex.Matches(str, "^\s*" + "([a-zA-Z]+)" + "\s*" + "(.*\s*.*)")
            'Dim matches As MatchCollection = Regex.Matches(str, "\s*" + "([a-zA-Z]+)" + "\s*" + "([0-9]+)" + "\s*" + "(.*)")
            'Dim matches As MatchCollection = Regex.Matches(str, "(.*)")

            'Dim variables As New Dictionary(Of String, String)()
            For Each m As Match In matches      'should only be 1

                'lines.Add(New KeyValuePair(Of String, String)(m.Groups(2).ToString, m.Groups(3).ToString))

                Dim l As String = m.Groups(2).ToString      'whole line, including line breaks

                'Shouldn't need all these - just trying to get rid of line breaks in longer lines
                l = Regex.Replace(l, "(\r\n|\n|\r)", "")
                l = Regex.Replace(l, "\s+", " ")
                l = l.Replace(Environment.NewLine, "") ' Equals CR
                l = l.Replace(ControlChars.CrLf, "") ' CR and LF
                l = l.Replace(ControlChars.Cr, "") ' Carriage Return (CR)
                l = l.Replace(ControlChars.Lf, "") ' Line Feed (LF)

                l = l.Replace("#", "")      'This character causes an error - need to figure out why

                mNewLines.Add(l)

            Next m

        Next

    End Sub


End Class
