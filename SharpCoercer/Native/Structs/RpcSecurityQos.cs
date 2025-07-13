using System.Runtime.InteropServices;

namespace SharpCoercer.Win32.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct RpcSecurityQos
    {
        public int Version;
        public int Capabilities;
        public int IdentityTracking;
        public int ImpersonationType;
    };

}
