Imports System.Collections.ObjectModel


Public Class MainViewModel

    Private mSTankProj As STankProj
    Private portNameDefault = "No Active Comm Ports"


    ''' <summary>
    ''' Create new project with at least one panel
    ''' </summary>
    ''' <remarks></remarks>
    Sub IntializeProject()
        mSTankProj = New STankProj()
        mSTankProj.Name = "New Project"
        mSTankProj.Panels = New List(Of Panel)
        'mSTankProj.Directory = New WorkingDirectory()
        'mSTankProj.Directory.IntializeData()

        Dim panel = New Panel()
        panel.InitializeData()

        panel.Name = "New Panel"
        panel.Database = New PanelDatabase()
        panel.Database.IntializeData()
        panel.Port = New CommPort()
        panel.Port.IntializeData()
        panel.Port.PortName = portNameDefault

        mSTankProj.Panels.Add(panel)

        mSTankProj.Panel = panel


        'DimnameChangeDoc = New NameChangeDoc()
        'mNameChangeDoc.Path = "C:\"

        'mPanelAttributesDoc = New PanelAttributesDoc()
        'mPanelAttributesDoc.Path = "C:\"
        'mPath = "C:\"




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

        Dim error1 = "No Panel Attributes Document Specified"
        Dim error2 = "No Active Comm Ports"

        If Not My.Computer.FileSystem.FileExists(mSTankProj.Panel.PanelAttributesDocument.Path) Then
            allErrors.Add(error1)
        End If

        If mSTankProj.Panel.Port.PortName = portNameDefault Then
            allErrors.Add(error2)
        End If

        Return allErrors
    End Function

    ''' <summary>
    ''' Notifications that will hinder the user from being able to use the find and replace
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getActivityErrorLogs() As List(Of String)

        Dim allErrors As List(Of String) = New List(Of String)
        Dim error1 = "No PPCL document provided"
        Dim error2 = "No Name Change Document Path Specified"

        If Not My.Computer.FileSystem.FileExists(mSTankProj.Panel.Ppcl.Path) Then
            allErrors.Add(error1)
        End If

        If Not My.Computer.FileSystem.FileExists(mSTankProj.Panel.NameChangeDocument.Path) Then
            allErrors.Add(error2)
        End If

        Return allErrors
    End Function


End Class
