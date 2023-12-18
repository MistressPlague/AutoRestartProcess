using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Auto_Restart_Process
{
    public partial class Setup : Form
    {
        public Setup()
        {
            InitializeComponent();
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(MaintainThis.Text))
            {
                MessageBox.Show("Please Choose A Program First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            using var popup = new OpenFileDialog();

            if (popup.ShowDialog() == DialogResult.OK)
            {
                MaintainThis.Text = popup.FileName;
            }
        }

        private void Setup_Load(object sender, EventArgs e)
        {
            WindowStartState.SelectedIndex = 0;
        }
    }
}
