using System;
using KeyboardToolkit.Common;

namespace KeyboardToolkit.Receiver
{
    public interface IKeyReceiver
    {
        event EventHandler<KeyEventArgs> KeyReceived;
        void Dispose();
        void Install();
        void Uninstall();
    }
}