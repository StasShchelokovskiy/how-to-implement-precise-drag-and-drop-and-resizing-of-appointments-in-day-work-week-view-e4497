Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports DevExpress.XtraScheduler
Imports DevExpress.XtraScheduler.Drawing
Imports System.Drawing
Imports System.Windows.Forms

Namespace OneMinuteDragDrop.Handlers
    Public MustInherit Class SchedulerMoveEventHandler
        Protected control As SchedulerControl

        Protected Friend ReadOnly Property MousePosition() As Point
            Get
                Return control.PointToClient(Form.MousePosition)
            End Get
        End Property
        Protected Friend ReadOnly Property TimeCellViewInfo() As SelectableIntervalViewInfo
            Get
                Return GetTimeCellViewInfo()
            End Get
        End Property
        Protected Friend ReadOnly Property AppointmentViewInfo() As AppointmentViewInfo
            Get
                Return GetAppointmentViewInfo()
            End Get
        End Property

        Public Sub New(ByVal control As SchedulerControl)
            Me.control = control
        End Sub

        Public Overridable Sub AttachToControl()
        End Sub
        Public Overridable Sub DetachFromControl()
        End Sub

        Protected Friend Function GetTimeCellViewInfo() As SelectableIntervalViewInfo
            Dim shi As SchedulerHitInfo = control.ActiveView.ViewInfo.CalcHitInfo(MousePosition, True)
            Return shi.ViewInfo
        End Function

        Protected Friend Function GetAppointmentViewInfo() As AppointmentViewInfo
            Dim shi As SchedulerHitInfo = control.ActiveView.ViewInfo.CalcHitInfo(MousePosition, False)
            Return If(shi.HitTest = SchedulerHitTest.AppointmentContent, CType(shi.ViewInfo, AppointmentViewInfo), Nothing)
        End Function

        Protected Friend Function GetCellTimeShift() As TimeSpan
            Dim percent As Double = If(TimeCellViewInfo.Bounds.Height <> 0, CDbl(MousePosition.Y - TimeCellViewInfo.Bounds.Y) / CDbl(TimeCellViewInfo.Bounds.Height), 0)
            Return TimeSpan.FromMinutes(TimeCellViewInfo.Interval.Duration.TotalMinutes * percent)
        End Function
    End Class
End Namespace
