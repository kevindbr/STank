﻿#ExternalChecksum("..\..\MainWindow.xaml","{406ea660-64cf-4c82-b6f0-42d48172a799}","8BD91BD1967C9FAB5399708180F509CF")
'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.34209
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
'''MainWindow
'''</summary>
<Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>  _
Partial Public Class MainWindow
    Inherits MahApps.Metro.Controls.MetroWindow
    Implements System.Windows.Markup.IComponentConnector
    
    
    #ExternalSource("..\..\MainWindow.xaml",16)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents LoadData As System.Windows.Controls.MenuItem
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\MainWindow.xaml",17)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents SaveData As System.Windows.Controls.MenuItem
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\MainWindow.xaml",18)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents [Exit] As System.Windows.Controls.MenuItem
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\MainWindow.xaml",22)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents ShowDocumentation As System.Windows.Controls.MenuItem
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\MainWindow.xaml",26)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents Help As System.Windows.Controls.MenuItem
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\MainWindow.xaml",30)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents About As System.Windows.Controls.MenuItem
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\MainWindow.xaml",59)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents workingDirectory As System.Windows.Controls.TextBox
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\MainWindow.xaml",64)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents nameChangeDocument As System.Windows.Controls.TextBox
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\MainWindow.xaml",69)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents panelAttributesDocument As System.Windows.Controls.TextBox
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\MainWindow.xaml",85)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents serialPortList As System.Windows.Controls.ListBox
    
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
        Dim resourceLocater As System.Uri = New System.Uri("/STank;component/mainwindow.xaml", System.UriKind.Relative)
        
        #ExternalSource("..\..\MainWindow.xaml",1)
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
            
            #ExternalSource("..\..\MainWindow.xaml",10)
            AddHandler CType(target,MainWindow).Loaded, New System.Windows.RoutedEventHandler(AddressOf Me.IntializeMainWindow)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 2) Then
            Me.LoadData = CType(target,System.Windows.Controls.MenuItem)
            
            #ExternalSource("..\..\MainWindow.xaml",16)
            AddHandler Me.LoadData.Click, New System.Windows.RoutedEventHandler(AddressOf Me.LoadData_Click_1)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 3) Then
            Me.SaveData = CType(target,System.Windows.Controls.MenuItem)
            
            #ExternalSource("..\..\MainWindow.xaml",17)
            AddHandler Me.SaveData.Click, New System.Windows.RoutedEventHandler(AddressOf Me.LoadData_Click_1)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 4) Then
            Me.[Exit] = CType(target,System.Windows.Controls.MenuItem)
            
            #ExternalSource("..\..\MainWindow.xaml",18)
            AddHandler Me.[Exit].Click, New System.Windows.RoutedEventHandler(AddressOf Me.LoadData_Click_1)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 5) Then
            Me.ShowDocumentation = CType(target,System.Windows.Controls.MenuItem)
            
            #ExternalSource("..\..\MainWindow.xaml",22)
            AddHandler Me.ShowDocumentation.Click, New System.Windows.RoutedEventHandler(AddressOf Me.LoadData_Click_1)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 6) Then
            Me.Help = CType(target,System.Windows.Controls.MenuItem)
            
            #ExternalSource("..\..\MainWindow.xaml",26)
            AddHandler Me.Help.Click, New System.Windows.RoutedEventHandler(AddressOf Me.LoadData_Click_1)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 7) Then
            Me.About = CType(target,System.Windows.Controls.MenuItem)
            
            #ExternalSource("..\..\MainWindow.xaml",30)
            AddHandler Me.About.Click, New System.Windows.RoutedEventHandler(AddressOf Me.LoadData_Click_1)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 8) Then
            
            #ExternalSource("..\..\MainWindow.xaml",53)
            AddHandler CType(target,System.Windows.Controls.Button).Click, New System.Windows.RoutedEventHandler(AddressOf Me.browseDirectoryClicked)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 9) Then
            
            #ExternalSource("..\..\MainWindow.xaml",54)
            AddHandler CType(target,System.Windows.Controls.Button).Click, New System.Windows.RoutedEventHandler(AddressOf Me.browseNameClicked)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 10) Then
            
            #ExternalSource("..\..\MainWindow.xaml",55)
            AddHandler CType(target,System.Windows.Controls.Button).Click, New System.Windows.RoutedEventHandler(AddressOf Me.browseAttributeClicked)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 11) Then
            Me.workingDirectory = CType(target,System.Windows.Controls.TextBox)
            Return
        End If
        If (connectionId = 12) Then
            Me.nameChangeDocument = CType(target,System.Windows.Controls.TextBox)
            Return
        End If
        If (connectionId = 13) Then
            Me.panelAttributesDocument = CType(target,System.Windows.Controls.TextBox)
            Return
        End If
        If (connectionId = 14) Then
            
            #ExternalSource("..\..\MainWindow.xaml",83)
            AddHandler CType(target,System.Windows.Controls.Button).Click, New System.Windows.RoutedEventHandler(AddressOf Me.showConnectionView)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 15) Then
            Me.serialPortList = CType(target,System.Windows.Controls.ListBox)
            
            #ExternalSource("..\..\MainWindow.xaml",85)
            AddHandler Me.serialPortList.SelectionChanged, New System.Windows.Controls.SelectionChangedEventHandler(AddressOf Me.LoadData_Click_1)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 16) Then
            
            #ExternalSource("..\..\MainWindow.xaml",92)
            AddHandler CType(target,System.Windows.Controls.Button).Click, New System.Windows.RoutedEventHandler(AddressOf Me.findAndReplaceClicked)
            
            #End ExternalSource
            Return
        End If
        Me._contentLoaded = true
    End Sub
End Class

