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

Class EnhancedAlarmsMainView

    Public mMainViewModel As MainViewModel
    Public mEnhancedAlarmsMainViewModel As EnhancedAlarmsMainViewModel

    Private bw As BackgroundWorker = New BackgroundWorker


    ''' <summary>
    ''' Bring in mainViewModel to update and change project data
    ''' </summary>
    ''' <param name="mainViewModel"></param>
    ''' <remarks></remarks>
    Sub New(ByRef mainViewModel As MainViewModel)
        mMainViewModel = mainViewModel
        mEnhancedAlarmsMainViewModel = New EnhancedAlarmsMainViewModel(mMainViewModel.getProj)
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

        schedulerReport.DataContext = mMainViewModel.getProj()

        AddHandler mMainViewModel.getProj.Panel.PanelAttributesDocument.PropertyChanged, AddressOf updateMainWindow     'do this in main view code?

        updateMainWindow()
    End Sub

    Private Sub showConnectionView(sender As Object, e As RoutedEventArgs)
        Dim connectionView As New ConnectionView(mMainViewModel)
        connectionView.Show()
    End Sub

    'Private Sub showDefineView(sender As Object, e As RoutedEventArgs)
    '    Dim defineView As New DefineView(mMainViewModel)
    '    defineView.Show()
    'End Sub

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
            mMainViewModel.getProj().Panel.PanelAttributesDocument.Path = dlg.FileName
        End If
    End Sub

    Private Sub replaceClicked(sender As Object, e As RoutedEventArgs)

        Dim fnrView As New EnhancedAlarmsProgressView(mMainViewModel)
        fnrView.Show()


    End Sub




    Private Sub updateMainWindow(Optional sender As Object = Nothing, Optional e As PropertyChangedEventArgs = Nothing)     'hacky
        mEnhancedAlarmsMainViewModel.updateAllLogs(activityLog)
        updateButtons()
    End Sub


    Private Sub updateButtons()

        Dim listOfErrors As List(Of String) = mEnhancedAlarmsMainViewModel.getActivityErrorLogs()
        Dim listOfWarnings As List(Of String) = mEnhancedAlarmsMainViewModel.getActivityWarningLogs()

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
