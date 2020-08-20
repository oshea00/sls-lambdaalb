# Serverless example - Invoke Lambda via ElasticLoadBalancer

Creates application load balancer and targets a lambda function
using the 'alb' event in serverless.yml

Also creates in-line the load balancer and the target-group
registration of the lambda.

The example Handler.cs simply dumps the request passed in by the ALB to the
log and returns a "Hello 123" string.

This project was started using `sls create -t aws-csharp -p lambdaalb`. 

The heavy lifting is here:
* serverless.yml which sets up the ALB and references the function produced in the 'functions:' section,
  along with the reference to your certificate ARN, which must exist in the same region
  as your ALB. Much of this in CF yaml in the Resources section.
* `Handler.cs` has to handle the JWT validation, see `Authorizer.cs`.
* Lastly you have to implement CORS yourself and provide OPTIONS request handling.

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

### CORS

The ELB doesn't support CORS - you need to implement it yourself. The Handler.cs does this.
Also, you have to pass the correct Access-Control-Allow-Origin header on your API response.
