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

To test your own OIDC and JWT token:
* Update the environment in `.vscode/launch.json` to use your domain and audience from your IdP's application settings.
* Insert your JWT in the authorization header in request.json.
* You can debug the handler by placing breakpoints - then running the project (F5). The `Main.cs` will bootstrap up the handler.
* You can run locally and use the request.json as input to your lambda to test invocations:
  * `sls invoke local -f webapi --path request.json`


### OIDC Testing

To test your own OIDC and JWT token:
* Update the environment in `.vscode/launch.json` to use your domain and audience from your IdP's application settings.
* You can run the project (F5 )- insert your JWT in the authorization header in request.json.

