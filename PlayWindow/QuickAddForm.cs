using System;
using System.Windows.Forms;
using WindowControl;
using WindowDummy;

namespace PlayWindow
{
    public partial class QuickAddForm : Form
    {
        public MouseHook.MouseHook mh;//钩子实例
        public WindowDummyCenter WDC = null;
        private bool isHooking = false;
        private IntPtr intPtr;

        public QuickAddForm()
        {
            InitializeComponent();
        }

        private void UpdateEvent(object sender, MouseEventArgs e)
        {
            if (WindowFuncs.GetWindowRectangle(this.Handle).Contains(e.Location))
            {
                return;
            }
            intPtr = WindowFuncs.GetRoot(WindowFuncs.GetHandleFromCursor(true));
            tb_intPtr.Text = intPtr.ToString();
        }

        private void btn_Hook_Click(object sender, EventArgs e)
        {
            if (!isHooking)
            {
                mh.MouseMoveEvent += UpdateEvent;
                btn_Hook.Text = "Stop";
                isHooking = true;
            }
            else
            {
                mh.MouseMoveEvent -= UpdateEvent;
                btn_Hook.Text = "Start";
                isHooking = false;
            }
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            if (WDC != null)
                WDC.RunNewWindowAsync((IntPtr)Convert.ToInt64(tb_intPtr.Text), mh);
        }

        private void QuickAddForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            mh.MouseMoveEvent -= UpdateEvent;
        }
    }
}