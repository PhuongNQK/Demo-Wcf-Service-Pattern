Subject/Title: A recommended pattern to create WCF references.

# Introduction
WCF is a great software development kit for developing and deploying services, many types from Internet-based to Intranet-based. With the assistance of Visual Studio IDE and SDK tools, it is even easier to apply WCF. What you typically do to create and consume a service should be:
1. For the service:
..- Create a project for the service
..- Define an interface with proper attributes (for example, ServiceContract, OperationContract) applied
..- Implement that interface
..- Implement a host for it
..- Build, configure and run that project so that we have a service ready for comsumption
2. For the service client:
..- Create a project for the service client
..- Use Add Service Reference command to generate proxy code to the service
..- Add the code that uses the auto-generated proxy

Those steps work, but there are some limitations:
1. Previously, the generated code did not contain generics. For each original generics-based class, there would be a non-generic version to be generated. Because I am a fan of generics, this limitation made me annoyed a lot. Luckily, this limitation has been removed at least from Visual Studio 2015.
2. When we change anything in the original classes such as renaming, adding or removing elements, etc., we have to run Update Service Reference command to reflect the changes in the generated code. The most annoying case of changes is renaming a type element (property, method, etc.) that has been used in many places because you have to manually fix relevant places while it seems the tool should be able to do it for you.
3. If we run Add Service Reference to the same service for more than one project, we create a clone of the generated classes. When we update the service interface or contract, we need to run Update Service Reference in all relevant projects to update all clones. That is sure to be a tedious task. 
4. Change deployment/hosting condition

This demo will try to This is not the right way 

# Prerequisites
This demo assumes that you have basic knowledge of WCF concepts and have created at least one WCF service. In case you are not sure about that, then you should read through [these WCF tutorials](https://www.tutorialspoint.com/wcf/index.htm) first.

For the configuration used in the demo, see also: https://docs.microsoft.com/en-us/dotnet/framework/configure-apps/file-schema/wcf/system-servicemodel

Let us start with some requirements:


When you work with WCF references, it is likely that you will use the Add Service Reference feature in Visual Studio IDE.

# Drawbacks of the traditional way
* Changes to data/message/service contracts are not immediately reflected if the code is not opened in Visual Studio IDE. For example, the build tool will still succeed because the reference code is not updated yet.
* Renaming does not work.
* 

Note:
- Assembly naming style: Interfaces.XXX, Tests.XXX, XXX
- In Interfaces.Demo.Services.HelloWcf/Proxies, there are 2 folders. They are merely for easy comparison, and should not be used in production code.
- All message contracts (i.e. Request/Response) classes must have a parameterless constructor and all properties should be get/set-able. Otherwise, serialization exceptions may be thrown.

Here are needed assemblies:
- Demo.Services.Core: contains the core classes that can be used by all services. Do not add non-service common features to this assembly, or unnecessary dependencies will be required between non-service assemblies and this one.
- Interfaces.Demo.Services.X: contains all service / message / data contracts for service X. Also contains the necessary proxies for X. As a result, X's consumers only need to reference this assembly to be able to work with X.
- Demo.Services.X: contains the implementation for X.
- Demo.Services.XHost: contains the hosting of X via a Windows service.
- Demo.Console.XHost: contains the hosting of X via a console application.
- Demo.Web.X: a demo web consumer

For simple solutions with just a few services or for services that are not reusable a lot, we can combine Demo.Services.X and Demo.Services.XHost into one assembly.

Naming convetion in this class:
- OldStyled: refers to elements that use/demonstrate how to use WCF client the old way
- NewStyled: refers to elements that use/demonstrate how to use WCF client the new way
	