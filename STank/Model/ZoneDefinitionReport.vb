Imports System.ComponentModel
Imports System.Data
Imports System.Data.OleDb
Imports System.IO
Imports System.Text.RegularExpressions

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
        For i As Integer = 0 To lines.Length - 1
            Dim line As String = lines(i)
            Dim name As String
            Dim val As String

            'Handle duplicate "Mode" if both start and night optimization enabled
            Dim nameSuffix As String = ""
            If line.Contains("Parameter") Then
                Dim modeType As String = Regex.Split(line.Trim, "\s+")(0)    'assuming first one isn't empty
                nameSuffix = " (" + modeType + ")"
                i += 1
                line = lines(i)
            End If

            If Not line.Contains(":") Then Continue For
            Dim vals() As String = line.Split(":"c)
            'check if vals().length >= 2?  Or is it guaranteed due to above condition?
            name = vals(0).Trim()
            val = vals(1).Trim()

            If (name <> "" And val <> "") Then
                Try
                    If Not (mZoneData.Keys.Contains(name + nameSuffix)) Then
                        mZoneData.Add(name + nameSuffix, val)
                    End If

                Catch
                    MsgBox("Please check zone definition report")
                End Try
            End If
        Next

        For Each line As String In lines

        Next line
        'Return replacementValues
    End Sub


    Private Sub getNewTemperaturePointNames()
        'this does require that name change has already happened....

        Dim outsideTemp As String = mPanel.NameChangeDocument.ReplacementValues(mZoneData("Outside Temperature"))
        Dim zoneTemp As String = mPanel.NameChangeDocument.ReplacementValues(mZoneData("Zone Temperature"))

    End Sub


End Class
