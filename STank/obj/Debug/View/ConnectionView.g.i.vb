﻿#ExternalChecksum("..\..\..\View\ConnectionView.xaml","{406ea660-64cf-4c82-b6f0-42d48172a799}","8ADC997E763AB73C0F6A74AB93068922")
'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports MahApps.Metro.Controls
Imports System
Imports System.Diagnostics
Imports System.Windows
Imports System.Windows.Automation
Imports System.Windows.Controls
Imports System.Windows.Controls.Primitives
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Ink
Imports System.Windows.Input
Imports System.Windows.Markup
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Media.Effects
Imports System.Windows.Media.Imaging
Imports System.Windows.Media.Media3D
Imports System.Windows.Media.TextFormatting
Imports System.Windows.Navigation
Imports System.Windows.Shapes
Imports System.Windows.Shell


'''<summary>
'''ConnectionView
'''</summary>
<Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>  _
Partial Public Class ConnectionView
    Inherits MahApps.Metro.Controls.MetroWindow
    Implements System.Windows.Markup.IComponentConnector
    
    
    #ExternalSource("..\..\..\View\ConnectionView.xaml",13)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents [Exit] As System.Windows.Controls.MenuItem
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\View\ConnectionView.xaml",40)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents serialStackPanel As System.Windows.Controls.StackPanel
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\View\ConnectionView.xaml",43)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents serialPortList As System.Windows.Controls.ListBox
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\View\ConnectionView.xaml",46)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents tcpStackPanel As System.Windows.Controls.StackPanel
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\View\ConnectionView.xaml",49)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents hostString As System.Windows.Controls.TextBox
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\View\ConnectionView.xaml",69)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents tcpPort As System.Windows.Controls.TextBox
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\View\ConnectionView.xaml",73)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents sshVersion As System.Windows.Controls.ComboBox
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\View\ConnectionView.xaml",80)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents tcpProtocol As System.Windows.Controls.ComboBox
    
    #End ExternalSource
    
    Private _contentLoaded As Boolean
    
    '''<summary>
    '''InitializeComponent
    '''</summary>
    <System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")>  _
    Public Sub InitializeComponent() Implements System.Windows.Markup.IComponentConnector.InitializeComponent
        If _contentLoaded Then
            Return
        End If
        _contentLoaded = true
        Dim resourceLocater As System.Uri = New System.Uri("/STank;component/view/connectionview.xaml", System.UriKind.Relative)
        
        #ExternalSource("..\..\..\View\ConnectionView.xaml",1)
        System.Windows.Application.LoadComponent(Me, resourceLocater)
        
        #End ExternalSource
    End Sub
    
    <System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0"),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never),  _
     System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes"),  _
     System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity"),  _
     System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")>  _
    Sub System_Windows_Markup_IComponentConnector_Connect(ByVal connectionId As Integer, ByVal target As Object) Implements System.Windows.Markup.IComponentConnector.Connect
        If (connectionId = 1) Then
            Me.[Exit] = CType(target,System.Windows.Controls.MenuItem)
            Return
        End If
        If (connectionId = 2) Then
            
            #ExternalSource("..\..\..\View\ConnectionView.xaml",29)
            AddHandler CType(target,System.Windows.Controls.RadioButton).Click, New System.Windows.RoutedEventHandler(AddressOf Me.showTCPOptions)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 3) Then
            
            #ExternalSource("..\..\..\View\ConnectionView.xaml",35)
            AddHandler CType(target,System.Windows.Controls.RadioButton).Click, New System.Windows.RoutedEventHandler(AddressOf Me.showSerialOptions)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 4) Then
            Me.serialStackPanel = CType(target,System.Windows.Controls.StackPanel)
            Return
        End If
        If (connectionId = 5) Then
            Me.serialPortList = CType(target,System.Windows.Controls.ListBox)
            
            #ExternalSource("..\..\..\View\ConnectionView.xaml",43)
            AddHandler Me.serialPortList.SelectionChanged, New System.Windows.Controls.SelectionChangedEventHandler(AddressOf Me.serialPortChanged)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 6) Then
            Me.tcpStackPanel = CType(target,System.Windows.Controls.StackPanel)
            Return
        End If
        If (connectionId = 7) Then
            Me.hostString = CType(target,System.Windows.Controls.TextBox)
            Return
        End If
        If (connectionId = 8) Then
            
            #ExternalSource("..\..\..\View\ConnectionView.xaml",55)
            AddHandler CType(target,System.Windows.Controls.RadioButton).Click, New System.Windows.RoutedEventHandler(AddressOf Me.tcpServiceTypeChanged)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 9) Then
            
            #ExternalSource("..\..\..\View\ConnectionView.xaml",60)
            AddHandler CType(target,System.Windows.Controls.RadioButton).Click, New System.Windows.RoutedEventHandler(AddressOf Me.tcpServiceTypeChanged)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 10) Then
            
            #ExternalSource("..\..\..\View\ConnectionView.xaml",65)
            AddHandler CType(target,System.Windows.Controls.RadioButton).Click, New System.Windows.RoutedEventHandler(AddressOf Me.tcpServiceTypeChanged)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 11) Then
            Me.tcpPort = CType(target,System.Windows.Controls.TextBox)
            Return
        End If
        If (connectionId = 12) Then
            Me.sshVersion = CType(target,System.Windows.Controls.ComboBox)
            
            #ExternalSource("..\..\..\View\ConnectionView.xaml",73)
            AddHandler Me.sshVersion.SelectionChanged, New System.Windows.Controls.SelectionChangedEventHandler(AddressOf Me.sshVersionChanged)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 13) Then
            Me.tcpProtocol = CType(target,System.Windows.Controls.ComboBox)
            
            #ExternalSource("..\..\..\View\ConnectionView.xaml",80)
            AddHandler Me.tcpProtocol.SelectionChanged, New System.Windows.Controls.SelectionChangedEventHandler(AddressOf Me.protocolChanged)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 14) Then
            
            #ExternalSource("..\..\..\View\ConnectionView.xaml",89)
            AddHandler CType(target,System.Windows.Controls.Button).Click, New System.Windows.RoutedEventHandler(AddressOf Me.saveNewConnection)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 15) Then
            
            #ExternalSource("..\..\..\View\ConnectionView.xaml",90)
            AddHandler CType(target,System.Windows.Controls.Button).Click, New System.Windows.RoutedEventHandler(AddressOf Me.saveNewConnection)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 16) Then
            
            #ExternalSource("..\..\..\View\ConnectionView.xaml",91)
            AddHandler CType(target,System.Windows.Controls.Button).Click, New System.Windows.RoutedEventHandler(AddressOf Me.exitView)
            
            #End ExternalSource
            Return
        End If
        Me._contentLoaded = true
    End Sub
End Class

