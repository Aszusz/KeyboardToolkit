using System.Windows.Input;
using KeyboardToolkit.Common;
using KeyboardToolkit.Interops.Enums;
using KeyboardToolkit.Interops.Methods;

namespace KeyboardToolkit.StateMonitor
{
    public class KeyStateMonitor : IKeyStateMonitor
    {
        public KeyState GetKeyState(Key key)
        {
            var virtualKeyCode = KeyInterop.VirtualKeyFromKey(key);
            return (ushort) GetKeyStateInterop.GetKeyState((VIRTUAL_KEY_CODE) virtualKeyCode) >> 15 == 1
                ? KeyState.KeyDown
                : KeyState.KeyUp;
        }
    }
}