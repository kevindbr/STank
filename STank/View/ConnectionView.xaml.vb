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
        mConnectionViewModel = New ConnectionViewModel(mMainViewModel.getProj.Panel.Port)
        InitializeComponent()

        hostString.DataContext = mConnectionViewModel.mCommPort
        tcpPort.DataContext = mConnectionViewModel.mCommPort
        userName.DataContext = mConnectionViewModel.mCommPort
        password.DataContext = mConnectionViewModel.mCommPort

        searchForPorts()
        serialStackPanel.Visibility = Windows.Visibility.Visible
        tcpStackPanel.Visibility = Windows.Visibility.Hidden
        serialStackPanel.Height = "240"
        isSerial = True
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
        tcpStackPanel.Height = "240"
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
        serialStackPanel.Height = "240"
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
        Dim successfulConnection = mConnectionViewModel.validateCommPort()

        If (successfulConnection) Then
            mMainViewModel.addNewPort(mConnectionViewModel.mCommPort)
            mMainViewModel.getProj.Panel.Port.PortName = mConnectionViewModel.mCommPort.PortName
            Dim message As GeneralPopupView = New GeneralPopupView("New panel connection established using : " + mConnectionViewModel.mCommPort.PortName)
            message.Show()
        End If

        If Not (successfulConnection) Then
            Dim message As GeneralPopupView = New GeneralPopupView("Connection to panel failed, please check username and password for: " + mConnectionViewModel.mCommPort.PortName)
            mMainViewModel.getProj.Panel.Port.PortName = mConnectionViewModel.mCommPort.PortName
            message.Show()
        End If




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
