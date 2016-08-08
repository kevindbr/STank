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
        doneButton.IsEnabled = False
    End Sub

    Private Sub IntializeWindow()
        ' updateDefineGrid()

        BaseMainViewModel.InitUI(Windows.Threading.Dispatcher.CurrentDispatcher, log, progressBar)

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

        BaseMainViewModel.ResetUI()

    End Sub


    Private Sub exitView(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub

    Private Sub showDone()
        Dim message As GeneralPopupView = New GeneralPopupView("Find and Replace Finished!  A copy of the original PPCL has been created with _new appended to the name this file contains the replaced names.")
        doneButton.Content = "Done"
        doneButton.IsEnabled = True
        message.Show()
    End Sub


End Class
