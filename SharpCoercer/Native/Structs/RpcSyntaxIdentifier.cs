using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace SharpCoercer.Win32.Structs
{
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    [StructLayout(LayoutKind.Sequential)]
    internal struct RpcSyntaxIdentifier
    {
        public Guid SyntaxGUID;
        public RpcVersion SyntaxVersion;
    }
}
