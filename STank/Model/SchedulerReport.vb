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

    Private mSchedules As Dictionary(Of String, Tuple(Of String, String))


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
            NotifyPropertyChanged("ScheduleId")
        End Set
    End Property


    Public Property Schedules As Dictionary(Of String, Tuple(Of String, String))
        Get
            Return mSchedules
        End Get

        Set(value As Dictionary(Of String, Tuple(Of String, String)))
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
        mSchedules = New Dictionary(Of String, Tuple(Of String, String))

        Dim lines() As String = File.ReadAllLines(mPath)

        For i As Integer = 0 To lines.Length - 1

            Dim line As String = lines(i)

            Dim isMatch As Boolean = False
            Dim day As String = ""
            For Each weekday As String In Weekdays
                If line.StartsWith(weekday) Then
                    isMatch = True
                    day = weekday
                    Exit For
                End If
            Next
            If Not isMatch Then Continue For

            i += 1  'next line contains schedule data


            Dim scheduleData As String = lines(i)
            Dim vals() As String = Regex.Split(scheduleData, "\s+")

            Dim status As String = vals(2)
            Dim startTime As String = vals(3)
            Dim endTime As String = vals(4)

            If status = "Enabled" Then
                mSchedules.Add(day, New Tuple(Of String, String)(startTime, endTime))
            End If

        Next


        'Return replacementValues
    End Sub




End Class
