using System;
using System.Runtime.InteropServices;

namespace SharpCoercer.Win32.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MidlStubDesc
    {
        public IntPtr /*RPC_CLIENT_INTERFACE*/ RpcInterfaceInformation;
        public IntPtr pfnAllocate;
        public IntPtr pfnFree;
        public IntPtr pAutoBindHandle;
        public IntPtr /*NDR_RUNDOWN*/ apfnNdrRundownRoutines;
        public IntPtr /*GENERIC_BINDING_ROUTINE_PAIR*/ aGenericBindingRoutinePairs;
        public IntPtr /*EXPR_EVAL*/ apfnExprEval;
        public IntPtr /*XMIT_ROUTINE_QUINTUPLE*/ aXmitQuintuple;
        public IntPtr pFormatTypes;
        public int fCheckBounds;
        /* Ndr library version. */
        public uint Version;
        public IntPtr /*MALLOC_FREE_STRUCT*/ pMallocFreeStruct;
        public int MIDLVersion;
        public IntPtr CommFaultOffsets;

        // New fields for version 3.0+
        public IntPtr /*USER_MARSHAL_ROUTINE_QUADRUPLE*/ aUserMarshalQuadruple;

        // Notify routines - added for NT5, MIDL 5.0
        public IntPtr /*NDR_NOTIFY_ROUTINE*/ NotifyRoutineTable;

        public IntPtr mFlags;

        // International support routines - added for 64bit post NT5
        public IntPtr /*NDR_CS_ROUTINES*/ CsRoutineTables;

        public IntPtr ProxyServerInfo;
        public IntPtr /*NDR_EXPR_DESC*/ pExprInfo;
        // Fields up to now present in win2000 release.

        public MidlStubDesc(IntPtr pFormatTypesPtr, IntPtr RpcInterfaceInformationPtr,
                                IntPtr pfnAllocatePtr, IntPtr pfnFreePtr,IntPtr genericBindingRoutinePairs, int midlVersion)
        {
            pFormatTypes = pFormatTypesPtr;
            RpcInterfaceInformation = RpcInterfaceInformationPtr;
            CommFaultOffsets = IntPtr.Zero;
            pfnAllocate = pfnAllocatePtr;
            pfnFree = pfnFreePtr;
            pAutoBindHandle = IntPtr.Zero;
            apfnNdrRundownRoutines = IntPtr.Zero;
            aGenericBindingRoutinePairs = genericBindingRoutinePairs;
            apfnExprEval = IntPtr.Zero;
            aXmitQuintuple = IntPtr.Zero;
            fCheckBounds = 1;
            Version = 0x50002u;
            pMallocFreeStruct = IntPtr.Zero;
            MIDLVersion = midlVersion;
            aUserMarshalQuadruple = IntPtr.Zero;
            NotifyRoutineTable = IntPtr.Zero;
            mFlags = new IntPtr(0x00000001);
            CsRoutineTables = IntPtr.Zero;
            ProxyServerInfo = IntPtr.Zero;
            pExprInfo = IntPtr.Zero;
        }
    }
}
