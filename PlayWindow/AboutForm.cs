using System;
using System.Windows.Forms;

namespace PlayWindow
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AboutContent_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (AboutContent.SelectedIndex)
            {
                case 0:
                    this.Height = 300;
                    break;
                case 1:
                    this.Height = 600;
                    break;
                default:
                    this.Height = 300;
                    break;
            }
        }

        private void HomeLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/2333-not-found/PlayWindow");
        }
    }
}