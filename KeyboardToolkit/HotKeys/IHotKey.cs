using System;
using System.Windows.Forms;

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