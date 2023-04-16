
namespace PlayWindow
{
    partial class QuickAddForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_Hook = new System.Windows.Forms.Button();
            this.tb_intPtr = new System.Windows.Forms.TextBox();
            this.btn_Add = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_Hook
            // 
            this.btn_Hook.Location = new System.Drawing.Point(1, 1);
            this.btn_Hook.Name = "btn_Hook";
            this.btn_Hook.Size = new System.Drawing.Size(75, 23);
            this.btn_Hook.TabIndex = 0;
            this.btn_Hook.Text = "Start";
            this.btn_Hook.UseVisualStyleBackColor = true;
            this.btn_Hook.Click += new System.EventHandler(this.btn_Hook_Click);
            // 
            // tb_intPtr
            // 
            this.tb_intPtr.Location = new System.Drawing.Point(1, 26);
            this.tb_intPtr.Name = "tb_intPtr";
            this.tb_intPtr.Size = new System.Drawing.Size(75, 21);
            this.tb_intPtr.TabIndex = 1;
            // 
            // btn_Add
            // 
            this.btn_Add.Location = new System.Drawing.Point(1, 50);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(75, 23);
            this.btn_Add.TabIndex = 2;
            this.btn_Add.Text = "Add";
            this.btn_Add.UseVisualStyleBackColor = true;
            this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // QuickAddForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(77, 74);
            this.Controls.Add(this.btn_Add);
            this.Controls.Add(this.tb_intPtr);
            this.Controls.Add(this.btn_Hook);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "QuickAddForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "QuickAddForm";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.QuickAddForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Hook;
        private System.Windows.Forms.TextBox tb_intPtr;
        private System.Windows.Forms.Button btn_Add;
    }
}