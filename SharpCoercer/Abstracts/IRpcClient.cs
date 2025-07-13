using System;

namespace SharpCoercer.Abstracts
{
    internal interface IRpcClient
    {
        /// <summary> Gets the name of the RPC client.</summary>
        string Name { get; }
        /// <summary> Gets the description of the RPC client.</summary>
        string Description { get; }
        /// <summary> Gets a value indicating whether the RPC client supports HTTP coercion.</summary>
        bool HttpCoerce { get; }

        /// <summary>
        /// Binds the RPC client the specified computer handle.
        /// </summary>
        /// <param name="hComputer"></param>
        /// <returns></returns>
        IntPtr Bind(IntPtr hComputer);

        /// <summary> Gets Coerce functions available in the RPC client.</summary>
        CoerceFunction[] GetFunctions();

        /// <summary> Sets the authentication details for the RPC client.</summary>
        void SetAuthenticationDetails(string domain, string username, string passwrod);
        
        /// <summary>
        /// Retrieves the name of the pipe used for inter-process communication.
        /// </summary>
        string GetPipeName();
    }
}
