using SharpCoercer.Win32.Structs;
using System;
using System.Runtime.InteropServices;

namespace SharpCoercer.Win32
{
    internal class Rpcrt4
    {


        [DllImport("Rpcrt4.dll", CharSet = CharSet.Unicode)]
        public static extern int RpcBindingSetObject(
    IntPtr Binding,
    ref RpcClientInterface IfSpec
);

        // 3) P/Invoke for RpcEpResolveBinding  
        [DllImport("Rpcrt4.dll", CharSet = CharSet.Unicode)]
        public static extern int RpcEpResolveBinding(
            IntPtr Binding,
            ref RpcClientInterface IfSpec
        );
        [DllImport("Rpcrt4.dll", EntryPoint = "RpcBindingFromStringBindingW", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = false)]
        internal static extern int RpcBindingFromStringBinding(string bindingString, out IntPtr lpBinding);

        [DllImport("Rpcrt4.dll", EntryPoint = "RpcBindingFree", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = false)]
        internal static extern int RpcBindingFree(ref IntPtr lpString);


        [DllImport("Rpcrt4.dll", EntryPoint = "RpcStringBindingComposeW", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = false)]
        internal static extern int RpcStringBindingCompose(string ObjUuid, string ProtSeq, string NetworkAddr, string Endpoint, string Options, out IntPtr lpBindingString);


        [DllImport("Rpcrt4.dll", EntryPoint = "RpcBindingSetOption", CallingConvention = CallingConvention.StdCall, SetLastError = false)]
        internal static extern int RpcBindingSetOption(IntPtr Binding, uint Option, IntPtr OptionValue);

        [DllImport("Rpcrt4.dll", EntryPoint = "RpcBindingSetOption", CallingConvention = CallingConvention.StdCall, SetLastError = false)]
        internal static extern int RpcBindingSetOption(IntPtr Binding, uint Option, uint OptionValue);

        [DllImport("Rpcrt4.dll", EntryPoint = "RpcBindingSetAuthInfoExW", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = false)]
        internal static extern int RpcBindingSetAuthInfoEx(IntPtr lpBinding, string ServerPrincName, uint AuthnLevel, uint AuthnSvc, ref SecWinNTAuthIdentity AuthIdentity, uint AuthzSvc, ref RpcSecurityQos SecurityQOS);

        [DllImport("Rpcrt4.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        public static extern int RpcBindingSetAuthInfoEx(IntPtr Binding,[MarshalAs(UnmanagedType.LPWStr)]string ServerPrincName,uint AuthnLevel,uint AuthnSvc,ref SecWinNTAuthIdentity AuthInfo, uint AuthzSvc,IntPtr SecurityQOS  );
        [DllImport("Rpcrt4.dll", EntryPoint = "RpcBindingSetAuthInfoW", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = false)]
        internal static extern int RpcBindingSetAuthInfo(IntPtr lpBinding, string ServerPrincName, uint AuthnLevel, uint AuthnSvc, IntPtr AuthIdentity, uint AuthzSvc);



        [DllImport("Rpcrt4.dll", EntryPoint = "NdrClientCall2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = false)]
        internal static extern IntPtr EfsRpcOpenFileRaw(IntPtr pMIDL_STUB_DESC, IntPtr formatString, IntPtr binding, out IntPtr hContext, string FileName, int Flags);

        [DllImport("Rpcrt4.dll", EntryPoint = "NdrClientCall2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = false)]
        internal static extern IntPtr EfsRpcRemoveUsersFromFile(IntPtr pMIDL_STUB_DESC, IntPtr formatString, IntPtr binding, string FileName, out IntPtr efsObject);


        [DllImport("Rpcrt4.dll", EntryPoint = "NdrClientCall2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = false)]
        internal static extern IntPtr EfsRpcQueryUsersOnFile(IntPtr pMIDL_STUB_DESC, IntPtr formatString, IntPtr binding, string FileName, out IntPtr efsObject);


        [DllImport("Rpcrt4.dll", EntryPoint = "NdrClientCall2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = false)]
        internal static extern IntPtr EfsRpcQueryRecoveryAgents(IntPtr pMIDL_STUB_DESC, IntPtr formatString, IntPtr binding, string FileName, out IntPtr efsObject);


        [DllImport("Rpcrt4.dll", EntryPoint = "NdrClientCall2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = false)]
        internal static extern IntPtr EfsRpcDecryptFileSrv(IntPtr pMIDL_STUB_DESC, IntPtr formatString, IntPtr binding, string FileName, ulong Flags);

        [DllImport("Rpcrt4.dll", EntryPoint = "NdrClientCall2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = false)]
        internal static extern IntPtr EfsRpcEncryptFileSrv(IntPtr pMIDL_STUB_DESC, IntPtr formatString, IntPtr binding, string FileName);

        [DllImport("Rpcrt4.dll", EntryPoint = "NdrClientCall2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = false)]
        internal static extern IntPtr EfsRpcAddUsersToFile(IntPtr pMIDL_STUB_DESC, IntPtr formatString, IntPtr binding, string FileName, out IntPtr efsObject);


        [DllImport("Rpcrt4.dll", EntryPoint = "NdrClientCall2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = false)]
        public static extern IntPtr EfsRpcFileKeyInfo(
           IntPtr pMIDL_STUB_DESC,
           IntPtr pFormat,
           IntPtr Handle,
           [MarshalAs(UnmanagedType.LPWStr)] string FileName,
           uint InfoClass,
           out IntPtr KeyInfo
       );

        [DllImport("Rpcrt4.dll", EntryPoint = "NdrClientCall2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = false)]
        public static extern IntPtr EfsRpcDuplicateEncryptionInfoFile(
          IntPtr pMIDL_STUB_DESC,
          IntPtr pFormat,
          IntPtr BindingHandle,
          [MarshalAs(UnmanagedType.LPWStr)] string SrcFileName,
          [MarshalAs(UnmanagedType.LPWStr)] string DstFileName,
          uint dwCreationDisposition,
          uint dwAttributes,
          [MarshalAs(UnmanagedType.LPWStr)] string lpStreamName,  // can be null (unique string)
          int fOverwrite
      );
        [DllImport("Rpcrt4.dll", EntryPoint = "NdrClientCall2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = false)]
        public static extern IntPtr EfsRpcAddUsersToFileEx(
          IntPtr pMIDL_STUB_DESC,
          IntPtr pFormat,
          IntPtr BindingHandle,
          uint dwFlags,
          IntPtr reservedPtr,
          [MarshalAs(UnmanagedType.LPWStr)] string fileName,
          IntPtr encryptionCertList
      );

        [DllImport("Rpcrt4.dll", EntryPoint = "NdrClientCall2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = false)]
        public static extern IntPtr NdrClientCall2(
    IntPtr pMIDL_STUB_DESC,
    IntPtr pFormat,
    IntPtr BindingHandle,
    uint dwFileKeyInfoFlags,
    IntPtr reservedPtr,
    [MarshalAs(UnmanagedType.LPWStr)] string FileName,
    uint infoClass,
    out IntPtr keyInfo
);
        [DllImport("Rpcrt4.dll", EntryPoint = "NdrClientCall2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = false)]
        public static extern IntPtr NetrDfsRemoveStdRoot(
           IntPtr pStubDesc,          // &netdfs_StubDesc from your BuildStub
           IntPtr pProcFormatString,  // &ms2Ddfsnm__MIDL_ProcFormatString.Format[714]
           IntPtr hBinding,           // IDL_handle
           [MarshalAs(UnmanagedType.LPWStr)] string ServerName,
           [MarshalAs(UnmanagedType.LPWStr)] string RootShare,
           uint ApiFlags
       );

        [DllImport("Rpcrt4.dll", EntryPoint = "NdrClientCall2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = false)]
        public static extern IntPtr NetrDfsAddStdRoot(
           IntPtr pStubDesc,          // &netdfs_StubDesc from your BuildStub
           IntPtr pProcFormatString,  // &ms2Ddfsnm__MIDL_ProcFormatString.Format[714]
           IntPtr hBinding,           // IDL_handle
           [MarshalAs(UnmanagedType.LPWStr)] string ServerName,
           [MarshalAs(UnmanagedType.LPWStr)] string RootShare,
              [MarshalAs(UnmanagedType.LPWStr)] string Comment,
           uint ApiFlags
       );

        [DllImport("Rpcrt4.dll", EntryPoint = "NdrClientCall2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = false)]
        internal static extern IntPtr NdrClientCall2x64(IntPtr pMIDL_STUB_DESC, IntPtr formatString, string pPrinterName, out IntPtr pHandle, string pDatatype, ref DevModeContainer pDevModeContainer, int AccessRequired);
        [DllImport("Rpcrt4.dll", EntryPoint = "NdrClientCall2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = false)]
        internal static extern IntPtr NdrClientCall2x64(IntPtr pMIDL_STUB_DESC, IntPtr formatString, ref IntPtr Handle);


        [DllImport("Rpcrt4.dll", EntryPoint = "NdrClientCall2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = false)]
        internal static extern IntPtr NdrClientCall2x64(IntPtr pMIDL_STUB_DESC, IntPtr formatString, IntPtr hPrinter, uint fdwFlags, uint fdwOptions, string pszLocalMachine, uint dwPrinterLocal, IntPtr intPtr3);
        [DllImport("Rpcrt4.dll",
   EntryPoint = "NdrClientCall2",
   CallingConvention = CallingConvention.Cdecl,
   CharSet = CharSet.Unicode)]
        public static extern IntPtr NdrClientCall2(
   IntPtr pMIDL_STUB_DESC,          // pointer to your MIDL_STUB_DESC
   IntPtr pProcFormatString,        // pointer to the ProcFormatString byte[] chunk
   IntPtr bindingHandle,            // your RPC binding handle
   [MarshalAs(UnmanagedType.LPWStr)]
        string path                      // the wchar_t* path parameter
        ,
   out bool SupportedByThisProvider,
   out IntPtr pContextHandle
);
      
    }
}
