# Specmatic Sample: .NET core BFF calling Domain API

* [Specmatic Website](https://specmatic.io)
* [Specmatic Documentation](https://docs.specmatic.io)

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
4. Docker Desktop

# Running Tests

Please make sure Docker Desktop is running on you local machine.

```shell
dotnet test
```

This will start the [ContractTest](specmatic-order-bff-csharp.test/contract/ContractTests.cs) which reads through [specmatic.yaml](specmatic-order-bff-csharp.test/specmatic.yaml) and does the following.
* Starts a Stub Server which represents the Domain API based on OpenAPI spec listed under consumes section of the config
* Starts the application programmatically
* Runs Contract Test on the application based on the OpenAPI spec listed under the provides section of the config
* You will now see a **Rich HTML API Coverage report** appear in [specmatic-order-bff-csharp.test/build/reports/specmatic/html/index.html](specmatic-order-bff-csharp.test/build/reports/specmatic/html/index.html)

## Understand how to run the tests step by step manually if required

This section will walk you through each of the steps that were programmatically invoked in the above setup. Please make sure Docker Desktop is running on you local machine.

### Start Domain API Stub Server (To emulate service dependencies)
* Open a terminal and navigate to the test project:<br/><br/>
  ```shell
  cd specmatic-order-bff-csharp.test
  ```
* Start the stub Server
* For Unix and PowerShell:<br/><br/>
  ```shell
  docker run --rm -v "$(pwd)/specmatic.yaml:/usr/src/app/specmatic.yaml" -v "$(pwd)/examples:/usr/src/app/examples/domain_service" -p 9000:9000 specmatic/specmatic stub --examples=examples
  ```
* For Windows CMD Prompt:<br/><br/>
  ```shell
  docker run --rm -v "%cd%/specmatic.yaml:/usr/src/app/specmatic.yaml" -v "%cd%/examples:/usr/src/app/examples/domain_service" -p 9000:9000 specmatic/specmatic stub --examples=examples
  ```


### Start the BFF Application (The System Under Test)
* Open another terminal and navigate to application project:<br/><br/>
  ```shell
  cd specmatic-order-bff-csharp
  ```
* Run the application:<br/><br/>
  ```shell
  dotnet run
  ```
* Curl the `/findAvailableProducts` endpoint:<br/><br/>
  ```shell
  curl -H "pageSize: 10" "http://localhost:8080/findAvailableProducts?type=gadget"
  ```
* The result should look something like this `[{"id":10,"name":"iPhone","type":"gadget","inventory":798}]` (Note: You might not see 798 for inventory, it can vary as it is a generated value as per current stub setup)
* You can also do the above using [Swagger UI]("http://localhost:8080/swagger")


### Running Contract Tests
* In a fresh terminal navigate to the test project:<br/><br/>
  ```shell
  cd specmatic-order-bff-csharp.test
  ```
* Run the tests
- For Unix and PowerShell:<br/><br/>
  ```shell
  docker run --rm --network host -v "$(pwd)/specmatic.yaml:/usr/src/app/specmatic.yaml" -v "$(pwd)/examples:/usr/src/app/examples/bff" -v "$(pwd)/build/reports/specmatic:/usr/src/app/build/reports/specmatic" specmatic/specmatic test --port=8080 --host=host.docker.internal --examples=examples
  ```
- For Windows CMD Prompt:<br/><br/>
  ```shell
  docker run --rm --network host -v "%cd%/specmatic.yaml:/usr/src/app/specmatic.yaml" -v "%cd%/examples:/usr/src/app/examples/bff" -v "%cd%/build/reports/specmatic:/usr/src/app/build/reports/specmatic" specmatic/specmatic test --port=8080 --host=host.docker.internal --examples=examples
  ```
* The **Rich HTML API Coverage report** will be available in [specmatic-order-bff-csharp.test/build/reports/specmatic/html/index.html](specmatic-order-bff-csharp.test/build/reports/specmatic/html/index.html)

Please observe the logs in the Specmatic HTTP Stub Server to get an understanding of how a request made by our contract test to the application results in application in turn calling the Specmatic HTTP Stub Server which returns response as per the expectations / examples that have been provided.
