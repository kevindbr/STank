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

Class MainWindow

    Public mMainViewModel As MainViewModel

    Private bw As BackgroundWorker = New BackgroundWorker


    ''' <summary>
    ''' This gets called once the window has loaded
    ''' At first, the user has two options, load and existing project or begin a new project
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub IntializeMainWindow()
        mMainViewModel = New MainViewModel()
        mMainViewModel.IntializeProject()
        bw.WorkerReportsProgress = True
        bw.WorkerSupportsCancellation = True
        LogDirectory.DataContext = mMainViewModel.getProj()
        ' AddHandler bw.DoWork, AddressOf bw_RunFindAndReplace
		
        'AddHandler bw.ProgressChanged, AddressOf bw_ProgressChanged
        'AddHandler bw.RunWorkerCompleted, AddressOf bw_RunWorkerCompleted

        ' AddHandler mMainViewModel.getProj.Panel.NameChangeDocument.PropertyChanged, AddressOf updateDefineGrid
        mMainViewModel.getProj.Panel.NameChangeDocument.Path = mMainViewModel.getProj.Panel.NameChangeDocument.Path
        'Gets event to trigger now that handler is in place

        AddHandler mMainViewModel.getProj.Panel.Ppcl.PropertyChanged, AddressOf updateMainWindow
        AddHandler mMainViewModel.getProj.Panel.NameChangeDocument.PropertyChanged, AddressOf updateMainWindow
        AddHandler mMainViewModel.getProj.Panel.PanelAttributesDocument.PropertyChanged, AddressOf updateMainWindow
        AddHandler mMainViewModel.getProj.Panel.Port.PropertyChanged, AddressOf updateMainWindow
        AddHandler mMainViewModel.getProj.PropertyChanged, AddressOf updateMainWindow
        updateMainWindow()
    End Sub

    Private Sub showConnectionView(sender As Object, e As RoutedEventArgs)
        'mMainViewModel.getProj.Panel.InitializePaths()
        Dim connectionView As New ConnectionView(mMainViewModel)
        connectionView.Show()
    End Sub

    'Private Sub showDefineView(sender As Object, e As RoutedEventArgs)
    '    Dim defineView As New DefineView(mMainViewModel)
    '    defineView.Show()
    'End Sub

    Private Sub LoadData_Click_1(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub browseProgramClicked(sender As Object, e As RoutedEventArgs)
        'Dim folderDialog = New FolderBrowserDialog()
        'folderDialog.SelectedPath = "C:\"

        'Dim result = folderDialog.ShowDialog()
        'If (result.ToString() = "OK") Then
        '    mMainViewModel.getProj().Directory.Path = folderDialog.SelectedPath
        'End If

        'Create OpenFileDialog
        Dim dlg = New Microsoft.Win32.OpenFileDialog()

        ' Set filter for file extension and default file extension
        ' Display OpenFileDialog by calling ShowDialog method
        Dim result = dlg.ShowDialog()

        ' Get the selected file name and display in a TextBox
        If (result = True) Then
            mMainViewModel.getProj().Panel.Ppcl.Path = dlg.FileName
        End If

    End Sub



    Private Sub browseClicked(sender As Object, e As RoutedEventArgs)
        'Create OpenFileDialog
        Dim dlg = New System.Windows.Forms.FolderBrowserDialog()


        ' Set filter for file extension and default file extension
        ' Display OpenFileDialog by calling ShowDialog method
        Dim result = dlg.ShowDialog()

        ' Get the selected file name and display in a TextBox
        If (result = True) Then
            mMainViewModel.getProj().LogPath = dlg.SelectedPath
        End If
    End Sub


    Private Sub browseNameClicked(sender As Object, e As RoutedEventArgs)
        'Create OpenFileDialog
        Dim dlg = New Microsoft.Win32.OpenFileDialog()

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

    Private Sub updateMainWindow()
        updateAllLogs()
        updateButtons()
    End Sub

    Private Sub updateAllLogs()
        activityLog.Items.Clear()

        Dim listOfIncomplete As List(Of String) = mMainViewModel.getIncompleteSteps()
        Dim listOfPartial As List(Of String) = mMainViewModel.getPartialSteps()
        Dim listOfComplete As List(Of String) = mMainViewModel.getCompleteSteps()

        For Each notification As String In listOfIncomplete
            Dim newItem As TextBlock = New TextBlock()

            Dim noticeImage As Image = New Image()
            noticeImage.Width = 20
            noticeImage.Height = 20

            Dim bi3 As New BitmapImage
            bi3.BeginInit()
            bi3.UriSource = New Uri("Resources/Notice.png", UriKind.Relative)
            bi3.EndInit()
            noticeImage.Stretch = Stretch.Fill
            noticeImage.Source = bi3

            newItem.Inlines.Add(New LineBreak)
            Dim container As InlineUIContainer = New InlineUIContainer(noticeImage)
            newItem.Inlines.Add(container)

            Dim dependencyMessage = mMainViewModel.getDependency(notification)

            If Not dependencyMessage.Equals("none") Then
                newItem.IsEnabled = False
                notification = notification + " (Dependency : " + dependencyMessage + " )"

            End If

            Dim newLine As Run = New Run(" " + notification)
            newLine.Foreground = Brushes.Red
            newItem.Inlines.Add(newLine)



            activityLog.Items.Add(newItem)
        Next

        For Each notification As String In listOfPartial
            Dim newItem As TextBlock = New TextBlock()
            Dim noticeImage As Image = New Image()
            noticeImage.Width = 20
            noticeImage.Height = 20

            Dim bi3 As New BitmapImage
            bi3.BeginInit()
            bi3.UriSource = New Uri("Resources/Warning.png", UriKind.Relative)
            bi3.EndInit()
            noticeImage.Stretch = Stretch.Fill
            noticeImage.Source = bi3


            newItem.Inlines.Add(New LineBreak)
            Dim container As InlineUIContainer = New InlineUIContainer(noticeImage)
            newItem.Inlines.Add(container)

            Dim newLine As Run = New Run(" " + notification)
            newLine.Foreground = Brushes.DarkOrange
            newItem.Inlines.Add(newLine)
            activityLog.Items.Add(newItem)
        Next

        For Each notification As String In listOfComplete
            Dim newItem As TextBlock = New TextBlock()
            Dim noticeImage As Image = New Image()
            noticeImage.Width = 20
            noticeImage.Height = 20

            Dim bi3 As New BitmapImage
            bi3.BeginInit()
            bi3.UriSource = New Uri("Resources/Complete.png", UriKind.Relative)
            bi3.EndInit()
            noticeImage.Stretch = Stretch.Fill
            noticeImage.Source = bi3

            newItem.Inlines.Add(New LineBreak)
            Dim container As InlineUIContainer = New InlineUIContainer(noticeImage)
            newItem.Inlines.Add(container)

            Dim newLine As Run = New Run(" " + notification)

            newItem.Inlines.Add(newLine)

            activityLog.Items.Add(newItem)
        Next

    End Sub

    Private Sub updateButtons()

        If Not mMainViewModel.getProj().Panel.Port.PortName.Equals("No Active Comm Ports") And mMainViewModel.getProj.Panel.Port.LoginValid Then

            Dim bi3 As New BitmapImage
            bi3.BeginInit()
            bi3.UriSource = New Uri("Resources/plug.png", UriKind.Relative)
            bi3.EndInit()
            connectionImage.Source = bi3
            connectionImage.ToolTip = "Panel Connected"
        Else

            Dim bi3 As New BitmapImage
            bi3.BeginInit()
            bi3.UriSource = New Uri("Resources/unplug.png", UriKind.Relative)
            bi3.EndInit()
            connectionImage.Source = bi3
            connectionImage.ToolTip = "Panel Disconnected"
        End If





    End Sub

    Private Sub nextStepClicked(sender As Object, e As RoutedEventArgs)

        Try
            Dim nextStep As Integer = mMainViewModel.getNextStep()

            If nextStep = 1 Then
                Dim nextView As FindAndReplaceMainView = New FindAndReplaceMainView(mMainViewModel)
                nextView.Show()
            End If

            If nextStep = 2 Then
                Dim nextView As EngineeringUnitsMainView = New EngineeringUnitsMainView(mMainViewModel)
                nextView.Show()
            End If

            If nextStep = 3 Then
                Dim nextView As StateTextMainView = New StateTextMainView(mMainViewModel)
                nextView.Show()
            End If

            If nextStep = 4 Then
                Dim nextView As EnhancedAlarmsMainView = New EnhancedAlarmsMainView(mMainViewModel)
                nextView.Show()
            End If

            If nextStep = 6 Then
                Dim nextView As SSTOMainView = New SSTOMainView(mMainViewModel)
                nextView.Show()
            End If

            If nextStep = 5 Then
                Dim nextView As SchedulesMainView = New SchedulesMainView(mMainViewModel)
                nextView.Show()
            End If

            If nextStep = 0 Then
                Dim nextView As MessageView = New MessageView()
                nextView.Show()
            End If
        Catch ex As Exception

        End Try



    End Sub

    Private Sub stepClicked(sender As Object, e As RoutedEventArgs)

        'Return


        Try

            Dim selectedItem = activityLog.SelectedValue.Inlines.LastInline.Text
            Dim showMessage = False

            Dim dependencyMessage = mMainViewModel.getDependency(selectedItem)

            If dependencyMessage.Equals("none") Then
                showMessage = True

            End If

            Dim nextStep As Integer = mMainViewModel.getNumericalEquvialent(selectedItem)

            If nextStep = 1 Then
                Dim nextView As FindAndReplaceMainView = New FindAndReplaceMainView(mMainViewModel)
                nextView.Show()
            End If

            If nextStep = 2 Then
                Dim nextView As EngineeringUnitsMainView = New EngineeringUnitsMainView(mMainViewModel)
                nextView.Show()
            End If

            If nextStep = 3 Then

                If showMessage Then
                    Dim nextView As StateTextMainView = New StateTextMainView(mMainViewModel)
                    nextView.Show()
                End If
                If Not showMessage Then
                    Dim message As GeneralPopupView = New GeneralPopupView("Please complete all dependencies before proceeding to this step")
                    message.Show()
                End If

            End If

            If nextStep = 4 Then

                If showMessage Then
                    Dim nextView As EnhancedAlarmsMainView = New EnhancedAlarmsMainView(mMainViewModel)
                    nextView.Show()
                End If

                If Not showMessage Then
                    Dim message As GeneralPopupView = New GeneralPopupView("Please complete all dependencies before proceeding to this step")
                    message.Show()
                End If
            End If


            If nextStep = 5 Then
                If showMessage Then
                    Dim nextView As SchedulesMainView = New SchedulesMainView(mMainViewModel)
                    nextView.Show()
                End If

                If Not showMessage Then
                    Dim message As GeneralPopupView = New GeneralPopupView("Please complete all dependencies before proceeding to this step")
                    message.Show()
                End If
            End If


            If nextStep = 6 Then
                Dim nextView As SSTOMainView = New SSTOMainView(mMainViewModel)
                nextView.Show()
            End If



            If nextStep = 0 Then
                Dim nextView As MessageView = New MessageView()
                nextView.Show()
            End If

        Catch ex As Exception

        End Try


    End Sub


    Private Sub connectionClicked(sender As Object, e As RoutedEventArgs)

        'Return






    End Sub

End Class
