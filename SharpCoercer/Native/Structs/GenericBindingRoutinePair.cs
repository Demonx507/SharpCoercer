using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace SharpCoercer.Win32.Structs
{
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1049:TypesThatOwnNativeResourcesShouldBeDisposable"), StructLayout(LayoutKind.Sequential)]
    internal struct GenericBindingRoutinePair
    {
        public IntPtr Bind;
        public IntPtr Unbind;
    }

}
