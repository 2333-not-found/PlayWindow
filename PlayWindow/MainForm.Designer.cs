
namespace PlayWindow
{
    partial class MainForm
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
            this.菜单ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助_关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.tabPageMain = new System.Windows.Forms.TabPage();
            this.cb_intPtrFromLabel = new System.Windows.Forms.Label();
            this.cb_intPtrFrom = new System.Windows.Forms.ComboBox();
            this.IntPtrListView = new System.Windows.Forms.ListView();
            this.IntPtrListView_WindowName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IntPtrListView_WindowIntPtrDummy = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IntPtrListView_WindowIntPtr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IntPtrListView_WindowRect = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btn_AddIntPtr = new System.Windows.Forms.Button();
            this.IntPtrLabel = new System.Windows.Forms.Label();
            this.IntPtrTextBox = new System.Windows.Forms.TextBox();
            this.cb_isRotate = new System.Windows.Forms.CheckBox();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageDebug = new System.Windows.Forms.TabPage();
            this.DEBUG_Engine_groupBox = new System.Windows.Forms.GroupBox();
            this.DEBUG_Engine_rotateBody = new System.Windows.Forms.NumericUpDown();
            this.DEBUG_tb_rotateBody = new System.Windows.Forms.TextBox();
            this.DEBUG_btn_rotateBody = new System.Windows.Forms.Button();
            this.DEBUG_tb_addImpulse = new System.Windows.Forms.TextBox();
            this.DEBUG_btn_addImpulse = new System.Windows.Forms.Button();
            this.DEBUG_Engine_addImpulseLabel = new System.Windows.Forms.Label();
            this.DEBUG_Engine_addImpulseY = new System.Windows.Forms.NumericUpDown();
            this.DEBUG_Engine_addImpulseX = new System.Windows.Forms.NumericUpDown();
            this.DEBUG_btn_manualGlobalUpdate = new System.Windows.Forms.Button();
            this.listRefreshTimer = new System.Windows.Forms.Timer(this.components);
            this.contentMenuStrip.SuspendLayout();
            this.tabPageMain.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageDebug.SuspendLayout();
            this.DEBUG_Engine_groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DEBUG_Engine_rotateBody)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DEBUG_Engine_addImpulseY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DEBUG_Engine_addImpulseX)).BeginInit();
            this.SuspendLayout();
            // 
            // contentMenuStrip
            // 
            this.contentMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.菜单ToolStripMenuItem,
            this.帮助ToolStripMenuItem});
            this.contentMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.contentMenuStrip.Name = "contentMenuStrip";
            this.contentMenuStrip.Size = new System.Drawing.Size(584, 25);
            this.contentMenuStrip.TabIndex = 0;
            // 
            // 菜单ToolStripMenuItem
            // 
            this.菜单ToolStripMenuItem.Name = "菜单ToolStripMenuItem";
            this.菜单ToolStripMenuItem.Size = new System.Drawing.Size(64, 21);
            this.菜单ToolStripMenuItem.Text = "菜单(&M)";
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.帮助_关于ToolStripMenuItem});
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(61, 21);
            this.帮助ToolStripMenuItem.Text = "帮助(&H)";
            // 
            // 帮助_关于ToolStripMenuItem
            // 
            this.帮助_关于ToolStripMenuItem.Name = "帮助_关于ToolStripMenuItem";
            this.帮助_关于ToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.帮助_关于ToolStripMenuItem.Text = "关于(&A)";
            this.帮助_关于ToolStripMenuItem.Click += new System.EventHandler(this.帮助_关于ToolStripMenuItem_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // tabPageMain
            // 
            this.tabPageMain.Controls.Add(this.cb_intPtrFromLabel);
            this.tabPageMain.Controls.Add(this.cb_intPtrFrom);
            this.tabPageMain.Controls.Add(this.IntPtrListView);
            this.tabPageMain.Controls.Add(this.btn_AddIntPtr);
            this.tabPageMain.Controls.Add(this.IntPtrLabel);
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
            // cb_intPtrFromLabel
            // 
            this.cb_intPtrFromLabel.AutoSize = true;
            this.cb_intPtrFromLabel.Location = new System.Drawing.Point(291, 11);
            this.cb_intPtrFromLabel.Name = "cb_intPtrFromLabel";
            this.cb_intPtrFromLabel.Size = new System.Drawing.Size(113, 12);
            this.cb_intPtrFromLabel.TabIndex = 6;
            this.cb_intPtrFromLabel.Text = "物理引擎使用的句柄";
            // 
            // cb_intPtrFrom
            // 
            this.cb_intPtrFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cb_intPtrFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_intPtrFrom.FormattingEnabled = true;
            this.cb_intPtrFrom.Items.AddRange(new object[] {
            "自原窗口",
            "自WindowDummy"});
            this.cb_intPtrFrom.Location = new System.Drawing.Point(409, 7);
            this.cb_intPtrFrom.Name = "cb_intPtrFrom";
            this.cb_intPtrFrom.Size = new System.Drawing.Size(81, 20);
            this.cb_intPtrFrom.TabIndex = 5;
            this.cb_intPtrFrom.SelectionChangeCommitted += new System.EventHandler(this.cb_intPtrFrom_SelectionChangeCommitted);
            // 
            // IntPtrListView
            // 
            this.IntPtrListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.IntPtrListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.IntPtrListView_WindowName,
            this.IntPtrListView_WindowIntPtrDummy,
            this.IntPtrListView_WindowIntPtr,
            this.IntPtrListView_WindowRect});
            this.IntPtrListView.GridLines = true;
            this.IntPtrListView.HideSelection = false;
            this.IntPtrListView.Location = new System.Drawing.Point(10, 34);
            this.IntPtrListView.Name = "IntPtrListView";
            this.IntPtrListView.Size = new System.Drawing.Size(558, 316);
            this.IntPtrListView.TabIndex = 4;
            this.IntPtrListView.UseCompatibleStateImageBehavior = false;
            this.IntPtrListView.View = System.Windows.Forms.View.Details;
            // 
            // IntPtrListView_WindowName
            // 
            this.IntPtrListView_WindowName.Text = "窗口标题";
            this.IntPtrListView_WindowName.Width = 150;
            // 
            // IntPtrListView_WindowIntPtrDummy
            // 
            this.IntPtrListView_WindowIntPtrDummy.Text = "Dummy句柄";
            this.IntPtrListView_WindowIntPtrDummy.Width = 70;
            // 
            // IntPtrListView_WindowIntPtr
            // 
            this.IntPtrListView_WindowIntPtr.Text = "句柄";
            this.IntPtrListView_WindowIntPtr.Width = 70;
            // 
            // IntPtrListView_WindowRect
            // 
            this.IntPtrListView_WindowRect.Text = "窗口矩形大小";
            this.IntPtrListView_WindowRect.Width = 220;
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
            // IntPtrLabel
            // 
            this.IntPtrLabel.AutoSize = true;
            this.IntPtrLabel.Location = new System.Drawing.Point(8, 11);
            this.IntPtrLabel.Name = "IntPtrLabel";
            this.IntPtrLabel.Size = new System.Drawing.Size(53, 12);
            this.IntPtrLabel.TabIndex = 2;
            this.IntPtrLabel.Text = "窗口句柄";
            // 
            // IntPtrTextBox
            // 
            this.IntPtrTextBox.Location = new System.Drawing.Point(104, 7);
            this.IntPtrTextBox.Name = "IntPtrTextBox";
            this.IntPtrTextBox.Size = new System.Drawing.Size(100, 21);
            this.IntPtrTextBox.TabIndex = 1;
            // 
            // cb_isRotate
            // 
            this.cb_isRotate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
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
            this.tabControlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.tabPageDebug.Controls.Add(this.DEBUG_Engine_groupBox);
            this.tabPageDebug.Controls.Add(this.DEBUG_btn_manualGlobalUpdate);
            this.tabPageDebug.Location = new System.Drawing.Point(4, 22);
            this.tabPageDebug.Name = "tabPageDebug";
            this.tabPageDebug.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDebug.Size = new System.Drawing.Size(576, 359);
            this.tabPageDebug.TabIndex = 1;
            this.tabPageDebug.Text = "Debug";
            this.tabPageDebug.UseVisualStyleBackColor = true;
            // 
            // DEBUG_Engine_groupBox
            // 
            this.DEBUG_Engine_groupBox.Controls.Add(this.DEBUG_Engine_rotateBody);
            this.DEBUG_Engine_groupBox.Controls.Add(this.DEBUG_tb_rotateBody);
            this.DEBUG_Engine_groupBox.Controls.Add(this.DEBUG_btn_rotateBody);
            this.DEBUG_Engine_groupBox.Controls.Add(this.DEBUG_tb_addImpulse);
            this.DEBUG_Engine_groupBox.Controls.Add(this.DEBUG_btn_addImpulse);
            this.DEBUG_Engine_groupBox.Controls.Add(this.DEBUG_Engine_addImpulseLabel);
            this.DEBUG_Engine_groupBox.Controls.Add(this.DEBUG_Engine_addImpulseY);
            this.DEBUG_Engine_groupBox.Controls.Add(this.DEBUG_Engine_addImpulseX);
            this.DEBUG_Engine_groupBox.Location = new System.Drawing.Point(122, 7);
            this.DEBUG_Engine_groupBox.Name = "DEBUG_Engine_groupBox";
            this.DEBUG_Engine_groupBox.Size = new System.Drawing.Size(402, 100);
            this.DEBUG_Engine_groupBox.TabIndex = 2;
            this.DEBUG_Engine_groupBox.TabStop = false;
            this.DEBUG_Engine_groupBox.Text = "Engine";
            // 
            // DEBUG_Engine_rotateBody
            // 
            this.DEBUG_Engine_rotateBody.DecimalPlaces = 3;
            this.DEBUG_Engine_rotateBody.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.DEBUG_Engine_rotateBody.Location = new System.Drawing.Point(119, 49);
            this.DEBUG_Engine_rotateBody.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.DEBUG_Engine_rotateBody.Minimum = new decimal(new int[] {
            2147483647,
            0,
            0,
            -2147483648});
            this.DEBUG_Engine_rotateBody.Name = "DEBUG_Engine_rotateBody";
            this.DEBUG_Engine_rotateBody.Size = new System.Drawing.Size(161, 21);
            this.DEBUG_Engine_rotateBody.TabIndex = 11;
            // 
            // DEBUG_tb_rotateBody
            // 
            this.DEBUG_tb_rotateBody.Location = new System.Drawing.Point(287, 49);
            this.DEBUG_tb_rotateBody.Name = "DEBUG_tb_rotateBody";
            this.DEBUG_tb_rotateBody.Size = new System.Drawing.Size(100, 21);
            this.DEBUG_tb_rotateBody.TabIndex = 10;
            // 
            // DEBUG_btn_rotateBody
            // 
            this.DEBUG_btn_rotateBody.Location = new System.Drawing.Point(6, 48);
            this.DEBUG_btn_rotateBody.Name = "DEBUG_btn_rotateBody";
            this.DEBUG_btn_rotateBody.Size = new System.Drawing.Size(107, 23);
            this.DEBUG_btn_rotateBody.TabIndex = 9;
            this.DEBUG_btn_rotateBody.Text = "设置角度";
            this.DEBUG_btn_rotateBody.UseVisualStyleBackColor = true;
            this.DEBUG_btn_rotateBody.Click += new System.EventHandler(this.DEBUG_btn_rotateBody_Click);
            // 
            // DEBUG_tb_addImpulse
            // 
            this.DEBUG_tb_addImpulse.Location = new System.Drawing.Point(287, 21);
            this.DEBUG_tb_addImpulse.Name = "DEBUG_tb_addImpulse";
            this.DEBUG_tb_addImpulse.Size = new System.Drawing.Size(100, 21);
            this.DEBUG_tb_addImpulse.TabIndex = 8;
            // 
            // DEBUG_btn_addImpulse
            // 
            this.DEBUG_btn_addImpulse.Location = new System.Drawing.Point(6, 20);
            this.DEBUG_btn_addImpulse.Name = "DEBUG_btn_addImpulse";
            this.DEBUG_btn_addImpulse.Size = new System.Drawing.Size(107, 23);
            this.DEBUG_btn_addImpulse.TabIndex = 7;
            this.DEBUG_btn_addImpulse.Text = "施加线性冲量";
            this.DEBUG_btn_addImpulse.UseVisualStyleBackColor = true;
            this.DEBUG_btn_addImpulse.Click += new System.EventHandler(this.DEBUG_btn_addImpulse_Click);
            // 
            // DEBUG_Engine_addImpulseLabel
            // 
            this.DEBUG_Engine_addImpulseLabel.AutoSize = true;
            this.DEBUG_Engine_addImpulseLabel.Location = new System.Drawing.Point(194, 26);
            this.DEBUG_Engine_addImpulseLabel.Name = "DEBUG_Engine_addImpulseLabel";
            this.DEBUG_Engine_addImpulseLabel.Size = new System.Drawing.Size(11, 12);
            this.DEBUG_Engine_addImpulseLabel.TabIndex = 6;
            this.DEBUG_Engine_addImpulseLabel.Text = ",";
            // 
            // DEBUG_Engine_addImpulseY
            // 
            this.DEBUG_Engine_addImpulseY.DecimalPlaces = 3;
            this.DEBUG_Engine_addImpulseY.Increment = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.DEBUG_Engine_addImpulseY.Location = new System.Drawing.Point(205, 21);
            this.DEBUG_Engine_addImpulseY.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.DEBUG_Engine_addImpulseY.Minimum = new decimal(new int[] {
            2147483647,
            0,
            0,
            -2147483648});
            this.DEBUG_Engine_addImpulseY.Name = "DEBUG_Engine_addImpulseY";
            this.DEBUG_Engine_addImpulseY.Size = new System.Drawing.Size(75, 21);
            this.DEBUG_Engine_addImpulseY.TabIndex = 5;
            // 
            // DEBUG_Engine_addImpulseX
            // 
            this.DEBUG_Engine_addImpulseX.DecimalPlaces = 3;
            this.DEBUG_Engine_addImpulseX.Increment = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.DEBUG_Engine_addImpulseX.Location = new System.Drawing.Point(119, 21);
            this.DEBUG_Engine_addImpulseX.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.DEBUG_Engine_addImpulseX.Minimum = new decimal(new int[] {
            2147483647,
            0,
            0,
            -2147483648});
            this.DEBUG_Engine_addImpulseX.Name = "DEBUG_Engine_addImpulseX";
            this.DEBUG_Engine_addImpulseX.Size = new System.Drawing.Size(75, 21);
            this.DEBUG_Engine_addImpulseX.TabIndex = 4;
            // 
            // DEBUG_btn_manualGlobalUpdate
            // 
            this.DEBUG_btn_manualGlobalUpdate.Location = new System.Drawing.Point(8, 27);
            this.DEBUG_btn_manualGlobalUpdate.Name = "DEBUG_btn_manualGlobalUpdate";
            this.DEBUG_btn_manualGlobalUpdate.Size = new System.Drawing.Size(107, 23);
            this.DEBUG_btn_manualGlobalUpdate.TabIndex = 1;
            this.DEBUG_btn_manualGlobalUpdate.Text = "手动Update";
            this.DEBUG_btn_manualGlobalUpdate.UseVisualStyleBackColor = true;
            this.DEBUG_btn_manualGlobalUpdate.Click += new System.EventHandler(this.DEBUG_btn_manualGlobalUpdate_Click);
            // 
            // listRefreshTimer
            // 
            this.listRefreshTimer.Interval = 200;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 412);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.contentMenuStrip);
            this.MainMenuStrip = this.contentMenuStrip;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.contentMenuStrip.ResumeLayout(false);
            this.contentMenuStrip.PerformLayout();
            this.tabPageMain.ResumeLayout(false);
            this.tabPageMain.PerformLayout();
            this.tabControlMain.ResumeLayout(false);
            this.tabPageDebug.ResumeLayout(false);
            this.DEBUG_Engine_groupBox.ResumeLayout(false);
            this.DEBUG_Engine_groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DEBUG_Engine_rotateBody)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DEBUG_Engine_addImpulseY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DEBUG_Engine_addImpulseX)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip contentMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 菜单ToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.TabPage tabPageMain;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageDebug;
        private System.Windows.Forms.CheckBox cb_isRotate;
        private System.Windows.Forms.Button DEBUG_btn_manualGlobalUpdate;
        private System.Windows.Forms.Label IntPtrLabel;
        private System.Windows.Forms.TextBox IntPtrTextBox;
        private System.Windows.Forms.Button btn_AddIntPtr;
        private System.Windows.Forms.GroupBox DEBUG_Engine_groupBox;
        private System.Windows.Forms.Button DEBUG_btn_addImpulse;
        private System.Windows.Forms.Label DEBUG_Engine_addImpulseLabel;
        private System.Windows.Forms.NumericUpDown DEBUG_Engine_addImpulseY;
        private System.Windows.Forms.NumericUpDown DEBUG_Engine_addImpulseX;
        private System.Windows.Forms.TextBox DEBUG_tb_addImpulse;
        private System.Windows.Forms.ListView IntPtrListView;
        private System.Windows.Forms.ColumnHeader IntPtrListView_WindowName;
        private System.Windows.Forms.ColumnHeader IntPtrListView_WindowIntPtr;
        private System.Windows.Forms.ColumnHeader IntPtrListView_WindowRect;
        private System.Windows.Forms.Timer listRefreshTimer;
        private System.Windows.Forms.ComboBox cb_intPtrFrom;
        private System.Windows.Forms.Label cb_intPtrFromLabel;
        private System.Windows.Forms.ColumnHeader IntPtrListView_WindowIntPtrDummy;
        private System.Windows.Forms.TextBox DEBUG_tb_rotateBody;
        private System.Windows.Forms.Button DEBUG_btn_rotateBody;
        private System.Windows.Forms.NumericUpDown DEBUG_Engine_rotateBody;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助_关于ToolStripMenuItem;
    }
}

