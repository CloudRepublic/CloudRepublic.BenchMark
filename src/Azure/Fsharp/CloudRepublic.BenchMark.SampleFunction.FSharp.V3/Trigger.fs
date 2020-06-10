namespace CloudRepublic.BenchMark.SampleFunction.FSharp.V3

module Trigger =

    open System
    open Microsoft.Extensions.Primitives
    open Microsoft.AspNetCore.Mvc
    open Microsoft.Azure.WebJobs
    open Microsoft.AspNetCore.Http
    open Microsoft.Azure.WebJobs.Extensions.Http
    open Microsoft.Extensions.Logging

    [<FunctionName("Trigger")>]
    let Run ([<HttpTrigger(AuthorizationLevel.Function, "GET", Route = null)>] req:HttpRequest) (log:ILogger) =
        "C# HTTP trigger function processed a request." |> log.LogInformation

        let name = req.Query.["name"]

        if StringValues.IsNullOrEmpty(name) then BadRequestObjectResult("Please pass a name on the query string") :> ActionResult
            else OkObjectResult(String.Format("Hello, {0}", name.ToString())) :> ActionResult