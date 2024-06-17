using IdentityModel.OidcClient;
using System.Diagnostics;
using System.Net;

var clientIdSod = "2c93a3cb13e16645360190f0da7520a7";
var redirectUri = "http://127.0.0.1:7999/callback/";
//var genericListeningUri = "http://+:7999/callback/";

//Partially adapted from: https://github.com/SuperOffice/SuperOffice.DevNet.OpenIDConnectNativeApp/blob/master/Source/SuperOffice.DevNet.OpenIDConnectNativeApp/Program.cs

var options = new OidcClientOptions
{
    Authority = "https://sod.superoffice.com/login",
    LoadProfile = false,
    ClientId = clientIdSod,
    // ClientSecret should not be needed with Authorization Code Flow with PKCE
    Scope = "openid profile api", // "openid",
    RedirectUri = redirectUri,

    // N.B. Hybrid flow not supported anymore by OidcClient. ResponseMode & Flow deprecated.
};
//options.Policy.Discovery.RequireHttps = false;
options.Policy.Discovery.ValidateIssuerName = false;
options.Policy.RequireAccessTokenHash = false;

var client = new OidcClient(options);
var state = await client.PrepareLoginAsync();

using var listener = new HttpListener();
listener.Prefixes.Add(redirectUri);
listener.Start();

Console.WriteLine("Starting login uri: " + state.StartUrl);
Process.Start(new ProcessStartInfo("cmd", $"/c start {state.StartUrl}") { CreateNoWindow = true });

while (true)
{
    var ctx = await listener.GetContextAsync();
    Console.WriteLine("Callback received: " + ctx.Request.Url);

    // TODO: read request & send response
}
