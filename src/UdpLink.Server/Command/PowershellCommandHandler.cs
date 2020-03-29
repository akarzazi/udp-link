namespace UdpLink.Server.Command
{
    using System;
    using System.Diagnostics;
    using System.IO;

    using UdpLink.Shared.Helpers;
    using UdpLink.Shared.Command;

    public class PowershellCommandHandler : CommandHandlerBase<PowershellCommand>
    {
        public override string Handle(PowershellCommand payload)
        {
            try
            {
                var path = Path.Combine(ProgramUtils.GetBaseDir(), "scripts", DateTime.Now.ToString("HH-mm-ss_fff") + ".ps1");
                var fi = new FileInfo(path);

                fi.Directory.Create();
                File.WriteAllText(path, payload.CommandText);

                Process process = new Process();
                process.StartInfo.FileName = "powershell.exe";
                process.StartInfo.Arguments = $@"-File  ""{ fi.FullName}"" -NoLogo ";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string err = process.StandardError.ReadToEnd();

                process.WaitForExit();

                return
$@"
Command:
{payload.CommandText}

Standardoutput :
{output} 

ErrorOuput:
{err}  ";
            }
            catch (Exception ex)
            {
                return
$@" Command:
{ payload.CommandText}

ExecutionError:
{ ex}
";
            }
        }
    }
}
