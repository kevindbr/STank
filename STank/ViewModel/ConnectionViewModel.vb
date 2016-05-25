Public Class ConnectionViewModel

    Public mCommPort As CommPort
    Public mConnectedPorts As List(Of String)


    Sub New()
        mCommPort = New CommPort()
        mConnectedPorts = New List(Of String)
        mCommPort.IntializeData()
    End Sub

    ''' <summary>
    ''' Update comm port object depending on information received from UI
    ''' </summary>
    ''' <remarks></remarks>
    Sub validateCommPort()

    End Sub

End Class
