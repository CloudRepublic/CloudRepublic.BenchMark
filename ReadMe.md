# Serverless Benchmark

We've created a continuous benchmark tool for Azure Functions to measure the performance differences between Linux and Windows hosted Azure Functions. Secondly it measures the differences between functions written in C# .NET , NodeJS and Python. The performance benchmarking is only focussed on Http Triggered functions.

## builds
[![Build Status](https://dev.azure.com/rgertsen/BenchMark/_apis/build/status/BenchMark?branchName=master)](https://dev.azure.com/rgertsen/BenchMark/_build/latest?definitionId=1&branchName=master)

## How the benchmark is executed

There's an orchestrator function that executes HTTP GET requests to every function app instance available. The first 5 calls are classified as coldstart, we then wait for 30 seconds to execute 10 requests per function instance to measure the warmed up HTTP requests.


## Adding additional benchmark Language
1- Expand the enum of: cloudprovider, hostenvironment, runtime or an create additional setting (like V3) (located in the Orchestrator)
2- Enter a new entry into the BenchMarkTypeGenerator and assign a name (the name is currently the name of the settings combined) (Orchestrator)
3- Make sure the new entry has the following environment variables (Environment.GetEnvironmentVariable() are set (dots are the assigned name from step 2) : .....CLIENT, ... URL (and ....KEY if needed)
4- create the matching function/trigger (located in the 'Azure' folder, not all are loaded in solution).
5- add the function to the deployment files (Deployment folder-> Deploy.azcli, Deploy.ps1, not loaded in solution)
6- Load the Frontend project -> BenchMark.Vue and expand the Enums and benchmarkOptions with the newly added entry (Located in Web -> Views -> benchmark.vue)
7- Verify all test are still succesfull and Pull request

## Global Architecture

Core to the architecture is the app configuration service.
It is used to register the test endpoints and details like the names, os, skus, etc.
This allows for an easy extension of the run test set by just adding a bicep deployment with the correct details.


<img src='docs/global architecture.png' />

## Author

cloudrepublic.nl

## License

MIT License

Copyright (c) 2019 Cloud Republic

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
