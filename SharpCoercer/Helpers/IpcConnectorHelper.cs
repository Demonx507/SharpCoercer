using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace SharpCoercer.Helpers
{
 
    public static class IpcConnectorHelper
    {
        private const int RESOURCETYPE_DISK = 0x00000001;
        private const int NO_ERROR = 0;
        private const int CONNECT_UPDATE_PROFILE = 0;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct NETRESOURCE
        {
            public uint dwScope;
            public uint dwType;
            public uint dwDisplayType;
            public uint dwUsage;
            public string lpLocalName;
            public string lpRemoteName;
            public string lpComment;
            public string lpProvider;
        }

        [DllImport("mpr.dll", CharSet = CharSet.Unicode)]
        private static extern int WNetAddConnection2(
            ref NETRESOURCE lpNetResource,
            string lpPassword,
            string lpUserName,
            uint dwFlags
        );

        [DllImport("mpr.dll", CharSet = CharSet.Unicode)]
        private static extern int WNetCancelConnection2(
            string lpName,
            uint dwFlags,
            bool fForce
        );

        /// <summary>
        /// Mounts \\TARGET\IPC$ using the supplied credentials.
        /// </summary>
        public static bool ConnectToIpc(
            string targetHostname,
            string domain,
            string username,
            string password
        )
        {
            // Build \\<target>\IPC$
            string remote = $@"\\{targetHostname}\IPC$";

            // Clean up any existing connection
            WNetCancelConnection2(remote, CONNECT_UPDATE_PROFILE, true);

            var nr = new NETRESOURCE
            {
                dwType = RESOURCETYPE_DISK,
                lpRemoteName = remote,
                lpLocalName = null,
                lpProvider = null
            };

            int result = NO_ERROR;
            string user = string.Empty;
            if (string.IsNullOrEmpty(domain) && string.IsNullOrEmpty(username))
            {

                result = WNetAddConnection2(ref nr,null,null,0);
            }
            else 
            {
                user = $"{domain}\\{username}";
                result = WNetAddConnection2(
                    ref nr,
                    password,
                    user,
                    0
                );

            }
            // DOMAIN\Username

            if (result != NO_ERROR)
            {
                Console.WriteLine($"[-] WNetAddConnection2 failed: 0x{new Win32Exception(result).Message:X}");
                return false;
            }

            if(string.IsNullOrEmpty(user))
                user  = Environment.UserName;
            //write connect successfully

            Console.WriteLine($"[+] Connected to {remote} as {user}");
            return true;
        }
    }


}
