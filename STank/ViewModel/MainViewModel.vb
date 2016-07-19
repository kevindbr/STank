Imports System.Collections.ObjectModel


Public Class MainViewModel

    Private mSTankProj As STankProj
    Private portNameDefault = "No Active Comm Ports"
    Private activity1 = "System Name Change"
    Private activity2 = "Convert Engineering Units to Bacnet"
    Private activity3 = "Update State Text Tables"
    Private activity4 = "Replace Enhanced Alarms"
    Private activity6 = "Convert Create Start Stop Time Optimization Zones to Bacnet"
    Private activity5 = "Convert Schedules to Bacnet"


    ''' <summary>
    ''' Create new project with at least one panel
    ''' </summary>
    ''' <remarks></remarks>
    Sub IntializeProject()
        mSTankProj = New STankProj()
        mSTankProj.InitializeData()
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
        Dim error3 = "Set Define Statements for PPCL"

        If Not My.Computer.FileSystem.FileExists(mSTankProj.Panel.Ppcl.Path) Then
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


    ''' <summary>
    ''' Notifications that will hinder the user from being able to use the find and replace
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getCompleteSteps() As List(Of String)

        Dim allActivities As List(Of String) = New List(Of String)

        If mSTankProj.NameChangeStatus = "complete" Then
            allActivities.Add(activity1)
        End If

        If mSTankProj.EngineeringUnitsStatus = "complete" Then
            allActivities.Add(activity2)
        End If

        If mSTankProj.StateTextStatus = "complete" Then
            allActivities.Add(activity3)
        End If

        If mSTankProj.EnhancedAlarmsStatus = "complete" Then
            allActivities.Add(activity4)
        End If

        If mSTankProj.StartStopStatus = "complete" Then
            allActivities.Add(activity5)
        End If

        If mSTankProj.SchedulesStatus = "complete" Then
            allActivities.Add(activity6)
        End If

        Return allActivities
    End Function


    ''' <summary>
    ''' Notifications that will hinder the user from being able to use the find and replace
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getPartialSteps() As List(Of String)
        Dim allActivities As List(Of String) = New List(Of String)

        If mSTankProj.NameChangeStatus = "partial" Then
            allActivities.Add(activity1)
        End If

        If mSTankProj.EngineeringUnitsStatus = "partial" Then
            allActivities.Add(activity2)
        End If

        If mSTankProj.StateTextStatus = "partial" Then
            allActivities.Add(activity3)
        End If

        If mSTankProj.EnhancedAlarmsStatus = "partial" Then
            allActivities.Add(activity4)
        End If

        If mSTankProj.StartStopStatus = "partial" Then
            allActivities.Add(activity5)
        End If

        If mSTankProj.SchedulesStatus = "partial" Then
            allActivities.Add(activity6)
        End If

        Return allActivities
    End Function


    ''' <summary>
    ''' Notifications that will hinder the user from being able to use the find and replace
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getIncompleteSteps() As List(Of String)

        Dim allActivities As List(Of String) = New List(Of String)


        If mSTankProj.NameChangeStatus = "incomplete" Then
            allActivities.Add(activity1)
        End If

        If mSTankProj.EngineeringUnitsStatus = "incomplete" Then
            allActivities.Add(activity2)
        End If

        If mSTankProj.StateTextStatus = "incomplete" Then
            allActivities.Add(activity3)
        End If

        If mSTankProj.EnhancedAlarmsStatus = "incomplete" Then
            allActivities.Add(activity4)
        End If

        If mSTankProj.StartStopStatus = "incomplete" Then
            allActivities.Add(activity5)
        End If

        If mSTankProj.SchedulesStatus = "incomplete" Then
            allActivities.Add(activity6)
        End If

        Return allActivities
    End Function

    Function getNextStep() As Integer

        Dim numericalStepEquvialent As Integer = 0
        Dim allActivities As List(Of String) = New List(Of String)
        allActivities.AddRange(getIncompleteSteps)
        allActivities.AddRange(getPartialSteps())

        If (allActivities.Count > 0) Then
            numericalStepEquvialent = getNumericalEquvialent(allActivities.First())
        End If



        Return numericalStepEquvialent
    End Function

    Public Function getNumericalEquvialent(activity As String) As Integer

        Dim activityNum As Integer = 0

        If activity.Contains(activity1) Then
            activityNum = 1
        End If

        If activity.Contains(activity2) Then
            activityNum = 2
        End If

        If activity.Contains(activity3) Then
            activityNum = 3
        End If

        If activity.Contains(activity4) Then
            activityNum = 4
        End If

        If activity.Contains(activity5) Then
            activityNum = 5
        End If

        If activity.Contains(activity6) Then
            activityNum = 6
        End If

        Return activityNum
    End Function


End Class
