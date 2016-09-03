Imports System.ComponentModel
Imports System.Text.RegularExpressions
Imports System.IO

Public Class Ppcl
    Implements INotifyPropertyChanged



    Private mText As List(Of KeyValuePair(Of String, String))
    Private mNewText As List(Of KeyValuePair(Of String, String))
    Private mNewLines As New List(Of String)



    'Private mContents As String
    Private mVariables As New Dictionary(Of String, String)


    Private mNewVariables As New Dictionary(Of String, String)



    Private mOldDefinitions As New Collection   'entire DEFINE statements

    Private mPath As String
    Private mPaths As List(Of String)

    Public Shared EmptyPath As String = "No PPCL path Specified"


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

    Public Property Paths As List(Of String)
        Get
            Return mPaths
        End Get

        Set(value As List(Of String))
            mPaths = value
            mText = New List(Of KeyValuePair(Of String, String))

            Dim allPaths = ""
            If (mPaths.Count > 0) Then
                For Each singlePath In mPaths
                    If isValidDocument(singlePath) Then
                        'findAndReplaceNoQuotes()    'reads file contents into mText, doing some basic error checking along the way
                        'mText += File.ReadAllText(singlePath)

                        mText.Add(New KeyValuePair(Of String, String)(singlePath, File.ReadAllText(singlePath)))

                        allPaths += singlePath + " "
                    End If
                Next
                getVariables()
                getOldDefine()
                Path = allPaths

            End If
            NotifyPropertyChanged("Paths")
        End Set
    End Property


    Public Property Text As List(Of KeyValuePair(Of String, String))
        Get
            Return mText
        End Get

        Set(value As List(Of KeyValuePair(Of String, String)))
            mText = value
            NotifyPropertyChanged("Text")
        End Set
    End Property


    Public Property NewText As List(Of KeyValuePair(Of String, String))
        Get
            Return mNewText
        End Get

        Set(value As List(Of KeyValuePair(Of String, String)))
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


    Public Property Variables As Dictionary(Of String, String)
        Get
            Return mVariables
        End Get

        Set(value As Dictionary(Of String, String))
            mVariables = value
            NotifyPropertyChanged("Variables")
        End Set
    End Property


    Public Property NewVariables As Dictionary(Of String, String)
        Get
            Return mNewVariables
        End Get

        Set(value As Dictionary(Of String, String))
            mNewVariables = value
            NotifyPropertyChanged("NewVariables")
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

        If mPath = EmptyPath Then
            isValidFile = False
        End If

        Return isValidFile
    End Function




    Public Sub findAndReplaceNoQuotes()
        '' scans the entire PPCL and finds any use of %x% without quotation 
        '' adds quotes to lines that missing it
        'Dim Position As Integer = 0
        'Dim lastPosition As Integer = 0
        'Dim outputText As List(Of String) = New List(Of String)
        'Dim insStr As String = """"
        'Dim searchStr As String = "%"
        'Dim splitStr() As String
        'Dim counter As Integer = 0
        'Dim tempStr As String
        'Dim a As String

        'Dim objReader As New System.IO.StreamReader(mPath)

        'Do While objReader.Peek() <> -1

        '    counter = 0

        '    tempStr = Trim(objReader.ReadLine())


        '    splitStr = tempStr.Split({" "c, "-"c, "="c, "("c, ")"c, """"c, "	"c})



        '    For Each x In splitStr

        '        Position = InStr(x, searchStr)
        '        If Position > 0 Then

        '            a = x

        '            splitStr(counter) = splitStr(counter).Insert(0, insStr)
        '            splitStr(counter) = splitStr(counter).Insert(x.Length + 1, insStr)

        '            Position = tempStr.IndexOf(a)
        '            If tempStr(Position - 1) <> """" Then
        '                tempStr = tempStr.Replace(a, splitStr(counter))

        '            End If

        '        End If
        '        counter += 1
        '    Next

        '    outputText.Add(tempStr)

        'Loop

        '' File.WriteAllLines(filePath & "_1", outputText)
        'tempStr = String.Join(vbNewLine, outputText)

        'mText = tempStr

        'mNewText = tempStr
    End Sub




    Public Sub findAndReplaceInFile2(ByVal replacementValues As Dictionary(Of String, String))

        mNewText = mText
        Dim newListOfValues As List(Of KeyValuePair(Of String, String)) = New List(Of KeyValuePair(Of String, String))

        For Each newTextSingleValue As KeyValuePair(Of String, String) In mNewText
            Dim tempValue = newTextSingleValue.Value
            ' First, go through and expand all variables so that the full names are in the file and will be matched by the entries in the name change document
            For Each kvp As KeyValuePair(Of String, String) In mVariables
                tempValue = tempValue.Replace("%" + kvp.Key + "%", kvp.Value)
            Next

            ' Replace old full names with new full names
            For Each kvp As KeyValuePair(Of String, String) In replacementValues
                'mNewText = mNewText.Replace(kvp.Key, kvp.Value)
                tempValue = tempValue.Replace(kvp.Key, kvp.Value)
                BaseMainViewModel.WriteLog(String.Format("Changing name '{0}' to '{1}' in PPCL", kvp.Key, kvp.Value))
            Next

            ' Replace old variable definitions with new variable definitions
            'For i As Integer = 0 To mVariables.Count - 1
            '    Dim variable = mVariables.Keys(i)
            '    Dim strReplaceVal = "%" & variable & "%"
            '    mNewText = mNewText.Replace(mNewVariables(variable), strReplaceVal)
            'Next
            For Each kvp As KeyValuePair(Of String, String) In mNewVariables
                ' mNewText = mNewText.Replace(kvp.Value, "%" + kvp.Key + "%")
                tempValue = tempValue.Replace("%" + kvp.Value + "%", kvp.Key)
                BaseMainViewModel.WriteLog(String.Format("Changing variable '{0}' to '{1}' in PPCL", kvp.Value, kvp.Key))
            Next

            ' However, this will also affect DEFINE statements and cause them to read like DEFINE(X, "%X"), so this must be corrected
            Dim matches As MatchCollection = Regex.Matches(tempValue, Regex.Escape("DEFINE(") & "(.*)" & Regex.Escape(",""") & "(.*)" & """" & "(.*)")
            For Each m As Match In matches
                Dim variable As String = m.Groups(1).ToString
                Dim val As String = m.Groups(2).ToString

                If (mNewVariables.Count > 0) Then
                    If (mNewVariables.Keys.Contains(variable)) Then
                        ' mNewText = mNewText.Replace(m.ToString, "DEFINE(" + variable + ",""" + mNewVariables(variable) + """)")
                        tempValue = tempValue.Replace(m.ToString, "DEFINE(" + variable + ",""" + mNewVariables(variable) + """)") 
                    End If
                End If
            Next m

            newListOfValues.Add(New KeyValuePair(Of String, String)(newTextSingleValue.Key, tempValue))

            File.WriteAllText(newTextSingleValue.Key & ".new", tempValue)
        Next

        If (newListOfValues.Count > 0) Then
            mNewText = newListOfValues
        End If
    End Sub


    Public Sub findAndReplaceInFile(ByVal replacementValues As Dictionary(Of String, String), ByVal newDefinitions As Collection)

        ''Dim contents As String = findAndReplaceNoQuotes(mPath) ' reads file in another function and returns a string

        ''Dim variables As Dictionary(Of String, String) = getVariables(contents)

        ''Dim oldDefinitions As New Collection
        'Dim keys(mVariables.Keys.Count - 1) As String
        'Dim strReplaceVal As String

        '' findAndReplaceNoQuotes(filePath)


        'mVariables.Values.CopyTo(keys, 0)

        'For i As Integer = 0 To mVariables.Count - 1
        '    Dim count As Integer = Regex.Matches(mNewText, Regex.Escape("%") & "(.*?)" & Regex.Escape("%") & "(.*?)" & """").Count
        '    Dim matchNum As Integer = 0
        '    For t As Integer = 0 To count - 1
        '        Dim matches As MatchCollection = Regex.Matches(mNewText, Regex.Escape("%") & "(.*?)" & Regex.Escape("%") & "(.*?)" & """") '(.*)" + "[^\"]+");
        '        If matchNum >= matches.Count Then
        '            Exit For
        '        End If
        '        Dim m As Match = matches(matchNum)



        '        'Console.WriteLine (m.Groups [2].ToString ());
        '        'Console.WriteLine (keys[i]);


        '        If mVariables(m.Groups(1).ToString()) = keys(i) Then
        '            Dim key As String
        '            key = keys(i) & m.Groups(2).ToString
        '            'Console.WriteLine (key);
        '            If Not replacementValues.ContainsKey(key) Then
        '                Dim nextline As String = "Key, " & key & " does not exist in file " & mPath
        '                'log.Add(nextline)
        '                'Console.WriteLine(nextline)
        '                matchNum += 1
        '            Else
        '                Dim value As String = replacementValues(key)
        '                mNewText = mNewText.Replace(m.Value, value & """")        'would really be better to use a separate variable for the new text...
        '            End If
        '        Else
        '            matchNum += 1
        '        End If


        '    Next t
        'Next i

        '' getting old DEFINE statements
        ''oldDefinitions = getOldDefine()

        '' replacing parts of point names with %xxxxx% statements
        'For i As Integer = 0 To mVariables.Count - 1
        '    strReplaceVal = "%" & mVariables.Keys(i) & "%"
        '    mNewText = mNewText.Replace(newDefinitions(i + 1).ToString, strReplaceVal)


        'Next

        '' replacing old DEFINE statements with new ones
        'For i As Integer = 0 To mVariables.Count - 1

        '    mNewText = mNewText.Replace(mOldDefinitions(i + 1).ToString, "DEFINE (" & mVariables.Keys(i) & "," & """" & newDefinitions(i + 1).ToString & """")

        'Next

        'File.WriteAllText(mPath & ".txt", mNewText)

    End Sub



    Public Sub getOldDefine()

        For Each textSingleValue As KeyValuePair(Of String, String) In mText
            Dim matches As MatchCollection = Regex.Matches(textSingleValue.Value, Regex.Escape("DEFINE(") & "(.*)" & Regex.Escape(",""") & "(.*)" & """")
            'Dim variables As New Collection
            For Each m As Match In matches
                'variables.Add(m.ToString)
                mOldDefinitions.Add(m.ToString)
            Next m
            'Return variables
        Next

    End Sub



    Public Sub getVariables()
        For Each textSingleValue As KeyValuePair(Of String, String) In mText
            Dim matches As MatchCollection = Regex.Matches(textSingleValue.Value, Regex.Escape("DEFINE(") & "(.*)" & Regex.Escape(",""") & "(.*)" & """" & "(.*)")
            'Dim variables As New Dictionary(Of String, String)()
            For Each m As Match In matches
                If (Not mVariables.Keys.Contains(m.Groups(1).ToString())) Then
                    mVariables.Add(m.Groups(1).ToString(), m.Groups(2).ToString())
                End If
            Next m
            'Return variables
        Next
    End Sub

    'Public Sub getOldDefineStrings()    'why is this different?
    '    Dim matches As MatchCollection = Regex.Matches(mText, Regex.Escape("DEFINE(") & "(.*)" & Regex.Escape(",""") & "(.*)" & """" & "(.*)")
    '    'Dim defineStrings As New Collection
    '    Dim x, y As Integer
    '    For Each m As Match In matches
    '        x = InStrRev(mText, m.ToString)
    '        y = InStrRev(mText, vbNewLine, x)
    '        mOldDefineStrings.Add(mText.ToString.Substring(y, x - y + Len(m.ToString)))
    '    Next m
    '    'Return defineStrings
    'End Sub




    Public Sub changeNames(ByVal nameChangeVals As Dictionary(Of String, String))

        NewText = mText
        Dim newListOfValues As List(Of KeyValuePair(Of String, String)) = New List(Of KeyValuePair(Of String, String))

        For Each newTextSingleValue As KeyValuePair(Of String, String) In mNewText
            Dim tempValue = newTextSingleValue.Value

            For Each kvp As KeyValuePair(Of String, String) In mVariables

                tempValue = tempValue.Replace("%" + kvp.Key + "%", kvp.Value)
            Next

            For Each kvp As KeyValuePair(Of String, String) In nameChangeVals

                tempValue = tempValue.Replace(kvp.Key, kvp.Value)
            Next

            newListOfValues.Add(New KeyValuePair(Of String, String)(newTextSingleValue.Key, tempValue))
        Next

        mNewText = newListOfValues
        StripLines()

    End Sub


    Public Sub StripLines()

        For Each newTextSingleValue As KeyValuePair(Of String, String) In mNewText

            Dim strs As String() = newTextSingleValue.Value.Split(vbNullChar)
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
        Next

    End Sub



End Class
