
namespace PlayWindow
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.AboutTextBox = new System.Windows.Forms.RichTextBox();
            this.ConfirmButton = new System.Windows.Forms.Button();
            this.AboutContent = new System.Windows.Forms.TabControl();
            this.AboutTab = new System.Windows.Forms.TabPage();
            this.ThankTab = new System.Windows.Forms.TabPage();
            this.ThankTextBox = new System.Windows.Forms.RichTextBox();
            this.HomeLink = new System.Windows.Forms.LinkLabel();
            this.AboutContent.SuspendLayout();
            this.AboutTab.SuspendLayout();
            this.ThankTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // AboutTextBox
            // 
            this.AboutTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.AboutTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AboutTextBox.Location = new System.Drawing.Point(3, 3);
            this.AboutTextBox.Name = "AboutTextBox";
            this.AboutTextBox.ReadOnly = true;
            this.AboutTextBox.Size = new System.Drawing.Size(407, 139);
            this.AboutTextBox.TabIndex = 0;
            this.AboutTextBox.Text = "Talk is Cheap,Show me the Code.\n\nBy 2333_not_found\nThanks for using!";
            // 
            // ConfirmButton
            // 
            this.ConfirmButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ConfirmButton.Location = new System.Drawing.Point(345, 177);
            this.ConfirmButton.Name = "ConfirmButton";
            this.ConfirmButton.Size = new System.Drawing.Size(75, 23);
            this.ConfirmButton.TabIndex = 1;
            this.ConfirmButton.Text = "确定(&O)";
            this.ConfirmButton.UseVisualStyleBackColor = true;
            this.ConfirmButton.Click += new System.EventHandler(this.ConfirmButton_Click);
            // 
            // AboutContent
            // 
            this.AboutContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AboutContent.Controls.Add(this.AboutTab);
            this.AboutContent.Controls.Add(this.ThankTab);
            this.AboutContent.Location = new System.Drawing.Point(0, 0);
            this.AboutContent.Name = "AboutContent";
            this.AboutContent.SelectedIndex = 0;
            this.AboutContent.Size = new System.Drawing.Size(421, 171);
            this.AboutContent.TabIndex = 2;
            this.AboutContent.SelectedIndexChanged += new System.EventHandler(this.AboutContent_SelectedIndexChanged);
            // 
            // AboutTab
            // 
            this.AboutTab.Controls.Add(this.AboutTextBox);
            this.AboutTab.Location = new System.Drawing.Point(4, 22);
            this.AboutTab.Name = "AboutTab";
            this.AboutTab.Padding = new System.Windows.Forms.Padding(3);
            this.AboutTab.Size = new System.Drawing.Size(413, 145);
            this.AboutTab.TabIndex = 0;
            this.AboutTab.Text = "关于";
            this.AboutTab.UseVisualStyleBackColor = true;
            // 
            // ThankTab
            // 
            this.ThankTab.Controls.Add(this.ThankTextBox);
            this.ThankTab.Location = new System.Drawing.Point(4, 22);
            this.ThankTab.Name = "ThankTab";
            this.ThankTab.Padding = new System.Windows.Forms.Padding(3);
            this.ThankTab.Size = new System.Drawing.Size(413, 145);
            this.ThankTab.TabIndex = 1;
            this.ThankTab.Text = "致谢";
            this.ThankTab.UseVisualStyleBackColor = true;
            // 
            // ThankTextBox
            // 
            this.ThankTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ThankTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ThankTextBox.Location = new System.Drawing.Point(3, 3);
            this.ThankTextBox.Name = "ThankTextBox";
            this.ThankTextBox.ReadOnly = true;
            this.ThankTextBox.Size = new System.Drawing.Size(407, 139);
            this.ThankTextBox.TabIndex = 1;
            this.ThankTextBox.Text = resources.GetString("ThankTextBox.Text");
            // 
            // HomeLink
            // 
            this.HomeLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.HomeLink.AutoSize = true;
            this.HomeLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.HomeLink.Location = new System.Drawing.Point(2, 182);
            this.HomeLink.Name = "HomeLink";
            this.HomeLink.Size = new System.Drawing.Size(329, 12);
            this.HomeLink.TabIndex = 3;
            this.HomeLink.TabStop = true;
            this.HomeLink.Text = "项目主页：https://github.com/2333-not-found/PlayWindow";
            this.HomeLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HomeLink_LinkClicked);
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 212);
            this.Controls.Add(this.HomeLink);
            this.Controls.Add(this.ConfirmButton);
            this.Controls.Add(this.AboutContent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "关于";
            this.AboutContent.ResumeLayout(false);
            this.AboutTab.ResumeLayout(false);
            this.ThankTab.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox AboutTextBox;
        private System.Windows.Forms.Button ConfirmButton;
        private System.Windows.Forms.TabControl AboutContent;
        private System.Windows.Forms.TabPage AboutTab;
        private System.Windows.Forms.TabPage ThankTab;
        private System.Windows.Forms.RichTextBox ThankTextBox;
        private System.Windows.Forms.LinkLabel HomeLink;
    }
}