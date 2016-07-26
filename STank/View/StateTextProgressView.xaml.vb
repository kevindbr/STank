﻿Imports System.ComponentModel
Imports System.Data
Imports System.Windows.Automation
Imports System.Windows.Automation.Provider
Imports System.Windows.Automation.Peers
Imports System.Windows.Threading
Imports System.Data.OleDb

Public Class StateTextProgressView

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
        BaseMainViewModel.InitUI(Windows.Threading.Dispatcher.CurrentDispatcher, log, progressBar)
        bw.WorkerReportsProgress = True
        bw.WorkerSupportsCancellation = True
        AddHandler bw.DoWork, AddressOf bw_RunFindAndReplace
        bw.RunWorkerAsync()
    End Sub



    Private Sub bw_RunFindAndReplace(ByVal sender As Object, ByVal e As DoWorkEventArgs)

        Dim panel = mMainViewModel.getProj.Panel
        Dim port = mMainViewModel.getProj.Panel.Port
        Dim panelAttributesDoc = mMainViewModel.getProj.Panel.PanelAttributesDocument


        Dim modifiedStateTextTableIds = New List(Of String)

        Dim i As Integer = 1
        For Each kvp As KeyValuePair(Of String, String) In panelAttributesDoc.LenumPoints

            Dim pointName As String = kvp.Key
            Dim stateTextTableId As String = kvp.Value

            BaseMainViewModel.WriteLog(String.Format("Re-creating LENUM point '{0}'", pointName))
            BaseMainViewModel.UpdateProgress(0.25 * i / panelAttributesDoc.LenumPoints.Count)

            Dim instanceNumber As String = port.GetPointInstanceNumber(pointName)
            If Not instanceNumber = "" Then port.DeletePoint(pointName)

            Dim stateTextTable As StateTextDoc.StateTextTable = panel.StateTextDocument.GetStateTextByID(stateTextTableId)

            'Don't want to do this more than once per panel - all mode points will likely use same state text
            If Not modifiedStateTextTableIds.Contains(stateTextTable.tableId) Then
                BaseMainViewModel.WriteLog(String.Format("Creating state text table '{0}'", stateTextTable.tableId))
                port.CreateStateTextTable(stateTextTable)
                modifiedStateTextTableIds.Add(stateTextTable.tableId)
            End If

            BaseMainViewModel.UpdateProgress(0.75 * i / panelAttributesDoc.LenumPoints.Count)

            port.CreatePoint(pointName, instanceNumber, stateTextTable.tableId)

            BaseMainViewModel.UpdateProgress(1 * i / panelAttributesDoc.LenumPoints.Count)
            'System.Threading.Thread.Sleep(100) 

            i += 1
        Next

        'should already have lenum points from when panel attributes doc path was set

        BaseMainViewModel.ResetUI()

    End Sub





    'Private Sub bw_RunFindAndReplace2(ByVal sender As Object, ByVal e As DoWorkEventArgs)

    '    Dim stateTextDoc = mMainViewModel.getProj.Panel.StateTextDocument

    '    stateTextDoc.OpenConnection()


    '    For value As Integer = 0 To 16

    '        Dispatcher.Invoke(Sub(ByRef i As Integer)
    '                              progressBar.Value = i / 17 * 100
    '                          End Sub, value + 1)

    '        stateTextDoc.ModifyField(value)

    '        AddToLog(String.Format("Modifying field 'Value{0}'", value))

    '        System.Threading.Thread.Sleep(100)  'just for debugging, to more easily see progress

    '    Next

    '    stateTextDoc.Connection.Close()

    'End Sub



    Private Sub exitView(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub


End Class
