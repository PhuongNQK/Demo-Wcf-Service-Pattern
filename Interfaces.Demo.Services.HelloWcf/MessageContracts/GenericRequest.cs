using System.Collections.Generic;

using Interfaces.Demo.Services.HelloWcf.DataContracts;

namespace Interfaces.Demo.Services.HelloWcf.MessageContracts
{
    /// <summary>
    /// This message contract follows RPC style. If you want to make it
    /// message-styled contract, you need to apply <see cref="MessageContractAttribute"/> 
    /// to the class and <see cref="MessageBodyMemberAttribute"/>  to the properties.
    /// For example:
    ///     [MessageContract(WrapperName = "GenericRequestOf{0}")]
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericRequest<T>
    {
        public T Data { get; set; }

        public GenericPerson<string> Person { get; set; }

        public List<GenericPerson<int>> People { get; set; }

        public Dictionary<string, List<GenericPerson<int>>> PeopleMap { get; set; }
    }
}