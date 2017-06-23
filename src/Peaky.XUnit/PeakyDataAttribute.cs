using System;
using Xunit;

namespace Peaky.XUnit
{
    public class PeakyDataAttribute : ClassDataAttribute
    {
        public PeakyDataAttribute(Type testType) : base(testType)
        {
        }
        
    }
}
