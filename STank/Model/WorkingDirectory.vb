Imports System.ComponentModel

Public Class WorkingDirectory
    Implements INotifyPropertyChanged

    Private mPanel As Panel
    Private mNameChangeDoc As NameChangeDoc
    Private mPanelAttributesDoc As PanelAttributesDoc
    Private mPath As String


    Public Event PropertyChanged As PropertyChangedEventHandler _
  Implements INotifyPropertyChanged.PropertyChanged

    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub


    Public Property NameChangeDocument As NameChangeDoc
        Get
            Return mNameChangeDoc
        End Get

        Set(value As NameChangeDoc)
            mNameChangeDoc = value
            NotifyPropertyChanged("NameChangeDocument")
        End Set
    End Property

    Public Property PanelAttributesDocument As PanelAttributesDoc
        Get
            Return mPanelAttributesDoc
        End Get

        Set(value As PanelAttributesDoc)
            mPanelAttributesDoc = value
            NotifyPropertyChanged("PanelAttributesDocument")
        End Set
    End Property

    Public Property Path As String
        Get
            Return mPath
        End Get

        Set(value As String)
            mPath = value
            NotifyPropertyChanged("Path")
        End Set
    End Property

    Sub IntializeData()
        mNameChangeDoc = New NameChangeDoc()
        mNameChangeDoc.Path = "C:\"

        mPanelAttributesDoc = New PanelAttributesDoc()
        mPanelAttributesDoc.Path = "C:\"
        mPath = "C:\"
    End Sub

End Class
