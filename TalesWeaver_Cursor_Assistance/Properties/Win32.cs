using System;
using System.Runtime.InteropServices;

// Token: 0x02000002 RID: 2
internal class Win32
{
	// Token: 0x06000001 RID: 1
	[DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
	public static extern Win32.Bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Win32.Point pptDst, ref Win32.Size psize, IntPtr hdcSrc, ref Win32.Point pprSrc, int crKey, ref Win32.BLENDFUNCTION pblend, int dwFlags);

	// Token: 0x06000002 RID: 2
	[DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
	public static extern IntPtr GetDC(IntPtr hWnd);

	// Token: 0x06000003 RID: 3
	[DllImport("user32.dll", ExactSpelling = true)]
	public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

	// Token: 0x06000004 RID: 4
	[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
	public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

	// Token: 0x06000005 RID: 5
	[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
	public static extern Win32.Bool DeleteDC(IntPtr hdc);

	// Token: 0x06000006 RID: 6
	[DllImport("gdi32.dll", ExactSpelling = true)]
	public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

	// Token: 0x06000007 RID: 7
	[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
	public static extern Win32.Bool DeleteObject(IntPtr hObject);

	// Token: 0x06000008 RID: 8
	[DllImport("user32.dll")]
	public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

	// Token: 0x06000009 RID: 9
	[DllImport("user32.dll")]
	public static extern long GetWindowLong(IntPtr hWnd, int nIndex);

	// Token: 0x04000001 RID: 1
	public const int ULW_COLORKEY = 1;

	// Token: 0x04000002 RID: 2
	public const int ULW_ALPHA = 2;

	// Token: 0x04000003 RID: 3
	public const int ULW_OPAQUE = 4;

	// Token: 0x04000004 RID: 4
	public const byte AC_SRC_OVER = 0;

	// Token: 0x04000005 RID: 5
	public const byte AC_SRC_ALPHA = 1;

	// Token: 0x02000003 RID: 3
	public enum Bool
	{
		// Token: 0x04000007 RID: 7
		False,
		// Token: 0x04000008 RID: 8
		True
	}

	// Token: 0x02000004 RID: 4
	public struct Point
	{
		// Token: 0x0600000B RID: 11 RVA: 0x00002058 File Offset: 0x00000258
		public Point(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		// Token: 0x04000009 RID: 9
		public int x;

		// Token: 0x0400000A RID: 10
		public int y;
	}

	// Token: 0x02000005 RID: 5
	public struct Size
	{
		// Token: 0x0600000C RID: 12 RVA: 0x00002068 File Offset: 0x00000268
		public Size(int cx, int cy)
		{
			this.cx = cx;
			this.cy = cy;
		}

		// Token: 0x0400000B RID: 11
		public int cx;

		// Token: 0x0400000C RID: 12
		public int cy;
	}

	// Token: 0x02000006 RID: 6
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	private struct ARGB
	{
		// Token: 0x0400000D RID: 13
		public byte Blue;

		// Token: 0x0400000E RID: 14
		public byte Green;

		// Token: 0x0400000F RID: 15
		public byte Red;

		// Token: 0x04000010 RID: 16
		public byte Alpha;
	}

	// Token: 0x02000007 RID: 7
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct BLENDFUNCTION
	{
		// Token: 0x04000011 RID: 17
		public byte BlendOp;

		// Token: 0x04000012 RID: 18
		public byte BlendFlags;

		// Token: 0x04000013 RID: 19
		public byte SourceConstantAlpha;

		// Token: 0x04000014 RID: 20
		public byte AlphaFormat;
	}
}
