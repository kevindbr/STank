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
    Private Property logFName = "StateText"

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
        BaseMainViewModel.InitUI(Dispatcher.CurrentDispatcher, log, progressBar, logFName, mMainViewModel.getProj.LogPath)
        bw.WorkerReportsProgress = True
        bw.WorkerSupportsCancellation = True
        AddHandler bw.DoWork, AddressOf bw_RunFindAndReplace
        AddHandler bw.RunWorkerCompleted, AddressOf showDone
        bw.RunWorkerAsync()
    End Sub



    Private Sub bw_RunFindAndReplace(ByVal sender As Object, ByVal e As DoWorkEventArgs)

        Dim panel = mMainViewModel.getProj.Panel
        Dim port = panel.Port
        Dim panelAttributesDoc = panel.PanelAttributesDocument
        Dim stateTextDoc = panel.StateTextDocument

        port.Login()

        Dim modifiedStateTextTableIds = New List(Of String)
        Dim newStateTextTables = New List(Of StateTextDoc.StateTextTable)()


        Dim i As Integer = 1
        Dim pointNames As New List(Of String)(panelAttributesDoc.LenumPoints.Keys)
        For Each pointName As String In pointNames
            'For Each kvp As KeyValuePair(Of String, String) In panelAttributesDoc.LenumPoints

            'Dim pointName As String = kvp.Key
            Dim stateTextTableId As String = panelAttributesDoc.LenumPoints(pointName)

            BaseMainViewModel.WriteLog(String.Format("Re-creating LENUM point '{0}'", pointName))
            BaseMainViewModel.UpdateProgress(0.25 * i / panelAttributesDoc.LenumPoints.Count)

            Dim instanceNumber As String = port.GetPointInstanceNumber(pointName)
            If Not instanceNumber = "" Then port.DeletePoint(pointName)

            Dim stateTextTable As StateTextDoc.StateTextTable = stateTextDoc.GetStateTextByID(stateTextTableId)

            Dim newStateTextTableId As String = stateTextTable.tableId.TrimStart("-")

            If stateTextTable.tableId = newStateTextTableId Then 'This is not a negative table id, must find next available ID
                newStateTextTableId = getNextStateTextId(panelAttributesDoc.LenumPoints, stateTextTable, newStateTextTables) 'Also if we find that another lenum point has same state text, use that id
            End If

            stateTextTable.tableId = newStateTextTableId



            panelAttributesDoc.LenumPoints(pointName) = newStateTextTableId      'change reference from old table to new table



            stateTextDoc.StateTextTables.Remove(stateTextTableId)
            stateTextDoc.StateTextTables.Add(newStateTextTableId, stateTextTable)

            'For now, we are simply taking the existing state text table ID with the minus sign stripped off (all built-in text tables have negative
            'numbers, while positives are reserved for custom state text).  More robust would be some code here to look in the panel

            'Don't want to do this more than once per panel - all mode points will likely use same state text
            If Not modifiedStateTextTableIds.Contains(stateTextTable.tableId) Then
                BaseMainViewModel.WriteLog(String.Format("Creating state text table '{0}'", stateTextTable.tableId))
                port.CreateStateTextTable(stateTextTable)
                modifiedStateTextTableIds.Add(stateTextTable.tableId)
            End If

            BaseMainViewModel.UpdateProgress(0.75 * i / panelAttributesDoc.LenumPoints.Count)

            port.CreatePoint(pointName, instanceNumber, stateTextTable.tableId)
            newStateTextTables.Add(stateTextTable)


            BaseMainViewModel.UpdateProgress(1 * i / panelAttributesDoc.LenumPoints.Count)
            'System.Threading.Thread.Sleep(100) 

            i += 1
        Next

        port.Logout()

        'should already have lenum points from when panel attributes doc path was set

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

    Private Sub showDone(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs)
        Dim message = "State text tables have been updated! Please refer to state text log file for panel output."

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

    Private Sub exitView(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub

    Private Function getNextStateTextId(lPoints As Dictionary(Of String, String), stateTextTable As StateTextDoc.StateTextTable, newStateTextTables As List(Of StateTextDoc.StateTextTable)) As String
        Dim newId As Int32 = 0

        For Each pointName In lPoints
            Dim tempNum As Int32 = Int32.Parse(pointName.Value)
            If newId < Math.Abs(tempNum) Then
                newId = tempNum
            End If
        Next

        newId = newId + 1

        For Each sText In newStateTextTables 'Checks if we created a new id and uses that id if state text is the same

            If stateTextTable.Equals(sText) Then
                newId = sText.tableId
            End If

        Next

        Return newId

    End Function



End Class
