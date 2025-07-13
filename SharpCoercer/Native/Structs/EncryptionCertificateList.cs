using System;
using System.Runtime.InteropServices;

namespace SharpCoercer.Win32.Structs
{

    [StructLayout(LayoutKind.Sequential)]
    public struct EFS_RPC_BLOB
    {
        public uint cbData;
        public IntPtr bData;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ENCRYPTION_CERTIFICATE
    {
        public uint cbTotalLength;
        public IntPtr pUserSid;
        public IntPtr pCertBlob;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct EncryptionCertificateList
    {
        public uint nUsers;
        public IntPtr pUsers; // pointer to array of ENCRYPTION_CERTIFICATE*

        public static IntPtr BuildFakeEncryptionCertificateList()
        {
            // 1. Dummy SID: S-1-5-32-544 (Administrators)
            byte[] sid = new byte[] { 1, 1, 0, 0, 0, 0, 0, 5, 32, 2 };
            IntPtr sidPtr = Marshal.AllocHGlobal(sid.Length);
            Marshal.Copy(sid, 0, sidPtr, sid.Length);

            // 2. Dummy certificate blob
            byte[] certBytes = new byte[] { 0x01, 0x02, 0x03 };
            IntPtr certDataPtr = Marshal.AllocHGlobal(certBytes.Length);
            Marshal.Copy(certBytes, 0, certDataPtr, certBytes.Length);

            EFS_RPC_BLOB blob = new EFS_RPC_BLOB
            {
                cbData = (uint)certBytes.Length,
                bData = certDataPtr
            };
            IntPtr blobPtr = Marshal.AllocHGlobal(Marshal.SizeOf<EFS_RPC_BLOB>());
            Marshal.StructureToPtr(blob, blobPtr, false);

            // 3. ENCRYPTION_CERTIFICATE
            ENCRYPTION_CERTIFICATE cert = new ENCRYPTION_CERTIFICATE
            {
                cbTotalLength = (uint)Marshal.SizeOf<ENCRYPTION_CERTIFICATE>(),
                pUserSid = sidPtr,
                pCertBlob = blobPtr
            };
            IntPtr certPtr = Marshal.AllocHGlobal(Marshal.SizeOf<ENCRYPTION_CERTIFICATE>());
            Marshal.StructureToPtr(cert, certPtr, false);

            // 4. Array of ENCRYPTION_CERTIFICATE pointers
            IntPtr certArray = Marshal.AllocHGlobal(IntPtr.Size);
            Marshal.WriteIntPtr(certArray, certPtr);

            // 5. ENCRYPTION_CERTIFICATE_LIST
            var certList = new EncryptionCertificateList
            {
                nUsers = 1,
                pUsers = certArray
            };
            IntPtr certListPtr = Marshal.AllocHGlobal(Marshal.SizeOf<EncryptionCertificateList>());
            Marshal.StructureToPtr(certList, certListPtr, false);

            return certListPtr;
        }
    }

}
