namespace SharpCoercer.RpcClients.Efs
{
    internal class EfsLsassRpcClient : EfsRpcClientBase
    {
        public EfsLsassRpcClient() : base("c681d488-d850-11d0-8c52-00c04fd90f7e", "\\PIPE\\lsass")
        {
        }
    }
}
