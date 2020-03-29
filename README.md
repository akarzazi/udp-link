# UDP - Link
Client / Server programs to perform remote operations over UDP.
The solution has two executables for the Client and Server.

## Goal
Execute remote commands and perform diagnostics over UDP.

## Flow
The client sends an AES encrypted payload to the server over UDP on a specific port.

The server receives the commands and sends back the results to the client.

## Features
### Commands
Command     | Description 
---         | --- 
Echo        | Server replies with the same payload
PS          | Server executes an inline **Powershell** command
PSF         | Server executes **Powershell** command from a local file on the client
Reboot      | Reboots the remote server

### Security
Payload is encrypted using AES with a private key known by the client and the server.

## Client configuration
The client configuration is contained in the file `appsettings.json`
```json
{
  "EndPointConfig": {
    "Host": "127.0.0.1",
    "Port": "11000",
    "Secret": "My_Secret_Key"
  }
}
```

## Server configuration
The server configuration is contained in the file `appsettings.json`
```json
{
  "ListenerConfig": {
    "Port": "11000",
    "Secret": "My_Secret_Key"
  },

  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  }
}
```

## Run the Server as windows Service
To install the Server as a Windows service, use the command below

```powershell
$params = @{
    Name = "UdpLinkServer"
    BinaryPathName = "path-to-binary\UdpLink.Server.exe"
    DisplayName = "UdpLinkServer"
    StartupType = "Manual"
  }
  
New-Service @params
```


