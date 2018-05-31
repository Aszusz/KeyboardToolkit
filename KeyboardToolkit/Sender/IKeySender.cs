using System.Collections.Generic;
using KeyboardToolkit.Common;

namespace KeyboardToolkit.Sender
{
    public interface IKeySender
    {
        void Send(IEnumerable<KeyEventArgs> sequence);
    }
}