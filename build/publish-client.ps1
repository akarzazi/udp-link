$solutionRoot = (get-item $PSScriptRoot ).parent.FullName;
dotnet publish "$solutionRoot\src\UdpLink.Client" -o "$solutionRoot\out\udp-link-client\" -r win-x64 -c Release --self-contained true  /p:PublishSingleFile=true