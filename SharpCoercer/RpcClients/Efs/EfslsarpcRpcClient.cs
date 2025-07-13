namespace SharpCoercer.RpcClients.Efs
{
    internal class EfslsarpcRpcClient : EfsRpcClientBase
    {
        public EfslsarpcRpcClient() : base("c681d488-d850-11d0-8c52-00c04fd90f7e", "\\pipe\\lsarpc")
        {
        }
    }
}
