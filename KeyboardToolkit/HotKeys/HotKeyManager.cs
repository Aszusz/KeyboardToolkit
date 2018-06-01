using System;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using KeyboardToolkit.Common;

namespace KeyboardToolkit.HotKeys
{
    internal static class HotKeyManager
    {
        private static readonly ManualResetEvent WindowReadyEvent = new ManualResetEvent(false);
        private static IntPtr _hwnd;
        private static HotKeyMessageWindow _wnd;

        static HotKeyManager()
        {
            var messageLoop = new Thread(delegate() { Application.Run(new HotKeyMessageWindow()); })
            {
                Name = "HotKeyMessageLoopThread",
                IsBackground = true
            };
            messageLoop.Start();
        }

        private delegate void RegisterHotKeyDelegate(IntPtr hwnd, int id, uint modifiers, uint key);

        private delegate void UnRegisterHotKeyDelegate(IntPtr hwnd, int id);

        internal static event EventHandler<HotKeyEventArgs> HotKeyPressed;

        internal static void RegisterHotKey(Key key, ModifierKeys modifiers, int id)
        {
            var virtualKeyCode = KeyInterop.VirtualKeyFromKey(key);
            WindowReadyEvent.WaitOne();
            _wnd.Invoke(new RegisterHotKeyDelegate(RegisterHotKeyInternal), _hwnd, id, (uint) modifiers, (uint) virtualKeyCode);
        }

        internal static void UnregisterHotKey(int id)
        {
            _wnd.Invoke(new UnRegisterHotKeyDelegate(UnRegisterHotKeyInternal), _hwnd, id);
        }

        private static void RaiseHotKeyPressed(HotKeyEventArgs e)
        {
            HotKeyPressed?.Invoke(null, e);
        }

        private static void RegisterHotKeyInternal(IntPtr hwnd, int id, uint modifiers, uint key)
        {
            HotKeyInteropMethods.RegisterHotKey(hwnd, id, modifiers, key);
        }

        private static void UnRegisterHotKeyInternal(IntPtr hwnd, int id)
        {
            HotKeyInteropMethods.UnregisterHotKey(_hwnd, id);
        }

        private sealed class HotKeyMessageWindow : Form
        {
            // ReSharper disable once InconsistentNaming
            private const int WM_HOTKEY = 0x312;

            public HotKeyMessageWindow()
            {
                _wnd = this;
                _hwnd = Handle;
                WindowReadyEvent.Set();
            }

            protected override void SetVisibleCore(bool value)
            {
                // Ensure the window never becomes visible
                base.SetVisibleCore(false);
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == WM_HOTKEY)
                {
                    var e = new HotKeyEventArgs(m.LParam, m.WParam);
                    RaiseHotKeyPressed(e);
                }

                base.WndProc(ref m);
            }
        }
    }
}