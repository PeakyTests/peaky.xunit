using System.Xml.Serialization;

namespace Peaky.XUnit.Tests.Model
{
    public class UnitTestResult
    {   
        [XmlAttribute("testName")]
        public string TestName { get; set; }
        
        [XmlAttribute("outcome")]
        public string Outcome { get; set; }
    }
}