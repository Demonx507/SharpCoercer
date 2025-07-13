using SharpCoercer.RpcClients.Efs;

namespace SharpCoercer.RpcClients
{
    internal class EfsrpcClient : EfsRpcClientBase
    {
        public EfsrpcClient() : base("df1941c5-fe89-4e79-bf10-463657acf44d", "\\PIPE\\efsrpc")
        {
        }
    }
}
