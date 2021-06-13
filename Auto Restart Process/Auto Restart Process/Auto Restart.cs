using Libraries;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security.Principal;
using System.Windows.Forms;

namespace Auto_Restart_Process
{
    public partial class AutoRestartForm : Form
    {
        public AutoRestartForm()
        {
            Instance = this;

            JsonConfig.LoadConfig(ref Config);

            InitializeComponent();
        }

        public static AutoRestartForm Instance;

        public Configuration Config = new Configuration();

        public class Configuration
        {
            public bool IsAutoRestarting
            {
                get => Instance != null && Instance.checkBox1.Checked;
                set
                {
                    try
                    {
                        Instance.checkBox1.Checked = value;
                    }
                    catch
                    {

                    }
                }
            }

            public decimal Interval
            {
                get => Instance.numericUpDown1.Value;
                set
                {
                    try
                    {
                        Instance.numericUpDown1.Value = value;
                    }
                    catch
                    {

                    }
                }
            }

            public bool RunOnStartup
            {
                get => Instance.checkBox2.Checked;
                set
                {
                    try
                    {
                        Instance.checkBox2.Checked = value;
                    }
                    catch
                    {

                    }
                }
            }

            public string MaintainThis
            {
                get => Instance.textBox1.Text;
                set
                {
                    try
                    {
                        Instance.textBox1.Text = value;
                    }
                    catch
                    {

                    }
                }
            }

            public string Arguments
            {
                get => Instance.textBox2.Text;
                set
                {
                    try
                    {
                        Instance.textBox2.Text = value;
                    }
                    catch
                    {

                    }
                }
            }

            public bool CreateNoWindow
            {
                get => Instance.checkBox3.Checked;
                set
                {
                    try
                    {
                        Instance.checkBox3.Checked = value;
                    }
                    catch
                    {

                    }
                }
            }

            public int WindowStartState
            {
                get => Instance.comboBox1.SelectedIndex;
                set
                {
                    try
                    {
                        Instance.comboBox1.SelectedIndex = value;
                    }
                    catch
                    {

                    }
                }
            }

            public Point Pos
            {
                get => Instance.Location;
                set
                {
                    try
                    {
                        Instance.Location = value;
                    }
                    catch
                    {

                    }
                }
            }

            public bool KillIfNotResponding
            {
                get => Instance.NotRespondingPref.Checked;
                set
                {
                    try
                    {
                        Instance.NotRespondingPref.Checked = value;
                    }
                    catch
                    {

                    }
                }
            }

            public decimal NotRespondingTime
            {
                get => Instance.numericUpDown2.Value;
                set
                {
                    try
                    {
                        Instance.numericUpDown2.Value = value;
                    }
                    catch
                    {

                    }
                }
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
            if (checkBox1.Checked && !RestartWorker.IsBusy)
            {
                RestartWorker.RunWorkerAsync();
            }

            if (HasInit)
            {
                JsonConfig.SaveConfig(Config);
            }
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
            if (HasInit)
            {
                JsonConfig.SaveConfig(Config);
            }
        }

        private bool HasInit = false;

        private void Form1_Load(object sender, EventArgs e)
        {
            IsUserAdministrator();

            comboBox1.SelectedIndex = 0;

            JsonConfig.LoadConfig(ref Config);

            HasInit = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (HasInit)
            {
                JsonConfig.SaveConfig(Config);
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            var rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (rk != null)
            {
                if (checkBox2.Checked)
                {
                    rk.SetValue(Application.ExecutablePath, "\"" + Application.ExecutablePath + "\"");
                }
                else
                {
                    rk.DeleteValue(Application.ExecutablePath, false);
                }
            }

            if (HasInit)
            {
                JsonConfig.SaveConfig(Config);
            }
        }

        // ReSharper disable once MethodNameNotMeaningful
        public void Log(string text)
        {
            try
            {
                LogBox.AppendText("[" + DateTime.Now.ToString("hh:MM:ss tt") + "] " + text + "\r\n");
            }
            catch
            {

            }
        }

        private Stopwatch TimePassed = new Stopwatch();
        private Stopwatch HungTimePassed = new Stopwatch();

        private Process Proc;

        private int RestartCount = 0;

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
                        var Processes = Process.GetProcessesByName((Path.GetFileName(textBox1.Text) ?? "UnknownFileName")
                            .Replace((Path.GetExtension(textBox1.Text) ?? "UnknownExtension"), ""));

                        if (Processes.Length > 0)
                        {
                            Proc = Processes[0];

                            Log("Existing Process Found!");

                            goto AlreadyStarted;
                        }

                        Log("Restarting!");

                        var Info = new ProcessStartInfo
                        {
                            FileName = textBox1.Text ?? "",
                            WorkingDirectory = Path.GetDirectoryName(textBox1.Text) ?? Environment.CurrentDirectory,
                            Arguments = textBox2.Text.Replace("%APPDIR%", Path.GetDirectoryName(textBox1.Text) ?? Environment.CurrentDirectory).Replace("%TIME%", DateTime.Now.ToString("dd MM yyyy - hh mm ss tt")),
                            CreateNoWindow = checkBox3.Checked,
                            WindowStyle = (ProcessWindowStyle)comboBox1.SelectedIndex
                        };

                        Proc = Process.Start(Info);

                        Log("Process Started!");

                    AlreadyStarted:
                        while (Proc != null && !Proc.HasExited)
                        {
                            if (NotRespondingPref.Checked)
                            {
                                if (HungTimePassed.ElapsedMilliseconds >= numericUpDown2.Value)
                                {
                                    HungTimePassed.Reset();
                                    Proc.Kill();
                                    break;
                                }
                            }

                            if (Proc.HasExited)
                            {
                                break;
                            }

                            if (NotRespondingPref.Checked)
                            {
                                if (!Proc.Responding)
                                {
                                    HungTimePassed.Start();
                                }
                                else
                                {
                                    HungTimePassed.Reset();
                                }
                            }
                        }

                        Log("Process Died" + (checkBox1.Checked ? " - Restarting Soon" : "") + "!");

                        TimePassed.Restart();

                        if (checkBox1.Checked)
                        {
                            RestartCount++;

                            label5.Text = "Restart Count: " + RestartCount;
                        }
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
            if (HasInit)
            {
                JsonConfig.SaveConfig(Config);
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (HasInit)
            {
                JsonConfig.SaveConfig(Config);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (HasInit)
            {
                JsonConfig.SaveConfig(Config);
            }
        }

        private void AutoRestartForm_LocationChanged(object sender, EventArgs e)
        {
            if (HasInit)
            {
                JsonConfig.SaveConfig(Config);
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (HasInit)
            {
                JsonConfig.SaveConfig(Config);
            }
        }

        private void NotRespondingPref_CheckedChanged(object sender, EventArgs e)
        {
            if (HasInit)
            {
                JsonConfig.SaveConfig(Config);
            }
        }
    }
}
