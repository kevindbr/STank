Imports System.ComponentModel
Imports System.Data
Imports System.Windows.Automation
Imports System.Windows.Automation.Provider
Imports System.Windows.Automation.Peers
Imports System.Windows.Threading
Imports System.Data.OleDb

Public Class EngineeringUnitsProgressView

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

    End Sub

    Private Sub IntializeWindow()
        ' updateDefineGrid()
        bw.WorkerReportsProgress = True
        bw.WorkerSupportsCancellation = True
        AddHandler bw.DoWork, AddressOf bw_RunFindAndReplace
        bw.RunWorkerAsync()
    End Sub

    Private Sub bw_RunFindAndReplace(ByVal sender As Object, ByVal e As DoWorkEventArgs)


        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)


        'TODO: copy original panel attributes document?  Or...just copy when first loaded...append "_new"

        'mEngineeringUnits = New Dictionary(Of String, String)

        'sConnection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + mPath + ";Extended Properties=""Excel 12.0;HDR=No;IMEX=1"""
        Dim sConnection = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='{0}';Extended Properties=""Excel 12.0;HDR=Yes;""", mMainViewModel.getProj.Panel.PanelAttributesDocument.Path)
        'can we deploy this without the ACE OLEDB provider being installed on target machine?

        Dim oleExcelConnection = New OleDbConnection(sConnection)
        oleExcelConnection.Open()

        Dim sSheetName = "Points$"  'Panel Attributes document always has same format
        Dim oleExcelCommand As OleDbCommand

        Dim engineeringUnits = mMainViewModel.getProj.Panel.PanelAttributesDocument.EngineeringUnits
        Dim i As Integer = 1
        For Each kvp As KeyValuePair(Of String, String) In engineeringUnits
            oleExcelCommand = oleExcelConnection.CreateCommand()
            oleExcelCommand.CommandType = CommandType.Text
            oleExcelCommand.CommandText = String.Format("UPDATE [{0}] SET [Eng units] = '{1}' WHERE [Eng units] = '{2}'", sSheetName, kvp.Value, kvp.Key)
            oleExcelCommand.ExecuteNonQuery()

            AddToLog(String.Format("Replacing unit '{0}' with '{1}'", kvp.Key, kvp.Value))

            System.Threading.Thread.Sleep(100)  'just for debugging, to more easily see progress

            Dispatcher.Invoke(Sub()
                                  progressBar.Value = i / engineeringUnits.Count * 100
                              End Sub)

            i = i + 1

        Next

        oleExcelConnection.Close()

    End Sub



    'TODO: put this in public place
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
