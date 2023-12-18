using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Auto_Restart_Process
{
    internal class Native
    {
        internal class ShowWindowCommand
        {
            internal static int SW_MAXIMIZE = 3;
            internal static int SW_MINIMIZE = 6;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    }
}
