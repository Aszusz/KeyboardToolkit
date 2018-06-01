using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Input;
using KeyboardToolkit.Common;
using KeyboardToolkit.Interops.Enums;
using KeyboardToolkit.Interops.Methods;
using KeyboardToolkit.Interops.Structs;

namespace KeyboardToolkit.Sender
{
    public class KeySender : IKeySender
    {
        public void Send(IEnumerable<KeyEventArgs> sequence)
        {
            var inputs = sequence.Select(arg => new INPUT
            {
                type = INPUT_TYPE.INPUT_KEYBOARD,
                U = new INPUT_UNION
                {
                    ki = new KEYBDINPUT
                    {
                        dwFlags = arg.KeyState == KeyState.KeyUp
                            ? KEYEVENTF.KEYUP
                            : 0,
                        wVk = (VIRTUAL_KEY_CODE) KeyInterop.VirtualKeyFromKey(arg.Key)
                    }
                }
            }).ToArray();

            var size = Marshal.SizeOf(inputs[0]);
            var result = SendInputInterop.SendInput((uint) inputs.Length, inputs, size);
            if (result != inputs.Length)
            {
                throw new InvalidOperationException();
            }
        }
    }
}