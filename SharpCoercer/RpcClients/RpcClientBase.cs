using SharpCoercer.Delegates;
using SharpCoercer.Win32;
using SharpCoercer.Win32.Structs;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace SharpCoercer.RpcClients
{
    internal class RpcClientBase
    {



        protected Guid _interfaceId;

        protected string _pipeName ;

        protected  uint RPCTimeOut = 5000;

        protected AllocMemoryDelegate AllocateMemoryDelegate = AllocateMemory;
        protected FreeMemoryDelegate FreeMemoryDelegate = FreeMemory;



        protected BindDelegate _bindDelegate;
        protected UnBindDelegate _unbindDelegate;
        protected string _domain;
        protected string _username;
        protected string _password;

        protected GCHandle clientInterface;
        protected GCHandle formatString;
        protected GCHandle stub;
        protected GCHandle bindinghandle;
        protected IntPtr rpcClientInterface;


        protected byte[] _midl_typeFromat;
        protected byte[] _midl_procFromat;



        public string GetPipeName() 
        {
            return _pipeName;
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected static IntPtr AllocateMemory(int size)
        {
            IntPtr memory = Marshal.AllocHGlobal(size);
            Trace.WriteLine("allocating " + memory.ToString());
            return memory;
        }


        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected static void FreeMemory(IntPtr memory)
        {
            Trace.WriteLine("freeing " + memory.ToString());
            Marshal.FreeHGlobal(memory);
        }


        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]

        protected IntPtr GetProcStringHandle(int offset)
        {
            return Marshal.UnsafeAddrOfPinnedArrayElement(_midl_procFromat, offset);
        }


        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected static void Unbind(IntPtr IntPtrserver, IntPtr hBinding)
        {
            //server name
            string server = Marshal.PtrToStringUni(IntPtrserver);


            Trace.WriteLine("unbinding " + server);

            //Free binding
            Rpcrt4.RpcBindingFree(ref hBinding);
        }


        public void SetAuthenticationDetails(string domain ,string username,string passwrod) 
        {
            _domain = domain;
            _username = username;
            _password = passwrod;
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected IntPtr GetStubHandle()
        {
            return stub.AddrOfPinnedObject();
        }
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected int AuthenticateRpcBinding(IntPtr binding, string server)
        {

            int status = 0;
            // Set authentication information for the binding
            if (!string.IsNullOrEmpty(_username))
            {
                // Build identity:
                var identity = new SecWinNTAuthIdentity
                {
                    User = _username,
                    UserLength = (ushort)_username.Length,  // count of WCHARs
                    Domain = _domain,
                    DomainLength = (ushort)_domain.Length,
                    Password = _password,
                    PasswordLength = (ushort)_password.Length,
                    Flags = Constants.SEC_WINNT_AUTH_IDENTITY_UNICODE
                };

                // Set authentication info with identity
                status = Rpcrt4.RpcBindingSetAuthInfoEx(
                    binding,
                    server,
                    Constants.RPC_C_AUTHN_LEVEL_PKT_PRIVACY,  // 6
                    Constants.RPC_C_AUTHN_WINNT,             // 10
                    ref identity,
                    Constants.RPC_C_AUTHZ_NONE,
                    IntPtr.Zero
                );
            }
            else
            {
                // Set authentication info without identity (using current context)
                status = Rpcrt4.RpcBindingSetAuthInfo(
                   binding,
                   server,
                   Constants.RPC_C_AUTHN_LEVEL_PKT_PRIVACY,  // 6
                   Constants.RPC_C_AUTHN_WINNT,             // 10
                   IntPtr.Zero, //
                   Constants.RPC_C_AUTHZ_NONE
               );
            }
            return status;
        }
    }
}
