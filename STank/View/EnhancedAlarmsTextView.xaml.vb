Imports System.ComponentModel
Imports System.Data
Imports System.Windows.Automation
Imports System.Windows.Automation.Provider
Imports System.Windows.Automation.Peers
Imports System.Windows.Threading
Imports System.Data.OleDb
Imports System.Text.RegularExpressions

Public Class EnhancedAlarmsTextView

    Private mAlarmsDataRow As DataRow

    ''' <summary>
    ''' Bring in mainViewModel to update and change project data
    ''' </summary>
    ''' <param name="alarmsDataRow"></param>
    ''' <remarks></remarks>
    Sub New(ByVal alarmsDataRow As DataRow)

        mAlarmsDataRow = alarmsDataRow

        InitializeComponent()


    End Sub

    Private Sub IntializeWindow()
        log.Items.Add("LOCAL(ALMDLYTMR)")
        log.Items.Add("")
        log.Items.Add("SAMPLE(2) ""$ALMDLYTMR"" = ""$ALMDLYTMR"" + 2")
        log.Items.Add("")
        log.Items.Add(String.Format("IF(""{0}"" .EQ. OFF) THEN SET(0.0,""$ALMDLYTMR"")", mAlarmsDataRow.Item("Mode Point")))
        log.Items.Add(String.Format("IF(""$ALMDLYTMR"" .LT. 900) THEN ""{0}"" = ""{1}"" ELSE ""{0}"" = ""{2}""",
                                                 mAlarmsDataRow.Item("SysName") + "V", mAlarmsDataRow.Item("SysName"),
                                                 mAlarmsDataRow.Item("D1 Set Point Name")))
    End Sub



    Private Sub exitView(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub

End Class
