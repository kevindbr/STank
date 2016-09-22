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
    'Zone, list of zone data pertaining to that zone
    Private mZoneData As Dictionary(Of String, Dictionary(Of String, String))

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


    Public Property ZoneData As Dictionary(Of String, Dictionary(Of String, String))
        Get
            Return mZoneData
        End Get

        Set(value As Dictionary(Of String, Dictionary(Of String, String)))
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
        mZoneData = New Dictionary(Of String, Dictionary(Of String, String))
        Dim currentListOfZoneData = New Dictionary(Of String, String)
        Dim currentZoneName = ""


        Dim lines() As String = File.ReadAllLines(mPath)
        For i As Integer = 0 To lines.Length - 1


            Dim line As String = lines(i)
            Dim name As String
            Dim val As String


            If line.Contains("Zone Name:") Then
                If (currentListOfZoneData.Count > 0) Then
                    If Not (currentZoneName = "") Then
                        mZoneData.Add(currentZoneName, currentListOfZoneData)
                    End If
                End If

                currentZoneName = line.Split(":"c)(1).Trim()
                currentListOfZoneData = New Dictionary(Of String, String)
            End If

            'Handle duplicate "Mode" if both start and night optimization enabled
            Dim nameSuffix As String = ""
            If line.Contains("Parameter") Then
                Dim modeType As String = Regex.Split(line.Trim, "\s+")(0)    'assuming first one isn't empty
                nameSuffix = " (" + modeType + ")"
                i += 1
                line = lines(i)
            End If


            If Not line.Contains(":") Then Continue For
            If Not currentZoneName = "" Then


                Dim vals() As String = line.Split(":"c)
                'check if vals().length >= 2?  Or is it guaranteed due to above condition?
                name = vals(0).Trim()
                val = vals(1).Trim()

                If (name <> "" And val <> "") Then
                    Try
                        ' If Not (mZoneData..Contains(name + nameSuffix)) Then
                        'mZoneData.Add(name + nameSuffix, val)

                        If Not (currentListOfZoneData.Keys.Contains(name + nameSuffix)) Then
                            currentListOfZoneData.Add(name + nameSuffix, val)
                        End If
                    Catch
                        MsgBox("Please check zone definition report")
                    End Try
                End If
            End If
        Next


        mZoneData.Add(currentZoneName, currentListOfZoneData) 'Last list of zone data
        For Each line As String In lines

        Next line
        'Return replacementValues
    End Sub


    Private Sub getNewTemperaturePointNames(zoneName As String)
        'this does require that name change has already happened....

        For Each listOfZoneData In mZoneData
            If (listOfZoneData.Key = zoneName) Then
                Dim outsideTemp As String = mPanel.NameChangeDocument.ReplacementValues(listOfZoneData.Value("Outside Temperature"))
                Dim zoneTemp As String = mPanel.NameChangeDocument.ReplacementValues(listOfZoneData.Value("Zone Temperature"))
                Exit For
            End If
        Next
    End Sub


End Class
