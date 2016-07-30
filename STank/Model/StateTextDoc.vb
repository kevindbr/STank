Imports System.ComponentModel
Imports System.Data
Imports System.Data.OleDb
Imports System.IO

Public Class StateTextDoc
    Implements INotifyPropertyChanged


    Public Structure StateTextTable
        Public tableName As String
        Public tableId As String
        Public namesList As List(Of String)
        Public valuesList As List(Of String)
    End Structure



    Private mPanel As Panel
    Private mPath As String     'holds new path.  Whenever path set, make a copy of the old doc
    Public Shared EmptyPath As String = "No State Text Document Specified"
    Private Const sNewDocSuffix = "_new"

    Private mConnection As OleDbConnection

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


    Public Property Connection As OleDbConnection
        Get
            Return mConnection
        End Get

        Set(value As OleDbConnection)
            mConnection = value
            NotifyPropertyChanged("Connection")
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



    Private Function NewPath(ByVal sOriginalPath As String) As String
        'make a copy of the original document and perform all change operations on that copy

        Return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(sOriginalPath),
                                              System.IO.Path.GetFileNameWithoutExtension(sOriginalPath) + sNewDocSuffix + System.IO.Path.GetExtension(sOriginalPath))
    End Function




    Public Sub OpenConnection()

        'sConnection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + mPath + ";Extended Properties=""Excel 12.0;HDR=No;IMEX=1"""
        Dim sConnection = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='{0}';Extended Properties=""Excel 12.0;HDR=Yes""", mPath)

        mConnection = New OleDbConnection(sConnection)
        mConnection.Open()

    End Sub



    Public Sub ModifyField(ByVal value As Integer)

        Dim sSheetName = "'Text Table$'"  'Panel Attributes document always has same format

        Dim oleExcelCommand As OleDbCommand = mConnection.CreateCommand()
        oleExcelCommand.CommandType = CommandType.Text

        Try
            If Not mConnection.State = ConnectionState.Open Then mConnection.Open()
            oleExcelCommand.CommandText = String.Format("UPDATE [Text Table$] SET [Value{1}] = {2} WHERE [Value{1}] = {1}", sSheetName, CStr(value), CStr(value + 1))
            oleExcelCommand.ExecuteNonQuery()
        Catch ex As Exception
            If Not mConnection.State = ConnectionState.Open Then mConnection.Open()
            oleExcelCommand.CommandText = String.Format("UPDATE [Text Table$] SET [Value{1}] = '{2}' WHERE [Value{1}] = '{1}'", sSheetName, CStr(value), CStr(value + 1))
            oleExcelCommand.ExecuteNonQuery()
        End Try

    End Sub



    Public Function GetStateTextByID(ByVal id As String) As StateTextTable

        OpenConnection()

        Dim oleExcelCommand As OleDbCommand = mConnection.CreateCommand()
        oleExcelCommand.CommandType = CommandType.Text
        oleExcelCommand.CommandText = String.Format("Select * From ['Text Table$'] where [ID] = {0}", id)

        Dim oleExcelReader As OleDbDataReader = oleExcelCommand.ExecuteReader
        Dim data As New DataTable
        data.Load(oleExcelReader)

        oleExcelReader.Close()
        mConnection.Close()

        Return ParseStateText(data)

    End Function



    Public Function ParseStateText(ByVal stateTextData As DataTable) As StateTextTable

        Dim tableName As String = stateTextData.Rows(0).Item("Descriptor")
        Dim tableId As String = stateTextData.Rows(0).Item("ID")


        Dim newData As New DataTable(tableName)
        newData.Columns.Add("Value")
        newData.Columns.Add("State Text")


        Dim namesList As List(Of String) = New List(Of String)()
        Dim valuesList As List(Of String) = New List(Of String)()
        Dim isZeroBased As Boolean = False

        For Each column As DataColumn In stateTextData.Columns

            Dim columnName As String = column.ColumnName
            Dim item As Object = stateTextData.Rows(0).Item(column)
            If IsDBNull(item) Then item = ""
            Dim data As String = CStr(item)

            If data.Trim = "" Then Continue For

            If columnName.StartsWith("Name") Then namesList.Add(data)

            If columnName.StartsWith("Value") Then
                Dim val As Integer = CInt(data)
                If val = 0 Then isZeroBased = True
                If isZeroBased Then val += 1
                valuesList.Add(CStr(val))
            End If


        Next

        Dim table As StateTextTable

        table.tableName = tableName
        table.tableId = tableId
        table.namesList = namesList
        table.valuesList = valuesList

        Return table


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
