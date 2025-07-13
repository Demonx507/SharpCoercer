namespace SharpCoercer.Delegates
{
    public delegate string BuildUrlDelegate(
        string listener,
        int smbPort,
        int httpPort,
        bool isHttp = false);
}
