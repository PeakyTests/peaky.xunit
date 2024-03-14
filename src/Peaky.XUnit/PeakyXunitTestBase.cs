using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Peaky.Client;

namespace Peaky.XUnit;

public abstract class PeakyXunitTestBase : IEnumerable<object[]>
{
    public abstract PeakyClient PeakyClient { get; } 

    public IEnumerator<object[]> GetEnumerator()
    {
        return PeakyClient
               .GetTests()
               .Result
               .Select(t => new object [] {t.Url})
               .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}