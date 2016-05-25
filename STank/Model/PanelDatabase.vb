Public Class PanelDatabase

    'The Panel database object represents the any and all forms of data stored on the panel device

    ' -------------
    ' Data Members
    ' -------------
    ' mPoints, convient list of objects in the db
    ' mPanelCode, text representation of code stored on device

    Private mPoints As List(Of Point)
    Private mPanelCode As String
    Private mPanel As Panel

    Sub IntializeData()
        mPoints = New List(Of Point)
        mPanelCode = "null"

    End Sub

End Class
