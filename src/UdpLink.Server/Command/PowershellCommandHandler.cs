namespace UdpLink.Server.Command
{
    using System;
    using System.Diagnostics;
    using System.IO;

    using UdpLink.Shared.Helpers;
    using UdpLink.Shared.Command;
    using System.Text;

    public class PowershellCommandHandler : CommandHandlerBase<PowershellCommand>
    {
        public override string Handle(PowershellCommand payload)
        {
            try
            {
                var path = Path.Combine(ProgramUtils.GetBaseDir(), "scripts", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss_fff") + ".ps1");
                var fi = new FileInfo(path);

                fi.Directory.Create();
                File.WriteAllText(path, payload.CommandText, Encoding.UTF8);

                var outputSb = new StringBuilder();
                var errorSb = new StringBuilder();

                var processStartInfo = new ProcessStartInfo
                {
                    StandardOutputEncoding = Encoding.GetEncoding(850),
                    StandardErrorEncoding = Encoding.GetEncoding(850),

                    FileName = "powershell.exe",
                    Arguments = $@"-File  ""{ fi.FullName}"" -NoLogo -ExecutionPolicy bypass",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                };

                using Process process = new Process();

                process.StartInfo = processStartInfo;

                process.OutputDataReceived += (sender, eventArgs) => outputSb.AppendLine(eventArgs.Data);
                process.ErrorDataReceived += (sender, eventArgs) => errorSb.AppendLine(eventArgs.Data);

                process.Start();

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit();

                string output = outputSb.ToString();
                string err = errorSb.ToString();

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
