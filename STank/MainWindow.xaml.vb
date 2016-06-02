' To access MetroWindow, add the following reference
Imports System
Imports MahApps.Metro.Controls
Imports System.IO.Ports
Imports STank.ConnectionView
Imports System.Windows.Forms

Class MainWindow

    Public mMainViewModel As MainViewModel






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
    End Sub

    Private Sub showConnectionView(sender As Object, e As RoutedEventArgs)
        Dim connectionView As New ConnectionView(mMainViewModel)
        connectionView.Show()
    End Sub

    Private Sub LoadData_Click_1(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub browseDirectoryClicked(sender As Object, e As RoutedEventArgs)
        Dim folderDialog = New FolderBrowserDialog()
        folderDialog.SelectedPath = "C:\"

        Dim result = folderDialog.ShowDialog()
        If (result.ToString() = "OK") Then
            mMainViewModel.getProj().Directory.Path = folderDialog.SelectedPath
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
            mMainViewModel.getProj().Directory.NameChangeDocument.Path = dlg.FileName
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
            mMainViewModel.getProj().Directory.PanelAttributesDocument.Path = dlg.FileName
        End If
    End Sub

    Private Sub findAndReplaceClicked(sender As Object, e As RoutedEventArgs)

        'Dim commPort = serialPortList.SelectedItem      'shouldn't this be retrieved from panel object instead?

        ' Mouse.OverrideCursor = Cursors.



        'Cursor



        Dim panel As Panel = mMainViewModel.getPanels().Item(0)     'for now there's only one panel
        Dim program = New Program(panel.Port.RetrieveProgram)
        program.changeNames(mMainViewModel.getProj().Directory.NameChangeDocument.getReplacementValues)


        panel.Port.ReplaceProgram(program.NewLines)

        Dim timeStamp As String = DateTime.Now.ToString("MMddyyyyhhmmss")
        Dim cwd As String = mMainViewModel.getProj().Directory.Path

        System.IO.File.WriteAllText(cwd + System.IO.Path.DirectorySeparatorChar + "program_old_" + timeStamp + ".pcl", program.Text)
        System.IO.File.WriteAllText(cwd + System.IO.Path.DirectorySeparatorChar + "program_new_" + timeStamp + ".pcl", program.NewText)


        Return

        Dim folderDialog = New FolderBrowserDialog()
        folderDialog.SelectedPath = "C:\"

        Dim result = folderDialog.ShowDialog()
        If (result.ToString() = "OK") Then
            mMainViewModel.getProj().Directory.Path = folderDialog.SelectedPath
        End If
    End Sub



End Class
