using System;
using System.Windows.Input;

namespace KeyboardToolkit.HotKeys
{
    public interface IHotKey
    {
        event Action Pressed;
        int Id { get; }
        Key Key { get; }
        ModifierKeys Modifiers { get; }
        void Register();
        void Unregister();
        void Dispose();
    }
}