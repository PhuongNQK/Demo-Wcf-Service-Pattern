namespace Interfaces.Demo.Services.HelloWcf.Constants
{
    public static class Values
    {
        public static class Service
        {
            public const string InterfaceName = "IHelloWcf";

            /// <summary>
            /// This value is use to set the contract attribute of the 
            /// corresponding service endpoint. It cannot contain '/'.
            /// </summary>
            public const string ConfigurationName = "Demo.Services.HelloWcf";

            public const string Namespace = "http://Demo.Services.HelloWcf";
        }

        public static class Client
        {
            /// <summary>
            /// This value is use to set the contract attribute of the 
            /// corresponding client endpoint. It can be anything (as
            /// long as it does not contain '/'), but using the same 
            /// value as the corresponding service contract saves us 
            /// from remembering more than one value.
            /// </summary>
            public const string ConfigurationName = Service.ConfigurationName;
        }
    }
}