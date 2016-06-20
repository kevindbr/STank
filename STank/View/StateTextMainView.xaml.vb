' To access MetroWindow, add the following reference
Imports System
Imports MahApps.Metro.Controls
Imports System.IO.Ports
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Windows.Automation.Peers
Imports System.Windows.Automation.Provider
Imports System.Windows.Automation
Imports System.Data
Imports System.Collections.ObjectModel

Class StateTextMainView

    Public mMainViewModel As MainViewModel
    Public mStateTextMainViewModel As StateTextMainViewModel

    Private bw As BackgroundWorker = New BackgroundWorker


    ''' <summary>
    ''' Bring in mainViewModel to update and change project data
    ''' </summary>
    ''' <param name="mainViewModel"></param>
    ''' <remarks></remarks>
    Sub New(ByRef mainViewModel As MainViewModel)
        mMainViewModel = mainViewModel
        mStateTextMainViewModel = New StateTextMainViewModel(mMainViewModel.getProj)
        '
        InitializeComponent()

    End Sub

    ''' <summary>
    ''' This gets called once the window has loaded
    ''' At first, the user has two options, load and existing project or begin a new project
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub IntializeMainWindow()

        stateTextDoc.DataContext = mMainViewModel.getProj()

        AddHandler mMainViewModel.getProj.Panel.StateTextDocument.PropertyChanged, AddressOf updateMainWindow     'do this in main view code?

        updateMainWindow()
    End Sub

    'Private Sub showConnectionView(sender As Object, e As RoutedEventArgs)
    '    Dim connectionView As New ConnectionView(mMainViewModel)
    '    connectionView.Show()
    'End Sub

    Private Sub showDefineView(sender As Object, e As RoutedEventArgs)
        Dim defineView As New DefineView(mMainViewModel)
        defineView.Show()
    End Sub

    Private Sub LoadData_Click_1(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub panelAttributeDetails(sender As Object, e As RoutedEventArgs)

    End Sub


    Private Sub browseAttributesClicked(sender As Object, e As RoutedEventArgs)
        'Create OpenFileDialog
        Dim dlg = New Microsoft.Win32.OpenFileDialog()

        ' Set filter for file extension and default file extension
        ' Display OpenFileDialog by calling ShowDialog method
        Dim result = dlg.ShowDialog()

        ' Get the selected file name and display in a TextBox
        If (result = True) Then
            mMainViewModel.getProj().Panel.StateTextDocument.Path = dlg.FileName
        End If
    End Sub

    Private Sub replaceClicked(sender As Object, e As RoutedEventArgs)

        Dim fnrView As New StateTextProgressView(mMainViewModel)
        fnrView.Show()

        'bw.RunWorkerAsync()     'Run find and replace on background thread.  Shouldn't need this if we are using a modal window instead


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



    Private Sub updateMainWindow()
        updateAllLogs()
        updateButtons()
    End Sub

    Private Sub updateAllLogs()
        activityLog.Text = ""

        Dim listOfErrors As List(Of String) = mStateTextMainViewModel.getActivityErrorLogs()
        Dim listOfWarnings As List(Of String) = mStateTextMainViewModel.getActivityWarningLogs()

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

    End Sub

    Private Sub updateButtons()

        Dim listOfErrors As List(Of String) = mStateTextMainViewModel.getActivityErrorLogs()
        Dim listOfWarnings As List(Of String) = mStateTextMainViewModel.getActivityWarningLogs()

        If listOfErrors.Count = 0 Then
            'runFnRButton.IsEnabled = True
        Else
            'runFnRButton.IsEnabled = False
        End If

    End Sub

    Private Sub exitView(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub



End Class
