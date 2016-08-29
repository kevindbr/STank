' To access MetroWindow, add the following reference
Imports System
Imports MahApps.Metro.Controls
Imports System.IO.Ports
Imports STank.ConnectionView
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Windows.Automation.Peers
Imports System.Windows.Automation.Provider
Imports System.Windows.Automation
Imports System.Data
Imports System.Collections.ObjectModel

Class FindAndReplaceMainView

    Public mMainViewModel As MainViewModel
    Public mFindAndReplaceMainViewModel As FindAndReplaceMainViewModel
    Public runClicked As Boolean

    Private bw As BackgroundWorker = New BackgroundWorker


    ''' <summary>
    ''' Bring in mainViewModel to update and change project data
    ''' </summary>
    ''' <param name="mainViewModel"></param>
    ''' <remarks></remarks>
    Sub New(ByRef mainViewModel As MainViewModel)
        mMainViewModel = mainViewModel
        mFindAndReplaceMainViewModel = New FindAndReplaceMainViewModel(mMainViewModel.getProj)
        runClicked = False
        InitializeComponent()

    End Sub

    ''' <summary>
    ''' This gets called once the window has loaded
    ''' At first, the user has two options, load and existing project or begin a new project
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub IntializeMainWindow()
        workingDirectory.DataContext = mFindAndReplaceMainViewModel.getProj()
        nameChangeDocument.DataContext = mFindAndReplaceMainViewModel.getProj()
        bw.WorkerReportsProgress = True
        bw.WorkerSupportsCancellation = True

        AddHandler mFindAndReplaceMainViewModel.getProj.Panel.Ppcl.PropertyChanged, AddressOf updateMainWindow
        AddHandler mFindAndReplaceMainViewModel.getProj.Panel.NameChangeDocument.PropertyChanged, AddressOf updateMainWindow

        updateMainWindow()
    End Sub

    Private Sub showConnectionView(sender As Object, e As RoutedEventArgs)
        Dim connectionView As New ConnectionView(mMainViewModel)
        connectionView.Show()
    End Sub

    Private Sub showDefineView(sender As Object, e As RoutedEventArgs)
        Dim defineView As New DefineView(mMainViewModel, Me)
        defineView.Show()
    End Sub

    Private Sub LoadData_Click_1(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub browseProgramClicked(sender As Object, e As RoutedEventArgs)

        'Create OpenFileDialog
        Dim dlg = New Microsoft.Win32.OpenFileDialog()
        dlg.DefaultExt = ".pcl" ' Default file extension
        dlg.Filter = "pcl documents (.pcl)|*.pcl" ' Filter files by extension
        dlg.Multiselect = True
        ' Set filter for file extension and default file extension
        ' Display OpenFileDialog by calling ShowDialog method
        Dim result = dlg.ShowDialog()

        Dim allPaths = New List(Of String)

        ' Get the selected file name and display in a TextBox
        If (result = True) Then
            For Each file In dlg.FileNames
                allPaths.Add(file)
            Next

            mMainViewModel.getProj().Panel.Ppcl.Paths = allPaths
        End If
    End Sub


    Private Sub browseNameClicked(sender As Object, e As RoutedEventArgs)
        'Create OpenFileDialog
        Dim dlg = New Microsoft.Win32.OpenFileDialog()
        dlg.DefaultExt = ".csv" ' Default file extension
        dlg.Filter = "csv documents (.csv)|*.csv" ' Filter files by extension

        ' Set filter for file extension and default file extension
        ' Display OpenFileDialog by calling ShowDialog method
        Dim result = dlg.ShowDialog()

        ' Get the selected file name and display in a TextBox
        If (result = True) Then
            mMainViewModel.getProj().Panel.NameChangeDocument.Path = dlg.FileName
        End If
    End Sub

    Private Sub browseAttributeClicked(sender As Object, e As RoutedEventArgs)
        'Create OpenFileDialog
        Dim dlg = New Microsoft.Win32.OpenFileDialog()

        ' Set filter for file extension and default file extension
        ' Display OpenFileDialog by calling ShowDialog method
        Dim result = dlg.ShowDialog()

        ' Get the selected file name and display in a TextBox
        If (result = True) Then
            mMainViewModel.getProj().Panel.PanelAttributesDocument.Path = dlg.FileName
        End If
    End Sub

    Private Sub findAndReplaceClicked(sender As Object, e As RoutedEventArgs)

        Dim fnrView As New FindAndReplaceView(mMainViewModel)
        fnrView.Show()
        runClicked = True
        updateMainWindow()

    End Sub









    'Private Sub updateDefineGrid(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs)

    '    If (e.PropertyName <> "Path") Then Return


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


    'Private Sub bw_RunFindAndReplace(ByVal sender As Object, ByVal e As DoWorkEventArgs)

    '    Dim fnrView As New FindAndReplaceView(mMainViewModel)
    '    fnrView.Show()

    '    Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)
    '    Dim replacementValues As Dictionary(Of String, String) = mMainViewModel.getProj.Panel.NameChangeDocument.ReplacementValues
    '    Dim ppcl As Ppcl = mMainViewModel.getProj.Panel.Ppcl
    '    Dim newDefinitions As Collection = New Collection()
    '    For Each row As DataRowView In defineGrid.ItemsSource
    '        newDefinitions.Add(row.Item(2))
    '    Next

    '    ppcl.findAndReplaceInFile(replacementValues, newDefinitions)    'TODO: logging

    '    Dim i As Integer = 1

    '    For Each kvp As KeyValuePair(Of String, String) In replacementValues

    '        Dim oldName As String = kvp.Key
    '        Dim newName As String = kvp.Value
    '        Dim sysName As String = newName
    '        Dim cmd As String = "ChangeSystemName2 " + oldName + " " + newName + " " + sysName
    '        cmd = "ChangeSystemName " 'shouldn't run without arguments

    '        Dim process = New System.Diagnostics.Process()
    '        Dim startInfo = New System.Diagnostics.ProcessStartInfo
    '        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
    '        startInfo.FileName = "cmd.exe"
    '        startInfo.Arguments = "/C " + cmd
    '        startInfo.RedirectStandardOutput = True
    '        startInfo.RedirectStandardError = True
    '        startInfo.UseShellExecute = False

    '        startInfo.CreateNoWindow = True

    '        process.StartInfo = startInfo
    '        process.Start()

    '        AddToLog("Changing name '" + oldName + "' to '" + newName + "'")

    '        process.WaitForExit()

    '        AddToLog(process.StandardOutput.ReadToEnd)
    '        AddToLog(process.StandardError.ReadToEnd)

    '        System.Threading.Thread.Sleep(100)  'just for debugging, to more easily see progress

    '        Dispatcher.Invoke(Sub()
    '                              progressBar.Value = i / replacementValues.Count * 100
    '                          End Sub)

    '        i = i + 1

    '    Next
    'End Sub




    'Private Sub AddToLog(ByVal line As String)

    '    If line = "" Then Return 'when stderr is non-empty, stdout will be

    '    'Need this because this code doesn't run on UI thread but needs to modify UI elements
    '    Dispatcher.Invoke(Sub()

    '                          log.Items.Add(line)

    '                          If log.Items.Count > 10 Then
    '                              log.Items.RemoveAt(0)
    '                          End If

    '                          'log.SelectedIndex = log.Items.Count - 1
    '                          'log.SelectedIndex = -1

    '                          'Scroll to last entry
    '                          Dim svAutomation As ListBoxAutomationPeer = ScrollViewerAutomationPeer.CreatePeerForElement(log)
    '                          Dim scrollInterface As IScrollProvider = svAutomation.GetPattern(PatternInterface.Scroll)
    '                          Dim scrollVertical As ScrollAmount = ScrollAmount.LargeIncrement
    '                          Dim scrollHorizontal As ScrollAmount = ScrollAmount.NoAmount
    '                          If scrollInterface.VerticallyScrollable Then
    '                              scrollInterface.Scroll(scrollHorizontal, scrollVertical)
    '                          End If

    '                          'og.ScrollIntoView(log.Items.Item(log.Items.Count - 1))   'doesn't work for duplicate entries

    '                      End Sub)
    'End Sub

    Public Sub updateMainWindow()
        updateAllLogs()
        updateButtons()
        'mMainViewModel.
    End Sub

    Private Sub updateAllLogs()
        activityLog.Text = ""

        Dim listOfErrors As List(Of String) = mFindAndReplaceMainViewModel.getActivityErrorLogs()
        Dim listOfWarnings As List(Of String) = mFindAndReplaceMainViewModel.getActivityWarningLogs()

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
        Dim maxNumOfErrors As Integer = mFindAndReplaceMainViewModel.getMaxNumOfErrors()

        If (numberOfErrors = 0) Then
             Dim noticeImage As Image = New Image()
            noticeImage.Width = 20
            noticeImage.Height = 20

            Dim bi3 As New BitmapImage
            bi3.BeginInit()
            bi3.UriSource = New Uri("Resources/Complete.png", UriKind.Relative)
            bi3.EndInit()
            noticeImage.Stretch = Stretch.Fill
            noticeImage.Source = bi3

            Dim container As InlineUIContainer = New InlineUIContainer(noticeImage)
            activityLog.Inlines.Add(container)

            Dim newLine As Run = New Run(" All Steps Complete.  Please click run find and replace.")
            activityLog.Inlines.Add(newLine)
            activityLog.Inlines.Add(New LineBreak)
        End If

        If Not runClicked Then
            If Not mMainViewModel.getProj().NameChangeStatus.Equals("complete") Then
                numberOfErrors += 1
            End If
        End If

        'For now, if the user clicks run, then we set status to complete, later we need to actually check if run was completed without errors
        If runClicked Then
            numberOfErrors = 0
        End If

        Dim status = "incomplete"

        If ((maxNumOfErrors - numberOfErrors) = maxNumOfErrors) Then
            status = "complete"
        End If

        If ((maxNumOfErrors - numberOfErrors) > 0 And (maxNumOfErrors - numberOfErrors) < maxNumOfErrors) Then
            status = "partial"
        End If

        mFindAndReplaceMainViewModel.setStatus(1, status)

    End Sub

    Private Sub updateButtons()

        Dim listOfErrors As List(Of String) = mFindAndReplaceMainViewModel.getActivityErrorLogs()
        Dim listOfWarnings As List(Of String) = mFindAndReplaceMainViewModel.getActivityWarningLogs()

        If listOfErrors.Count = 0 Then
            runFnRButton.IsEnabled = True
        Else
            runFnRButton.IsEnabled = False
        End If

    End Sub


    Private Sub exitView(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub


End Class
