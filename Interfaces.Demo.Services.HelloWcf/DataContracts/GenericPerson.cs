using System.Runtime.Serialization;

namespace Interfaces.Demo.Services.HelloWcf.DataContracts
{
    [DataContract(Name = "GenericPersonOf{0}")]
    public class GenericPerson<T>
    {
        [DataMember]
        public T SSID { get; set; }
    }
}