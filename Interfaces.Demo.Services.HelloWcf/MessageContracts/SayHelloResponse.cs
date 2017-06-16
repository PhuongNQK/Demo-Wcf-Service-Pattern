namespace Interfaces.Demo.Services.HelloWcf.MessageContracts
{
    /// <summary>
    /// This message contract follows RPC style. If you want to make it
    /// message-styled contract, you need to apply <see cref="MessageContractAttribute"/> 
    /// to the class and <see cref="MessageBodyMemberAttribute"/>  to the properties.
    /// </summary>
    public class SayHelloResponse
    {
        public string Message { get; set; }
    }
}