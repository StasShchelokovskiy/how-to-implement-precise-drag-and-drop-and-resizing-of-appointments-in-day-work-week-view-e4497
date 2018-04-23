Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraScheduler
Imports DevExpress.XtraScheduler.Drawing
Imports OneMinuteDragDrop.Handlers

Namespace OneMinuteDragDrop
	Public Class AppointmentResizeHelper
		Inherits SchedulerMoveEventHandler

		Public Sub New(ByVal control As SchedulerControl)
			MyBase.New(control)
		End Sub

		Public Overrides Sub AttachToControl()
			AddHandler control.AppointmentResizing, AddressOf AppointmentResizeHandler
			AddHandler control.AppointmentResized, AddressOf AppointmentResizeHandler
		End Sub

		Public Overrides Sub DetachFromControl()
			RemoveHandler control.AppointmentResizing, AddressOf AppointmentResizeHandler
			RemoveHandler control.AppointmentResized, AddressOf AppointmentResizeHandler
		End Sub

		Protected Overridable Sub AppointmentResizeHandler(ByVal sender As Object, ByVal e As AppointmentResizeEventArgs)
			Dim borderPos As Integer = If(e.ResizedSide = ResizedSide.AtStartTime, TimeCellViewInfo.Bounds.Y, TimeCellViewInfo.Bounds.Y + TimeCellViewInfo.Bounds.Height)
			If Math.Abs(MousePosition.Y - borderPos) > 1 Then

				Dim cellTimeShift As TimeSpan = GetCellTimeShift()

				If e.ResizedSide = ResizedSide.AtStartTime Then
					If e.SourceAppointment.End > e.HitInterval.Start + cellTimeShift Then
						e.EditedAppointment.Start = e.HitInterval.Start + cellTimeShift
						e.EditedAppointment.End = e.SourceAppointment.End
					End If
				Else
					If e.HitInterval.Start + cellTimeShift > e.SourceAppointment.Start Then
						e.EditedAppointment.Start = e.SourceAppointment.Start
						e.EditedAppointment.End = e.HitInterval.Start + cellTimeShift
					End If
				End If

				e.Handled = True
			End If
		End Sub
	End Class
End Namespace
