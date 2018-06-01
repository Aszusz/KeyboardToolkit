using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
using KeyboardToolkit.Common;
using KeyboardToolkit.Interops.Constants;
using KeyboardToolkit.Interops.Methods;

namespace KeyboardToolkit.Receiver
{
    public class KeyReceiver : IDisposable, IKeyReceiver
    {
        private const int WH_KEYBOARD_LL = 13;
        private IntPtr _hookId = IntPtr.Zero;
        public event EventHandler<KeyEventArgs> KeyReceived;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Install()
        {
            if (_hookId != IntPtr.Zero) return;
            _hookId = SetHook(HookFunc);
        }

        public void Uninstall()
        {
            if (_hookId == IntPtr.Zero) return;
            WindowsHookExInterop.UnhookWindowsHookEx(_hookId);
            _hookId = IntPtr.Zero;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Uninstall();
            }
        }

        ~KeyReceiver()
        {
            Dispose(false);
        }

        protected virtual void RaiseKeyEvent(KeyEventArgs args)
        {
            KeyReceived?.Invoke(this, args);
        }

        private static IntPtr SetHook(WindowsHookExInterop.KeyboardHook windowsHookDelegate)
        {
            using (var module = Process.GetCurrentProcess().MainModule)
            {
                return WindowsHookExInterop.SetWindowsHookEx(
                    WH_KEYBOARD_LL,
                    windowsHookDelegate,
                    GetModuleHandleInterop.GetModuleHandle(module.ModuleName),
                    0);
            }
        }

        private IntPtr HookFunc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
            {
                return WindowsHookExInterop.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
            }

            var iwParam = wParam.ToInt32();

            if (iwParam == (uint) WINDOW_MESSAGE.WM_KEYDOWN ||
                iwParam == (uint) WINDOW_MESSAGE.WM_SYSKEYDOWN)
            {
                {
                    var code = Marshal.ReadInt32(lParam);
                    var key = KeyInterop.KeyFromVirtualKey(code);
                    RaiseKeyEvent(new KeyEventArgs(key, KeyState.KeyDown));
                }
            }
            else if (iwParam == (uint) WINDOW_MESSAGE.WM_KEYUP ||
                     iwParam == (uint) WINDOW_MESSAGE.WM_SYSKEYUP)
            {
                {
                    var code = Marshal.ReadInt32(lParam);
                    var key = KeyInterop.KeyFromVirtualKey(code);
                    RaiseKeyEvent(new KeyEventArgs(key, KeyState.KeyUp));
                }
            }

            return WindowsHookExInterop.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }
    }
}