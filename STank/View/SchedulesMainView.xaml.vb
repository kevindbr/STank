﻿' To access MetroWindow, add the following reference
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

Class SchedulesMainView

    Public mMainViewModel As MainViewModel
    Public mSchedulesMainViewModel As SchedulesMainViewModel
    Public runClicked As Boolean
    Private bw As BackgroundWorker = New BackgroundWorker


    ''' <summary>
    ''' Bring in mainViewModel to update and change project data
    ''' </summary>
    ''' <param name="mainViewModel"></param>
    ''' <remarks></remarks>
    Sub New(ByRef mainViewModel As MainViewModel)
        mMainViewModel = mainViewModel
        mSchedulesMainViewModel = New SchedulesMainViewModel(mMainViewModel.getProj)
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

        schedulerReport.DataContext = mMainViewModel.getProj()

        AddHandler mMainViewModel.getProj.Panel.SchedulerReport.PropertyChanged, AddressOf updateMainWindow     'do this in main view code?
        AddHandler mMainViewModel.getProj.Panel.Port.PropertyChanged, AddressOf updateMainWindow
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
        dlg.DefaultExt = ".txt" ' Default file extension
        dlg.Filter = "txt documents (.txt)|*.txt" ' Filter files by extension

        ' Set filter for file extension and default file extension
        ' Display OpenFileDialog by calling ShowDialog method
        Dim result = dlg.ShowDialog()

        ' Get the selected file name and display in a TextBox
        If (result = True) Then
            mMainViewModel.getProj().Panel.SchedulerReport.Path = dlg.FileName
        End If
    End Sub

    Private Sub replaceClicked(sender As Object, e As RoutedEventArgs)

        Dim fnrView As New SchedulesProgressView(mMainViewModel)
        fnrView.Show()
        runClicked = True
        updateMainWindow()


    End Sub

    Private Sub updateAllLogs()
        activityLog.Text = ""

        Dim listOfErrors As List(Of String) = mSchedulesMainViewModel.getActivityErrorLogs()
        Dim listOfWarnings As List(Of String) = mSchedulesMainViewModel.getActivityWarningLogs()

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
        Dim maxNumOfErrors As Integer = mSchedulesMainViewModel.getMaxNumOfErrors()

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

            Dim newLine As Run = New Run(" All Steps Complete.  Please click configure schedules.")
            activityLog.Inlines.Add(newLine)
            activityLog.Inlines.Add(New LineBreak)
        End If

        If Not runClicked Then
            If Not mMainViewModel.getProj().SchedulesStatus.Equals("complete") Then
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

        mSchedulesMainViewModel.setStatus(5, status)
    End Sub


    Private Sub updateMainWindow(Optional sender As Object = Nothing, Optional e As PropertyChangedEventArgs = Nothing)     'hacky
        'mSchedulesMainViewModel.updateAllLogs(activityLog)
        updateButtons()
        updateAllLogs()

    End Sub



    Private Sub updateButtons()

        Dim listOfErrors As List(Of String) = mSchedulesMainViewModel.getActivityErrorLogs()
        Dim listOfWarnings As List(Of String) = mSchedulesMainViewModel.getActivityWarningLogs()

        If listOfErrors.Count = 0 Then
            replaceButton.IsEnabled = True
        Else
            replaceButton.IsEnabled = False
        End If

        If Not mMainViewModel.getProj().Panel.Port.PortName.Equals("No Active Comm Ports") And mMainViewModel.getProj.Panel.Port.LoginValid Then

            Dim bi3 As New BitmapImage
            bi3.BeginInit()
            bi3.UriSource = New Uri("../Images/Siemens_Icons/plug.png", UriKind.Relative)
            bi3.EndInit()
            connectionImage.Source = bi3
            connectionImage.ToolTip = "Panel Connected"
        Else

            Dim bi3 As New BitmapImage
            bi3.BeginInit()
            bi3.UriSource = New Uri("../Images/Siemens_Icons/unplug.png", UriKind.Relative)
            bi3.EndInit()
            connectionImage.Source = bi3
            connectionImage.ToolTip = "Panel Disconnected"
        End If



    End Sub

    Private Sub exitView(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub


End Class
