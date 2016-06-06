Imports System.ComponentModel
Imports System.Data
Imports System.Windows.Automation
Imports System.Windows.Automation.Provider
Imports System.Windows.Automation.Peers

Public Class FindAndReplaceView

    Private mMainViewModel As MainViewModel
    Private bw As BackgroundWorker = New BackgroundWorker

    ''' <summary>
    ''' Bring in mainViewModel to update and change project data
    ''' </summary>
    ''' <param name="mainViewModel"></param>
    ''' <remarks></remarks>
    Sub New(ByRef mainViewModel As MainViewModel)
        mMainViewModel = mainViewModel
        InitializeComponent()

    End Sub

    Private Sub IntializeWindow()
        ' updateDefineGrid()
        bw.WorkerReportsProgress = True
        bw.WorkerSupportsCancellation = True
        AddHandler bw.DoWork, AddressOf bw_RunFindAndReplace
        bw.RunWorkerAsync()
    End Sub

    Private Sub bw_RunFindAndReplace(ByVal sender As Object, ByVal e As DoWorkEventArgs)

        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)
        Dim replacementValues As Dictionary(Of String, String) = mMainViewModel.getProj.Panel.NameChangeDocument.ReplacementValues
        Dim ppcl As Ppcl = mMainViewModel.getProj.Panel.Ppcl
        'Dim newDefinitions As Collection = New Collection()

        'For Each row As DataRowView In defineGrid.ItemsSource
        '    newDefinitions.Add(row.Item(2))
        'Next

        'For Each kvp As KeyValuePair(Of String, String) In mMainViewModel.getProj.Panel.Ppcl.Variables
        '    newDefinitions.Add(kvp.Value)
        'Next

        ppcl.findAndReplaceInFile2(replacementValues)    'TODO: logging
        'ppcl.findAndReplaceInFile(replacementValues, newDefinitions)    'TODO: logging

        Dim i As Integer = 1

        For Each kvp As KeyValuePair(Of String, String) In replacementValues

            Dim oldName As String = kvp.Key
            Dim newName As String = kvp.Value
            Dim sysName As String = newName
            Dim cmd As String = "ChangeSystemName2 " + oldName + " " + newName + " " + sysName
            cmd = "ChangeSystemName " 'shouldn't run without arguments

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

            AddToLog("Changing name '" + oldName + "' to '" + newName + "'")

            process.WaitForExit()

            AddToLog(process.StandardOutput.ReadToEnd)
            AddToLog(process.StandardError.ReadToEnd)

            System.Threading.Thread.Sleep(100)  'just for debugging, to more easily see progress

            Dispatcher.Invoke(Sub()
                                  progressBar.Value = i / replacementValues.Count * 100
                              End Sub)

            i = i + 1

        Next
    End Sub

    Private Sub AddToLog(ByVal line As String)

        If line = "" Then Return 'when stderr is non-empty, stdout will be

        'Need this because this code doesn't run on UI thread but needs to modify UI elements
        Dispatcher.Invoke(Sub()

                              log.Items.Add(line)

                              If log.Items.Count > 10 Then
                                  log.Items.RemoveAt(0)
                              End If

                              'log.SelectedIndex = log.Items.Count - 1
                              'log.SelectedIndex = -1

                              'Scroll to last entry
                              Dim svAutomation As ListBoxAutomationPeer = ScrollViewerAutomationPeer.CreatePeerForElement(log)
                              Dim scrollInterface As IScrollProvider = svAutomation.GetPattern(PatternInterface.Scroll)
                              Dim scrollVertical As ScrollAmount = ScrollAmount.LargeIncrement
                              Dim scrollHorizontal As ScrollAmount = ScrollAmount.NoAmount
                              If scrollInterface.VerticallyScrollable Then
                                  scrollInterface.Scroll(scrollHorizontal, scrollVertical)
                              End If

                              'og.ScrollIntoView(log.Items.Item(log.Items.Count - 1))   'doesn't work for duplicate entries

                          End Sub)


    End Sub

    'Private Sub updateDefineGrid()
    '    Dim dt As New DataTable

    '    dt.Columns.Add("Variable")
    '    dt.Columns.Add("Current Def")
    '    dt.Columns.Add("New Def")

    '    For Each kvp As KeyValuePair(Of String, String) In mMainViewModel.getProj.Panel.Ppcl.Variables
    '        dt.Rows.Add(kvp.Key, kvp.Value, kvp.Value)
    '    Next


    '    'defineGrid.DataContext = data.DefaultView

    '    Dispatcher.Invoke(Sub()
    '                          defineGrid.ItemsSource = dt.AsDataView

    '                          defineGrid.Columns(0).IsReadOnly = True
    '                          defineGrid.Columns(1).IsReadOnly = True
    '                          defineGrid.Columns(2).IsReadOnly = False
    '                      End Sub)
    'End Sub

End Class
