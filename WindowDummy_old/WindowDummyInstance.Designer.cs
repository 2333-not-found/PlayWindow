
namespace WindowDummy
{
    partial class WindowDummyInstance
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
            this.IntPtrLabel = new System.Windows.Forms.Label();
            this.IntPtrLabel_p = new System.Windows.Forms.Label();
            this.RECTLabel_p = new System.Windows.Forms.Label();
            this.WindowPic = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.WindowPic)).BeginInit();
            this.SuspendLayout();
            // 
            // IntPtrLabel
            // 
            this.IntPtrLabel.AutoSize = true;
            this.IntPtrLabel.Location = new System.Drawing.Point(185, 266);
            this.IntPtrLabel.Name = "IntPtrLabel";
            this.IntPtrLabel.Size = new System.Drawing.Size(47, 12);
            this.IntPtrLabel.TabIndex = 0;
            this.IntPtrLabel.Text = "IntPtr:";
            // 
            // IntPtrLabel_p
            // 
            this.IntPtrLabel_p.AutoSize = true;
            this.IntPtrLabel_p.Location = new System.Drawing.Point(315, 266);
            this.IntPtrLabel_p.Name = "IntPtrLabel_p";
            this.IntPtrLabel_p.Size = new System.Drawing.Size(95, 12);
            this.IntPtrLabel_p.TabIndex = 2;
            this.IntPtrLabel_p.Text = "父窗口的IntPtr:";
            // 
            // RECTLabel_p
            // 
            this.RECTLabel_p.AutoSize = true;
            this.RECTLabel_p.Location = new System.Drawing.Point(185, 278);
            this.RECTLabel_p.Name = "RECTLabel_p";
            this.RECTLabel_p.Size = new System.Drawing.Size(83, 12);
            this.RECTLabel_p.TabIndex = 3;
            this.RECTLabel_p.Text = "父窗口的RECT:";
            // 
            // WindowPic
            // 
            this.WindowPic.BackColor = System.Drawing.SystemColors.Control;
            this.WindowPic.Location = new System.Drawing.Point(0, 0);
            this.WindowPic.Name = "WindowPic";
            this.WindowPic.Size = new System.Drawing.Size(600, 300);
            this.WindowPic.TabIndex = 4;
            this.WindowPic.TabStop = false;
            this.WindowPic.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WindowPic_MouseDown);
            this.WindowPic.MouseUp += new System.Windows.Forms.MouseEventHandler(this.WindowPic_MouseUp);
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(457, 266);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(85, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "手动旋转30度";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // WindowDummyInstance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(600, 300);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.RECTLabel_p);
            this.Controls.Add(this.IntPtrLabel_p);
            this.Controls.Add(this.IntPtrLabel);
            this.Controls.Add(this.WindowPic);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "WindowDummyInstance";
            this.Text = "Form1";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.WindowDummyInstance_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WindowDummyInstance_MouseDown);
            this.MouseLeave += new System.EventHandler(this.WindowDummyInstance_MouseLeave);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.WindowDummyInstance_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.WindowPic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label IntPtrLabel;
        private System.Windows.Forms.Label IntPtrLabel_p;
        private System.Windows.Forms.Label RECTLabel_p;
        private System.Windows.Forms.PictureBox WindowPic;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
    }
}

