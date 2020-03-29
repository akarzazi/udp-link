
$solutionRoot = (get-item $PSScriptRoot ).parent.FullName;

$params = @{
    Name = "UdpLinkServer"
    BinaryPathName = "$solutionRoot\out\UdpLink.Server.exe"
    DisplayName = "UdpLinkServer"
    StartupType = "Manual"
  }
  
Remove-Service $params.Name

New-Service @params