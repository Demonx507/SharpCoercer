using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace SharpCoercer.Win32.Structs
{
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]

    [StructLayout(LayoutKind.Sequential)]
    internal struct RpcVersion
    {
        public ushort MajorVersion;
        public ushort MinorVersion;

        public RpcVersion(ushort InterfaceVersionMajor, ushort InterfaceVersionMinor)
        {
            MajorVersion = InterfaceVersionMajor;
            MinorVersion = InterfaceVersionMinor;
        }
    }
}
