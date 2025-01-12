# Specmatic Sample: .NET core BFF calling Domain API

* [Specmatic Website](https://specmatic.io)
* [Specmatic Documentation](https://specmatic.io/documentation.html)

This sample project demonstrates below aspects of Contract-Driven Development
* Contract testing a .NET core (C#) application by leveraging its OpenAPI spec to generate tests (#NOCODE)
* Service Virtualization (Stub / Mock) dependencies of this application (System Under Test), again using OpenAPI specifications of those dependency services (#NOCODE)

## Background

The Backend For Frontend (BFF) application here is the System Under Test (SUT). It depends on Domain API service which is being Stubbed out using Specmatic thereby effectively isolating the SUT so that we can now use Specmatic to run Contract Tests. Both Service Stubbing and Contract Testing is based on OpenAPI specifications here.

![CSharp Contract Driven Development](assets/specmatic-order-bff-architecture.gif)

## Tech Stack
1. .NET core (version 8)
2. Specmatic
3. Test Containers
4. Docker Desktop (on Local) and Test Container Cloud (in CI)
5. If you are on a Windows OS, please use PowerShell.

# Running Tests

This will start the specmatic stub server for domain api using the information in specmatic.yaml and run contract tests using Specmatic.

```shell
dotnet test
```

## Understand how to run the tests step by step
  - Start Docker Desktop
  - Navigate to Project (`cd specmatic-order-bff-csharp`)
  - Run the application `dotnet run`
  - Navigate to test Project (`cd specmatic-order-bff-csharp.test`)
  - Start the stub Server `docker run -v "$PWD/specmatic.yaml:/usr/src/app/specmatic.yaml" -v "$PWD/examples:/usr/src/app/examples" -p 9000:9000 znsio/specmatic stub --examples=examples`
  - Run the tests `docker run --network host -v "$PWD/specmatic.yaml:/usr/src/app/specmatic.yaml" -v "$PWD/build/reports/specmatic:/usr/src/app/build/reports/specmatic"  znsio/specmatic test --port=8080 --host=host.docker.internal`

# Break down each component to understand what is happening

## Start the dependent components

### Start domain api stub server

```shell
cd specmatic-order-bff-csharp.test
docker run -v "$PWD/specmatic.yaml:/usr/src/app/specmatic.yaml" -v "$PWD/examples:/usr/src/app/examples" -p 9000:9000 znsio/specmatic stub --examples=examples
```

## Start BFF Server
This will start the .NET core BFF server
```shell
cd specmatic-order-bff-csharp
dotnet run
```

## Test if everything is working

Note: For Windows OS, add `.exe` extension to curl command on PowerShell or use `cmd.exe` instead.

```shell
curl -H "pageSize: 10" "http://localhost:8080/findAvailableProducts?type=gadget"
```

You result should look like:
```json
[{"id":10,"name":"iPhone","type":"gadget","inventory":701}]
```
Note: You might not get 701 for inventory, it can return can random integer.

Also observe the logs in the Specmatic HTTP Stub Server.
