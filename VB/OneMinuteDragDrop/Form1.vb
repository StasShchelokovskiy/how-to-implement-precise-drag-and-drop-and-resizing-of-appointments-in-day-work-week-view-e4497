Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports DevExpress.XtraScheduler
Imports DevExpress.XtraScheduler.Drawing

Namespace OneMinuteDragDrop
    Partial Public Class Form1
        Inherits Form

        Private dragHandler As AppointmentDragHelper
        Private resizeHandler As AppointmentResizeHelper

        Public Sub New()
            InitializeComponent()
            schedulerControl1.DayView.AppointmentDisplayOptions.SnapToCellsMode = AppointmentSnapToCellsMode.Never
            schedulerControl1.WorkWeekView.AppointmentDisplayOptions.SnapToCellsMode = AppointmentSnapToCellsMode.Never

            dragHandler = New AppointmentDragHelper(schedulerControl1)
            resizeHandler = New AppointmentResizeHelper(schedulerControl1)
        End Sub

        Private Sub checkEdit1_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles checkEdit1.CheckedChanged
            If DirectCast(sender, CheckEdit).Checked Then
                dragHandler.AttachToControl()
            Else
                dragHandler.DetachFromControl()
            End If
        End Sub

        Private Sub checkEdit2_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles checkEdit2.CheckedChanged
            If DirectCast(sender, CheckEdit).Checked Then
                resizeHandler.AttachToControl()
            Else
                resizeHandler.DetachFromControl()
            End If
        End Sub
    End Class
End Namespace