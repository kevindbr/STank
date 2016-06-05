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
        serialPortList.ItemsSource = mMainViewModel.getPanels()
        workingDirectory.DataContext = mMainViewModel.getProj()
        nameChangeDocument.DataContext = mMainViewModel.getProj()
        panelAttributesDocument.DataContext = mMainViewModel.getProj()

        bw.WorkerReportsProgress = True
        bw.WorkerSupportsCancellation = True
        AddHandler bw.DoWork, AddressOf bw_RunFindAndReplace
        'AddHandler bw.ProgressChanged, AddressOf bw_ProgressChanged
        'AddHandler bw.RunWorkerCompleted, AddressOf bw_RunWorkerCompleted


    End Sub

    Private Sub showConnectionView(sender As Object, e As RoutedEventArgs)
        Dim connectionView As New ConnectionView(mMainViewModel)
        connectionView.Show()
    End Sub

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

    Private Sub findAndReplaceClicked(sender As Object, e As RoutedEventArgs)

        bw.RunWorkerAsync()     'Run find and replace on background thread.  Shouldn't need this if we are using a modal window instead



        'Dim panel As Panel = mMainViewModel.getPanels().Item(0)     'for now there's only one panel
        'Dim program = New Program(panel.Port.RetrieveProgram)
        'program.changeNames(panel.NameChangeDocument.getReplacementValues)


        'panel.Port.ReplaceProgram(program.NewLines)


        'Dim timeStamp As String = DateTime.Now.ToString("MMddyyyyhhmmss")
        'Dim cwd As String = mMainViewModel.getProj().Directory.Path

        'System.IO.File.WriteAllText(cwd + System.IO.Path.DirectorySeparatorChar + "program_old_" + timeStamp + ".pcl", program.Text)
        'System.IO.File.WriteAllText(cwd + System.IO.Path.DirectorySeparatorChar + "program_new_" + timeStamp + ".pcl", program.NewText)


        'Return

        'Dim folderDialog = New FolderBrowserDialog()
        'folderDialog.SelectedPath = "C:\"

        'Dim result = folderDialog.ShowDialog()
        'If (result.ToString() = "OK") Then
        '    mMainViewModel.getProj().Directory.Path = folderDialog.SelectedPath
        'End If
    End Sub



    Private Sub bw_RunFindAndReplace(ByVal sender As Object, ByVal e As DoWorkEventArgs)
        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)

        Dim replacementValues As Dictionary(Of String, String) = mMainViewModel.getProj.Panel.NameChangeDocument.getReplacementValues()

        Dim i As Integer = 1

        For Each kvp As KeyValuePair(Of String, String) In mMainViewModel.getProj.Panel.NameChangeDocument.getReplacementValues()

            Dim oldName As String = kvp.Key
            Dim newName As String = kvp.Value
            Dim sysName As String = newName
            Dim cmd As String = "ChangeSysName2 " + oldName + " " + newName + " " + sysName     'don't use actual name yet

            Dim process = New System.Diagnostics.Process()
            Dim startInfo = New System.Diagnostics.ProcessStartInfo
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
            startInfo.FileName = "cmd.exe"
            startInfo.Arguments = "/C " + cmd
            startInfo.RedirectStandardOutput = True
            startInfo.RedirectStandardError = True
            startInfo.UseShellExecute = False

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



End Class
