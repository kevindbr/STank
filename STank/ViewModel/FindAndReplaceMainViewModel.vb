Imports System.Collections.ObjectModel


Public Class FindAndReplaceMainViewModel
    Inherits BaseMainViewModel

    Private portNameDefault = "No Active Comm Ports"

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

        Dim error1 = "Please Click 'Edit Variables' Under Project Details to Edit PPCL Define Statements"
        Dim sameVarible = True

        ''error1 condition is true if ppcl variable are the same, this means the user may not have come to change them yet
        For Each kvp As KeyValuePair(Of String, String) In mSTankProj.Panel.Ppcl.Variables
            For Each kvp2 As KeyValuePair(Of String, String) In mSTankProj.Panel.Ppcl.NewVariables
                If kvp.Key.Equals(kvp2.Key) Then
                    If Not kvp.Value.Equals(kvp2.Value) Then
                        sameVarible = False
                    End If
                End If
            Next
        Next

        If sameVarible Then
            allErrors.Add(error1)
        End If

        Return allErrors
    End Function

    ''' <summary>
    ''' Notifications that will hinder the user from being able to use the find and replace
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function getActivityErrorLogs() As List(Of String)

        Dim allErrors As List(Of String) = New List(Of String)
        Dim error1 = "No PPCL document provided"
        Dim error2 = "No Name Change Document Path Specified"

        If Not My.Computer.FileSystem.FileExists(mSTankProj.Panel.Ppcl.Paths.First) Then
            allErrors.Add(error1)
        End If

        If Not My.Computer.FileSystem.FileExists(mSTankProj.Panel.NameChangeDocument.Path) Then
            allErrors.Add(error2)
        End If

        'If mSTankProj.Panel.Ppcl.NewVariables.Count > 0 Then
        '    allErrors.Add(error3)
        'End If

        Return allErrors
    End Function

    Sub updateStatus()
        Throw New NotImplementedException
    End Sub


End Class
