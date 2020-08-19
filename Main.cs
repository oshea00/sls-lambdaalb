using System;
using System.IO;
using System.Collections.Generic;

namespace Namespace {

public class Program {
    public static void Main(string[] args) {
        var request = File.Open("request.json",FileMode.Open);
        var api = new API();
        var response = api.FunctionHandler(request);
        Console.WriteLine(response.ToString());

        // var domain = Environment.GetEnvironmentVariable("AUTH_DOMAIN");
        // var audience = Environment.GetEnvironmentVariable("AUTH_AUDIENCE");
        // var permission = Environment.GetEnvironmentVariable("AUTH_PERMISSIONS");
        // var permissions = new List<string>(permission.Split(','));
        // var isAuthorized = JWTAuthorizer.Authorizer.IsAuthorized(authHeader, domain, audience, permissions);

        if (response.StatusCode == 200) 
        {
            Console.WriteLine(response.Body);
        } 
        else
        {
            Console.WriteLine(response.StatusDescription);
        }
    }
}


}