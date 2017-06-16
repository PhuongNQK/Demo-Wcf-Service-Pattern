using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Threading.Tasks;

using Demo.Web.HelloWcf.Constants;
using Interfaces.Demo.Services.HelloWcf.DataContracts;
using Interfaces.Demo.Services.HelloWcf.MessageContracts;
using Interfaces.Demo.Services.HelloWcf.Proxies;

namespace Demo.Web.HelloWcf.Controllers
{
    public class HelloWcfController : AsyncController
    {
        #region HelloWcfClient: Old-styled vs. New-styled

        #region OldStyledHelloWcfClient

        /// <summary>
        /// Demonstrate how to use <see cref="IOldStyledHelloWcfClient"/> synchronously.
        /// </summary>
        /// <returns></returns>
        public JsonResult OldStyled_Sync()
        {
            var person = new Person()
            {
                Name = nameof(OldStyled_Sync),
                Age = 30
            };
            var request = new SayHelloRequest() { Person = person };

            string message;
            using (var client = GetOldStyledHelloWcfClient())
            {
                message = client.SayHello(request).Message;
            }

            return JsonGet(new { data = message });
        }

        /// <summary>
        /// Demonstrate how to use <see cref="IOldStyledHelloWcfClient"/> asynchronously
        /// via Begin/End (BE) pattern.
        /// </summary>
        public void OldStyled_BEAsync()
        {
            AsyncManager.OutstandingOperations.Increment();

            var person = new Person()
            {
                Name = "OldStyled_BE",
                Age = 30
            };
            var request = new SayHelloRequest() { Person = person };

            var client = GetOldStyledHelloWcfClient();
            var callback = new AsyncCallback(ar =>
            {
                AsyncManager.Parameters["response"] = client.EndSayHello(ar);
                AsyncManager.OutstandingOperations.Decrement();
            });

            client.BeginSayHello(request, callback, null);
        }

        public JsonResult OldStyled_BECompleted(SayHelloResponse response)
        {
            return JsonGet(new { data = response.Message });
        }

        /// <summary>
        /// Demonstrate how to use <see cref="IOldStyledHelloWcfClient"/> asynchronously
        /// via Async/Completed (AC) pattern.
        /// </summary>
        public void OldStyled_ACAsync()
        {
            AsyncManager.OutstandingOperations.Increment();

            var person = new Person()
            {
                Name = "OldStyled_AC",
                Age = 30
            };
            var request = new SayHelloRequest() { Person = person };

            var client = GetOldStyledHelloWcfClient();
            client.SayHelloCompleted += (sender, args) =>
            {
                AsyncManager.Parameters["response"] = args.Result;
                AsyncManager.OutstandingOperations.Decrement();
            };
            client.SayHelloAsync(request);
        }

        public JsonResult OldStyled_ACCompleted(SayHelloResponse response)
        {
            return JsonGet(new { data = response.Message });
        }

        #endregion HelloWcfClient



        #region NewStyledHelloWcfClient

        /// <summary>
        /// Demonstrate how to use <see cref="INewStyledHelloWcfClient"/> synchronously.
        /// </summary>
        /// <returns></returns>
        public JsonResult NewStyled_Sync()
        {
            var person = new Person()
            {
                Name = nameof(NewStyled_Sync),
                Age = 30
            };
            var request = new SayHelloRequest() { Person = person };
            string message;
            using (var client = GetNewStyledHelloWcfClient())
            {
                message = client.SayHello.Operate(request).Message;
            }

            return JsonGet(new { data = message });
        }

        /// <summary>
        /// Demonstrate how to use <see cref="INewStyledHelloWcfClient"/> asynchronously 
        /// via Begin/End (BE) pattern.
        /// </summary>
        public void NewStyled_BEAsync()
        {
            AsyncManager.OutstandingOperations.Increment();

            var person = new Person()
            {
                Name = "NewStyled_BE",
                Age = 30
            };
            var request = new SayHelloRequest() { Person = person };

            var client = GetNewStyledHelloWcfClient();
            var operation = client.SayHello;
            var callback = new AsyncCallback(ar =>
            {
                AsyncManager.Parameters["response"] = operation.EndOperate(ar);
                AsyncManager.OutstandingOperations.Decrement();
            });

            operation.BeginOperate(request, callback, null);
        }

        public JsonResult NewStyled_BECompleted(SayHelloResponse response)
        {
            return JsonGet(new { data = response.Message });
        }

        /// <summary>
        /// Demonstrate how to use <see cref="INewStyledHelloWcfClient"/> asynchronously 
        /// via Async/Completed (AC) pattern.
        /// </summary>
        public void NewStyled_ACAsync()
        {
            AsyncManager.OutstandingOperations.Increment();

            var person = new Person()
            {
                Name = "NewStyled_AC",
                Age = 30
            };
            var request = new SayHelloRequest() { Person = person };

            var client = GetNewStyledHelloWcfClient();
            var operation = client.SayHello;
            operation.OperationCompleted += (sender, args) =>
            {
                AsyncManager.Parameters["response"] = args.Result;
                AsyncManager.OutstandingOperations.Decrement();
            };
            operation.OperateAsync(request);
        }

        public JsonResult NewStyled_ACCompleted(SayHelloResponse response)
        {
            return JsonGet(new { data = response.Message });
        }

        /// <summary>
        /// Demonstrate how to use <see cref="INewStyledHelloWcfClient"/> asynchronously 
        /// via Task-based pattern when async/await keywords are not available, i.e. 
        /// in MVC3 or earlier.
        /// </summary>
        public void NewStyled_TaskAsync()
        {
            AsyncManager.OutstandingOperations.Increment();

            var person = new Person()
            {
                Name = "NewStyled_Task",
                Age = 30
            };
            var request = new SayHelloRequest() { Person = person };

            var client = GetNewStyledHelloWcfClient();
            var operation = client.SayHello;
            operation
                .OperateAsTask(request)
                .ContinueWith(antecedent =>
                {
                    AsyncManager.Parameters["response"] = antecedent.Result;
                    AsyncManager.OutstandingOperations.Decrement();
                });
        }

        public JsonResult NewStyled_TaskCompleted(SayHelloResponse response)
        {
            return JsonGet(new { data = response.Message });
        }

        /// <summary>
        /// Demonstrate how to use <see cref="INewStyledHelloWcfClient"/> asynchronously 
        /// via Task-based pattern when async/await keywords are available, i.e. in MVC4
        /// or later.
        /// </summary>
        public async Task<JsonResult> NewStyled_TaskWithAsyncAwait()
        {
            var person = new Person()
            {
                Name = nameof(NewStyled_TaskWithAsyncAwait),
                Age = 30
            };
            var request = new SayHelloRequest() { Person = person };

            using (var client = GetNewStyledHelloWcfClient())
            {
                SayHelloResponse response = await client.SayHello.OperateAsTask(request);
                return JsonGet(new { data = response.Message });
            }
        }

        #endregion NewStyledHelloWcfClient

        #endregion HelloWcfClient: Old-styled vs. New-styled



        /*
        * This region demonstrates that we can use generics to work with WCF 
        * calls using <see cref="NewStyledHelloWcfClient"/>, which is not 
        * possible using <see cref="OldStyledHelloWcfClient"/>.
        */
        #region Generic request/response
 
        public JsonResult NewStyled_Generic()
        {
            var data = new Person() { Name = nameof(NewStyled_Generic), Age = 30 };
            var request = BuildGenericRequest(data);
            
            GenericResponse<Person> response;
            using (var client = GetNewStyledHelloWcfClient())
            {
                response = client.SayHelloGeneric.Operate(request);
            }

            return JsonGet(response);
        }

        public JsonResult NewStyled_Generic2()
        {
            var data = new Person() { Name = nameof(NewStyled_Generic2), Age = 30 };
            var request = BuildGenericRequest(data);

            GenericResponse<Person> response;
            using (var client = GetNewStyledHelloWcfClient())
            {
                response = client.SayHelloGeneric2.Operate(request);
            }

            return JsonGet(response);
        }

        public JsonResult NewStyled_Generic3()
        {
            var request = BuildGenericRequest(100);

            GenericResponse<string> response;
            using (var client = GetNewStyledHelloWcfClient())
            {
                response = client.SayHelloGeneric3.Operate(request);
            }

            return JsonGet(response);
        }

        private static GenericRequest<TData> BuildGenericRequest<TData>(TData data)
        {
            var person = new GenericPerson<string>()
            {
                SSID = "D001"
            };

            var people = new List<GenericPerson<int>>() {
                new GenericPerson<int>(){ SSID = 1 },
                new GenericPerson<int>(){ SSID = 2 }
            };

            var peopleMap = new Dictionary<string, List<GenericPerson<int>>>()
            {
                {"001", people},
                {"002", people}
            };

            var request = new GenericRequest<TData>()
            {
                Data = data,
                Person = person,
                People = people,
                PeopleMap = peopleMap
            };

            return request;
        }

        #endregion



        #region Utils

        private JsonResult JsonGet(object data)
        {
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        
        private static NewStyledHelloWcfClient GetNewStyledHelloWcfClient()
        {
            return new NewStyledHelloWcfClient(Names.ClientEndpoints.HelloWcf);
        }

        private static OldStyledHelloWcfClient GetOldStyledHelloWcfClient()
        {
            return new OldStyledHelloWcfClient(Names.ClientEndpoints.HelloWcf);
        }

        #endregion
    }
}