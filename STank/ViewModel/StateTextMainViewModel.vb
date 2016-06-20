Imports System.Collections.ObjectModel


Public Class StateTextMainViewModel

    Private mSTankProj As STankProj
    'Private portNameDefault = "No Active Comm Ports"


    Public Sub New(ByVal sTankProj As STankProj)

        mSTankProj = sTankProj

    End Sub


    ''' <summary>
    ''' We want to allow the user to have more than one comm port connection stored
    ''' We will need to add logic to cleanly handle panel to comm port logic
    ''' </summary>
    ''' <param name="commPort"></param>
    ''' <remarks></remarks>
    Sub addNewPort(commPort As CommPort)
        If mSTankProj.Panels.Count = 1 Then
            mSTankProj.Panels.ElementAt(0).Port = commPort
        End If
    End Sub

    Function getPanels() As List(Of Panel)
        Return mSTankProj.Panels
    End Function

    Function getProj() As STankProj
        Return mSTankProj
    End Function

    ''' <summary>
    ''' Warnings are notifications that won't effect the user's ability to perform a find and replace
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getActivityWarningLogs() As List(Of String)

        Dim allErrors As List(Of String) = New List(Of String)

        Return allErrors

        'Dim error1 = "No Panel Attributes Document Specified"
        'Dim error2 = "No Active Comm Ports"


        'If Not My.Computer.FileSystem.FileExists(mSTankProj.Panel.PanelAttributesDocument.Path) Then
        '    allErrors.Add(error1)
        'End If

        'If mSTankProj.Panel.Port.PortName = portNameDefault Then
        '    allErrors.Add(error2)
        'End If




    End Function

    ''' <summary>
    ''' Notifications that will hinder the user from being able to use the find and replace
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getActivityErrorLogs() As List(Of String)

        Dim allErrors As List(Of String) = New List(Of String)
        Dim error1 = "No State Text document provided"
        'Dim error1 = "No PPCL document provided"
        'Dim error2 = "No Name Change Document Path Specified"
        'Dim error3 = "Set Define Statements for PPCL"

        If Not My.Computer.FileSystem.FileExists(mSTankProj.Panel.StateTextDocument.Path) Then
            allErrors.Add(error1)
        End If

        'If Not My.Computer.FileSystem.FileExists(mSTankProj.Panel.NameChangeDocument.Path) Then
        '    allErrors.Add(error2)
        'End If

        'If mSTankProj.Panel.Ppcl.NewVariables.Count > 0 Then
        '    allErrors.Add(error3)
        'End If

        Return allErrors
    End Function


End Class
