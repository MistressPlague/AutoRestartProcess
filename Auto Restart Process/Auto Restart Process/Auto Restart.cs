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
using Newtonsoft.Json;
using static Auto_Restart_Process.AutoRestartForm;

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
                get => Instance != null && Instance.AutoRestartCheckBox.Checked;
                set
                {
                    try
                    {
                        Instance.AutoRestartCheckBox.Checked = value;
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
            public bool KillAfter;
            public decimal KillAfterTime;
            public string WebhookPrefix;
            public string Webhook;

            public bool AutoMinimize;
        }

        private class TempData
        {
            public System.Windows.Forms.Timer timer;
            public Webhook webHook;
            public Process Proc;
            public int RestartCount = 0;
            //public Stopwatch TimePassed = new();
            public Stopwatch HungTimePassed = new();
            public Stopwatch RunTimePassed = new();

            public bool WasMinimized;
        }

        /// <summary>
        /// Temp Shit, What Is Handled
        /// </summary>
        private Dictionary<ProgramToRestart, TempData> TempDataStorage = new();

        private void AutoRestartCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (AutoRestartCheckBox.Checked)
            {
                StartAllTimers();
            }
            else
            {
                StopAllTimers();
            }
        }

        private void StopAllTimers()
        {
            foreach (var entry in TempDataStorage)
            {
                entry.Value.timer?.Stop();
                entry.Value.timer?.Dispose();
                entry.Value.timer = null;
            }
        }

        private void StartAllTimers()
        {
            foreach (var entry in TempDataStorage)
            {
                entry.Value.timer = new System.Windows.Forms.Timer();
                entry.Value.timer.Interval = (int)entry.Key.Interval;
                entry.Value.timer.Tick += (_, _) =>
                {
                    HandleProgram(entry);
                };
                entry.Value.timer.Start();
            }
        }

        private void RunOnStartupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

            if (RunOnStartupCheckBox.Checked)
            {
                if (File.Exists(Path.Combine(dir, "AutoRestartProcess.lnk")))
                {
                    return;
                }

                var link = (IShellLink)new ShellLink();

                // setup shortcut information
                link.SetDescription("Auto Restart Process AutoStart");
                link.SetPath(Application.ExecutablePath);
                link.SetWorkingDirectory(Path.GetDirectoryName(Application.ExecutablePath));

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

        private void HandleProgram(KeyValuePair<ProgramToRestart, TempData> program)
        {
            if (program.Value.Proc != null)
            {
                goto AlreadyStarted;
            } // If false, DoStart will auto occur

            var Processes = Process.GetProcessesByName((Path.GetFileNameWithoutExtension(program.Key.MaintainThis) ?? "UnknownFileName")).Where(o => Path.GetDirectoryName(o?.MainModule?.FileName) == Path.GetDirectoryName(program.Key.MaintainThis)).ToArray();

            if (Processes.Length > 0)
            {
                if (!Processes[0].HasExited)
                {
                    program.Value.Proc = Processes[0];
                    Log($"Existing {Path.GetFileNameWithoutExtension(program.Key.MaintainThis)} Found!");
                    program.Value.RunTimePassed.Start();
                    goto AlreadyStarted;
                }

                try
                {
                    Processes[0].Kill();
                }
                catch
                {

                }
            }

            Log($"Restarting {Path.GetFileNameWithoutExtension(program.Key.MaintainThis)}!", program.Value.webHook, program.Key.WebhookPrefix);

            program.Value.RestartCount++;

            // Update Display
            var entryindex = Entries.ToList().FindIndex(o => o.ProcessName == Path.GetFileNameWithoutExtension(program.Key.MaintainThis));

            Entries[entryindex].RestartCount = program.Value.RestartCount;
            Entries[entryindex].LastRestart = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
            ProgramList.UpdateCellValue(1, entryindex);
            ProgramList.UpdateCellValue(2, entryindex);

            var Info = new ProcessStartInfo
            {
                FileName = program.Key.MaintainThis ?? "",
                WorkingDirectory = Path.GetDirectoryName(program.Key.MaintainThis) ?? Environment.CurrentDirectory,
                Arguments = program.Key.Arguments.Replace("%APPDIR%", Path.GetDirectoryName(program.Key.MaintainThis) ?? Environment.CurrentDirectory).Replace("%TIME%", DateTime.Now.ToString("dd MM yyyy - hh mm ss tt")),
                CreateNoWindow = program.Key.CreateNoWindow,
                WindowStyle = (ProcessWindowStyle)program.Key.WindowStartState
            };

            program.Value.WasMinimized = false;

            program.Value.Proc = Process.Start(Info);

            Log($"{Path.GetFileNameWithoutExtension(program.Key.MaintainThis)} Started!");

            program.Value.RunTimePassed.Start();

            return;

            AlreadyStarted:
            if (program.Key.AutoMinimize && !program.Value.WasMinimized)
            {
                // Minimize
                Log($"Minimizing {Path.GetFileNameWithoutExtension(program.Key.MaintainThis)}");
                program.Value.WasMinimized = true;
                Native.ShowWindow(program.Value.Proc.MainWindowHandle, Native.ShowWindowCommand.SW_MINIMIZE);
            }

            if (program.Value.Proc.HasExited)
            {
                program.Value.Proc = null;
                goto RestartLog;
            }

            if (program.Key.KillAfter)
            {
                if (program.Value.RunTimePassed.ElapsedMilliseconds >= program.Key.KillAfterTime)
                {
                    program.Value.Proc.Kill();
                    program.Value.Proc = null;
                    Log($"{Path.GetFileNameWithoutExtension(program.Key.MaintainThis)} Killed - Kill After Time Passed!", program.Value.webHook, program.Key.WebhookPrefix);
                    goto RestartLog;
                }
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

                if (program.Value.HungTimePassed.ElapsedMilliseconds >= program.Key.NotRespondingTime)
                {
                    program.Value.HungTimePassed.Reset();
                    program.Value.Proc.Kill();
                    program.Value.Proc = null;
                    Log($"{Path.GetFileNameWithoutExtension(program.Key.MaintainThis)} Killed - Failed To Respond!", program.Value.webHook, program.Key.WebhookPrefix);
                    goto RestartLog;
                }
            }

            return;

            RestartLog:
            program.Value.WasMinimized = false;

            Log($"{Path.GetFileNameWithoutExtension(program.Key.MaintainThis)} Died" + (AutoRestartCheckBox.Checked ? " - Restarting Soon" : "") + $"!\r\n{program.Key.GetLastErrorFromEventLog()}");

            program.Value.RunTimePassed.Reset();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            using var popup = new Setup();

            if (popup.ShowDialog() == DialogResult.OK)
            {
                StopAllTimers();

                var entry = new ProgramToRestart
                {
                    MaintainThis = popup.MaintainThis.Text,
                    Arguments = popup.Arguments.Text,
                    CreateNoWindow = popup.CreateNoWindow.Checked,
                    Interval = popup.Interval.Value,
                    KillIfNotResponding = popup.KillIfNotResponding.Checked,
                    NotRespondingTime = popup.NotRespondingTime.Value,
                    KillAfter = popup.KillAfter.Checked,
                    KillAfterTime = popup.KillAfterTime.Value * 60000,
                    WindowStartState = popup.WindowStartState.SelectedIndex,
                    WebhookPrefix = popup.WebhookPrefix.Text,
                    Webhook = popup.Webhook.Text,
                    AutoMinimize = popup.AutoMinimize.Checked
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

                if (AutoRestartCheckBox.Checked)
                {
                    StartAllTimers();
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

        /// <summary>
        /// UI
        /// </summary>
        public BindingList<Entry> Entries = new();

        private void AutoRestartForm_Load(object sender, EventArgs e)
        {
            RunOnStartupCheckBox.Checked = File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "AutoRestartProcess.lnk"));

            Config = new ConfigLib<Configuration>(Environment.CurrentDirectory + "\\RestarterConfig.json");

            ProgramList.DataSource = Entries;

            ProgramList.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ProgramList.Columns[0].FillWeight = 50;

            ProgramList.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ProgramList.Columns[1].FillWeight = 20;

            ProgramList.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ProgramList.Columns[2].FillWeight = 30;

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

            if (AutoRestartCheckBox.Checked)
            {
                StartAllTimers();
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            StopAllTimers();

            var procname = ProgramList.SelectedRows[0].Cells[0].Value.ToString();
            var restarts = int.Parse(ProgramList.SelectedRows[0].Cells[1].Value.ToString());

            var program = Config.InternalConfig.ProgramsToRestart.First(o => Path.GetFileNameWithoutExtension(o.MaintainThis) == procname && TempDataStorage[o].RestartCount == restarts);

            Log($"Removing: {Path.GetFileNameWithoutExtension(program.MaintainThis)}..");

            TempDataStorage.Remove(TempDataStorage.First(o => o.Key == program).Key);
            Config.InternalConfig.ProgramsToRestart.Remove(program);
            Entries.Remove(Entries.First(o => o.ProcessName == procname && o.RestartCount == restarts));

            Log("Done.");

            if (AutoRestartCheckBox.Checked)
            {
                StartAllTimers();
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            using var popup = new Setup();

            var procname = ProgramList.SelectedRows[0].Cells[0].Value.ToString();
            var restarts = int.Parse(ProgramList.SelectedRows[0].Cells[1].Value.ToString());

            var program = Config.InternalConfig.ProgramsToRestart.First(o => Path.GetFileNameWithoutExtension(o.MaintainThis) == procname && TempDataStorage[o].RestartCount == restarts);
            var programIndex = Config.InternalConfig.ProgramsToRestart.IndexOf(program);

            popup.MaintainThis.Text = program.MaintainThis;
            popup.Arguments.Text = program.Arguments;
            popup.CreateNoWindow.Checked = program.CreateNoWindow;
            popup.Interval.Value = program.Interval;
            popup.KillIfNotResponding.Checked = program.KillIfNotResponding;
            popup.NotRespondingTime.Value = program.NotRespondingTime;
            popup.KillAfter.Checked = program.KillAfter;
            popup.KillAfterTime.Value = program.KillAfterTime / 60000;
            popup.WindowStartState.SelectedIndex = program.WindowStartState;
            popup.WebhookPrefix.Text = program.WebhookPrefix;
            popup.Webhook.Text = program.Webhook;
            popup.AutoMinimize.Checked = program.AutoMinimize;

            if (popup.ShowDialog() == DialogResult.OK)
            {
                StopAllTimers();

                Config.InternalConfig.ProgramsToRestart[programIndex].MaintainThis = popup.MaintainThis.Text;
                Config.InternalConfig.ProgramsToRestart[programIndex].Arguments = popup.Arguments.Text;
                Config.InternalConfig.ProgramsToRestart[programIndex].CreateNoWindow = popup.CreateNoWindow.Checked;
                Config.InternalConfig.ProgramsToRestart[programIndex].Interval = popup.Interval.Value;
                Config.InternalConfig.ProgramsToRestart[programIndex].KillIfNotResponding = popup.KillIfNotResponding.Checked;
                Config.InternalConfig.ProgramsToRestart[programIndex].NotRespondingTime = popup.NotRespondingTime.Value;
                Config.InternalConfig.ProgramsToRestart[programIndex].KillAfter = popup.KillAfter.Checked;
                Config.InternalConfig.ProgramsToRestart[programIndex].KillAfterTime = popup.KillAfterTime.Value * 60000;
                Config.InternalConfig.ProgramsToRestart[programIndex].WindowStartState = popup.WindowStartState.SelectedIndex;
                Config.InternalConfig.ProgramsToRestart[programIndex].WebhookPrefix = popup.WebhookPrefix.Text;
                Config.InternalConfig.ProgramsToRestart[programIndex].Webhook = popup.Webhook.Text;
                Config.InternalConfig.ProgramsToRestart[programIndex].AutoMinimize = popup.AutoMinimize.Checked;

                var data = TempDataStorage[Config.InternalConfig.ProgramsToRestart[programIndex]];

                data.RestartCount = restarts;

                var entryindex = Entries.ToList().FindIndex(o => o.ProcessName == Path.GetFileNameWithoutExtension(Config.InternalConfig.ProgramsToRestart[programIndex].MaintainThis));

                var gridentry = Entries[entryindex];

                gridentry.ProcessName = Path.GetFileNameWithoutExtension(popup.MaintainThis.Text);

                ProgramList.UpdateCellValue(0, entryindex);

                if (AutoRestartCheckBox.Checked)
                {
                    StartAllTimers();
                }
            }
        }

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private void NotificationAreaIcon_DoubleClick(object sender, EventArgs e)
        {
            NotificationAreaIcon.Visible = false;
            Show();
            SetForegroundWindow(Handle);
        }

        private void AutoRestartForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            Hide();

            NotificationAreaIcon.Visible = true;
            NotificationAreaIcon.ShowBalloonTip(5000, "Info", "Auto Restart Process will keep running in the background. To exit, right click the icon in the notification area.", ToolTipIcon.Info);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotificationAreaIcon.Visible = false;
            Environment.Exit(0);
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
    }

    public static class Ext
    {
        public static TValue SwapKey<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey fromKey, TKey toKey)
        {
            var value = dic[fromKey];
            dic.Remove(fromKey);
            dic[toKey] = value;

            return dic[toKey];
        }

        public static string GetLastErrorFromEventLog(this ProgramToRestart program)
        {
            var log = new EventLog("Application");

            var entry = log.Entries.Cast<EventLogEntry>().FirstOrDefault(o => o.Source.Contains(Path.GetFileNameWithoutExtension(program.MaintainThis)));

            return entry?.Message;
        }
    }
}