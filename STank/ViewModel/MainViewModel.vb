Imports System.Collections.ObjectModel


Public Class MainViewModel

    Private mSTankProj As STankProj


    ''' <summary>
    ''' Create new project with at least one panel
    ''' </summary>
    ''' <remarks></remarks>
    Sub IntializeProject()
        mSTankProj = New STankProj()
        mSTankProj.Name = "New Project"
        mSTankProj.Panels = New List(Of Panel)
        mSTankProj.Directory = New WorkingDirectory()
        mSTankProj.Directory.IntializeData()

        Dim panel = New Panel()
        panel.Name = "New Panel"
        panel.Database = New PanelDatabase()
        panel.Database.IntializeData()
        panel.Port = New CommPort()
        panel.Port.IntializeData()

        mSTankProj.Panels.Add(panel)
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

End Class
