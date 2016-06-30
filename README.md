# BasicAuth
Basic Auth Middleware for AspNet Core (frameworks: net451, netstandard1.1)

## Installation
##### NuGet: [BasicAuth](https://www.nuget.org/packages/ EdjCase.BasicAuth)

using nuget command line:
```cs
Install-Package EdjCase.BasicAuth
```
or for pre-release versions:
```cs
Install-Package EdjCase.BasicAuth -Pre
```

## Usage
Create a AspNet 5/Dnx Web Application, reference this library and in the `Startup` class configure the following:


Add the dependency injected services in the `ConfigureServices` method:
```cs
public void ConfigureServices(IServiceCollection services)
{
	services.AddBasicAuth();
	//Adds default IBasicAuthParser implementation to the services collection.
	//(Can be overridden by custom implementations if desired)
}
```

Add the JsonRpc router the pipeline in the `Configure` method:
```cs
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
	app.UseBasicAuth(options => 
	{
	    options.Realm = "YourRealm"; //Replace with your Basic Auth realm
        options.AuthenticateCredential = this.AuthenticateCredential;
        options.AutomaticAuthenticate = true; //Defaults to false. True means its will auth all requests, False means it will only auth basic auth requests
        options.AutomaticChallenge = true; 
		options.Events = new BasicAuthEvents
		{
			OnAuthenticationFailed = this.OnAuthenticationFailed
		};
	});
}

//Custom (and required) method that you will use to check the basic auth credential
//It needs to return an 'AuthenticationTicket' that holds the user principal IF the user is authenticated
//(Here I just check for Test:Password credential, dont do that)
private Task<AuthenticationTicket> AuthenticateCredential(BasicAuthInfo authInfo)
{
	AuthenticationTicket ticket = null;
	if (authInfo.Credential.Username == "Test" && authInfo.Credential.Password == "Password")
	{
		ClaimsIdentity identity = new ClaimsIdentity(authInfo.AuthenticationScheme);
		identity.AddClaim(new Claim(ClaimTypes.Name, "Test"));
		identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "TestId"));
		ClaimsPrincipal principal = new ClaimsPrincipal(identity);
		ticket = new AuthenticationTicket(principal, authInfo.Properties, authInfo.AuthenticationScheme);
	}
	return Task.FromResult(ticket);
}

//Optional method to handle the failure in the basic auth process
//The failure details are in the context object
private Task OnAuthenticationFailed(BasicAuthFailedContext context)
{
	//if...(something that can be handled)...context.HandleResponse();
	//if...(should skip to next middleware)...context.SkipToNextMiddleware();
	return Task.FromResult(0);
}
```

## Contributions

Contributions welcome. Fork as much as you want. All pull requests will be considered.

Best way to develop is to use Visual Studio 2015+ or Visual Studio Code on other platforms besides windows.

Also the correct dnx runtime has to be installed if visual studio does not automatically do that for you. 
Information on that can be found at the [Asp.Net Repo](https://github.com/aspnet/Home).

Note: I am picky about styling/readability of the code. Try to keep it similar to the current format. 

## Feedback
If you do not want to contribute directly, feel free to do bug/feature requests through github or send me and email [Gekctek@Gmail.com](mailto:Gekctek@Gmail.com)

### Todos

 - Better sample app
 - Performance testing

License
----
[MIT](https://raw.githubusercontent.com/Gekctek/BasicAuth/master/LICENSE)
