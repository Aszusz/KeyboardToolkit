using System;
using System.Windows.Forms;
using KeyboardToolkit.Common;

namespace KeyboardToolkit.HotKeys
{
    public interface IHotKey
    {
        event Action Pressed;
        int Id { get; }
        Keys Key { get; }
        KeyModifiers Modifiers { get; }
        void Register();
        void Unregister();
        void Dispose();
    }
}