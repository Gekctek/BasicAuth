# BasicAuth
Basic Auth Middleware for Dnx (frameworks: dnx451, dnxcore50)

## Installation
##### NuGet: [BasicAuth](https://www.nuget.org/packages/BasicAuth)

using nuget command line:
```cs
Install-Package BasicAuth
```
or for pre-release versions:
```cs
Install-Package BasicAuth -Pre
```

## Usage
Create a AspNet 5/Dnx Web Application, reference this library and in the `Startup` class configure the following:

Add the JsonRpc router the pipeline in the `Configure` method:
```cs
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
    string realm = "YourRealm"; //Replace with your Basic Auth realm
    bool automaticAuthentication = true; //Defaults to false. True means its will auth all requests, False means it will only auth basic auth requests
	app.UseBasicAuth(realm, this.AuthenticateCredential, this.OnException, automaticAuthentication);
}

//Custom (and required) method that you will use to check the basic auth credential
//It needs to return an 'AuthenticationTicket' that holds the user principal IF the user is authenticated
//(Here I just check for Test:Password credential, dont do that)
private Task<AuthenticationTicket> AuthenticateCredential(BasicAuthInfo authInfo)
{
	AuthenticationTicket ticket = null;
	if (authInfo.Credential.Username == "Test" && authInfo.Credential.Password == "Password")
	{
		ClaimsIdentity identity = new ClaimsIdentity(authInfo.Options.AuthenticationScheme);
		identity.AddClaim(new Claim(ClaimTypes.Name, "Test"));
		identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "TestId"));
		ClaimsPrincipal principal = new ClaimsPrincipal(identity);
		ticket = new AuthenticationTicket(principal, authInfo.Properties, authInfo.Options.AuthenticationScheme);
	}
	return Task.FromResult(ticket);
}

//Optional method to handle the exception thrown by the basic auth process
//The exception is in the failedNotification object and any Exception that this library throws is a
//child class of the BasicAuthException class
private Task OnException(AuthenticationFailedNotification<string, BasicAuthOptions> failedNotification)
{
    //if...(something that can be handled)...failedNotification.HandleResponse();
    //if...(should skip to next middleware)...failedNotifcation.SkipToNextMiddleware();
    //if...(want to alter response)...failedNotification.HttpContext.Response...
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
 - Keep up to date with latest aspnet beta

License
----
[MIT](https://raw.githubusercontent.com/Gekctek/BasicAuth/master/LICENSE)
