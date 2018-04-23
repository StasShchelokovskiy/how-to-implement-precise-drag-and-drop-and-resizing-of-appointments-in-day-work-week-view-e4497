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
	Public Class AppointmentDragHelper
		Inherits SchedulerMoveEventHandler

		Private dragInfo As AppointmentDragInfo

		Protected Friend ReadOnly Property CurrentTimeStamp() As TimeSpan
			Get
				Return GetCurrentTimeStamp()
			End Get
		End Property
		Protected Friend ReadOnly Property CurrentDate() As DateTime
			Get
				Return TimeCellViewInfo.Interval.Start.Date
			End Get
		End Property

		Public Sub New(ByVal control As SchedulerControl)
			MyBase.New(control)
		End Sub

		Public Overrides Sub AttachToControl()
			AddHandler control.MouseDown, AddressOf MouseDownHandler
			AddHandler control.AppointmentDrop, AddressOf AppointmentDropHandler
			AddHandler control.AppointmentDrag, AddressOf AppointmentDragHandler
		End Sub

		Public Overrides Sub DetachFromControl()
			RemoveHandler control.MouseDown, AddressOf MouseDownHandler
			RemoveHandler control.AppointmentDrop, AddressOf AppointmentDropHandler
			RemoveHandler control.AppointmentDrag, AddressOf AppointmentDragHandler
		End Sub

		Protected Overridable Sub MouseDownHandler(ByVal sender As Object, ByVal e As MouseEventArgs)
			If AppointmentViewInfo IsNot Nothing Then
				dragInfo = New AppointmentDragInfo() With {.Appointment = AppointmentViewInfo.Appointment, .TimeAtCursor = CurrentTimeStamp}
			End If
		End Sub

		Protected Overridable Sub AppointmentDragHandler(ByVal sender As Object, ByVal e As AppointmentDragEventArgs)
			AdjustSourceAptStart(e)
			OnDragDrop(e)
		End Sub

		Protected Overridable Sub AppointmentDropHandler(ByVal sender As Object, ByVal e As AppointmentDragEventArgs)
			OnDragDrop(e)
		End Sub

		Private Sub OnDragDrop(ByVal e As AppointmentDragEventArgs)
			e.EditedAppointment.Start = e.SourceAppointment.Start + CurrentTimeStamp - dragInfo.TimeAtCursor
			e.Handled = True
		End Sub

		Private Sub AdjustSourceAptStart(ByVal e As AppointmentDragEventArgs)
			If e.SourceAppointment.Start.Day <> CurrentDate.Day Then
				e.SourceAppointment.Start = If(e.SourceAppointment.End < e.EditedAppointment.End, e.SourceAppointment.Start.AddDays(1), e.SourceAppointment.Start.AddDays(-1))
			End If
		End Sub

		Private Function GetCurrentTimeStamp() As TimeSpan
			Return TimeCellViewInfo.Interval.Start.TimeOfDay + GetCellTimeShift()
		End Function
	End Class

	Friend Class AppointmentDragInfo
		Public Sub New()
		End Sub

		Private privateAppointment As Appointment
		Public Property Appointment() As Appointment
			Get
				Return privateAppointment
			End Get
			Set(ByVal value As Appointment)
				privateAppointment = value
			End Set
		End Property
		Private privateTimeAtCursor As TimeSpan
		Public Property TimeAtCursor() As TimeSpan
			Get
				Return privateTimeAtCursor
			End Get
			Set(ByVal value As TimeSpan)
				privateTimeAtCursor = value
			End Set
		End Property
	End Class
End Namespace
