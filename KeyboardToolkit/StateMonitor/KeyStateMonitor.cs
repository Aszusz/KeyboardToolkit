using System.Windows.Input;
using KeyboardToolkit.Common;
using PInvoke;

namespace KeyboardToolkit.StateMonitor
{
    public class KeyStateMonitor : IKeyStateMonitor
    {
        public KeyState GetKeyState(Key key)
        {
            var virtualKeyCode = KeyInterop.VirtualKeyFromKey(key);
            return (ushort) User32.GetKeyState(virtualKeyCode) >> 15 == 1
                ? KeyState.KeyDown
                : KeyState.KeyUp;
        }
    }
}