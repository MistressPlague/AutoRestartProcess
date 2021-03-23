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

namespace Auto_Restart_Process
{
    public partial class AutoRestartForm : Form
    {
        public AutoRestartForm()
        {
            InitializeComponent();
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

            AddUpdateAppSettings("IsTicked", checkBox1.Checked.ToString());
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
            AddUpdateAppSettings("Interval", numericUpDown1.Value.ToString());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IsUserAdministrator();

            if (!string.IsNullOrEmpty(ReadSetting("Interval")))
            {
                numericUpDown1.Value = int.Parse(ReadSetting("Interval"));
            }

            if (!string.IsNullOrEmpty(ReadSetting("StartThis")))
            {
                textBox1.Text = ReadSetting("StartThis");
            }

            if (!string.IsNullOrEmpty(ReadSetting("IsTicked")))
            {
                checkBox1.Checked = bool.Parse(ReadSetting("IsTicked"));
            }

            if (!string.IsNullOrEmpty(ReadSetting("RunOnPCStartup")))
            {
                checkBox2.Checked = bool.Parse(ReadSetting("RunOnPCStartup"));
            }

            comboBox1.SelectedIndex = 0;
        }

        static string ReadSetting(string key)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                var settings = configFile.AppSettings.Settings;

                return settings[key].Value;
            }
            catch
            {
                return null;
            }
        }

        static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                var settings = configFile.AppSettings.Settings;

                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }

                configFile.Save(ConfigurationSaveMode.Modified);

                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            AddUpdateAppSettings("StartThis", textBox1.Text);
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

            AddUpdateAppSettings("RunOnPCStartup", checkBox2.Checked.ToString());
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
    }
}
