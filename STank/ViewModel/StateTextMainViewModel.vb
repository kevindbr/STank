Imports System.Collections.ObjectModel


Public Class StateTextMainViewModel
    Inherits BaseMainViewModel


    Public Sub New(ByVal sTankProj As STankProj)
        MyBase.New(sTankProj)
    End Sub



    ''' <summary>
    ''' Warnings are notifications that won't effect the user's ability to perform a find and replace
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function getActivityWarningLogs() As List(Of String)

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
    Public Overrides Function getActivityErrorLogs() As List(Of String)

        Dim allErrors As List(Of String) = New List(Of String)
        Dim error1 = "No State Text Document Provided"
        Dim error2 = "No Panel Connection Detected"


        If Not My.Computer.FileSystem.FileExists(mSTankProj.Panel.StateTextDocument.Path) Then
            allErrors.Add(error1)
        End If

        If mSTankProj.Panel.Port.PortName.Equals("No Active Comm Ports") Or Not mSTankProj.Panel.Port.LoginValid Then
            allErrors.Add(error2)
        End If

        Return allErrors
    End Function


End Class
