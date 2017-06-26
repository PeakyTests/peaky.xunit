using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using FluentAssertions;
using Peaky.Client;
using Peaky.XUnit.Tests.Model;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Peaky.XUnit.Tests
{
    public class Given_a_PeakyTest_class_running_against_the_demo_peaky_service
    {
        private static  TestRun _testRun;

        [Fact]
        public void It_identifies_15_tests()
        {
            _testRun.Results.Should().HaveCount(15);
        }

        [Fact]
        public void It_identifies_11_passing_tests()
        {
            _testRun
                .Results
                .Where(r => r.Outcome == "Passed")
                .Should().HaveCount(11);
        }

        [Fact]
        public void It_identifies_4_failing_tests()
        {
            _testRun
                .Results
                .Where(r => r.Outcome == "Failed")
                .Should().HaveCount(4);
        }

        [Fact]
        public void It_produces_tests_with_the_test_Uri_in_the_test_name()
        {
            var preString = "Peaky.Sample.Tests.PeakySampleTests.The_peaky_test_passes(url: ";
            var postString = ")";

            _testRun
                .Results
                .Select(r => r.TestName.Substring(preString.Length))
                .Select(n => n.Substring(0, n.Length - postString.Length))
                .Select(n => new Uri(n))
                .Where(u => u.IsAbsoluteUri)
                .Should().HaveCount(15);
        }

        static Given_a_PeakyTest_class_running_against_the_demo_peaky_service() 
        { 
            if (_testRun != null) 
            { 
                return; 
            } 
 
            GenerateTestRun(); 
        } 
        
        private static void GenerateTestRun()
        {
            var trxFileInfo = new FileInfo("Given_a_PeakyTest_class_running_against_the_demo_peaky_service.trx");

            var sampleTestProjectDirectory = GetSampleTestProjectDirectory();

            try
            {
                DotnetTest.TryRunTests(sampleTestProjectDirectory, trxFileInfo);

                _testRun = DeserializeTrxFile(trxFileInfo);
            }
            finally
            {
                trxFileInfo.Refresh();

                if (trxFileInfo.Exists)
                {
                    trxFileInfo.Delete();
                }
            }
        }

        private static TestRun DeserializeTrxFile(FileInfo trxFileInfo)
        {
            var serializer = new XmlSerializer(
                typeof(TestRun),
                new XmlRootAttribute("TestRun")
                {
                    Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010"
                });

            TestRun testRun;
            
            using (var trxFile = File.Open(
                trxFileInfo.FullName,
                FileMode.Open))
            {
                testRun = (TestRun) serializer.Deserialize(trxFile);
            }

            return testRun;
        }

        private static DirectoryInfo GetSampleTestProjectDirectory()
        {
            var sampleTestProjectDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
            
            while (sampleTestProjectDirectory.GetFiles("*.sln").Length != 1)
            {
                sampleTestProjectDirectory = sampleTestProjectDirectory.Parent;
            }
            
            sampleTestProjectDirectory =
                new DirectoryInfo(Path
                    .Combine(sampleTestProjectDirectory.FullName, "test", "Peaky.Sample.tests"));
            
            return sampleTestProjectDirectory;
        }
    }
}