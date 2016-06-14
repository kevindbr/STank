Imports System.ComponentModel
Imports System.Data
Imports System.Data.OleDb
Imports System.IO

Public Class PanelAttributesDoc
    Implements INotifyPropertyChanged

    Private mPanel As Panel
    Private mPath As String

    Private mReplacementValues As Dictionary(Of String, String)     'TODO: get these from a different file

    Private mPanelAttributesData As DataTable


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
                'mReplacementValues = mPanel.NameChangeDocument.ReplacementValues\
                getReplacementValues()
                readDoc()
            End If

            NotifyPropertyChanged("Path")
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


    Private Sub readExcelIntoDataTable()
        mPanelAttributesData = New DataTable

        Dim strConnString As String
        strConnString = "Driver={Microsoft Excel Driver (*.xls, *.xlsx, *.xlsm, *.xlsb)};DBQ=" & mPath & ";"


        Dim sheetName = "Book1"
        Dim strSQL As String
        strSQL = "SELECT * FROM [" & sheetName & "$]"

        Dim y As New Odbc.OdbcDataAdapter(strSQL, strConnString)

        y.Fill(mPanelAttributesData)


    End Sub



    Private Sub readDoc()

        mPanelAttributesData = New DataTable

        Dim sSheetName As String
        Dim sConnection As String
        Dim dtTablesList As DataTable
        Dim oleExcelCommand As OleDbCommand
        Dim oleExcelReader As OleDbDataReader
        Dim oleExcelConnection As OleDbConnection

        'sConnection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + mPath + ";Extended Properties=""Excel 12.0;HDR=No;IMEX=1"""
        sConnection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + mPath + ";Extended Properties=""Excel 12.0;HDR=No;"""

        oleExcelConnection = New OleDbConnection(sConnection)
        oleExcelConnection.Open()

        dtTablesList = oleExcelConnection.GetSchema("Tables")

        If dtTablesList.Rows.Count > 0 Then
            sSheetName = dtTablesList.Rows(0)("TABLE_NAME").ToString
        End If

        dtTablesList.Clear()
        dtTablesList.Dispose()

        If sSheetName <> "" Then

            oleExcelCommand = oleExcelConnection.CreateCommand()
            oleExcelCommand.CommandText = "Select * From [" & sSheetName & "]"
            oleExcelCommand.CommandType = CommandType.Text

            oleExcelReader = oleExcelCommand.ExecuteReader


            mPanelAttributesData.Load(oleExcelReader)


            For Each kvp As KeyValuePair(Of String, String) In mReplacementValues


                oleExcelCommand = oleExcelConnection.CreateCommand()
                oleExcelCommand.CommandText = "UPDATE [" + sSheetName + "] SET F9 = '" + kvp.Value + "' WHERE F9 = '" + kvp.Key + "'"
                oleExcelCommand.CommandType = CommandType.Text

                oleExcelCommand.ExecuteNonQuery()



            Next


            'TODO: get list of column names from 1st row


            'cmd.CommandText = "UPDATE [" + sSheetName + "] SET name = 'DDDD' WHERE id = 3;";
            'cmd.ExecuteNonQuery();



            'DataTable()

            'nOutputRow = 0

            'While oleExcelReader.Read

            'oleExcelReader.

            'End While

            oleExcelReader.Close()

        End If

        oleExcelConnection.Close()


    End Sub




    Private Sub getReplacementValues()
        mReplacementValues = New Dictionary(Of String, String)
        Dim lines() As String = File.ReadAllLines("unitConversions.csv")
        For Each line As String In lines
            Dim vals() As String = line.Split(","c)
            If (vals(0) <> "" And vals(1) <> "") Then
                Try

                    mReplacementValues.Add(vals(0), vals(1))
                Catch
                    MsgBox("Please check columns in csv file")
                End Try
            End If
        Next line
        'Return replacementValues
    End Sub



End Class
