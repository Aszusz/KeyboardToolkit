using System;
using System.Windows.Forms;

namespace KeyboardToolkit.HotKeys
{
    public class HotKeyEventArgs : EventArgs
    {
        public readonly int Id;
        public readonly Keys Key;
        public readonly KeyModifiers Modifiers;

        public HotKeyEventArgs(Keys key, KeyModifiers modifiers, int id)
        {
            Key = key;
            Modifiers = modifiers;
            Id = id;
        }

        public HotKeyEventArgs(IntPtr hotKeyLParam, IntPtr hotKeyWParam)
        {
            var virtualKeyCode = (uint) hotKeyLParam.ToInt32();
            Key = (Keys) ((virtualKeyCode & 0xffff0000) >> 16);
            Modifiers = (KeyModifiers) (virtualKeyCode & 0x0000ffff);
            Id = (int) hotKeyWParam;
        }
    }
}