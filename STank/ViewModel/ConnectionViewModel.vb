Public Class ConnectionViewModel

    Public mCommPort As CommPort
    Public mConnectedPorts As List(Of String)


    Sub New(commPort As CommPort)
        mCommPort = commPort
    End Sub

    ''' <summary>
    ''' Update comm port object depending on information received from UI
    ''' </summary>
    ''' <remarks></remarks>
    Function validateCommPort() As Boolean

        Dim response = mCommPort.TestLogin()
        Return response

    End Function

End Class
