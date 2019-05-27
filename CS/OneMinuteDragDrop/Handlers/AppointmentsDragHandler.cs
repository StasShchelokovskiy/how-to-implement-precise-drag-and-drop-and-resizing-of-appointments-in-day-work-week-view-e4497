using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraScheduler;
using OneMinuteDragDrop.Handlers;

namespace OneMinuteDragDrop {
    public class AppointmentDragHelper : SchedulerMoveEventHandler {

        AppointmentDragInfo dragInfo;

        protected internal TimeSpan CalcCurrentTimeStamp(TimeInterval viewInfoInterval) {
            return GetCurrentTimeStamp(viewInfoInterval);
        }
        protected internal DateTime CurrentDate {
            get { return TimeCellViewInfo.Interval.Start.Date; }
        }

        public AppointmentDragHelper(SchedulerControl control) : base(control) { }

        public override void AttachToControl() {
            control.MouseDown += MouseDownHandler;
            control.AppointmentDrop += AppointmentDropHandler;
            control.AppointmentDrag += AppointmentDragHandler;
        }

        public override void DetachFromControl() {
            control.MouseDown -= MouseDownHandler;
            control.AppointmentDrop -= AppointmentDropHandler;
            control.AppointmentDrag -= AppointmentDragHandler;
        }

        protected virtual void MouseDownHandler(object sender, MouseEventArgs e) {
            var aptViewInfo = AppointmentViewInfo;
            if (aptViewInfo != null) {
                var timePosition = CalcCurrentTimeStamp(aptViewInfo.Interval);
                dragInfo = new AppointmentDragInfo() {
                    Appointment = aptViewInfo.Appointment,
                    TimeAtCursor = timePosition,
                    ViewInfoInterval = aptViewInfo.Interval
                };
            }
        }

        Appointment sourceAppointment;
        protected virtual void AppointmentDragHandler(object sender, AppointmentDragEventArgs e) {
            sourceAppointment = e.SourceAppointment;
            AdjustSourceAptStart(e);
            OnDragDrop(e);
        }

        protected virtual void AppointmentDropHandler(object sender, AppointmentDragEventArgs e) {
            OnDragDrop(e);
        }

        void OnDragDrop(AppointmentDragEventArgs e) {
            var currentTimeStamp = GetCurrentTimeStamp(this.dragInfo.ViewInfoInterval);
            DateTime temp = e.SourceAppointment.Start + currentTimeStamp - dragInfo.TimeAtCursor;
            e.EditedAppointment.Start = temp;
            e.Allow = true;
        }

        void AdjustSourceAptStart(AppointmentDragEventArgs e) {
            if (e.SourceAppointment.Start.Day != CurrentDate.Day && control.ActiveViewType != SchedulerViewType.Gantt) {
                e.SourceAppointment.Start = e.SourceAppointment.End < e.EditedAppointment.End ? e.SourceAppointment.Start.AddDays(1) : e.SourceAppointment.Start.AddDays(-1);
            }
        }

        private TimeSpan GetCurrentTimeStamp(TimeInterval viewInfoInterval) {
            TimeSpan timeStamp = TimeCellViewInfo.Interval.Start.TimeOfDay + (control.ActiveView.Type == SchedulerViewType.Gantt ? GetCellTimeShiftHorizontally() : GetCellTimeShiftVertically());
            return control.ActiveView.Type == SchedulerViewType.Gantt ? GetDaysTimeSpan(TimeCellViewInfo.Interval, viewInfoInterval) + timeStamp : timeStamp;
        }

        TimeSpan GetDaysTimeSpan(TimeInterval interval, TimeInterval appointmentViewInfoInterval) {
            if (sourceAppointment != null)
                return TimeSpan.FromDays(interval.Start.Day - appointmentViewInfoInterval.Start.Day);
            else
                return TimeSpan.FromDays(interval.Start.Day - interval.End.Day);

        }
    }

    class AppointmentDragInfo {
        public AppointmentDragInfo() { }

        public Appointment Appointment { get; set; }
        public TimeSpan TimeAtCursor { get; set; }
        public TimeInterval ViewInfoInterval { get; set; }
    }
}
