namespace Auto_Restart_Process
{
    partial class AutoRestartForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoRestartForm));
            this.AutoRestartCheckBox = new System.Windows.Forms.CheckBox();
            this.RunOnStartupCheckBox = new System.Windows.Forms.CheckBox();
            this.FilePopup = new System.Windows.Forms.OpenFileDialog();
            this.LogBox = new System.Windows.Forms.TextBox();
            this.ProgramList = new System.Windows.Forms.DataGridView();
            this.AddButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.EditButton = new System.Windows.Forms.Button();
            this.NotificationAreaIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.NotificationAreaRightClickMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.ProgramList)).BeginInit();
            this.NotificationAreaRightClickMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // AutoRestartCheckBox
            // 
            this.AutoRestartCheckBox.AutoSize = true;
            this.AutoRestartCheckBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AutoRestartCheckBox.Location = new System.Drawing.Point(10, 12);
            this.AutoRestartCheckBox.Name = "AutoRestartCheckBox";
            this.AutoRestartCheckBox.Size = new System.Drawing.Size(85, 17);
            this.AutoRestartCheckBox.TabIndex = 1;
            this.AutoRestartCheckBox.Text = "Auto Restart";
            this.AutoRestartCheckBox.UseVisualStyleBackColor = true;
            this.AutoRestartCheckBox.CheckedChanged += new System.EventHandler(this.AutoRestartCheckBox_CheckedChanged);
            // 
            // RunOnStartupCheckBox
            // 
            this.RunOnStartupCheckBox.AutoSize = true;
            this.RunOnStartupCheckBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RunOnStartupCheckBox.Location = new System.Drawing.Point(10, 35);
            this.RunOnStartupCheckBox.Name = "RunOnStartupCheckBox";
            this.RunOnStartupCheckBox.Size = new System.Drawing.Size(100, 17);
            this.RunOnStartupCheckBox.TabIndex = 8;
            this.RunOnStartupCheckBox.Text = "Run On Startup";
            this.RunOnStartupCheckBox.UseVisualStyleBackColor = true;
            this.RunOnStartupCheckBox.CheckedChanged += new System.EventHandler(this.RunOnStartupCheckBox_CheckedChanged);
            // 
            // FilePopup
            // 
            this.FilePopup.RestoreDirectory = true;
            // 
            // LogBox
            // 
            this.LogBox.BackColor = System.Drawing.Color.Black;
            this.LogBox.ForeColor = System.Drawing.Color.Magenta;
            this.LogBox.Location = new System.Drawing.Point(10, 260);
            this.LogBox.Multiline = true;
            this.LogBox.Name = "LogBox";
            this.LogBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.LogBox.Size = new System.Drawing.Size(584, 155);
            this.LogBox.TabIndex = 12;
            // 
            // ProgramList
            // 
            this.ProgramList.AllowUserToAddRows = false;
            this.ProgramList.AllowUserToDeleteRows = false;
            this.ProgramList.AllowUserToResizeRows = false;
            this.ProgramList.BackgroundColor = System.Drawing.Color.Black;
            this.ProgramList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ProgramList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ProgramList.Location = new System.Drawing.Point(10, 87);
            this.ProgramList.MultiSelect = false;
            this.ProgramList.Name = "ProgramList";
            this.ProgramList.ReadOnly = true;
            this.ProgramList.RowHeadersVisible = false;
            this.ProgramList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ProgramList.Size = new System.Drawing.Size(584, 166);
            this.ProgramList.TabIndex = 16;
            // 
            // AddButton
            // 
            this.AddButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AddButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddButton.Location = new System.Drawing.Point(502, 58);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(92, 23);
            this.AddButton.TabIndex = 17;
            this.AddButton.Text = "Add";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // RemoveButton
            // 
            this.RemoveButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RemoveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemoveButton.Location = new System.Drawing.Point(10, 58);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(92, 23);
            this.RemoveButton.TabIndex = 18;
            this.RemoveButton.Text = "Remove";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // EditButton
            // 
            this.EditButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.EditButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EditButton.Location = new System.Drawing.Point(404, 58);
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(92, 23);
            this.EditButton.TabIndex = 19;
            this.EditButton.Text = "Edit";
            this.EditButton.UseVisualStyleBackColor = true;
            this.EditButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // NotificationAreaIcon
            // 
            this.NotificationAreaIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.NotificationAreaIcon.BalloonTipText = "Right Click To Exit";
            this.NotificationAreaIcon.BalloonTipTitle = "Auto Restart Process";
            this.NotificationAreaIcon.ContextMenuStrip = this.NotificationAreaRightClickMenu;
            this.NotificationAreaIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NotificationAreaIcon.Icon")));
            this.NotificationAreaIcon.Text = "Auto Restart Process";
            this.NotificationAreaIcon.DoubleClick += new System.EventHandler(this.NotificationAreaIcon_DoubleClick);
            // 
            // NotificationAreaRightClickMenu
            // 
            this.NotificationAreaRightClickMenu.BackColor = System.Drawing.Color.Black;
            this.NotificationAreaRightClickMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.NotificationAreaRightClickMenu.Name = "NotificationAreaRightClickMenu";
            this.NotificationAreaRightClickMenu.ShowImageMargin = false;
            this.NotificationAreaRightClickMenu.Size = new System.Drawing.Size(69, 26);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.ForeColor = System.Drawing.Color.Magenta;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(68, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // AutoRestartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(604, 427);
            this.Controls.Add(this.EditButton);
            this.Controls.Add(this.RemoveButton);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.ProgramList);
            this.Controls.Add(this.LogBox);
            this.Controls.Add(this.RunOnStartupCheckBox);
            this.Controls.Add(this.AutoRestartCheckBox);
            this.ForeColor = System.Drawing.Color.Magenta;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AutoRestartForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Auto Restart Process";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AutoRestartForm_FormClosing);
            this.Load += new System.EventHandler(this.AutoRestartForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ProgramList)).EndInit();
            this.NotificationAreaRightClickMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox AutoRestartCheckBox;
        private System.Windows.Forms.CheckBox RunOnStartupCheckBox;
        private System.Windows.Forms.OpenFileDialog FilePopup;
        private System.Windows.Forms.TextBox LogBox;
        private System.Windows.Forms.DataGridView ProgramList;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.Button EditButton;
        private System.Windows.Forms.NotifyIcon NotificationAreaIcon;
        private System.Windows.Forms.ContextMenuStrip NotificationAreaRightClickMenu;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    }
}

