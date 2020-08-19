# Serverless example - Invoke Lambda via ElasticLoadBalancer

Creates application load balancer and targets a lambda function
using the 'alb' event in serverless.yml

Also creates in-line the load balancer and the target-group
registration of the lambda.

The example Handler.cs simply dumps the request passed in by the ALB to the
log and returns a "Hello 123" string.

This project was started using `sls create -t aws-csharp -p lambdaalb`. Added enough code to 
the handler to dump the request and return some data. The heavy lifting is in the serverless.
yaml which sets up the ALB and references the function produced in the 'functions:' section.

You can use the same serverless.yml in your own project using whatever runtime you want 
for the actual lambda - just create a project using the runtime you want and use this
serverless.yml as a starting point.

## Compile / Deploy

To compile and deploy:
* `./build.sh`
* `sls deploy -v`

To remove:
* `sls remove`
Note: make sure to cleanup if you are experimenting - the ALB/ELB charges hourly while on 
your account.

Note the `LoadBalancerDNSName: lambd-LoadB-xxxxx.elb.amazonaws.com` displayed as stack output.
(you can always find it on the stack outputs tab in the AWS console)

Your url will be `https://{LoadBalancerDNSName}/api`

Note: You will need the ARN of your certificate in AWS - must be in
same region as the ALB.

## Local testing
You can use the request.json as input to your lambda:
`sls invoke local -f webapi --path request.json`

## Miscellaneous

ALB setup for OIDC and TLS requires setting up an authorizer lambda and certificate
on the ALB - working example forthcoming.


-------------------------------------------------------------------
You may only use the Microsoft .NET Core Debugger (vsdbg) with
Visual Studio Code, Visual Studio or Visual Studio for Mac software
to help you develop and test your applications.
-------------------------------------------------------------------
Exception thrown: 'System.InvalidOperationException' in System.Private.CoreLib.dll
Exception thrown: 'System.InvalidOperationException' in System.Private.CoreLib.dll
IDX20803: Unable to obtain configuration from: 'System.String'.
