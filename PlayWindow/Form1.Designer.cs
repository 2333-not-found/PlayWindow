﻿
namespace PlayWindow
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contentMenuStrip = new System.Windows.Forms.MenuStrip();
            this.menuMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.tabPageMain = new System.Windows.Forms.TabPage();
            this.cb_isRotate = new System.Windows.Forms.CheckBox();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageDebug = new System.Windows.Forms.TabPage();
            this.DEBUG_btn_manualGlobalUpdate = new System.Windows.Forms.Button();
            this.DEBUG_btn_openDummy = new System.Windows.Forms.Button();
            this.IntPtrTextBox = new System.Windows.Forms.TextBox();
            this.intPtrLabel = new System.Windows.Forms.Label();
            this.btn_AddIntPtr = new System.Windows.Forms.Button();
            this.contentMenuStrip.SuspendLayout();
            this.tabPageMain.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageDebug.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentMenuStrip
            // 
            this.contentMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMenuItem});
            this.contentMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.contentMenuStrip.Name = "contentMenuStrip";
            this.contentMenuStrip.Size = new System.Drawing.Size(584, 25);
            this.contentMenuStrip.TabIndex = 0;
            // 
            // menuMenuItem
            // 
            this.menuMenuItem.Name = "menuMenuItem";
            this.menuMenuItem.Size = new System.Drawing.Size(64, 21);
            this.menuMenuItem.Text = "菜单(&M)";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // tabPageMain
            // 
            this.tabPageMain.Controls.Add(this.btn_AddIntPtr);
            this.tabPageMain.Controls.Add(this.intPtrLabel);
            this.tabPageMain.Controls.Add(this.IntPtrTextBox);
            this.tabPageMain.Controls.Add(this.cb_isRotate);
            this.tabPageMain.Location = new System.Drawing.Point(4, 22);
            this.tabPageMain.Name = "tabPageMain";
            this.tabPageMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMain.Size = new System.Drawing.Size(576, 359);
            this.tabPageMain.TabIndex = 0;
            this.tabPageMain.Text = "Main";
            this.tabPageMain.UseVisualStyleBackColor = true;
            // 
            // cb_isRotate
            // 
            this.cb_isRotate.AutoSize = true;
            this.cb_isRotate.Location = new System.Drawing.Point(496, 11);
            this.cb_isRotate.Name = "cb_isRotate";
            this.cb_isRotate.Size = new System.Drawing.Size(72, 16);
            this.cb_isRotate.TabIndex = 0;
            this.cb_isRotate.Text = "isRotate";
            this.cb_isRotate.UseVisualStyleBackColor = true;
            this.cb_isRotate.CheckedChanged += new System.EventHandler(this.cb_isRotate_CheckedChanged);
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageMain);
            this.tabControlMain.Controls.Add(this.tabPageDebug);
            this.tabControlMain.Location = new System.Drawing.Point(0, 28);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(584, 385);
            this.tabControlMain.TabIndex = 1;
            // 
            // tabPageDebug
            // 
            this.tabPageDebug.Controls.Add(this.DEBUG_btn_manualGlobalUpdate);
            this.tabPageDebug.Controls.Add(this.DEBUG_btn_openDummy);
            this.tabPageDebug.Location = new System.Drawing.Point(4, 22);
            this.tabPageDebug.Name = "tabPageDebug";
            this.tabPageDebug.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDebug.Size = new System.Drawing.Size(576, 359);
            this.tabPageDebug.TabIndex = 1;
            this.tabPageDebug.Text = "Debug";
            this.tabPageDebug.UseVisualStyleBackColor = true;
            // 
            // DEBUG_btn_manualGlobalUpdate
            // 
            this.DEBUG_btn_manualGlobalUpdate.Location = new System.Drawing.Point(9, 36);
            this.DEBUG_btn_manualGlobalUpdate.Name = "DEBUG_btn_manualGlobalUpdate";
            this.DEBUG_btn_manualGlobalUpdate.Size = new System.Drawing.Size(107, 23);
            this.DEBUG_btn_manualGlobalUpdate.TabIndex = 1;
            this.DEBUG_btn_manualGlobalUpdate.Text = "手动Update";
            this.DEBUG_btn_manualGlobalUpdate.UseVisualStyleBackColor = true;
            this.DEBUG_btn_manualGlobalUpdate.Click += new System.EventHandler(this.DEBUG_btn_manualGlobalUpdate_Click);
            // 
            // DEBUG_btn_openDummy
            // 
            this.DEBUG_btn_openDummy.Location = new System.Drawing.Point(9, 7);
            this.DEBUG_btn_openDummy.Name = "DEBUG_btn_openDummy";
            this.DEBUG_btn_openDummy.Size = new System.Drawing.Size(107, 23);
            this.DEBUG_btn_openDummy.TabIndex = 0;
            this.DEBUG_btn_openDummy.Text = "打开一个Dummy";
            this.DEBUG_btn_openDummy.UseVisualStyleBackColor = true;
            this.DEBUG_btn_openDummy.Click += new System.EventHandler(this.DEBUG_btn_openDummy_Click);
            // 
            // IntPtrTextBox
            // 
            this.IntPtrTextBox.Location = new System.Drawing.Point(104, 7);
            this.IntPtrTextBox.Name = "IntPtrTextBox";
            this.IntPtrTextBox.Size = new System.Drawing.Size(100, 21);
            this.IntPtrTextBox.TabIndex = 1;
            // 
            // intPtrLabel
            // 
            this.intPtrLabel.AutoSize = true;
            this.intPtrLabel.Location = new System.Drawing.Point(8, 11);
            this.intPtrLabel.Name = "intPtrLabel";
            this.intPtrLabel.Size = new System.Drawing.Size(53, 12);
            this.intPtrLabel.TabIndex = 2;
            this.intPtrLabel.Text = "窗口句柄";
            // 
            // btn_AddIntPtr
            // 
            this.btn_AddIntPtr.Location = new System.Drawing.Point(210, 6);
            this.btn_AddIntPtr.Name = "btn_AddIntPtr";
            this.btn_AddIntPtr.Size = new System.Drawing.Size(75, 23);
            this.btn_AddIntPtr.TabIndex = 3;
            this.btn_AddIntPtr.Text = "添加";
            this.btn_AddIntPtr.UseVisualStyleBackColor = true;
            this.btn_AddIntPtr.Click += new System.EventHandler(this.btn_AddIntPtr_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 412);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.contentMenuStrip);
            this.MainMenuStrip = this.contentMenuStrip;
            this.Name = "Form1";
            this.Text = "Form1";
            this.contentMenuStrip.ResumeLayout(false);
            this.contentMenuStrip.PerformLayout();
            this.tabPageMain.ResumeLayout(false);
            this.tabPageMain.PerformLayout();
            this.tabControlMain.ResumeLayout(false);
            this.tabPageDebug.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip contentMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuMenuItem;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.TabPage tabPageMain;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageDebug;
        private System.Windows.Forms.CheckBox cb_isRotate;
        private System.Windows.Forms.Button DEBUG_btn_openDummy;
        private System.Windows.Forms.Button DEBUG_btn_manualGlobalUpdate;
        private System.Windows.Forms.Label intPtrLabel;
        private System.Windows.Forms.TextBox IntPtrTextBox;
        private System.Windows.Forms.Button btn_AddIntPtr;
    }
}
