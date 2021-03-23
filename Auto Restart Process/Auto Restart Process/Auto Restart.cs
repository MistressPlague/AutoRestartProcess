using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using Libraries;

namespace Auto_Restart_Process
{
    public partial class AutoRestartForm : Form
    {
        public AutoRestartForm()
        {
            InitializeComponent();

            Instance = this;
        }

        public static AutoRestartForm Instance;

        public Configuration Config = new Configuration();

        public class Configuration
        {
            public bool IsAutoRestarting
            {
                get => Instance.checkBox1.Checked;
                set => Instance.checkBox1.Checked = value;
            }

            public decimal Interval
            {
                get => Instance.numericUpDown1.Value;
                set => Instance.numericUpDown1.Value = value;
            }

            public bool RunOnStartup
            {
                get => Instance.checkBox2.Checked;
                set => Instance.checkBox2.Checked = value;
            }

            public string MaintainThis
            {
                get => Instance.textBox1.Text;
                set => Instance.textBox1.Text = value;
            }

            public string Arguments
            {
                get => Instance.textBox2.Text;
                set => Instance.textBox2.Text = value;
            }

            public bool CreateNoWindow
            {
                get => Instance.checkBox3.Checked;
                set => Instance.checkBox3.Checked = value;
            }

            public int WindowStartState
            {
                get => Instance.comboBox1.SelectedIndex;
                set => Instance.comboBox1.SelectedIndex = value;
            }
        }

        public static bool IsAdmin = false;

        public bool IsUserAdministrator()
        {
            WindowsIdentity user = null;
            try
            {
                //get the currently logged in user
                user = WindowsIdentity.GetCurrent();

                IsAdmin = new WindowsPrincipal(user).IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (UnauthorizedAccessException)
            {
                IsAdmin = false;
            }
            catch (Exception)
            {
                IsAdmin = false;
            }
            finally
            {
                if (user != null)
                {
                    user.Dispose();
                }
            }
            return IsAdmin;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                RestartWorker.RunWorkerAsync();
            }

            JsonConfig.SaveConfig(Config);
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Proc != null && !Proc.HasExited)
                {
                    Proc.Kill();
                }
            }
            catch
            {

            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            JsonConfig.SaveConfig(Config);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IsUserAdministrator();

            JsonConfig.LoadConfig(ref Config);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            JsonConfig.SaveConfig(Config);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (checkBox2.Checked)
            {
                rk.SetValue(Application.ExecutablePath, "\"" + Application.ExecutablePath + "\"");
            }
            else
            {
                rk.DeleteValue(Application.ExecutablePath, false);
            }

            JsonConfig.SaveConfig(Config);
        }

        public void Log(string text)
        {
            LogBox.AppendText("[" + DateTime.Now.ToString("hh:MM:ss tt") + "] " + text + "\r\n");
        }

        private Stopwatch TimePassed = new Stopwatch();

        private Process Proc;

        private void RestartWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Log("RestartWorker Init!");

            if (!TimePassed.IsRunning)
            {
                TimePassed.Start();
            }

            try
            {
                while (checkBox1.Checked)
                {
                    if (TimePassed.ElapsedMilliseconds >= (long)numericUpDown1.Value)
                    {
                        Log("Restarting!");

                        ProcessStartInfo Info = new ProcessStartInfo
                        {
                            FileName = textBox1.Text,
                            WorkingDirectory = Path.GetDirectoryName(textBox1.Text) ?? Environment.CurrentDirectory,
                            Arguments = textBox2.Text.Replace("%APPDIR%", Path.GetDirectoryName(textBox1.Text) ?? Environment.CurrentDirectory).Replace("%TIME%", DateTime.Now.ToString("dd MM ss tt")),
                            CreateNoWindow = checkBox3.Checked,
                            WindowStyle = (ProcessWindowStyle)comboBox1.SelectedIndex
                        };

                        Proc = Process.Start(Info);

                        Log("Process Started!");

                        Proc?.WaitForExit();

                        Log("Process Died" + (checkBox1.Checked ? " - Restarting Soon" : "") + "!");

                        TimePassed.Restart();
                    }
                }
            }
            catch (Exception ex)
            {
                Log("Exception: " + ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var Result = FilePopup.ShowDialog();

            if (Result == DialogResult.OK)
            {
                textBox1.Text = FilePopup.FileName.Replace("\\", "//");
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            JsonConfig.SaveConfig(Config);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            JsonConfig.SaveConfig(Config);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            JsonConfig.SaveConfig(Config);
        }
    }
}
