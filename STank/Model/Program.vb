Imports System.ComponentModel
Imports System.Text.RegularExpressions
Imports System.IO

Public Class Program

    'Not currently used; see class Ppcl

    Implements INotifyPropertyChanged

    Sub New(ByVal text As String)
        mText = text
        getVariables()
        getDefineStrings()
    End Sub



    Private files() As String
    Private csvLocation As String
    Private folder As String
    Private log As List(Of String)
    Private replacementValues As Dictionary(Of String, String)



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

















    Public Function findAllDefine() As Dictionary(Of String, String)
        Dim contents As String
        Dim variables As New Dictionary(Of String, String)
        Dim variables1 As New Collection
        Dim variables2 As New Collection

        For Each filePath As String In files
            If filePath = csvLocation OrElse Path.GetExtension(filePath) <> ".pcl" Then
                Continue For
            End If
            log.Add("PCL file found: " & filePath)
            contents = File.ReadAllText(filePath)
            variables = getVariables(contents)

            variables1 = getDefineStrings(contents)
            variables2.Add("X, TEST_DEF1")
            variables2.Add("Y, TEST_DEF2")
            contents = insertNewDefinitions(contents, variables1, variables2)


            ' findAndReplaceInFile(filePath)
        Next filePath
        findAllDefine = variables
    End Function

    Public Function getOldDefine(ByVal contents As String) As Collection
        Dim matches As MatchCollection = Regex.Matches(contents, Regex.Escape("DEFINE(") & "(.*)" & Regex.Escape(",""") & "(.*)" & """")
        Dim variables As New Collection
        For Each m As Match In matches
            variables.Add(m.ToString)
        Next m
        Return variables
    End Function

    Public Function findAndReplaceNoQuotes(ByVal filePath As String) As String
        ' scans the entire PPCL and finds any use of %x% without quotation 
        ' adds quotes to lines that missing it
        Dim Position As Integer = 0
        Dim lastPosition As Integer = 0
        Dim outputText As List(Of String) = New List(Of String)
        Dim insStr As String = """"
        Dim searchStr As String = "%"
        Dim splitStr() As String
        Dim counter As Integer = 0
        Dim tempStr As String
        Dim a As String

        Dim objReader As New System.IO.StreamReader(filePath)

        Do While objReader.Peek() <> -1

            counter = 0

            tempStr = Trim(objReader.ReadLine())


            splitStr = tempStr.Split({" "c, "-"c, "="c, "("c, ")"c, """"c, "	"c})



            For Each x In splitStr

                Position = InStr(x, searchStr)
                If Position > 0 Then

                    a = x

                    splitStr(counter) = splitStr(counter).Insert(0, insStr)
                    splitStr(counter) = splitStr(counter).Insert(x.Length + 1, insStr)

                    Position = tempStr.IndexOf(a)
                    If tempStr(Position - 1) <> """" Then
                        tempStr = tempStr.Replace(a, splitStr(counter))

                    End If

                End If
                counter += 1
            Next

            outputText.Add(tempStr)

        Loop

        ' File.WriteAllLines(filePath & "_1", outputText)
        tempStr = String.Join(vbNewLine, outputText)

        Return tempStr
    End Function

    Public Sub findAndReplaceMain(ByVal newDefinitions As Collection)


        For Each filePath As String In files
            If filePath = csvLocation OrElse Path.GetExtension(filePath) <> ".pcl" Then
                Continue For
            End If
            log.Add("PCL file found: " & filePath)

            findAndReplaceInFile(filePath, newDefinitions)

        Next filePath

        log.Add("Program finished")
        File.WriteAllLines(System.IO.Path.Combine(folder, "log.txt"), log)
    End Sub



    Public Sub findAndReplaceInFile(ByVal filePath As String, ByVal newDefinitions As Collection)

        Dim contents As String = findAndReplaceNoQuotes(filePath) ' reads file in another function and returns a string

        Dim variables As Dictionary(Of String, String) = getVariables(contents)

        Dim oldDefinitions As New Collection
        Dim keys(variables.Keys.Count - 1) As String
        Dim strReplaceVal As String

        ' findAndReplaceNoQuotes(filePath)


        variables.Values.CopyTo(keys, 0)

        For i As Integer = 0 To variables.Count - 1
            Dim count As Integer = Regex.Matches(contents, Regex.Escape("%") & "(.*?)" & Regex.Escape("%") & "(.*?)" & """").Count
            Dim matchNum As Integer = 0
            For t As Integer = 0 To count - 1
                Dim matches As MatchCollection = Regex.Matches(contents, Regex.Escape("%") & "(.*?)" & Regex.Escape("%") & "(.*?)" & """") '(.*)" + "[^\"]+");
                If matchNum >= matches.Count Then
                    Exit For
                End If
                Dim m As Match = matches(matchNum)



                'Console.WriteLine (m.Groups [2].ToString ());
                'Console.WriteLine (keys[i]);


                If variables(m.Groups(1).ToString()) = keys(i) Then
                    Dim key As String
                    key = keys(i) & m.Groups(2).ToString
                    'Console.WriteLine (key);
                    If Not replacementValues.ContainsKey(key) Then
                        Dim nextline As String = "Key, " & key & " does not exist in file " & filePath
                        log.Add(nextline)
                        Console.WriteLine(nextline)
                        matchNum += 1
                    Else
                        Dim value As String = replacementValues(key)
                        contents = contents.Replace(m.Value, value & """")
                    End If
                Else
                    matchNum += 1
                End If


            Next t
        Next i

        ' getting old DEFINE statements
        oldDefinitions = getOldDefine(contents)

        ' replacing parts of point names with %xxxxx% statements
        For i As Integer = 0 To variables.Count - 1
            strReplaceVal = "%" & variables.Keys(i) & "%"
            contents = contents.Replace(newDefinitions(i + 1).ToString, strReplaceVal)


        Next

        ' replacing old DEFINE statements with new ones
        For i As Integer = 0 To variables.Count - 1

            contents = contents.Replace(oldDefinitions(i + 1).ToString, "DEFINE (" & variables.Keys(i) & "," & newDefinitions(i + 1).ToString)

        Next

        File.WriteAllText(filePath & ".txt", contents)

    End Sub
    Public Function getVariables(ByVal contents As String) As Dictionary(Of String, String)
        Dim matches As MatchCollection = Regex.Matches(contents, Regex.Escape("DEFINE(") & "(.*)" & Regex.Escape(",""") & "(.*)" & """" & "(.*)")
        Dim variables As New Dictionary(Of String, String)()
        For Each m As Match In matches
            variables.Add(m.Groups(1).ToString(), m.Groups(2).ToString())
        Next m
        Return variables

    End Function

    Public Function getDefineStrings(ByVal contents As String) As Collection
        Dim matches As MatchCollection = Regex.Matches(contents, Regex.Escape("DEFINE(") & "(.*)" & Regex.Escape(",""") & "(.*)" & """" & "(.*)")
        Dim variables As New Collection
        Dim x, y As Integer
        For Each m As Match In matches
            x = InStrRev(contents, m.ToString)
            y = InStrRev(contents, vbNewLine, x)
            variables.Add(contents.ToString.Substring(y, x - y + Len(m.ToString)))
        Next m
        Return variables

    End Function
    Public Function insertNewDefinitions(ByVal contents As String, ByVal existingDefinitions As Collection, ByVal newDefinitions As Collection) As String
        Dim x, y, z As Integer

        Dim tempStr As String
        Dim vals() As String
        Dim newLineNumber As Integer

        ' - line number of the first define statement
        x = InStrRev(contents, existingDefinitions(existingDefinitions.Count).ToString) ' finds first define statement
        y = InStrRev(contents, vbNewLine, x) ' find previous 
        z = x - y + Len(existingDefinitions(1).ToString) - 1
        tempStr = contents.ToString.Substring(y, z)
        vals = tempStr.Split(" "c, vbTab)

        Dim lineNumber As Integer = vals(0)

        '-------------- next line number
        x = InStr(y + 1, contents, vbNewLine)
        y = InStr(x + 1, contents, vbNewLine)
        tempStr = contents.ToString.Substring(x, y - x)
        vals = tempStr.Split(" "c, vbTab)

        Dim nextLineNumber As Integer = vals(0)

        newLineNumber = lineNumber

        For Each m In newDefinitions
            newLineNumber = newLineNumber + 1
            ' check if new line of code can be inserted
            If newLineNumber < nextLineNumber Then
                Dim tempDefString As String = ""
                For k = 1 To 5 - newLineNumber.ToString.Length
                    tempDefString = "0" & tempDefString
                Next
                tempDefString = tempDefString & newLineNumber.ToString & vbTab & "DEFINE (" & m.ToString & ")" & vbNewLine
                tempStr = contents.Insert(x, tempDefString)
                contents = tempStr
                x = x + tempDefString.Length
            End If



        Next

        Return tempStr


    End Function

    Public Sub getReplacementsFromCSV()
        Dim lines() As String = IO.File.ReadAllLines(csvLocation)
        For Each line As String In lines
            Dim vals() As String = line.Split(","c)
            If (vals(0) <> "" And vals(1) <> "") Then
                Try
                    replacementValues.Add(vals(0), vals(1))

                Catch
                    MsgBox("Please check columns in csv file")
                    Exit Sub
                End Try
            End If
        Next line
    End Sub











End Class
