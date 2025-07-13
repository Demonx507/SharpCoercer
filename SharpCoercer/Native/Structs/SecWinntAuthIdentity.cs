using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace SharpCoercer.Win32.Structs
{
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct SecWinNTAuthIdentity
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string User;

        public int UserLength;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string Domain;

        public int DomainLength;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string Password;

        public int PasswordLength;
        public int Flags;
    };

}
