using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace SharpCoercer.Win32.Structs
{

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    [StructLayout(LayoutKind.Sequential)]
    internal struct RpcClientInterface
    {
        public uint Length;
        public RpcSyntaxIdentifier InterfaceId;
        public RpcSyntaxIdentifier TransferSyntax;
        public IntPtr /*PRPC_DISPATCH_TABLE*/ DispatchTable;
        public uint RpcProtseqEndpointCount;
        public IntPtr /*PRPC_PROTSEQ_ENDPOINT*/ RpcProtseqEndpoint;
        public IntPtr Reserved;
        public IntPtr InterpreterInfo;
        public uint Flags;

        public static Guid IID_SYNTAX = new Guid(0x8A885D04u, 0x1CEB, 0x11C9, 0x9F, 0xE8, 0x08, 0x00, 0x2B, 0x10, 0x48, 0x60);

        public RpcClientInterface(Guid iid, ushort InterfaceVersionMajor, ushort InterfaceVersionMinor)
        {
            Length = (uint)Marshal.SizeOf(typeof(RpcClientInterface));
            RpcVersion rpcVersion = new RpcVersion(InterfaceVersionMajor, InterfaceVersionMinor);
            InterfaceId = new RpcSyntaxIdentifier();
            InterfaceId.SyntaxGUID = iid;
            InterfaceId.SyntaxVersion = rpcVersion;
            rpcVersion = new RpcVersion(2, 0);
            TransferSyntax = new RpcSyntaxIdentifier();
            TransferSyntax.SyntaxGUID = IID_SYNTAX;
            TransferSyntax.SyntaxVersion = rpcVersion;
            DispatchTable = IntPtr.Zero;
            RpcProtseqEndpointCount = 0u;
            RpcProtseqEndpoint = IntPtr.Zero;
            Reserved = IntPtr.Zero;
            InterpreterInfo = IntPtr.Zero;
            Flags = 0u;
        }
    }

}
