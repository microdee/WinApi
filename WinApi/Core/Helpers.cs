using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi.Core
{
    public static class WinApiHelpers
    {
        public static uint BgrColor(byte r, byte g, byte b)
        {
            return (uint)b | ((uint)g << 8) | ((uint)r << 16);
        }
        public static uint BgraColor(byte r, byte g, byte b, byte a)
        {
            return (uint)b | ((uint)g << 8) | ((uint)r << 16) | ((uint)a << 24);
        }
    }
}
