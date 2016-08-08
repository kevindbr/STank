Imports System.IO.Ports

Public Class ConnectionView

    Private mMainViewModel As MainViewModel
    Private mConnectionViewModel As ConnectionViewModel
    Public isSerial As Boolean = False

    ''' <summary>
    ''' Bring in mainViewModel to update and change project data
    ''' </summary>
    ''' <param name="mainViewModel"></param>
    ''' <remarks></remarks>
    Sub New(ByRef mainViewModel As MainViewModel)
        mMainViewModel = mainViewModel
        mConnectionViewModel = New ConnectionViewModel()
        InitializeComponent()

        hostString.DataContext = mConnectionViewModel.mCommPort
        tcpPort.DataContext = mConnectionViewModel.mCommPort
        searchForPorts()
    End Sub

    ''' <summary>
    ''' Show the tcp options
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub showTCPOptions(sender As Object, e As RoutedEventArgs)
        tcpStackPanel.Visibility = Windows.Visibility.Visible
        serialStackPanel.Visibility = Windows.Visibility.Hidden
        serialStackPanel.Height = "0"
        tcpStackPanel.Height = "200"
        isSerial = False
    End Sub

    ''' <summary>
    ''' Show the serial options
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub showSerialOptions(sender As Object, e As RoutedEventArgs)
        serialStackPanel.Visibility = Windows.Visibility.Visible
        tcpStackPanel.Visibility = Windows.Visibility.Hidden
        serialStackPanel.Height = "200"
        tcpStackPanel.Height = "0"
        isSerial = True
    End Sub

    ''' <summary>
    ''' Save connection that was defined
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub saveNewConnection(sender As Object, e As RoutedEventArgs)
        mConnectionViewModel.validateCommPort()
        mMainViewModel.addNewPort(mConnectionViewModel.mCommPort)
        Close()
    End Sub

    Private Sub searchForPorts()
        Dim ports As String() = SerialPort.GetPortNames()

        Dim port As String
        For Each port In ports
            Dim listBoxItem = New ListBoxItem()
            listBoxItem.Content = port
            serialPortList.Items.Add(listBoxItem)
        Next
    End Sub

    Private Sub exitView(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub

    Private Sub serialPortChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim selectedItem = serialPortList.SelectedValue.Content
        mConnectionViewModel.mCommPort.PortName = selectedItem
        mMainViewModel.getProj.Panel.Port.PortName = selectedItem
    End Sub

    Private Sub sshVersionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim selectedItem = sshVersion.SelectedValue.Content
        mConnectionViewModel.mCommPort.SshVersion = selectedItem
    End Sub

    Private Sub protocolChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim selectedItem = tcpProtocol.SelectedValue.Content
        mConnectionViewModel.mCommPort.Protocol = selectedItem
    End Sub

    Private Sub tcpServiceTypeChanged(sender As Object, e As RoutedEventArgs)
        Dim selectedItem As RadioButton = sender
        mConnectionViewModel.mCommPort.ServiceType = selectedItem.Content
    End Sub
End Class
