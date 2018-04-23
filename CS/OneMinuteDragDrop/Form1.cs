using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Drawing;

namespace OneMinuteDragDrop {
    public partial class Form1 : Form {
        AppointmentDragHelper dragHandler;
        AppointmentResizeHelper resizeHandler;

        public Form1() {
            InitializeComponent();
            schedulerControl1.DayView.AppointmentDisplayOptions.SnapToCellsMode = AppointmentSnapToCellsMode.Never;
            schedulerControl1.WorkWeekView.AppointmentDisplayOptions.SnapToCellsMode = AppointmentSnapToCellsMode.Never;

            dragHandler = new AppointmentDragHelper(schedulerControl1);
            resizeHandler = new AppointmentResizeHelper(schedulerControl1);
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e) {
            if (((CheckEdit)sender).Checked)
                dragHandler.AttachToControl();
            else
                dragHandler.DetachFromControl();
        }

        private void checkEdit2_CheckedChanged(object sender, EventArgs e) {
            if (((CheckEdit)sender).Checked)
                resizeHandler.AttachToControl();
            else
                resizeHandler.DetachFromControl();
        }
    }
}