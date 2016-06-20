Imports System.ComponentModel
Imports System.Data
Imports System.Data.OleDb
Imports System.IO

Public Class StateTextDoc
    Implements INotifyPropertyChanged

    Private mPanel As Panel
    Private mPath As String     'holds new path.  Whenever path set, make a copy of the old doc
    Private Const sNewDocSuffix = "_new"

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

            If Not isValidDocument(value) Then Return

            Dim sNewPath = NewPath(value)
            File.Copy(value, sNewPath, True)
            mPath = sNewPath


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

        If mPath = "No State Text Document Specified" Then
            isValidFile = False
        End If

        Return isValidFile
    End Function


    Private Function NewPath(ByVal sOriginalPath As String) As String
        'make a copy of the original document and perform all change operations on that copy

        Return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(sOriginalPath),
                                              System.IO.Path.GetFileNameWithoutExtension(sOriginalPath) + sNewDocSuffix + System.IO.Path.GetExtension(sOriginalPath))
    End Function


    Private Sub readExcelIntoDataTable()
        Dim mPanelAttributesData As New DataTable

        Dim strConnString As String
        strConnString = "Driver={Microsoft Excel Driver (*.xls, *.xlsx, *.xlsm, *.xlsb)};DBQ=" & mPath & ";"

        Dim sheetName = "Book1"
        Dim strSQL As String
        strSQL = "SELECT * FROM [" & sheetName & "$]"

        Dim y As New Odbc.OdbcDataAdapter(strSQL, strConnString)

        y.Fill(mPanelAttributesData)

    End Sub





End Class
