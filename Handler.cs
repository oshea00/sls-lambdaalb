using System;
using Amazon.Lambda.Core;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using Amazon.Lambda.ApplicationLoadBalancerEvents;

[assembly:LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace Namespace
{
    public class API
    {
      public ApplicationLoadBalancerResponse FunctionHandler(Stream input)
      {
        ApplicationLoadBalancerResponse response = null;
        var request = new StreamReader(input).ReadToEnd();
        var dict = JsonSerializer.Deserialize<Dictionary<string,object>>(request);
        var requestHeaders = JsonSerializer.Deserialize<Dictionary<string,string>>(dict["headers"].ToString());
        var authorization = "";

        System.Console.WriteLine(request);

        if (dict["httpMethod"].ToString()=="OPTIONS") {
          Dictionary<string, string> corsHeaders = new Dictionary<string, string>();
          corsHeaders.Add("Content-Type", "application/json");
          corsHeaders.Add("Access-Control-Allow-Origin", "*");
          corsHeaders.Add("Access-Control-Allow-Headers", "*");
          corsHeaders.Add("Access-Control-Allow-Methods", "*");
          var corsResponse = new ApplicationLoadBalancerResponse() {
            IsBase64Encoded = false,
            StatusCode = 200,
            StatusDescription = "200 OK",
            Headers = corsHeaders,
          };
          return corsResponse;
        }

        if (requestHeaders.ContainsKey("authorization"))
          authorization = requestHeaders["authorization"];

        // Get these from the Lambda ENV
        var domain = Environment.GetEnvironmentVariable("AUTH_DOMAIN");
        var audience = Environment.GetEnvironmentVariable("AUTH_AUDIENCE");
        var permission = Environment.GetEnvironmentVariable("AUTH_PERMISSIONS");
        var permissions = new List<string>(permission.Split(','));

        // See if authorized
        if (JWTAuthorizer.Authorizer.IsAuthorized(authorization, domain, audience, permissions)) 
        {
          var data = new Response { Message = "Hello, 123!"};

          Dictionary<string, string> headers = new Dictionary<string, string>();
          headers.Add("Content-Type", "application/json");
          headers.Add("Access-Control-Allow-Origin", "*");

          response = new ApplicationLoadBalancerResponse() {
            IsBase64Encoded = false,
            StatusCode = 200,
            StatusDescription = "200 OK",
            Headers = headers,
            Body = JsonSerializer.Serialize<Response>(data)
          };

        } 
        else 
        {
          response =  new ApplicationLoadBalancerResponse() {
            IsBase64Encoded = false,
            StatusCode = 403,
            StatusDescription = "403 Unauthorized",
            };
        }

        return response;
      }
    }

    class Response {
      public string Message { get; set; }
    }
}