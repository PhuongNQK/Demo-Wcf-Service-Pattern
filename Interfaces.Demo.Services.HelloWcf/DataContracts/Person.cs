using System.Runtime.Serialization;

namespace Interfaces.Demo.Services.HelloWcf.DataContracts
{
    [DataContract]
    public class Person
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Age { get; set; }
    }
}