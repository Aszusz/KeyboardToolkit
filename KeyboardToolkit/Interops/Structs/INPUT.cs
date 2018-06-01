using System.Runtime.InteropServices;
using KeyboardToolkit.Interops.Enums;

// ReSharper disable InconsistentNaming

namespace KeyboardToolkit.Interops.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct INPUT
    {
        public INPUT_TYPE type;
        public INPUT_UNION U;
        public static int Size => Marshal.SizeOf(typeof(INPUT));
    }
}