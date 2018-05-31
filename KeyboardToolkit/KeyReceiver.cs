using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
using PInvoke;

namespace KeyboardToolkit
{
    public class KeyReceiver : IDisposable, IKeyReceiver
    {
        private User32.SafeHookHandle _hookId = User32.SafeHookHandle.Null;
        public event EventHandler<KeyEventArgs> KeyReceived;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Install()
        {
            if(_hookId != User32.SafeHookHandle.Null) return;
            _hookId = SetHook(HookFunc);
        }

        public void Uninstall()
        {
            if (_hookId == User32.SafeHookHandle.Null) return;
            _hookId.Close();
            _hookId = User32.SafeHookHandle.Null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _hookId.Dispose();
            }
        }

        protected virtual void RaiseKeyEvent(KeyEventArgs args)
        {
            KeyReceived?.Invoke(this, args);
        }

        private static User32.SafeHookHandle SetHook(User32.WindowsHookDelegate windowsHookDelegate)
        {
            using (var module = Process.GetCurrentProcess().MainModule)
            {
                return User32.SetWindowsHookEx(
                    User32.WindowsHookType.WH_KEYBOARD_LL,
                    windowsHookDelegate,
                    Kernel32.GetModuleHandle(module.ModuleName).DangerousGetHandle(),
                    0);
            }
        }

        private int HookFunc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
            {
                return User32.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
            }

            var iwParam = wParam.ToInt32();

            if (iwParam == (uint) User32.WindowMessage.WM_KEYDOWN ||
                iwParam == (uint) User32.WindowMessage.WM_SYSKEYDOWN)
            {
                {
                    var code = Marshal.ReadInt32(lParam);
                    var key = KeyInterop.KeyFromVirtualKey(code);
                    RaiseKeyEvent(new KeyEventArgs(key, KeyAction.KeyDown));
                }
            }
            else if (iwParam == (uint) User32.WindowMessage.WM_KEYUP ||
                     iwParam == (uint) User32.WindowMessage.WM_SYSKEYUP)
            {
                {
                    var code = Marshal.ReadInt32(lParam);
                    var key = KeyInterop.KeyFromVirtualKey(code);
                    RaiseKeyEvent(new KeyEventArgs(key, KeyAction.KeyUp));
                }
            }

            return User32.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }
    }
}