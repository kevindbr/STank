Imports System.ComponentModel
Imports System.Data
Imports System.Data.OleDb
Imports System.IO
Imports System.Text.RegularExpressions

Public Class SchedulerReport
    Implements INotifyPropertyChanged

    Private mPanel As Panel
    Private mPath As String
    Public Shared EmptyPath As String = "No Scheduler Report Specified"

    Private mScheduleId As String
    Private mZoneName As String
    Private mZoneNames As List(Of String)
    Private mSchedules As List(Of KeyValuePair(Of String, Tuple(Of String, String, String)))


    'Private Const mEngineeringUnitsSpreadsheet = "BACnet_unit_conversion_spreadsheet.xlsx"
    'Private mEngineeringUnits As Dictionary(Of String, String)

    Public Event PropertyChanged As PropertyChangedEventHandler _
  Implements INotifyPropertyChanged.PropertyChanged

    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub

    Public Property Path As String
        Get
            Return mPath
        End Get

        Set(value As String)
            mPath = value

            If isValidDocument(mPath) Then
                getSchedules()
            End If

            NotifyPropertyChanged("Path")
        End Set
    End Property


    'not really the proper place to store this...how to separate between document stuff and comm stuff?

    Public Property ScheduleId As String
        Get
            Return mScheduleId
        End Get

        Set(value As String)
            mScheduleId = value
            'NotifyPropertyChanged("ScheduleId")
        End Set
    End Property


    Public Property ZoneName As String
        Get
            Return mZoneName
        End Get

        Set(value As String)
            mZoneName = value
            'NotifyPropertyChanged("ScheduleId")
        End Set
    End Property



    Public Property Schedules As List(Of KeyValuePair(Of String, Tuple(Of String, String, String)))
        Get
            Return mSchedules
        End Get

        Set(value As List(Of KeyValuePair(Of String, Tuple(Of String, String, String))))
            mSchedules = value
            NotifyPropertyChanged("Schedules")
        End Set
    End Property


    ''' <summary>
    ''' Check file extension here
    ''' </summary>
    ''' <param name="mPath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function isValidDocument(mPath As String) As Boolean
        Dim isValidFile = True

        If mPath = EmptyPath Then
            isValidFile = False
        End If

        Return isValidFile
    End Function


    Private Shared Weekdays() As String = {"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"}


    Public Sub getSchedules()
        mSchedules = New List(Of KeyValuePair(Of String, Tuple(Of String, String, String)))
        mZoneName = ""
        mZoneNames = New List(Of String)
        Dim currentDay = ""
        Dim lines() As String = File.ReadAllLines(mPath)

        For i As Integer = 0 To lines.Length - 1
            Dim line As String = lines(i)

            'Snag all unique zone names
            If line.StartsWith("Zone") Then
                Dim zoneVals() As String = Regex.Split(line, "\s+")
                mZoneName = zoneVals(1)

                If Not (mZoneNames.Contains(mZoneName)) Then
                    mZoneNames.Add(mZoneName)
                End If
            End If

            If Not (currentDay = "") Then
                If line.StartsWith("Zone") Then
                    Dim scheduleData As String = lines(i)
                    Dim vals() As String = Regex.Split(scheduleData, "\s+")

                    Dim status As String = vals(2)
                    Dim startTime As String = vals(3)
                    Dim endTime As String = vals(4)

                    If status = "Enabled" Then
                        ' mSchedules.Add(New KeyValuePair( currentDay, New Tuple(Of String, String, String)(mZoneName, startTime, endTime))))
                        Dim SEZ As New Tuple(Of String, String, String)(mZoneName, startTime, endTime)
                        mSchedules.Add(New KeyValuePair(Of String, Tuple(Of String, String, String))(currentDay, SEZ))
                    End If
                End If
            End If

            Dim day As String = ""
            For Each weekday As String In Weekdays
                If line.StartsWith(weekday) Then
                    day = weekday
                    currentDay = weekday

                    Exit For
                End If
            Next

        Next
    End Sub



    'Public Sub getSchedules()
    '    mSchedules = New Dictionary(Of String, Tuple(Of String, String))
    '    mSchedulesAndZones = New Dictionary(Of String, Dictionary(Of String, Tuple(Of String, String)))
    '    mZoneName = ""
    '    Dim lines() As String = File.ReadAllLines(mPath)

    '    For i As Integer = 0 To lines.Length - 1
    '        Dim line As String = lines(i)

    '        If line.StartsWith("Zone") Then
    '            'Populate list of zones For every new selection section and populate last one on the way out
    '            If Not (mZoneName = "") Then
    '                mSchedulesAndZones.Add(mZoneName, mSchedules)
    '            End If

    '            mZoneName = line.Split(":")(1).Trim
    '            mZoneNames.Add(line.Split(":")(1).Trim)
    '            Continue For
    '        End If

    '        Dim isMatch As Boolean = False
    '        Dim day As String = ""
    '        For Each weekday As String In Weekdays
    '            If line.StartsWith(weekday) Then
    '                isMatch = True
    '                day = weekday
    '                Exit For
    '            End If

    '            If Not isMatch Then Continue For
    '            i += 1  'next line contains schedule data
    '            Dim scheduleData As String = lines(i)
    '            Dim vals() As String = Regex.Split(scheduleData, "\s+")

    '            Dim status As String = vals(2)
    '            Dim startTime As String = vals(3)
    '            Dim endTime As String = vals(4)

    '            If status = "Enabled" Then
    '                mSchedules.Add(day, New Tuple(Of String, String)(startTime, endTime))
    '            End If
    '        Next

    '        'Add last set of zones and schedules
    '        If (i = lines.Length - 1) Then
    '            mSchedulesAndZones.Add(mZoneName, mSchedules)
    '        End If
    '    Next


    '    'Return replacementValues
    'End Sub




End Class
