using System;
using System.Runtime.InteropServices;
using KeyboardToolkit.Interops.Enums;

// ReSharper disable InconsistentNaming

namespace KeyboardToolkit.Interops.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public int mouseData;
        public MOUSEEVENTF dwFlags;
        public uint time;
        public UIntPtr dwExtraInfo;
    }
}