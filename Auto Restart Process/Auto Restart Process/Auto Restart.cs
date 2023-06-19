using Libraries;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DiscordWebhook;
using System.Runtime.InteropServices;
using System.Text;
using System.Runtime.InteropServices.ComTypes;

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

                foreach (var canceller in CancelTokens)
                {
                    canceller?.Cancel();
                }

                CancelTokens.Clear();
            }
        }

        [ComImport]
        [Guid("00021401-0000-0000-C000-000000000046")]
        internal class ShellLink
        {
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("000214F9-0000-0000-C000-000000000046")]
        internal interface IShellLink
        {
            void GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, out IntPtr pfd, int fFlags);
            void GetIDList(out IntPtr ppidl);
            void SetIDList(IntPtr pidl);
            void GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);
            void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
            void GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
            void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
            void GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
            void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
            void GetHotkey(out short pwHotkey);
            void SetHotkey(short wHotkey);
            void GetShowCmd(out int piShowCmd);
            void SetShowCmd(int iShowCmd);
            void GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cchIconPath, out int piIcon);
            void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
            void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);
            void Resolve(IntPtr hwnd, int fFlags);
            void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

            if (checkBox2.Checked)
            {
                var link = (IShellLink)new ShellLink();

                // setup shortcut information
                link.SetDescription("Auto Restart Process AutoStart");
                link.SetPath(Application.ExecutablePath);

                // save it
                var file = (IPersistFile)link;
                file.Save(Path.Combine(dir, "AutoRestartProcess.lnk"), false);

                Log($"Saved Shortcut To: {dir}");
            }
            else
            {
                File.Delete(Path.Combine(dir, "AutoRestartProcess.lnk"));

                Log($"Deleted Shortcut: {Path.Combine(dir, "AutoRestartProcess.lnk")}");
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

        private List<CancellationTokenSource> CancelTokens = new List<CancellationTokenSource>();

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

                    var canceller = new CancellationTokenSource();
                    CancelTokens.Add(canceller);

                    Task.Run(() =>
                    {
                        if (!program.Value.TimePassed.IsRunning)
                        {
                            program.Value.TimePassed.Start();
                        }

                        while (checkBox1.Checked)
                        {
                            if (e.Cancel || canceller.IsCancellationRequested || canceller.Token.IsCancellationRequested)
                            {
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
                                            Log($"{Path.GetFileNameWithoutExtension(program.Key.MaintainThis)} Killed - Failed To Respond!", program.Value.webHook, program.Key.WebhookPrefix);
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

                                if (e.Cancel || canceller.IsCancellationRequested || canceller.Token.IsCancellationRequested)
                                {
                                    return;
                                }

                                Log($"{Path.GetFileNameWithoutExtension(program.Key.MaintainThis)} Died" + (checkBox1.Checked ? " - Restarting Soon" : "") + "!");
                                program.Value.TimePassed.Restart();
                            }
                        }
                    }, canceller.Token);
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

                foreach (var canceller in CancelTokens)
                {
                    canceller?.Cancel();
                }

                CancelTokens.Clear();

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
            checkBox2.Checked = File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "AutoRestartProcess.lnk"));

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

            foreach (var canceller in CancelTokens)
            {
                canceller?.Cancel();
            }

            CancelTokens.Clear();

            if (checkBox1.Checked)
            {
                RestartWorker.RunWorkerAsync();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using var popup = new Setup();

            var procname = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            var restarts = int.Parse(dataGridView1.SelectedRows[0].Cells[1].Value.ToString());

            var program = Config.InternalConfig.ProgramsToRestart.First(o => Path.GetFileNameWithoutExtension(o.MaintainThis) == procname && TempDataStorage[o].RestartCount == restarts);
            var programIndex = Config.InternalConfig.ProgramsToRestart.IndexOf(program);

            popup.MaintainThis.Text = program.MaintainThis;
            popup.Arguments.Text = program.Arguments;
            popup.CreateNoWindow.Checked = program.CreateNoWindow;
            popup.Interval.Value = program.Interval;
            popup.KillIfNotResponding.Checked = program.KillIfNotResponding;
            popup.NotRespondingTime.Value = program.NotRespondingTime;
            popup.WindowStartState.SelectedIndex = program.WindowStartState;
            popup.WebhookPrefix.Text = program.WebhookPrefix;
            popup.Webhook.Text = program.Webhook;

            if (popup.ShowDialog() == DialogResult.OK)
            {
                // Remove Temp
                TempDataStorage.Remove(program);

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

                Config.InternalConfig.ProgramsToRestart[programIndex] = entry;

                var data = new TempData();

                data.RestartCount = restarts;

                if (!string.IsNullOrEmpty(entry.Webhook))
                {
                    data.webHook = new Webhook(entry.Webhook);
                }

                TempDataStorage.Add(entry, data);

                var entryindex = Entries.ToList().FindIndex(o => o.ProcessName == Path.GetFileNameWithoutExtension(program.MaintainThis));

                var gridentry = Entries[entryindex];

                gridentry.ProcessName = Path.GetFileNameWithoutExtension(popup.MaintainThis.Text);

                dataGridView1.UpdateCellValue(0, entryindex);

                RestartWorker.CancelAsync();

                foreach (var canceller in CancelTokens)
                {
                    canceller?.Cancel();
                }

                CancelTokens.Clear();

                if (checkBox1.Checked)
                {
                    RestartWorker.RunWorkerAsync();
                }
            }
        }
    }
}
