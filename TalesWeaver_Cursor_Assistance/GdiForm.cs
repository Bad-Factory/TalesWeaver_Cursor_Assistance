using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

internal partial class GdiForm : Form
{
    private const int WS_EX_TRANSPARENT = 0x00000020;
    private const int WS_EX_LAYERED = 0x00080000;
    private const int WS_EX_NOACTIVATE = 0x08000000;

    public GdiForm()
    {
        base.FormBorderStyle = FormBorderStyle.None;
        base.TopMost = true;
        base.ShowInTaskbar = false;
        base.ControlBox = false;
        this.AllowDrop = false;
    }

    public void SetBitmap(Bitmap bitmap, byte opacity)
    {
        if (bitmap == null)
        {
            throw new ArgumentNullException(nameof(bitmap));
        }

        if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
        {
            throw new ApplicationException("The bitmap must be 32bpp with alpha-channel.");
        }

        if (this.IsDisposed)
        {
            return;
        }

        if (!this.IsHandleCreated)
        {
            IntPtr _ = this.Handle;
        }

        IntPtr screenDc = IntPtr.Zero;
        IntPtr memDc = IntPtr.Zero;
        IntPtr hBitmap = IntPtr.Zero;
        IntPtr oldBitmap = IntPtr.Zero;

        try
        {
            screenDc = Win32.GetDC(IntPtr.Zero);
            if (screenDc == IntPtr.Zero)
            {
                return;
            }

            memDc = Win32.CreateCompatibleDC(screenDc);
            if (memDc == IntPtr.Zero)
            {
                return;
            }

            hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
            if (hBitmap == IntPtr.Zero)
            {
                return;
            }

            oldBitmap = Win32.SelectObject(memDc, hBitmap);

            Win32.Size size = new Win32.Size(bitmap.Width, bitmap.Height);
            Win32.Point pointSource = new Win32.Point(0, 0);
            Win32.Point topPos = new Win32.Point(base.Left, base.Top);

            Win32.BLENDFUNCTION blend = new Win32.BLENDFUNCTION
            {
                BlendOp = Win32.AC_SRC_OVER,
                BlendFlags = 0,
                SourceConstantAlpha = opacity,
                AlphaFormat = Win32.AC_SRC_ALPHA
            };

            Win32.UpdateLayeredWindow(
                base.Handle,
                screenDc,
                ref topPos,
                ref size,
                memDc,
                ref pointSource,
                0,
                ref blend,
                Win32.ULW_ALPHA);
        }
        finally
        {
            if (memDc != IntPtr.Zero)
            {
                if (oldBitmap != IntPtr.Zero)
                {
                    Win32.SelectObject(memDc, oldBitmap);
                }

                Win32.DeleteDC(memDc);
            }

            if (hBitmap != IntPtr.Zero)
            {
                Win32.DeleteObject(hBitmap);
            }

            if (screenDc != IntPtr.Zero)
            {
                Win32.ReleaseDC(IntPtr.Zero, screenDc);
            }
        }
    }

    protected override CreateParams CreateParams
    {
        get
        {
            CreateParams createParams = base.CreateParams;
            createParams.ExStyle |= WS_EX_LAYERED | WS_EX_TRANSPARENT | WS_EX_NOACTIVATE;
            return createParams;
        }
    }

    protected override bool ShowWithoutActivation => true;
}