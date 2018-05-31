using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Input;
using PInvoke;

namespace KeyboardToolkit
{
    public class KeySender : IKeySender
    {
        public void Send(IEnumerable<KeyEventArgs> sequence)
        {
            var inputs = sequence.Select(arg => new User32.INPUT
            {
                type = User32.InputType.INPUT_KEYBOARD,
                Inputs = new User32.INPUT.InputUnion
                {
                    ki = new User32.KEYBDINPUT
                    {
                        dwFlags = arg.KeyAction == KeyAction.KeyUp
                            ? User32.KEYEVENTF.KEYEVENTF_KEYUP
                            : 0,
                        wVk = (User32.VirtualKey) KeyInterop.VirtualKeyFromKey(arg.Key)
                    }
                }
            }).ToArray();

            var size = Marshal.SizeOf(inputs[0]);
            var result = User32.SendInput(inputs.Length, inputs, size);
            if (result != inputs.Length)
            {
                throw new InvalidOperationException();
            }
        }
    }
}