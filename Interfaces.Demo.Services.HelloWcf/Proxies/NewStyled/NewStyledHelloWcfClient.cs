using System.ServiceModel;

using Demo.Services.Core;
using Interfaces.Demo.Services.HelloWcf.DataContracts;
using Interfaces.Demo.Services.HelloWcf.MessageContracts;

namespace Interfaces.Demo.Services.HelloWcf.Proxies
{
    public class NewStyledHelloWcfClient : ClientBase<IHelloWcf>, INewStyledHelloWcfClient // Note: our client can inherit like this: ClientBase<TInterface1>, TInterface2
    {
        private ServiceOperation<SayHelloRequest, SayHelloResponse> m_SayHelloOperation;
        private ServiceOperation<GenericRequest<Person>, GenericResponse<Person>> m_SayHelloGenericOperation, m_SayHelloGeneric2Operation;
        private ServiceOperation<GenericRequest<int>, GenericResponse<string>> m_SayHelloGeneric3Operation;
        


        public NewStyledHelloWcfClient() : base() { }

        public NewStyledHelloWcfClient(string endpointConfigName) : base(endpointConfigName) { }



        public ServiceOperation<SayHelloRequest, SayHelloResponse> SayHello
        {
            get
            {
                if (m_SayHelloOperation == null)
                {
                    m_SayHelloOperation = new ServiceOperation<SayHelloRequest, SayHelloResponse>(request => Channel.SayHello(request));
                }
                return m_SayHelloOperation;
            }
        }
        
        public ServiceOperation<GenericRequest<Person>, GenericResponse<Person>> SayHelloGeneric
        {
            get
            {
                if (m_SayHelloGenericOperation == null)
                {
                    m_SayHelloGenericOperation = new ServiceOperation<GenericRequest<Person>, GenericResponse<Person>>(request => Channel.SayHelloGeneric(request));
                }
                return m_SayHelloGenericOperation;
            }
        }

        public ServiceOperation<GenericRequest<Person>, GenericResponse<Person>> SayHelloGeneric2
        {
            get
            {
                if (m_SayHelloGeneric2Operation == null)
                {
                    m_SayHelloGeneric2Operation = new ServiceOperation<GenericRequest<Person>, GenericResponse<Person>>(request => Channel.SayHelloGeneric2(request));
                }
                return m_SayHelloGeneric2Operation;
            }
        }

        public ServiceOperation<GenericRequest<int>, GenericResponse<string>> SayHelloGeneric3
        {
            get
            {
                if (m_SayHelloGeneric3Operation == null)
                {
                    m_SayHelloGeneric3Operation = new ServiceOperation<GenericRequest<int>, GenericResponse<string>>(request => Channel.SayHelloGeneric3(request));
                }
                return m_SayHelloGeneric3Operation;
            }
        }
    }
}