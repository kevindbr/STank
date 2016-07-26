Imports System.ComponentModel
Imports System.Data
Imports System.Data.OleDb
Imports System.IO

Public Class ZoneDefinitionReport
    Implements INotifyPropertyChanged

    Private mPanel As Panel
    Private mPath As String
    Public Shared EmptyPath As String = "No Zone Definition Report Specified"

    Private mZoneData As Dictionary(Of String, String)

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
                getZoneData()
            End If

            NotifyPropertyChanged("Path")
        End Set
    End Property


    Public Property ZoneData As Dictionary(Of String, String)
        Get
            Return mZoneData
        End Get

        Set(value As Dictionary(Of String, String))
            mZoneData = value
            NotifyPropertyChanged("ZoneData")
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



    Public Sub getZoneData()
        mZoneData = New Dictionary(Of String, String)
        Dim lines() As String = File.ReadAllLines(mPath)
        For Each line As String In lines
            If Not line.Contains(":") Then Continue For
            Dim vals() As String = line.Split(":"c)
            'check if vals().length >= 2?  Or is it guaranteed due to above condition?
            Dim name As String = vals(0).Trim()
            Dim value As String = vals(1).Trim()
            If (name <> "" And value <> "") Then
                Try
                    mZoneData.Add(name, value)
                Catch
                    MsgBox("Please check zone definition report")
                End Try
            End If
        Next line
        'Return replacementValues
    End Sub


    Private Sub getNewTemperaturePointNames()
        'this does require that name change has already happened....

        Dim outsideTemp As String = mPanel.NameChangeDocument.ReplacementValues(mZoneData("Outside Temperature"))
        Dim zoneTemp As String = mPanel.NameChangeDocument.ReplacementValues(mZoneData("Zone Temperature"))

    End Sub


End Class
