
Class GeneralPopupView

    Public mMainViewModel As MainViewModel
    Public mFindAndReplaceMainViewModel As FindAndReplaceMainViewModel
    Public runClicked As Boolean


    ''' <summary>
    ''' Send message show message
    ''' </summary>
    ''' <param name="message"></param>
    ''' <remarks></remarks>
    Sub New(message As String)
        InitializeComponent()
        messageView.Text = message

    End Sub

    Private Sub exitView(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub


    Private Sub IntializeWindow()

    End Sub

End Class
