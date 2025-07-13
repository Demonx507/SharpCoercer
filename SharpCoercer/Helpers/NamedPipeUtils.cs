namespace SharpCoercer.Helpers
{
    using System.ComponentModel;
    using System.Runtime.InteropServices;

    public static class NamedPipeUtils
    {
        // P/Invoke declaration for WaitNamedPipe
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool WaitNamedPipe(string name, uint timeout);

        /// <summary>
        /// Checks whether a named pipe exists on a remote server.
        /// </summary>
        /// <param name="server">The remote server name or IP (no slashes).</param>
        /// <param name="pipeName">The pipe name (without leading backslashes).</param>
        /// <returns>True if the pipe exists; false otherwise.</returns>
        public static bool RemotePipeExists(string server, string pipeName)
        {
            // Build the UNC path to the pipe
            string fullPipePath = $@"\\{server}{pipeName}";

            // timeout = 0 => return immediately
            bool exists = WaitNamedPipe(fullPipePath, 0);
            if (!exists)
            {
                int err = Marshal.GetLastWin32Error();
                // ERROR_FILE_NOT_FOUND (2) means "no such pipe"
                // ERROR_BAD_NETPATH (53) might mean server unreachable or no share.
                // You can handle/log specific errors if needed.
                if (err != 2)
                {
                    // for other errors you might want to log or rethrow
                    throw new Win32Exception(err, $"Error checking pipe {fullPipePath}");
                }
            }

            return exists;
        }
    }

}
