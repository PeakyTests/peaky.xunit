﻿using System;
using System.Collections.Generic;
using Peaky.Client;

namespace Peaky.XUnit
{
    public class PeakyXunitTests
    {
        public static IEnumerable<object[]> GetPeakyResults(string serviceUri)
        {
            using (var client = new PeakyClient(new Uri(serviceUri)))
            {
                foreach (var test in client.GetTests().Result)
                {
                    var result = client.GetResultFor(test).Result;

                    Console.WriteLine(result.GetType());
                    
                    yield return new object[] {test.Application, test.Environment, test.Tags, test.Url, result.Content, result.Passed};
                }
            }
        }
    }
}
