using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using FluentAssertions;
using Peaky.Client;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Peaky.XUnit.Tests
{
    public static class DotnetTest
    {
        public static bool TryRunTests(DirectoryInfo workingDirectory, FileInfo logFileName)
        {
            StringBuilder stdOut = new StringBuilder();
            StringBuilder stdErr = new StringBuilder();

            using (AutoResetEvent outWaitHandle = new AutoResetEvent(false))
            using (AutoResetEvent errWaitHandle = new AutoResetEvent(false))
            {
                var psi = new ProcessStartInfo
                {
                    Arguments = $"test --logger \"trx;LogFileName={logFileName.FullName}\"",
                    FileName = "dotnet",
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    WorkingDirectory = workingDirectory.FullName
                };
                
                using (Process proc = new Process())
                {
                    proc.StartInfo = psi;
                    
                    proc.BeginOutputReadLine();
                    proc.BeginErrorReadLine();

                    proc.OutputDataReceived += (sender, e) => {
                        if (e.Data == null)
                        {
                            outWaitHandle.Set();
                        }
                        else
                        {
                            stdOut.AppendLine(e.Data);
                        }
                    };

                    proc.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data == null)
                        {
                            errWaitHandle.Set();
                        }
                        else
                        {
                            stdErr.AppendLine(e.Data);
                        }
                    };

                    proc.Start();

                    proc.BeginOutputReadLine();
                    proc.BeginErrorReadLine();

                    var timeout = 60000;

                    proc.WaitForExit(timeout);
                    outWaitHandle.WaitOne(timeout);
                    errWaitHandle.WaitOne(timeout);
                    
                    return proc.ExitCode == 0;
                }
            }
        }
    }
}

