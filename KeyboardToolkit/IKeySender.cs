using System.Collections.Generic;

namespace KeyboardToolkit
{
    public interface IKeySender
    {
        void Send(IEnumerable<KeyEventArgs> sequence);
    }
}