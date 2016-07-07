Imports System.ComponentModel
Imports System.Data
Imports System.Data.OleDb
Imports System.IO

Public Class SchedulerReport
    Implements INotifyPropertyChanged

    Private mPanel As Panel
    Private mPath As String
    Public Shared EmptyPath As String = "No Scheduler Report Specified"

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
            NotifyPropertyChanged("Path")
        End Set
    End Property


    'Public Property EngineeringUnits As Dictionary(Of String, String)
    '    Get
    '        getEngineeringUnits()

    '        Return mEngineeringUnits
    '    End Get

    '    Set(value As Dictionary(Of String, String))

    '        mEngineeringUnits = value

    '        NotifyPropertyChanged("EngineeringUnits")
    '    End Set
    'End Property



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






End Class
