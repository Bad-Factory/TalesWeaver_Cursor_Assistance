using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace TalesWeaver_Cursor_Assistance
{
	internal static class Program
	{
		private const int HWND_BROADCAST = 0xffff;
		public static readonly int WM_SHOW_EXISTING_INSTANCE = RegisterWindowMessage("WM_TALESWEAVER_CURSOR_ASSISTANCE_SHOW");

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern int RegisterWindowMessage(string lpString);

		[DllImport("user32.dll")]
		private static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

		[DllImport("shcore.dll", SetLastError = true, EntryPoint = "SetProcessDpiAwareness")]
		[return: MarshalAs(UnmanagedType.U4)]
		public static extern int SetProcessDpiAwareness(Program.PROCESS_DPI_AWARENESS value);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern Program.DPI_AWARENESS_CONTEXT SetThreadDpiAwarenessContext(Program.DPI_AWARENESS_CONTEXT value);

		[STAThread]
		private static void Main()
		{
			if (Environment.OSVersion.Version.Major >= 6)
			{
				Program.SetProcessDpiAwareness(Program.PROCESS_DPI_AWARENESS.PROCESS_PER_MONITOR_DPI_AWARE);
			}
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			string name = "TalesWeaver_Cursor_Assistance";
			bool createdNew;
			using (Mutex mutex = new Mutex(true, name, out createdNew))
			{
				if (!createdNew)
				{
					// 기존 실행 중인 트레이 앱에 창 복원 메시지 전달 (팝업 미출력)
					PostMessage((IntPtr)HWND_BROADCAST, WM_SHOW_EXISTING_INSTANCE, IntPtr.Zero, IntPtr.Zero);
					return;
				}

				Application.Run(new Form1());
			}
		}

		public enum PROCESS_DPI_AWARENESS
		{
			PROCESS_DPI_UNAWARE,
			PROCESS_SYSTEM_DPI_AWARE,
			PROCESS_PER_MONITOR_DPI_AWARE
		}

		public enum DPI_AWARENESS_CONTEXT
		{
			DPI_AWARENESS_CONTEXT_DEFAULT,
			DPI_AWARENESS_CONTEXT_UNAWARE = -1,
			DPI_AWARENESS_CONTEXT_SYSTEM_AWARE = -2,
			DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE = -3,
			DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 = -4
		}
	}
}