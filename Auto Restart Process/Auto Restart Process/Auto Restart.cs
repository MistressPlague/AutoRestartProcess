using Libraries;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DiscordWebhook;

namespace Auto_Restart_Process
{
    public partial class AutoRestartForm : Form
    {
        public AutoRestartForm()
        {
            Instance = this;

            InitializeComponent();
        }

        public static AutoRestartForm Instance;

        public ConfigLib<Configuration> Config;

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

            public List<ProgramToRestart> ProgramsToRestart = new();
        }

        public class ProgramToRestart
        {
            public decimal Interval;
            public string MaintainThis;
            public string Arguments;
            public bool CreateNoWindow;
            public int WindowStartState;
            public bool KillIfNotResponding;
            public decimal NotRespondingTime;
            public string WebhookPrefix;
            public string Webhook;
        }

        private class TempData
        {
            public Webhook webHook;
            public Process Proc;
            public int RestartCount = 0;
            public Stopwatch TimePassed = new();
            public Stopwatch HungTimePassed = new();
        }

        private Dictionary<ProgramToRestart, TempData> TempDataStorage = new();

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked && !RestartWorker.IsBusy)
            {
                RestartWorker.RunWorkerAsync();
            }
            else
            {
                RestartWorker.CancelAsync();
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
        }

        // ReSharper disable once MethodNameNotMeaningful
        public void Log(string text, Webhook webhook = null, string prefix = null)
        {
            try
            {
                LogBox.AppendText("[" + DateTime.Now.ToString("hh:MM:ss tt") + "] " + text + "\r\n");

                if (prefix == null)
                {
                    webhook?.Send(text);
                }
                else
                {
                    webhook?.Send($"{prefix} {text}");
                }
            }
            catch
            {

            }
        }

        private void RestartWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Log("RestartWorker Init!");

            try
            {
                foreach (var program in TempDataStorage)
                {
                    if (e.Cancel)
                    {
                        Log("Cancelled.");
                        return;
                    }

                    Task.Run(() =>
                    {
                        if (!program.Value.TimePassed.IsRunning)
                        {
                            program.Value.TimePassed.Start();
                        }

                        while (checkBox1.Checked)
                        {
                            if (e.Cancel)
                            {
                                Log("Cancelled.");
                                return;
                            }

                            if (program.Value.TimePassed.ElapsedMilliseconds >= (long)program.Key.Interval)
                            {
                                var Processes = Process.GetProcessesByName((Path.GetFileName(program.Key.MaintainThis) ?? "UnknownFileName").Replace((Path.GetExtension(program.Key.MaintainThis) ?? "UnknownExtension"), "")).Where(o => Path.GetDirectoryName(o?.MainModule?.FileName) == Path.GetDirectoryName(program.Key.MaintainThis)).ToArray();
                                if (Processes.Length > 0)
                                {
                                    program.Value.Proc = Processes[0];
                                    Log($"Existing {Path.GetFileNameWithoutExtension(program.Key.MaintainThis)} Found!");
                                    goto AlreadyStarted;
                                }
                                Log($"Restarting {Path.GetFileNameWithoutExtension(program.Key.MaintainThis)}!", program.Value.webHook, program.Key.WebhookPrefix);

                                program.Value.RestartCount++;

                                // Update Display
                                var entryindex = Entries.ToList().FindIndex(o => o.ProcessName == Path.GetFileNameWithoutExtension(program.Key.MaintainThis));

                                Entries[entryindex].RestartCount = program.Value.RestartCount;
                                Entries[entryindex].LastRestart = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                                dataGridView1.UpdateCellValue(1, entryindex);
                                dataGridView1.UpdateCellValue(2, entryindex);

                                var Info = new ProcessStartInfo
                                {
                                    FileName = program.Key.MaintainThis ?? "",
                                    WorkingDirectory = Path.GetDirectoryName(program.Key.MaintainThis) ?? Environment.CurrentDirectory,
                                    Arguments = program.Key.Arguments.Replace("%APPDIR%", Path.GetDirectoryName(program.Key.MaintainThis) ?? Environment.CurrentDirectory).Replace("%TIME%", DateTime.Now.ToString("dd MM yyyy - hh mm ss tt")),
                                    CreateNoWindow = program.Key.CreateNoWindow,
                                    WindowStyle = (ProcessWindowStyle)program.Key.WindowStartState
                                };

                                program.Value.Proc = Process.Start(Info);

                                Log($"{Path.GetFileNameWithoutExtension(program.Key.MaintainThis)} Started!");

                            AlreadyStarted:
                                while (program.Value.Proc != null && !program.Value.Proc.HasExited)
                                {
                                    if (program.Key.KillIfNotResponding)
                                    {
                                        if (program.Value.HungTimePassed.ElapsedMilliseconds >= program.Key.NotRespondingTime)
                                        {
                                            program.Value.HungTimePassed.Reset();
                                            program.Value.Proc.Kill();
                                            Log($"{Path.GetFileNameWithoutExtension(program.Key.MaintainThis)} Killed - Failed To Respond!");
                                            break;
                                        }
                                    }

                                    if (program.Value.Proc.HasExited)
                                    {
                                        break;
                                    }

                                    if (program.Key.KillIfNotResponding)
                                    {
                                        if (!program.Value.Proc.Responding)
                                        {
                                            program.Value.HungTimePassed.Start();
                                        }
                                        else
                                        {
                                            program.Value.HungTimePassed.Reset();
                                        }
                                    }
                                }

                                Log($"{Path.GetFileNameWithoutExtension(program.Key.MaintainThis)} Died" + (checkBox1.Checked ? " - Restarting Soon" : "") + "!");
                                program.Value.TimePassed.Restart();
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Log("Exception: " + ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using var popup = new Setup();

            if (popup.ShowDialog() == DialogResult.OK)
            {
                var entry = new ProgramToRestart
                {
                    MaintainThis = popup.MaintainThis.Text,
                    Arguments = popup.Arguments.Text,
                    CreateNoWindow = popup.CreateNoWindow.Checked,
                    Interval = popup.Interval.Value,
                    KillIfNotResponding = popup.KillIfNotResponding.Checked,
                    NotRespondingTime = popup.NotRespondingTime.Value,
                    WindowStartState = popup.WindowStartState.SelectedIndex,
                    WebhookPrefix = popup.WebhookPrefix.Text,
                    Webhook = popup.Webhook.Text
                };

                Config.InternalConfig.ProgramsToRestart.Add(entry);

                var data = new TempData();

                if (!string.IsNullOrEmpty(entry.Webhook))
                {
                    data.webHook = new Webhook(entry.Webhook);
                }

                TempDataStorage.Add(entry, data);

                Entries.Add(new Entry
                {
                    ProcessName = Path.GetFileNameWithoutExtension(popup.MaintainThis.Text),
                    RestartCount = 0
                });

                RestartWorker.CancelAsync();

                if (checkBox1.Checked)
                {
                    RestartWorker.RunWorkerAsync();
                }
            }
        }

        public class Entry
        {
            [DisplayName("Process Name")]
            public string ProcessName { get; set; }

            [DisplayName("Restart Count")]
            public int RestartCount { get; set; }

            [DisplayName("Last Restart")]
            public string LastRestart { get; set; }
        }

        public BindingList<Entry> Entries = new();

        private void AutoRestartForm_Load(object sender, EventArgs e)
        {
            Config = new ConfigLib<Configuration>(Environment.CurrentDirectory + "\\RestarterConfig.json");

            dataGridView1.DataSource = Entries;

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[0].FillWeight = 50;

            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].FillWeight = 20;

            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].FillWeight = 30;

            foreach (var entry in Config.InternalConfig.ProgramsToRestart)
            {
                var data = new TempData();

                if (!string.IsNullOrEmpty(entry.Webhook))
                {
                    data.webHook = new Webhook(entry.Webhook);
                }

                TempDataStorage.Add(entry, data);

                Entries.Add(new Entry
                {
                    ProcessName = Path.GetFileNameWithoutExtension(entry.MaintainThis),
                    RestartCount = 0
                });
            }
        }

        private void AutoRestartForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var procname = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            var restarts = int.Parse(dataGridView1.SelectedRows[0].Cells[1].Value.ToString());

            var program = Config.InternalConfig.ProgramsToRestart.First(o => Path.GetFileNameWithoutExtension(o.MaintainThis) == procname && TempDataStorage[o].RestartCount == restarts);

            TempDataStorage.Remove(program);
            Entries.Remove(Entries.FirstOrDefault(o => o.ProcessName == procname && o.RestartCount == restarts));
            Config.InternalConfig.ProgramsToRestart.Remove(program);

            RestartWorker.CancelAsync();

            if (checkBox1.Checked)
            {
                RestartWorker.RunWorkerAsync();
            }
        }
    }
}
