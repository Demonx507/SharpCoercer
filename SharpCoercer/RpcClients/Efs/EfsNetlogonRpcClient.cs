using SharpCoercer.RpcClients.Efs;

namespace SharpCoercer.RpcClients
{
    internal class EfsNetlogonRpcClient : EfsRpcClientBase
    {
        public EfsNetlogonRpcClient() : base("c681d488-d850-11d0-8c52-00c04fd90f7e", "\\pipe\\netlogon")
        {
        }
    }
}
