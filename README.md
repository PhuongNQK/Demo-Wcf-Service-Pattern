# Prerequisites
1. This demo solution assumes that you have basic knowledge of WCF concepts and have created at least one WCF service. In case you are not sure about that, then you should read through [these WCF tutorials](https://www.tutorialspoint.com/wcf/index.htm) first.
2. For details on the configuration used in the demo, see: https://docs.microsoft.com/en-us/dotnet/framework/configure-apps/file-schema/wcf/system-servicemodel

# Introduction
WCF is a great software development kit for developing and deploying services, many types from Internet-based to Intranet-based. With the assistance of Visual Studio IDE and SDK tools, it is even easier to apply WCF. What you typically do to create and consume a service should be:
1. For the service:
..- Create a project for the service
..- Define an interface with proper attributes (for example, *ServiceContract*, *OperationContract*) applied
..- Implement that interface
..- Implement a host for it
..- Build, configure and run that project so that we have a service ready for comsumption
2. For the service client:
..- Create a project for the service client
..- Use *Add Service Reference* command to generate proxy code to the service
..- Add the code that uses the auto-generated proxy

Those steps work, but there are some limitations:
1. Previously, the generated proxy code could not contain generics. For each original generics-based class, there would be a non-generic version to be generated. Because I am a fan of generics, this limitation made me annoyed a lot. Luckily, this limitation has been removed at least from Visual Studio 2015.
2. When we change anything in the original classes such as renaming, adding or removing elements, etc., we have to run *Update Service Reference* command to reflect the changes in the generated code. The most annoying case of changes is renaming a type member (property, method, etc.) that has been used in many places because you have to manually fix relevant places while it seems the tool should be able to do it for you.
3. If we run *Add Service Reference* to the same service for more than one project, we create a clone of the generated classes. When we update the service interface or contract, we need to run *Update Service Reference* in all relevant projects to update all clones. That is sure to be a tedious task. 
4. The generated proxy code depends on the version of the tool. When the tool is upgraded to generate better code and we want that better code for existing generated references, we need to run *Update Service Reference* for those existing references.
5. Depending on our settings, the generated code can contain a lot of code which we may never need. With all settings, the generated code is always lengthy with fully-qualified types like this:
```CSharp
public System.Threading.Tasks.Task<Interfaces.Demo.Services.HelloWcf.MessageContracts.GenericResponse<string>> SayHelloGenericAsync(Interfaces.Demo.Services.HelloWcf.MessageContracts.GenericRequest<int> request) {
	return base.Channel.SayHelloGenericAsync(request);
}
```
while this more readable version is often preferred:
```CSharp
public Task<GenericResponse<string>> SayHelloGenericAsync(GenericRequest<int> request) {
	return base.Channel.SayHelloGenericAsync(request);
}
```

Luckily, those limitations can be fixed. If we extract the generated code and refactor it a bit, we can get a solution that:
1. Free us from running *Add/Update Service Reference* (not a single time, except when we want to test those commands)
2. Contain no redundant code, i.e. no need for clones of generated classes
3. Centralize changes into one place. For example, we can rename a type member using the handy *Refactor -> Rename* command. When we want to improve the proxy code, we do it in one place and it will be available to all references.
4. This is minor again, but we will not have to see lengthy code if we do not intentionally write it out.

Of course, we need to do a little setup, but that should be fairly easy!

# About the demo
## Problem statement
I have a simple service:
```CSharp
public interface IHelloWcf
{
	[OperationContract]
	SayHelloResponse SayHello(SayHelloRequest request);
	
	...
}
```
I want to host it in a Windows service and a console application so that it can be consumed by either a web or console application.

## Solution
The code will be organized using these projects:
1. **Demo.Services.Core**: contains the core classes that can be used by all services. Do not add non-service common features to this assembly, or unnecessary dependencies will be required between non-service assemblies and this one.
2. **Interfaces.Demo.Services.X**: contains all service / message / data contracts for service X (or service group X). It also contains the necessary proxies for X, which makes it depend on **Demo.Services.Core**. As a result, X's consumers only need to reference this project and **Demo.Services.Core** to be able to work with X.
3. **Demo.Services.X**: contains all service implementations for service X (or service group X). It depends on **Demo.Services.Core** and **Interfaces.Demo.Services.X**.
4. **Demo.Services.XHost**, **Demo.ConsoleApp.XHost**: each contains a different type of host for service X (or service group X). They depend on **Demo.Services.X**.
5. **Demo.Web.X**, **Demo.ConsoleApp.XClient**: each contains a different type of X's clients.

With the above setup, if you run *Add Service Reference* using only the following *Advanced* options checked:
1. *Allow generation of asynchronous operations* and *Generate task-based operations*
2. *Reuse types in referenced assemblies* and *Reuse types in all referenced assemblies*
you will have a rather nice generated code. Try it!

Note:
- All message contracts (i.e. Request/Response) classes must have a parameterless constructor and all properties should be get/set-able. Otherwise, serialization exceptions may be thrown.
- In the code and comments, *OldStyled* refers to elements that use/demonstrate how to use WCF client the old way while *NewStyled* refers to elements that use/demonstrate how to use WCF client the new way.
- In *Interfaces.Demo.Services.HelloWcf/Proxies*, there are 2 folders named *NewStyled* and *OldStyled*. They are merely for easy comparison, and should not be used in production code.
- For simple solutions with just a few services or for services that are not reusable a lot, we can combine **Demo.Services.X** and **Demo.Services.XHost** into one assembly.

## How to build
- It requires .NET 4.5.2 and ASP.NET MVC 4 or later
- It can be built using Visual Studio 2015 or later

## How to run
The current configuration allows us to test the whole solution in a single PC. 
1. Install *Demo.Services.HelloWcfHost* as a Windows service and start it so that the service will be available at *localhost:8001*. File *hello-wcf-host.bat* can help you with all that: 
..1. Open a *Command Prompt* as Administrator from the folder containing that file (*...\Demo.Services.HelloWcfHost*).
..2. Type *hello-wcf-host.bat install* to install the test service
..3. Type *hello-wcf-host.bat start* to start it
..4. After testing, you can run *hello-wcf-host.bat uninstall* to remove the test service.
2. Run *Demo.ConsoleApp.HelloWcfHost* so that the service will be available at *localhost:8003*.
3. Update web.config and run *Demo.Web.HelloWcf* to connect to the service you love. In the current setting, it will connect to *localhost:8001*.
4. Update app.config and run *Demo.ConsoleApp.HelloWcfClient* to connect to the service you love. In the current setting, it will connect to *localhost:8001*.

Enjoy :)!