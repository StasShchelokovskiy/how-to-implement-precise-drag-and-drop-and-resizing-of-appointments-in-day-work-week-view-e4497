using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Drawing;
using System.Drawing;
using System.Windows.Forms;

namespace OneMinuteDragDrop.Handlers {
    public abstract class SchedulerMoveEventHandler {
        protected SchedulerControl control;

        protected internal Point MousePosition {
            get { return control.PointToClient(Form.MousePosition); }
        }
        protected internal SelectableIntervalViewInfo TimeCellViewInfo {
            get { return GetTimeCellViewInfo(); }
        }
        protected internal AppointmentViewInfo AppointmentViewInfo {
            get { return GetAppointmentViewInfo(); }
        }

        public SchedulerMoveEventHandler(SchedulerControl control) {
            this.control = control;
        }

        public virtual void AttachToControl() { }
        public virtual void DetachFromControl() { }

        protected internal SelectableIntervalViewInfo GetTimeCellViewInfo() {
            SchedulerHitInfo shi = control.ActiveView.ViewInfo.CalcHitInfo(MousePosition, true);
            return shi.ViewInfo;
        }

        protected internal AppointmentViewInfo GetAppointmentViewInfo() {
            SchedulerHitInfo shi = control.ActiveView.ViewInfo.CalcHitInfo(MousePosition, false);
            return (shi.HitTest == SchedulerHitTest.AppointmentContent) ? (AppointmentViewInfo)shi.ViewInfo : null;
        }

        protected internal TimeSpan GetCellTimeShift() {
            double percent = (TimeCellViewInfo.Bounds.Height != 0) ?
               (double)(MousePosition.Y - TimeCellViewInfo.Bounds.Y) / (double)TimeCellViewInfo.Bounds.Height : 0;
            return TimeSpan.FromMinutes(TimeCellViewInfo.Interval.Duration.TotalMinutes * percent);
        }
    }
}
