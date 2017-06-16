using System;

using Interfaces.Demo.Services.HelloWcf;
using Interfaces.Demo.Services.HelloWcf.DataContracts;
using Interfaces.Demo.Services.HelloWcf.MessageContracts;

namespace Demo.Services.HelloWcf
{
    public class HelloWcf : IHelloWcf
    {
        public SayHelloResponse SayHello(SayHelloRequest request)
        {
            Person person = request.Person;

            int number;
            if (person.Age <= 0 || int.TryParse(person.Name, out number))
            {
                throw new ArgumentException("Invalid person name or age.");
            }

            var response = new SayHelloResponse()
            {
                Message = string.Format("Hello {0}. You're {1} years old now.", person.Name, person.Age)
            };
            return response;
        }
        
        public GenericResponse<Person> SayHelloGeneric(GenericRequest<Person> request)
        {
            var response = new GenericResponse<Person>()
            {
                Data = request.Data,
                Person = request.Person,
                People = request.People,
                PeopleMap = request.PeopleMap
            };
            return response;
        }

        public GenericResponse<Person> SayHelloGeneric2(GenericRequest<Person> request)
        {
            var response = SayHelloGeneric(request);
            return response;
        }

        public GenericResponse<string> SayHelloGeneric3(GenericRequest<int> request)
        {
            var response = new GenericResponse<string>()
            {
                Data = request.Data.ToString(),
                Person = request.Person,
                People = request.People,
                PeopleMap = request.PeopleMap
            };
            return response;
        }
    }
}