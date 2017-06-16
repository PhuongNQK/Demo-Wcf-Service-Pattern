namespace Interfaces.Demo.Services.HelloWcf.MessageContracts
{
    /// <summary>
    /// Practically, responses should not inherit from requests. But here, 
    /// I use it to demonstrate both generics and inheritance with the new-styled
    /// pattern.
    /// 
    /// This message contract follows RPC style. If you want to make it
    /// message-styled contract, you need to apply <see cref="MessageContractAttribute"/> 
    /// to the class and <see cref="MessageBodyMemberAttribute"/>  to the properties.
    /// For example:
    ///     [MessageContract(WrapperName = "GenericRequestOf{0}")]
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericResponse<T> : GenericRequest<T>
    {
    }
}