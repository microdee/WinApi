using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using WinApi.Core;
using WinApi.Kernel32;
using WinApi.User32;
using WinApi.Desktop;
using WinApi.DwmApi;
using WinApi.Gdi32;
using WinApi.User32.Experimental;
using WinApi.UxTheme;
using WinApi.Windows;
using WinApi.Windows.Helpers;

namespace Sample.DirectX
{
    internal class WinConstParams : ConstructionParams
    {
        public bool Right;
        public override int X => Right ? 512 : 0;
        public override int Y => 0;
        public override int Width => 512;
        public override int Height => 512;

        public override WindowStyles Styles =>
            WindowStyles.WS_POPUPWINDOW;

        public override WindowExStyles ExStyles =>
            WindowExStyles.WS_EX_APPWINDOW |
            WindowExStyles.WS_EX_NOREDIRECTIONBITMAP;
    }

    internal class Program
    {
        static int Main(string[] args)
        {
            try
            {
                ApplicationHelpers.SetupDefaultExceptionHandlers();
                var factory = WindowFactory.Create(hBgBrush: IntPtr.Zero);
                // Create the window without a dependency on WinApi.Windows.Controls
                var win1 = factory.CreateWindow(
                    () => new MainD2DWindow(),
                    "Hello", constructionParams: new WinConstParams()
                );
                win1.Show();

                var win2 = factory.CreateWindow(
                    () => new MainD2DWindow(),
                    "Hello", constructionParams: new WinConstParams { Right = true }
                );
                win2.EnableBlurBehind(true, WinApiHelpers.BgraColor(55, 55, 55, 140));
                win2.Show();

                var res = new EventLoop().Run(win1);
                win1.Dispose();
                win2.Dispose();
                return res;
            }
            catch (Exception ex)
            {
                MessageBoxHelpers.ShowError(ex);
                return 1;
            }
        }
    }
}