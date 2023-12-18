namespace Auto_Restart_Process
{
    partial class Setup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Setup));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.KillAfterTime = new System.Windows.Forms.NumericUpDown();
            this.KillAfter = new System.Windows.Forms.CheckBox();
            this.WebhookPrefix = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Webhook = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.NotRespondingTime = new System.Windows.Forms.NumericUpDown();
            this.KillIfNotResponding = new System.Windows.Forms.CheckBox();
            this.Interval = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.WindowStartState = new System.Windows.Forms.ComboBox();
            this.MaintainThis = new System.Windows.Forms.TextBox();
            this.CreateNoWindow = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.Arguments = new System.Windows.Forms.TextBox();
            this.ConfirmButton = new System.Windows.Forms.Button();
            this.AutoMinimize = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.KillAfterTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotRespondingTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Interval)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.AutoMinimize);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.KillAfterTime);
            this.groupBox1.Controls.Add(this.KillAfter);
            this.groupBox1.Controls.Add(this.WebhookPrefix);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.Webhook);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.NotRespondingTime);
            this.groupBox1.Controls.Add(this.KillIfNotResponding);
            this.groupBox1.Controls.Add(this.Interval);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.WindowStartState);
            this.groupBox1.Controls.Add(this.MaintainThis);
            this.groupBox1.Controls.Add(this.CreateNoWindow);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.BrowseButton);
            this.groupBox1.Controls.Add(this.Arguments);
            this.groupBox1.Location = new System.Drawing.Point(12, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(465, 233);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 179);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(120, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "Kill After Time (Minutes):";
            // 
            // KillAfterTime
            // 
            this.KillAfterTime.BackColor = System.Drawing.Color.Black;
            this.KillAfterTime.Cursor = System.Windows.Forms.Cursors.Hand;
            this.KillAfterTime.DecimalPlaces = 1;
            this.KillAfterTime.ForeColor = System.Drawing.Color.Magenta;
            this.KillAfterTime.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.KillAfterTime.Location = new System.Drawing.Point(143, 177);
            this.KillAfterTime.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.KillAfterTime.Name = "KillAfterTime";
            this.KillAfterTime.Size = new System.Drawing.Size(121, 20);
            this.KillAfterTime.TabIndex = 24;
            this.KillAfterTime.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // KillAfter
            // 
            this.KillAfter.AutoSize = true;
            this.KillAfter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.KillAfter.Location = new System.Drawing.Point(9, 159);
            this.KillAfter.Name = "KillAfter";
            this.KillAfter.Size = new System.Drawing.Size(94, 17);
            this.KillAfter.TabIndex = 25;
            this.KillAfter.Text = "Kill After Delay";
            this.KillAfter.UseVisualStyleBackColor = true;
            // 
            // WebhookPrefix
            // 
            this.WebhookPrefix.BackColor = System.Drawing.Color.Black;
            this.WebhookPrefix.ForeColor = System.Drawing.Color.Magenta;
            this.WebhookPrefix.Location = new System.Drawing.Point(165, 203);
            this.WebhookPrefix.Name = "WebhookPrefix";
            this.WebhookPrefix.Size = new System.Drawing.Size(142, 20);
            this.WebhookPrefix.TabIndex = 22;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 207);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(151, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Webhook Alerts: (Prefix | URL)";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Webhook
            // 
            this.Webhook.BackColor = System.Drawing.Color.Black;
            this.Webhook.ForeColor = System.Drawing.Color.Magenta;
            this.Webhook.Location = new System.Drawing.Point(313, 203);
            this.Webhook.Name = "Webhook";
            this.Webhook.Size = new System.Drawing.Size(142, 20);
            this.Webhook.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Interval:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 135);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(136, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Max Not Responding Time:";
            // 
            // NotRespondingTime
            // 
            this.NotRespondingTime.BackColor = System.Drawing.Color.Black;
            this.NotRespondingTime.Cursor = System.Windows.Forms.Cursors.Hand;
            this.NotRespondingTime.ForeColor = System.Drawing.Color.Magenta;
            this.NotRespondingTime.Location = new System.Drawing.Point(143, 133);
            this.NotRespondingTime.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.NotRespondingTime.Name = "NotRespondingTime";
            this.NotRespondingTime.Size = new System.Drawing.Size(121, 20);
            this.NotRespondingTime.TabIndex = 16;
            this.NotRespondingTime.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // KillIfNotResponding
            // 
            this.KillIfNotResponding.AutoSize = true;
            this.KillIfNotResponding.Cursor = System.Windows.Forms.Cursors.Hand;
            this.KillIfNotResponding.Location = new System.Drawing.Point(9, 115);
            this.KillIfNotResponding.Name = "KillIfNotResponding";
            this.KillIfNotResponding.Size = new System.Drawing.Size(262, 17);
            this.KillIfNotResponding.TabIndex = 16;
            this.KillIfNotResponding.Text = "Kill And Restart On Not Responding For Too Long";
            this.KillIfNotResponding.UseVisualStyleBackColor = true;
            // 
            // Interval
            // 
            this.Interval.BackColor = System.Drawing.Color.Black;
            this.Interval.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Interval.ForeColor = System.Drawing.Color.Magenta;
            this.Interval.Location = new System.Drawing.Point(53, 13);
            this.Interval.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.Interval.Name = "Interval";
            this.Interval.Size = new System.Drawing.Size(69, 20);
            this.Interval.TabIndex = 18;
            this.Interval.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(131, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Start In Window State:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // WindowStartState
            // 
            this.WindowStartState.BackColor = System.Drawing.Color.Black;
            this.WindowStartState.Cursor = System.Windows.Forms.Cursors.Hand;
            this.WindowStartState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.WindowStartState.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.WindowStartState.ForeColor = System.Drawing.Color.Magenta;
            this.WindowStartState.FormattingEnabled = true;
            this.WindowStartState.Items.AddRange(new object[] {
            "Normal",
            "Hidden",
            "Minimized",
            "Maximized"});
            this.WindowStartState.Location = new System.Drawing.Point(251, 90);
            this.WindowStartState.Name = "WindowStartState";
            this.WindowStartState.Size = new System.Drawing.Size(204, 21);
            this.WindowStartState.TabIndex = 14;
            // 
            // MaintainThis
            // 
            this.MaintainThis.BackColor = System.Drawing.Color.Black;
            this.MaintainThis.ForeColor = System.Drawing.Color.Magenta;
            this.MaintainThis.Location = new System.Drawing.Point(85, 39);
            this.MaintainThis.Name = "MaintainThis";
            this.MaintainThis.Size = new System.Drawing.Size(273, 20);
            this.MaintainThis.TabIndex = 0;
            // 
            // CreateNoWindow
            // 
            this.CreateNoWindow.AutoSize = true;
            this.CreateNoWindow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CreateNoWindow.Location = new System.Drawing.Point(9, 92);
            this.CreateNoWindow.Name = "CreateNoWindow";
            this.CreateNoWindow.Size = new System.Drawing.Size(116, 17);
            this.CreateNoWindow.TabIndex = 13;
            this.CreateNoWindow.Text = "Create No Window";
            this.CreateNoWindow.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Maintain This:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Arguments:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BrowseButton
            // 
            this.BrowseButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BrowseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BrowseButton.Location = new System.Drawing.Point(364, 37);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(92, 23);
            this.BrowseButton.TabIndex = 9;
            this.BrowseButton.Text = "Browse";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // Arguments
            // 
            this.Arguments.BackColor = System.Drawing.Color.Black;
            this.Arguments.ForeColor = System.Drawing.Color.Magenta;
            this.Arguments.Location = new System.Drawing.Point(85, 66);
            this.Arguments.Name = "Arguments";
            this.Arguments.Size = new System.Drawing.Size(371, 20);
            this.Arguments.TabIndex = 10;
            // 
            // ConfirmButton
            // 
            this.ConfirmButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ConfirmButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ConfirmButton.Location = new System.Drawing.Point(12, 246);
            this.ConfirmButton.Name = "ConfirmButton";
            this.ConfirmButton.Size = new System.Drawing.Size(465, 40);
            this.ConfirmButton.TabIndex = 17;
            this.ConfirmButton.Text = "Confirm";
            this.ConfirmButton.UseVisualStyleBackColor = true;
            this.ConfirmButton.Click += new System.EventHandler(this.ConfirmButton_Click);
            // 
            // AutoMinimize
            // 
            this.AutoMinimize.AutoSize = true;
            this.AutoMinimize.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AutoMinimize.Location = new System.Drawing.Point(364, 159);
            this.AutoMinimize.Name = "AutoMinimize";
            this.AutoMinimize.Size = new System.Drawing.Size(91, 17);
            this.AutoMinimize.TabIndex = 26;
            this.AutoMinimize.Text = "Auto-Minimize";
            this.AutoMinimize.UseVisualStyleBackColor = true;
            // 
            // Setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(489, 298);
            this.Controls.Add(this.ConfirmButton);
            this.Controls.Add(this.groupBox1);
            this.ForeColor = System.Drawing.Color.Magenta;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Setup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Setup";
            this.Load += new System.EventHandler(this.Setup_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.KillAfterTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotRespondingTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Interval)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.Button ConfirmButton;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.NumericUpDown NotRespondingTime;
        public System.Windows.Forms.CheckBox KillIfNotResponding;
        public System.Windows.Forms.ComboBox WindowStartState;
        public System.Windows.Forms.TextBox MaintainThis;
        public System.Windows.Forms.CheckBox CreateNoWindow;
        public System.Windows.Forms.TextBox Arguments;
        public System.Windows.Forms.NumericUpDown Interval;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.TextBox Webhook;
        public System.Windows.Forms.TextBox WebhookPrefix;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.NumericUpDown KillAfterTime;
        public System.Windows.Forms.CheckBox KillAfter;
        public System.Windows.Forms.CheckBox AutoMinimize;
    }
}