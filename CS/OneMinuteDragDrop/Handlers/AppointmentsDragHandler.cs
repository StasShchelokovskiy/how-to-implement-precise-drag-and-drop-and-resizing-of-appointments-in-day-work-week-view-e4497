using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Drawing;
using OneMinuteDragDrop.Handlers;

namespace OneMinuteDragDrop {
    public class AppointmentDragHelper : SchedulerMoveEventHandler {

        AppointmentDragInfo dragInfo;

        protected internal TimeSpan CurrentTimeStamp {
            get { return GetCurrentTimeStamp(); }
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
            if (AppointmentViewInfo != null) {
                dragInfo = new AppointmentDragInfo() {
                    Appointment = AppointmentViewInfo.Appointment,
                    TimeAtCursor = CurrentTimeStamp
                };
            }
        }

        protected virtual void AppointmentDragHandler(object sender, AppointmentDragEventArgs e) {
            AdjustSourceAptStart(e);
            OnDragDrop(e);
        }

        protected virtual void AppointmentDropHandler(object sender, AppointmentDragEventArgs e) {
            OnDragDrop(e);
        }

        void OnDragDrop(AppointmentDragEventArgs e) {
            e.EditedAppointment.Start = e.SourceAppointment.Start + CurrentTimeStamp - dragInfo.TimeAtCursor;
        }

        void AdjustSourceAptStart(AppointmentDragEventArgs e) {
            if (e.SourceAppointment.Start.Day != CurrentDate.Day) {
                e.SourceAppointment.Start = e.SourceAppointment.End < e.EditedAppointment.End ?
                    e.SourceAppointment.Start.AddDays(1) : e.SourceAppointment.Start.AddDays(-1);
            }
        }

        private TimeSpan GetCurrentTimeStamp() {
            return TimeCellViewInfo.Interval.Start.TimeOfDay + GetCellTimeShift();
        }
    }

    class AppointmentDragInfo {
        public AppointmentDragInfo() { }

        public Appointment Appointment { get; set; }
        public TimeSpan TimeAtCursor { get; set; }
    }
}
