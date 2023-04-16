using System;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using WindowDummy;
using WindowControl;

namespace PlayWindow
{
    public partial class MainForm : Form
    {
        public Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        public ConnectionStringsSection csSection;

        //设置项
        private bool isRotate;
        private string fromIntPtr;

        public MouseHook.MouseHook mh;//钩子实例
        public WindowDummyCenter WDC = new WindowDummyCenter();
        QuickAddForm quickAddForm = null;

        public MainForm()
        {
            InitializeComponent();
            Start();
        }

        private void Start()
        {
            //初始化钩子
            mh = new MouseHook.MouseHook();
            mh.SetHook();

            //添加事件
            //GlobalEvent.Register.UpdateEvent += this.UpdateEvent;
            listRefreshTimer.Tick += this.UpdateEvent;
            mh.MouseDownEvent += OtherFuncs.MouseDownEvent;
            mh.MouseUpEvent += OtherFuncs.MouseUpEvent;
            mh.MouseMoveEvent += OtherFuncs.MouseMoveEvent;

            //UI初始化
            csSection = config.ConnectionStrings;
            isRotate = Convert.ToBoolean(ConfigurationManager.ConnectionStrings["isRotate"].ConnectionString);
            cb_isRotate.Checked = isRotate;
            fromIntPtr = ConfigurationManager.ConnectionStrings["fromIntPtr"].ConnectionString;
            switch (fromIntPtr)
            {
                case "fromWindow":
                    cb_intPtrFrom.SelectedIndex = 0;
                    break;
                case "fromWD":
                    cb_intPtrFrom.SelectedIndex = 1;
                    break;
                default:
                    cb_intPtrFrom.SelectedIndex = 0;
                    break;
            }

            WDC.StartEngine();
            listRefreshTimer.Start();
            IntPtrTextBox.Focus();
        }

        private void UpdateEvent(object sender, EventArgs e)
        {
            IntPtrListView.Items.Clear();

            for (int i = 0; i < WDC.windowDummyInstances.Count; i++)
            {
                WindowDummyInstance singleWDC = WDC.windowDummyInstances[i];
                if (singleWDC == null)
                {
                    WDC.windowDummyInstances.RemoveAt(i);
                    break;
                }
                ListViewItem item = new ListViewItem();
                StringBuilder sb = new StringBuilder(255);
                WindowFuncs.GetWindowText(singleWDC.intPtr_p, sb, sb.Capacity);
                item.SubItems[0].Text = sb.ToString();
                ListViewItem.ListViewSubItem windowIntPtr = new ListViewItem.ListViewSubItem
                {
                    Text = Convert.ToString(singleWDC.intPtr)
                };
                item.SubItems.Add(windowIntPtr);
                ListViewItem.ListViewSubItem windowIntPtrDummy = new ListViewItem.ListViewSubItem
                {
                    Text = Convert.ToString(singleWDC.intPtr_p)
                };
                item.SubItems.Add(windowIntPtrDummy);
                ListViewItem.ListViewSubItem windowRect = new ListViewItem.ListViewSubItem
                {
                    Text = Convert.ToString(WindowFuncs.GetWindowRectangle(singleWDC.intPtr_p))
                };
                item.SubItems.Add(windowRect);

                IntPtrListView.Items.Add(item);
            }
        }
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            WDC.engine.Stop();
            mh.UnHook();
        }

        #region UI事件
        private void cb_isRotate_CheckedChanged(object sender, EventArgs e)
        {
            isRotate = cb_isRotate.Checked;
            csSection.ConnectionStrings["isRotate"].ConnectionString = Convert.ToString(cb_isRotate.Checked);
            config.Save();
            ConfigurationManager.RefreshSection("ConnectionStrings");
        }

        private void cb_intPtrFrom_SelectionChangeCommitted(object sender, EventArgs e)
        {
            switch (cb_intPtrFrom.SelectedIndex)
            {
                case 0:
                    csSection.ConnectionStrings["fromIntPtr"].ConnectionString = "fromWindow";
                    break;
                case 1:
                    csSection.ConnectionStrings["fromIntPtr"].ConnectionString = "fromWD";
                    break;
            }
            config.Save();
            ConfigurationManager.RefreshSection("ConnectionStrings");
            cb_intPtrFromLabel.Text = "重启生效";
        }

        private void btn_AddIntPtr_Click(object sender, EventArgs e)
        {
            if (WDC.NewDummy((IntPtr)Convert.ToInt64(IntPtrTextBox.Text), mh))
            {
                IntPtrLabel.Text = "窗口句柄";
            }
            else
            {
                IntPtrLabel.Text = "请输入有效的IntPtr";
            }
        }

        private void 帮助_关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        private void 快捷添加窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (quickAddForm == null)
            //{
            quickAddForm = new QuickAddForm();
            quickAddForm.Show();
            quickAddForm.mh = mh;
            quickAddForm.WDC = WDC;
            //}
        }

        #endregion

        #region Debug stuff

        private void DEBUG_btn_manualGlobalUpdate_Click(object sender, EventArgs e)
        {
            GlobalEvent.Register.UpdateEventAction();
        }

        private void DEBUG_btn_setBodyPos_Click(object sender, EventArgs e)
        {
            WDC.engine._tumbler.SetBodyPos((IntPtr)Convert.ToInt32(DEBUG_tb_setBodyPos.Text), new System.Numerics.Vector2((float)DEBUG_Engine_setBodyPosX.Value, (float)DEBUG_Engine_setBodyPosY.Value));
        }

        private void DEBUG_btn_addImpulse_Click(object sender, EventArgs e)
        {
            WDC.engine._tumbler.AddImpulse((IntPtr)Convert.ToInt32(DEBUG_tb_addImpulse.Text), new System.Numerics.Vector2((float)DEBUG_Engine_addImpulseX.Value, (float)DEBUG_Engine_addImpulseY.Value));
        }

        private void DEBUG_btn_rotateBody_Click(object sender, EventArgs e)
        {
            WDC.engine._tumbler.RotateBody((IntPtr)Convert.ToInt32(DEBUG_tb_rotateBody.Text), (float)DEBUG_Engine_rotateBody.Value);
        }

        #endregion
    }
}
