using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using System.Diagnostics;
using System.Net;
using static System.Net.WebRequestMethods;

var clientIdSod = "2c93a3cb13e16645360190f0da7520a7";
var redirectUri = "http://127.0.0.1:7890/callback/";
//var genericListeningUri = "http://+:7999/callback/";

//Partially adapted from: https://github.com/SuperOffice/SuperOffice.DevNet.OpenIDConnectNativeApp/blob/master/Source/SuperOffice.DevNet.OpenIDConnectNativeApp/Program.cs

var browser = new SystemBrowser(redirectUri);

var options = new OidcClientOptions
{
    Authority = "https://sod.superoffice.com/login",
    ClientId = clientIdSod,
    RedirectUri = redirectUri,
    Scope = "openid profile api",
    FilterClaims = false,
    LoadProfile = false,
    Browser = browser // Your configured browser instance
};
//options.Policy.Discovery.RequireHttps = false;
options.Policy.Discovery.ValidateIssuerName = false;
options.Policy.RequireAccessTokenHash = false;

var oidcClient = new OidcClient(options);
var loginResult = await oidcClient.LoginAsync(new LoginRequest());

if (loginResult.IsError)
{
    Console.WriteLine($"Error: {loginResult.Error}");
    return;
}

else
{
    //DoStuff
    Console.WriteLine($"Result: {loginResult.AccessToken}");
}