Imports System.ComponentModel
Imports System.Data
Imports System.Windows.Automation
Imports System.Windows.Automation.Provider
Imports System.Windows.Automation.Peers
Imports System.Windows.Threading
Imports System.Data.OleDb
Imports System.Text.RegularExpressions

Public Class EnhancedAlarmsProgressView

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

        Dim panel = mMainViewModel.getProj.Panel
        Dim alarmsData = panel.PanelAttributesDocument.AlarmsData

        panel.Port.Login()

        For Each row As DataRow In alarmsData.Rows

            panel.Port.CreateEnhancedAlarms(row)

            Dispatcher.Invoke(Sub()
                                  Dim textView As New EnhancedAlarmsTextView(row)
                                  textView.Show()
                              End Sub)

        Next

        panel.Port.Logout()


        BaseMainViewModel.ResetUI()

    End Sub



    Private Sub showTextView(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub showDone()
        Dim message As GeneralPopupView = New GeneralPopupView("Enhanced alarm replacement finished!  Please refer to enhanced alarm log file for panel output.")
        doneButton.Content = "Done"
        doneButton.IsEnabled = True
        message.Show()
    End Sub

    Private Sub exitView(sender As Object, e As RoutedEventArgs)
        Close()
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
