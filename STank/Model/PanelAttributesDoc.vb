Imports System.ComponentModel
Imports System.Data
Imports System.Data.OleDb
Imports System.IO

Public Class PanelAttributesDoc
    Implements INotifyPropertyChanged

    Private mPanel As Panel
    Private mPath As String     'holds new path.  Whenever path set, make a copy of the old doc
    Private Const sNewDocSuffix = "_new"

    Private Const mEngineeringUnitsSpreadsheet = "BACnet_unit_conversion_spreadsheet.xlsx"
    Private mEngineeringUnits As Dictionary(Of String, String)

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


    Public Property EngineeringUnits As Dictionary(Of String, String)
        Get
            getEngineeringUnits()

            Return mEngineeringUnits
        End Get

        Set(value As Dictionary(Of String, String))

            mEngineeringUnits = value

            NotifyPropertyChanged("EngineeringUnits")
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

        If mPath = "No Panel Attributes Document Specified" Then
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



    Private Sub getEngineeringUnits()

        mEngineeringUnits = New Dictionary(Of String, String)

        'sConnection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + mPath + ";Extended Properties=""Excel 12.0;HDR=No;IMEX=1"""
        'Dim sConnection = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='{0}';Extended Properties=""Excel 12.0;HDR=Yes;""", System.IO.Path.Combine(Directory.GetCurrentDirectory, mEngineeringUnitsSpreadsheet))
        'can we deploy this without the ACE OLEDB provider being installed on target machine?

        Dim sConnection = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='{0}';Extended Properties=""Excel 12.0;HDR=No;""", System.IO.Path.Combine(Directory.GetCurrentDirectory, mEngineeringUnitsSpreadsheet))


        Dim oleExcelConnection = New OleDbConnection(sConnection)
        oleExcelConnection.Open()

        Dim t = oleExcelConnection.GetSchema("Tables")


        Dim dtSheets As DataTable =
          oleExcelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)


        Dim sSheetName = oleExcelConnection.GetSchema("Tables").Rows(0)("TABLE_NAME").ToString      'just use name of first sheet
        'Dim sSheetName = "BACnet Unit Conversion Spreadsh"

        Dim oleExcelCommand As OleDbCommand = oleExcelConnection.CreateCommand()
        oleExcelCommand.CommandType = CommandType.Text
        oleExcelCommand.CommandText = String.Format("Select * From [{0}]", sSheetName)      'will this get only non-blank rows?

        Dim oleExcelReader As OleDbDataReader = oleExcelCommand.ExecuteReader
        Dim engineeringUnitsData As New DataTable
        engineeringUnitsData.Load(oleExcelReader)
        For Each row As DataRow In engineeringUnitsData.Rows
            If row.Item(0).ToString.Trim = "" Then Continue For
            Try
                mEngineeringUnits.Add(row.Item(0).ToString, row.Item(1).ToString)
            Catch ex As ArgumentException
            End Try

        Next

        oleExcelReader.Close()
        oleExcelConnection.Close()

    End Sub



    'Public Sub ReplaceEngineeringUnits()

    '    'TODO: copy original panel attributes document?  Or...just copy when first loaded...append "_new"

    '    'mEngineeringUnits = New Dictionary(Of String, String)

    '    'sConnection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + mPath + ";Extended Properties=""Excel 12.0;HDR=No;IMEX=1"""
    '    Dim sConnection = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='{0}';Extended Properties=""Excel 12.0;HDR=Yes;""", mPath)
    '    'can we deploy this without the ACE OLEDB provider being installed on target machine?

    '    Dim oleExcelConnection = New OleDbConnection(sConnection)
    '    oleExcelConnection.Open()

    '    Dim sSheetName = "Points$"  'Panel Attributes document always has same format
    '    Dim oleExcelCommand As OleDbCommand

    '    For Each kvp As KeyValuePair(Of String, String) In mEngineeringUnits
    '        oleExcelCommand = oleExcelConnection.CreateCommand()
    '        oleExcelCommand.CommandType = CommandType.Text
    '        oleExcelCommand.CommandText = String.Format("UPDATE [{0}] SET [Eng units] = '{1}' WHERE [Eng units] = '{2}'", sSheetName, kvp.Value, kvp.Key)
    '        oleExcelCommand.ExecuteNonQuery()
    '    Next

    '    oleExcelConnection.Close()

    'End Sub






End Class
