Imports System.ComponentModel
Imports System.IO

Public Class NameChangeDoc
    Implements INotifyPropertyChanged



    Private mPanel As Panel
    Private mPath As String
    Public Shared EmptyPath As String = "No Name Change Document Path Specified"

    Private mReplacementValues As Dictionary(Of String, String)



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
                getReplacementValues()
            End If

            NotifyPropertyChanged("Path")
        End Set
    End Property


    Public Property ReplacementValues As Dictionary(Of String, String)
        Get
            Return mReplacementValues
        End Get

        Set(value As Dictionary(Of String, String))
            mReplacementValues = value
            NotifyPropertyChanged("ReplacementValues")
        End Set
    End Property



    'TODO: does it make sense to store these, or retrieve them from the file every time?
    Public Sub getReplacementValues()
        mReplacementValues = New Dictionary(Of String, String)
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
        'Return replacementValues
    End Sub



    Public Sub PerformNameChange()

        Dim i As Integer = 1
        For Each kvp As KeyValuePair(Of String, String) In ReplacementValues

            Dim process = RunNameChangeUtility(kvp.Key, kvp.Value)

            'BaseMainViewModel.WriteLog("Changing name '" + kvp.Key + "' to '" + kvp.Value + "'")

            process.WaitForExit()

            BaseMainViewModel.WriteLog(process.StandardOutput.ReadToEnd)
            BaseMainViewModel.WriteLog(process.StandardError.ReadToEnd)

            'System.Threading.Thread.Sleep(100)  'just for debugging, to more easily see progress

            BaseMainViewModel.UpdateProgress(i / ReplacementValues.Count)

            i = i + 1

        Next


    End Sub

    Private Function RunNameChangeUtility(ByVal oldName As String, ByVal newName As String) As System.Diagnostics.Process

        Dim sysName As String = newName
        Dim cmd As String = "ChangeSystemName " + oldName + " " + newName + " " + sysName
        'cmd = "ChangeSystemName " 'shouldn't run without arguments

        BaseMainViewModel.WriteLog(String.Format("Running command '{0}'", cmd))


        Dim process = New System.Diagnostics.Process()
        Dim startInfo = New System.Diagnostics.ProcessStartInfo
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
        startInfo.FileName = "cmd.exe"
        startInfo.Arguments = "/C " + cmd
        startInfo.RedirectStandardOutput = True
        startInfo.RedirectStandardError = True
        startInfo.UseShellExecute = False

        startInfo.CreateNoWindow = True

        process.StartInfo = startInfo
        process.Start()


        process.WaitForExit()


        Return process

    End Function




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

End Class
