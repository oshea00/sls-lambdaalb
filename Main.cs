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