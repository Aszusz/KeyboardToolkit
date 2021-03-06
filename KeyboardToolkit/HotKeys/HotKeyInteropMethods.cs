using System;
using System.Runtime.InteropServices;

namespace KeyboardToolkit.HotKeys
{
    public static class HotKeyInteropMethods
    {
        [DllImport("user32", SetLastError = true)]
        internal static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32", SetLastError = true)]
        internal static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }
}