﻿using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using WindowDummy;
using GlobalEvent;
using Box2DEngine;

namespace PlayWindow
{
    public partial class Form1 : Form
    {
        public Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        public ConnectionStringsSection csSection;

        public NormalTest engine = new NormalTest();

        private bool isRotate;

        public WindowDummyCenter WDC = new WindowDummyCenter();

        public Form1()
        {
            InitializeComponent();
            Start();

            GlobalEvent.Register.UpdateEvent += this.UpdateEvent;
            WindowDummy.WindowDummyInstance.OnWindowResize += this.OnWindowResize;
            this.FormClosed += (sender, eventArgs) => { engine.Stop(); };
        }

        private void Start()
        {
            Thread thread = new Thread(new ThreadStart(EngineThread));
            thread.Start();


            csSection = config.ConnectionStrings;
            isRotate = Convert.ToBoolean(ConfigurationManager.ConnectionStrings["isRotate"].ConnectionString);

            cb_isRotate.Checked = isRotate;
        }

        private void cb_isRotate_CheckedChanged(object sender, EventArgs e)
        {
            isRotate = cb_isRotate.Checked;
            csSection.ConnectionStrings["isRotate"].ConnectionString = Convert.ToString(cb_isRotate.Checked);
            config.Save();
            ConfigurationManager.RefreshSection("ConnectionStrings");
        }

        private void UpdateEvent()
        {
            Console.WriteLine("test");
        }

        private void OnWindowResize()
        {
            
        }

        private void btn_AddIntPtr_Click(object sender, EventArgs e)
        {
            try
            {
                WDC.NewDummy((IntPtr)Convert.ToInt64(IntPtrTextBox.Text));
                intPtrLabel.Text = "窗口句柄";
            }
            catch
            {
                intPtrLabel.Text = "请输入有效的IntPtr";
            }
        }

        void EngineThread()
        {
            engine.Run();
        }

        #region Debug stuff

        private void DEBUG_btn_openDummy_Click(object sender, EventArgs e)
        {
            WindowDummyInstance windowDummyInstance = new WindowDummyInstance();
            windowDummyInstance.Show();
        }

        private void DEBUG_btn_manualGlobalUpdate_Click(object sender, EventArgs e)
        {
            GlobalEvent.Register.Update();
        }


        #endregion
    }
}
