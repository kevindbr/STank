Imports System.ComponentModel
Imports System.Data
Imports System.Windows.Automation
Imports System.Windows.Automation.Provider
Imports System.Windows.Automation.Peers

Public Class FindAndReplaceView

    Private mMainViewModel As MainViewModel
    Private bw As BackgroundWorker = New BackgroundWorker
    Private Property logFName = "FindAndReplace"

    ''' <summary>
    ''' Bring in mainViewModel to update and change project data
    ''' </summary>
    ''' <param name="mainViewModel"></param>
    ''' <remarks></remarks>
    Sub New(ByRef mainViewModel As MainViewModel)
        mMainViewModel = mainViewModel
        InitializeComponent()
        doneButton.IsEnabled = False
    End Sub

    Private Sub IntializeWindow()
        ' updateDefineGrid()

        BaseMainViewModel.InitUI(Windows.Threading.Dispatcher.CurrentDispatcher, log, progressBar, logFName, mMainViewModel.getProj.LogPath)

        bw.WorkerReportsProgress = True
        bw.WorkerSupportsCancellation = True
        AddHandler bw.DoWork, AddressOf bw_RunFindAndReplace
        AddHandler bw.RunWorkerCompleted, AddressOf showDone
        bw.RunWorkerAsync()



    End Sub

    Private Sub bw_RunFindAndReplace(ByVal sender As Object, ByVal e As DoWorkEventArgs)

        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)
        Dim nameChangeDoc = mMainViewModel.getProj.Panel.NameChangeDocument
        Dim ppcl As Ppcl = mMainViewModel.getProj.Panel.Ppcl

        ppcl.findAndReplaceInFile2(nameChangeDoc.ReplacementValues)
        nameChangeDoc.PerformNameChange()

    End Sub


    Private Sub exitView(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub

    Private Sub showDone(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs)
        Dim message = "Find and Replace Finished!  A copy of the original PPCL has been created with _new appended to the name this file contains the replaced names."

        If e.Cancelled = True Then
            message = "Operation cancelled."

        ElseIf e.Error IsNot Nothing Then
            message = "Error. " + e.Error.Message

        Else

        End If

        Dim messageWindow As GeneralPopupView = New GeneralPopupView(message)
        doneButton.Content = "Done"
        doneButton.IsEnabled = True
        messageWindow.Show()
        BaseMainViewModel.WriteFile()
        BaseMainViewModel.ResetUI()
    End Sub

End Class
