# Serverless example - Invoke Lambda via ElasticLoadBalancer

Creates application load balancer and targets a lambda function
using the 'alb' event in serverless.yml

Also creates in-line the load balancer and the target-group
registration of the lambda.

The example Handler.cs simply dumps the request passed in by the ALB to the
log and returns a "Hello 123" string.

This project was started using `sls create -t aws-csharp -p lambdaals`. Added enough code to 
the handler to dump the request and return some data. The heavy lifting is in the serverless.
yaml which sets up the ALB and references the function produced in the 'functions:' section.

You can use the same serverless.yml in your own project using whatever runtime you want 
for the actual lambda - just create a project using the runtime you want and use this
serverless.yml as a starting point.

## Compile / Deploy

To compile and deploy:
* `./build.sh`
* `sls deploy -v`

Note the `LoadBalancerDNSName: lambd-LoadB-xxxxx.elb.amazonaws.com` displayed as stack output.
(you can always find it on the stack outputs tab in the AWS console)

Your url will be `http://{LoadBalancerDNSName}/api`

## Local testing
You can use the request.json as input to your lambda:
`sls invoke local -f webapi --path request.json`

## Miscellaneous

ALB setup for OIDC and TLS requires setting up an authorizer lambda and certificate
on the ALB - working example forthcoming.


