using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
            var psi = new ProcessStartInfo
            {
                Arguments = $"test --logger \"trx;LogFileName={logFileName.FullName}\"",
                FileName = "dotnet",
                WorkingDirectory = workingDirectory.FullName
            };

            var proc = Process.Start(psi);

            proc.WaitForExit();

            return proc.ExitCode == 0;
        }
    }
}

