using System.Windows.Input;
using KeyboardToolkit.Common;

namespace KeyboardToolkit.StateMonitor
{
    public interface IKeyStateMonitor
    {
        KeyState GetKeyState(Key key);
    }
}