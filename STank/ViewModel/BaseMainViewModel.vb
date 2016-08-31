Imports System.Collections.ObjectModel
Imports System.Windows.Threading
Imports System.Windows.Automation.Peers
Imports System.Windows.Automation.Provider
Imports System.Windows.Automation
Imports System.Text


Public MustInherit Class BaseMainViewModel

    Protected mSTankProj As STankProj
    'Private portNameDefault = "No Active Comm Ports"
    Private Shared dispatcher As Dispatcher

    Private Shared logBox As System.Windows.Controls.ListBox

    Private Shared progressBar As System.Windows.Controls.ProgressBar

    Private Shared Property logFileName As String

    Private Shared Property allLogText As StringBuilder

    Private Shared Property logFilePath As String

    Public Shared Sub InitUI(ByVal dispatcher As Dispatcher, ByVal logBox As System.Windows.Controls.ListBox,
                             ByVal progressBar As System.Windows.Controls.ProgressBar, ByVal logFName As String, ByVal logFPath As String)

        BaseMainViewModel.dispatcher = dispatcher
        BaseMainViewModel.logBox = logBox
        BaseMainViewModel.progressBar = progressBar
        BaseMainViewModel.logFileName = logFName
        BaseMainViewModel.logFilePath = logFPath
        BaseMainViewModel.allLogText = New StringBuilder()

    End Sub

    Public Shared Sub ResetUI()

        BaseMainViewModel.dispatcher = Nothing
        BaseMainViewModel.logBox = Nothing
        BaseMainViewModel.progressBar = Nothing
        BaseMainViewModel.logFileName = Nothing
        BaseMainViewModel.logFilePath = Nothing
        BaseMainViewModel.allLogText.Clear()

    End Sub





    Public Sub New(ByVal sTankProj As STankProj)
        mSTankProj = sTankProj
    End Sub


    ''' <summary>
    ''' We want to allow the user to have more than one comm port connection stored
    ''' We will need to add logic to cleanly handle panel to comm port logic
    ''' </summary>
    ''' <param name="commPort"></param>
    ''' <remarks></remarks>
    Sub addNewPort(commPort As CommPort)
        If mSTankProj.Panels.Count = 1 Then
            mSTankProj.Panels.ElementAt(0).Port = commPort
        End If
    End Sub

    Function getPanels() As List(Of Panel)
        Return mSTankProj.Panels
    End Function

    Function getProj() As STankProj
        Return mSTankProj
    End Function

    ''' <summary>
    ''' Warnings are notifications that won't effect the user's ability to perform a find and replace
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 


    Public MustOverride Function getActivityWarningLogs() As List(Of String)


    Public MustOverride Function getActivityErrorLogs() As List(Of String)



    Function getMaxNumOfErrors() As Integer
        Return 4
    End Function

    Public Sub setStatus(viewNum, status)

        If viewNum = 1 Then
            mSTankProj.NameChangeStatus = status
        End If

        If viewNum = 2 Then
            mSTankProj.EngineeringUnitsStatus = status
        End If

        If viewNum = 3 Then
            mSTankProj.StateTextStatus = status
        End If

        If viewNum = 4 Then
            mSTankProj.EnhancedAlarmsStatus = status
        End If

        If viewNum = 5 Then
            mSTankProj.SchedulesStatus = status
        End If

        If viewNum = 6 Then
            mSTankProj.StartStopStatus = status
        End If

    End Sub


    Public Delegate Sub Logger(ByVal line As String)



    'Public Function GetLogger(ByVal logBox As System.Windows.Controls.ListBox) As Logger

    '    Return Sub(line As String)

    '               Dim disp As Dispatcher = dispatcher.CurrentDispatcher    'not sure if this will get the UI thread dispatcher...

    '               'dispatcher.

    '               disp.Invoke(Sub()
    '                               Log(logBox, line)
    '                           End Sub)


    '           End Sub

    '    'End Sub Dispatcher.Invoke(Sub() (Log(logBox, line)))

    'End Function


    Public Shared Sub WriteLog(ByVal line As String)
        If Not (dispatcher Is Nothing) Then
            dispatcher.Invoke(Sub()

                                  If line = "" Then Return 'when stderr is non-empty, stdout will be

                                  If Not (logBox Is Nothing) Then
                                      logBox.Items.Add(line)
                                      allLogText.AppendLine(line)

                                      If logBox.Items.Count > 10 Then
                                          logBox.Items.RemoveAt(0)
                                      End If

                                      'log.SelectedIndex = log.Items.Count - 1
                                      'log.SelectedIndex = -1

                                      'Scroll to last entry
                                      Dim svAutomation As ListBoxAutomationPeer = ScrollViewerAutomationPeer.CreatePeerForElement(logBox)
                                      Dim scrollInterface As IScrollProvider = svAutomation.GetPattern(PatternInterface.Scroll)
                                      Dim scrollVertical As ScrollAmount = ScrollAmount.LargeIncrement
                                      Dim scrollHorizontal As ScrollAmount = ScrollAmount.NoAmount
                                      If scrollInterface.VerticallyScrollable Then
                                          scrollInterface.Scroll(scrollHorizontal, scrollVertical)
                                      End If
                                  End If
                              End Sub)
        End If
    End Sub

    Public Shared Sub WriteFile()

        If Not (allLogText Is Nothing) Then
            IO.File.WriteAllText(logFilePath & "\" & String.Format("text-{0:yyyy-MM-dd_hh-mm-ss-tt}.bin", DateTime.Now) & logFileName & ".txt", allLogText.ToString())
        End If

    End Sub



    'Public Delegate Sub ProgressBar(ByVal fraction As Double)


    'Public Function GetProgressBar(ByVal progressBar As System.Windows.Controls.ProgressBar) As Logger

    '    Return Sub(fraction As Double)

    '               Dim disp As Dispatcher = Dispatcher.CurrentDispatcher    'not sure if this will get the UI thread dispatcher...

    '               'dispatcher.

    '               disp.Invoke(Sub()
    '                               UpdateProgress(progressBar, fraction)
    '                           End Sub)


    '           End Sub


    '    'End Sub Dispatcher.Invoke(Sub() (Log(logBox, line)))

    'End Function


    Public Shared Sub UpdateProgress(ByVal fraction As Double)

        dispatcher.Invoke(Sub()

                              progressBar.Value = fraction * 100

                          End Sub)

    End Sub




    Public Sub updateAllLogs(ByVal activityLog As System.Windows.Controls.TextBlock)
        activityLog.Text = ""

        Dim listOfErrors As List(Of String) = getActivityErrorLogs()
        Dim listOfWarnings As List(Of String) = getActivityWarningLogs()

        For Each notification As String In listOfErrors
            Dim noticeImage As Image = New Image()
            noticeImage.Width = 20
            noticeImage.Height = 20

            Dim bi3 As New BitmapImage
            bi3.BeginInit()
            bi3.UriSource = New Uri("Resources/Notice.png", UriKind.Relative)
            bi3.EndInit()
            noticeImage.Stretch = Stretch.Fill
            noticeImage.Source = bi3

            Dim container As InlineUIContainer = New InlineUIContainer(noticeImage)
            activityLog.Inlines.Add(container)

            Dim newLine As Run = New Run(" " + notification)
            newLine.Foreground = Brushes.Red
            activityLog.Inlines.Add(newLine)
            activityLog.Inlines.Add(New LineBreak)
        Next

        For Each notification As String In listOfWarnings
            Dim noticeImage As Image = New Image()
            noticeImage.Width = 20
            noticeImage.Height = 20

            Dim bi3 As New BitmapImage
            bi3.BeginInit()
            bi3.UriSource = New Uri("Resources/Warning.png", UriKind.Relative)
            bi3.EndInit()
            noticeImage.Stretch = Stretch.Fill
            noticeImage.Source = bi3

            Dim container As InlineUIContainer = New InlineUIContainer(noticeImage)
            activityLog.Inlines.Add(container)

            Dim newLine As Run = New Run(" " + notification)
            newLine.Foreground = Brushes.DarkOrange
            activityLog.Inlines.Add(newLine)
            activityLog.Inlines.Add(New LineBreak)
        Next

        Dim numberOfErrors As Integer = listOfErrors.Count + listOfWarnings.Count
        Dim maxNumOfErrors As Integer = getMaxNumOfErrors()


    End Sub

End Class
