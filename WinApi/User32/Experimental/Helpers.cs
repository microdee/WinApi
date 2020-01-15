using System;
using System.Runtime.InteropServices;
using WinApi.Windows;

namespace WinApi.User32.Experimental
{
    public static class User32ExperimentalHelpers
    {
        /// <summary>
        /// Enable blur behind window with DWM Window Composition
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="acrylic">Only works after Windows 10 1803 (10.0.17134) (Redstone 4)</param>
        /// <param name="bgColor">BGRA bytes</param>
        public static void EnableBlurBehind(IntPtr hwnd, bool acrylic, uint bgColor)
        {
            var accent = new AccentPolicy();
            var accentStructSize = Marshal.SizeOf<AccentPolicy>();

            accent.AccentState = acrylic ?
                AccentState.ACCENT_ENABLE_ACRYLICBLURBEHIND :
                AccentState.ACCENT_ENABLE_BLURBEHIND;

            if (acrylic)
                accent.GradientColor = bgColor;

            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            try
            {
                Marshal.StructureToPtr(accent, accentPtr, false);
                var data = new WindowCompositionAttributeData
                {
                    Attribute = WindowCompositionAttributeType.WCA_ACCENT_POLICY,
                    DataSize = accentStructSize,
                    Data = accentPtr
                };
                User32ExperimentalMethods.SetWindowCompositionAttribute(hwnd, ref data);
            }
            finally { Marshal.FreeHGlobal(accentPtr); }
        }

        /// <summary>
        /// Enable blur behind window with DWM Window Composition
        /// </summary>
        /// <param name="win"></param>
        /// <param name="acrylic">Only works after Windows 10 1803 (10.0.17134) (Redstone 4)</param>
        /// <param name="bgColor">BGRA bytes</param>
        public static void EnableBlurBehind(this NativeWindow win, bool acrylic, uint bgColor)
        {
            EnableBlurBehind(win.Handle, acrylic, bgColor);
        }
    }
}