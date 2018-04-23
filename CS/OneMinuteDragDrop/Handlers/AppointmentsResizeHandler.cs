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
    public class AppointmentResizeHelper : SchedulerMoveEventHandler {

        public AppointmentResizeHelper(SchedulerControl control) : base(control) { }

        public override void AttachToControl() {
            control.AppointmentResizing += AppointmentResizeHandler;
            control.AppointmentResized += AppointmentResizeHandler;
        }

        public override void DetachFromControl() {
            control.AppointmentResizing -= AppointmentResizeHandler;
            control.AppointmentResized -= AppointmentResizeHandler;
        }

        protected virtual void AppointmentResizeHandler(object sender, AppointmentResizeEventArgs e) {
            int borderPos = e.ResizedSide == ResizedSide.AtStartTime ? TimeCellViewInfo.Bounds.Y : TimeCellViewInfo.Bounds.Y + TimeCellViewInfo.Bounds.Height;
            if (Math.Abs(MousePosition.Y - borderPos) > 1) {

                TimeSpan cellTimeShift = GetCellTimeShift();

                if (e.ResizedSide == ResizedSide.AtStartTime) {
                    if (e.SourceAppointment.End > e.HitInterval.Start + cellTimeShift) {
                        e.EditedAppointment.Start = e.HitInterval.Start + cellTimeShift;
                        e.EditedAppointment.End = e.SourceAppointment.End;
                    }
                } else {
                    if (e.HitInterval.Start + cellTimeShift > e.SourceAppointment.Start) {
                        e.EditedAppointment.Start = e.SourceAppointment.Start;
                        e.EditedAppointment.End = e.HitInterval.Start + cellTimeShift;
                    }
                }

                e.Handled = true;
            }
        }
    }
}
