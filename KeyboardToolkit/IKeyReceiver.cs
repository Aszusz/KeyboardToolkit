using System;

namespace KeyboardToolkit
{
    public interface IKeyReceiver
    {
        event EventHandler<KeyEventArgs> KeyReceived;
        void Dispose();
        void Install();
        void Uninstall();
    }
}