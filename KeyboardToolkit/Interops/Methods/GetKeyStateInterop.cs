using System.Runtime.InteropServices;
using KeyboardToolkit.Interops.Enums;

namespace KeyboardToolkit.Interops.Methods
{
    internal class GetKeyStateInterop
    {
        [DllImport("USER32.dll")]
        internal static extern short GetKeyState(VIRTUAL_KEY_CODE nVirtKey);
    }
}