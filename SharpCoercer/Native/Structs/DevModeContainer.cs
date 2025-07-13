using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace SharpCoercer.Win32.Structs
{
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct DevModeContainer
    {
        private Int32 cbBuf;
        private IntPtr pDevMode;
    }
}
